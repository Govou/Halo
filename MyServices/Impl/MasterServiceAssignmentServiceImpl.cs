using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
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
    public class MasterServiceAssignmentServiceImpl: IMasterServiceAssignmentService
    {
        private readonly IServiceAssignmentMasterRepository _serviceAssignmentMasterRepository;
        private readonly IServiceRegistrationRepository _serviceRegistrationRepository;
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly ICustomerDivisionRepository _CustomerDivisionRepo;
        private readonly IMapper _mapper;

        public MasterServiceAssignmentServiceImpl(IMapper mapper, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, 
            IServiceRegistrationRepository serviceRegistrationRepository, IDTSMastersRepository dTSMastersRepository, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository, ICustomerDivisionRepository CustomerDivisionRepo)
        {
            _mapper = mapper;
            _serviceAssignmentMasterRepository = serviceAssignmentMasterRepository;
            _serviceRegistrationRepository = serviceRegistrationRepository;
            _dTSMastersRepository = dTSMastersRepository;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
            _CustomerDivisionRepo = CustomerDivisionRepo;

        }

        public async Task<ApiCommonResponse> AddMasterServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var master = _mapper.Map<MasterServiceAssignment>(masterReceivingDTO);
            DateTime pickofftime = Convert.ToDateTime(masterReceivingDTO.PickoffTime.AddHours(1));
            pickofftime = pickofftime.AddSeconds(-1 * pickofftime.Second);
            pickofftime = pickofftime.AddMilliseconds(-1 * pickofftime.Millisecond);
            var getRegService = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
            long getId = 0;
           
            //var NameExist = _armedEscortsRepository.GetTypename(armedEscortTypeReceivingDTO.Name);
            //if (NameExist != null)
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
            //}
         
            //master.CreatedBy = context.User.Claims();
            master.PickoffTime = pickofftime;
            master.CreatedAt = DateTime.UtcNow;
            master.TripTypeId = 1;
            master.SAExecutionStatus = 0;
            master.AssignmentStatus = "Open";
            var savedRank = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
             getId = savedRank.Id;
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            if(masterReceivingDTO.IsReturnJourney == true)
            {
                master.Id = 0;
                master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                master.TripTypeId = 2;
                master.SAExecutionStatus = 0;
                master.PickoffTime = pickofftime;
                master.AssignmentStatus = "open";
                master.PrimaryTripAssignmentId = getId ;
                //master.CreatedById = context.GetLoggedInUserId();
                master.CreatedAt = DateTime.UtcNow;
                 var savedItem = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
                if (savedItem == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
            }
            //var typeTransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteMasterServiceAssignment(long id)
        {

            var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            var escortToDelete = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(id);
            var commanderToDelete = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(id);
            var pilotToDelete = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(id);
            var vehicleToDelete = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(id);
            var passengerToDelete = await _serviceAssignmentDetailsRepository.FindAllPassengersByAssignmentId(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.DeleteServiceAssignment(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            if (escortToDelete.Count() != 0)
            {
                foreach (var item in escortToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteEscortServiceAssignmentDetail(item);
                }
                
            }
            if (commanderToDelete.Count() != 0)
            {
                foreach (var item in commanderToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteCommanderServiceAssignmentDetail(item);
                }
            }
            if (pilotToDelete.Count() != 0)
            {
                foreach (var item in pilotToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeletePilotServiceAssignmentDetail(item);
                }
            }
            if (vehicleToDelete.Count() != 0)
            {
                foreach (var item in vehicleToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteVehicleServiceAssignmentDetail(item);
                }
            }
            if (passengerToDelete.Count() != 0)
            {
                foreach (var item in passengerToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeletePassenger(item);
                }
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllCustomerDivisions()
        {
            var CustomerDivisions = await _serviceAssignmentMasterRepository.FindAllCustomerDivision();
            if (CustomerDivisions == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            // var CustomerDivisionTransferDTOs = _mapper.Map<IEnumerable<CustomerDivisionTransferDTO>>(CustomerDivisions);
            return CommonResponse.Send(ResponseCodes.SUCCESS, CustomerDivisions);
        }

        public async Task<ApiCommonResponse> GetAllMasterServiceAssignments()
        {
            var master = await _serviceAssignmentMasterRepository.FindAllServiceAssignments();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetMasterServiceAssignmentById(long id)
        {
            var master = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.DropoffLocation = masterReceivingDTO.DropoffLocation;
            itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            itemToUpdate.ServiceRegistrationId = masterReceivingDTO.ServiceRegistrationId;

            itemToUpdate.PickoffTime = masterReceivingDTO.PickoffTime;
            itemToUpdate.CustomerDivisionId = masterReceivingDTO.CustomerDivisionId;
            //itemToUpdate.TripTypeId = masterReceivingDTO.TripTypeId;
            itemToUpdate.SMORouteId = masterReceivingDTO.SMORouteId;
            itemToUpdate.SMORegionId = masterReceivingDTO.SMORegionId;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentMasterRepository.UpdateServiceAssignment(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<MasterServiceAssignmentTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateReadyStatus(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }
    }
}
