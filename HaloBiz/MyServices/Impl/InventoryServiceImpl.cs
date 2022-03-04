using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models.Complaints;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using HaloBiz.DTOs.MailDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class InventoryServiceImpl : IInventoryService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<InventoryServiceImpl> _logger;
        private readonly IMapper _mapper;
        public InventoryServiceImpl(HalobizContext context, ILogger<InventoryServiceImpl> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
    }
}
