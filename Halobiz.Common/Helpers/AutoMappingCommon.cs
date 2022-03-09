using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Common.DTOs.TransferDTOs.RoleManagement;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.Helpers;
using HalobizMigrations.Models;

namespace Halobiz.Common.Helpers
{
    public class AutoMappingCommon : Profile
    {
        public AutoMappingCommon()
        {
            CreateMap<UserProfileReceivingDTO, UserProfile>()
               .ForMember(member => member.DateOfBirth,
                   opt => opt.MapFrom(src => DateTime.Parse(src.DateOfBirth)));
            CreateMap<UserProfile, UserProfileTransferDTO>()
                .ForMember(member => member.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<OperatingEntity, OperatingEntityWithoutServiceGroupDTO>();

            CreateMap<StrategicBusinessUnit, StrategicBusinessUnitTransferDTO>().AfterMap((x, y) =>
            {
                y.Members = x.UserProfiles;
            });

            CreateMap<UserProfileTransferDTO, UserProfile>();

            CreateMap<RoleReceivingDTO, Role>();
            CreateMap<Role, RoleTransferDTO>();
        }
    }
}
