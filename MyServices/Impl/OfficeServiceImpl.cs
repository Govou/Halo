using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
using HaloBiz.Repository;

namespace HaloBiz.MyServices.Impl
{
    public class OfficeServiceImpl : IOfficeService
    {
        private readonly IOfficeRepository _officeRepo;
        private readonly IModificationHistoryRepository _modificationRepo;
        private readonly IMapper _mapper;

        public OfficeServiceImpl(IOfficeRepository officeRepo, IModificationHistoryRepository modificationRepo, IMapper mapper)
        {
            this._officeRepo = officeRepo;
            this._modificationRepo = modificationRepo;
            this._mapper = mapper;
        }

        public async Task<ApiResponse> AddOffice(OfficeReceivingDTO officeReceivingDTO)
        {
            var officeToSave = _mapper.Map<Office>(officeReceivingDTO);
            var savedOffice = await _officeRepo.SaveOffice(officeToSave);
            if (savedOffice == null)
            {
                return new ApiResponse(500);
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(savedOffice);
            return new ApiOkResponse(officeTransferDTOs);
        }

        public async Task<ApiResponse> GetAllOffices()
        {
            var offices = await _officeRepo.FindAllOffices();
            if (offices == null)
            {
                return new ApiResponse(404);
            }
            var officeTransferDTOs = _mapper.Map<IEnumerable<OfficeTransferDTO>>(offices);
            return new ApiOkResponse(officeTransferDTOs);
        }

        public async Task<ApiResponse> GetOfficeById(long id)
        {
            var office = await _officeRepo.FindOfficeById(id);
            if (office == null)
            {
                return new ApiResponse(404);
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(office);
            return new ApiOkResponse(officeTransferDTOs);
        }

        public async Task<ApiResponse> GetOfficeByName(string name)
        {
            var office = await _officeRepo.FindOfficeByName(name);
            if (office == null)
            {
                return new ApiResponse(404);
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(office);
            return new ApiOkResponse(officeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateOffice(long id, OfficeReceivingDTO branchReceivingDTO)
        {
            var officeToUpdate = await _officeRepo.FindOfficeById(id);
            if (officeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {officeToUpdate.ToString()} \n" ;

            officeToUpdate.Name = branchReceivingDTO.Name;
            officeToUpdate.BranchId = branchReceivingDTO.BranchId;
            officeToUpdate.PhoneNumber = branchReceivingDTO.PhoneNumber;
            officeToUpdate.Lgaid = branchReceivingDTO.LGAId;
            officeToUpdate.StateId = branchReceivingDTO.StateId;
            officeToUpdate.Description = branchReceivingDTO.Description;
            officeToUpdate.HeadId = branchReceivingDTO.HeadId;


            var updatedOffice = await _officeRepo.UpdateOffice(officeToUpdate);

            if (updatedOffice == null)
            {
                return new ApiResponse(500);
            }

            summary += $"Details after change, \n {updatedOffice.ToString()} \n";
            
            //TODO save modification history

            var officeTransferDTO = _mapper.Map<OfficeTransferDTO>(updatedOffice);
            return new ApiOkResponse(officeTransferDTO);

        }

        public async Task<ApiResponse> DeleteOffice(long id)
        {
            var officeToDelete = await _officeRepo.FindOfficeById(id);
            if (officeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _officeRepo.DeleteOffice(officeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        } 
    }
}