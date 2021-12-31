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


        public async Task<ApiResponse> GetStateById(long id)
        {
            var state = await _stateRepo.FindStateById(id);
            if (state == null)
            {
                return new ApiResponse(404);
            }
            var stateTransferDto = _mapper.Map<StateTransferDTO>(state);
            return new ApiOkResponse(stateTransferDto);

        }

        public async Task<ApiResponse> GetStateByName(string name)
        {
            var state = await _stateRepo.FindStateByName(name);
            if (state == null)
            {
                return new ApiResponse(404);
            }
            var stateTransferDto = _mapper.Map<StateTransferDTO>(state);
            return new ApiOkResponse(stateTransferDto);
        }

        public async Task<ApiResponse> GetAllStates()
        {
            var states = await _stateRepo.FindAllStates();
            if (states == null)
            {
                return new ApiResponse(404);
            }
            var statesTransferDto = _mapper.Map<IEnumerable<StateTransferDTO>>(states);
            return new ApiOkResponse(statesTransferDto);
        }

        public async Task<ApiResponse> GetAllLgas()
        {
            var lgas = await _stateRepo.FindAllLgas();
            if(lgas == null)
            {
                return new ApiResponse(404);
            }
            var lgasTransferDto = _mapper.Map<IEnumerable<LgasTransferDTO>>(lgas);
            return new ApiOkResponse(lgasTransferDto);
        }
    }
}