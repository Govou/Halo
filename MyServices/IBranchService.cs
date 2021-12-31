using HaloBiz.DTOs.ApiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices
{
    public interface IBranchService
    {
        Task<ApiResponse> AddBranch(BranchReceivingDTO branchReceivingDTO);
        Task<ApiResponse> GetBranchById(long id);
        Task<ApiResponse> GetBranchByName(string name);
        Task<ApiResponse> GetAllBranches();
        Task<ApiResponse> UpdateBranch(long id, BranchReceivingDTO branchReceivingDTO);
        Task<ApiResponse> DeleteBranch(long id);
    }
}
