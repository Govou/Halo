using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace HaloBiz.MyServices.Impl.LAMS
{
    public class ContractServiceServiceImpl : IContractServiceService
    {
        private readonly ILogger<ContractServiceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IContractServiceRepository _contractServiceRepo;
        private readonly IMapper _mapper;

        public ContractServiceServiceImpl(IModificationHistoryRepository historyRepo, IContractServiceRepository contractServiceRepo, ILogger<ContractServiceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._contractServiceRepo = contractServiceRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> DeleteContractService(long id)
        {
            var contractServiceToDelete = await _contractServiceRepo.FindContractServiceById(id);
            if (contractServiceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _contractServiceRepo.DeleteContractService(contractServiceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllContractsServcieForAContract(long contractId)
        {
            var contractService = await _contractServiceRepo.FindAllContractServicesForAContract(contractId);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractServiceTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractServiceTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractServiceByTag(string tag)
        {
            var contractService = await _contractServiceRepo.FindContractServiceByTag(tag);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<ContractServiceForContractTransferDTO>(contractService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetContractServiceByReferenceNumber(string refNo)
        {
            var contractService = await _contractServiceRepo.FindContractServicesByReferenceNumber(refNo);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractServiceByGroupInvoiceNumber(string refNo)
        {
            var contractService = await _contractServiceRepo.FindContractServicesByGroupInvoiceNumber(refNo);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetContractServiceById(long id)
        {
            var contractService = await _contractServiceRepo.FindContractServiceById(id);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var contractTransferDTOs = _mapper.Map<ContractServiceTransferDTO>(contractService);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractTransferDTOs);
        }

        public async Task<ApiResponse> GetAllContractsServcie()
        {
            var contractService = await _contractServiceRepo.FindAllContractServices();
            if (contractService == null)
            {
                return new ApiResponse(404);
            }
            var contractServiceTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractService);
            return new ApiOkResponse(contractServiceTransferDTOs);
        }
    }
}