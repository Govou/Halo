using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IServiceRegistrationService
    {
        Task<ApiResponse> AddServiceReg(HttpContext context, ServiceRegistrationReceivingDTO serviceRegReceivingDTO);
        Task<ApiResponse> GetAllServiceRegs();
        Task<ApiResponse> GetServiceRegById(long id);
        Task<ApiResponse> UpdateServiceReg(HttpContext context, long id, ServiceRegistrationReceivingDTO serviceRegReceivingDTO);
        Task<ApiResponse> DeleteServiceReg(long id);

      
        //
        Task<ApiResponse> AddUpVehicleType(HttpContext context, long id, VehicleTypeRegReceivingDTO vehicleTypeReceivingDTO);
        Task<ApiResponse> AddUpPilotType(HttpContext context, long id, PilotTypeRegReceivingDTO pilotTypeReceivingDTO);
        Task<ApiResponse> AddUpCommanderType(HttpContext context, long id, CommanderTypeRegReceivingDTO commanderTypeReceivingDTO);
        //Task<ApiResponse> AddUpArmedEscortType(HttpContext context, long id, AEscortTypeRegReceivingDTO armedEscortTypeReceivingDTO);
        //Task<ApiResponse> AddUpAllTypes(HttpContext context, long id, AllResourceTypesPerServiceRegReceivingDTO allApplicableTypesRegReceivingDTO);
        //Task<ApiResponse> DeleteServiceReg(long id);
    }
}
