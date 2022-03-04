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
        Task<ApiCommonResponse> AddServiceReg(HttpContext context, ServiceRegistrationReceivingDTO serviceRegReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceRegs();
        Task<ApiCommonResponse> GetServiceRegById(long id);
        Task<ApiCommonResponse> UpdateServiceReg(HttpContext context, long id, ServiceRegistrationReceivingDTO serviceRegReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceReg(long id);

      
        //
        Task<ApiCommonResponse> AddResourceRequired(HttpContext context, AllResourceTypesPerServiceRegReceivingDTO ResouredRequiredReceivingDTO);
        //ArmedEscort
        Task<ApiCommonResponse> GetAllArmedEscortResourceRequired();
        Task<ApiCommonResponse> GetArmedEscortResourceById(long id);
        Task<ApiCommonResponse> DeleteArmedEscortResource(long id);

        //Pilot
        Task<ApiCommonResponse> GetAllPilotResourceRequired();
        Task<ApiCommonResponse> GetPilotResourceById(long id);
        Task<ApiCommonResponse> DeletePilotResource(long id);
        //Commander
        Task<ApiCommonResponse> GetAllCommanderResourceRequired();
        Task<ApiCommonResponse> GetCommanderResourceById(long id);
        Task<ApiCommonResponse> DeleteCommanderResource(long id);
        //Vehicle
        Task<ApiCommonResponse> GetAllVehicleResourceRequired();
        Task<ApiCommonResponse> GetVehicleResourceById(long id);
        Task<ApiCommonResponse> DeleteVehicleResource(long id);
        //Task<ApiCommonResponse> AddUpArmedEscortType(HttpContext context, long id, AEscortTypeRegReceivingDTO armedEscortTypeReceivingDTO);
        //Task<ApiCommonResponse> AddUpAllTypes(HttpContext context, long id, AllResourceTypesPerServiceRegReceivingDTO allApplicableTypesRegReceivingDTO);
        //Task<ApiCommonResponse> DeleteServiceReg(long id);
    }
}
