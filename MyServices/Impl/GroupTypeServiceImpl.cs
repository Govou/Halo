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
    public class GroupTypeServiceImpl : IGroupTypeService
    {
        private readonly ILogger<GroupTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IGroupTypeRepository _groupTypeRepo;
        private readonly IMapper _mapper;

        public GroupTypeServiceImpl(IModificationHistoryRepository historyRepo, IGroupTypeRepository GroupTypeRepo, ILogger<GroupTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._groupTypeRepo = GroupTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddGroupType(HttpContext context, GroupTypeReceivingDTO groupTypeReceivingDTO)
        {
            var groupType = _mapper.Map<GroupType>(groupTypeReceivingDTO);
            groupType.CreatedById = context.GetLoggedInUserId();
            var savedGroupType = await _groupTypeRepo.SaveGroupType(groupType);
            if (savedGroupType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var groupTypeTransferDTO = _mapper.Map<GroupTypeTransferDTO>(groupType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,groupTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllGroupType()
        {
            var groupTypes = await _groupTypeRepo.FindAllGroupType();
            if (groupTypes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var groupTypeTransferDTO = _mapper.Map<IEnumerable<GroupTypeTransferDTO>>(groupTypes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,groupTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetGroupTypeById(long id)
        {
            var groupType = await _groupTypeRepo.FindGroupTypeById(id);
            if (groupType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var groupTypeTransferDTOs = _mapper.Map<GroupTypeTransferDTO>(groupType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,groupTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetGroupTypeByName(string name)
        {
            var groupType = await _groupTypeRepo.FindGroupTypeByName(name);
            if (groupType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var groupTypeTransferDTOs = _mapper.Map<GroupTypeTransferDTO>(groupType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,groupTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateGroupType(HttpContext context, long id, GroupTypeReceivingDTO groupTypeReceivingDTO)
        {
            var groupTypeToUpdate = await _groupTypeRepo.FindGroupTypeById(id);
            if (groupTypeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {groupTypeToUpdate.ToString()} \n" ;

            groupTypeToUpdate.Caption = groupTypeReceivingDTO.Caption;
            groupTypeToUpdate.Description = groupTypeReceivingDTO.Description;
            var updatedGroupType = await _groupTypeRepo.UpdateGroupType(groupTypeToUpdate);

            summary += $"Details after change, \n {updatedGroupType.ToString()} \n";

            if (updatedGroupType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "GroupType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedGroupType.Id
            };

            await _historyRepo.SaveHistory(history);

            var groupTypeTransferDTOs = _mapper.Map<GroupTypeTransferDTO>(updatedGroupType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,groupTypeTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteGroupType(long id)
        {
            var groupTypeToDelete = await _groupTypeRepo.FindGroupTypeById(id);
            if (groupTypeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _groupTypeRepo.DeleteGroupType(groupTypeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}