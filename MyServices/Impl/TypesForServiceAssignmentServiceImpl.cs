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
    public class TypesForServiceAssignmentServiceImpl: ITypesForServiceAssignmentService
    {
        private readonly ITypesForServiceAssignmentRepository _typesForServiceAssignmentRepository;
        private readonly IMapper _mapper;

        public TypesForServiceAssignmentServiceImpl(IMapper mapper, ITypesForServiceAssignmentRepository typesForServiceAssignmentRepository)
        {
            _mapper = mapper;
            _typesForServiceAssignmentRepository = typesForServiceAssignmentRepository;
        }

        public async Task<ApiResponse> AddPassengerType(HttpContext context, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO)
        {
            var type = _mapper.Map<PassengerType>(passengerReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetPassengerName(passengerReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SavePassengerType(type);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<PassengerTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> AddReleaseType(HttpContext context, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO)
        {
            var type = _mapper.Map<ReleaseType>(releaseReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetReleaseName(releaseReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveReleaseType(type);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> AddSourceType(HttpContext context, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO)
        {
            var type = _mapper.Map<SourceType>(sourceReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetSourceName(sourceReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveSourceType(type);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> AddTripType(HttpContext context, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO)
        {
            var type = _mapper.Map<TripType>(tripReceivingDTO);
            var NameExist = _typesForServiceAssignmentRepository.GetName(tripReceivingDTO.TypeName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            type.CreatedById = context.GetLoggedInUserId();
            type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _typesForServiceAssignmentRepository.SaveTripType(type);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> DeletePassengerType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _typesForServiceAssignmentRepository.DeletePassengerType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteReleaseType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteReleaseType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteSourceType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteSourceType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteTripType(long id)
        {
            var typeToDelete = await _typesForServiceAssignmentRepository.FindTripTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _typesForServiceAssignmentRepository.DeleteTripType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllPassengerTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllPassengerTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTypesForServiceAssignmentTransferDTO>>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllReleaseTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllReleaseTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ReleaseTypesForServiceAssignmentTransferDTO>>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllSourceTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllSourceTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<SourceTypesForServiceAssignmentTransferDTO>>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllTripTypes()
        {
            var Type = await _typesForServiceAssignmentRepository.FindAllTripTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<TripTypesForServiceAssignmentTransferDTO>>(Type);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPassengerTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);
            if (type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetReleaseTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);
            if (type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetSourceTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);
            if (type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetTripTypeById(long id)
        {
            var type = await _typesForServiceAssignmentRepository.FindTripTypeById(id);
            if (type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> UpdatePassengerType(HttpContext context, long id, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindPassengerTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = passengerReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdatePassengerType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PassengerTypesForServiceAssignmentTransferDTO>(updatedType);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateReleaseType(HttpContext context, long id, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindReleaseTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = releaseReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateReleaseType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ReleaseTypesForServiceAssignmentTransferDTO>(updatedType);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateSourceType(HttpContext context, long id, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindSourceTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = sourceReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateSourceType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<SourceTypesForServiceAssignmentTransferDTO>(updatedType);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateTripType(HttpContext context, long id, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO)
        {
            var typeToUpdate = await _typesForServiceAssignmentRepository.FindTripTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = tripReceivingDTO.TypeName;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _typesForServiceAssignmentRepository.UpdateTripType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<TripTypesForServiceAssignmentTransferDTO>(updatedType);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
