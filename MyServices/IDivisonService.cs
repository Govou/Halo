using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IDivisonService
    {
        Task<ApiResponse> AddDivision(DivisionReceivingDTO divisionReceivingDTO);
        Task<ApiResponse> GetDivisionnById(long id);
        Task<ApiResponse> GetDivisionByName(string name);
        Task<ApiResponse> GetAllDivisions();
        Task<ApiResponse> UpdateDivision(long id, DivisionReceivingDTO branchReceivingDTO);
        Task<ApiResponse> DeleteDivision(long id);
        Task<ApiResponse> GetAllDivisionsAndSbu();
    }
}
