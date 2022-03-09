using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ServiceWishlistServiceImpl : IServiceWishlistService
    {
        private readonly IServiceWishlistRepository _serviceWishlistRepo;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo ;
        public ServiceWishlistServiceImpl(IServiceWishlistRepository serviceWishlistRepo, 
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _serviceWishlistRepo = serviceWishlistRepo;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> AddServiceWishlist(HttpContext context, ServiceWishlistReceivingDTO serviceWishlistReceivingDTO)
        {
            var serviceWishlist = _mapper.Map<ServiceWishlist>(serviceWishlistReceivingDTO);
            serviceWishlist.CreatedById = context.GetLoggedInUserId();
            var savedServiceWishlist = await _serviceWishlistRepo.SaveServiceWishlist(serviceWishlist);
            if(savedServiceWishlist == null)
            {
                return new ApiResponse(500);
            }
            var serviceWishlistTransferDto = _mapper.Map<ServiceWishlistTransferDTO>(serviceWishlist);
            return new ApiOkResponse(serviceWishlistTransferDto);
        }

        public async Task<ApiResponse> FindServiceWishlistById(long id)
        {
            var serviceWishlist = await _serviceWishlistRepo.FindServiceWishlistById(id);
            if(serviceWishlist == null)
            {
                return new ApiResponse(404);
            }
            var serviceWishlistTransferDto = _mapper.Map<ServiceWishlistTransferDTO>(serviceWishlist);
            return new ApiOkResponse(serviceWishlistTransferDto);
        }

        public async Task<ApiResponse> FindServiceWishlistsByProspectId(long id)
        {
            var serviceWishlists = await _serviceWishlistRepo.FindServiceWishlistsByProspectId(id);
            if (serviceWishlists == null)
            {
                return new ApiResponse(404);
            }
            var serviceWishlistTransferDto = _mapper.Map<IEnumerable<ServiceWishlistTransferDTO>>(serviceWishlists);
            return new ApiOkResponse(serviceWishlistTransferDto);
        }

        public async Task<ApiResponse> FindAllServiceWishlists()
        {
            var serviceWishlists = await _serviceWishlistRepo.FindAllServiceWishlists();
            if(serviceWishlists == null )
            {
                return new ApiResponse(404);
            }
            var serviceWishlistsTransferDto = _mapper.Map<IEnumerable<ServiceWishlistTransferDTO>>(serviceWishlists);
            return new ApiOkResponse(serviceWishlistsTransferDto);
        }

        public async Task<ApiResponse> UpdateServiceWishlist(HttpContext context, long serviceWishlistId, ServiceWishlistReceivingDTO serviceWishlistReceivingDTO)
        {
            var serviceWishlistToUpdate = await _serviceWishlistRepo.FindServiceWishlistById(serviceWishlistId);
            if(serviceWishlistToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {serviceWishlistToUpdate.ToString()} \n" ;
            serviceWishlistToUpdate.ProspectId = serviceWishlistReceivingDTO.ProspectId;
            serviceWishlistToUpdate.ServiceId = serviceWishlistReceivingDTO.ServiceId;

            summary += $"Details after change, \n {serviceWishlistToUpdate} \n";

            var updatedServiceWishlist = await _serviceWishlistRepo.UpdateServiceWishlist(serviceWishlistToUpdate);

            if(updatedServiceWishlist == null)
            {
                return new ApiResponse(500);
            }      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ServiceWishlist",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedServiceWishlist.Id
            };

            await _historyRepo.SaveHistory(history);

            var serviceWishlistTransferDto = _mapper.Map<ServiceWishlistTransferDTO>(updatedServiceWishlist);
            return new ApiOkResponse(serviceWishlistTransferDto);
        }

        public async Task<ApiResponse> DeleteServiceWishlist(long serviceWishlistId)
        {
            var serviceWishlistToDelete = await _serviceWishlistRepo.FindServiceWishlistById(serviceWishlistId);
            if(serviceWishlistToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(!await _serviceWishlistRepo.RemoveServiceWishlist(serviceWishlistToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}