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
    public class SupplierServiceServiceImpl : ISupplierServiceService
    {
        private readonly ILogger<SupplierServiceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISupplierServiceRepository _supplierCategoryRepo;
        private readonly IMapper _mapper;

        public SupplierServiceServiceImpl(IModificationHistoryRepository historyRepo, ISupplierServiceRepository supplierCategoryRepo, ILogger<SupplierServiceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierCategoryRepo = supplierCategoryRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddSupplierService(HttpContext context, SupplierServiceReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategory = _mapper.Map<SupplierService>(supplierCategoryReceivingDTO);
            supplierCategory.CreatedById = context.GetLoggedInUserId();
            var savedSupplierService = await _supplierCategoryRepo.SaveSupplierService(supplierCategory);
            if (savedSupplierService == null)
            {
                return new ApiResponse(500);
            }
            var supplierCategoryTransferDTO = _mapper.Map<SupplierServiceTransferDTO>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public async Task<ApiResponse> DeleteSupplierService(long id)
        {
            var supplierCategoryToDelete = await _supplierCategoryRepo.FindSupplierServiceById(id);
            if(supplierCategoryToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _supplierCategoryRepo.DeleteSupplierService(supplierCategoryToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
        public async Task<ApiResponse> GetSupplierServiceById(long id)
        {
            var SupplierService = await _supplierCategoryRepo.FindSupplierServiceById(id);
            if (SupplierService == null)
            {
                return new ApiResponse(404);
            }
            var SupplierServiceTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(SupplierService);
            return new ApiOkResponse(SupplierServiceTransferDTOs);
        }
        public async Task<ApiResponse> GetAllSupplierServiceCategories()
        {
            var supplierCategory = await _supplierCategoryRepo.GetSupplierServices();
            if (supplierCategory == null)
            {
                return new ApiResponse(404);
            }
            var supplierCategoryTransferDTO = _mapper.Map<IEnumerable<SupplierServiceTransferDTO>>(supplierCategory);
            return new ApiOkResponse(supplierCategoryTransferDTO);
        }

        public  async Task<ApiResponse> UpdateSupplierService(HttpContext context, long id, SupplierServiceReceivingDTO supplierCategoryReceivingDTO)
        {
            var supplierCategoryToUpdate = await _supplierCategoryRepo.FindSupplierServiceById(id);
            if (supplierCategoryToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {supplierCategoryToUpdate.ToString()} \n" ;

            // supplierCategoryToUpdate.CategoryName = supplierCategoryReceivingDTO.CategoryName;
            supplierCategoryToUpdate.Description = supplierCategoryReceivingDTO.Description;
            var updatedSupplierService = await _supplierCategoryRepo.UpdateSupplierService(supplierCategoryToUpdate);

            summary += $"Details after change, \n {updatedSupplierService.ToString()} \n";

            if (updatedSupplierService == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "SupplierService",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSupplierService.Id
            };

            await _historyRepo.SaveHistory(history);

            var supplierCategoryTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(updatedSupplierService);
            return new ApiOkResponse(supplierCategoryTransferDTOs);
        }
    }
}