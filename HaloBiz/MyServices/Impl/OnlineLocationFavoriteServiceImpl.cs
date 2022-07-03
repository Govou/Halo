using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class OnlineLocationFavoriteServiceImpl: IOnlineLocationFavoriteService
    {
        private readonly IOnlineLocationFavoriteRepository _onlineLocationFavoriteRepository;
        private readonly IMapper _mapper;

        public OnlineLocationFavoriteServiceImpl(IMapper mapper, IOnlineLocationFavoriteRepository onlineLocationFavoriteRepositor)
        {
            _mapper = mapper;
            _onlineLocationFavoriteRepository = onlineLocationFavoriteRepositor;
        }

        public async Task<ApiCommonResponse> AddOnlineLocationFavorite(HttpContext context, OnlineLocationFavoriteReceivingDTO favoriteReceivingDTO)
        {
            var onlineLocation = _mapper.Map<OnlineLocationFavourite>(favoriteReceivingDTO);
            //var NameExist = _armedEscortsRepository.GetTypename(armedEscortTypeReceivingDTO.Name);
            //if (NameExist != null)
            //{
            //    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No record exists");
            //}

           
            //favoriteReceivingDTO.cre = DateTime.Now;
            onlineLocation.CreatedById = context.GetLoggedInUserId();
            //onlineLocation.CreatedById = favoriteReceivingDTO.ClientId;
            onlineLocation.CreatedAt = DateTime.Now;
            var savedRank = await _onlineLocationFavoriteRepository.SaveLocationFavorite(onlineLocation);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<OnlineLocationFavoriteTransferDTO>(onlineLocation);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteOnlineLocationFavorite(long id)
        {
            var itemToDelete = await _onlineLocationFavoriteRepository.FindLocationFavoriteById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _onlineLocationFavoriteRepository.DeleteLocationFavorite(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllOnlineLocationFavorites()
        {
            var location = await _onlineLocationFavoriteRepository.FindAllLocationFavorites();
            if (location == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<OnlineLocationFavoriteTransferDTO>>(location);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetOnlineLocationFavoriteByOnlineProfileId(long onlineProfileId)
        {
            var location = await _onlineLocationFavoriteRepository.FindAllLocationFavoritesByOnlineProfileId(onlineProfileId);
            if (location == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NotFound404); ;
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<OnlineLocationFavoriteTransferDTO>>(location);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetOnlineLocationFavoriteById(long id)
        {
            var location = await _onlineLocationFavoriteRepository.FindLocationFavoriteById(id);
            if (location == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var rankTransferDTO = _mapper.Map<OnlineLocationFavoriteTransferDTO>(location);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateOnlineLocationFavorite(HttpContext context, long id, OnlineLocationFavoriteReceivingDTO favoriteReceivingDTO)
        {
            var itemToUpdate = await _onlineLocationFavoriteRepository.FindLocationFavoriteById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.LocationFullAddress = favoriteReceivingDTO.LocationFullAddress;
            itemToUpdate.LocationGeometry = favoriteReceivingDTO.LocationGeometry;
            itemToUpdate.LocationLatitude = favoriteReceivingDTO.LocationLatitude;
            itemToUpdate.LocationLongitude = favoriteReceivingDTO.LocationLongitude;
            itemToUpdate.LocationLGAId = favoriteReceivingDTO.LocationLGAId;
            itemToUpdate.LocationStateId = favoriteReceivingDTO.LocationStateId;
            itemToUpdate.LocationStreetAddress = favoriteReceivingDTO.LocationStreetAddress;
           
            itemToUpdate.UpdatedAt = DateTime.Now;
            var updatedLocation = await _onlineLocationFavoriteRepository.UpdateLocationFavorite(itemToUpdate);

            //summary += $"Details after change, \n {updatedLocation.ToString()} \n";

            if (updatedLocation == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var loactionTransferDTOs = _mapper.Map<OnlineLocationFavoriteTransferDTO>(updatedLocation);
            return CommonResponse.Send(ResponseCodes.SUCCESS, loactionTransferDTOs);
        }
    }
}
