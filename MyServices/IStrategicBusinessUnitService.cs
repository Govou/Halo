using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IStrategicBusinessUnitService
    {
        Task<ApiCommonResponse> AddStrategicBusinessUnit(StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO);
        Task<ApiCommonResponse> GetStrategicBusinessUnitById(long id);
        Task<ApiCommonResponse> GetStrategicBusinessUnitByName(string name);
        Task<ApiCommonResponse> GetAllStrategicBusinessUnit();
        Task<ApiCommonResponse> GetRMSbusWithClientsInfo();
        Task<ApiCommonResponse> GetRMSbus();
        Task<ApiCommonResponse> UpdateStrategicBusinessUnit(long id, StrategicBusinessUnitReceivingDTO strategicBusinessUnitReceivingDTO);
        Task<ApiCommonResponse> DeleteStrategicBusinessUnit(long id);
    }
}
