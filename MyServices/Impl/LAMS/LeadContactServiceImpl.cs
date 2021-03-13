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
    public class LeadContactServiceImpl : ILeadContactService
    {
        private readonly DataContext _context;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadContactRepository _leadContactRepo;
        private readonly ILogger<LeadContactServiceImpl> _logger;
        private readonly IMapper _mapper;

        public LeadContactServiceImpl(
                                        DataContext context, 
                                        IModificationHistoryRepository historyRepo,  
                                        IMapper mapper, 
                                        ILeadContactRepository leadContactRepo,
                                        ILogger<LeadContactServiceImpl> logger
                                        )
        {
            this._mapper = mapper;
            this._context = context;
            this._historyRepo = historyRepo;
            this._leadContactRepo = leadContactRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLeadContact(HttpContext context, long leadId,  LeadContactReceivingDTO leadContactReceivingDTO)
        {
            

            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    //Get lead associated with the id
                    var lead = await _context.Leads.FirstOrDefaultAsync(lead => lead.Id == leadId);
                    if(lead == null)
                    {
                        return new ApiResponse(404);
                    }

                    //Map Dto to LeadContact, add the logged in user info and save the LeadContact
                    var leadContact = _mapper.Map<LeadContact>(leadContactReceivingDTO);

                    leadContact.CreatedById = context.GetLoggedInUserId();
                    var contactEntity = await _context.LeadContacts.AddAsync(leadContact);
                    await _context.SaveChangesAsync();
                    var savedLeadContact = contactEntity.Entity;

                    //Assign Attach the lead contact to lead based on the contact Contact and update lead
                    if(leadContactReceivingDTO.Type == ContactType.Primary)
                    {
                        lead.PrimaryContactId = savedLeadContact.Id;
                    }else{
                        lead.SecondaryContactId = savedLeadContact.Id;
                    }
                    _context.Leads.Update(lead);
                    await _context.SaveChangesAsync();
                    //Map savedContact to a dto and return it
                    await transaction.CommitAsync();
                    var leadContactTransferDTO = _mapper.Map<LeadContactTransferDTO>(savedLeadContact);
                    return new ApiOkResponse(leadContactTransferDTO);                    
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return new ApiResponse(500);
                }
            }

        }

        public async Task<ApiResponse> GetAllLeadContact()
        {
            var leadContacts = await _leadContactRepo.FindAllLeadContact();
            if (leadContacts == null)
            {
                return new ApiResponse(404);
            }
            var leadContactTransferDTO = _mapper.Map<IEnumerable<LeadContactTransferDTO>>(leadContacts);
            return new ApiOkResponse(leadContactTransferDTO);
        }

        public async Task<ApiResponse> GetLeadContactById(long id)
        {
            var leadContact = await _leadContactRepo.FindLeadContactById(id);
            if (leadContact == null)
            {
                return new ApiResponse(404);
            }
            var leadContactTransferDTO = _mapper.Map<LeadContactTransferDTO>(leadContact);
            return new ApiOkResponse(leadContactTransferDTO);
        }

        

        public async Task<ApiResponse> UpdateLeadContact(HttpContext context, long id, LeadContactReceivingDTO leadContactReceivingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    var leadContactToUpdate = await _context.LeadContacts.FirstOrDefaultAsync(contact => contact.Id == id);
                    if (leadContactToUpdate == null)
                    {
                        return new ApiResponse(404);
                    }
                    
                    var summary = $"Initial details before change, \n {leadContactToUpdate.ToString()} \n" ;

                    leadContactToUpdate.FirstName = leadContactReceivingDTO.FirstName;
                    leadContactToUpdate.LastName = leadContactReceivingDTO.LastName;
                    leadContactToUpdate.DateOfBirth = leadContactReceivingDTO.DateOfBirth;
                    leadContactToUpdate.Designation = leadContactReceivingDTO.Designation;
                    leadContactToUpdate.Email = leadContactReceivingDTO.Email;
                    leadContactToUpdate.MobileNumber = leadContactReceivingDTO.MobileNumber;
                    
                    var updatedLeadContact = _context.LeadContacts.Update(leadContactToUpdate).Entity;
                    await _context.SaveChangesAsync();

                    summary += $"Details after change, \n {updatedLeadContact.ToString()} \n";

                    ModificationHistory history = new ModificationHistory(){
                        ModelChanged = "LeadContact",
                        ChangeSummary = summary,
                        ChangedById = context.GetLoggedInUserId(),
                        ModifiedModelId = updatedLeadContact.Id
                    };

                    await _context.ModificationHistories.AddAsync(history);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var leadContactTransferDTOs = _mapper.Map<LeadContactTransferDTO>(updatedLeadContact);
                    return new ApiOkResponse(leadContactTransferDTOs);
                    
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return new ApiResponse(500);
                }
            }
        }

        public async Task<ApiResponse> DeleteLeadContact(long id)
        {
            var leadContactToDelete = await _leadContactRepo.FindLeadContactById(id);
            if (leadContactToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _leadContactRepo.DeleteLeadContact(leadContactToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}