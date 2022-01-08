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

namespace HaloBiz.MyServices.Impl
{
    public class BankServiceImpl : IBankService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IBankRepository _bankRepo;
        private readonly IMapper _mapper;

        public BankServiceImpl(IModificationHistoryRepository historyRepo, IBankRepository BankRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._bankRepo = BankRepo;
        }

        public async Task<ApiCommonResponse> AddBank(HttpContext context, BankReceivingDTO bankReceivingDTO)
        {
            var bank = _mapper.Map<Bank>(bankReceivingDTO);
            bank.CreatedById = context.GetLoggedInUserId();
            var savedBank = await _bankRepo.SaveBank(bank);
            if (savedBank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var bankTransferDTO = _mapper.Map<BankTransferDTO>(bank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,bankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllBank()
        {
            var banks = await _bankRepo.FindAllBank();
            if (banks == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var bankTransferDTO = _mapper.Map<IEnumerable<BankTransferDTO>>(banks);
            return CommonResponse.Send(ResponseCodes.SUCCESS,bankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetBankById(long id)
        {
            var bank = await _bankRepo.FindBankById(id);
            if (bank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var bankTransferDTOs = _mapper.Map<BankTransferDTO>(bank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,bankTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetBankByName(string name)
        {
            var bank = await _bankRepo.FindBankByName(name);
            if (bank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var bankTransferDTOs = _mapper.Map<BankTransferDTO>(bank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,bankTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateBank(HttpContext context, long id, BankReceivingDTO bankReceivingDTO)
        {
            var bankToUpdate = await _bankRepo.FindBankById(id);
            if (bankToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {bankToUpdate.ToString()} \n" ;

            bankToUpdate.Name = bankReceivingDTO.Name;
            bankToUpdate.Alias = bankReceivingDTO.Alias;
            bankToUpdate.Slogan = bankReceivingDTO.Slogan;
            bankToUpdate.IsActive = bankReceivingDTO.IsActive;
            var updatedBank = await _bankRepo.UpdateBank(bankToUpdate);

            summary += $"Details after change, \n {updatedBank.ToString()} \n";

            if (updatedBank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Bank",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedBank.Id
            };

            await _historyRepo.SaveHistory(history);

            var bankTransferDTO = _mapper.Map<BankTransferDTO>(updatedBank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,bankTransferDTO);

        }

        public async Task<ApiCommonResponse> DeleteBank(long id)
        {
            var bankToDelete = await _bankRepo.FindBankById(id);
            if (bankToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _bankRepo.DeleteBank(bankToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}