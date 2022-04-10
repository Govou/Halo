using AutoMapper;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using HalobizMigrations.Models.OnlinePortal;
using Halobiz.Common.Helpers;
using HalobizMigrations.Models;
using System;
using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Models.Complaints;

namespace OnlinePortalBackend.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {            
            CreateMap<ServiceRatingReceivingDTO, ServiceRating>();
            CreateMap<ServiceRating, ServiceRatingTransferDTO>();
            CreateMap<SecurityQuestionReceivingDTO, SecurityQuestion>();
            CreateMap<SecurityQuestion, SecurityQuestionTransferDTO>();
            CreateMap<WelcomeNoteReceivingDTO, WelcomeNote>();
            CreateMap<WelcomeNote, WelcomeNoteTransferDTO>();
            CreateMap<UserFriendlyQuestionReceivingDTO, UserFriendlyQuestion>();
            CreateMap<UserFriendlyQuestion, UserFriendlyQuestionTransferDTO>();
            CreateMap<PortalComplaintReceivingDTO, PortalComplaint>();
            CreateMap<PortalComplaint, PortalComplaintTransferDTO>();
            CreateMap<ServiceWishlistReceivingDTO, ServiceWishlist>();
            CreateMap<ServiceWishlist, ServiceWishlistTransferDTO>();
            CreateMap<OnlineProfile,OnlineProfileTransferDTO>();
            CreateMap<CustomerDivision, CustomerInfoTransferDTO>();
            CreateMap<Contract, ContractTransferDTO>();
            CreateMap<ContractService, ContractServiceTransferDTO>();
            CreateMap<Invoice, InvoiceTransferDTO>();
            CreateMap<CartContract, CartContractDTO>();
            CreateMap<CartContractDetailDTO, CartContractService>();
            CreateMap<Object, CartContract>();
            CreateMap<CartContractService, CartContractDetailDTO>();
            CreateMap<ContractServiceForEndorsement, EndorsementDTO>();
            CreateMap<Complaint, ComplaintDTO>();
            CreateMap<Receipt, ReceiptTransferDTO>();
            //CreateMap<CartItemsReceiving, CartItemsReceivingDTO>();
            //CreateMap<CompletePaymentReceiving, CompletePaymentReceivingDTO>();


        }
    }
}