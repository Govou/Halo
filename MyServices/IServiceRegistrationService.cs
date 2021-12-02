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
        Task<ApiResponse> AddResourceRequired(HttpContext context, AllResourceTypesPerServiceRegReceivingDTO ResouredRequiredReceivingDTO);
        //ArmedEscort
        Task<ApiResponse> GetAllArmedEscortResourceRequired();
        Task<ApiResponse> GetArmedEscortResourceById(long id);
        Task<ApiResponse> DeleteArmedEscortResource(long id);

        //Pilot
        Task<ApiResponse> GetAllPilotResourceRequired();
        Task<ApiResponse> GetPilotResourceById(long id);
        Task<ApiResponse> DeletePilotResource(long id);
        //Commander
        Task<ApiResponse> GetAllCommanderResourceRequired();
        Task<ApiResponse> GetCommanderResourceById(long id);
        Task<ApiResponse> DeleteCommanderResource(long id);
        //Vehicle
        Task<ApiResponse> GetAllVehicleResourceRequired();
        Task<ApiResponse> GetVehicleResourceById(long id);
        Task<ApiResponse> DeleteVehicleResource(long id);
        //Task<ApiResponse> AddUpArmedEscortType(HttpContext context, long id, AEscortTypeRegReceivingDTO armedEscortTypeReceivingDTO);
        //Task<ApiResponse> AddUpAllTypes(HttpContext context, long id, AllResourceTypesPerServiceRegReceivingDTO allApplicableTypesRegReceivingDTO);
        //Task<ApiResponse> DeleteServiceReg(long id);
    }
}
