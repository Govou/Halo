using AutoMapper;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Common.DTOs.TransferDTOs.RoleManagement;
using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.Helpers
{
    public class AutoMappingSuper : Profile
    {
        public AutoMappingSuper()
        {
            CreateMap<RoleReceivingDTO, Role>();
            CreateMap<Role, RoleTransferDTO>();
        }
    }
}
