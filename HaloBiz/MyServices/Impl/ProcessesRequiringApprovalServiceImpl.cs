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
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class ProcessesRequiringApprovalServiceImpl : IProcessesRequiringApprovalService
    {
        private readonly ILogger<ProcessesRequiringApprovalServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IProcessesRequiringApprovalRepository _processesRequiringApprovalRepo;
        private readonly IMapper _mapper;

        public ProcessesRequiringApprovalServiceImpl(IModificationHistoryRepository historyRepo, IProcessesRequiringApprovalRepository processesRequiringApprovalRepo, ILogger<ProcessesRequiringApprovalServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._processesRequiringApprovalRepo = processesRequiringApprovalRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddProcessesRequiringApproval(HttpContext context, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO)
        {
            var processesRequiringApproval = _mapper.Map<ProcessesRequiringApproval>(processesRequiringApprovalReceivingDTO);
            processesRequiringApproval.CreatedById = context.GetLoggedInUserId();
            var savedProcessesRequiringApproval = await _processesRequiringApprovalRepo.SaveProcessesRequiringApproval(processesRequiringApproval);
            if (savedProcessesRequiringApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var processesRequiringApprovalTransferDTO = _mapper.Map<ProcessesRequiringApprovalTransferDTO>(processesRequiringApproval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,processesRequiringApprovalTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteProcessesRequiringApproval(long id)
        {
            var processesRequiringApprovalToDelete = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalById(id);
            if(processesRequiringApprovalToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _processesRequiringApprovalRepo.DeleteProcessesRequiringApproval(processesRequiringApprovalToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllProcessesRequiringApproval()
        {
            var processesRequiringApproval = await _processesRequiringApprovalRepo.GetProcessesRequiringApprovals();
            if (processesRequiringApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var processesRequiringApprovalTransferDTO = _mapper.Map<IEnumerable<ProcessesRequiringApprovalTransferDTO>>(processesRequiringApproval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,processesRequiringApprovalTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateProcessesRequiringApproval(HttpContext context, long id, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO)
        {
            var processesRequiringApprovalToUpdate = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalById(id);
            if (processesRequiringApprovalToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {processesRequiringApprovalToUpdate.ToString()} \n" ;

            processesRequiringApprovalToUpdate.Caption = processesRequiringApprovalReceivingDTO.Caption;
            processesRequiringApprovalToUpdate.Description = processesRequiringApprovalReceivingDTO.Description;
            var updatedProcessesRequiringApproval = await _processesRequiringApprovalRepo.UpdateProcessesRequiringApproval(processesRequiringApprovalToUpdate);

            summary += $"Details after change, \n {updatedProcessesRequiringApproval.ToString()} \n";

            if (updatedProcessesRequiringApproval == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ProcessesRequiringApproval",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedProcessesRequiringApproval.Id
            };

            await _historyRepo.SaveHistory(history);

            var processesRequiringApprovalTransferDTOs = _mapper.Map<ProcessesRequiringApprovalTransferDTO>(updatedProcessesRequiringApproval);
            return CommonResponse.Send(ResponseCodes.SUCCESS,processesRequiringApprovalTransferDTOs);
        }
    }
}