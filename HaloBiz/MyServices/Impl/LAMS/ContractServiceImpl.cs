using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> DeleteContract(long id)
        {
            var contractToDelete = await _contractRepo.FindContractById(id);
            if (contractToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _contractRepo.DeleteContract(contractToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllContracts()
        {
            var contracts = await _contractRepo.FindAllContract();
            if (contracts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractTransferDTO>>(contracts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractByReferenceNumber(string refNo)
        {
            var contract = await _contractRepo.FindContractByReferenceNumber(refNo);
            if (contract == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<ContractTransferDTO>(contract);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractById(long id)
        {
            var contract = await _contractRepo.FindContractById(id);
            if (contract == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<ContractTransferDTO>(contract);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractsByLeadId(long leadId)
        {
            var contracts = await _contractRepo.FindContractsByLeadId(leadId);
            if (contracts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractTransferDTO>>(contracts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractsByCustomerId(long customerId)
        {
            var contracts = await _contractRepo.FindContractsByCustomerId(customerId);
            if (contracts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractTransferDTO>>(contracts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }
    }
}