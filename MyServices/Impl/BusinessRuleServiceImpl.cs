using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class BusinessRuleServiceImpl:IBusinessRuleService
    {
        private readonly IBusinessRulesRepository _businessRulesRepository;
        private readonly IMapper _mapper;

        public BusinessRuleServiceImpl(IMapper mapper, IBusinessRulesRepository businessRulesRepository)
        {
            _mapper = mapper;
            _businessRulesRepository = businessRulesRepository;
        }

        public async Task<ApiCommonResponse> AddBusinessRule(HttpContext context, BusinessRuleReceivingDTO businessRuleReceivingDTO)
        {
            var rule = _mapper.Map<BusinessRule>(businessRuleReceivingDTO);
            var IdExist = _businessRulesRepository.GetRegServiceId(businessRuleReceivingDTO.ServiceRegistrationId);
            if (IdExist != null)
            {
                return new ApiResponse(409);
            }

            rule.CreatedById = context.GetLoggedInUserId();
            rule.CreatedAt = DateTime.UtcNow;
            var savedType = await _businessRulesRepository.SaveRule(rule);
            if (savedType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<BusinessRuleTransferDTO>(rule);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> AddPairable(HttpContext context, BRPairableReceivingDTO bRPairableReceivingDTO)
        {
            //var pair = _mapper.Map<BRPairable>(bRPairableReceivingDTO);
            //var IdExist = _businessRulesRepository.GetBusinessRileRegServiceId(bRPairableReceivingDTO.BusinessRuleId);
            //if (IdExist != null)
            //{
            //    return new ApiResponse(409);
            //}
            var pair = new BRPairable();

            for (int i = 0; i < bRPairableReceivingDTO.ServiceRegistrationId.Length; i++)
            {
                pair.Id = 0;
                pair.BusinessRuleId = bRPairableReceivingDTO.BusinessRuleId;
                pair.ServiceRegistrationId = bRPairableReceivingDTO.ServiceRegistrationId[i];
                var IdExist = _businessRulesRepository.GetBusinessAndRegServiceId(bRPairableReceivingDTO.BusinessRuleId, bRPairableReceivingDTO.ServiceRegistrationId[i]);
                if (IdExist == null)
                {
                    pair.CreatedById = context.GetLoggedInUserId();
                    pair.CreatedAt = DateTime.UtcNow;

                    var savedType = await _businessRulesRepository.SavePairable(pair);
                    if (savedType == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    //return new ApiResponse(409);
                }
               
            }

            return new ApiOkResponse("Records Added");
        }



        public async Task<ApiCommonResponse> DeleteBusinessRule(long id)
        {
            var itemToDelete = await _businessRulesRepository.FindRuleById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _businessRulesRepository.DeleteRule(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeletePairable(long id)
        {
            var itemToDelete = await _businessRulesRepository.FindPairableById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _businessRulesRepository.DeletePairable(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllActivePairables()
        {
            var pairables = await _businessRulesRepository.FindAllActivePairables();
            if (pairables == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<BRPairableTransferDTO>>(pairables);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllBusinessRules()
        {
            var rule = await _businessRulesRepository.FindAllRules();
            if (rule == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<BusinessRuleTransferDTO>>(rule);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPairableBusinessRules()
        {
            var rule = await _businessRulesRepository.FindAllPairableRules();
            if (rule == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<BusinessRuleTransferDTO>>(rule);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPairables()
        {
            var pairables = await _businessRulesRepository.FindAllPairables();
            if (pairables == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<BRPairableTransferDTO>>(pairables);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetBusinessRuleById(long id)
        {
            var rule = await _businessRulesRepository.FindRuleById(id);
            if (rule == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<BusinessRuleTransferDTO>(rule);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPairableById(long id)
        {
            var pair = await _businessRulesRepository.FindPairableById(id);
            if (pair == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<BRPairableTransferDTO>(pair);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateBusinessRule(HttpContext context, long id, BusinessRuleReceivingDTO businessRuleReceivingDTO)
        {
            var itemToUpdate = await _businessRulesRepository.FindRuleById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.IsPairingRequired = businessRuleReceivingDTO.IsPairingRequired;
            itemToUpdate.IsQuantityRequired = businessRuleReceivingDTO.IsQuantityRequired;
            itemToUpdate.MaxQuantity = businessRuleReceivingDTO.MaxQuantity;
            //itemToUpdate.ServiceRegistrationId = businessRuleReceivingDTO.ServiceRegistrationId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updated = await _businessRulesRepository.UpdateRule(itemToUpdate);

            summary += $"Details after change, \n {updated.ToString()} \n";

            if (updated == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<BusinessRuleTransferDTO>(updated);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePairable(HttpContext context, long id, BRPairableReceivingDTO bRPairableReceivingDTO)
        {
            //var summary = "";
            //var itemToUpdate =[];
            //var updated = "";
            var pair = new BRPairable();
            var itemToUpdate = await _businessRulesRepository.FindPairableById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            for (int i = 0; i < bRPairableReceivingDTO.ServiceRegistrationId.Length; i++)
            {
                //pair.Id = 0;
                //pair.BusinessRuleId = bRPairableReceivingDTO.BusinessRuleId;
                itemToUpdate.ServiceRegistrationId = bRPairableReceivingDTO.ServiceRegistrationId[i];
                pair.UpdatedAt = DateTime.UtcNow;

                var updated = await _businessRulesRepository.UpdatePairable(itemToUpdate);
                if (updated == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

           




            return new ApiOkResponse(200);
        }
    }
}
