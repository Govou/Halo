using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HaloBiz.Model.LAMS;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadContactServiceImpl : ILeadContactService
    {
        private readonly HalobizContext _context;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadContactRepository _leadContactRepo;
        private readonly ILogger<LeadContactServiceImpl> _logger;
        private readonly IMapper _mapper;

        public LeadContactServiceImpl(
                                        HalobizContext context, 
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

        public async Task<ApiCommonResponse> AddLeadContact(HttpContext context, long leadId,  LeadContactReceivingDTO leadContactReceivingDTO)
        {
            

            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    //Get lead associated with the id
                    var lead = await _context.Leads.FirstOrDefaultAsync(lead => lead.Id == leadId);
                    if(lead == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }

                    //Map Dto to LeadContact, add the logged in user info and save the LeadContact
                    var leadContact = _mapper.Map<LeadContact>(leadContactReceivingDTO);

                    leadContact.CreatedById = context.GetLoggedInUserId();
                    var contactEntity = await _context.LeadContacts.AddAsync(leadContact);
                    await _context.SaveChangesAsync();
                    var savedLeadContact = contactEntity.Entity;
                    _context.Leads.Update(lead);
                    await _context.SaveChangesAsync();
                    //Map savedContact to a dto and return it
                    await transaction.CommitAsync();
                    var leadContactTransferDTO = _mapper.Map<LeadContactTransferDTO>(savedLeadContact);
                    return CommonResponse.Send(ResponseCodes.SUCCESS,leadContactTransferDTO);                    
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        public async Task<ApiCommonResponse> GetAllLeadContact()
        {
            var leadContacts = await _leadContactRepo.FindAllLeadContact();
            if (leadContacts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadContactTransferDTO = _mapper.Map<IEnumerable<LeadContactTransferDTO>>(leadContacts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadContactTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadContactById(long id)
        {
            var leadContact = await _leadContactRepo.FindLeadContactById(id);
            if (leadContact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var leadContactTransferDTO = _mapper.Map<LeadContactTransferDTO>(leadContact);
            return CommonResponse.Send(ResponseCodes.SUCCESS,leadContactTransferDTO);
        }

        

        public async Task<ApiCommonResponse> UpdateLeadContact(HttpContext context, long id, LeadContactReceivingDTO leadContactReceivingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    var leadContactToUpdate = await _context.LeadContacts.FirstOrDefaultAsync(contact => contact.Id == id);
                    if (leadContactToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }
                    
                    var summary = $"Initial details before change, \n {leadContactToUpdate.ToString()} \n" ;

                    leadContactToUpdate.FirstName = leadContactReceivingDTO.FirstName;
                    leadContactToUpdate.LastName = leadContactReceivingDTO.LastName;
                    leadContactToUpdate.Gender = leadContactReceivingDTO.Gender;
                    leadContactToUpdate.Title = leadContactReceivingDTO.Title;
                    leadContactToUpdate.DateOfBirth = leadContactReceivingDTO.DateOfBirth;
                    leadContactToUpdate.DesignationId = leadContactReceivingDTO.DesignationId;
                    leadContactToUpdate.ClientContactQualificationId = leadContactReceivingDTO.ClientContactQualificationId;
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
                    return CommonResponse.Send(ResponseCodes.SUCCESS,leadContactTransferDTOs);
                    
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }

        public async Task<ApiCommonResponse> DeleteLeadContact(long id)
        {
            var leadContactToDelete = await _leadContactRepo.FindLeadContactById(id);
            if (leadContactToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _leadContactRepo.DeleteLeadContact(leadContactToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

    }
}