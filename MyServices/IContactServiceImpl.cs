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

    }
}
