using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class BranchServiceImpl : IBranchService
    {
        private readonly ILogger<BranchServiceImpl> _logger;
        private readonly IOfficeService _officeService;
        private readonly IBranchRepository _branchRepo;
        private readonly IMapper _mapper;

        public BranchServiceImpl(IOfficeService officeService, IBranchRepository branchRepo, ILogger<BranchServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._officeService = officeService;
            this._branchRepo = branchRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddBranch(BranchReceivingDTO branchReceivingDTO)
        {
            var branch = _mapper.Map<Branch>(branchReceivingDTO);
            var savedBranch = await _branchRepo.SaveBranch(branch);
            if (savedBranch == null)
            {
                return new ApiResponse(500);
            }
            var branchTransferDTOs = _mapper.Map<BranchTransferDTO>(branch);
            return new ApiOkResponse(branchTransferDTOs);
        }

        public async Task<ApiResponse> GetAllBranches()
        {
            var branches = await _branchRepo.FindAllBranches();
            if (branches == null)
            {
                return new ApiResponse(404);
            }
            var branchTransferDTOs = _mapper.Map<IEnumerable<BranchTransferDTO>>(branches);
            return new ApiOkResponse(branchTransferDTOs);
        }

        public async Task<ApiResponse> GetBranchById(long id)
        {
            var branch = await _branchRepo.FindBranchById(id);
            if (branch == null)
            {
                return new ApiResponse(404);
            }
            var branchTransferDTOs = _mapper.Map<BranchTransferDTO>(branch);
            return new ApiOkResponse(branchTransferDTOs);
        }

        public async Task<ApiResponse> GetBranchByName(string name)
        {
            var branch = await _branchRepo.FindBranchByName(name);
            if (branch == null)
            {
                return new ApiResponse(404);
            }
            var branchTransferDTOs = _mapper.Map<BranchTransferDTO>(branch);
            return new ApiOkResponse(branchTransferDTOs);
        }

        public async Task<ApiResponse> UpdateBranch(long id, BranchReceivingDTO branchReceivingDTO)
        {
            var branchToUpdate = await _branchRepo.FindBranchById(id);
            if (branchToUpdate == null)
            {
                return new ApiResponse(404);
            }
            branchToUpdate.Name = branchReceivingDTO.Name;
            branchToUpdate.Description = branchReceivingDTO.Description;
            branchToUpdate.Address = branchReceivingDTO.Address;
            branchToUpdate.HeadId = branchReceivingDTO.HeadId;
            var updatedBranch = await _branchRepo.UpdateBranch(branchToUpdate);

            if (updatedBranch == null)
            {
                return new ApiResponse(500);
            }
            var branchTransferDTOs = _mapper.Map<BranchTransferDTO>(updatedBranch);
            return new ApiOkResponse(branchTransferDTOs);


        }

        public async Task<ApiResponse> DeleteBranch(long id)
        {
            var branchToDelete = await _branchRepo.FindBranchById(id);
            if (branchToDelete == null)
            {
                return new ApiResponse(404);
            }

            foreach (Office office in branchToDelete.Offices)
            {
                await _officeService.DeleteOffice(office.Id);
            }

            if (!await _branchRepo.DeleteBranch(branchToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}
