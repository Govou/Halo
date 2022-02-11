using System;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ContactDTO;
using HaloBiz.MyServices;
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

        [HttpPut("DetatchContact/{suspectId}/{contactId}")]
        public async Task<ApiCommonResponse> DetatchContact(long suspectId,long contactId)
        {
            return await _contactServiceImpl.detachContact(HttpContext,suspectId,contactId);
        }
    }
}
