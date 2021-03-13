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

        public async Task<ApiResponse> AddBank(HttpContext context, BankReceivingDTO bankReceivingDTO)
        {
            var bank = _mapper.Map<Bank>(bankReceivingDTO);
            bank.CreatedById = context.GetLoggedInUserId();
            var savedBank = await _bankRepo.SaveBank(bank);
            if (savedBank == null)
            {
                return new ApiResponse(500);
            }
            var bankTransferDTO = _mapper.Map<BankTransferDTO>(bank);
            return new ApiOkResponse(bankTransferDTO);
        }

        public async Task<ApiResponse> GetAllBank()
        {
            var banks = await _bankRepo.FindAllBank();
            if (banks == null)
            {
                return new ApiResponse(404);
            }
            var bankTransferDTO = _mapper.Map<IEnumerable<BankTransferDTO>>(banks);
            return new ApiOkResponse(bankTransferDTO);
        }

        public async Task<ApiResponse> GetBankById(long id)
        {
            var bank = await _bankRepo.FindBankById(id);
            if (bank == null)
            {
                return new ApiResponse(404);
            }
            var bankTransferDTOs = _mapper.Map<BankTransferDTO>(bank);
            return new ApiOkResponse(bankTransferDTOs);
        }

        public async Task<ApiResponse> GetBankByName(string name)
        {
            var bank = await _bankRepo.FindBankByName(name);
            if (bank == null)
            {
                return new ApiResponse(404);
            }
            var bankTransferDTOs = _mapper.Map<BankTransferDTO>(bank);
            return new ApiOkResponse(bankTransferDTOs);
        }

        public async Task<ApiResponse> UpdateBank(HttpContext context, long id, BankReceivingDTO bankReceivingDTO)
        {
            var bankToUpdate = await _bankRepo.FindBankById(id);
            if (bankToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Bank",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedBank.Id
            };

            await _historyRepo.SaveHistory(history);

            var bankTransferDTO = _mapper.Map<BankTransferDTO>(updatedBank);
            return new ApiOkResponse(bankTransferDTO);

        }

        public async Task<ApiResponse> DeleteBank(long id)
        {
            var bankToDelete = await _bankRepo.FindBankById(id);
            if (bankToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _bankRepo.DeleteBank(bankToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}