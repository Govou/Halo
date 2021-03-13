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
        public async  Task<ApiResponse> AddProcessesRequiringApproval(HttpContext context, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO)
        {
            var processesRequiringApproval = _mapper.Map<ProcessesRequiringApproval>(processesRequiringApprovalReceivingDTO);
            processesRequiringApproval.CreatedById = context.GetLoggedInUserId();
            var savedProcessesRequiringApproval = await _processesRequiringApprovalRepo.SaveProcessesRequiringApproval(processesRequiringApproval);
            if (savedProcessesRequiringApproval == null)
            {
                return new ApiResponse(500);
            }
            var processesRequiringApprovalTransferDTO = _mapper.Map<ProcessesRequiringApprovalTransferDTO>(processesRequiringApproval);
            return new ApiOkResponse(processesRequiringApprovalTransferDTO);
        }

        public async Task<ApiResponse> DeleteProcessesRequiringApproval(long id)
        {
            var processesRequiringApprovalToDelete = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalById(id);
            if(processesRequiringApprovalToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _processesRequiringApprovalRepo.DeleteProcessesRequiringApproval(processesRequiringApprovalToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllProcessesRequiringApproval()
        {
            var processesRequiringApproval = await _processesRequiringApprovalRepo.GetProcessesRequiringApprovals();
            if (processesRequiringApproval == null)
            {
                return new ApiResponse(404);
            }
            var processesRequiringApprovalTransferDTO = _mapper.Map<IEnumerable<ProcessesRequiringApprovalTransferDTO>>(processesRequiringApproval);
            return new ApiOkResponse(processesRequiringApprovalTransferDTO);
        }

        public  async Task<ApiResponse> UpdateProcessesRequiringApproval(HttpContext context, long id, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO)
        {
            var processesRequiringApprovalToUpdate = await _processesRequiringApprovalRepo.FindProcessesRequiringApprovalById(id);
            if (processesRequiringApprovalToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {processesRequiringApprovalToUpdate.ToString()} \n" ;

            processesRequiringApprovalToUpdate.Caption = processesRequiringApprovalReceivingDTO.Caption;
            processesRequiringApprovalToUpdate.Description = processesRequiringApprovalReceivingDTO.Description;
            var updatedProcessesRequiringApproval = await _processesRequiringApprovalRepo.UpdateProcessesRequiringApproval(processesRequiringApprovalToUpdate);

            summary += $"Details after change, \n {updatedProcessesRequiringApproval.ToString()} \n";

            if (updatedProcessesRequiringApproval == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ProcessesRequiringApproval",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedProcessesRequiringApproval.Id
            };

            await _historyRepo.SaveHistory(history);

            var processesRequiringApprovalTransferDTOs = _mapper.Map<ProcessesRequiringApprovalTransferDTO>(updatedProcessesRequiringApproval);
            return new ApiOkResponse(processesRequiringApprovalTransferDTOs);
        }
    }
}