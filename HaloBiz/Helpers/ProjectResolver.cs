using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using HaloBiz.MyServices.Impl;
using Task = HalobizMigrations.Models.ProjectManagement.Task;


namespace HaloBiz.Helpers
{

    public class ProjectResolver:IProjectResolver

    {
    private readonly HalobizContext _context;
    private readonly IProjectAllocationRepositoryImpl _projectAllocationRepository;
    private readonly IMapper _mapper;

    public ProjectResolver(HalobizContext context, IProjectAllocationRepositoryImpl projectAllocationRepository,
        ILogger<ProjectAllocationServiceImpl> logger, IMapper mapper)
    {
        this._mapper = mapper;
        this._projectAllocationRepository = projectAllocationRepository;
        this._context = context;
    }

    public async Task<ApiCommonResponse> CreateEndorseMentProject(HttpContext httpContext,ContractServiceForEndorsement endorsement)
    {
        var getWatchers = await _context.Divisions.AsNoTracking()
            .Where(x => x.Name == endorsement.Service.Division.Name)
            .Include(x => x.Head)
            .ToListAsync();
        var distinctWatchers = getWatchers.GroupBy(x => x.Id).Select(x => x.First())
            .ToArray();
        var getDefaultWorkspace = await _context.Workspaces.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsActive == true && x.IsDefault == true);
        if (endorsement != null)
        {
            var projectInstance = new Project()
            {
                Caption =
                    $"{endorsement.EndorsementType.Caption} for {endorsement.CustomerDivision.DivisionName} Fulfillment",
                Alias =
                    $"{endorsement.EndorsementType.Caption} for {endorsement.CustomerDivision.DivisionName} Fulfillment",
                Description =
                    $"{endorsement.EndorsementType.Caption} for {endorsement.CustomerDivision.DivisionName} Fulfillment",
                IsActive = true,
                CreatedById = 46, //httpContext.GetLoggedInUserId(),
                CreatedAt = DateTime.Now,
                WorkspaceId = getDefaultWorkspace.Id,
                ProjectImage = endorsement.CustomerDivision.LogoUrl
            };
            await _context.Projects.AddAsync(projectInstance);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                var watcherArray = new List<Watcher>();
                foreach (var watcher in distinctWatchers)
                {
                    var watcherInstance = new Watcher()
                    {
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        ProjectWatcherId = watcher.Head.Id,
                        ProjectId = projectInstance.Id
                    };
                    watcherArray.Add(watcherInstance);
                }

                await _context.Watchers.AddRangeAsync(watcherArray);
                await _context.SaveChangesAsync();
            }

            await createEndorsementTask(httpContext,projectInstance.Id, endorsement);
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "successfully created task");
        }

        return CommonResponse.Send(ResponseCodes.FAILURE, null, "An error occurred before a project could be created");
    }

    public async Task<ApiCommonResponse> CreateServiceProject(HttpContext httpContext, Service service)
    {
        var getWatchers = await _context.Divisions.AsNoTracking()
            .Where(x => x.Name == service.Division.Name)
            .Include(x => x.Head)
            .ToListAsync();
        var distinctWatchers = getWatchers.GroupBy(x => x.Id).Select(x => x.First())
            .ToArray();
        var getDefaultWorkspace = await _context.Workspaces.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsActive == true && x.IsDefault == true);
        if (service != null)
        {
            var projectInstance = new Project()
            {
                Caption =
                    $"Service creation {service.Name}-{service.Division.Name}-Fulfillment",
                Alias =
                    $"Service creation {service.Name}-{service.Division.Name}-Fulfillment",
                Description =
                    $"Service creation for service type {service.ServiceType.Caption} with a service code of {service.ServiceCode}",
                IsActive = true,
                CreatedById = httpContext.GetLoggedInUserId(),
                CreatedAt = DateTime.Now,
                WorkspaceId = getDefaultWorkspace.Id,
                ProjectImage = service.ImageUrl
            };
            await _context.Projects.AddAsync(projectInstance);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                var watcherArray = new List<Watcher>();
                foreach (var watcher in distinctWatchers)
                {
                    var watcherInstance = new Watcher()
                    {
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        ProjectWatcherId = watcher.Head.Id,
                        ProjectId = projectInstance.Id
                    };
                    watcherArray.Add(watcherInstance);
                }

                await _context.Watchers.AddRangeAsync(watcherArray);
                await _context.SaveChangesAsync();
            }

            await createServiceTask(httpContext,projectInstance.Id, service);
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "successfully created task");
        }
        return CommonResponse.Send(ResponseCodes.FAILURE, null, "An application error occurred at creating service project");
    }

    public async Task<ApiCommonResponse> createServiceTask(HttpContext httpContext,long Id, Service service)
    {
        if (service != null)
        {


            var taskInstance = new Task()
            {
                Alias =
                    $"Task  {service.Name}, fetched based on service-group {service.ServiceGroup.Name}",
                Caption =
                    $"Task  {service.Name}, fetched based on service-group {service.ServiceGroup.Name}",
                IsActive = true,
                IsAssigned = false,
                CreatedAt = DateTime.Now,
                IsReassigned = false,
                WorkingManHours = 0,
                Description =
                    service.Description,
                TaskStartDate =
                     DateTime.Now,
                TaskEndDate =
                     DateTime.Now,
                CreatedById = httpContext.GetLoggedInUserId(),
                ProjectId = Id
            };
            await _context.Tasks.AddAsync(taskInstance);
            await _context.SaveChangesAsync();
            var getUsers = await _context.ProjectAllocations
                .Where(x => x.ServiceCategoryId == service.ServiceCategoryId && x.IsDeleted == false)
                .ToListAsync();
            if (getUsers.Any())
            {
                var taskAssigneeArray = new List<TaskAssignee>();
                foreach (var user in getUsers)
                {
                    var taskAssignee = new TaskAssignee()
                    {
                        IsActive = true,
                        Name = user.ManagerName,
                        CreatedAt = DateTime.Now,
                        TaskId = taskInstance.Id,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UserImage = user.ManagerImageUrl,
                        TaskAssigneeId = user.ManagerId
                    };
                    taskAssigneeArray.Add(taskAssignee);
                }

                await _context.AddRangeAsync(taskAssigneeArray);
                await _context.SaveChangesAsync();
            }
        }

        return CommonResponse.Send(ResponseCodes.SUCCESS, null, "successfully created task");
    }
    public async Task<ApiCommonResponse> createEndorsementTask(HttpContext httpContext,long Id, ContractServiceForEndorsement endorsement)
    {
        if (endorsement != null)
        {


            var taskInstance = new Task()
            {
                Alias =
                    $"Task with unique-tag {endorsement.UniqueTag}, fetched based on {endorsement.Service.Name}  for {endorsement.EndorsementType.Caption}.",
                Caption =
                    $"Task with unique-tag {endorsement.UniqueTag}, fetched based on {endorsement.Service.Name}  for {endorsement.EndorsementType.Caption}.",
                IsActive = true,
                IsAssigned = false,
                CreatedAt = DateTime.Now,
                IsReassigned = false,
                WorkingManHours = 0,
                Description =
                    endorsement.EndorsementDescription,
                TaskStartDate =
                    endorsement.ContractStartDate ?? DateTime.Now,
                TaskEndDate =
                    endorsement.ContractEndDate ?? DateTime.Now,
                CreatedById = httpContext.GetLoggedInUserId(),
                ProjectId = Id
            };
            await _context.Tasks.AddAsync(taskInstance);
            await _context.SaveChangesAsync();
            var getUsers = await _context.ProjectAllocations
                .Where(x => x.ServiceCategoryId == endorsement.Service.ServiceCategoryId && x.IsDeleted == false)
                .ToListAsync();
            if (getUsers.Any())
            {
                var taskAssigneeArray = new List<TaskAssignee>();
                foreach (var user in getUsers)
                {
                    var taskAssignee = new TaskAssignee()
                    {
                        IsActive = true,
                        Name = user.ManagerName,
                        CreatedAt = DateTime.Now,
                        TaskId = taskInstance.Id,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UserImage = user.ManagerImageUrl,
                        TaskAssigneeId = user.ManagerId
                    };
                    taskAssigneeArray.Add(taskAssignee);
                }

                await _context.AddRangeAsync(taskAssigneeArray);
                await _context.SaveChangesAsync();
            }
        }

        return CommonResponse.Send(ResponseCodes.SUCCESS, null, "successfully created task");
    }

    public async Task<List<ContractFulfillMentStructure>> StructureServices(List<QuoteService> quoteServices)
    {
        var contractServiceFufilmentArray = new List<ContractFulfillMentStructure>();
        if (quoteServices.Any())
        {
            var divisionArray = new List<Division>();
            foreach (var division in quoteServices)
            {
                divisionArray.Add(division.Service.Division);
            }

            var distinctDivision = divisionArray.GroupBy(x => x.Id).Select(x => x.First()).ToArray();

            foreach (var division in distinctDivision)
            {
                var newQuoteService = quoteServices.Where(x =>
                    x.Service.Division.Name.Trim() == division.Name.Trim() &&
                    x.Service.Name != "GUARD ADMIN SERVICE").ToList();
                var contractServiceFufilmentInstance = new ContractFulfillMentStructure()
                {
                    FulfillmentClass = division.Name,
                    QuoteService = newQuoteService.GroupBy(x=>x.Id).Select(x=>x.First()).ToArray()
                };
                contractServiceFufilmentArray.Add(contractServiceFufilmentInstance);
            }
        }

        return contractServiceFufilmentArray;
    }





    public async Task<ApiCommonResponse> CreateProjectForFulfilmentProject(HttpContext httpContext,
        LeadDivision leadDivision, List<ContractFulfillMentStructure> contractFulfillMentStructures)
    {

        var getDefaultWorkspace = await _context.Workspaces.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsActive == true && x.IsDefault == true);

        if (getDefaultWorkspace != null)
        {
            var projectArray = new List<Project>();
            foreach (var division in contractFulfillMentStructures)
            {
                var getWatchers = await _context.Divisions.AsNoTracking()
                    .Where(x => x.Name == division.FulfillmentClass)
                    .Include(x => x.Head)
                    .ToListAsync();
                var distinctWatchers = getWatchers.GroupBy(x => x.Id).Select(x => x.First())
                    .ToArray();
                var projectInstance = new Project()
                {
                    Caption = $"{leadDivision.DivisionName}-{division.FulfillmentClass} Contract Fulfillment",
                    Alias = $"{leadDivision.DivisionName}-{division.FulfillmentClass} Fulfillment",
                    Description =
                        $"{leadDivision.DivisionName}-{division.FulfillmentClass} Fulfillment project",
                    IsActive = true,
                    CreatedById = httpContext.GetLoggedInUserId(),
                    CreatedAt = DateTime.Now,
                    WorkspaceId = getDefaultWorkspace.Id,
                    ProjectImage = leadDivision.Lead.LogoUrl
                };
                await _context.Projects.AddAsync(projectInstance);
                var status = await _context.SaveChangesAsync();
                await CreateTaskFromProject(httpContext,projectInstance.Id, division.QuoteService);

                if (status > 0)
                {
                    var watcherArray = new List<Watcher>();
                    foreach (var watcher in distinctWatchers)
                    {
                        var watcherInstance = new Watcher()
                        {
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            CreatedById = httpContext.GetLoggedInUserId(),
                            ProjectWatcherId = watcher.Head.Id,
                            ProjectId = projectInstance.Id
                        };
                        watcherArray.Add(watcherInstance);
                    }

                    await _context.Watchers.AddRangeAsync(watcherArray);
                    await _context.SaveChangesAsync();
                }
            }
        }

        return CommonResponse.Send(ResponseCodes.SUCCESS, null,
            "Done creating project");
    }


    public async Task<ApiCommonResponse> CreateTaskFromProject(HttpContext httpContext,long Id, QuoteService[] quoteServices)
    {
        if (quoteServices.Any())
        {
            int index = 0;
            foreach (var quoteService in quoteServices)
            {
                index++;
                var taskInstance = new Task()
                {
                    Alias =
                        $"T{index} with unique-tag {quoteService.UniqueTag}, fetched based on {quoteService.Service.Name ?? " Name not available"}.",
                    Caption =
                        $"T{index} with unique-tag {quoteService.UniqueTag}, fetched based on {quoteService.Service.Name ?? " Name not available"}.",
                    IsActive = true,
                    IsAssigned = false,
                    CreatedAt = DateTime.Now,
                    IsReassigned = false,
                    WorkingManHours = 0,
                    Description =
                        $"Task with unique-tag {quoteService.UniqueTag},fetched based on {quoteService.Service.Name} with a budget of " +
                        $"{quoteService.Budget} at a quantity of {quoteService.Quantity}.",
                    TaskStartDate =
                        quoteService.ContractStartDate ?? DateTime.Now,
                    TaskEndDate =
                        quoteService.ContractEndDate ?? DateTime.Now,
                    CreatedById = httpContext.GetLoggedInUserId(),
                    ProjectId = Id,
                    
                };
                await _context.Tasks.AddAsync(taskInstance);
                await _context.SaveChangesAsync();

                var serviceCategoryId = quoteService.Service.ServiceCategoryId;
                var getUsers = await _context.ProjectAllocations.Where(x =>
                    x.ServiceCategoryId == serviceCategoryId && x.IsDeleted == false).ToListAsync();
                if (getUsers.Any())
                {
                    var taskAssigneeArray = new List<TaskAssignee>();
                    foreach (var user in getUsers)
                    {
                        var taskAssignee = new TaskAssignee()
                        {
                            IsActive = true,
                            Name = user.ManagerName,
                            CreatedAt = DateTime.Now,
                            TaskId = taskInstance.Id,
                            CreatedById = 46, //httpContext.GetLoggedInUserId(),
                            UserImage = user.ManagerImageUrl,
                            TaskAssigneeId = user.ManagerId
                        };
                        taskAssigneeArray.Add(taskAssignee);
                    }

                    await _context.AddRangeAsync(taskAssigneeArray);
                    await _context.SaveChangesAsync();
                }
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null,
                "Succesfully created task");
        }

        return CommonResponse.Send(ResponseCodes.FAILURE, null,
            "Could not successfully create task");
    }
    }
}