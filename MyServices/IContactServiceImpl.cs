using System;
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
    }
}
