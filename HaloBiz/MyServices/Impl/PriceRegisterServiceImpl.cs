using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> AddPriceRegister(HttpContext context, PriceRegisterReceivingDTO priceRegisterReceivingDTO)
        {
            var priceReg = _mapper.Map<PriceRegister>(priceRegisterReceivingDTO);
            var NameExist = _priceRegisterRepository.GetServiceRegIdRegionAndRoute(priceRegisterReceivingDTO.ServiceRegistrationId, priceRegisterReceivingDTO.SMORouteId);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            priceReg.CreatedById = context.GetLoggedInUserId();
            //priceReg.MarkupPrice = 
            priceReg.CreatedAt = DateTime.UtcNow;
            var Save = await _priceRegisterRepository.SavePriceRegister(priceReg);
            if (Save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<PriceRegisterTransferDTO>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeletePriceRegister(long id)
        {
            var itemToDelete = await _priceRegisterRepository.FindPriceRegisterById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _priceRegisterRepository.DeletePriceRegister(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllPriceRegisters()
        {
            var priceReg = await _priceRegisterRepository.FindAllPriceRegisters();
            if (priceReg == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PriceRegisterTransferDTO>>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPriceRegistersByRouteId(long routeId)
        {
            var priceReg = await _priceRegisterRepository.FindAllPriceRegistersWithByRouteId(routeId);
            if (priceReg == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PriceRegisterTransferDTO>>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPriceRegistersByRouteId_(long? routeId, long? categoryId)
        {
            var priceReg = await _priceRegisterRepository.FindAllPriceRegistersWithByRouteId_(routeId, categoryId);
            if (priceReg == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PriceRegisterTransferDTO>>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPriceRegistersByServiceCategoryId(long categoryId)
        {
            var priceReg = await _priceRegisterRepository.FindAllPriceRegistersWithByServiceCategoryId(categoryId);
            if (priceReg == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PriceRegisterTransferDTO>>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPriceRegisterId(long id)
        {
            var priceReg = await _priceRegisterRepository.FindPriceRegisterById(id);
            if (priceReg == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PriceRegisterTransferDTO>(priceReg);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdatePriceRegister(HttpContext context, long id, PriceRegisterReceivingDTO priceRegisterReceivingDTO)
        {
            var itemToUpdate = await _priceRegisterRepository.FindPriceRegisterById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<PriceRegisterTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }
    }
}
