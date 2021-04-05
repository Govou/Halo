using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class SupplierServiceImpl : ISupplierService
    {
        private readonly ILogger<SupplierServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISupplierRepository _supplierCategoryRepo;
        private readonly IMapper _mapper;

        public SupplierServiceImpl(IModificationHistoryRepository historyRepo, ISupplierRepository supplierCategoryRepo, ILogger<SupplierServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierCategoryRepo = supplierCategoryRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddSupplier(HttpContext context, SupplierReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategory = _mapper.Map<Supplier>(supplierCategoryReceivingDTO);
            supplierCategory.CreatedById = context.GetLoggedInUserId();
            var savedSupplier = await _supplierCategoryRepo.SaveSupplier(supplierCategory);
            if (savedSupplier == null)
            {
                return new ApiResponse(500);
            }
            var supplierCategoryTransferDTO = _mapper.Map<SupplierTransferDTO>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public async Task<ApiResponse> GetSupplierById(long id)
        {
            var groupType = await _supplierCategoryRepo.FindSupplierById(id);
            if (groupType == null)
            {
                return new ApiResponse(404);
            }
            var groupTypeTransferDTOs = _mapper.Map<GroupTypeTransferDTO>(groupType);
            return new ApiOkResponse(groupTypeTransferDTOs);
        }

        public async Task<ApiResponse> DeleteSupplier(long id)
        {
            var supplierCategoryToDelete = await _supplierCategoryRepo.FindSupplierById(id);
            if(supplierCategoryToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _supplierCategoryRepo.DeleteSupplier(supplierCategoryToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllSupplierCategories()
        {
            var supplierCategory = await _supplierCategoryRepo.GetSuppliers();
            if (supplierCategory == null)
            {
                return new ApiResponse(404);
            }
            var supplierCategoryTransferDTO = _mapper.Map<IEnumerable<SupplierTransferDTO>>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public  async Task<ApiResponse> UpdateSupplier(HttpContext context, long id, SupplierReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategoryToUpdate = await _supplierCategoryRepo.FindSupplierById(id);
            if (supplierCategoryToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {supplierCategoryToUpdate.ToString()} \n" ;

            // supplierCategoryToUpdate.CategoryName = supplierCategoryReceivingDTO.CategoryName;
            supplierCategoryToUpdate.Description = supplierCategoryReceivingDTO.Description;
            var updatedSupplier = await _supplierCategoryRepo.UpdateSupplier(supplierCategoryToUpdate);

            summary += $"Details after change, \n {updatedSupplier.ToString()} \n";

            if (updatedSupplier == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Supplier",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSupplier.Id
            };

            await _historyRepo.SaveHistory(history);

            var supplierCategoryTransferDTOs = _mapper.Map<SupplierTransferDTO>(updatedSupplier);
            return new ApiOkResponse(supplierCategoryTransferDTOs);
        }
    }
}