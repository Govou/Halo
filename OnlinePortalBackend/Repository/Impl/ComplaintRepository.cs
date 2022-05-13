using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models.Complaints;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EndorsementRepositoryImpl> _logger;
        private readonly IMapper _mapper;

        public ComplaintRepository(HalobizContext context, IMapper mapper, ILogger<EndorsementRepositoryImpl> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }



        public async Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaintReceivingDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var complaintOriginId = _context.ComplaintOrigins.FirstOrDefault(x => x.Caption.ToLower().Contains("client")).Id;
            var complaintSourceId = _context.ComplaintSources.FirstOrDefault(x => x.Caption.ToLower().Contains("web")).Id;
            try
            {
                var loggedInUserId = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

                var complaint = new Complaint
                {
                    ComplaintSourceId = complaintSourceId,
                    ComplainantId = complaintReceivingDTO.ComplainantId,
                    ComplaintDescription = complaintReceivingDTO.ComplaintDescription,
                    ComplaintOriginId = complaintOriginId,
                    ComplaintTypeId = complaintReceivingDTO.ComplaintTypeId,
                    DateCreated = DateTime.Now
                };
                var evidences = new List<Evidence>();

                foreach (var item in complaintReceivingDTO.Evidences)
                {
                    evidences.Add(new Evidence
                    {
                        ComplaintStage = item.ComplaintStage,
                        ComplaintId = (long)item.ComplaintId,
                        Caption = item.Caption,
                        ImageUrl = item.ImageUrl
                    });
                }

                complaint.Evidences = evidences;

                complaint.CreatedById = loggedInUserId;
                complaint.RegisteredById = loggedInUserId;
                complaint.DateComplaintReported = complaintReceivingDTO.DateCreated;
                complaint.DateCreated = DateTime.Now;
                complaint.DateRegistered = DateTime.Now;

                complaint.IsRegistered = true;
                foreach (var evidence in complaint.Evidences)
                {
                    evidence.CreatedById = loggedInUserId;
                    evidence.CreatedAt = DateTime.Now;
                    evidence.EvidenceCaptureById = loggedInUserId;
                    evidence.DateCaptured = DateTime.Now;
                }
                await _context.Complaints.AddAsync(complaint);
                await _context.SaveChangesAsync();
                var savedcomplaint = complaint;
                if (savedcomplaint == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                var complaintType = await _context.ComplaintTypes.FindAsync(savedcomplaint.ComplaintTypeId);
                var complaintOrigin = await _context.ComplaintOrigins.FindAsync(savedcomplaint.ComplaintOriginId);

                savedcomplaint.TrackingId = $"#COMPL{savedcomplaint.Id}-{complaintType.Code}-{complaintOrigin.Code}";
                _context.Complaints.Update(savedcomplaint);
                await _context.SaveChangesAsync();

                if (complaintReceivingDTO.Evidences.Count > 0)
                {
                    foreach (var evidence in evidences)
                    {
                        evidence.ComplaintId = savedcomplaint.Id;
                        evidence.CreatedById = loggedInUserId;
                        evidence.EvidenceCaptureById = loggedInUserId;
                        evidence.Id = 0;
                    }

                    await _context.Evidences.AddRangeAsync(evidences);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

               // var complaintTransferDTO = _mapper.Map<ComplaintDTO>(complaint);
                return CommonResponse.Send(ResponseCodes.SUCCESS, "Success");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<IEnumerable<ComplaintTypeDTO>> GetComplainTypes()
        {
           return _context.ComplaintTypes.Select(t => new ComplaintTypeDTO
            {
                Caption = t.Caption,
                Description = t.Description,
                Id = (int)t.Id
            });
        }

        public async Task<int> GetComplaintOrigin()
        {
            var result =  _context.ComplaintOrigins.FirstOrDefault(x => x.Caption.ToLower() == "client").Id;
            return (int)result;
        }

        public async Task<int> GetComplaintSource()
        {
            var result = _context.ComplaintSources.FirstOrDefault(x => x.Caption.ToLower().Contains("web")).Id;
            return (int)result;
        }

        public async Task<IEnumerable<ComplaintItemDTO>> GetAllComplaints(int userId)
        {
            var originId = _context.ComplaintOrigins.FirstOrDefault(x => x.Caption.ToLower().Equals("client")).Id;
            return _context.Complaints.Where(x => x.ComplainantId == userId && x.ComplaintOriginId == originId).Select(x => new ComplaintItemDTO
            {
                DateCreated = x.DateCreated,
                Description = x.ComplaintDescription,
                Id = (int)x.Id,
                TrackingId = x.TrackingId,
                IsResolved = x.IsResolved
            });
        }
    }
}
