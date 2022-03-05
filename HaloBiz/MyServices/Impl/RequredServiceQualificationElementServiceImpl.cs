using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
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

        public async Task<ApiCommonResponse> AddRequredServiceQualificationElement(HttpContext context, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO)
        {
            var RequredServiceQualificationElement = _mapper.Map<RequredServiceQualificationElement>(RequredServiceQualificationElementReceivingDTO);
            RequredServiceQualificationElement.CreatedById = context.GetLoggedInUserId();
            var savedRequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.SaveRequredServiceQualificationElement(RequredServiceQualificationElement);
            if (savedRequredServiceQualificationElement == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var RequredServiceQualificationElementTransferDTO = _mapper.Map<RequredServiceQualificationElementTransferDTO>(RequredServiceQualificationElement);
            return CommonResponse.Send(ResponseCodes.SUCCESS,RequredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllRequredServiceQualificationElements()
        {
            var requredServiceQualificationElements = await _RequredServiceQualificationElementRepo.FindAllRequredServiceQualificationElements();
            if (requredServiceQualificationElements == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var requredServiceQualificationElementTransferDTO = _mapper.Map<IEnumerable<RequredServiceQualificationElementTransferDTO>>(requredServiceQualificationElements);
            return CommonResponse.Send(ResponseCodes.SUCCESS,requredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllRequredServiceQualificationElementsByServiceCategory(long serviceCategoryId)
        {
            var requredServiceQualificationElements = await _RequredServiceQualificationElementRepo.FindAllRequredServiceQualificationElementsByServiceCategory(serviceCategoryId);
            if (requredServiceQualificationElements == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var requredServiceQualificationElementTransferDTO = _mapper.Map<IEnumerable<RequredServiceQualificationElementTransferDTO>>(requredServiceQualificationElements);
            return CommonResponse.Send(ResponseCodes.SUCCESS,requredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetRequredServiceQualificationElementById(long id)
        {
            var requredServiceQualificationElement = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);
            if (requredServiceQualificationElement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var requredServiceQualificationElementTransferDTO = _mapper.Map<RequredServiceQualificationElementTransferDTO>(requredServiceQualificationElement);
            return CommonResponse.Send(ResponseCodes.SUCCESS,requredServiceQualificationElementTransferDTO);
        }

        public async Task<ApiCommonResponse> GetRequredServiceQualificationElementByName(string name)
        {
            var RequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementByName(name);
            if (RequredServiceQualificationElement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var RequredServiceQualificationElementTransferDTOs = _mapper.Map<RequredServiceQualificationElementTransferDTO>(RequredServiceQualificationElement);
            return CommonResponse.Send(ResponseCodes.SUCCESS,RequredServiceQualificationElementTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateRequredServiceQualificationElement(HttpContext context, long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO)
        {
            var RequredServiceQualificationElementToUpdate = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);
            if (RequredServiceQualificationElementToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {RequredServiceQualificationElementToUpdate.ToString()} \n";

            RequredServiceQualificationElementToUpdate.Caption = RequredServiceQualificationElementReceivingDTO.Caption;
            RequredServiceQualificationElementToUpdate.Description = RequredServiceQualificationElementReceivingDTO.Description;
            RequredServiceQualificationElementToUpdate.Type = RequredServiceQualificationElementReceivingDTO.Type;
            RequredServiceQualificationElementToUpdate.ServiceCategoryId = RequredServiceQualificationElementReceivingDTO.ServiceCategoryId;
            var updatedRequredServiceQualificationElement = await _RequredServiceQualificationElementRepo.UpdateRequredServiceQualificationElement(RequredServiceQualificationElementToUpdate);

            summary += $"Details after change, \n {updatedRequredServiceQualificationElement.ToString()} \n";

            if (updatedRequredServiceQualificationElement == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,RequredServiceQualificationElementTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteRequredServiceQualificationElement(long id)
        {
            var RequredServiceQualificationElementToDelete = await _RequredServiceQualificationElementRepo.FindRequredServiceQualificationElementById(id);

            if (RequredServiceQualificationElementToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _RequredServiceQualificationElementRepo.DeleteRequredServiceQualificationElement(RequredServiceQualificationElementToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

    }
}
