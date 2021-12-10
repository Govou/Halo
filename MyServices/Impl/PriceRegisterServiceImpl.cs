using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
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
    public class PriceRegisterServiceImpl:IPriceRegisterService
    {
        private readonly IPriceRegisterRepository _priceRegisterRepository;
        private readonly IMapper _mapper;

        public PriceRegisterServiceImpl(IMapper mapper, IPriceRegisterRepository priceRegisterRepository)
        {
            _mapper = mapper;
            _priceRegisterRepository = priceRegisterRepository;
        }

        public async Task<ApiResponse> AddPriceRegister(HttpContext context, PriceRegisterReceivingDTO priceRegisterReceivingDTO)
        {
            var priceReg = _mapper.Map<PriceRegister>(priceRegisterReceivingDTO);
            var NameExist = _priceRegisterRepository.GetServiceRegIdRegionAndRoute(priceRegisterReceivingDTO.ServiceRegistrationId, priceRegisterReceivingDTO.SMORouteId);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            priceReg.CreatedById = context.GetLoggedInUserId();
            //priceReg.MarkupPrice = 
            priceReg.CreatedAt = DateTime.UtcNow;
            var Save = await _priceRegisterRepository.SavePriceRegister(priceReg);
            if (Save == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<PriceRegisterTransferDTO>(priceReg);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> DeletePriceRegister(long id)
        {
            var itemToDelete = await _priceRegisterRepository.FindPriceRegisterById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _priceRegisterRepository.DeletePriceRegister(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllPriceRegisters()
        {
            var priceReg = await _priceRegisterRepository.FindAllPriceRegisters();
            if (priceReg == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PriceRegisterTransferDTO>>(priceReg);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPriceRegisterId(long id)
        {
            var priceReg = await _priceRegisterRepository.FindPriceRegisterById(id);
            if (priceReg == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PriceRegisterTransferDTO>(priceReg);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdatePriceRegister(HttpContext context, long id, PriceRegisterReceivingDTO priceRegisterReceivingDTO)
        {
            var itemToUpdate = await _priceRegisterRepository.FindPriceRegisterById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

          
            itemToUpdate.CostPrice = priceRegisterReceivingDTO.CostPrice;
            itemToUpdate.MarkupPercentage = priceRegisterReceivingDTO.MarkupPercentage;
            itemToUpdate.SellingPrice = priceRegisterReceivingDTO.SellingPrice;
            itemToUpdate.MarkupPrice = priceRegisterReceivingDTO.MarkupPrice;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _priceRegisterRepository.UpdatePriceRegister(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PriceRegisterTransferDTO>(updatedRank);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
