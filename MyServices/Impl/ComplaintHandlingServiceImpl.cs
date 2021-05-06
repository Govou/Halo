using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models.Complaints;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintHandlingServiceImpl : IComplaintHandlingService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintHandlingServiceImpl> _logger;
        private readonly IMapper _mapper;
        public ComplaintHandlingServiceImpl(HalobizContext context, ILogger<ComplaintHandlingServiceImpl> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse> GetComplaintHandlingStats(HttpContext context, long userProfileID)
        {
            try
            {
                List<EscalationMatrixUserProfile> allEscalationMatrixTiedToUser = await _context.EscalationMatrixUserProfiles
                    .Include(x => x.EscalationMatrix)
                    .Where(x => x.UserProfileId == userProfileID && x.EscalationMatrix.IsDeleted == false)
                    .ToListAsync();

                List<Complaint> allCoplaintsAssigned = await _context.Complaints
                    .Where(x => allEscalationMatrixTiedToUser
                                .Select(y => y.EscalationMatrix.ComplaintTypeId)
                                .Contains(x.ComplaintTypeId) && x.IsDeleted == false)
                    .ToListAsync();

                var resultObject = new ComplaintHandlingStatsDTO()
                {
                    TotalComplaintsAssigned = allCoplaintsAssigned.Count(),
                    TotalComplaintsClosed = allCoplaintsAssigned.Where(x => x.IsClosed != null).ToList().Count(),
                    TotalComplaintsBeingHandled = allCoplaintsAssigned.Where(x => (x.IsAssesed != null || x.IsInvestigated != null || x.IsResolved != null) && x.IsClosed == null).ToList().Count(),
                    TotalComplaintsInWorkbench = allCoplaintsAssigned.Count()
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }

        public async Task<ApiResponse> GetComplaintsHandling(HttpContext context, long userProfileID)
        {
            try
            {
                List<EscalationMatrixUserProfile> allEscalationMatrixTiedToUser = await _context.EscalationMatrixUserProfiles
                    .Include(x => x.EscalationMatrix)
                    .Where(x => x.UserProfileId == userProfileID && x.EscalationMatrix.IsDeleted == false)
                    .ToListAsync();

                List<Complaint> allComplaintsAssigned = await _context.Complaints
                    .Include(x => x.ComplaintOrigin)
                    .Include(x => x.ComplaintType)
                    .Include(x => x.ComplaintSource)
                    .Where(x => allEscalationMatrixTiedToUser
                                .Select(y => y.EscalationMatrix.ComplaintTypeId)
                                .Contains(x.ComplaintTypeId) && x.IsDeleted == false)
                    .ToListAsync();
                var complaintTransferDTOs = _mapper.Map<IEnumerable<ComplaintTransferDTO>>(allComplaintsAssigned);

                foreach (var complaint in complaintTransferDTOs)
                {
                    switch (complaint.ComplaintOrigin.Caption.ToLower())
                    {
                        case "supplier":
                            complaint.Complainant = await _context.Suppliers.FindAsync(complaint.ComplainantId);
                            break;
                        case "staff":
                            complaint.Complainant = await _context.UserProfiles.FindAsync(complaint.ComplainantId);
                            break;
                        case "client":
                            complaint.Complainant = await _context.CustomerDivisions.FindAsync(complaint.ComplainantId);
                            break;
                    }
                }

                var resultObject = new ComplaintHandlingDTO
                {
                    TotalComplaintsAssigned = complaintTransferDTOs.Count(),
                    TotalComplaintsClosed = complaintTransferDTOs.Where(x => x.IsClosed != null).ToList().Count(),
                    TotalComplaintsBeingHandled = complaintTransferDTOs.Where(x => (x.IsAssesed != null || x.IsInvestigated != null || x.IsResolved != null) && x.IsClosed == null).ToList().Count(),
                    TotalComplaintsInWorkbench = complaintTransferDTOs.Count(),
                    assignedComplaints = complaintTransferDTOs.ToList(),
                    workbenchComplaints = new List<ComplaintTransferDTO>()
                };
                return new ApiOkResponse(resultObject);
            }
            catch(Exception error)
            {
                return new ApiResponse(500, error.Message);
            }
        }
    }
}
