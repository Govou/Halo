using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl
{
    public class RequiredServiceDocumentServiceImpl : IRequiredServiceDocumentService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IRequiredServiceDocumentRepository _requiredServiceDocumentRepo;
        private readonly IMapper _mapper;
        private readonly IServiceRequiredServiceDocumentRepository _servicerequiredServiceDocRepo;

        public RequiredServiceDocumentServiceImpl(IModificationHistoryRepository historyRepo, IRequiredServiceDocumentRepository requiredServiceDocumentRepo, IMapper mapper,  IServiceRequiredServiceDocumentRepository servicerequiredServiceDocRepo)
        {
            this._mapper = mapper;
            this._servicerequiredServiceDocRepo = servicerequiredServiceDocRepo;
            this._historyRepo = historyRepo;
            this._requiredServiceDocumentRepo = requiredServiceDocumentRepo;
        }

        public async Task<ApiResponse> AddRequiredServiceDocument(HttpContext context, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO)
        {
            var requiredServiceDocument = _mapper.Map<RequiredServiceDocument>(requiredServiceDocumentReceivingDTO);
            requiredServiceDocument.CreatedById = context.GetLoggedInUserId();
            var savedRequiredServiceDocument = await _requiredServiceDocumentRepo.SaveRequiredServiceDocument(requiredServiceDocument);
            if (savedRequiredServiceDocument == null)
            {
                return new ApiResponse(500);
            }
            var requiredServiceDocumentTransferDTO = _mapper.Map<RequiredServiceDocumentTransferDTO>(requiredServiceDocument);
            return new ApiOkResponse(requiredServiceDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetAllRequiredServiceDocument()
        {
            var requiredServiceDocuments = await _requiredServiceDocumentRepo.FindAllRequiredServiceDocuments();
            if (requiredServiceDocuments == null)
            {
                return new ApiResponse(404);
            }
            var requiredServiceDocumentTransferDTO = _mapper.Map<IEnumerable<RequiredServiceDocumentTransferDTO>>(requiredServiceDocuments);
            return new ApiOkResponse(requiredServiceDocumentTransferDTO);
        }

        public async Task<ApiResponse> GetRequiredServiceDocumentById(long id)
        {
            var requiredServiceDocument = await _requiredServiceDocumentRepo.FindRequiredServiceDocumentById(id);
            if (requiredServiceDocument == null)
            {
                return new ApiResponse(404);
            }
            var requiredServiceDocumentTransferDTOs = _mapper.Map<RequiredServiceDocumentTransferDTO>(requiredServiceDocument);
            return new ApiOkResponse(requiredServiceDocumentTransferDTOs);
        }

        public async Task<ApiResponse> GetRequiredServiceDocumentByName(string name)
        {
            var requiredServiceDocument = await _requiredServiceDocumentRepo.FindRequiredServiceDocumentByName(name);
            if (requiredServiceDocument == null)
            {
                return new ApiResponse(404);
            }
            var requiredServiceDocumentTransferDTOs = _mapper.Map<RequiredServiceDocumentTransferDTO>(requiredServiceDocument);
            return new ApiOkResponse(requiredServiceDocumentTransferDTOs);
        }

        public async Task<ApiResponse> UpdateRequiredServiceDocument(HttpContext context, long id, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceivingDTO)
        {
            var requiredServiceDocumentToUpdate = await _requiredServiceDocumentRepo.FindRequiredServiceDocumentById(id);
            if (requiredServiceDocumentToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {requiredServiceDocumentToUpdate.ToString()} \n" ;

            requiredServiceDocumentToUpdate.Caption = requiredServiceDocumentReceivingDTO.Caption;
            requiredServiceDocumentToUpdate.Description = requiredServiceDocumentReceivingDTO.Description;
            requiredServiceDocumentToUpdate.Type = requiredServiceDocumentReceivingDTO.Type;
            var updatedRequiredServiceDocument = await _requiredServiceDocumentRepo.UpdateRequiredServiceDocument(requiredServiceDocumentToUpdate);

            summary += $"Details after change, \n {updatedRequiredServiceDocument.ToString()} \n";

            if (updatedRequiredServiceDocument == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "RequiredServiceDocument",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRequiredServiceDocument.Id
            };

            await _historyRepo.SaveHistory(history);

            var requiredServiceDocumentTransferDTO = _mapper.Map<RequiredServiceDocumentTransferDTO>(updatedRequiredServiceDocument);
            return new ApiOkResponse(requiredServiceDocumentTransferDTO);

        }

        public async Task<ApiResponse> DeleteRequiredServiceDocument(long id)
        {
            IList<ServiceRequiredServiceDocument> serviceRequiredServiceDocument = new List<ServiceRequiredServiceDocument>();

            var requiredServiceDocumentToDelete = await _requiredServiceDocumentRepo.FindRequiredServiceDocumentById(id);
            if (requiredServiceDocumentToDelete == null)
            {
                return new ApiResponse(404);
            }
            

            await _servicerequiredServiceDocRepo.DeleteRangeServiceRequiredServiceDocument(requiredServiceDocumentToDelete.Services);

            if (!await _requiredServiceDocumentRepo.DeleteRequiredServiceDocument(requiredServiceDocumentToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}