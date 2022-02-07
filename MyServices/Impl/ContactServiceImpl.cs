using System;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Helpers;
//using Microsoft.AspNetCore.Http;
using HaloBiz.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HaloBiz.Model;
using HalobizMigrations.Models.Halobiz;
using HaloBiz.DTOs.ContactDTO;
using System.Linq;

namespace HaloBiz.MyServices.Impl
{
    public class ContactServiceImpl:IContactServiceImpl
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ContactServiceImpl> _logger;
        private readonly IMapper _mapper;

        public ContactServiceImpl(HalobizContext context, ILogger<ContactServiceImpl> logger, IMapper mapper)
        {
         
        this._mapper = mapper;
        this._logger = logger;
        this._context = context;
        
       }

        public async Task<ApiCommonResponse> AddNewContact(HttpContext context,Contactdto contactDTO)
        {


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                                                           {
                    //Check if the user with the same lastname,email or phonenumber already exist
                    var contactInstance = await _context.Contacts.FirstOrDefaultAsync(x => x.Email  == contactDTO.Email && x.IsDeleted == false
                                                                                      || x.LastName == contactDTO.LastName && x.IsDeleted == false
                                                                                      || x.Mobile == contactDTO.Mobile && x.IsDeleted == false);
                    if (contactInstance != null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,"A user with the same Email,LastName or Mobile number already exists"); ;
                    }

                    //Map Dto to constact, add the logged in user info and save the contact
                    var newContact = _mapper.Map<Contact>(contactDTO);

                    newContact.CreatedById = context.GetLoggedInUserId();
                    newContact.IsDeleted = false;
                    newContact.CreatedAt = DateTime.Now;
                    newContact.Mobile = contactDTO.Mobile.Replace("+234-", "0");
                    newContact.Mobile2 = contactDTO.Mobile2.Replace("+234-", "0");

                    var contactEntity = await _context.Contacts.AddAsync(newContact);
                    await _context.SaveChangesAsync();
                    var savedContact = contactEntity.Entity;
                    await transaction.CommitAsync();
                    
                    return CommonResponse.Send(ResponseCodes.SUCCESS, savedContact);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }


        public async Task<ApiCommonResponse> disableContact(HttpContext httpContext,long contactId)
        {
            var contact = await _context.Contacts.Where(x => x.IsDeleted == false && x.Id == contactId).FirstOrDefaultAsync();
            if(contact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            else
            {
                contact.IsDeleted = true;
                _context.Contacts.Update(contact);
                _context.SaveChanges();
                return CommonResponse.Send(ResponseCodes.SUCCESS);

            }
        }

        public async Task<ApiCommonResponse> GetAllContact(HttpContext httpContext)
        {
            var contact = await _context.Contacts.Where(x => x.IsDeleted == false && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            if (contact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, contact);
            }
            
        }





    }
}
