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
    public class VehicleServiceImpl:IVehicleService
    {
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IMapper _mapper;

        public VehicleServiceImpl(IMapper mapper, IVehiclesRepository vehiclesRepository)
        {
            _mapper = mapper;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<ApiCommonResponse> AddVehicleType(HttpContext context, VehicleTypeReceivingDTO vehicleTypeReceivingDTO)
        {
            var Type = _mapper.Map<VehicleType>(vehicleTypeReceivingDTO);
            var NameExist = _vehiclesRepository.GetTypename(vehicleTypeReceivingDTO.TypeName);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            Type.CreatedById = context.GetLoggedInUserId();
            Type.IsDeleted = false;
            Type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _vehiclesRepository.SaveVehicleType(Type);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<VehicleTypeTransferDTO>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteVehicleType(long id)
        {
            var typeToDelete = await _vehiclesRepository.FindVehicleTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _vehiclesRepository.DeleteVehicleType(typeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllVehicleTypes()
        {
            var Type = await _vehiclesRepository.FindAllVehicleTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleTypeTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleTypeById(long id)
        {
            var Type = await _vehiclesRepository.FindVehicleTypeById(id);
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<VehicleTypeTransferDTO>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateVehicleType(HttpContext context, long id, VehicleTypeReceivingDTO vehicleTypeReceivingDTO)
        {
            var typeToUpdate = await _vehiclesRepository.FindVehicleTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = vehicleTypeReceivingDTO.TypeName;
            typeToUpdate.TypeDesc = vehicleTypeReceivingDTO.TypeDesc;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _vehiclesRepository.UpdateVehicleType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var typeTransferDTOs = _mapper.Map<VehicleTypeTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTOs);
        }
    }
}
