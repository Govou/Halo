using System;
using AutoMapper;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
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
            CreateMap<OperatingEntity, OperatingEntityWithoutServiceGroupDTO>();
            CreateMap<OperatingEntityReceivingDTO, OperatingEntity>();
            CreateMap<Division,DivisionWithoutOperatingEntityDTO>();
            CreateMap<State, StateWithoutLGATransferDto>();
            CreateMap<OfficeReceivingDTO, Office>();
            CreateMap<Office, OfficeTransferDTO>();
            CreateMap<StrategicBusinessUnit, StrategicBusinessUnitTransferDTO>();
            CreateMap<StrategicBusinessUnit, SBUWithoutOperatingEntityTransferDTO>();
            CreateMap<StrategicBusinessUnitReceivingDTO, StrategicBusinessUnit>();
            CreateMap<ServiceGroup, ServiceGroupTransferDTO>();
            CreateMap<ServiceGroup, ServiceGroupWithoutServiceCategoryDTO>();
            CreateMap<ServiceGroupReceivingDTO, ServiceGroup>();
            CreateMap<ServiceCategoryReceivingDTO, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryTransferDTO>();
            CreateMap<ServiceCategory, ServiceCategoryWithoutServicesTransferDTO>();
            CreateMap<Services, ServicesTransferDTO>()
                .ForMember(dest => dest.RequiredServiceDocument, opt => 
                opt.MapFrom(src => src.RequiredServiceDocument.GetListOfRequiredDocuments()));
            CreateMap<ServicesReceivingDTO, Services>();
            CreateMap<AccountClass, AccountClassTransferDTO>();
            CreateMap<AccountClassReceivingDTO, AccountClass>();
            CreateMap<LeadTypeReceivingDTO, LeadType>();
            CreateMap<LeadType, LeadTypeTransferDTO>();
            CreateMap<DropReasonReceivingDTO, DropReason>();
            CreateMap<DropReason, DropReasonTransferDTO>();
            CreateMap<LeadOriginReceivingDTO, LeadOrigin>();
            CreateMap<LeadOrigin, LeadOriginTransferDTO>();
            CreateMap<FinancialVoucherTypeReceivingDTO, FinanceVoucherType>();
            CreateMap<FinanceVoucherType, FinancialVoucherTypeTransferDTO>();
            CreateMap<AccountReceivingDTO, Account>();
            CreateMap<Account, AccountTransferDTO>();
            CreateMap<GroupTypeReceivingDTO, GroupType>();
            CreateMap<GroupType, GroupTypeTransferDTO>();
            CreateMap<RelationshipReceivingDTO, Relationship>();
            CreateMap<Relationship, RelationshipTransferDTO>();
            CreateMap<BankReceivingDTO, Bank>();
            CreateMap<Bank, BankTransferDTO>();
            CreateMap<StandardSLAForOperatingEntitiesReceivingDTO, StandardSLAForOperatingEntities>();
            CreateMap<StandardSLAForOperatingEntities, StandardSLAForOperatingEntitiesTransferDTO>();
            CreateMap<TargetReceivingDTO, Target>();
            CreateMap<Target, TargetTransferDTO>();
            CreateMap<Target, BaseSetupTransferDTO>();
            CreateMap<MeansOfIdentificationReceivingDTO, MeansOfIdentification>();
            CreateMap<MeansOfIdentification, MeansOfIdentificationTransferDTO>();
            CreateMap<AccountDetailReceivingDTO, AccountDetail>();
            CreateMap<AccountDetail, AccountDetailTransferDTO>();
            CreateMap<AccountMasterReceivingDTO, AccountMaster>();
            CreateMap<AccountMaster, AccountMasterTransferDTO>();
            CreateMap<ServiceCategoryTaskReceivingDTO, ServiceCategoryTask>();
            CreateMap<ServiceCategoryTask, ServiceCategoryTaskTransferDTO>();
            CreateMap<ServiceCategoryTask, BaseSetupTransferDTO>();
            CreateMap<ServiceTaskDeliverableReceivingDTO, ServiceTaskDeliverable>();
            CreateMap<ServiceTaskDeliverable, ServiceTaskDeliverableTransferDTO>();
            CreateMap<ServiceTaskDeliverable, BaseSetupTransferDTO>();
            CreateMap<RequiredServiceDocumentReceivingDTO, RequiredServiceDocument>();
            CreateMap<RequiredServiceDocument, RequiredServiceDocumentTransferDTO>();
            CreateMap<ServiceType, ServiceTypeTransferDTO>();
            CreateMap<ServiceTypeReceivingDTO, ServiceType>();
            CreateMap<ServiceType, BaseSetupTransferDTO>();
            CreateMap<RequredServiceQualificationElementReceivingDTO, RequredServiceQualificationElement>();
            CreateMap<RequredServiceQualificationElement, RequredServiceQualificationElementTransferDTO>();
            CreateMap<RequredServiceQualificationElement, BaseSetupTransferDTO>();
            CreateMap<LeadContactReceivingDTO, LeadContact>();
            CreateMap<LeadContact, LeadContactTransferDTO>();
            CreateMap<LeadDivisionContactReceivingDTO, LeadDivisionContact>();
            CreateMap<LeadDivisionContact, LeadDivisionContactTransferDTO>();
            CreateMap<CustomerReceivingDTO, Customer>();
            CreateMap<Customer, CustomerTransferDTO>();
        }
    }
}