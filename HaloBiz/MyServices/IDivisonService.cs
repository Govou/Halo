using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IDivisonService
    {
        Task<ApiCommonResponse> AddDivision(DivisionReceivingDTO divisionReceivingDTO);
        Task<ApiCommonResponse> GetDivisionnById(long id);
        Task<ApiCommonResponse> GetDivisionByName(string name);
        Task<ApiCommonResponse> GetAllDivisions();
        Task<ApiCommonResponse> UpdateDivision(long id, DivisionReceivingDTO branchReceivingDTO);
        Task<ApiCommonResponse> DeleteDivision(long id);
        Task<ApiCommonResponse> GetAllDivisionsAndSbu();
    }
}
