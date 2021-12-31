using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class StatesServiceImpl : IStatesService
    {
        private readonly ILogger<StatesServiceImpl> _logger;
        private readonly IStateRepository _stateRepo;
        private readonly IMapper _mapper;
        public StatesServiceImpl(IStateRepository stateRepo, ILogger<StatesServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._stateRepo = stateRepo;
            this._logger = logger;
        }


        public async Task<ApiCommonResponse> GetStateById(long id)
        {
            var state = await _stateRepo.FindStateById(id);
            if (state == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var stateTransferDto = _mapper.Map<StateTransferDTO>(state);
            return new ApiOkResponse(stateTransferDto);

        }

        public async Task<ApiCommonResponse> GetStateByName(string name)
        {
            var state = await _stateRepo.FindStateByName(name);
            if (state == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var stateTransferDto = _mapper.Map<StateTransferDTO>(state);
            return new ApiOkResponse(stateTransferDto);
        }

        public async Task<ApiCommonResponse> GetAllStates()
        {
            var states = await _stateRepo.FindAllStates();
            if (states == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var statesTransferDto = _mapper.Map<IEnumerable<StateTransferDTO>>(states);
            return new ApiOkResponse(statesTransferDto);
        }

        public async Task<ApiCommonResponse> GetAllLgas()
        {
            var lgas = await _stateRepo.FindAllLgas();
            if(lgas == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var lgasTransferDto = _mapper.Map<IEnumerable<LgasTransferDTO>>(lgas);
            return new ApiOkResponse(lgasTransferDto);
        }
    }
}