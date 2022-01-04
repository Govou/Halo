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
    public class MasterServiceAssignmentServiceImpl: IMasterServiceAssignmentService
    {
        private readonly IServiceAssignmentMasterRepository _serviceAssignmentMasterRepository;
        private readonly IServiceRegistrationRepository _serviceRegistrationRepository;
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly IMapper _mapper;

        public MasterServiceAssignmentServiceImpl(IMapper mapper, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, 
            IServiceRegistrationRepository serviceRegistrationRepository, IDTSMastersRepository dTSMastersRepository)
        {
            _mapper = mapper;
            _serviceAssignmentMasterRepository = serviceAssignmentMasterRepository;
            _serviceRegistrationRepository = serviceRegistrationRepository;
            _dTSMastersRepository = dTSMastersRepository;
        }

        public async Task<ApiResponse> AddMasterServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
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
            //    return new ApiResponse(409);
            //}
         
            master.CreatedById = context.GetLoggedInUserId();
            master.PickoffTime = pickofftime;
            master.CreatedAt = DateTime.UtcNow;
            master.TripTypeId = 1;
            var savedRank = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
             getId = savedRank.Id;
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }

            if(masterReceivingDTO.IsReturnJourney == true)
            {
                master.Id = 0;
                master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                master.TripTypeId = 2;
                master.PickoffTime = pickofftime;
                master.PrimaryTripAssignmentId = getId ;
                master.CreatedById = context.GetLoggedInUserId();
                master.CreatedAt = DateTime.UtcNow;
                 var savedItem = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
                if (savedItem == null)
                {
                    return new ApiResponse(500);
                }
            }
            //var typeTransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(master);
            return new ApiOkResponse("Record(s) Added Successfully");
        }

        public async Task<ApiResponse> DeleteMasterServiceAssignment(long id)
        {

            var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentMasterRepository.DeleteServiceAssignment(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllMasterServiceAssignments()
        {
            var master = await _serviceAssignmentMasterRepository.FindAllServiceAssignments();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetMasterServiceAssignmentById(long id)
        {
            var master = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<MasterServiceAssignmentTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateReadyStatus(long id)
        {
            var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}
