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
    public class DesignationServiceImpl : IDesignationService
    {
        private readonly ILogger<DesignationServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IDesignationRepository _designationRepo;
        private readonly IMapper _mapper;

        public DesignationServiceImpl(IModificationHistoryRepository historyRepo, IDesignationRepository designationRepo, ILogger<DesignationServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._designationRepo = designationRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddDesignation(HttpContext context, DesignationReceivingDTO designationReceivingDTO)
        {
            var designation = _mapper.Map<Designation>(designationReceivingDTO);
            designation.CreatedById = context.GetLoggedInUserId();
            var savedDesignation = await _designationRepo.SaveDesignation(designation);
            if (savedDesignation == null)
            {
                return new ApiResponse(500);
            }
            var designationTransferDTO = _mapper.Map<DesignationTransferDTO>(designation);
            return new ApiOkResponse(designationTransferDTO);
        }

        public async Task<ApiResponse> DeleteDesignation(long id)
        {
            var designationToDelete = await _designationRepo.FindDesignationById(id);
            if(designationToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _designationRepo.DeleteDesignation(designationToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllDesignation()
        {
            var designation = await _designationRepo.GetDesignations();
            if (designation == null)
            {
                return new ApiResponse(404);
            }
            var designationTransferDTO = _mapper.Map<IEnumerable<DesignationTransferDTO>>(designation);
            return new ApiOkResponse(designationTransferDTO);
        }

        public  async Task<ApiResponse> UpdateDesignation(HttpContext context, long id, DesignationReceivingDTO designationReceivingDTO)
        {
            var designationToUpdate = await _designationRepo.FindDesignationById(id);
            if (designationToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {designationToUpdate.ToString()} \n" ;

            designationToUpdate.Caption = designationReceivingDTO.Caption;
            designationToUpdate.Description = designationReceivingDTO.Description;
            var updatedDesignation = await _designationRepo.UpdateDesignation(designationToUpdate);

            summary += $"Details after change, \n {updatedDesignation.ToString()} \n";

            if (updatedDesignation == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Designation",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDesignation.Id
            };

            await _historyRepo.SaveHistory(history);

            var designationTransferDTOs = _mapper.Map<DesignationTransferDTO>(updatedDesignation);
            return new ApiOkResponse(designationTransferDTOs);
        }
    }
}