using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class FinancialVoucherTypeServiceImpl : IFinancialVoucherTypeService
    {
        
        private readonly ILogger<FinancialVoucherTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IFinancialVoucherTypeRepository _voucherTypeRepo;
        private readonly IMapper _mapper;

        public LeadOriginServiceImpl(IModificationHistoryRepository historyRepo, 
        IFinancialVoucherTypeRepository voucherTypeRepo, ILogger<FinancialVoucherTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._voucherTypeRepo = voucherTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddFinancialVoucherType(HttpContext context, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO)
        {
            var voucherType = _mapper.Map<FinanceVoucherType>(voucherTypeReceivingDTO);
            voucherType.CreatedById = context.GetLoggedInUserId();
            var savedVoucherType = await _voucherTypeRepo.SaveFinanceVoucherType(voucherType);
            if (savedVoucherType == null)
            {
                return new ApiResponse(500);
            }
            var voucherTypeTransferDTO = _mapper.Map<FinancialVoucherTypeTransferDTO>(voucherType);
            return new ApiOkResponse(voucherTypeTransferDTO);
        }

        public async Task<ApiResponse> GetAllFinancialVoucherTypes()
        {
            var voucherTypes = await _voucherTypeRepo.FindAllFinanceVoucherTypes();
            if (voucherTypes == null)
            {
                return new ApiResponse(404);
            }
            var voucherTypeTransferDTO = _mapper.Map<IEnumerable<FinancialVoucherTypeTransferDTO>>(voucherTypes);
            return new ApiOkResponse(voucherTypeTransferDTO);
        }

        public async Task<ApiResponse> GetFinancialVoucherTypeById(long id)
        {
            var voucherType = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherType == null)
            {
                return new ApiResponse(404);
            }
            var voucherTypeTransferDTO = _mapper.Map<FinancialVoucherTypeTransferDTO>(voucherType);
            return new ApiOkResponse(voucherTypeTransferDTO);
        }

        public async Task<ApiResponse> UpdateFinancialVoucherType(HttpContext context, long id, FinancialVoucherTypeReceivingDTO VoucherTypeReceivingDTO)
        {
            var voucherTypeToUpdate = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {voucherTypeToUpdate.ToString()} \n" ;

            voucherTypeToUpdate.Description = VoucherTypeReceivingDTO.Description;
            voucherTypeToUpdate.VoucherType = VoucherTypeReceivingDTO.VoucherType;
            var updatedVoucherType = await _voucherTypeRepo.UpdateFinanceVoucherType(voucherTypeToUpdate);

            summary += $"Details after change, \n {updatedVoucherType.ToString()} \n";

            if (updatedVoucherType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "FinanceVoucherType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedVoucherType.Id
            };

            await _historyRepo.SaveHistory(history);

            var voucherTypeTransferDTOs = _mapper.Map<FinancialVoucherTypeTransferDTO>(updatedVoucherType);
            return new ApiOkResponse(voucherTypeTransferDTOs);

        }

        public async Task<ApiResponse> DeleteFinancialVoucherType(long id)
        {
            var voucherTypeToDelete = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _voucherTypeRepo.DeleteFinanceVoucherType(voucherTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}