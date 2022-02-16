﻿using System;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ContactDTO;
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
    }

}
