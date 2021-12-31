using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HaloBiz.MyServices;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.MyServices.Impl
{
    public class ServicesServiceImpl : IServicesService
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly IMapper _mapper;
        private readonly IServiceRequiredServiceDocumentRepository _requiredServiceDocRepo;
        private readonly IDeleteLogRepository _deleteLogRepo;
        private readonly IModificationHistoryRepository _modificationRepo;
        private readonly IServiceRequredServiceQualificationElementRepository _reqServiceElementRepo;
        private readonly IApprovalService _approvalService;
        private readonly HalobizContext _context;
        private readonly ILogger<ServicesServiceImpl> _logger;

        public ServicesServiceImpl(
                                IServicesRepository servicesRepository,
                                IMapper mapper,
                                IServiceRequiredServiceDocumentRepository requiredServiceDocRepo,
                                IDeleteLogRepository deleteLogRepo,
                                IModificationHistoryRepository modificationRepo,
                                IServiceRequredServiceQualificationElementRepository reqServiceElementRepo,
                                IApprovalService approvalService,
                                HalobizContext context,
                                ILogger<ServicesServiceImpl> logger
                                )
        {
            this._mapper = mapper;
            this._requiredServiceDocRepo = requiredServiceDocRepo;
            this._deleteLogRepo = deleteLogRepo;
            this._modificationRepo = modificationRepo;
            this._reqServiceElementRepo = reqServiceElementRepo;
            this._approvalService = approvalService;
            this._context = context;
            this._logger = logger;
            this._servicesRepository = servicesRepository;
        }

        public async Task<ApiCommonResponse> AddService(HttpContext context, ServiceReceivingDTO servicesReceivingDTO)
        {
            //check if this is an admin and if the direct service is specified
            if (servicesReceivingDTO.ServiceRelationshipEnum == ServiceRelationshipEnum.Admin)
            {
                //check if the direct service is specified
                if(servicesReceivingDTO.DirectServiceId == null)
                    return new ApiResponse(400, "The direct service is not specified for this admin service.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    IList<ServiceRequiredServiceDocument> serviceRequiredServiceDocument = new List<ServiceRequiredServiceDocument>();
                    IList<ServiceRequredServiceQualificationElement> serviceQualificationElements = new List<ServiceRequredServiceQualificationElement>();

                    var service = _mapper.Map<Service>(servicesReceivingDTO);
                    service.CreatedById = context.GetLoggedInUserId();
                    var savedService = await _servicesRepository.SaveService(service);

                    foreach (long id in servicesReceivingDTO.RequiredServiceFieldsId)
                    {
                        serviceQualificationElements.Add(new ServiceRequredServiceQualificationElement()
                        {
                            ServicesId = savedService.Id,
                            RequredServiceQualificationElementId = id
                        });
                    }

                    foreach (long id in servicesReceivingDTO.RequiredDocumentsId)
                    {
                        serviceRequiredServiceDocument.Add(new ServiceRequiredServiceDocument()
                        {
                            ServicesId = savedService.Id,
                            RequiredServiceDocumentId = id
                        });
                    }

                    

                    var isFieldsSaved = await _reqServiceElementRepo.SaveRangeServiceRequredServiceQualificationElement(serviceQualificationElements);

                    var isDocSaved = await _requiredServiceDocRepo.SaveRangeServiceRequiredServiceDocument(serviceRequiredServiceDocument);

                    var successful = await _approvalService.SetUpApprovalsForServiceCreation(savedService, context);
                    if (!successful) 
                    {
                        await transaction.RollbackAsync();
                        return new ApiResponse(500, "Could not set up approvals for the service."); 
                    }

                    var servicesTransferDTO = _mapper.Map<ServiceTransferDTO>(savedService);
                    await transaction.CommitAsync();
                    return new ApiOkResponse(servicesTransferDTO);

                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }

        public async Task<ApiCommonResponse> GetAllServices()
        {
            var services = await _servicesRepository.FindAllServices();
            if (services == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTransferDTO = _mapper.Map<IEnumerable<ServiceTransferDTO>>(services);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetUnpublishedServices()
        {
            var services = await _servicesRepository.FindAllUnplishedServices();
            if (services == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTransferDTO = _mapper.Map<IEnumerable<ServiceTransferDTO>>(services);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetOnlinePortalServices()
        {
            var services = await _servicesRepository.FindOnlinePortalServices();
            if (services == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTransferDTO = _mapper.Map<IEnumerable<ServiceTransferDTO>>(services);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceById(long id)
        {
            var service = await _servicesRepository.FindServicesById(id);
            if (service == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(service);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceByName(string name)
        {
            var service = await _servicesRepository.FindServiceByName(name);
            if (service == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(service);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateService(HttpContext context, long id, ServiceReceivingDTO serviceReceivingDTO)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {serviceToUpdate.ToString()} \n";


            serviceToUpdate.Name = serviceReceivingDTO.Name;
            serviceToUpdate.Description = serviceReceivingDTO.Description;
            serviceToUpdate.ImageUrl = serviceReceivingDTO.ImageUrl;
            serviceToUpdate.TargetId = serviceReceivingDTO.TargetId;
            serviceToUpdate.UnitPrice = serviceReceivingDTO.UnitPrice;
            serviceToUpdate.ServiceTypeId = serviceReceivingDTO.ServiceTypeId;
            serviceToUpdate.IsVatable = serviceReceivingDTO.IsVatable;
            serviceToUpdate.CanBeSoldOnline = serviceReceivingDTO.CanBeSoldOnline;
            serviceToUpdate.ServiceRelationshipEnum = serviceReceivingDTO.ServiceRelationshipEnum;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            summary += $"Details after change, \n {serviceToUpdate.ToString()} \n";

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);
            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }
        public async Task<ApiCommonResponse> UpdateServices(HttpContext context, long id, ServiceReceivingDTO serviceReceivingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Gets the service to update
                    var serviceToUpdate = await _context.Services.Include(service => service.Target)
                        .Include(service => service.ServiceType)
                        .Include(service => service.Account)
                        .Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
                            .ThenInclude(row => row.RequiredServiceDocument)
                        .Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
                            .ThenInclude(row => row.RequredServiceQualificationElement)
                        .FirstOrDefaultAsync( service => service.Id == id && service.IsDeleted == false);
                    
                    if (serviceToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }
                    
                    var listOfDocToAdd = new List<ServiceRequiredServiceDocument>(); 

                    //Goes through the list checks if the id exists in the list of docId 
                    //In the incoming request, if yes, it removes the id from the incoming list because there 
                    //will be no need for creating a new one, else it removes it creates a new ServiceServiceRequiredDoc
                    //And drops it from the record
                    foreach(var doc in serviceToUpdate.ServiceRequiredServiceDocuments)
                    {
                        if (serviceReceivingDTO.RequiredDocumentsId.Contains(doc.RequiredServiceDocumentId))
                        {
                            serviceReceivingDTO.RequiredDocumentsId.Remove(doc.RequiredServiceDocumentId);
                        }else{
                            _context.ServiceRequiredServiceDocuments.Remove(doc);
                        }
                    }
                    await _context.SaveChangesAsync();

                    foreach (long docId in serviceReceivingDTO.RequiredDocumentsId)
                    {
                        listOfDocToAdd.Add(new ServiceRequiredServiceDocument()
                        {
                            ServicesId = id,
                            RequiredServiceDocumentId = docId,
                        });
                    }
                        
                    
                    if(listOfDocToAdd.Count > 0)
                        await _context.ServiceRequiredServiceDocuments.AddRangeAsync(listOfDocToAdd);


                    var listOfElementToAdd = new List<ServiceRequredServiceQualificationElement>(); 

                    foreach(var element in serviceToUpdate.ServiceRequredServiceQualificationElements)
                    {
                        if (serviceReceivingDTO.RequiredServiceFieldsId.Contains(element.RequredServiceQualificationElementId))
                        {
                            serviceReceivingDTO.RequiredServiceFieldsId.Remove(element.RequredServiceQualificationElementId);
                        }else{
                           _context.ServiceRequredServiceQualificationElements.Remove(element);
                        }
                    }

                    foreach (long elementId in serviceReceivingDTO.RequiredServiceFieldsId)
                    {
                        listOfElementToAdd.Add(new ServiceRequredServiceQualificationElement()
                        {
                            ServicesId = id,
                            RequredServiceQualificationElementId = elementId
                        });
                    }

                    await _context.SaveChangesAsync();
                    
                    if(listOfElementToAdd.Count > 0)
                        await _context.ServiceRequredServiceQualificationElements.AddRangeAsync(listOfElementToAdd);
                    
                    var summary = $"Initial details before change, \n {serviceToUpdate.ToString()} \n";

                    serviceToUpdate.Name = serviceReceivingDTO.Name;
                    serviceToUpdate.Description = serviceReceivingDTO.Description;
                    serviceToUpdate.ImageUrl = serviceReceivingDTO.ImageUrl;
                    serviceToUpdate.TargetId = serviceReceivingDTO.TargetId;
                    serviceToUpdate.UnitPrice = serviceReceivingDTO.UnitPrice;
                    serviceToUpdate.ServiceTypeId = serviceReceivingDTO.ServiceTypeId;
                    serviceToUpdate.IsVatable = serviceReceivingDTO.IsVatable;
                    serviceToUpdate.CanBeSoldOnline = serviceReceivingDTO.CanBeSoldOnline;
                    serviceToUpdate.ServiceRelationshipEnum = serviceReceivingDTO.ServiceRelationshipEnum;
                    serviceToUpdate.DirectServiceId = serviceReceivingDTO.DirectServiceId;

                    var updatedService =  _context.Services.Update(serviceToUpdate).Entity;
                    await _context.SaveChangesAsync();

                    summary += $"Details after change, \n {updatedService.ToString()} \n";

                    ModificationHistory history = new ModificationHistory()
                    {
                        ModelChanged = "Service",
                        ChangeSummary = summary,
                        ChangedById = context.GetLoggedInUserId(),
                        ModifiedModelId = updatedService.Id
                    };

                    await _context.ModificationHistories.AddAsync(history);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var service = await _servicesRepository.FindServicesById(id);

                    var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(service);
                    return new ApiOkResponse(serviceTransferDTO);

                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }
        public async Task<ApiCommonResponse> ApproveService(HttpContext context, long id, long sequence)
        {

            var approvalsForTheService = await _context.Approvals.Where(x => !x.IsDeleted && x.ServicesId == id).ToListAsync();

            var theApproval = approvalsForTheService.SingleOrDefault(x => x.Sequence == sequence);

            if(theApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            theApproval.IsApproved = true;
            theApproval.DateTimeApproved = DateTime.Now;
            _context.Approvals.Update(theApproval);
            await _context.SaveChangesAsync();

            bool allApprovalsApproved = approvalsForTheService.All(x => x.IsApproved);

            // Return scenario 1
            // All the approvals for service not yet approved.
            if (!allApprovalsApproved) return CommonResponse.Send(ResponseCodes.SUCCESS);

            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            serviceToUpdate.PublishedApprovedStatus = true;
            serviceToUpdate.IsRequestedForPublish = true;
            serviceToUpdate.IsPublished = true;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"Service with ServiceId: {updatedService.Id} was approved by user with userid {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiCommonResponse> RequestPublishService(HttpContext context, long id)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            serviceToUpdate.IsRequestedForPublish = true;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"User with userId {context.GetLoggedInUserId()} requested for Service with ServiceId: {updatedService.Id} to be published ",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }
        public async Task<ApiCommonResponse> DisapproveService(HttpContext context, long serviceId, long sequence)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(serviceId);
            if (serviceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            serviceToUpdate.PublishedApprovedStatus = false;
            serviceToUpdate.IsRequestedForPublish = true;
            serviceToUpdate.IsPublished = true;


            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"Service with ServiceId: {updatedService.Id} was disapproved by user with userid {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServiceTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }

        public async Task<ApiCommonResponse> DeleteService(HttpContext context, long id)
        {
            var serviceToDelete = await _servicesRepository.FindServicesById(id);
            if (serviceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _servicesRepository.DeleteService(serviceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            DeleteLog deleteLog = new DeleteLog()
            {
                DeletedById = context.GetLoggedInUserId(),
                DeletedModelId = serviceToDelete.Id,
                ChangeSummary = $"Deleted service with Id: {serviceToDelete.Id}",

            };

            await _deleteLogRepo.SaveDeleteLog(deleteLog);

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
        public async Task<ApiCommonResponse> DeleteService(long id)
        {
            var serviceToDelete = await _servicesRepository.FindServicesById(id);
            if (serviceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _servicesRepository.DeleteService(serviceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}