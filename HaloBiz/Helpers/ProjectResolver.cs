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

    
       public async Task<ApiCommonResponse> ResolveService(long requestId,HttpContext httpContext)
       {
            var getService = await _context.Services.AsNoTracking()
                .Where(x=>x.Id == requestId)
                .Include(x => x.ServiceCategory)
                .ThenInclude(x => x.ServiceCategoryTasks)
                .FirstOrDefaultAsync();
            
            if (!getService.ServiceCategory.ServiceCategoryTasks.Any() && getService.Name == "GUARD ADMIN SERVICE")
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not create project fulfilment");
            }
            else
            {
                var projectToCreate = new Project()
                {
                   Caption = getService.Name + " Service Creation",
                   Alias = getService.Name + " Service Creation",
                   Description = getService.Description,
                   IsActive = true,
                   ProjectImage = getService.ImageUrl,
                   WorkspaceId = 148,
                   CreatedAt = DateTime.Now,
                   CreatedById = httpContext.GetLoggedInUserId(),
                };
                var projectEntity = await _context.Projects.AddAsync(projectToCreate);
                await _context.SaveChangesAsync();
                await SaveTask(getService, projectEntity.Entity.Id, httpContext);
            }
            
            return CommonResponse.Send(ResponseCodes.SUCCESS, getService, "SuccessFully savedComments");
        }


       public async Task<ApiCommonResponse> SaveTask(Service service,long projectId,HttpContext httpContext)
       {
           foreach (var task in service.ServiceCategory.ServiceCategoryTasks)
           {
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
               .Where(x => x.Id == getEndorsement.ServiceId)
               .Include(x => x.ServiceCategory)
               .ThenInclude(x => x.ServiceCategoryTasks)
               .FirstOrDefaultAsync();
           
           if (!getService.ServiceCategory.ServiceCategoryTasks.Any() && getService.Name == "GUARD ADMIN SERVICE")
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
               await SaveTask(getService, projectEntity.Entity.Id, httpContext);
           }
            
           return CommonResponse.Send(ResponseCodes.SUCCESS, getService, "SuccessFully savedComments");

       }

       // public async Task<ApiCommonResponse> ResolveLead(long requestId, HttpContext httpContext)
       // {
       //     var serviceArray = new List<Service>();
       //     var getLead = await _context.Leads.AsNoTracking()
       //         .Where(x => x.Id == requestId)
       //         .FirstOrDefaultAsync();
       //
       //     var getService = await _context.RepAmortizationMasters.AsNoTracking()
       //         .Where(x => x.ClientId == getLead.CustomerId)
       //         .Include(x => x.Service)
       //         .ThenInclude(x => x.ServiceCategory)
       //         .ThenInclude(x => x.ServiceCategoryTasks.Where(x => x.IsDeleted == false))
       //         .ToListAsync();
       //
       //     foreach (var service in getService)
       //     {
       //         
       //         serviceArray.Add(service.Service);
       //     }
       //
       //     if (!serviceArray.Any())
       //     {
       //         return CommonResponse.Send(ResponseCodes.FAILURE, null, "Lead is Empty");
       //     }
       //
       //     foreach (var service in serviceArray)
       //     {
       //         if (!service.ServiceCategory.ServiceCategoryTasks.Any())
       //         {
       //             return CommonResponse.Send(ResponseCodes.FAILURE, null, "Service Category is Empty");
       //         }
       //         
       //         
       //             
       //     }
       // }
       
   
    }
}