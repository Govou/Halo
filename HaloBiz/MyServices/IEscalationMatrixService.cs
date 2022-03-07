using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddEscalationMatrix(HttpContext context, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO);
        Task<ApiCommonResponse> GetAllEscalationMatrix();
        Task<ApiCommonResponse> GetHandlers(long complaintTypeId);
        Task<ApiCommonResponse> GetEscalationMatrixById(long id);
        //Task<ApiCommonResponse> GetEscalationMatrixByName(string name);
        Task<ApiCommonResponse> UpdateEscalationMatrix(HttpContext context, long id, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO);
        Task<ApiCommonResponse> DeleteEscalationMatrix(long id);
    }
}
