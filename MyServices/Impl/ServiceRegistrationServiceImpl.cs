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

        public async Task<ApiResponse> AddResourceRequired(HttpContext context, AllResourceTypesPerServiceRegReceivingDTO ResouredRequiredReceivingDTO)
        {
            var escort = new ArmedEscortResourceRequiredPerService();
            var commander = new CommanderResourceRequiredPerService();
            var pilot = new PilotResourceRequiredPerService();
            var vehicle = new VehicleResourceRequiredPerService();

            for (int i = 0; i < ResouredRequiredReceivingDTO.ArmedEscortTypeId.Length; i++)
            {
                escort.Id = 0;
                escort.ServiceRegistrationId = ResouredRequiredReceivingDTO.ServiceRegistrationId;
                escort.ArmedEscortTypeId = ResouredRequiredReceivingDTO.ArmedEscortTypeId[i];
                var IdExist = _serviceregRepository.GetArmedEscortTypeAndRegServiceId(ResouredRequiredReceivingDTO.ServiceRegistrationId, ResouredRequiredReceivingDTO.ArmedEscortTypeId[i]);
                if (IdExist == null)
                {
                    escort.CreatedById = context.GetLoggedInUserId();
                    escort.CreatedAt = DateTime.UtcNow;

                    var savedType = await _serviceregRepository.SaveArmedEscortResource(escort);
                    if (savedType == null)
                    {
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }

            for (int i = 0; i < ResouredRequiredReceivingDTO.CommanderTypeId.Length; i++)
            {
                commander.Id = 0;
                commander.ServiceRegistrationId = ResouredRequiredReceivingDTO.ServiceRegistrationId;
                commander.CommanderTypeId = ResouredRequiredReceivingDTO.CommanderTypeId[i];
                var IdExist = _serviceregRepository.GetCommanderTypeAndRegServiceId(ResouredRequiredReceivingDTO.ServiceRegistrationId, ResouredRequiredReceivingDTO.CommanderTypeId[i]);
                if (IdExist == null)
                {
                    commander.CreatedById = context.GetLoggedInUserId();
                    commander.CreatedAt = DateTime.UtcNow;

                    var savedType = await _serviceregRepository.SaveCommanderResource(commander);
                    if (savedType == null)
                    {
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }
            for (int i = 0; i < ResouredRequiredReceivingDTO.PilotTypeId.Length; i++)
            {
                pilot.Id = 0;
                pilot.ServiceRegistrationId = ResouredRequiredReceivingDTO.ServiceRegistrationId;
                pilot.PilotTypeId = ResouredRequiredReceivingDTO.PilotTypeId[i];
                var IdExist = _serviceregRepository.GetPilotTypeAndRegServiceId(ResouredRequiredReceivingDTO.ServiceRegistrationId, ResouredRequiredReceivingDTO.PilotTypeId[i]);
                if (IdExist == null)
                {
                    pilot.CreatedById = context.GetLoggedInUserId();
                    pilot.CreatedAt = DateTime.UtcNow;

                    var savedType = await _serviceregRepository.SavePilotResource(pilot);
                    if (savedType == null)
                    {
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }
            for (int i = 0; i < ResouredRequiredReceivingDTO.VehicleTypeId.Length; i++)
            {
                vehicle.Id = 0;
                vehicle.ServiceRegistrationId = ResouredRequiredReceivingDTO.ServiceRegistrationId;
                vehicle.VehicleTypeId = ResouredRequiredReceivingDTO.VehicleTypeId[i];
                var IdExist = _serviceregRepository.GetVehicleTypeAndRegServiceId(ResouredRequiredReceivingDTO.ServiceRegistrationId, ResouredRequiredReceivingDTO.VehicleTypeId[i]);
                if (IdExist == null)
                {
                    vehicle.CreatedById = context.GetLoggedInUserId();
                    vehicle.CreatedAt = DateTime.UtcNow;

                    var savedType = await _serviceregRepository.SaveResourceVehicle(vehicle);
                    if (savedType == null)
                    {
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }

            return new ApiOkResponse("Records Added");
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
            service.Description = "Not required";
            var savedType = await _serviceregRepository.SaveService(service);
            if (savedType == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<ServiceRegistrationTransferDTO>(service);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> DeleteArmedEscortResource(long id)
        {
            var itemToDelete = await _serviceregRepository.FindArmedEscortResourceById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceregRepository.DeleteArmedEscortResource(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderResource(long id)
        {
            var itemToDelete = await _serviceregRepository.FindCommanderResourceById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceregRepository.DeleteCommanderResource(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePilotResource(long id)
        {
            var itemToDelete = await _serviceregRepository.FindPilotResourceById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceregRepository.DeletePilotResource(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
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

        public async Task<ApiResponse> DeleteVehicleResource(long id)
        {
            var itemToDelete = await _serviceregRepository.FindVehicleResourceById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceregRepository.DeleteVehicleResource(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscortResourceRequired()
        {
            var services = await _serviceregRepository.FindAllArmedEscortResources();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortResourceRequiredTransferDTO>>(services);
             
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderResourceRequired()
        {
            var services = await _serviceregRepository.FindAllCommanderResources();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderResourceRequiredTransferDTO>>(services);
             
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotResourceRequired()
        {
            var services = await _serviceregRepository.FindAllPilotResources();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotResourceRequiredTransferDTO>>(services);
            
            return new ApiOkResponse(TransferDTO);
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

        public async Task<ApiResponse> GetAllVehicleResourceRequired()
        {
            var services = await _serviceregRepository.FindAllVehicleResources();
            if (services == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleResourceRequiredTransferDTO>>(services);

            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortResourceById(long id)
        {
            var service = await _serviceregRepository.FindArmedEscortResourceById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<ArmedEscortResourceRequiredTransferDTO>(service);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderResourceById(long id)
        {
            var service = await _serviceregRepository.FindCommanderResourceById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderResourceRequiredTransferDTO>(service);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotResourceById(long id)
        {
            var service = await _serviceregRepository.FindPilotResourceById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotResourceRequiredTransferDTO>(service);
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

        public async Task<ApiResponse> GetVehicleResourceById(long id)
        {
            var service = await _serviceregRepository.FindVehicleResourceById(id);
            if (service == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<VehicleResourceRequiredTransferDTO>(service);
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
            //itemToUpdate.Description = serviceRegReceivingDTO.Description;
          
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
