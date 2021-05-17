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
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HaloBiz.Model;
using HalobizMigrations.Models.Complaints;

namespace HaloBiz.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<State, StateTransferDTO>();    
            CreateMap<Lga, LGATransferDTO>();
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
            CreateMap<StrategicBusinessUnit, StrategicBusinessUnitTransferDTO>().AfterMap((x, y) => 
            {
                y.Members = x.UserProfiles;
            });
            CreateMap<StrategicBusinessUnit, SBUWithoutOperatingEntityTransferDTO>();
            CreateMap<StrategicBusinessUnitReceivingDTO, StrategicBusinessUnit>();
            CreateMap<ServiceGroup, ServiceGroupTransferDTO>();
            CreateMap<ServiceGroup, ServiceGroupWithoutServiceCategoryDTO>();
            CreateMap<ServiceGroupReceivingDTO, ServiceGroup>();
            CreateMap<ServiceCategoryReceivingDTO, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryTransferDTO>();
            CreateMap<ServiceCategory, ServiceCategoryWithoutServiceTransferDTO>();
            CreateMap<Service, ServiceTransferDTO>()
                .ForMember(dest => dest.RequiredServiceDocument, opt => 
                opt.MapFrom(src => src.ServiceRequiredServiceDocuments.GetListOfRequiredDocuments()))
                .ForMember(dest => dest.RequiredServiceFields, opt => 
                opt.MapFrom(src => src.ServiceRequredServiceQualificationElements.GetListOfRequiredQualificationElements()));
            CreateMap<ServiceReceivingDTO, Service>();
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
            CreateMap<StandardSlaforOperatingEntityReceivingDTO, StandardSlaforOperatingEntity>();
            CreateMap<StandardSlaforOperatingEntity, StandardSlaforOperatingEntityTransferDTO>();
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
            CreateMap<ContractService, ContractService>();
            CreateMap<ContractService, ContractServiceForContractTransferDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ContractEndDate > DateTime.Now));
            CreateMap<Contract, ContractTransferDTO>();
            CreateMap<Contract, ContractForCustomerDivisionTransferDTO>();
            CreateMap<Contract, ContractSummaryTransferDTO>().
                ForMember(dest => dest.ContractValue, opt => opt.MapFrom(
                    src => src.ContractServices.GetTotalContractValue()
                ));
            CreateMap<SbutoQuoteServiceProportion, SbutoQuoteServiceProportionTransferDTO>();
            CreateMap<SbutoQuoteServiceProportionReceivingDTO, SbutoQuoteServiceProportion>();
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
            CreateMap<Receipt, Receipt>();
            CreateMap<ReceiptReceivingDTO, Receipt>();
            CreateMap<Invoice, InvoiceTransferDTO>().ForMember(dest => dest.TotalAmountReceipted, 
                opt => opt.MapFrom(src => src.Receipts.Sum(x => x.ReceiptValue)));
            CreateMap<InvoiceReceivingDTO, Invoice>();
            CreateMap<Invoice, Invoice>();
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
            CreateMap<EndorsementTypeReceivingDTO, EndorsementType>();
            CreateMap<EndorsementType, EndorsementTypeTransferDTO>();
            CreateMap<ClientBeneficiaryReceivingDTO, ClientBeneficiary>();
            CreateMap<ClientBeneficiary, ClientBeneficiaryTransferDTO>();
            CreateMap<SbuproportionReceivingDTO, Sbuproportion>();
            CreateMap<Sbuproportion, SbuproportionTransferDTO>();
            CreateMap<ContractServiceForEndorsementReceivingDto, ContractServiceForEndorsement>();
            CreateMap<ContractServiceForEndorsement, ContractServiceForEndorsementTransferDto>();
            CreateMap<ContractServiceForEndorsement, ContractService>();
            CreateMap<ModeOfTransportReceivingDTO, ModeOfTransport>();
            CreateMap<ModeOfTransport, ModeOfTransportTransferDTO>();
            CreateMap<SupplierReceivingDTO, Supplier>();
            CreateMap<Supplier, SupplierTransferDTO>();
            CreateMap<SupplierCategoryReceivingDTO, SupplierCategory>();
            CreateMap<SupplierCategory, SupplierCategoryTransferDTO>();
            CreateMap<SupplierServiceReceivingDTO, SupplierService>();
            CreateMap<SupplierService, SupplierServiceTransferDTO>();
            CreateMap<ClientEngagementReceivingDTO, ClientEngagement>();
            CreateMap<ClientEngagement, ClientEngagementTransferDTO>();
            CreateMap<LeadEngagementReceivingDTO, LeadEngagement>();
            CreateMap<LeadEngagement, LeadEngagementTransferDTO>();
            CreateMap<ClientContactQualificationReceivingDTO, ClientContactQualification>();
            CreateMap<ClientContactQualification, ClientContactQualificationTransferDTO>();
            CreateMap<EngagementTypeReceivingDTO, EngagementType>();
            CreateMap<EngagementType, EngagementTypeTransferDTO>();
            CreateMap<EngagementReasonReceivingDTO, EngagementReason>();
            CreateMap<EngagementReason, EngagementReasonTransferDTO>();
            CreateMap<ProspectReceivingDTO, Prospect>();
            CreateMap<Prospect, ProspectTransferDTO>();
            CreateMap<ServicePricingReceivingDTO, ServicePricing>();
            CreateMap<ServicePricing, ServicePricingTransferDTO>();
            CreateMap<ComplaintTypeReceivingDTO, ComplaintType>();
            CreateMap<ComplaintType, ComplaintTypeTransferDTO>();
            CreateMap<ComplaintOriginReceivingDTO, ComplaintOrigin>();
            CreateMap<ComplaintOrigin, ComplaintOriginTransferDTO>();
            CreateMap<EscalationLevelReceivingDTO, EscalationLevel>();
            CreateMap<EscalationLevel, EscalationLevelTransferDTO>();
            CreateMap<ProfileEscalationLevelReceivingDTO, ProfileEscalationLevel>();
            CreateMap<ProfileEscalationLevel, ProfileEscalationLevelTransferDTO>();      
            CreateMap<ComplaintReceivingDTO, Complaint>();
            CreateMap<Complaint, ComplaintTransferDTO>();
            CreateMap<ComplaintTransferDTO, Complaint>();
            CreateMap<ComplaintSourceReceivingDTO, ComplaintSource>();
            CreateMap<ComplaintSource, ComplaintSourceTransferDTO>();
            CreateMap<EscalationMatrixReceivingDTO, EscalationMatrix>();
            CreateMap<EscalationMatrix, EscalationMatrixTransferDTO>();
            CreateMap<EvidenceReceivingDTO, Evidence>();
            CreateMap<Evidence, EvidenceTransferDTO>();
            CreateMap<ClientPolicyReceivingDTO, ClientPolicy>();
            CreateMap<ClientPolicy, ClientPolicyTransferDTO>();
            CreateMap<EscalationLevelUserProfileReceivingDTO, EscalationMatrixUserProfile>();
            CreateMap<ComplaintDTO, Complaint>();
            CreateMap<ComplaintAssesment, ComplaintAssessmentTransferDTO>();
            CreateMap<ComplaintInvestigation, ComplaintInvestigationTransferDTO>();
            CreateMap<ComplaintResolution, ComplaintResolutionTransferDTO>();
            CreateMap<SuspectReceivingDTO, Suspect>();
            CreateMap<Suspect, SuspectTransferDTO>();
            CreateMap<SuspectQualificationReceivingDTO, SuspectQualification>();
            CreateMap<SuspectQualification, SuspectQualificationTransferDTO>();
            CreateMap<ServiceQualificationReceivingDTO, ServiceQualification>();
            CreateMap<ServiceQualification, ServiceQualificationTransferDTO>();
        }
    }
}