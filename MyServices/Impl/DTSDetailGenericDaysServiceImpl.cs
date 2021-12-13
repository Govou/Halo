using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class DTSDetailGenericDaysServiceImpl:IDTSDetailGenericDaysService
    {

        private readonly IDTSDetailGenericDaysRepository _dTSDetailGenericDaysRepository;
        private readonly IMapper _mapper;

        public DTSDetailGenericDaysServiceImpl(IMapper mapper, IDTSDetailGenericDaysRepository dTSDetailGenericDaysRepository)
        {
            _mapper = mapper;
            _dTSDetailGenericDaysRepository = dTSDetailGenericDaysRepository;
        }

        public Task<ApiResponse> AddArmedEscortGeneric(HttpContext context, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> AddCommanderGeneric(HttpContext context, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> AddPilotGeneric(HttpContext context, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> AddVehicleGeneric(HttpContext context, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteArmedEscortGeneric(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteCommanderGeneric(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeletePilotGeneric(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteVehicleGeneric(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllArmedEscortGenerics()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllCommanderGenerics()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllPilotGenerics()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllVehicleGenerics()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetArmedEscortGenericById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCommanderGenericById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetPilotGenericById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetVehicleGenericById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateArmedEscortGeneric(HttpContext context, long id, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateCommanderGeneric(HttpContext context, long id, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdatePilotGeneric(HttpContext context, long id, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateVehicleGeneric(HttpContext context, long id, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
        {
            throw new NotImplementedException();
        }
    }
}
