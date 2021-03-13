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

        public async Task<ApiResponse> DeleteContractService(long id)
        {
            var contractServiceToDelete = await _contractServiceRepo.FindContractServiceById(id);
            if (contractServiceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _contractServiceRepo.DeleteContractService(contractServiceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllContractsServcieForAContract(long contractId)
        {
            var contractServices = await _contractServiceRepo.FindAllContractServicesForAContract(contractId);
            if (contractServices == null)
            {
                return new ApiResponse(404);
            }
            var contractServiceTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractServices);
            return new ApiOkResponse(contractServiceTransferDTOs);
        }

        public async Task<ApiResponse> GetContractServiceByReferenceNumber(string refNo)
        {
            var contractServices = await _contractServiceRepo.FindContractServicesByReferenceNumber(refNo);
            if (contractServices == null)
            {
                return new ApiResponse(404);
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractServiceTransferDTO>>(contractServices);
            return new ApiOkResponse(contractTransferDTOs);
        }

        public async Task<ApiResponse> GetContractServiceById(long id)
        {
            var contractService = await _contractServiceRepo.FindContractServiceById(id);
            if (contractService == null)
            {
                return new ApiResponse(404);
            }
            var contractTransferDTOs = _mapper.Map<ContractServiceTransferDTO>(contractService);
            return new ApiOkResponse(contractTransferDTOs);
        }
    }
}