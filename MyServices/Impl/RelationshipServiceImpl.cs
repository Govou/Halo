using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl
{
    public class RelationshipServiceImpl : IRelationshipService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IRelationshipRepository _relationshipRepo;
        private readonly IMapper _mapper;

        public RelationshipServiceImpl(IModificationHistoryRepository historyRepo, IRelationshipRepository RelationshipRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._relationshipRepo = RelationshipRepo;
        }

        public async Task<ApiResponse> AddRelationship(HttpContext context, RelationshipReceivingDTO relationshipReceivingDTO)
        {
            var relationship = _mapper.Map<Relationship>(relationshipReceivingDTO);
            relationship.CreatedById = context.GetLoggedInUserId();
            var savedRelationship = await _relationshipRepo.SaveRelationship(relationship);
            if (savedRelationship == null)
            {
                return new ApiResponse(500);
            }
            var relationshipTransferDTO = _mapper.Map<RelationshipTransferDTO>(relationship);
            return new ApiOkResponse(relationshipTransferDTO);
        }

        public async Task<ApiResponse> GetAllRelationship()
        {
            var relationships = await _relationshipRepo.FindAllRelationship();
            if (relationships == null)
            {
                return new ApiResponse(404);
            }
            var relationshipTransferDTO = _mapper.Map<IEnumerable<RelationshipTransferDTO>>(relationships);
            return new ApiOkResponse(relationshipTransferDTO);
        }

        public async Task<ApiResponse> GetRelationshipById(long id)
        {
            var relationship = await _relationshipRepo.FindRelationshipById(id);
            if (relationship == null)
            {
                return new ApiResponse(404);
            }
            var relationshipTransferDTOs = _mapper.Map<RelationshipTransferDTO>(relationship);
            return new ApiOkResponse(relationshipTransferDTOs);
        }

        public async Task<ApiResponse> GetRelationshipByName(string name)
        {
            var relationship = await _relationshipRepo.FindRelationshipByName(name);
            if (relationship == null)
            {
                return new ApiResponse(404);
            }
            var relationshipTransferDTOs = _mapper.Map<RelationshipTransferDTO>(relationship);
            return new ApiOkResponse(relationshipTransferDTOs);
        }

        public async Task<ApiResponse> UpdateRelationship(HttpContext context, long id, RelationshipReceivingDTO relationshipReceivingDTO)
        {
            var relationshipToUpdate = await _relationshipRepo.FindRelationshipById(id);
            if (relationshipToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {relationshipToUpdate.ToString()} \n" ;

            relationshipToUpdate.Caption = relationshipReceivingDTO.Caption;
            relationshipToUpdate.Description = relationshipReceivingDTO.Description;
            var updatedRelationship = await _relationshipRepo.UpdateRelationship(relationshipToUpdate);

            summary += $"Details after change, \n {updatedRelationship.ToString()} \n";

            if (updatedRelationship == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Relationship",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRelationship.Id
            };

            await _historyRepo.SaveHistory(history);

            var relationshipTransferDTO = _mapper.Map<RelationshipTransferDTO>(updatedRelationship);
            return new ApiOkResponse(relationshipTransferDTO);

        }

        public async Task<ApiResponse> DeleteRelationship(long id)
        {
            var relationshipToDelete = await _relationshipRepo.FindRelationshipById(id);
            if (relationshipToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _relationshipRepo.DeleteRelationship(relationshipToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}