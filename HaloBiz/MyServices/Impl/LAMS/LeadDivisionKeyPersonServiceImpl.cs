using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadDivisionKeyPersonServiceImpl : ILeadDivisionKeyPersonService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadDivisionKeyPersonRepository _LeadDivisionKeyPersonRepo;
        private readonly IMapper _mapper;

        public LeadDivisionKeyPersonServiceImpl(IModificationHistoryRepository historyRepo,  IMapper mapper, ILeadDivisionKeyPersonRepository LeadDivisionKeyPersonRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._LeadDivisionKeyPersonRepo = LeadDivisionKeyPersonRepo;
        }

        public async Task<ApiCommonResponse> AddLeadDivisionKeyPerson(HttpContext context, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO)
        {
            var LeadDivisionKeyPerson = _mapper.Map<LeadDivisionKeyPerson>(LeadDivisionKeyPersonReceivingDTO);
            LeadDivisionKeyPerson.CreatedById = context.GetLoggedInUserId();
            var savedLeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.SaveLeadDivisionKeyPerson(LeadDivisionKeyPerson);
            if (savedLeadDivisionKeyPerson == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(LeadDivisionKeyPerson);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionKeyPersonTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllLeadDivisionKeyPerson()
        {
            var LeadDivisionKeyPersons = await _LeadDivisionKeyPersonRepo.FindAllLeadDivisionKeyPerson();
            if (LeadDivisionKeyPersons == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<IEnumerable<LeadDivisionKeyPersonTransferDTO>>(LeadDivisionKeyPersons);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionKeyPersonTransferDTO);
        }

        public async Task<ApiCommonResponse> GetLeadDivisionKeyPersonById(long id)
        {
            var LeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(id);
            if (LeadDivisionKeyPerson == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(LeadDivisionKeyPerson);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionKeyPersonTransferDTO);
        }

        

        public async Task<ApiCommonResponse> UpdateLeadDivisionKeyPerson(HttpContext context, long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO)
        {
            var LeadDivisionKeyPersonToUpdate = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(id);
            if (LeadDivisionKeyPersonToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {LeadDivisionKeyPersonToUpdate.ToString()} \n" ;

            LeadDivisionKeyPersonToUpdate.FirstName = LeadDivisionKeyPersonReceivingDTO.FirstName;
            LeadDivisionKeyPersonToUpdate.LastName = LeadDivisionKeyPersonReceivingDTO.LastName;
            LeadDivisionKeyPersonToUpdate.DateOfBirth = LeadDivisionKeyPersonReceivingDTO.DateOfBirth;
            LeadDivisionKeyPersonToUpdate.DesignationId = LeadDivisionKeyPersonReceivingDTO.DesignationId;
            LeadDivisionKeyPersonToUpdate.Email = LeadDivisionKeyPersonReceivingDTO.Email;
            LeadDivisionKeyPersonToUpdate.MobileNumber = LeadDivisionKeyPersonReceivingDTO.MobileNumber;
            
            var updatedLeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.UpdateLeadDivisionKeyPerson(LeadDivisionKeyPersonToUpdate);

            summary += $"Details after change, \n {updatedLeadDivisionKeyPerson.ToString()} \n";

            if (updatedLeadDivisionKeyPerson == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadDivisionKeyPerson",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadDivisionKeyPerson.Id
            };

            await _historyRepo.SaveHistory(history);

            var LeadDivisionKeyPersonTransferDTOs = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(updatedLeadDivisionKeyPerson);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionKeyPersonTransferDTOs);

        }

        public async Task<ApiCommonResponse> GetAllLeadDivisionKeyPersonsByLeadDivisionId(long leadDivisionId)
        {
            var LeadDivisionKeyPersons = await _LeadDivisionKeyPersonRepo.FindAllLeadDivisionKeyPersonByLeadDivisionId(leadDivisionId);
            if (LeadDivisionKeyPersons == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<IEnumerable<LeadDivisionKeyPersonTransferDTO>>(LeadDivisionKeyPersons);
            return CommonResponse.Send(ResponseCodes.SUCCESS,LeadDivisionKeyPersonTransferDTO); 
        }

        public async Task<ApiCommonResponse> DeleteKeyPerson(long Id)
        {
            var leadKeyPersonToDelete = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(Id);
            if (leadKeyPersonToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _LeadDivisionKeyPersonRepo.DeleteLeadDivisionKeyPerson(leadKeyPersonToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

    }
}