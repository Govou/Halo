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
    public class JourneyStartandStopServiceImpl: IJourneyStartandStopService
    {
        private readonly IJourneyStartandStopRepository _journeyStartandStopRepository;
        private readonly IServiceAssignmentMasterRepository _serviceAssignmentMasterRepository;
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly IMapper _mapper;

        public JourneyStartandStopServiceImpl(IMapper mapper, IJourneyStartandStopRepository journeyStartandStopRepository, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository)
        {
            _mapper = mapper;
            _journeyStartandStopRepository = journeyStartandStopRepository;
            _serviceAssignmentMasterRepository = serviceAssignmentMasterRepository;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
        }

        public async Task<ApiCommonResponse> AddJourneyIncident(HttpContext context, JourneyIncidentReceivingDTO journeyIncidentReceiving)
        {
            var itemToAdd = _mapper.Map<JourneyIncident>(journeyIncidentReceiving);
            //var NameExist = _armedEscortsRepository.GetTypename(armedEscortTypeReceivingDTO.Name);
            //if (NameExist != null)
            //{
            //    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No record exists");
            //}
            //if(journeyIncidentReceiving.JourneyIncidentType == Fire)
            itemToAdd.CreatedById = context.GetLoggedInUserId();
            //itemToAdd.CreatedBy = context.User()
            itemToAdd.JourneyIncidentTime = DateTime.UtcNow;
            itemToAdd.CreatedAt = DateTime.UtcNow;
            var savedRank = await _journeyStartandStopRepository.SaveJourneyIncident(itemToAdd);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<JourneyIncidentTransferDTO>(itemToAdd);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddJourneyIncidentPic(HttpContext context, JourneyIncidentPictureReceivingDTO journeyIncidentPicReceiving)
        {
            var itemToAdd = _mapper.Map<JourneyIncidentPicture>(journeyIncidentPicReceiving);
           
            itemToAdd.CreatedById = context.GetLoggedInUserId();
            itemToAdd.CreatedAt = DateTime.UtcNow;
            var savedRank = await _journeyStartandStopRepository.SaveJourneyIncidentPic(itemToAdd);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<JourneyIncidentPictureTransferDTO>(itemToAdd);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddJourneyLeadCommander(HttpContext context, JourneyLeadCommanderReceivingDTO journeyLeadCommanderReceiving)
        {
            var itemToAdd = _mapper.Map<JourneyLeadCommander>(journeyLeadCommanderReceiving);

            itemToAdd.CreatedById = context.GetLoggedInUserId();
            itemToAdd.CreatedAt = DateTime.UtcNow;
            itemToAdd.IsActive = true;
            var savedRank = await _journeyStartandStopRepository.SaveJourneyLeadCommander(itemToAdd);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<JourneyLeadCommanderTransferDTO>(itemToAdd);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddJourneyNote(HttpContext context, JourneyNoteReceivingDTO journeyNoteReceiving)
        {
            
                var itemToAdd = _mapper.Map<JourneyNote>(journeyNoteReceiving);

                itemToAdd.CreatedById = context.GetLoggedInUserId();
                itemToAdd.CreatedAt = DateTime.UtcNow;
                var savedRank = await _journeyStartandStopRepository.SaveJourneyNote(itemToAdd);
                if (savedRank == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
                var typeTransferDTO = _mapper.Map<JourneyNoteTransferDTO>(itemToAdd);
                return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
            
        }
        public async Task<ApiCommonResponse> AddSStopJourney(HttpContext context, JourneyStopReceivingDTO journeyStopReceiving)
        {
            var itemToAdd = _mapper.Map<ArmadaJourneyStop>(journeyStopReceiving);

            itemToAdd.CreatedById = context.GetLoggedInUserId();
            itemToAdd.CreatedAt = DateTime.UtcNow;
            itemToAdd.JourneyStopTime = DateTime.UtcNow;
            var savedRank = await _journeyStartandStopRepository.SaveJourneyStop(itemToAdd);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            else
            {
                var itemToUpdate = await _journeyStartandStopRepository.FindJourneyStartById(journeyStopReceiving.JourneyStartId);

                if (itemToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                }

                if (!await _journeyStartandStopRepository.UpdateStopJouneyStart(itemToUpdate))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
            }
            var typeTransferDTO = _mapper.Map<JourneyStopTransferDTO>(itemToAdd);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddStartJourney(HttpContext context, JourneyStartReceivingDTO journeyStartReceiving)
        {
            var itemToAdd = _mapper.Map<ArmadaJourneyStart>(journeyStartReceiving);

            itemToAdd.CreatedById = context.GetLoggedInUserId();
            itemToAdd.IsJourneyStarted = true;
            itemToAdd.JourneyStartDatetime = DateTime.UtcNow;
            itemToAdd.CreatedAt = DateTime.UtcNow;
            var savedRank = await _journeyStartandStopRepository.SaveJourneyStart(itemToAdd);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            else
            {
                var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(journeyStartReceiving.ServiceAssignmentId);

                if (itemToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                }

                if (!await _journeyStartandStopRepository.UpdateCServiceAssignmentOnJouneyStart(itemToUpdate))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
            }
            var typeTransferDTO = _mapper.Map<JourneyStartTransferDTO>(itemToAdd);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteJourneyIncident(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyIncidentById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJouneyIncident(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteJourneyIncidentPic(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyIncidentPicById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJouneyIncidentPic(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteJourneyLeadCommander(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyLeadCommanderById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJourneyLeadCommander(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteJourneyNote(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyNoteById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJourneyNote(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteStartJourney(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyStartById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJouneyStart(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteStopJourney(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyStopById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.DeleteJouneyStop(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllJourneyIncidentPics()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyIncidentPics();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentPictureTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyIncidentPicsByJourneyIncidentId(long incidentId)
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyIncidentPicsByIncidentId(incidentId);
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentPictureTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyIncidents()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyIncidents();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyIncidentsByJourneyStartId(long startId)
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyIncidentsByJourneyStartId(startId);
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyLeadCommanders()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyLeadCommanders();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyLeadCommanderTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyNotes()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJourneyNotes();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyNoteTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyNotesByJourneyStarttId(long startId)
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyNotesByStartId(startId);
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyNoteTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllJourneyStopsByJourneyStartId(long startId)
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyStopsByStartId(startId);
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyStopTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllStartJourneys()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyStarts();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyStartTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllStopJourneys()
        {
            var getAll = await _journeyStartandStopRepository.FindAllJouneyStops();
            if (getAll == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyNoteTransferDTO>>(getAll);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetJourneyIncidentById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyIncidentById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentTransferDTO>>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetJourneyIncidentPicById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyIncidentPicById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyIncidentPictureTransferDTO>>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetJourneyLeadCommanderById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyLeadCommanderById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyLeadCommanderTransferDTO>>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetJourneyNoteById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyNoteById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyNoteTransferDTO>>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetStartJourneyById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyStartById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<JourneyStartTransferDTO>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetStopJourneyById(long id)
        {
            var getItem = await _journeyStartandStopRepository.FindJourneyStopById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var TransferDTO = _mapper.Map<IEnumerable<JourneyStopTransferDTO>>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> RelinquishJourneyLeadCommander(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyLeadCommanderById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.RelinquishLeadCommander(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> UpdateCancelStartJourney(long id)
        {
            var itemToDelete = await _journeyStartandStopRepository.FindJourneyStartById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _journeyStartandStopRepository.UpdateCancelJouneyStart(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> UpdateEndStartJourney(HttpContext context, long id, JourneyEndReceivingDTO journeyEndReceiving)
        {
            var itemToUpdate = await _journeyStartandStopRepository.FindJourneyStartById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.JourneyEndActualAddress = journeyEndReceiving.JourneyEndActualAddress;
            itemToUpdate.JourneyEndActualLatitude = journeyEndReceiving.JourneyEndActualLatitude;
            itemToUpdate.JourneyEndActualLongitude = journeyEndReceiving.JourneyEndActualLongitude;
            
            itemToUpdate.JourneyEndDatetime = DateTime.UtcNow; 
            TimeSpan gethrs = itemToUpdate.JourneyStartDatetime - itemToUpdate.JourneyEndDatetime;
            int hours = (int)gethrs.TotalHours;
            //int min = (int)gethrs.Minutes;
            itemToUpdate.TotalTimeSpentOnJourney = hours;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _journeyStartandStopRepository.UpdateEndJouneyStart(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            else
            {
                var endItemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(journeyEndReceiving.ServiceAssignmentId);
                var escortToDelete = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(journeyEndReceiving.ServiceAssignmentId);
                var commanderToDelete = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(journeyEndReceiving.ServiceAssignmentId);
                var pilotToDelete = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(journeyEndReceiving.ServiceAssignmentId);
                var vehicleToDelete = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(journeyEndReceiving.ServiceAssignmentId);

                if (endItemToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                }

                if (!await _journeyStartandStopRepository.UpdateCServiceAssignmentOnJouneyStartForEndJourney(endItemToUpdate))
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
            }

            //var TransferDTOs = _mapper.Map<JourneyStartTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        //public Task<ApiCommonResponse> RelinquishJourneyNote(long id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ApiCommonResponse> UpdateJourneyIncident(HttpContext context, long id, JourneyIncidentReceivingDTO journeyIncidentReceiving)
        {
            var itemToUpdate = await _journeyStartandStopRepository.FindJourneyIncidentById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.JourneyIncidentCaption = journeyIncidentReceiving.JourneyIncidentCaption;
            itemToUpdate.JourneyIncidentDescription = journeyIncidentReceiving.JourneyIncidentDescription;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _journeyStartandStopRepository.UpdateJourneyIncident(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<JourneyIncidentTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }

        //public async Task<ApiCommonResponse> UpdateServiceAssignmentOnStartJourney(long serviceAssignmentId)
        //{
        //    //var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(serviceAssignmentId);

        //    //if (itemToDelete == null)
        //    //{
        //    //    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
        //    //}

        //    //if (!await _journeyStartandStopRepository.UpdateEndJouneyStart(itemToDelete))
        //    //{
        //    //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
        //    //}

        //    //return CommonResponse.Send(ResponseCodes.SUCCESS);
        //}

        public async Task<ApiCommonResponse> UpdateStopJourney(HttpContext context, long id, JourneyStopReceivingDTO journeyStopReceiving)
        {
            var itemToUpdate = await _journeyStartandStopRepository.FindJourneyStopById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.JourneyStopCaption = journeyStopReceiving.JourneyStopCaption;
            itemToUpdate.JourneyStopDescription = journeyStopReceiving.JourneyStopDescription;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _journeyStartandStopRepository.UpdateJourneyStop(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<JourneyStopTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }
    }
}
