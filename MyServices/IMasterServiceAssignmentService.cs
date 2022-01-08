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
        Task<ApiCommonResponse> AddMasterServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO);
        Task<ApiCommonResponse> GetAllMasterServiceAssignments();
        Task<ApiCommonResponse> GetMasterServiceAssignmentById(long id);
        Task<ApiCommonResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO);
        Task<ApiCommonResponse> DeleteMasterServiceAssignment(long id);
        Task<ApiCommonResponse> UpdateReadyStatus(long id);
    }
}
