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
    public class MakeServiceImpl : IMakeService
    {
        private readonly ILogger<MakeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IMakeRepository _makeRepo;
        private readonly IMapper _mapper;

        public MakeServiceImpl(IModificationHistoryRepository historyRepo, IMakeRepository makeRepo, ILogger<MakeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._makeRepo = makeRepo;
            this._logger = logger;
        }
        public async Task<ApiCommonResponse> AddMake(HttpContext context, MakeReceivingDTO makeReceivingDTO)
        {
            var makeService = _mapper.Map<Make>(makeReceivingDTO);
            makeService.CreatedById = context.GetLoggedInUserId();
            

            //check for duplicates before saving
            List<IValidation> duplicates = await _makeRepo.ValidateMake(makeService.Caption);

            if (duplicates.Count == 0)
            {
                var savedMake = await _makeRepo.SaveMake(makeService);
                if (savedMake == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                var makeTransferDTO = _mapper.Map<MakeTransferDTO>(makeService);
                return CommonResponse.Send(ResponseCodes.SUCCESS, makeTransferDTO);
            }

            string msg = "";

            for (var a = 0; a < duplicates.Count; a++)
            {
                msg += $"{duplicates[a].Message}, ";
            }
            msg.TrimEnd(',');

            return CommonResponse.Send(ResponseCodes.FAILURE, null, msg);



        }

        public async Task<ApiCommonResponse> DeleteMake(long id)
        {
            var makeToDelete = await _makeRepo.FindMakeById(id);
            if (makeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            if (!await _makeRepo.DeleteMake(makeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetMakeById(long id)
        {
            var Make = await _makeRepo.FindMakeById(id);
            if (Make == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var MakeTransferDTOs = _mapper.Map<MakeTransferDTO>(Make);
            return CommonResponse.Send(ResponseCodes.SUCCESS, MakeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllMake()
        {
            var make = await _makeRepo.GetMakes();
            if (make == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var makeTransferDTO = _mapper.Map<IEnumerable<MakeTransferDTO>>(make);
            return CommonResponse.Send(ResponseCodes.SUCCESS, makeTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateMake(HttpContext context, long id, MakeReceivingDTO makeReceivingDTO)
        {
            try
            {
                var makeToUpdate = await _makeRepo.FindMakeById(id);
                if (makeToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
                }

                var summary = $"Initial details before change, \n {makeToUpdate.ToString()} \n";

                makeToUpdate.Caption = makeReceivingDTO.Caption;
                makeToUpdate.Description = makeReceivingDTO.Description;
               
                var updatedMake = await _makeRepo.UpdateMake(makeToUpdate);

                
                summary += $"Details after change, \n {updatedMake.ToString()} \n";

                if (updatedMake == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "Make",
                    ChangeSummary = summary,
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = updatedMake.Id
                };

                await _historyRepo.SaveHistory(history);

                var makeTransferDTOs = _mapper.Map<MakeTransferDTO>(updatedMake);
                return CommonResponse.Send(ResponseCodes.SUCCESS, makeTransferDTOs);
            }
            catch (System.Exception error)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, error, "System errors");
            }
        }
    }
}