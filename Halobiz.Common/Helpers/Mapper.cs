using AutoMapper;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.DTOs.TransferDTOs.RoleManagement;
using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.Helpers
{
    public class Mapping
    {
        private IMapper _iMapper;

        public Mapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfileTransferDTO, UserProfile>();

            });

            _iMapper = config.CreateMapper();
        }

    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Source, Destination>();
            CreateMap<RoleTransferDTO, Role>();
            CreateMap<UserProfileTransferDTO, UserProfile>();
        }
    }
}
