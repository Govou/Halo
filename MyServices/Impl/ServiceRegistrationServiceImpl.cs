using AutoMapper;
using HaloBiz.DTOs;
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
    public class ServiceRegistrationServiceImpl:IServiceRegistrationService
    {
        private readonly IServiceRegistrationRepository _serviceregRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IArmedEscortsRepository _armedEscortsRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly ICommanderRepository _commanderRepository;
        private readonly IMapper _mapper;

        public ServiceRegistrationServiceImpl(ICommanderRepository commanderRepository, IPilotRepository pilotRepository, IArmedEscortsRepository armedEscortsRepository, IVehiclesRepository vehiclesRepository,IMapper mapper, IServiceRegistrationRepository serviceregRepository)
        {
            _mapper = mapper;
            _serviceregRepository = serviceregRepository;
            _commanderRepository = commanderRepository;
            _pilotRepository = pilotRepository;
            _armedEscortsRepository = armedEscortsRepository;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<ApiResponse> AddServiceReg(HttpContext context, ServiceRegistrationReceivingDTO serviceRegReceivingDTO)
        {
            var service = _mapper.Map<ServiceRegistration>(serviceRegReceivingDTO);
            var IdExist = _serviceregRepository.GetserviceId(serviceRegReceivingDTO.ServiceId);
            if (IdExist != null)
            {
                return new ApiResponse(409);
            }

            service.CreatedById = context.GetLoggedInUserId();
            service.CreatedAt = DateTime.UtcNow;
            var savedType = await _serviceregRepository.SaveService(service);
            if (savedType == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<ServiceRegistrationTransferDTO>(service);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddUpArmedEscortType(HttpContext context, long id, AEscortTypeRegReceivingDTO armedEscortTypeReceivingDTO)
        {
            //var summary = "";
            var pair = new ArmedEscortType();
            var itemToUpdate = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            for (int i = 0; i < armedEscortTypeReceivingDTO.ServiceRegistrationId.Length; i++)
            {
                //pair.Id = 0;
                //pair.BusinessRuleId = bRPairableReceivingDTO.BusinessRuleId;
                itemToUpdate.ServiceRegistrationId = armedEscortTypeReceivingDTO.ServiceRegistrationId[i];
                pair.UpdatedAt = DateTime.UtcNow;

                var updated = await _armedEscortsRepository.UpdateArmedEscortType(itemToUpdate);
                if (updated == null)
                {
                    return new ApiResponse(500);
                }
            }






            return new ApiOkResponse(200);


            //foreach (var item in armedEscortTypeReceivingDTO)
            //{
            //    var itemToUpdate = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            //    if (itemToUpdate == null)
            //    {
            //        return new ApiResponse(404);
            //    }

            //    itemToUpdate.ServiceRegistrationId = item.ServiceRegistrationId;
            //    itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //    summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";
            //    var updated = await _serviceregRepository.UpdateArmedEscortTypes(itemToUpdate);
            //    if (updated == null)
            //    {
            //        return new ApiResponse(500);
            //    }
            //    summary += $"Details after change, \n {updated.ToString()} \n";
            //    //var TransferDTOs = _mapper.Map<BRPairableTransferDTO>(updated);
            //}

            return new ApiOkResponse(200);
        }

        public async Task<ApiResponse> AddUpCommanderType(HttpContext context, long id, CommanderTypeRegReceivingDTO commanderTypeReceivingDTO)
        {
            var summary = "";

            
            //foreach (var item in commanderTypeReceivingDTO)
            //{
            //    var itemToUpdate = await _commanderRepository.FindCommanderTypeById(id);
            //    if (itemToUpdate == null)
            //    {
            //        return new ApiResponse(404);
            //    }

            //    itemToUpdate.ServiceRegistrationId = item.ServiceRegistrationId;
            //    itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //    summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";
            //    var updated = await _serviceregRepository.UpdateCommanderTypess(itemToUpdate);
            //    if (updated == null)
            //    {
            //        return new ApiResponse(500);
            //    }
            //    summary += $"Details after change, \n {updated.ToString()} \n";
            //}

            return new ApiOkResponse(200);
        }

        public async Task<ApiResponse> AddUpPilotType(HttpContext context, long id, PilotTypeRegReceivingDTO pilotTypeReceivingDTO)
        {
            var summary = "";
            
            //foreach (var item in pilotTypeReceivingDTO)
            //{
            //    var itemToUpdate = await _pilotRepository.FindPilotTypeById(id);
            //    if (itemToUpdate == null)
            //    {
            //        return new ApiResponse(404);
            //    }

            //    itemToUpdate.ServiceRegistrationId = item.ServiceRegistrationId;
            //    itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //    summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";
            //    var updated = await _serviceregRepository.UpdatePilotTypes(itemToUpdate);
            //    if (updated == null)
            //    {
            //        return new ApiResponse(500);
            //    }
            //    summary += $"Details after change, \n {updated.ToString()} \n";
            //    //var TransferDTOs = _mapper.Map<BRPairableTransferDTO>(updated);
            //}

            return new ApiOkResponse(200);
        }

        public async Task<ApiResponse> AddUpVehicleType(HttpContext context, long id, VehicleTypeRegReceivingDTO vehicleTypeReceivingDTO)
        {

            var summary = "";
            
            //foreach (var item in vehicleTypeReceivingDTO)
            //{
            //    var itemToUpdate = await _vehiclesRepository.FindVehicleTypeById(id);
            //    if (itemToUpdate == null)
            //    {
            //        return new ApiResponse(404);
            //    }

                
            //    itemToUpdate.ServiceRegistrationId = item.ServiceRegistrationId;
            //    itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //    summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";
            //    var updated = await _serviceregRepository.UpdateVehicleTypes(itemToUpdate);
            //    if (updated == null)
            //    {
            //        return new ApiResponse(500);
            //    }
            //    summary += $"Details after change, \n {updated.ToString()} \n";
            //    //var TransferDTOs = _mapper.Map<BRPairableTransferDTO>(updated);
            //}

            return new ApiOkResponse(200);
        }

        public async Task<ApiResponse> DeleteServiceReg(long id)
        {
            var itemToDelete = await _serviceregRepository.FindServiceById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceregRepository.DeleteService(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllServiceRegs()
        {

            var services = await _serviceregRepository.FindAllServicess();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ServiceRegistrationTransferDTO>>(services);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetServiceRegById(long id)
        {

            var service = await _serviceregRepository.FindServiceById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<ServiceRegistrationTransferDTO>(service);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateServiceReg(HttpContext context, long id, ServiceRegistrationReceivingDTO serviceRegReceivingDTO)
        {
            var itemToUpdate = await _serviceregRepository.FindServiceById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.RequiresVehicle = serviceRegReceivingDTO.RequiresVehicle;
            itemToUpdate.RequiresArmedEscort = serviceRegReceivingDTO.RequiresArmedEscort;
            itemToUpdate.RequiresCommander = serviceRegReceivingDTO.RequiresCommander;
            itemToUpdate.RequiresPilot = serviceRegReceivingDTO.RequiresPilot;

            itemToUpdate.VehicleQuantityRequired = serviceRegReceivingDTO.VehicleQuantityRequired;
            itemToUpdate.PilotQuantityRequired = serviceRegReceivingDTO.PilotQuantityRequired;
            itemToUpdate.CommanderQuantityRequired = serviceRegReceivingDTO.CommanderQuantityRequired;
            itemToUpdate.ArmedEscortQuantityRequired = serviceRegReceivingDTO.ArmedEscortQuantityRequired;
          
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updated = await _serviceregRepository.UpdateServices(itemToUpdate);

            summary += $"Details after change, \n {updated.ToString()} \n";

            if (updated == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ServiceRegistrationTransferDTO>(updated);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
