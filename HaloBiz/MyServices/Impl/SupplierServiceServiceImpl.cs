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
using Halobiz.Common.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class SupplierServiceServiceImpl : ISupplierServiceService
    {
        private readonly ILogger<SupplierServiceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISupplierServiceRepository _supplierServiceRepo;
        private readonly IMapper _mapper;

        public SupplierServiceServiceImpl(IModificationHistoryRepository historyRepo, ISupplierServiceRepository supplierServiceRepo, ILogger<SupplierServiceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierServiceRepo = supplierServiceRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddSupplierService(HttpContext context, SupplierServiceReceivingDTO supplierServiceReceivingDTO)
        {
            var supplierService = _mapper.Map<SupplierService>(supplierServiceReceivingDTO);
            supplierService.CreatedById = context.GetLoggedInUserId();
            supplierService.IsAvailable = true;

            //check for duplicates before saving
            List<IValidation> duplicates = await _supplierServiceRepo.ValidateSupplierService(supplierService.ServiceName, supplierService.SupplierId);

            if (duplicates.Count == 0)
            {
                var savedSupplierService = await _supplierServiceRepo.SaveSupplierService(supplierService);
                if (savedSupplierService == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                var supplierServiceTransferDTO = _mapper.Map<SupplierServiceTransferDTO>(supplierService);
                return CommonResponse.Send(ResponseCodes.SUCCESS, supplierServiceTransferDTO);
            }

            string msg = "";

            for (var a = 0; a < duplicates.Count; a++)
            {
                msg += $"{duplicates[a].Message}, ";
            }
            msg.TrimEnd(',');

            return CommonResponse.Send(ResponseCodes.FAILURE, null, msg);


            
        }

        public async Task<ApiCommonResponse> DeleteSupplierService(long id)
        {
            var supplierServiceToDelete = await _supplierServiceRepo.FindSupplierServiceById(id);
            if(supplierServiceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _supplierServiceRepo.DeleteSupplierService(supplierServiceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
        public async Task<ApiCommonResponse> GetSupplierServiceById(long id)
        {
            var SupplierService = await _supplierServiceRepo.FindSupplierServiceById(id);
            if (SupplierService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var SupplierServiceTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(SupplierService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,SupplierServiceTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetAllSupplierServiceCategories()
        {
            var supplierService = await _supplierServiceRepo.GetSupplierServices();
            if (supplierService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var supplierServiceTransferDTO = _mapper.Map<IEnumerable<SupplierServiceTransferDTO>>(supplierService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,supplierServiceTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateSupplierService(HttpContext context, long id, SupplierServiceReceivingDTO supplierServiceReceivingDTO)
        {
            var supplierServiceToUpdate = await _supplierServiceRepo.FindSupplierServiceById(id);
            if (supplierServiceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            
            var summary = $"Initial details before change, \n {supplierServiceToUpdate.ToString()} \n" ;

            supplierServiceToUpdate.AveragePrice = supplierServiceReceivingDTO.AveragePrice;
            supplierServiceToUpdate.SupplierId = supplierServiceReceivingDTO.SupplierId;
            supplierServiceToUpdate.StandardDiscount = supplierServiceReceivingDTO.StandardDiscount;
            supplierServiceToUpdate.UnitCostPrice = supplierServiceReceivingDTO.UnitCostPrice;
            supplierServiceToUpdate.IdentificationNumber = supplierServiceReceivingDTO.IdentificationNumber;
            supplierServiceToUpdate.SerialNumber = supplierServiceReceivingDTO.SerialNumber;
            supplierServiceToUpdate.ImageUrl = supplierServiceReceivingDTO.ImageUrl;
            supplierServiceToUpdate.ModelNumber = supplierServiceReceivingDTO.ModelNumber;
            supplierServiceToUpdate.Model = supplierServiceReceivingDTO.Model;
            supplierServiceToUpdate.Make = supplierServiceReceivingDTO.Make;
            supplierServiceToUpdate.Description = supplierServiceReceivingDTO.Description;
            supplierServiceToUpdate.ServiceName = supplierServiceReceivingDTO.ServiceName;

            supplierServiceToUpdate.InteriorViewImage = supplierServiceReceivingDTO.InteriorViewImage;
            supplierServiceToUpdate.TopViewImage = supplierServiceReceivingDTO.TopViewImage;
            supplierServiceToUpdate.LeftViewImage = supplierServiceReceivingDTO.LeftViewImage;
            supplierServiceToUpdate.RearViewImage = supplierServiceReceivingDTO.RearViewImage;
            supplierServiceToUpdate.RightViewImage = supplierServiceReceivingDTO.RightViewImage;
            supplierServiceToUpdate.FrontViewImage = supplierServiceReceivingDTO.FrontViewImage;
            var updatedSupplierService = await _supplierServiceRepo.UpdateSupplierService(supplierServiceToUpdate);

            summary += $"Details after change, \n {updatedSupplierService.ToString()} \n";

            if (updatedSupplierService == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "SupplierService",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSupplierService.Id
            };

            await _historyRepo.SaveHistory(history);

            var supplierServiceTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(updatedSupplierService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,supplierServiceTransferDTOs);
        }
    }
}