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
    public class RequredServiceQualificationElementServiceImpl : IRequredServiceQualificationElementService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IRequredServiceQualificationElementRepository _RequredServiceQualificationElementRepo;
        private readonly IMapper _mapper;

        public RequredServiceQualificationElementServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IRequredServiceQualificationElementRepository _RequredServiceQualificationElementRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._RequredServiceQualificationElementRepo = _RequredServiceQualificationElementRepo;
        }

        public async Task<ApiResponse> AddRequredServiceQualificationElement(HttpContext context, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO)
        {
            var RequredServiceQualificationElement = _mapper.Map<RequredServiceQualificationElement>(RequredServiceQualificationElementReceivingDTO);
            RequredServiceQualificationElement.CreatedById = context.GetLoggedInUserId();
            var savedRequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.SaveRequredServiceQualificationElement(RequredServiceQualificationElement);
            if (savedRequredServiceQualificationElement == null)
            {
                return new ApiResponse(500);
            }
            var RequredServiceQualificationElementTransferDTO = _mapper.Map<RequredServiceQualificationElementTransferDTO>(RequredServiceQualificationElement);
            return new ApiOkResponse(RequredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiResponse> GetAllRequredServiceQualificationElements()
        {
            var requredServiceQualificationElements = await _RequredServiceQualificationElementRepo.FindAllRequredServiceQualificationElements();
            if (requredServiceQualificationElements == null)
            {
                return new ApiResponse(404);
            }
            var requredServiceQualificationElementTransferDTO = _mapper.Map<IEnumerable<RequredServiceQualificationElementTransferDTO>>(requredServiceQualificationElements);
            return new ApiOkResponse(requredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiResponse> GetRequredServiceQualificationElementById(long id)
        {
            var requredServiceQualificationElement = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);
            if (requredServiceQualificationElement == null)
            {
                return new ApiResponse(404);
            }
            var requredServiceQualificationElementTransferDTO = _mapper.Map<RequredServiceQualificationElementTransferDTO>(requredServiceQualificationElement);
            return new ApiOkResponse(requredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiResponse> GetRequredServiceQualificationElementByName(string name)
        {
            var RequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementByName(name);
            if (RequredServiceQualificationElement == null)
            {
                return new ApiResponse(404);
            }
            var RequredServiceQualificationElementTransferDTOs = _mapper.Map<RequredServiceQualificationElementTransferDTO>(RequredServiceQualificationElement);
            return new ApiOkResponse(RequredServiceQualificationElementTransferDTOs);
        }

        public async Task<ApiResponse> UpdateRequredServiceQualificationElement(HttpContext context, long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO)
        {
            var RequredServiceQualificationElementToUpdate = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);
            if (RequredServiceQualificationElementToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {RequredServiceQualificationElementToUpdate.ToString()} \n";

            RequredServiceQualificationElementToUpdate.Caption = RequredServiceQualificationElementReceivingDTO.Caption;
            RequredServiceQualificationElementToUpdate.Description = RequredServiceQualificationElementReceivingDTO.Description;
            RequredServiceQualificationElementToUpdate.Type = RequredServiceQualificationElementReceivingDTO.Type;
            var updatedRequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.UpdateRequredServiceQualificationElement(RequredServiceQualificationElementToUpdate);

            summary += $"Details after change, \n {updatedRequredServiceQualificationElement.ToString()} \n";

            if (updatedRequredServiceQualificationElement == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "RequredServiceQualificationElement",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRequredServiceQualificationElement.Id
            };

            await _historyRepo.SaveHistory(history);

            var RequredServiceQualificationElementTransferDTOs = _mapper.Map<RequredServiceQualificationElementTransferDTO>(updatedRequredServiceQualificationElement);
            return new ApiOkResponse(RequredServiceQualificationElementTransferDTOs);

        }

        public async Task<ApiResponse> DeleteRequredServiceQualificationElement(long id)
        {
            var RequredServiceQualificationElementToDelete = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);

            if (RequredServiceQualificationElementToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _RequredServiceQualificationElementRepo.DeleteRequredServiceQualificationElement(RequredServiceQualificationElementToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}
