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
    public class VehicleRegistrationServiceImpl:IVehicleRegistrationService
    {
        private readonly IVehicleRegistrationRepository _vehiclesRepository;
        private readonly IMapper _mapper;

        public VehicleRegistrationServiceImpl(IMapper mapper, IVehicleRegistrationRepository vehiclesRepository)
        {
            _mapper = mapper;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<ApiCommonResponse> AddVehicle(HttpContext context, VehicleReceivingDTO vehicleReceivingDTO)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleReceivingDTO);
            var IdExist = _vehiclesRepository.FindVehicleServiceById(vehicleReceivingDTO.SupplierServiceId);
            if (IdExist != null)
            {
                return new ApiResponse(409);
            }

            vehicle.CreatedById = context.GetLoggedInUserId();
            //vehicle.IsDeleted = false;
            vehicle.CreatedAt = DateTime.UtcNow;
            var savedRank = await _vehiclesRepository.SaveVehicle(vehicle);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<VehicleTransferDTO>(vehicle);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddVehicleTie(HttpContext context, VehicleSMORoutesResourceTieReceivingDTO vehicleTieReceivingDTO)
        {
            var vehicle = new VehicleSMORoutesResourceTie();

            for (int i = 0; i < vehicleTieReceivingDTO.SMORouteId.Length; i++)
            {
                vehicle.Id = 0;
                vehicle.SMORegionId = vehicleTieReceivingDTO.SMORegionId;
                vehicle.ResourceId = vehicleTieReceivingDTO.ResourceId;
                vehicle.SMORouteId = vehicleTieReceivingDTO.SMORouteId[i];
                var IdExist = _vehiclesRepository.GetResourceRegIdRegionAndRouteId(vehicleTieReceivingDTO.ResourceId, vehicleTieReceivingDTO.SMORouteId[i], vehicleTieReceivingDTO.SMORegionId);
                if (IdExist == null)
                {
                    vehicle.CreatedById = context.GetLoggedInUserId();
                    vehicle.CreatedAt = DateTime.UtcNow;

                    var savedType = await _vehiclesRepository.SaveVehicleTie(vehicle);
                    if (savedType == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    //return new ApiResponse(409);
                }

            }
            return new ApiOkResponse("Record(s) Added");
        }

        public async Task<ApiCommonResponse> DeleteVehicle(long id)
        {
            var typeToDelete = await _vehiclesRepository.FindVehicleById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _vehiclesRepository.DeleteVehicle(typeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteVehicleTie(long id)
        {
            var typeToDelete = await _vehiclesRepository.FindVehicleTieById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _vehiclesRepository.DeleteVehicleTie(typeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllVehicles()
        {
            var vehicles = await _vehiclesRepository.FindAllVehicles();
            if (vehicles == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleTransferDTO>>(vehicles);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllVehicleTies()
        {
            var vehicles = await _vehiclesRepository.FindAllVehicleTies();
            if (vehicles == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleSMORoutesResourceTieTransferDTO>>(vehicles);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleById(long id)
        {
            var vehicle = await _vehiclesRepository.FindVehicleById(id);
            if (vehicle == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<VehicleTransferDTO>(vehicle);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleTieById(long id)
        {
            var vehicle = await _vehiclesRepository.FindVehicleTieById(id);
            if (vehicle == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<VehicleSMORoutesResourceTieTransferDTO>(vehicle);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateVehicle(HttpContext context, long id, VehicleReceivingDTO vehicleReceivingDTO)
        {
            var ToUpdate = await _vehiclesRepository.FindVehicleById(id);
            if (ToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {ToUpdate.ToString()} \n";

            ToUpdate.AttachedBranchId = vehicleReceivingDTO.AttachedBranchId;
            ToUpdate.AttachedOfficeId = vehicleReceivingDTO.AttachedOfficeId;
            //ToUpdate.SupplierServiceId = vehicleReceivingDTO.SupplierServiceId;
            ToUpdate.VehicleTypeId = vehicleReceivingDTO.VehicleTypeId;
            ToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _vehiclesRepository.UpdateVehicle(ToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var typeTransferDTOs = _mapper.Map<VehicleTransferDTO>(updatedType);
            return new ApiOkResponse(typeTransferDTOs);
        }
    }
}
