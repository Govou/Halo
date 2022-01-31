using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IOperatingEntityService
    {
        Task<ApiCommonResponse> AddOperatingEntity(OperatingEntityReceivingDTO operatingEntityReceivingDTO);
        Task<ApiCommonResponse> GetOperatingEntityById(long id);
        Task<ApiCommonResponse> GetOperatingEntityByName(string name);
        Task<ApiCommonResponse> GetAllOperatingEntities();
        Task<ApiCommonResponse> UpdateOperatingEntity(long id, OperatingEntityReceivingDTO operatingEntityReceivingDTO);
        Task<ApiCommonResponse> DeleteOperatingEntity(long id);
        Task<ApiCommonResponse> GetAllOperatingEntitiesAndSbuproportion();
    }
}
