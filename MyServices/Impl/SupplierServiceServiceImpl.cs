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
        private readonly ISupplierServiceRepository _supplierServiceRepo;
        private readonly IMapper _mapper;

        public SupplierServiceServiceImpl(IModificationHistoryRepository historyRepo, ISupplierServiceRepository supplierServiceRepo, ILogger<SupplierServiceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._supplierServiceRepo = supplierServiceRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddSupplierService(HttpContext context, SupplierServiceReceivingDTO supplierServiceReceivingDTO)
        {
            var supplierService = _mapper.Map<SupplierService>(supplierServiceReceivingDTO);
            supplierService.CreatedById = context.GetLoggedInUserId();
            supplierService.IsAvailable = true;
            var savedSupplierService = await _supplierServiceRepo.SaveSupplierService(supplierService);
            if (savedSupplierService == null)
            {
                return new ApiResponse(500);
            }
            var supplierServiceTransferDTO = _mapper.Map<SupplierServiceTransferDTO>(supplierService);
            return new ApiOkResponse(supplierServiceTransferDTO);
        }

        public async Task<ApiResponse> DeleteSupplierService(long id)
        {
            var supplierServiceToDelete = await _supplierServiceRepo.FindSupplierServiceById(id);
            if(supplierServiceToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _supplierServiceRepo.DeleteSupplierService(supplierServiceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
        public async Task<ApiResponse> GetSupplierServiceById(long id)
        {
            var SupplierService = await _supplierServiceRepo.FindSupplierServiceById(id);
            if (SupplierService == null)
            {
                return new ApiResponse(404);
            }
            var SupplierServiceTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(SupplierService);
            return new ApiOkResponse(SupplierServiceTransferDTOs);
        }
        public async Task<ApiResponse> GetAllSupplierServiceCategories()
        {
            var supplierService = await _supplierServiceRepo.GetSupplierServices();
            if (supplierService == null)
            {
                return new ApiResponse(404);
            }
            var supplierServiceTransferDTO = _mapper.Map<IEnumerable<SupplierServiceTransferDTO>>(supplierService);
            return new ApiOkResponse(supplierServiceTransferDTO);
        }

        public  async Task<ApiResponse> UpdateSupplierService(HttpContext context, long id, SupplierServiceReceivingDTO supplierServiceReceivingDTO)
        {
            var supplierServiceToUpdate = await _supplierServiceRepo.FindSupplierServiceById(id);
            if (supplierServiceToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {supplierServiceToUpdate.ToString()} \n" ;

            // supplierServiceToUpdate.CategoryName = supplierServiceReceivingDTO.CategoryName;
            supplierServiceToUpdate.Description = supplierServiceReceivingDTO.Description;
            var updatedSupplierService = await _supplierServiceRepo.UpdateSupplierService(supplierServiceToUpdate);

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

            var supplierServiceTransferDTOs = _mapper.Map<SupplierServiceTransferDTO>(updatedSupplierService);
            return new ApiOkResponse(supplierServiceTransferDTOs);
        }
    }
}