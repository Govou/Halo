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
        Task<ApiCommonResponse> AddBranch(BranchReceivingDTO branchReceivingDTO);
        Task<ApiCommonResponse> GetBranchById(long id);
        Task<ApiCommonResponse> GetBranchByName(string name);
        Task<ApiCommonResponse> GetAllBranches();
        Task<ApiCommonResponse> UpdateBranch(long id, BranchReceivingDTO branchReceivingDTO);
        Task<ApiCommonResponse> DeleteBranch(long id);
    }
}
