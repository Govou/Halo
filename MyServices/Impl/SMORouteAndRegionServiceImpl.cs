using HalobizMigrations.Data;
using HaloBiz.Helpers;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HaloBiz.DTOs.SMODTO;
using AutoMapper;
using HaloBiz.Repository;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class SMORouteAndRegionServiceImpl:ISMORouteAndRegionService
    {
        //private readonly HalobizContext _context;
        private readonly IMapper _mapper;
        private readonly ISMORouteAndRegionRepository _sMORouteAndRegionRepository;

        private readonly ILogger<SMORouteAndRegionServiceImpl> _logger;

        public SMORouteAndRegionServiceImpl( ILogger<SMORouteAndRegionServiceImpl> logger, IMapper mapper, ISMORouteAndRegionRepository sMORouteAndRegionRepository)
        {
            //_context = context;
            _mapper = mapper;
            _logger = logger;
            _sMORouteAndRegionRepository = sMORouteAndRegionRepository;
        }

        public async Task<ApiResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO)
        {
            var addItem = _mapper.Map<SMORegion>(sMOReceivingDTO);
            var NameExist = _sMORouteAndRegionRepository.GetRegionName(sMOReceivingDTO.RegionName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _sMORouteAndRegionRepository.SaveSMORegion(addItem);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<SMORegionTransferDTO>(addItem);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddSMOReturnRoute(HttpContext context, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO)
        {
            var addItem = _mapper.Map<SMOReturnRoute>(sMOReturnRouteReceivingDTO);
            //var hasReturnRoute = _sMORouteAndRegionRepository.hasReturnRoute(sMOReturnRouteReceivingDTO.SMORouteId);
            //if (!hasReturnRoute)
            //{
            //    return new ApiResponse(411);
            //}

            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _sMORouteAndRegionRepository.SaveSMOReturnRoute(addItem);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<SMOReturnRouteTransferDTO>(addItem);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddSMORoute(HttpContext context, SMORouteReceivingDTO sMORouteReceivingDTO)
        {
            var addItem = _mapper.Map<SMORoute>(sMORouteReceivingDTO);
            var addItem2 = _mapper.Map<SMOReturnRoute>(sMORouteReceivingDTO);
            var NameExist = _sMORouteAndRegionRepository.GetRouteName(sMORouteReceivingDTO.RouteName);
            if (NameExist != null)
            {
                return new ApiResponse(409);
            }
            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _sMORouteAndRegionRepository.SaveSMORoute(addItem);
            //if(addItem.IsReturnRouteRequired == true)
            //{
            //    addItem2.SMORouteId = addItem.Id;
                
            //    var savedRRoute = await _sMORouteAndRegionRepository.SaveSMOReturnRoute(addItem2);
            //}
            
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<SMORouteTransferDTO>(addItem);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> DeleteSMORegion(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMORegionById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMORegion(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteSMOReturnRoute(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMOReturnRoute(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteSMORoute(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMORouteById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMORoute(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllSMORegions()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORegions();
            if (allItems == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORegionTransferDTO>>(allItems);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetAllSMOReturnRoutes()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMOReturnRoutes();
            if (allItems == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMOReturnRouteTransferDTO>>(allItems);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetAllSMORoutes()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORoutes();
            if (allItems == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteTransferDTO>>(allItems);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetAllSMORoutesWithReturnRoute()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllRoutesWithReturnRoute();
            if (allItems == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteTransferDTO>>(allItems);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetSMORegionById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMORegionById(id);
            if (getItem == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<SMORegionTransferDTO>(getItem);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetSMOReturnRouteById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);
            if (getItem == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<SMOReturnRouteTransferDTO>(getItem);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> GetSMORouteById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMORouteById(id);
            if (getItem == null)
            {
                return new ApiResponse(404);
            }
            var itemTransferDTO = _mapper.Map<SMORouteTransferDTO>(getItem);
            return new ApiOkResponse(itemTransferDTO);
        }

        public async Task<ApiResponse> UpdateSMORegion(HttpContext context, long id, SMORegionReceivingDTO sMOReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMORegionById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.RegionName = sMOReceivingDTO.RegionName;
            itemToUpdate.RegionDescription = sMOReceivingDTO.RegionDescription;
           
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _sMORouteAndRegionRepository.UpdateSMORegion(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var itemTransferDTOs = _mapper.Map<SMORegionTransferDTO>(updatedRank);
            return new ApiOkResponse(itemTransferDTOs);
        }

        public async Task<ApiResponse> UpdateSMOReturnRoute(HttpContext context, long id, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.SMORouteId = sMOReturnRouteReceivingDTO.SMORouteId;
            itemToUpdate.RRecoveryTime = sMOReturnRouteReceivingDTO.RRecoveryTime;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _sMORouteAndRegionRepository.UpdateSMOReturnRoute(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var itemTransferDTOs = _mapper.Map<SMOReturnRouteTransferDTO>(updatedRank);
            return new ApiOkResponse(itemTransferDTOs);
        }

        public async Task<ApiResponse> UpdateSMORoute(HttpContext context, long id, SMORouteReceivingDTO sMORouteReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMORouteById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.RouteName = sMORouteReceivingDTO.RouteName;
            itemToUpdate.RRecoveryTime = sMORouteReceivingDTO.RRecoveryTime;
            itemToUpdate.IsReturnRouteRequired = sMORouteReceivingDTO.IsReturnRouteRequired;
            itemToUpdate.RouteDescription = sMORouteReceivingDTO.RouteDescription;
            itemToUpdate.SMORegionId = sMORouteReceivingDTO.SMORegionId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _sMORouteAndRegionRepository.UpdateSMORoute(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var itemTransferDTOs = _mapper.Map<SMORouteTransferDTO>(updatedRank);
            return new ApiOkResponse(itemTransferDTOs);
        }
    }


}
