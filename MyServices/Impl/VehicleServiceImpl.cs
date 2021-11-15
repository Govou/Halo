﻿using AutoMapper;
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
    public class VehicleServiceImpl:IVehicleService
    {
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IMapper _mapper;

        public VehicleServiceImpl(IMapper mapper, IVehiclesRepository vehiclesRepository)
        {
            _mapper = mapper;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<ApiResponse> AddVehicleType(HttpContext context, VehicleTypeReceivingDTO vehicleTypeReceivingDTO)
        {
            var Type = _mapper.Map<VehicleType>(vehicleTypeReceivingDTO);
            var NameExist = _vehiclesRepository.GetTypename(vehicleTypeReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            Type.CreatedById = context.GetLoggedInUserId();
            Type.IsDeleted = false;
            Type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _vehiclesRepository.SaveVehicleType(Type);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<VehicleTypeTransferDTO>(Type);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> DeleteVehicleType(long id)
        {
            var typeToDelete = await _vehiclesRepository.FindVehicleTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _vehiclesRepository.DeleteVehicleType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllVehicleTypes()
        {
            var Type = await _vehiclesRepository.FindAllVehicleTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleTypeTransferDTO>>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetVehicleTypeById(long id)
        {
            var Type = await _vehiclesRepository.FindVehicleTypeById(id);
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<VehicleTypeTransferDTO>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateVehicleType(HttpContext context, long id, VehicleTypeReceivingDTO vehicleTypeReceivingDTO)
        {
            var typeToUpdate = await _vehiclesRepository.FindVehicleTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = vehicleTypeReceivingDTO.TypeName;
            typeToUpdate.TypeDesc = vehicleTypeReceivingDTO.TypeDesc;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _vehiclesRepository.UpdateVehicleType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var typeTransferDTOs = _mapper.Map<VehicleTypeTransferDTO>(updatedType);
            return new ApiOkResponse(typeTransferDTOs);
        }
    }
}