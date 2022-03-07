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

namespace HaloBiz.MyServices.Impl
{
    public class DivisionServiceImpl : IDivisonService
    {
        private readonly ILogger<DivisionServiceImpl> _logger;
        private readonly IOperatingEntityService _operatingEntityService;
        private readonly IDivisionRepository _divisionRepo;
        private readonly IMapper _mapper;

        public DivisionServiceImpl(IOperatingEntityService operatingEntityService ,IDivisionRepository divisionRepo, ILogger<DivisionServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._operatingEntityService = operatingEntityService;
            this._divisionRepo = divisionRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddDivision(DivisionReceivingDTO divisionReceivingDTO)
        {
            var division = _mapper.Map<Division>(divisionReceivingDTO);
            division.CompanyId = 1;
            var saveddivision = await _divisionRepo.SaveDivision(division);
            if (saveddivision == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var divisionTransferDTOs = _mapper.Map<DivisionTransferDTO>(division);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);
        }

        public async Task<ApiCommonResponse> DeleteDivision(long id)
        {
            var divisionToDelete = await _divisionRepo.FindDivisionById(id);
            if (divisionToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            foreach (OperatingEntity operatingEntity in divisionToDelete.OperatingEntities)
            {
                await _operatingEntityService.DeleteOperatingEntity(operatingEntity.Id);
            }

            if (!await _divisionRepo.RemoveDivision(divisionToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllDivisions()
        {
            var divisions = await _divisionRepo.FindAllDivisions();
            if (divisions == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var divisionTransferDTOs = _mapper.Map<IEnumerable<DivisionTransferDTO>>(divisions);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetAllDivisionsAndSbu()
        {
            var divisions = await _divisionRepo.GetAllDivisionAndSbu();
            if (divisions == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var divisionTransferDTOs = _mapper.Map<IEnumerable<DivisionTransferDTO>>(divisions);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetDivisionByName(string name)
        {
            var division = await _divisionRepo.FindDivisionByName(name);
            if (division == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var divisionTransferDTOs = _mapper.Map<DivisionTransferDTO>(division);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetDivisionnById(long id)
        {
            var division = await _divisionRepo.FindDivisionById(id);
            if (division == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var divisionTransferDTOs = _mapper.Map<DivisionTransferDTO>(division);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);
        }
        public async Task<ApiCommonResponse> UpdateDivision(long id, DivisionReceivingDTO divisionReceivingDTO)
        {
            var divisionToUpdate = await _divisionRepo.FindDivisionById(id);
            if (divisionToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            divisionToUpdate.Name = divisionReceivingDTO.Name;
            divisionToUpdate.Description = divisionReceivingDTO.Description;
            divisionToUpdate.MissionStatement = divisionReceivingDTO.MissionStatement;
            divisionToUpdate.HeadId = divisionReceivingDTO.HeadId;
            var updatedDivision = await _divisionRepo.UpdateDivision(divisionToUpdate);

            if (updatedDivision == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var divisionTransferDTOs = _mapper.Map<DivisionTransferDTO>(updatedDivision);
            return CommonResponse.Send(ResponseCodes.SUCCESS,divisionTransferDTOs);


        }
    }
}
