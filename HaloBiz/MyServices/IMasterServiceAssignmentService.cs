using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddMasterAutoServiceAssignment(HttpContext context, MasterServiceAssignmentForAutoReceivingDTO masterReceivingDTO);
        Task<ApiCommonResponse> GetAllMasterServiceAssignments();
        Task<ApiCommonResponse> GetMasterServiceAssignmentById(long id);
        Task<ApiCommonResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO);
        Task<ApiCommonResponse> DeleteMasterServiceAssignment(long id);
        Task<ApiCommonResponse> UpdateReadyStatus(long id);
        Task<ApiCommonResponse> GetAllCustomerDivisions();

        //Secondary
        Task<ApiCommonResponse> AddSecondaryServiceAssignment(HttpContext context, SecondaryServiceAssignmentReceivingDTO secondaryReceivingDTO);
        Task<ApiCommonResponse> GetAllSecondaryServiceAssignments();
        Task<ApiCommonResponse> GetsecondaryServiceAssignmentById(long id);
        Task<ApiCommonResponse> DeleteSecondaryServiceAssignment(long id);
    }
}
