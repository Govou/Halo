using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.TransferDTOs;

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

        public async Task<ApiCommonResponse> AddStrategicBusinessUnit(StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO)
        {
            var strategicBusinessUnit = _mapper.Map<StrategicBusinessUnit>(strategicBusinessUnitReceivingDTO);
            var savedStrategicBusinessUnit = await _strategicBusinessUnitRepo.SaveStrategyBusinessUnit(strategicBusinessUnit);
            if (savedStrategicBusinessUnit == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllStrategicBusinessUnit()
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindAllStrategyBusinessUnits();
            if (strategicBusinessUnit == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<IEnumerable<StrategicBusinessUnitTransferDTO>>(strategicBusinessUnit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetRMSbus()
        {
            var strategicBusinessUnits = await _strategicBusinessUnitRepo.GetRMSbus();
            if (strategicBusinessUnits == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnits);
        }

        public async Task<ApiCommonResponse> GetRMSbusWithClientsInfo()
        {
            var strategicBusinessUnits = await _strategicBusinessUnitRepo.GetRMSbusWithClientsInfo();
            if (strategicBusinessUnits == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnits);
        }

        public async Task<ApiCommonResponse> GetStrategicBusinessUnitById(long id)
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnit == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetStrategicBusinessUnitByName(string name)
        {
            var strategicBusinessUnit = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitByName(name);
            if (strategicBusinessUnit == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(strategicBusinessUnit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnitTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateStrategicBusinessUnit(long id, StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO)
        {
            var strategicBusinessUnitToUpdate = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnitToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            strategicBusinessUnitToUpdate.Name = strategicBusinessUnitReceivingDTO.Name;
            strategicBusinessUnitToUpdate.Description = strategicBusinessUnitReceivingDTO.Description;
            strategicBusinessUnitToUpdate.Alias = strategicBusinessUnitReceivingDTO.Alias;
            strategicBusinessUnitToUpdate.OperatingEntityId = strategicBusinessUnitReceivingDTO.OperatingEntityId;

            var updatedStrategicBusinessUnit = await _strategicBusinessUnitRepo.UpdateStrategyBusinessUnit(strategicBusinessUnitToUpdate);

            if (updatedStrategicBusinessUnit == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var strategicBusinessUnitTransferDTOs = _mapper.Map<StrategicBusinessUnitTransferDTO>(updatedStrategicBusinessUnit);
            return CommonResponse.Send(ResponseCodes.SUCCESS,strategicBusinessUnitTransferDTOs);


        }

        public async Task<ApiCommonResponse> DeleteStrategicBusinessUnit(long id)
        {
            var strategicBusinessUnitToDelete = await _strategicBusinessUnitRepo.FindStrategyBusinessUnitById(id);
            if (strategicBusinessUnitToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _strategicBusinessUnitRepo.DeleteStrategyBusinessUnit(strategicBusinessUnitToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

    }
}
