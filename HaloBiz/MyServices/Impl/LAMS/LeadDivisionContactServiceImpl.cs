﻿using System;
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
    public class LeadDivisionContactServiceImpl : ILeadDivisionContactService
    {
        private readonly HalobizContext _context;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadDivisionContactRepository _LeadDivisionContactRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<LeadContactServiceImpl> _logger;

        public LeadDivisionContactServiceImpl(IModificationHistoryRepository historyRepo, 
            IMapper mapper, 
            ILeadDivisionContactRepository LeadDivisionContactRepo,
            HalobizContext context,
            ILogger<LeadContactServiceImpl> logger)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._LeadDivisionContactRepo = LeadDivisionContactRepo;
            this._context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddLeadDivisionContact(HttpContext context, long leadDivisionId, LeadDivisionContactReceivingDTO LeadDivisionContactReceivingDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var leadDivision = await _context.LeadDivisions.FirstOrDefaultAsync(ld => ld.Id == leadDivisionId && ld.IsDeleted == false);
                    if (leadDivision == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                    return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionContactTransferDTO);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }         
        }

        public async Task<ApiCommonResponse> GetAllLeadDivisionContact()
        {
            var LeadDivisionContacts = await _LeadDivisionContactRepo.FindAllLeadDivisionContact();
            if (LeadDivisionContacts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var LeadDivisionContactTransferDTO = _mapper.Map<IEnumerable<LeadDivisionContactTransferDTO>>(LeadDivisionContacts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionContactTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadDivisionContactById(long id)
        {
            var LeadDivisionContact = await _LeadDivisionContactRepo.FindLeadDivisionContactById(id);
            if (LeadDivisionContact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var LeadDivisionContactTransferDTO = _mapper.Map<LeadDivisionContactTransferDTO>(LeadDivisionContact);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionContactTransferDTO);
        }



        public async Task<ApiCommonResponse> UpdateLeadDivisionContact(HttpContext context, long id, LeadDivisionContactReceivingDTO LeadDivisionContactReceivingDTO)
        {
            var LeadDivisionContactToUpdate = await _LeadDivisionContactRepo.FindLeadDivisionContactById(id);
            if (LeadDivisionContactToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {LeadDivisionContactToUpdate.ToString()} \n";

            LeadDivisionContactToUpdate.FirstName = LeadDivisionContactReceivingDTO.FirstName;
            LeadDivisionContactToUpdate.LastName = LeadDivisionContactReceivingDTO.LastName;
            LeadDivisionContactToUpdate.DateOfBirth = LeadDivisionContactReceivingDTO.DateOfBirth;
            LeadDivisionContactToUpdate.DesignationId = LeadDivisionContactReceivingDTO.DesignationId;
            LeadDivisionContactToUpdate.Email = LeadDivisionContactReceivingDTO.Email;
            LeadDivisionContactToUpdate.MobileNumber = LeadDivisionContactReceivingDTO.MobileNumber;

            var updatedLeadDivisionContact = await _LeadDivisionContactRepo.UpdateLeadDivisionContact(LeadDivisionContactToUpdate);

            summary += $"Details after change, \n {updatedLeadDivisionContact.ToString()} \n";

            if (updatedLeadDivisionContact == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionContactTransferDTOs);

        }

    }
}
