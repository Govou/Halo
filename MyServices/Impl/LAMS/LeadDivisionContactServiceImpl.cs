using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadDivisionContactServiceImpl : ILeadDivisionContactService
    {
        private readonly DataContext _context;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadDivisionContactRepository _LeadDivisionContactRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<LeadContactServiceImpl> _logger;

        public LeadDivisionContactServiceImpl(IModificationHistoryRepository historyRepo, 
            IMapper mapper, 
            ILeadDivisionContactRepository LeadDivisionContactRepo,
            DataContext context,
            ILogger<LeadContactServiceImpl> logger)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._LeadDivisionContactRepo = LeadDivisionContactRepo;
            this._context = context;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadDivisionContact(HttpContext context, long leadDivisionId, LeadDivisionContactReceivingDTO LeadDivisionContactReceivingDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var leadDivision = await _context.LeadDivisions.FirstOrDefaultAsync(ld => ld.Id == leadDivisionId && ld.IsDeleted == false);
                    if (leadDivision == null)
                    {
                        return new ApiResponse(404);
                    }

                    var LeadDivisionContact = _mapper.Map<LeadDivisionContact>(LeadDivisionContactReceivingDTO);

                    LeadDivisionContact.CreatedById = context.GetLoggedInUserId();
                    var contactEntity = await _context.LeadDivisionContacts.AddAsync(LeadDivisionContact);
                    await _context.SaveChangesAsync();
                    var savedLeadDivisionContact = contactEntity.Entity;

                    if (LeadDivisionContactReceivingDTO.Type == ContactType.Primary)
                    {
                        leadDivision.PrimaryContactId = savedLeadDivisionContact.Id;
                    }
                    else
                    {
                        leadDivision.SecondaryContactId = savedLeadDivisionContact.Id;
                    }
                    _context.LeadDivisions.Update(leadDivision);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var LeadDivisionContactTransferDTO = _mapper.Map<LeadDivisionContactTransferDTO>(savedLeadDivisionContact);
                    return new ApiOkResponse(LeadDivisionContactTransferDTO);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return new ApiResponse(500);
                }
            }         
        }

        public async Task<ApiResponse> GetAllLeadDivisionContact()
        {
            var LeadDivisionContacts = await _LeadDivisionContactRepo.FindAllLeadDivisionContact();
            if (LeadDivisionContacts == null)
            {
                return new ApiResponse(404);
            }
            var LeadDivisionContactTransferDTO = _mapper.Map<IEnumerable<LeadDivisionContactTransferDTO>>(LeadDivisionContacts);
            return new ApiOkResponse(LeadDivisionContactTransferDTO);
        }

        public async Task<ApiResponse> GetLeadDivisionContactById(long id)
        {
            var LeadDivisionContact = await _LeadDivisionContactRepo.FindLeadDivisionContactById(id);
            if (LeadDivisionContact == null)
            {
                return new ApiResponse(404);
            }
            var LeadDivisionContactTransferDTO = _mapper.Map<LeadDivisionContactTransferDTO>(LeadDivisionContact);
            return new ApiOkResponse(LeadDivisionContactTransferDTO);
        }



        public async Task<ApiResponse> UpdateLeadDivisionContact(HttpContext context, long id, LeadDivisionContactReceivingDTO LeadDivisionContactReceivingDTO)
        {
            var LeadDivisionContactToUpdate = await _LeadDivisionContactRepo.FindLeadDivisionContactById(id);
            if (LeadDivisionContactToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {LeadDivisionContactToUpdate.ToString()} \n";

            LeadDivisionContactToUpdate.FirstName = LeadDivisionContactReceivingDTO.FirstName;
            LeadDivisionContactToUpdate.LastName = LeadDivisionContactReceivingDTO.LastName;
            LeadDivisionContactToUpdate.DateOfBirth = LeadDivisionContactReceivingDTO.DateOfBirth;
            LeadDivisionContactToUpdate.Designation = LeadDivisionContactReceivingDTO.Designation;
            LeadDivisionContactToUpdate.Email = LeadDivisionContactReceivingDTO.Email;
            LeadDivisionContactToUpdate.MobileNumber = LeadDivisionContactReceivingDTO.MobileNumber;

            var updatedLeadDivisionContact = await _LeadDivisionContactRepo.UpdateLeadDivisionContact(LeadDivisionContactToUpdate);

            summary += $"Details after change, \n {updatedLeadDivisionContact.ToString()} \n";

            if (updatedLeadDivisionContact == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "LeadDivisionContact",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadDivisionContact.Id
            };

            await _historyRepo.SaveHistory(history);

            var LeadDivisionContactTransferDTOs = _mapper.Map<LeadDivisionContactTransferDTO>(updatedLeadDivisionContact);
            return new ApiOkResponse(LeadDivisionContactTransferDTOs);

        }

    }
}
