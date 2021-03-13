using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class ContractServiceImpl : IContractService
    {
        private readonly ILogger<ContractServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IContractRepository _contractRepo;
        private readonly IMapper _mapper;

        public ContractServiceImpl(IModificationHistoryRepository historyRepo, IContractRepository ContractRepo, ILogger<ContractServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._contractRepo = ContractRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> DeleteContract(long id)
        {
            var contractToDelete = await _contractRepo.FindContractById(id);
            if (contractToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _contractRepo.DeleteContract(contractToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllContracts()
        {
            var contracts = await _contractRepo.FindAllContract();
            if (contracts == null)
            {
                return new ApiResponse(404);
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractTransferDTO>>(contracts);
            return new ApiOkResponse(contractTransferDTOs);
        }

        public async Task<ApiResponse> GetContractByReferenceNumber(string refNo)
        {
            var contract = await _contractRepo.FindContractByReferenceNumber(refNo);
            if (contract == null)
            {
                return new ApiResponse(404);
            }
            var contractTransferDTOs = _mapper.Map<ContractTransferDTO>(contract);
            return new ApiOkResponse(contractTransferDTOs);
        }

        public async Task<ApiResponse> GetContractById(long id)
        {
            var contract = await _contractRepo.FindContractById(id);
            if (contract == null)
            {
                return new ApiResponse(404);
            }
            var contractTransferDTOs = _mapper.Map<ContractTransferDTO>(contract);
            return new ApiOkResponse(contractTransferDTOs);
        }
    }
}