using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ProspectServiceImpl : IProspectService
    {
        private readonly ILogger<ProspectServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IProspectRepository _prospectRepo;
        private readonly IMapper _mapper;

        public ProspectServiceImpl(IModificationHistoryRepository historyRepo, IProspectRepository ProspectRepo, ILogger<ProspectServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._prospectRepo = ProspectRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddProspect(HttpContext context, ProspectReceivingDTO prospectReceivingDTO)
        {

            var prospect = _mapper.Map<Prospect>(prospectReceivingDTO);
            prospect.CreatedById = context.GetLoggedInUserId();
            var savedprospect = await _prospectRepo.SaveProspect(prospect);
            if (savedprospect == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var prospectTransferDTO = _mapper.Map<ProspectTransferDTO>(prospect);
            return new ApiOkResponse(prospectTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteProspect(long id)
        {
            var prospectToDelete = await _prospectRepo.FindProspectById(id);
            if (prospectToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _prospectRepo.DeleteProspect(prospectToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllProspect()
        {
            var prospects = await _prospectRepo.FindAllProspects();
            if (prospects == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var prospectTransferDTO = _mapper.Map<IEnumerable<ProspectTransferDTO>>(prospects);
            return new ApiOkResponse(prospectTransferDTO);
        }

        public async Task<ApiCommonResponse> GetProspectById(long id)
        {
            var prospect = await _prospectRepo.FindProspectById(id);
            if (prospect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var prospectTransferDTOs = _mapper.Map<ProspectTransferDTO>(prospect);
            return new ApiOkResponse(prospectTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetProspectByEmail(string email)
        {
            var prospect = await _prospectRepo.FindProspectByEmail(email);
            if (prospect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var prospectTransferDTOs = _mapper.Map<ProspectTransferDTO>(prospect);
            return new ApiOkResponse(prospectTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateProspect(HttpContext context, long id, ProspectReceivingDTO prospectReceivingDTO)
        {
            var prospectToUpdate = await _prospectRepo.FindProspectById(id);
            if (prospectToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {prospectToUpdate.ToString()} \n";

            prospectToUpdate.Address = prospectReceivingDTO.Address;
            prospectToUpdate.BranchId = prospectReceivingDTO.BranchId;
            prospectToUpdate.CompanyName = prospectReceivingDTO.CompanyName;
            prospectToUpdate.DivisionName = prospectReceivingDTO.DivisionName;
            prospectToUpdate.FirstName = prospectReceivingDTO.FirstName;
            prospectToUpdate.Email = prospectReceivingDTO.Email;
            prospectToUpdate.IdentificationNumber = prospectReceivingDTO.IdentificationNumber;
            prospectToUpdate.Industry = prospectReceivingDTO.Industry;
            prospectToUpdate.LastName = prospectReceivingDTO.LastName;
            prospectToUpdate.LeadId = prospectReceivingDTO.LeadId;
            prospectToUpdate.Lgaid = prospectReceivingDTO.Lgaid;
            prospectToUpdate.LogoUrl = prospectReceivingDTO.LogoUrl;
            prospectToUpdate.MeansOfIdentification = prospectReceivingDTO.MeansOfIdentification;
            prospectToUpdate.MobileNumber = prospectReceivingDTO.MobileNumber;
            prospectToUpdate.OfficeId = prospectReceivingDTO.OfficeId;
            prospectToUpdate.Origin = prospectReceivingDTO.Origin;
            prospectToUpdate.ProspectType = prospectReceivingDTO.ProspectType;
            prospectToUpdate.StateId = prospectReceivingDTO.StateId;
            prospectToUpdate.Street = prospectReceivingDTO.Street;
            prospectToUpdate.TentativeBugdet = prospectReceivingDTO.TentativeBugdet;
            prospectToUpdate.TentativeBusinessStartDate = prospectReceivingDTO.TentativeBusinessStartDate;
            prospectToUpdate.RCNumber = prospectReceivingDTO.RCNumber;

            var updatedprospect = await _prospectRepo.UpdateProspect(prospectToUpdate);

            summary += $"Details after change, \n {updatedprospect.ToString()} \n";

            if (updatedprospect == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "prospect",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedprospect.Id
            };
            await _historyRepo.SaveHistory(history);

            var prospectTransferDTOs = _mapper.Map<ProspectTransferDTO>(updatedprospect);
            return new ApiOkResponse(prospectTransferDTOs);
        }
    }
}
