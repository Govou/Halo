using System;
using System.Linq;
using AutoMapper;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HaloBiz.Model;
using HalobizMigrations.Models.Complaints;

using HalobizMigrations.Models.Armada;
using HalobizMigrations.Models.Shared;
using HaloBiz.Controllers;
using HaloBiz.DTOs.ContactDTO;
using Halobiz.Common.Helpers;

namespace HaloBiz.Helpers
{
    public class AutoMapping : AutoMappingCommon
    {
        public GroupContractCategory CategoryConversion_QuoteContract(GroupQuoteCategory groupQuote)
        {
            switch (groupQuote)
            {
                case GroupQuoteCategory.IndividualQuotes:
                    return GroupContractCategory.IndividualContract;

                case GroupQuoteCategory.GroupQuoteWithSameDetails:
                    return GroupContractCategory.GroupContractWithSameDetails;

                case GroupQuoteCategory.GroupQuoteWithIndividualDetails:
                    return GroupContractCategory.GroupContractWithIndividualDetails;
                default:
                    return GroupContractCategory.IndividualContract;
            }
        }

        public AutoMapping() : base()
        {
            CreateMap<ServiceRelationship, ServiceRelationshipDTO>();
            CreateMap<Service, ServicesLeanformatDTO>();
            CreateMap<State, StateTransferDTO>();    
            CreateMap<Lga, LGATransferDTO>();
           
            CreateMap<Branch, BranchTransferDTO>();
            CreateMap<Branch, BranchWithoutOfficeTransferDTO>();
            CreateMap<BranchReceivingDTO, Branch>();
            CreateMap<Division, DivisionTransferDTO>();
            CreateMap<DivisionReceivingDTO, Division>();
            CreateMap<OperatingEntity, OperatingEntityTransferDTO>();
            CreateMap<OperatingEntityReceivingDTO, OperatingEntity>();
            CreateMap<Division,DivisionWithoutOperatingEntityDTO>();
            CreateMap<State, StateWithoutLGATransferDto>();
            CreateMap<OfficeReceivingDTO, Office>();
            CreateMap<Office, OfficeTransferDTO>();
           
            CreateMap<Quote, Contract>().AfterMap((s, d) => 
            {
                d.Id = 0;
                d.QuoteId = s.Id;
                d.CreatedById = s.CreatedById;
                d.GroupInvoiceNumber = s.GroupInvoiceNumber;
                d.GroupContractCategory = CategoryConversion_QuoteContract(s.GroupQuoteCategory);
                d.IsDeleted = false;
                d.Version = s.Version;
                d.CreatedAt = DateTime.Now;
                d.UpdatedAt = DateTime.Now;                
            });

            CreateMap<QuoteService, ContractService>()
                 .ForMember(dest => dest.QuoteServiceId, opt =>
                opt.MapFrom(src => src.Id));

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
            CreateMap<SuspectContactDTO, SuspectContact>();
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
            CreateMap<ContactDTO, ContactTransferDTO>();
            CreateMap<Contactdto, Contact>();
            CreateMap<ContactResponse, Contact>();
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
            CreateMap<RoleClaimReceivingDTO, RoleClaim>();
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
            
            //SUPPLIER
            CreateMap<SupplierReceivingDTO, Supplier>();
            CreateMap<Supplier, SupplierTransferDTO>();
            CreateMap<SupplierCategoryReceivingDTO, SupplierCategory>();
            CreateMap<SupplierCategory, SupplierCategoryTransferDTO>();
            CreateMap<SupplierServiceReceivingDTO, SupplierService>();
            CreateMap<SupplierService, SupplierServiceTransferDTO>();

            //MAKE
            CreateMap<MakeReceivingDTO, Make>();
            CreateMap<Make, MakeTransferDTO>();

            //MODEL
            CreateMap<ModelReceivingDTO, HalobizMigrations.Models.Model>();
            CreateMap<HalobizMigrations.Models.Model, ModelTransferDTO>();

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
            CreateMap<ComplaintReassignment, ComplaintReassignmentTransferDTO>();
            CreateMap<AppReviewReceivingDTO, AppReview>();
            CreateMap<AppReview, AppReviewTransferDTO>();
            CreateMap<AppFeedbackReceivingDTO, AppFeedback>();
            CreateMap<AppFeedback, AppFeedbackTransferDTO>();
            CreateMap<NoteReceivingDTO, Note>();
            CreateMap<Note, NoteTransferDTO>();
            CreateMap<ActivityReceivingDTO, Activity>();
            CreateMap<Activity, ActivityTransferDTO>();

            CreateMap<Lga, LgasTransferDTO>();

            //yus
            CreateMap<SMORouteReceivingDTO, SMORoute>();
            CreateMap<SMORoute, SMORouteTransferDTO>();
            CreateMap<SMOReturnRouteReceivingDTO, SMOReturnRoute>();
            CreateMap<SMOReturnRoute, SMOReturnRouteTransferDTO>();
            CreateMap<SMORegionReceivingDTO, SMORegion>();
            CreateMap<SMORegion, SMORegionTransferDTO>();

            CreateMap<SMORegion, SMORouteRegionTransferDTO>();
            CreateMap<SMORoute, SMORouteRegionTransferDTO>();

            //CreateMap<SMORouteMapReceivingDTO, SMORouteAndStateMap>();
            //CreateMap<SMORouteAndStateMap, SMORouteMapTransferDTO>();

            CreateMap<CommanderTypeAndRankReceivingDTO, CommanderType>(); //for type
            CreateMap<CommanderRankReceivingDTO, CommanderRank>();
            CreateMap<CommanderType, CommanderTypeAndRankTransferDTO>(); //for type
            CreateMap<CommanderRank, CommanderRankTransferDTO>();

            CreateMap<ArmedEscortTypeReceivingDTO, ArmedEscortType>(); //for type
            CreateMap<ArmedEscortRankReceivingDTO, ArmedEscortRank>();
            CreateMap<ArmedEscortType, ArmedEscortTypeTransferDTO>(); //for type
            CreateMap<ArmedEscortRank, ArmedEscortRankTransferDTO>();

            CreateMap<PilotTypeReceivingDTO, PilotType>(); //for type
            CreateMap<PilotRankReceivingDTO, PilotRank>();
            CreateMap<PilotType, PilotTypeTransferDTO>(); //for type
            CreateMap<PilotRank, PilotRankTransferDTO>();

            CreateMap<VehicleTypeReceivingDTO, VehicleType>(); //for type VehicleReceivingDTO  
            CreateMap<VehicleType, VehicleTypeTransferDTO>(); //
            CreateMap<VehicleReceivingDTO, Vehicle>();
            CreateMap<Vehicle, VehicleTransferDTO>();

            CreateMap<PilotProfileReceivingDTO, PilotProfile>();
            CreateMap<PilotProfile, PilotProfileTransferDTO>();

            CreateMap<CommanderProfileReceivingDTO, CommanderProfile>();
            CreateMap<CommanderProfile, CommanderProfileTransferDTO>();

            CreateMap<ArmedEscortProfileReceivingDTO, ArmedEscortProfile>();
            CreateMap<ArmedEscortProfile, ArmedEscortProfileTransferDTO>();


            CreateMap<ServiceRegistrationReceivingDTO, ServiceRegistration>();
            CreateMap<ServiceRegistration, ServiceRegistrationTransferDTO>();

            CreateMap<BusinessRuleReceivingDTO, BusinessRule>();
            CreateMap<BusinessRule, BusinessRuleTransferDTO>();

            CreateMap<BRPairableReceivingDTO, BRPairable>();
            CreateMap<BRPairable, BRPairableTransferDTO>();

            CreateMap<BRPairableReceivingDTO[], BRPairable>();
            CreateMap<BRPairable, BRPairableTransferDTO[]>();

          

            CreateMap<PriceRegisterReceivingDTO, PriceRegister>();
            CreateMap<PriceRegister, PriceRegisterTransferDTO>();

            CreateMap<RegServicesResourceTypesReceivingDTO.CommanderTypeRegReceivingDTO, CommanderResourceRequiredPerService>();
            CreateMap<RegServicesResourceTypesReceivingDTO.PilotTypeRegReceivingDTO, PilotResourceRequiredPerService>();
            CreateMap<RegServicesResourceTypesReceivingDTO.VehicleTypeRegReceivingDTO, VehicleResourceRequiredPerService>();
            CreateMap<RegServicesResourceTypesReceivingDTO.AEscortTypeRegReceivingDTO, ArmedEscortResourceRequiredPerService>();

            CreateMap<CommanderResourceRequiredPerService, CommanderResourceRequiredTransferDTO>();
            CreateMap<PilotResourceRequiredPerService, PilotResourceRequiredTransferDTO>();
            CreateMap<VehicleResourceRequiredPerService, VehicleResourceRequiredTransferDTO>();
            CreateMap<ArmedEscortResourceRequiredPerService, ArmedEscortResourceRequiredTransferDTO>(); 

            //CreateMap<CommanderResourceRequiredPerService, CommanderTypeRegTransferDTO>(); 
            //CreateMap<PilotResourceRequiredPerService, PilotTypeRegTransferDTO>(); 
            //CreateMap<VehicleResourceRequiredPerService, VehicleTypeRegTransferDTO>();
            //CreateMap< ArmedEscortResourceRequiredPerService, ArmedEscortTypeRegReceivingDTO>();

            CreateMap<ArmedEscortSMORoutesResourceTieReceivingDTO, ArmedEscortSMORoutesResourceTie>();
            CreateMap<CommanderSMORoutesResourceTieReceivingDTO, CommanderSMORoutesResourceTie>();
            CreateMap<PilotSMORoutesResourceTieReceivingDTO, PilotSMORoutesResourceTie>();
            CreateMap<VehicleSMORoutesResourceTieReceivingDTO, VehicleSMORoutesResourceTie>();

            CreateMap<ArmedEscortSMORoutesResourceTie, ArmedEscortSMORoutesResourceTieTransferDTO>();
            CreateMap<CommanderSMORoutesResourceTie, CommanderSMORoutesResourceTieTransferDTO>();
            CreateMap<PilotSMORoutesResourceTie, PilotSMORoutesResourceTieTransferDTO>(); 
            CreateMap<VehicleSMORoutesResourceTie, VehicleSMORoutesResourceTieTransferDTO>(); 

            //Schedules
            CreateMap<ArmedEscortDTSMastersReceivingDTO, ArmedEscortDTSMaster>();
            CreateMap<ArmedEscortDTSMaster, ArmedEscortDTSMastersTransferDTO>();
            CreateMap<ArmedEscortDTSDetailGenericDaysReceivingDTO, ArmedEscortDTSDetailGenericDay>();
            CreateMap<ArmedEscortDTSDetailGenericDay, ArmedEscortDTSDetailGenericDaysTransferDTO>();

            CreateMap<CommanderDTSMastersReceivingDTO, CommanderDTSMaster>();
            CreateMap<CommanderDTSMaster, CommanderDTSMastersTransferDTO>();
            CreateMap<CommanderDTSDetailGenericDaysReceivingDTO, CommanderDTSDetailGenericDay>();
            CreateMap<CommanderDTSDetailGenericDay, CommanderDTSDetailGenericDaysTransferDTO>();

            CreateMap<PilotDTSMastersReceivingDTO, PilotDTSMaster>();
            CreateMap<PilotDTSMaster, PilotDTSMastersTransferDTO>();
            CreateMap<PilotDTSDetailGenericDaysReceivingDTO, PilotDTSDetailGenericDay>();
            CreateMap<PilotDTSDetailGenericDay, PilotDTSDetailGenericDaysTransferDTO>();

            CreateMap<VehicleDTSMastersReceivingDTO, VehicleDTSMaster>();
            CreateMap<VehicleDTSMaster, VehicleDTSMastersTransferDTO>();
            CreateMap<VehicleDTSDetailGenericDaysReceivingDTO, VehicleDTSDetailGenericDay>();
            CreateMap<VehicleDTSDetailGenericDay, VehicleDTSDetailGenericDaysTransferDTO>();

            //ServiceAssignment Types
            CreateMap<PassengerTypesForServiceAssignmentReceivingDTO, PassengerType>();
            CreateMap<TripTypesForServiceAssignmentReceivingDTO, TripType>();
            CreateMap<SourceTypesForServiceAssignmentReceivingDTO, SourceType>();
            CreateMap<ReleaseTypesForServiceAssignmentReceivingDTO, ReleaseType>();

            //Transfer
            CreateMap<PassengerType, PassengerTypesForServiceAssignmentTransferDTO>();
            CreateMap<TripType, TripTypesForServiceAssignmentTransferDTO>();
            CreateMap<SourceType, SourceTypesForServiceAssignmentTransferDTO>();
            CreateMap<ReleaseType, ReleaseTypesForServiceAssignmentTransferDTO>();

            //ServiceAssignment
            CreateMap<MasterServiceAssignmentReceivingDTO, MasterServiceAssignment>();
            CreateMap<MasterServiceAssignmentForAutoReceivingDTO, MasterServiceAssignment>();
            CreateMap<MasterServiceAssignment, MasterServiceAssignmentTransferDTO>();
            CreateMap<SecondaryServiceAssignmentReceivingDTO, SecondaryServiceAssignment>();
            CreateMap<SecondaryServiceAssignment, SecondaryServiceAssignmentTransferDTO>();

            //ServiceAssignmentDetail
            CreateMap<ArmedEscortServiceAssignmentDetailsReceivingDTO, ArmedEscortServiceAssignmentDetail>();
            CreateMap<CommanderServiceAssignmentDetailsReceivingDTO, CommanderServiceAssignmentDetail>();
            CreateMap<PilotServiceAssignmentDetailsReceivingDTO, PilotServiceAssignmentDetail>();
            CreateMap<VehicleServiceAssignmentDetailsReceivingDTO, VehicleServiceAssignmentDetail>();


            CreateMap<ArmedEscortServiceAssignmentDetail, ArmedEscortServiceAssignmentDetailsTransferDTO>();
            CreateMap<CommanderServiceAssignmentDetail, CommanderServiceAssignmentDetailsTransferDTO>();
            CreateMap<PilotServiceAssignmentDetail, PilotServiceAssignmentDetailsTransferDTO>();
            CreateMap<VehicleServiceAssignmentDetail, VehicleServiceAssignmentDetailsTransferDTO>();

            CreateMap<PassengerReceivingDTO, Passenger>();
            CreateMap<Passenger, PassengerTransferDTO>();

            //JourneyStartandStop
            CreateMap<JourneyStartReceivingDTO, ArmadaJourneyStart>();
            CreateMap<JourneyEndReceivingDTO, ArmadaJourneyStart>();
            CreateMap<ArmadaJourneyStart, JourneyStartTransferDTO>();
            //
            CreateMap<JourneyStopReceivingDTO, ArmadaJourneyStop>();
            CreateMap<ArmadaJourneyStop, JourneyStopTransferDTO>();
            //
            CreateMap<JourneyLeadCommanderReceivingDTO, JourneyLeadCommander>();
            CreateMap<JourneyLeadCommander, JourneyLeadCommanderTransferDTO>();
            //
            CreateMap<JourneyIncidentReceivingDTO, JourneyIncident>();
            CreateMap<JourneyIncident, JourneyIncidentTransferDTO>();
            //
            CreateMap<JourneyIncidentPictureReceivingDTO, JourneyIncidentPicture>();
            CreateMap<JourneyIncidentPicture, JourneyIncidentPictureTransferDTO>();
            //
            CreateMap<JourneyNoteReceivingDTO, JourneyNote>();
            CreateMap<JourneyNote, JourneyNoteTransferDTO>();

            //CreateMap<CommanderType, SMORegionTransferDTO>();
        }
    }
}