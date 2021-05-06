using HaloBiz.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IComplaintHandlingRepository
    {
        Task<ComplaintHandlingStatsDTO> GetComplaintHandlingStats(long userProfileID);
        Task<ComplaintHandlingDTO> GetComplaintsHandling(long userProfileID);
    }
}
