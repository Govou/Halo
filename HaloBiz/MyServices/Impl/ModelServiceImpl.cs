using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HaloBiz.Model;
using System;
using Halobiz.Common.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class ModelServiceImpl : IModelService
    {
        private readonly ILogger<ModelServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IMapper _mapper;

        public ModelServiceImpl(IModificationHistoryRepository historyRepo, IModelRepository modelRepo, ILogger<ModelServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._modelRepo = modelRepo;
            this._logger = logger;
        }
        public async Task<ApiCommonResponse> AddModel(HttpContext context, ModelReceivingDTO modelReceivingDTO)
        {
            var modelService = _mapper.Map<HalobizMigrations.Models.Model>(modelReceivingDTO);
            modelService.CreatedById = context.GetLoggedInUserId();


            //check for duplicates before saving
            List<IValidation> duplicates = await _modelRepo.ValidateModel(modelService.Caption);

            if (duplicates.Count == 0)
            {
                var savedModel = await _modelRepo.SaveModel(modelService);
                if (savedModel == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                var modelTransferDTO = _mapper.Map<ModelTransferDTO>(modelService);
                return CommonResponse.Send(ResponseCodes.SUCCESS, modelTransferDTO);
            }

            string msg = "";

            for (var a = 0; a < duplicates.Count; a++)
            {
                msg += $"{duplicates[a].Message}, ";
            }
            msg.TrimEnd(',');

            return CommonResponse.Send(ResponseCodes.FAILURE, null, msg);



        }

        public async Task<ApiCommonResponse> DeleteModel(long id)
        {
            var modelToDelete = await _modelRepo.FindModelById(id);
            if (modelToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            if (!await _modelRepo.DeleteModel(modelToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetModelById(long id)
        {
            var Model = await _modelRepo.FindModelById(id);
            if (Model == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var ModelTransferDTOs = _mapper.Map<ModelTransferDTO>(Model);
            return CommonResponse.Send(ResponseCodes.SUCCESS, ModelTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllModelByMake(int makeId)
        {
            var model = await _modelRepo.GetModel(makeId);
            if (model == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var modelTransferDTO = _mapper.Map<IEnumerable<ModelTransferDTO>>(model);
            return CommonResponse.Send(ResponseCodes.SUCCESS, modelTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateModel(HttpContext context, long id, ModelReceivingDTO modelReceivingDTO)
        {
            try
            {
                var modelToUpdate = await _modelRepo.FindModelById(id);
                if (modelToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
                }

                var summary = $"Initial details before change, \n {modelToUpdate.ToString()} \n";

                modelToUpdate.Caption = modelReceivingDTO.Caption;
                modelToUpdate.Description = modelReceivingDTO.Description;
                modelToUpdate.MakeId = modelReceivingDTO.MakeId;

                var updatedModel = await _modelRepo.UpdateModel(modelToUpdate);


                summary += $"Details after change, \n {updatedModel.ToString()} \n";

                if (updatedModel == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "Make",
                    ChangeSummary = summary,
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = updatedModel.Id
                };

                await _historyRepo.SaveHistory(history);

                var modelTransferDTOs = _mapper.Map<ModelTransferDTO>(updatedModel);
                return CommonResponse.Send(ResponseCodes.SUCCESS, modelTransferDTOs);
            }
            catch (System.Exception error)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, error, "System errors");
            }
        }
    }
}