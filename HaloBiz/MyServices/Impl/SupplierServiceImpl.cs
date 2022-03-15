using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HaloBiz.Model;
using System;
using System.Linq;
using Halobiz.Common.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class SupplierServiceImpl : ISupplierService
    {
        private readonly ILogger<SupplierServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISupplierRepository _supplierCategoryRepo;
        private readonly ISupplierContactRepository _supplierContactRepo;
        private readonly IMapper _mapper;

        public SupplierServiceImpl(IModificationHistoryRepository historyRepo, ISupplierRepository supplierCategoryRepo, ISupplierContactRepository supplierContactRepository, ILogger<SupplierServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierCategoryRepo = supplierCategoryRepo;
            this._supplierContactRepo = supplierContactRepository;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddSupplier(HttpContext context, SupplierReceivingDTO supplierCategoryReceivingDTO)
        {
            try
            {
                var supplierCategory = _mapper.Map<Supplier>(supplierCategoryReceivingDTO);
                supplierCategory.CreatedById = context.GetLoggedInUserId();
                
                //check for duplicates before saving
                List<IValidation> duplicates = await _supplierCategoryRepo.ValidateSupplier(supplierCategory.SupplierName, supplierCategory.SupplierEmail, supplierCategory.MobileNumber);

                if (duplicates.Count == 0)
                {
                    var savedSupplier = await _supplierCategoryRepo.SaveSupplier(supplierCategory);
                    if (savedSupplier == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    
                    var supplierCategoryTransferDTO = _mapper.Map<SupplierTransferDTO>(supplierCategory);

                    //Insert contacts into mapping tables
                    var contacts = supplierCategoryReceivingDTO.Contacts;
                    var contactsArr = contacts.Split(',').ToList();
                    foreach(var contact in contactsArr)
                    {

                        SupplierContactMapping contactMappingObj = new()
                        {
                            ContactId = Convert.ToInt64(Convert.ToDecimal(contact)),
                            CreatedById = context.GetLoggedInUserId(),
                            SupplierId = savedSupplier.Id
                        };
                        var savedContactMapping = await _supplierContactRepo.SaveSupplierContact(contactMappingObj);
                    }
                  
                    return CommonResponse.Send(ResponseCodes.SUCCESS, supplierCategoryTransferDTO);
                }

                string msg = "";

                for (var a = 0; a < duplicates.Count; a++)
                {
                    msg += $"{duplicates[a].Message}, ";
                }
                msg.TrimEnd(',');

                return CommonResponse.Send(ResponseCodes.FAILURE, null, msg);
            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e);
            }
        }

        public async Task<ApiCommonResponse> GetSupplierById(long id)
        {
            var Supplier = await _supplierCategoryRepo.FindSupplierById(id);
            if (Supplier == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var SupplierTransferDTOs = _mapper.Map<SupplierTransferDTO>(Supplier);
            
            //fetch suppliermappings
            var supplierMappings = await _supplierContactRepo.GetContactsBySupplier(SupplierTransferDTOs.Id);
            if (supplierMappings != null)
            {
                //map to response format
                List<string> contact = new List<string>();
                foreach (var contactMapping in supplierMappings)
                {
                    contact.Add(contactMapping.ContactId.ToString());
                }
                var con = string.Join(',', contact.ToArray());

                SupplierTransferDTOs.Contacts = con;
            }



            
            return CommonResponse.Send(ResponseCodes.SUCCESS,SupplierTransferDTOs);
        }

        public async Task<ApiCommonResponse> DeleteSupplier(long id)
        {
            var supplierCategoryToDelete = await _supplierCategoryRepo.FindSupplierById(id);
            if(supplierCategoryToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _supplierCategoryRepo.DeleteSupplier(supplierCategoryToDelete))
            {
                await _supplierContactRepo.DeleteSupplierContact(id);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllSupplierCategories()
        {
            var supplierCategory = await _supplierCategoryRepo.GetSuppliers();
            if (supplierCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var supplierCategoryTransferDTO = _mapper.Map<IEnumerable<SupplierTransferDTO>>(supplierCategory);

            foreach (var supplier in supplierCategoryTransferDTO)
            {
                //fetch suppliermappings
                var supplierMappings = await _supplierContactRepo.GetContactsBySupplier(supplier.Id);
                if (supplierMappings != null)
                {
                    //map to response format
                    List<string> contact = new List<string>();
                    foreach (var contactMapping in supplierMappings)
                    {
                        contact.Add(contactMapping.ContactId.ToString());
                    }
                    var con = string.Join(',', contact.ToArray());

                    supplier.Contacts = con;
                }

               

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,supplierCategoryTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateSupplier(HttpContext context, long id, SupplierReceivingDTO supplierCategoryReceivingDTO)
        {
            try
            {
                var supplierCategoryToUpdate = await _supplierCategoryRepo.FindSupplierById(id);
                if (supplierCategoryToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                }

                var summary = $"Initial details before change, \n {supplierCategoryToUpdate.ToString()} \n";

                supplierCategoryToUpdate.SupplierName = supplierCategoryReceivingDTO.SupplierName;
                supplierCategoryToUpdate.SupplierCategoryId = supplierCategoryReceivingDTO.SupplierCategoryId;
                supplierCategoryToUpdate.SupplierEmail = supplierCategoryReceivingDTO.SupplierEmail;
                supplierCategoryToUpdate.MobileNumber = supplierCategoryReceivingDTO.MobileNumber;
                supplierCategoryToUpdate.StateId = supplierCategoryReceivingDTO.StateId;
                supplierCategoryToUpdate.Lgaid = supplierCategoryReceivingDTO.LGAId;
                supplierCategoryToUpdate.Street = supplierCategoryReceivingDTO.Street;
                supplierCategoryToUpdate.Address = supplierCategoryReceivingDTO.Address;
                supplierCategoryToUpdate.ImageUrl = System.String.IsNullOrWhiteSpace(supplierCategoryReceivingDTO.Description) ? supplierCategoryToUpdate.ImageUrl : supplierCategoryReceivingDTO.ImageUrl;

                var updatedSupplier = await _supplierCategoryRepo.UpdateSupplier(supplierCategoryToUpdate);

                await _supplierContactRepo.DeleteSupplierContact(id);
                //var supplierMappedDTO = _mapper.Map<SupplierTransferDTO>(supplierCategoryToUpdate);
                var contacts = supplierCategoryReceivingDTO.Contacts;
                var contactsArr = contacts.Split(',').ToList();
                foreach (var contact in contactsArr)
                {

                    SupplierContactMapping contactMappingObj = new()
                    {
                        ContactId = Convert.ToInt64(Convert.ToDecimal(contact)),
                        CreatedById = context.GetLoggedInUserId(),
                        SupplierId = id
                    };
                    var savedContactMapping = await _supplierContactRepo.SaveSupplierContact(contactMappingObj);
                }

                summary += $"Details after change, \n {updatedSupplier.ToString()} \n";

                if (updatedSupplier == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "Supplier",
                    ChangeSummary = summary,
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = updatedSupplier.Id
                };

                await _historyRepo.SaveHistory(history);

                var supplierCategoryTransferDTOs = _mapper.Map<SupplierTransferDTO>(updatedSupplier);
                return CommonResponse.Send(ResponseCodes.SUCCESS,supplierCategoryTransferDTOs);
            }
            catch(System.Exception error)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE,error, "System errors");
            }
        }
    }
}