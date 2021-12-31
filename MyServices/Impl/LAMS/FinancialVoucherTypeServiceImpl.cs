using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

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

        public FinancialVoucherTypeServiceImpl(IModificationHistoryRepository historyRepo, 
        IFinancialVoucherTypeRepository voucherTypeRepo, ILogger<FinancialVoucherTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._voucherTypeRepo = voucherTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddFinancialVoucherType(HttpContext context, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO)
        {
            var voucherType = _mapper.Map<FinanceVoucherType>(voucherTypeReceivingDTO);
            voucherType.CreatedById = context.GetLoggedInUserId();
            var savedVoucherType = await _voucherTypeRepo.SaveFinanceVoucherType(voucherType);
            if (savedVoucherType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var voucherTypeTransferDTO = _mapper.Map<FinancialVoucherTypeTransferDTO>(voucherType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,voucherTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllFinancialVoucherTypes()
        {
            var voucherTypes = await _voucherTypeRepo.FindAllFinanceVoucherType();
            if (voucherTypes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var voucherTypeTransferDTO = _mapper.Map<IEnumerable<FinancialVoucherTypeTransferDTO>>(voucherTypes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,voucherTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetFinancialVoucherTypeById(long id)
        {
            var voucherType = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var voucherTypeTransferDTO = _mapper.Map<FinancialVoucherTypeTransferDTO>(voucherType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,voucherTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateFinancialVoucherType(HttpContext context, long id, FinancialVoucherTypeReceivingDTO VoucherTypeReceivingDTO)
        {
            var voucherTypeToUpdate = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherTypeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {voucherTypeToUpdate.ToString()} \n" ;

            voucherTypeToUpdate.Description = VoucherTypeReceivingDTO.Description;
            voucherTypeToUpdate.Alias = VoucherTypeReceivingDTO.Alias;
            voucherTypeToUpdate.VoucherType = VoucherTypeReceivingDTO.VoucherType;
            var updatedVoucherType = await _voucherTypeRepo.UpdateFinanceVoucherType(voucherTypeToUpdate);

            summary += $"Details after change, \n {updatedVoucherType.ToString()} \n";

            if (updatedVoucherType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "FinanceVoucherType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedVoucherType.Id
            };

            await _historyRepo.SaveHistory(history);

            var voucherTypeTransferDTOs = _mapper.Map<FinancialVoucherTypeTransferDTO>(updatedVoucherType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,voucherTypeTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteFinancialVoucherType(long id)
        {
            var voucherTypeToDelete = await _voucherTypeRepo.FindFinanceVoucherTypeById(id);
            if (voucherTypeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _voucherTypeRepo.DeleteFinanceVoucherType(voucherTypeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        
    }
}