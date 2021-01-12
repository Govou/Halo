using System;
using AutoMapper;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<State, StateTransferDTO>();    
            CreateMap<LGA, LGATransferDTO>();
            CreateMap<UserProfileReceivingDTO, UserProfile>()
                .ForMember(member => member.DateOfBirth, 
                    opt =>  opt.MapFrom(src => DateTime.Parse(src.DateOfBirth)));
            CreateMap<UserProfile, UserProfileTransferDTO>()
                .ForMember(member => member.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));
            CreateMap<Branch, BranchTransferDTO>();
            CreateMap<Branch, BranchWithoutOfficeTransferDTO>();
            CreateMap<BranchReceivingDTO, Branch>();
            CreateMap<Division, DivisionTransferDTO>();
            CreateMap<DivisionReceivingDTO, Division>();
            CreateMap<OperatingEntity, OperatingEntityTransferDTO>();
            CreateMap<OperatingEntityReceivingDTO, OperatingEntity>();
            CreateMap<State, StateWithoutLGATransferDto>();
            CreateMap<OfficeReceivingDTO, Office>();
            CreateMap<Office, OfficeTransferDTO>();
            CreateMap<StrategicBusinessUnit, StrategicBusinessUnitTransferDTO>();
            CreateMap<StrategicBusinessUnitReceivingDTO, StrategicBusinessUnit>();
            CreateMap<ServiceGroup, ServiceGroupTransferDTO>();
            CreateMap<ServiceGroupReceivingDTO, ServiceGroup>();
            CreateMap<ServiceCategoryReceivingDTO, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryTransferDTO>();
            CreateMap<Services, ServicesTransferDTO>();
            CreateMap<ServicesReceivingDTO, Services>();
            CreateMap<AccountClass, AccountClassTransferDTO>();
            CreateMap<AccountClassReceivingDTO, AccountClass>();
            CreateMap<LeadTypeReceivingDTO, LeadType>();
            CreateMap<LeadType, LeadTypeTransferDTO>();
            CreateMap<LeadOriginReceivingDTO, LeadOrigin>();
            CreateMap<LeadOrigin, LeadOriginTransferDTO>();
        }
    }
}