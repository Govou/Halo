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
    

       public async Task<ApiCommonResponse> SaveTask(Service service,long projectId,HttpContext httpContext)
       {
           
           foreach (var task in service.ServiceCategory.ServiceCategoryTasks)
           {
               //if(service.)
               var currentTask = new Task()
               {
                   IsActive = true,
                   ProjectId = projectId,
                   Caption = task.Caption + " " + service.Name + " " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                   Alias = task.Caption + " " + service.Name,
                   IsAssigned = false,
                   CreatedAt = DateTime.Now,
                   CreatedById = httpContext.GetLoggedInUserId(),
                   IsReassigned = false,
                   Description = task.Caption + " for service " + service.Name + $" {(service.ServiceCode)}.",
                   TaskStartDate = DateTime.Now,
                   TaskEndDate = DateTime.Now.AddDays(1),
                   WorkingManHours = (DateTime.Now.AddDays(1).Date - DateTime.Now.Date).Days,
               };
               var taskEntity = await _context.Tasks.AddAsync(currentTask);
               await _context.SaveChangesAsync();
               await SaveTaskAssignees(service.ServiceCategoryId, taskEntity.Entity.Id, httpContext);
           }
           return CommonResponse.Send(ResponseCodes.SUCCESS, null, "SuccessFully saved Task");
       }
       public async Task<ApiCommonResponse> SaveTaskAssignees(long serviceCategoryId,long taskId,HttpContext httpContext)
       {
           var getTaskAssignees = await _context.ProjectAllocations.AsNoTracking()
               .Where(x => x.ServiceCategoryId == serviceCategoryId).ToListAsync();

           var assigneeArray = new List<TaskAssignee>();
           foreach (var assignee in getTaskAssignees)
           {
               var currentAssignee = new TaskAssignee()
               {
                   IsActive = true,
                   TaskId = taskId,
                   Name = assignee.ManagerName,
                   CreatedAt = DateTime.Now,
                   UserImage = assignee.ManagerImageUrl,
                   CreatedById = httpContext.GetLoggedInUserId(),
                   TaskAssigneeId = assignee.ManagerId,
               };
               assigneeArray.Add(currentAssignee);
           }

           await _context.TaskAssignees.AddRangeAsync(assigneeArray);
           await _context.SaveChangesAsync();
           return CommonResponse.Send(ResponseCodes.SUCCESS, null, "SuccessFully saved Assignee(s)");
       }

       
       public async Task<ApiCommonResponse> ResolveEndorsement(long requestId, HttpContext httpContext)
       {

           var getEndorsement = await _context.ContractServiceForEndorsements.AsNoTracking()
               .Where(x => x.Id == requestId)
               .Include(x=>x.EndorsementType)
               .Include(x=>x.CustomerDivision)
               .FirstOrDefaultAsync();

           var getService = await _context.Services.AsNoTracking()
               .Where(x => x.Id == getEndorsement.ServiceId && x.Name != "GUARD ADMIN SERVICE")
               .Include(x => x.ServiceCategory)
               .ThenInclude(x => x.ServiceCategoryTasks)
               .FirstOrDefaultAsync();
           
           if (getService == null)
           {
               return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not create project fulfilment");
           }
           else
           {
               if (!getService.ServiceCategory.ServiceCategoryTasks.Any())
               {
                   return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not create project fulfilment");
               }
               else
               {
                   var projectToCreate = new Project()
                   {
                       Caption = getEndorsement.CustomerDivision.DivisionName + $" {getEndorsement.EndorsementType.Caption}",
                       Alias = getEndorsement.BeneficiaryName + $" {getEndorsement.EndorsementType.Caption}",
                       Description = getEndorsement.EndorsementDescription +" with unique tag " + getEndorsement.UniqueTag ,
                       IsActive = true,
                       ProjectImage = getEndorsement.DocumentUrl,
                       WorkspaceId = 148,
                       CreatedAt = DateTime.Now,
                       CreatedById = httpContext.GetLoggedInUserId(),
                   };
                   var projectEntity = await _context.Projects.AddAsync(projectToCreate);
                   await _context.SaveChangesAsync();
                   await CreateEndorsementProjectWatcher(projectEntity.Entity.Id, getService.Id, httpContext);
                   await SaveTask(getService, projectEntity.Entity.Id, httpContext);
                   
               }
             
           }
            
           return CommonResponse.Send(ResponseCodes.FAILURE, getService, "Could not create Project");

       }

       public async Task<ApiCommonResponse> ResolveLead(long requestId, HttpContext httpContext)
       {
           var serviceCategories = new List<ServiceCategory>();
           var serviceCategoryTasks = new List<ServiceCategoryTask>();
           var getQuote = await _context.LeadDivisions.AsNoTracking()
               .Include(x=>x.Lead)
               .Include(x => x.Quote)
               .ThenInclude(x => x.QuoteServices.Where(x => x.IsDeleted == false))
               .ThenInclude(x=>x.Service)
               .ThenInclude(x=>x.ServiceCategory)
               .ThenInclude(x=>x.ServiceCategoryTasks)
               .Where(x=>x.IsDeleted == false && x.LeadId == requestId)
               .OrderByDescending(x=>x.CreatedAt)
               .FirstOrDefaultAsync();

           if (getQuote != null)
           {
               foreach (var service in getQuote.Quote.QuoteServices)
               {
                   if (service.Service.Name != "GUARD ADMIN SERVICE")
                   {
                       serviceCategories.Add(service.Service.ServiceCategory);
                   }
                   
               }
           }

           if (!serviceCategories.Any())
           {
               return CommonResponse.Send(ResponseCodes.FAILURE, null, "Service Category is empty");
           }

           foreach (var serviceCategory in serviceCategories)
           {
                serviceCategoryTasks.AddRange(serviceCategory.ServiceCategoryTasks); 
           }
           
           var projectToCreate = new Project()
           {
               Caption = getQuote.Lead.GroupName + "  " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
               Alias = getQuote.Lead.GroupName + "  " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
               Description = "Contract creation for " + getQuote.Lead.GroupName +  " with reference " + getQuote.Lead.ReferenceNo,
               IsActive = true,
               ProjectImage = getQuote.Lead.LogoUrl,
               WorkspaceId = 148,
               CreatedAt = DateTime.Now,
               CreatedById = httpContext.GetLoggedInUserId(),
           };
           var projectEntity = await _context.Projects.AddAsync(projectToCreate);
           await _context.SaveChangesAsync();
           await CreateLeadProjectWatcher(projectEntity.Entity.Id, requestId, httpContext);

           foreach (var task in serviceCategoryTasks)
           {
               if (task.EndorsementTypeId == 1)
               {
                   var currentTask = new Task()
                   {
                       IsActive = true,
                       ProjectId = projectEntity.Entity.Id,
                       Caption = task.Caption + " " + task.ServiceCategory.Services.First().Name + " " +
                                 DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                       Alias = task.Caption + " " + task.ServiceCategory.Services.First().Name,
                       IsAssigned = false,
                       CreatedAt = DateTime.Now,
                       CreatedById = httpContext.GetLoggedInUserId(),
                       IsReassigned = false,
                       Description = task.Caption + " for service " + task.ServiceCategory.Services.First().Name +
                                     $" for service code {(task.ServiceCategory.Services.First().ServiceCode)}.",
                       TaskStartDate = DateTime.Now,
                       TaskEndDate = DateTime.Now.AddDays(1),
                       WorkingManHours = (DateTime.Now.AddDays(1).Date - DateTime.Now.Date).Days,
                   };
                    
                   var taskEntity = await _context.Tasks.AddAsync(currentTask);
                   await _context.SaveChangesAsync();
                   await SaveTaskAssignees(task.ServiceCategoryId, taskEntity.Entity.Id, httpContext);
               }
           }
           return CommonResponse.Send(ResponseCodes.FAILURE, null, "An error occurred");

       }

       
        public async Task<ApiCommonResponse> CreateEndorsementProjectWatcher(long projectId,long requestId,HttpContext httpContext)
       {
           var headers = new List<UserProfile>();
           var quoteService = new List<QuoteService>();
           var getHeaders = await _context.Services.AsNoTracking()
               .Include(x=>x.Division)
               .ThenInclude(x=>x.Head)
               .Include(x=>x.OperatingEntity)
               .ThenInclude(x=>x.Head)
               .Where(x=>x.IsDeleted == false && x.Id == requestId)
               .OrderByDescending(x=>x.CreatedAt)
               .FirstOrDefaultAsync();
           
           if (getHeaders != null)
           {
               headers.Add(getHeaders.Division.Head);
               headers.Add(getHeaders.OperatingEntity.Head);
           }

           var getDistinctHeaders = headers.GroupBy(x => x.Id)
               .Select(x => x.First())
               .ToList();

           if (!getDistinctHeaders.Any())
           {
               return CommonResponse.Send(ResponseCodes.FAILURE, null, "No watcher to add");
           }

           var projectWatcherList = new List<Watcher>();
           foreach (var header in getDistinctHeaders)
           {
               var projectWatcher = new Watcher()
               {
                   IsActive = true,
                   CreatedAt = DateTime.Now,
                   CreatedById = httpContext.GetLoggedInUserId(),
                   ProjectId = projectId,
                   ProjectWatcherId = header.Id,
               };
               
               projectWatcherList.Add(projectWatcher);
           }
           await _context.Watchers.AddRangeAsync(projectWatcherList);
           await _context.SaveChangesAsync();
           return CommonResponse.Send(ResponseCodes.FAILURE, null, "An error occurred");
           
       }

       

       public async Task<ApiCommonResponse> CreateLeadProjectWatcher(long projectId,long requestId,HttpContext httpContext)
       {
           var headers = new List<UserProfile>();
           var getHeadersFromMarket = await _context.LeadDivisions.AsNoTracking()
               .Include(x=>x.Quote)
               .ThenInclude(x=>x.QuoteServices)
               .ThenInclude(x=>x.Service)
               .ThenInclude(x=>x.OperatingEntity)
               .ThenInclude(x=>x.Head)
               .Where(x=>x.IsDeleted == false && x.LeadId == requestId)
               .OrderByDescending(x=>x.CreatedAt)
               .FirstOrDefaultAsync();
           
           var getHeadersFromDivision = await _context.LeadDivisions.AsNoTracking()
               .Include(x=>x.Quote)
               .ThenInclude(x=>x.QuoteServices)
               .ThenInclude(x=>x.Service)
               .ThenInclude(x=>x.Division)
               .ThenInclude(x=>x.Head)
               .Where(x=>x.IsDeleted == false && x.LeadId == requestId)
               .OrderByDescending(x=>x.CreatedAt)
               .FirstOrDefaultAsync(); 

           if (getHeadersFromMarket != null)
           {
               foreach (var quote in getHeadersFromMarket.Quote.QuoteServices)
               {
                   headers.Add(quote.Service.OperatingEntity.Head);
               }
           }

           if (getHeadersFromDivision != null)
           {
               foreach (var quote in getHeadersFromDivision.Quote.QuoteServices)
               {
                   headers.Add(quote.Service.Division.Head);
               }
           }

           var getDistinctHeaders = headers.GroupBy(x => x.Id)
               .Select(x => x.First())
               .ToList();

           if (!getDistinctHeaders.Any())
           {
               return CommonResponse.Send(ResponseCodes.FAILURE, null, "No watcher to add");
           }

           var projectWatcherList = new List<Watcher>();
           foreach (var header in getDistinctHeaders)
           {
               var projectWatcher = new Watcher()
               {
                   IsActive = true,
                   CreatedAt = DateTime.Now,
                   CreatedById = httpContext.GetLoggedInUserId(),
                   ProjectId = projectId,
                   ProjectWatcherId = header.Id,
               };
               
               projectWatcherList.Add(projectWatcher);
           }

           await _context.Watchers.AddRangeAsync(projectWatcherList);
           await _context.SaveChangesAsync();
           
           return CommonResponse.Send(ResponseCodes.FAILURE, null, "An error occurred");
           
       }
       
       
       
   
    }
}