using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IEscalationMatrixService
    {
        Task<ApiResponse> AddEscalationMatrix(HttpContext context, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO);
        Task<ApiResponse> GetAllEscalationMatrix();
        Task<ApiResponse> GetHandlers(long complaintTypeId);
        Task<ApiResponse> GetEscalationMatrixById(long id);
        //Task<ApiResponse> GetEscalationMatrixByName(string name);
        Task<ApiResponse> UpdateEscalationMatrix(HttpContext context, long id, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO);
        Task<ApiResponse> DeleteEscalationMatrix(long id);
    }
}
