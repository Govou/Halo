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

        public async Task<ApiCommonResponse> AddOffice(OfficeReceivingDTO officeReceivingDTO)
        {
            var officeToSave = _mapper.Map<Office>(officeReceivingDTO);
            var savedOffice = await _officeRepo.SaveOffice(officeToSave);
            if (savedOffice == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(savedOffice);
            return CommonResponse.Send(ResponseCodes.SUCCESS,officeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllOffices()
        {
            var offices = await _officeRepo.FindAllOffices();
            if (offices == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var officeTransferDTOs = _mapper.Map<IEnumerable<OfficeTransferDTO>>(offices);
            return CommonResponse.Send(ResponseCodes.SUCCESS,officeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetOfficeById(long id)
        {
            var office = await _officeRepo.FindOfficeById(id);
            if (office == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(office);
            return CommonResponse.Send(ResponseCodes.SUCCESS,officeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetOfficeByName(string name)
        {
            var office = await _officeRepo.FindOfficeByName(name);
            if (office == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var officeTransferDTOs = _mapper.Map<OfficeTransferDTO>(office);
            return CommonResponse.Send(ResponseCodes.SUCCESS,officeTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateOffice(long id, OfficeReceivingDTO branchReceivingDTO)
        {
            var officeToUpdate = await _officeRepo.FindOfficeById(id);
            if (officeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            summary += $"Details after change, \n {updatedOffice.ToString()} \n";
            
            //TODO save modification history

            var officeTransferDTO = _mapper.Map<OfficeTransferDTO>(updatedOffice);
            return CommonResponse.Send(ResponseCodes.SUCCESS,officeTransferDTO);

        }

        public async Task<ApiCommonResponse> DeleteOffice(long id)
        {
            var officeToDelete = await _officeRepo.FindOfficeById(id);
            if (officeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _officeRepo.DeleteOffice(officeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        } 
    }
}