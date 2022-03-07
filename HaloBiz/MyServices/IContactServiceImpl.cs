using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ContactDTO;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IContactServiceImpl
    {
        Task<ApiCommonResponse> AddNewContact(HttpContext context, Contactdto contactDTO);
        Task<ApiCommonResponse> GetAllContact(HttpContext httpContext);
        Task<ApiCommonResponse> disableContact(HttpContext httpContext, long contactId);
        Task<ApiCommonResponse> updateContact(HttpContext httpContext, long contactId, Contactdto contactdto);
        Task<ApiCommonResponse> GetAllSuspect();
        Task<ApiCommonResponse> AttachToSuspect(HttpContext httpContext,SuspectContactDTO suspectContactDTO);
        Task<ApiCommonResponse> GetContactsAttachedToSuspect(long suspectId);
        Task<ApiCommonResponse> GetContactsAttachedToCustomer(long customerId);
        Task<ApiCommonResponse> detachContact(HttpContext httpContext, long suspectid, long contactId);
        Task<ApiCommonResponse> removeMeeting(HttpContext httpContext, long meetingId);
        Task<ApiCommonResponse> createMeeting(HttpContext httpContext, MeetingDTO meetingDTO);
        Task<ApiCommonResponse> updateMeeting(HttpContext httpContext, long meetingId, MeetingDTO meetingDTO);
        Task<ApiCommonResponse> GetMeeting(long suspectId);
        Task<ApiCommonResponse> CreateGoal(HttpContext httpContext, long suspectid, GoalDTO goalDTO);
        Task<ApiCommonResponse> updateGoal(HttpContext httpContext, long goalId, GoalDTO goalDTO);
        Task<ApiCommonResponse> removeGoal(HttpContext httpContext, long goalId);
        Task<ApiCommonResponse> getGoalsBySuspectId(HttpContext httpContext, long suspectId);
        Task<ApiCommonResponse> CreateTodo(HttpContext httpContext, long goalId, TodoDTO todoDTO);
        Task<ApiCommonResponse> UpdateTodo(HttpContext httpContext, long todoId, TodoDTO todoDTO);
        Task<ApiCommonResponse> removeTodo(HttpContext httpContext, long todoId);
        Task<ApiCommonResponse> getTodoByGoalId(HttpContext httpContext, long goalId);
        Task<ApiCommonResponse> removeStaff(HttpContext httpContext, long meetingId, long staffId);
        Task<ApiCommonResponse> removeContact(HttpContext httpContext, long meetingId, long contactId);
        Task<ApiCommonResponse> AddmoreContact(HttpContext httpContext, long meetingId, List<MeetingContact> meetingDTO);
        Task<ApiCommonResponse> AddmoreStaff(HttpContext httpContext, long meetingId, List<MeetingStaff> meetingDTO);
        Task<ApiCommonResponse> getContactsForSuspectsById(HttpContext httpContext, long suspectId);
        Task<ApiCommonResponse> changeTodoStatus(HttpContext httpContext, long todoId);
        Task<ApiCommonResponse> getDashBoardForSuspect(HttpContext httpContext, long suspectId);
        Task<ApiCommonResponse> GetLeadClassificationsData(HttpContext httpContext);
        Task<ApiCommonResponse> GetLeadClassificationsDataByDates(HttpContext httpContext, DateTime startDate, DateTime endDate);
        Task<ApiCommonResponse> GetLeadClassificationsDataById(HttpContext httpContext, long CreatedById);
        Task<ApiCommonResponse> GetLeadsOpportunityData(HttpContext httpContext);
        Task<ApiCommonResponse> GetLeadsOpportunityDataByCreatedId(HttpContext httpContext, long createdById);
        Task<ApiCommonResponse> getContractByLeadId(long Id);


    }

}
