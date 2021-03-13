using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<ApiResponse> AddLeadDivisionKeyPerson(HttpContext context, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO)
        {
            var LeadDivisionKeyPerson = _mapper.Map<LeadDivisionKeyPerson>(LeadDivisionKeyPersonReceivingDTO);
            LeadDivisionKeyPerson.CreatedById = context.GetLoggedInUserId();
            var savedLeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.SaveLeadDivisionKeyPerson(LeadDivisionKeyPerson);
            if (savedLeadDivisionKeyPerson == null)
            {
                return new ApiResponse(500);
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(LeadDivisionKeyPerson);
            return new ApiOkResponse(LeadDivisionKeyPersonTransferDTO);
        }

        public async Task<ApiResponse> GetAllLeadDivisionKeyPerson()
        {
            var LeadDivisionKeyPersons = await _LeadDivisionKeyPersonRepo.FindAllLeadDivisionKeyPerson();
            if (LeadDivisionKeyPersons == null)
            {
                return new ApiResponse(404);
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<IEnumerable<LeadDivisionKeyPersonTransferDTO>>(LeadDivisionKeyPersons);
            return new ApiOkResponse(LeadDivisionKeyPersonTransferDTO);
        }

        public async Task<ApiResponse> GetLeadDivisionKeyPersonById(long id)
        {
            var LeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(id);
            if (LeadDivisionKeyPerson == null)
            {
                return new ApiResponse(404);
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(LeadDivisionKeyPerson);
            return new ApiOkResponse(LeadDivisionKeyPersonTransferDTO);
        }

        

        public async Task<ApiResponse> UpdateLeadDivisionKeyPerson(HttpContext context, long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO)
        {
            var LeadDivisionKeyPersonToUpdate = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(id);
            if (LeadDivisionKeyPersonToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {LeadDivisionKeyPersonToUpdate.ToString()} \n" ;

            LeadDivisionKeyPersonToUpdate.FirstName = LeadDivisionKeyPersonReceivingDTO.FirstName;
            LeadDivisionKeyPersonToUpdate.LastName = LeadDivisionKeyPersonReceivingDTO.LastName;
            LeadDivisionKeyPersonToUpdate.DateOfBirth = LeadDivisionKeyPersonReceivingDTO.DateOfBirth;
            LeadDivisionKeyPersonToUpdate.Designation = LeadDivisionKeyPersonReceivingDTO.Designation;
            LeadDivisionKeyPersonToUpdate.Email = LeadDivisionKeyPersonReceivingDTO.Email;
            LeadDivisionKeyPersonToUpdate.MobileNumber = LeadDivisionKeyPersonReceivingDTO.MobileNumber;
            
            var updatedLeadDivisionKeyPerson = await _LeadDivisionKeyPersonRepo.UpdateLeadDivisionKeyPerson(LeadDivisionKeyPersonToUpdate);

            summary += $"Details after change, \n {updatedLeadDivisionKeyPerson.ToString()} \n";

            if (updatedLeadDivisionKeyPerson == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "LeadDivisionKeyPerson",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLeadDivisionKeyPerson.Id
            };

            await _historyRepo.SaveHistory(history);

            var LeadDivisionKeyPersonTransferDTOs = _mapper.Map<LeadDivisionKeyPersonTransferDTO>(updatedLeadDivisionKeyPerson);
            return new ApiOkResponse(LeadDivisionKeyPersonTransferDTOs);

        }

        public async Task<ApiResponse> GetAllLeadDivisionKeyPersonsByLeadDivisionId(long leadDivisionId)
        {
            var LeadDivisionKeyPersons = await _LeadDivisionKeyPersonRepo.FindAllLeadDivisionKeyPersonByLeadDivisionId(leadDivisionId);
            if (LeadDivisionKeyPersons == null)
            {
                return new ApiResponse(404);
            }
            var LeadDivisionKeyPersonTransferDTO = _mapper.Map<IEnumerable<LeadDivisionKeyPersonTransferDTO>>(LeadDivisionKeyPersons);
            return new ApiOkResponse(LeadDivisionKeyPersonTransferDTO); 
        }

        public async Task<ApiResponse> DeleteKeyPerson(long Id)
        {
            var leadKeyPersonToDelete = await _LeadDivisionKeyPersonRepo.FindLeadDivisionKeyPersonById(Id);
            if (leadKeyPersonToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _LeadDivisionKeyPersonRepo.DeleteLeadDivisionKeyPerson(leadKeyPersonToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}