using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HaloBiz.MyServices;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceRelationshipServiceImpl : IServiceRelationshipService
    {
        private readonly IServiceRelationshipRepository _servicesRepository;
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRelationshipServiceImpl> _logger;
        private readonly IMapper _mapper;


        public ServiceRelationshipServiceImpl(
                                IServiceRelationshipRepository servicesRepository,                              
                                HalobizContext context,
                                IMapper mapper,
                                ILogger<ServiceRelationshipServiceImpl> logger
                                )
        {
            _context = context;
            _logger = logger;
            _servicesRepository = servicesRepository;
            _mapper = mapper;
        }

        public async Task<ApiCommonResponse> FindAllUnmappedDirects()
        {
            try
            {
                var services = await _servicesRepository.FindAllUnmappedDirects();

                if (services == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                }
                
                var leanformatService = _mapper.Map<IEnumerable<ServicesLeanformatDTO>>(services);
                return CommonResponse.Send(ResponseCodes.SUCCESS,leanformatService);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindAllUnmappedDirects", ex);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> FindAllRelationships()
        {           

            try
            {
                var services = await _servicesRepository.FindAllRelationships();
                return services == null ? CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE) : CommonResponse.Send(ResponseCodes.SUCCESS, services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindAllRelationships", ex);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> FindServiceRelationshipByAdminId(long id)
        {            
            try
            {
                var services = await _servicesRepository.FindServiceRelationshipByAdminId(id);
                return services == null ? CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE) : CommonResponse.Send(ResponseCodes.SUCCESS, services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindServiceRelationshipByAdminId", ex);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> FindServiceRelationshipByDirectId(long id)
        {           
            try
            {
                var services = await _servicesRepository.FindServiceRelationshipByDirectId(id);
                return services == null ? CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE) : CommonResponse.Send(ResponseCodes.SUCCESS, services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindServiceRelationshipByDirectId", ex);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }      
    }
}