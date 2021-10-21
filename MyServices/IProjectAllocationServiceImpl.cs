using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IProjectAllocationServiceImpl
    {

        Task<ApiResponse> AddNewManager(HttpContext context, ProjectAllocationRecievingDTO projectAllocationDTO);
        Task<ApiResponse> getProjectManagers(int serviceCategory);
        Task<ApiResponse> getManagersProjects(string email, int emailId);
        Task<ApiResponse> removeFromCategory(long id,int categoryId, long projectId);

    }
}
