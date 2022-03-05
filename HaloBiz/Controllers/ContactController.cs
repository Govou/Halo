using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ContactDTO;
using HaloBiz.MyServices;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController:ControllerBase
    {
        private readonly IContactServiceImpl _contactServiceImpl;

        public ContactController(IContactServiceImpl contactSerceImpl)
        {
            this._contactServiceImpl = contactSerceImpl;
        }

        [HttpPost("CreateNewContact")]
        public async Task<ApiCommonResponse> CreateNewContact(Contactdto contactDto)
        {
            return await _contactServiceImpl.AddNewContact(HttpContext, contactDto);
        }


        [HttpPost("AttachContactToSuspect")]
        public async Task<ApiCommonResponse> AttachContactToSuspect(SuspectContactDTO suspectContactDTO)
        {
            return await _contactServiceImpl.AttachToSuspect(HttpContext, suspectContactDTO);
        }



        [HttpGet("GetAllContacts")]
        public async Task<ApiCommonResponse> GetAllContacts()
        {
            return await _contactServiceImpl.GetAllContact(HttpContext);
        }

        [HttpDelete("DisableContact/{contactId}")]
        public async Task<ApiCommonResponse> DisableContact(long contactId)
        {
            return await _contactServiceImpl.disableContact(HttpContext, contactId);
        }

        [HttpPut("UpdateContact/{contactId}")]
        public async Task<ApiCommonResponse> UpdateContact(long contactId,Contactdto contactDto)
        {
            return await _contactServiceImpl.updateContact(HttpContext, contactId, contactDto);
        }

        [HttpGet("GetAllSupects")]
        public async Task<ApiCommonResponse> GetAllSupects()
        {
            return await _contactServiceImpl.GetAllSuspect();
        }

        [HttpGet("GetContactsAttachedToSuspects/{suspectId}")]
        public async Task<ApiCommonResponse> GetAllSupects(long suspectId)
        {
            return await _contactServiceImpl.GetContactsAttachedToSuspect(suspectId);
        }

        [HttpGet("GetContactsAttachedToCustomer/{customerId}")]
        public async Task<ApiCommonResponse> GetAllCustomers(long customerId)
        {
            return await _contactServiceImpl.GetContactsAttachedToCustomer(customerId);
        }

        [HttpPut("DetatchContact/{suspectId}/{contactId}")]
        public async Task<ApiCommonResponse> DetatchContact(long suspectId,long contactId)
        {
            return await _contactServiceImpl.detachContact(HttpContext,suspectId,contactId);
        }

        [HttpGet("GetAllMeetings/{suspectId}")]
        public async Task<ApiCommonResponse> GetAllMeetings(long suspectId)
        {
            return await _contactServiceImpl.GetMeeting(suspectId);
        }

        [HttpGet("GetAllContactsBySuspect/{suspectId}")]
        public async Task<ApiCommonResponse> GetAllContactsBySuspect(long suspectId)
        {
            return await _contactServiceImpl.getContactsForSuspectsById(HttpContext,suspectId);
        }


        [HttpPost("CreateMeeting")]
        public async Task<ApiCommonResponse> CreateMeeting(MeetingDTO meetingDTO)
        {
            return await _contactServiceImpl.createMeeting(HttpContext, meetingDTO);
        }

        [HttpPut("UpdateMeeting/{meetingId}")]
        public async Task<ApiCommonResponse> UpdateMeeting(long meetingId, MeetingDTO meetingDTO)
        {
            return await _contactServiceImpl.updateMeeting(HttpContext, meetingId, meetingDTO);
        }

        [HttpPut("changeTodoStatus/{todoId}")]
        public async Task<ApiCommonResponse> changeTodoStatus(long todoId)
        {
            return await _contactServiceImpl.changeTodoStatus(HttpContext, todoId);
        }

        [HttpPost("AddMorStaff/{meetingId}")]
        public async Task<ApiCommonResponse> AddMorStaff(long meetingId,List<MeetingStaff> meetingDTO)
        {
            return await _contactServiceImpl.AddmoreStaff(HttpContext,meetingId, meetingDTO);
        }

        [HttpPost("AddMoreContact/{meetingId}")]
        public async Task<ApiCommonResponse> AddMoreContacts(long meetingId, List<MeetingContact> meetingContacts)
        {
            return await _contactServiceImpl.AddmoreContact(HttpContext, meetingId, meetingContacts);
        }


        [HttpDelete("DisableMeeting/{meetingId}")]
        public async Task<ApiCommonResponse> DisableMeeting(long meetingId)
        {
            return await _contactServiceImpl.removeMeeting(HttpContext, meetingId);
        }

        [HttpDelete("DisableMeetingContact/{meetingId}/{contactId}")]
        public async Task<ApiCommonResponse> DisableMeetingContact(long meetingId,long contactId)
        {
            return await _contactServiceImpl.removeContact(HttpContext, meetingId,contactId);
        }

        [HttpDelete("DisableMeetingStaff/{meetingId}/{staffId}")]
        public async Task<ApiCommonResponse> DisableMeetingStaff(long meetingId, long staffId)
        {
            return await _contactServiceImpl.removeStaff(HttpContext, meetingId, staffId);
        }

        [HttpPost("CreateNewGoal/{suspectId}")]
        public async Task<ApiCommonResponse> CreateNewGoal(long suspectId,GoalDTO goalDTO)
        {
            return await _contactServiceImpl.CreateGoal(HttpContext, suspectId,goalDTO);
        }

        [HttpPut("UpdateGoal/{goalId}")]
        public async Task<ApiCommonResponse> UpdateGoal(long goalId, GoalDTO goalDTO)
        {
            return await _contactServiceImpl.updateGoal(HttpContext, goalId, goalDTO);
        }

        [HttpDelete("DisableGoal/{goalId}")]
        public async Task<ApiCommonResponse> DisableGoal(long goalId)
        {
            return await _contactServiceImpl.removeGoal(HttpContext, goalId);
        }

        [HttpDelete("DisableStaff/{meetingId}")]
        public async Task<ApiCommonResponse> DisableStaff(long goalId)
        {
            return await _contactServiceImpl.removeGoal(HttpContext, goalId);
        }

        [HttpGet("GetAllGoal/{suspectId}")]
        public async Task<ApiCommonResponse> GetAllGoal(long suspectId)
        {
            return await _contactServiceImpl.getGoalsBySuspectId(HttpContext, suspectId);
        }

        [HttpPost("CreateNewTodo/{goalId}")]
        public async Task<ApiCommonResponse> CreateNewTodo(long goalId, TodoDTO todoDTO)
        {
            return await _contactServiceImpl.CreateTodo(HttpContext, goalId, todoDTO);
        }

        [HttpPut("UpdateTodo/{todoId}")]
        public async Task<ApiCommonResponse> UpdateTodo(long todoId, TodoDTO todoDTO)
        {
            return await _contactServiceImpl.UpdateTodo(HttpContext, todoId, todoDTO);
        }

        [HttpDelete("DisableTodo/{todoId}")]
        public async Task<ApiCommonResponse> DisableTodo(long todoId)
        {
            return await _contactServiceImpl.removeTodo(HttpContext, todoId);
        }

        [HttpGet("GetAllTodo/{goalId}")]
        public async Task<ApiCommonResponse> GetAllTodo(long goalId)
        {
            return await _contactServiceImpl.getTodoByGoalId(HttpContext, goalId);
        }

        [HttpGet("GetDashBoardSummary/{suspectId}")]
        public async Task<ApiCommonResponse> GetDashBoardSummary(long suspectId)
        {
            return await _contactServiceImpl.getDashBoardForSuspect(HttpContext, suspectId);
        }

        [HttpGet("GetLeadClassificationsData")]
        public async Task<ApiCommonResponse> GetLeadClassificationsData()
        {
            return await _contactServiceImpl.GetLeadClassificationsData(HttpContext);
        }

        [HttpGet("GetLeadClassificationsDataById/{createdById}")]
        public async Task<ApiCommonResponse> GetLeadClassificationsDataBySuspectId(long createdById)
        {
            return await _contactServiceImpl.GetLeadClassificationsDataById(HttpContext, createdById);
        }

        [HttpGet("GetLeadClassificationsDataByDate/{startDate}/{endDate}")]
        public async Task<ApiCommonResponse> GetLeadClassificationsDataBySuspectId(DateTime startDate,DateTime endDate)
        {
            return await _contactServiceImpl.GetLeadClassificationsDataByDates(HttpContext, startDate,endDate);
        }

        [HttpGet("GetLeadsOpportunity")]
        public async Task<ApiCommonResponse> GetLeadsOpportunity()
        {
            return await _contactServiceImpl.GetLeadsOpportunityData(HttpContext);
        }

        [HttpGet("GetLeadsOpportunityById/{createdById}")]
        public async Task<ApiCommonResponse> GetLeadsOpportunityById(long createdById)
        {
            return await _contactServiceImpl.GetLeadsOpportunityDataByCreatedId(HttpContext, createdById);
        }

        [HttpGet("GetContractByLead/{Id}")]
        public async Task<ApiCommonResponse> GetContractByLead(long Id)
        {
            return await _contactServiceImpl.getContractByLeadId(Id);
        }
    }
}
