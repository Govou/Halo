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
    public class TypesForServiceAssignmentServiceImpl: ITypesForServiceAssignmentService
    {
        private readonly ITypesForServiceAssignmentRepository _typesForServiceAssignmentRepository;
        private readonly IMapper _mapper;

        public TypesForServiceAssignmentServiceImpl(IMapper mapper, ITypesForServiceAssignmentRepository typesForServiceAssignmentRepository)
        {
            _mapper = mapper;
            _typesForServiceAssignmentRepository = typesForServiceAssignmentRepository;
        }

        public async Task<ApiCommonResponse> AddPassengerType(HttpContext context, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO)
        {
            var type = _mapper.Map<PassengerType>(passengerReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetPassengerName(passengerReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
                
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SavePassengerType(type);
            if (savedRank == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
               
            }
            var typeTransferDTO = _mapper.Map<PassengerTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddReleaseType(HttpContext context, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO)
        {
            var type = _mapper.Map<ReleaseType>(releaseReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetReleaseName(releaseReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveReleaseType(type);
            if (savedRank == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddSourceType(HttpContext context, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO)
        {
            var type = _mapper.Map<SourceType>(sourceReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetSourceName(sourceReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveSourceType(type);
            if (savedRank == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddTripType(HttpContext context, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO)
        {
            var type = _mapper.Map<TripType>(tripReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetName(tripReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveTripType(type);
            if (savedRank == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var typeTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeletePassengerType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _typesForServiceAssignmentRepository.DeletePassengerType(typeToDelete))
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteReleaseType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteReleaseType(typeToDelete))
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteSourceType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteSourceType(typeToDelete))
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteTripType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindTripTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteTripType(typeToDelete))
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllPassengerTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllPassengerTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTypesForServiceAssignmentTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllReleaseTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllReleaseTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ReleaseTypesForServiceAssignmentTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllSourceTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllSourceTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<SourceTypesForServiceAssignmentTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllTripTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllTripTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<TripTypesForServiceAssignmentTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetPassengerTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);
            if (type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var rankTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetReleaseTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);
            if (type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var rankTransferDTO = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetSourceTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);
            if (type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var rankTransferDTO = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetTripTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindTripTypeById(id);
            if (type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var rankTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return CommonResponse.Send(ResponseCodes.SUCCESS, rankTransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdatePassengerType(HttpContext context, long id, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = passengerReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdatePassengerType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<PassengerTypesForServiceAssignmentTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateReleaseType(HttpContext context, long id, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = releaseReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateReleaseType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateSourceType(HttpContext context, long id, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = sourceReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateSourceType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateTripType(HttpContext context, long id, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindTripTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = tripReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateTripType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }
    }
}
