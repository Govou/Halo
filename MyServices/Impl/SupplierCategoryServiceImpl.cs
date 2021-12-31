using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class SupplierCategoryServiceImpl : ISupplierCategoryService
    {
        private readonly ILogger<SupplierCategoryServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISupplierCategoryRepository _supplierCategoryRepo;
        private readonly IMapper _mapper;

        public SupplierCategoryServiceImpl(IModificationHistoryRepository historyRepo, ISupplierCategoryRepository supplierCategoryRepo, ILogger<SupplierCategoryServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierCategoryRepo = supplierCategoryRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddSupplierCategory(HttpContext context, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategory = _mapper.Map<SupplierCategory>(supplierCategoryReceivingDTO);
            supplierCategory.CreatedById = context.GetLoggedInUserId();
            var savedSupplierCategory = await _supplierCategoryRepo.SaveSupplierCategory(supplierCategory);
            if (savedSupplierCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var supplierCategoryTransferDTO = _mapper.Map<SupplierCategoryTransferDTO>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSupplierCategoryById(long id)
        {
            var SupplierCategory = await _supplierCategoryRepo.FindSupplierCategoryById(id);
            if (SupplierCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var SupplierCategoryTransferDTOs = _mapper.Map<SupplierCategoryTransferDTO>(SupplierCategory);
            return new ApiOkResponse(SupplierCategoryTransferDTOs);
        }

        public async Task<ApiCommonResponse> DeleteSupplierCategory(long id)
        {
            var supplierCategoryToDelete = await _supplierCategoryRepo.FindSupplierCategoryById(id);
            if(supplierCategoryToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _supplierCategoryRepo.DeleteSupplierCategory(supplierCategoryToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllSupplierCategories()
        {
            var supplierCategory = await _supplierCategoryRepo.GetSupplierCategories();
            if (supplierCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var supplierCategoryTransferDTO = _mapper.Map<IEnumerable<SupplierCategoryTransferDTO>>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateSupplierCategory(HttpContext context, long id, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategoryToUpdate = await _supplierCategoryRepo.FindSupplierCategoryById(id);
            if (supplierCategoryToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {supplierCategoryToUpdate.ToString()} \n" ;

            supplierCategoryToUpdate.CategoryName = supplierCategoryReceivingDTO.CategoryName;
            supplierCategoryToUpdate.Description = supplierCategoryReceivingDTO.Description;
            var updatedSupplierCategory = await _supplierCategoryRepo.UpdateSupplierCategory(supplierCategoryToUpdate);

            summary += $"Details after change, \n {updatedSupplierCategory.ToString()} \n";

            if (updatedSupplierCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "SupplierCategory",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSupplierCategory.Id
            };

            await _historyRepo.SaveHistory(history);

            var supplierCategoryTransferDTOs = _mapper.Map<SupplierCategoryTransferDTO>(updatedSupplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTOs);
        }
    }
}