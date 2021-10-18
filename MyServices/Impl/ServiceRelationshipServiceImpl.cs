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

        public ServiceRelationshipServiceImpl(
                                IServiceRelationshipRepository servicesRepository,                              
                                HalobizContext context,
                                ILogger<ServiceRelationshipServiceImpl> logger
                                )
        {
            _context = context;
            _logger = logger;
            _servicesRepository = servicesRepository;
        }

        public async Task<ApiResponse> FindAllUnmappedDirects()
        {
            try
            {
                var services = await _servicesRepository.FindAllUnmappedDirects();
                return services == null ? new ApiResponse(404) : new ApiOkResponse(services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindAllUnmappedDirects", ex);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> FindAllRelationships()
        {           

            try
            {
                var services = await _servicesRepository.FindAllRelationships();
                return services == null ? new ApiResponse(404) : new ApiOkResponse(services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindAllRelationships", ex);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> FindServiceRelationshipByAdminId(long id)
        {            
            try
            {
                var services = await _servicesRepository.FindServiceRelationshipByAdminId(id);
                return services == null ? new ApiResponse(404) : new ApiOkResponse(services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindServiceRelationshipByAdminId", ex);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> FindServiceRelationshipByDirectId(long id)
        {           
            try
            {
                var services = await _servicesRepository.FindServiceRelationshipByDirectId(id);
                return services == null ? new ApiResponse(404) : new ApiOkResponse(services);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FindServiceRelationshipByDirectId", ex);
                return new ApiResponse(500, ex.Message);
            }
        }      
    }
}