using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IMasterServiceAssignmentService
    {
        Task<ApiResponse> AddMasterServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO);
        Task<ApiResponse> GetAllMasterServiceAssignments();
        Task<ApiResponse> GetMasterServiceAssignmentById(long id);
        Task<ApiResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO);
        Task<ApiResponse> DeleteMasterServiceAssignment(long id);
        Task<ApiResponse> UpdateReadyStatus(long id);
    }
}
