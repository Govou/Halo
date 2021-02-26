using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly DataContext _context;
        private readonly ILogger<ServicesServiceImpl> _logger;

        public ServicesServiceImpl(
                                IServicesRepository servicesRepository,
                                IMapper mapper,
                                IServiceRequiredServiceDocumentRepository requiredServiceDocRepo,
                                IDeleteLogRepository deleteLogRepo,
                                IModificationHistoryRepository modificationRepo,
                                IServiceRequredServiceQualificationElementRepository reqServiceElementRepo,
                                IApprovalService approvalService,
                                DataContext context,
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

        public async Task<ApiResponse> AddService(HttpContext context, ServicesReceivingDTO servicesReceivingDTO)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try{
                        IList<ServiceRequiredServiceDocument> serviceRequiredServiceDocument = new List<ServiceRequiredServiceDocument>();
                        IList<ServiceRequredServiceQualificationElement> serviceQualificationElements = new List<ServiceRequredServiceQualificationElement>();

                        var service = _mapper.Map<Services>(servicesReceivingDTO);
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

                        var servicesTransferDTO = _mapper.Map<ServicesTransferDTO>(savedService);
                        await transaction.CommitAsync();
                        return new ApiOkResponse(servicesTransferDTO);

                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
        }

        public async Task<ApiResponse> GetAllServices()
        {
            var services = await _servicesRepository.FindAllServices();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var serviceTransferDTO = _mapper.Map<IEnumerable<ServicesTransferDTO>>(services);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiResponse> GetUnpublishedServices()
        {
            var services = await _servicesRepository.FindAllUnplishedServices();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var serviceTransferDTO = _mapper.Map<IEnumerable<ServicesTransferDTO>>(services);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiResponse> GetServiceById(long id)
        {
            var service = await _servicesRepository.FindServicesById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(service);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiResponse> GetServiceByName(string name)
        {
            var service = await _servicesRepository.FindServiceByName(name);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(service);
            return new ApiOkResponse(serviceTransferDTO);
        }

        public async Task<ApiResponse> UpdateService(HttpContext context, long id, ServicesReceivingDTO serviceReceivingDTO)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {serviceToUpdate.ToString()} \n";


            serviceToUpdate.Name = serviceReceivingDTO.Name;
            serviceToUpdate.Description = serviceReceivingDTO.Description;
            serviceToUpdate.ImageUrl = serviceReceivingDTO.ImageUrl;
            serviceToUpdate.TargetId = serviceReceivingDTO.TargetId;
            serviceToUpdate.UnitPrice = serviceReceivingDTO.UnitPrice;
            serviceToUpdate.ServiceTypeId = serviceReceivingDTO.ServiceTypeId;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return new ApiResponse(500);
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
            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }
        public async Task<ApiResponse> UpdateServices(HttpContext context, long id, ServicesReceivingDTO serviceReceivingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Gets the service to update
                    var serviceToUpdate = await _context.Services.Include(service => service.Target)
                        .Include(service => service.ServiceType)
                        .Include(service => service.Account)
                        .Include(service => service.RequiredServiceDocument.Where(row => row.IsDeleted == false))
                            .ThenInclude(row => row.RequiredServiceDocument)
                        .Include(service => service.RequredServiceQualificationElement.Where(row => row.IsDeleted == false))
                            .ThenInclude(row => row.RequredServiceQualificationElement)
                        .FirstOrDefaultAsync( service => service.Id == id && service.IsDeleted == false);
                    
                    if (serviceToUpdate == null)
                    {
                        return new ApiResponse(404);
                    }
                    
                    var listOfDocToAdd = new List<ServiceRequiredServiceDocument>(); 

                    //Goes through the list checks if the id exists in the list of docId 
                    //In the incoming request, if yes, it removes the id from the incoming list because there 
                    //will be no need for creating a new one, else it removes it creates a new ServiceServiceRequiredDoc
                    //And drops it from the record
                    foreach(var doc in serviceToUpdate.RequiredServiceDocument)
                    {
                        if (serviceReceivingDTO.RequiredDocumentsId.Contains(doc.RequiredServiceDocumentId))
                        {
                            serviceReceivingDTO.RequiredDocumentsId.Remove(doc.RequiredServiceDocumentId);
                        }else{
                            _context.ServiceRequiredServiceDocument.Remove(doc);
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
                        await _context.ServiceRequiredServiceDocument.AddRangeAsync(listOfDocToAdd);


                    var listOfElementToAdd = new List<ServiceRequredServiceQualificationElement>(); 

                    foreach(var element in serviceToUpdate.RequredServiceQualificationElement)
                    {
                        if (serviceReceivingDTO.RequiredServiceFieldsId.Contains(element.RequredServiceQualificationElementId))
                        {
                            serviceReceivingDTO.RequiredServiceFieldsId.Remove(element.RequredServiceQualificationElementId);
                        }else{
                           _context.ServiceRequredServiceQualificationElement.Remove(element);
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
                        await _context.ServiceRequredServiceQualificationElement.AddRangeAsync(listOfElementToAdd);
                    
                    var summary = $"Initial details before change, \n {serviceToUpdate.ToString()} \n";

                    serviceToUpdate.Name = serviceReceivingDTO.Name;
                    serviceToUpdate.Description = serviceReceivingDTO.Description;
                    serviceToUpdate.ImageUrl = serviceReceivingDTO.ImageUrl;
                    serviceToUpdate.TargetId = serviceReceivingDTO.TargetId;
                    serviceToUpdate.UnitPrice = serviceReceivingDTO.UnitPrice;
                    serviceToUpdate.ServiceTypeId = serviceReceivingDTO.ServiceTypeId;
                    serviceToUpdate.AccountId = serviceReceivingDTO.AccountId;

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

                    var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(service);
                    return new ApiOkResponse(serviceTransferDTO);

                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return new ApiResponse(500);
                }
            }
        }
        public async Task<ApiResponse> ApproveService(HttpContext context, long id)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return new ApiResponse(404);
            }

            serviceToUpdate.PublishedApprovedStatus = true;
            serviceToUpdate.IsRequestedForPublish = true;
            serviceToUpdate.IsPublished = true;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"Service with ServiceId: {updatedService.Id} was approved by user with userid {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }

        public async Task<ApiResponse> RequestPublishService(HttpContext context, long id)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return new ApiResponse(404);
            }

            serviceToUpdate.IsRequestedForPublish = true;

            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return new ApiResponse(500);
            }

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"User with userId {context.GetLoggedInUserId()} requested for Service with ServiceId: {updatedService.Id} to be published ",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }
        public async Task<ApiResponse> DisapproveService(HttpContext context, long id)
        {
            var serviceToUpdate = await _servicesRepository.FindServicesById(id);
            if (serviceToUpdate == null)
            {
                return new ApiResponse(404);
            }

            serviceToUpdate.PublishedApprovedStatus = false;
            serviceToUpdate.IsRequestedForPublish = true;
            serviceToUpdate.IsPublished = true;


            var updatedService = await _servicesRepository.UpdateServices(serviceToUpdate);

            if (updatedService == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Service",
                ChangeSummary = $"Service with ServiceId: {updatedService.Id} was disapproved by user with userid {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedService.Id
            };

            await _modificationRepo.SaveHistory(history);

            var serviceTransferDTO = _mapper.Map<ServicesTransferDTO>(updatedService);
            return new ApiOkResponse(serviceTransferDTO);

        }

        public async Task<ApiResponse> DeleteService(HttpContext context, long id)
        {
            var serviceToDelete = await _servicesRepository.FindServicesById(id);
            if (serviceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _servicesRepository.DeleteService(serviceToDelete))
            {
                return new ApiResponse(500);
            }

            DeleteLog deleteLog = new DeleteLog()
            {
                DeletedById = context.GetLoggedInUserId(),
                DeletedModelId = serviceToDelete.Id,
                ChangeSummary = $"Deleted service with Id: {serviceToDelete.Id}",

            };

            await _deleteLogRepo.SaveDeleteLog(deleteLog);

            return new ApiOkResponse(true);
        }
        public async Task<ApiResponse> DeleteService(long id)
        {
            var serviceToDelete = await _servicesRepository.FindServicesById(id);
            if (serviceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _servicesRepository.DeleteService(serviceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}