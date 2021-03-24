using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class StrategicBusinessUnitServiceImpl : IStrategicBusinessUnitService
    {
        private readonly ILogger<StrategicBusinessUnitServiceImpl> _logger;
        private readonly IStrategicBusinessUnitRepository _strategicBusinessUnitRepo;
        private readonly IMapper _mapper;

        public StrategicBusinessUnitServiceImpl(IStrategicBusinessUnitRepository strategicBusinessUnitRepo, ILogger<StrategicBusinessUnitServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._strategicBusinessUnitRepo = strategicBusinessUnitRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddStrategicBusinessUnit(StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO)
        {
            var strategicBusinessUnit = _mapper.Map<StrategicBusinessUnit>(strategicBusinessUnitReceivingDTO);
            var savedStrategicBusinessUnit = await _strategicBusinessUnitRepo.SaveStrategyBusinessUnit(strategicBusinessUnit);
            if (savedStrategicBusinessUnit == null)
            {
                return new ApiResponse(500);
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return new ApiOkResponse(strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiResponse> GetAllStrategicBusinessUnit()
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindAllStrategyBusinessUnits();
            if (strategicBusinessUnit == null)
            {
                return new ApiResponse(404);
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<IEnumerable<StrategicBusinessUnitTransferDTO>>(strategicBusinessUnit);
            return new ApiOkResponse(strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiResponse> GetStrategicBusinessUnitById(long id)
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnit == null)
            {
                return new ApiResponse(404);
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return new ApiOkResponse(strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiResponse> GetStrategicBusinessUnitByName(string name)
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitByName(name);
            if (strategicBusinessUnit == null)
            {
                return new ApiResponse(404);
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return new ApiOkResponse(strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiResponse> UpdateStrategicBusinessUnit(long id, StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO)
        {
            var strategicBusinessUnitToUpdate = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnitToUpdate == null)
            {
                return new ApiResponse(404);
            }
            strategicBusinessUnitToUpdate.Name = strategicBusinessUnitReceivingDTO.Name;
            strategicBusinessUnitToUpdate.Description = strategicBusinessUnitReceivingDTO.Description;
            strategicBusinessUnitToUpdate.Alias = strategicBusinessUnitReceivingDTO.Alias;
            strategicBusinessUnitToUpdate.OperatingEntityId = strategicBusinessUnitReceivingDTO.OperatingEntityId;

            var updatedStrategicBusinessUnit = await _strategicBusinessUnitRepo.UpdateStrategyBusinessUnit(strategicBusinessUnitToUpdate);

            if (updatedStrategicBusinessUnit == null)
            {
                return new ApiResponse(500);
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(updatedStrategicBusinessUnit);
            return new ApiOkResponse(strategicBusinessUnitTransferDTOs);


        }

        public async Task<ApiResponse> DeleteStrategicBusinessUnit(long id)
        {
            var strategicBusinessUnitToDelete = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnitToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _strategicBusinessUnitRepo.DeleteStrategyBusinessUnit(strategicBusinessUnitToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}
