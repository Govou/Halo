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
        Task<ApiResponse> AddOperatingEntity(OperatingEntityReceivingDTO operatingEntityReceivingDTO);
        Task<ApiResponse> GetOperatingEntityById(long id);
        Task<ApiResponse> GetOperatingEntityByName(string name);
        Task<ApiResponse> GetAllOperatingEntities();
        Task<ApiResponse> UpdateOperatingEntity(long id, OperatingEntityReceivingDTO operatingEntityReceivingDTO);
        Task<ApiResponse> DeleteOperatingEntity(long id);
        Task<ApiResponse> GetAllOperatingEntitiesAndSbuproportion();
    }
}
