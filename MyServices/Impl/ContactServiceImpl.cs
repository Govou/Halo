using System;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Helpers;
//using Microsoft.AspNetCore.Http;
using HaloBiz.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using HaloBiz.Model;

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

        public async Task<ApiCommonResponse> AddNewContact(HttpContext context)
        {
            
           _context.LeadContacts.
        }





    }
}
