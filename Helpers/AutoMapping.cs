using System;
using System.Linq;
using AutoMapper;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.RoleManagement;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.RoleManagement;
using halobiz_backend.DTOs.ReceivingDTOs;
using halobiz_backend.DTOs.TransferDTOs;
using halobiz_backend.Model.AccountsModel;

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
                opt.MapFrom(src => src.RequiredServiceDocument.GetListOfRequiredDocuments()))
                .ForMember(dest => dest.RequiredServiceFields, opt => 
                opt.MapFrom(src => src.RequredServiceQualificationElement.GetListOfRequiredQualificationElements()));
            CreateMap<ServicesReceivingDTO, Services>();
            CreateMap<AccountClass, AccountClassTransferDTO>();
            CreateMap<AccountClass, AccountClassWithTotalTransferDTO>();
            CreateMap<AccountClassReceivingDTO, AccountClass>();
            CreateMap<LeadTypeReceivingDTO, LeadType>();
            CreateMap<LeadType, LeadTypeTransferDTO>();
            CreateMap<DropReasonReceivingDTO, DropReason>();
            CreateMap<DropReason, DropReasonTransferDTO>();
            CreateMap<LeadOriginReceivingDTO, LeadOrigin>();
            CreateMap<LeadOrigin, LeadOriginTransferDTO>();
            CreateMap<LeadOrigin, LeadOriginWithoutTypeTransferDTO>();
            CreateMap<LeadType, LeadTypeWithoutOriginDTO>();
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
            CreateMap<AccountDetail, AccountDetailWithoutAccountMasterTransferDTO>();
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
            CreateMap<LeadKeyPersonReceivingDTO, LeadKeyPerson>();
            CreateMap<LeadKeyPerson, LeadKeyPersonTransferDTO>();
            CreateMap<LeadKeyPerson, LeadKeyPersonWithoutLeadTransferDTO>();
            CreateMap<LeadDivisionContactReceivingDTO, LeadDivisionContact>();
            CreateMap<LeadDivisionContact, LeadDivisionContactTransferDTO>();
            CreateMap<CustomerReceivingDTO, Customer>();
            CreateMap<Customer, CustomerTransferDTO>();
            CreateMap<CustomerDivisionReceivingDTO, CustomerDivision>();
            CreateMap<CustomerDivision, CustomerDivisionTransferDTO>();
            CreateMap<CustomerDivision, CustomerDivisionWithoutObjectsTransferDTO>();
            CreateMap<LeadReceivingDTO, Lead>();
            CreateMap<Lead, LeadTransferDTO>();
            CreateMap<Lead, LeadWithoutModelsTransferDTO>();
            CreateMap<LeadDivisionKeyPersonReceivingDTO, LeadDivisionKeyPerson>();
            CreateMap<LeadDivisionKeyPerson, LeadDivisionKeyPersonTransferDTO>();
            CreateMap<ControlAccountReceivingDTO, ControlAccount>();
            CreateMap<ControlAccount, ControlAccountTransferDTO>();
            CreateMap<ControlAccount, ControlAccountWithoutAccountClassTransferDTO>();
            CreateMap<ControlAccount, ControlAccountWithTotal>();
            CreateMap<QuoteReceivingDTO, Quote>();
            CreateMap<Quote, QuoteTransferDTO>();
            CreateMap<Quote, QuoteWithoutLeadDivisionTransferDTO>();
            CreateMap<QuoteServiceReceivingDTO, QuoteService>();
            CreateMap<QuoteService, QuoteServiceTransferDTO>();
            CreateMap<LeadDivisionReceivingDTO, LeadDivision>();
            CreateMap<LeadDivision, LeadDivisionTransferDTO>();
            CreateMap<Contact, ContactTransferDTO>();
            CreateMap<ClosureDocument, ClosureDocumentTransferDTO>();
            CreateMap<ClosureDocumentReceivingDTO, ClosureDocument>();
            CreateMap<ClosureDocument, DocumentSetupTransferDTO>();
            CreateMap<QuoteServiceDocument, QuoteServiceDocumentTransferDTO>();
            CreateMap<QuoteServiceDocumentReceivingDTO, QuoteServiceDocument>();
            CreateMap<QuoteServiceDocument, DocumentSetupTransferDTO>();
            CreateMap<ContractService, ContractServiceTransferDTO>();
            CreateMap<ContractService, ContractServiceForContractTransferDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ContractEndDate > DateTime.Now));
            CreateMap<Contract, ContractTransferDTO>();
            CreateMap<Contract, ContractForCustomerDivisionTransferDTO>();
            CreateMap<Contract, ContractSummaryTransferDTO>().
                ForMember(dest => dest.ContractValue, opt => opt.MapFrom(
                    src => src.ContractServices.GetTotalContractValue()
                ));
            CreateMap<SBUToQuoteServiceProportion, SBUToQuoteServiceProportionTransferDTO>();
            CreateMap<SBUToQuoteServiceProportionReceivingDTO, SBUToQuoteServiceProportion>();
            CreateMap<RegionReceivingDTO, Region>();
            CreateMap<Region, RegionTransferDTO>();
            CreateMap<ZoneReceivingDTO, Zone>();
            CreateMap<Zone, ZoneTransferDTO>();
            CreateMap<NegotiationDocument, NegotiationDocumentTransferDTO>();
            CreateMap<NegotiationDocumentReceivingDTO, NegotiationDocument>();
            CreateMap<NegotiationDocument, DocumentSetupTransferDTO>();
            CreateMap<RoleReceivingDTO, Role>();
            CreateMap<Role, RoleTransferDTO>();
            CreateMap<Claim, ClaimTransferDTO>();
            CreateMap<RoleClaimReceivingDTO, RoleClaim>();
            CreateMap<RoleClaim, RoleClaimTransferDTO>();
            CreateMap<OtherLeadCaptureInfoReceivingDTO, OtherLeadCaptureInfo>();
            CreateMap<OtherLeadCaptureInfo, OtherLeadCaptureInfoTransferDTO>();
            CreateMap<TaskFulfillmentReceivingDTO, TaskFulfillment>();
            CreateMap<TaskFulfillment, TaskFulfillmentTransferDTO>()
                .ForMember(dest => dest.ProjectCode, opt => opt.MapFrom( src => $"{src.ServiceCode}/{src.ContractServiceId}"))
                .ForMember(dest => dest.ProjectDeliveryDate, opt => opt.MapFrom( src => src.ProjectDeliveryDate?? DateTime.Now));
            CreateMap<TaskFulfillment, TaskFulfillmentTransferDetailsDTO>()
                            .ForMember(dest => dest.ProjectCode, opt => opt.MapFrom( src => $"{src.ServiceCode}/{src.ContractServiceId}"))
                .ForMember(dest => dest.ProjectDeliveryDate, opt => opt.MapFrom( src => src.ProjectDeliveryDate?? DateTime.Now));
            CreateMap<DeliverableFulfillmentReceivingDTO, DeliverableFulfillment>();
            CreateMap<DeliverableFulfillment, DeliverableFulfillmentTransferDTO>();
            CreateMap<DeliverableFulfillment, DeliverableFulfillmentWithouthTaskFulfillmentTransferDTO>();
            CreateMap<Industry, IndustryTransferDTO>();
            CreateMap<IndustryReceivingDTO, Industry>();
            CreateMap<Receipt, ReceiptTransferDTO>();
            CreateMap<ReceiptReceivingDTO, Receipt>();
            CreateMap<Designation, DesignationTransferDTO>();
            CreateMap<DesignationReceivingDTO, Designation>();
            CreateMap<Receipt, ReceiptTransferDTO>();
            CreateMap<ReceiptReceivingDTO, Receipt>();
            CreateMap<Invoice, InvoiceTransferDTO>().ForMember(dest => dest.TotalAmountReceipted, 
                opt => opt.MapFrom(src => src.Receipts.Sum(x => x.InvoiceValue)));
            CreateMap<InvoiceReceivingDTO, Invoice>();
            CreateMap<ApproverLevelReceivingDTO, ApproverLevel>();
            CreateMap<ApproverLevel, ApproverLevelTransferDTO>();
            CreateMap<ApproverLevel, BaseSetupTransferDTO>();
            CreateMap<ProcessesRequiringApprovalReceivingDTO, ProcessesRequiringApproval>();
            CreateMap<ProcessesRequiringApproval, ProcessesRequiringApprovalTransferDTO>();
            CreateMap<ProcessesRequiringApproval, BaseSetupTransferDTO>();
            CreateMap<ApprovalLimitReceivingDTO, ApprovalLimit>();
            CreateMap<ApprovalLimit, ApprovalLimitTransferDTO>();
            CreateMap<ApprovalLimit, BaseSetupTransferDTO>();
            CreateMap<ApprovalReceivingDTO, Approval>();
            CreateMap<Approval, ApprovalTransferDTO>();
            CreateMap<CompanyReceivingDTO, Company>();
            CreateMap<Company, CompanyTransferDTO>();
        }
    }
}