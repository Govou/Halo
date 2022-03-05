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

        public async Task<ApiCommonResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO)
        {
            var addItem = _mapper.Map<SMORegion>(sMOReceivingDTO);
            var NameExist = _sMORouteAndRegionRepository.GetRegionName(sMOReceivingDTO.RegionName);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _sMORouteAndRegionRepository.SaveSMORegion(addItem);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<SMORegionTransferDTO>(addItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddSMOReturnRoute(HttpContext context, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO)
        {
            var addItem = _mapper.Map<SMOReturnRoute>(sMOReturnRouteReceivingDTO);
            var IdExist = _sMORouteAndRegionRepository.GetSMORouteId(sMOReturnRouteReceivingDTO.SMORouteId);
            if (IdExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            //var hasReturnRoute = _sMORouteAndRegionRepository.hasReturnRoute(sMOReturnRouteReceivingDTO.SMORouteId);
            //if (!hasReturnRoute)
            //{
            //    return new ApiCommonResponse(411);
            //}

            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _sMORouteAndRegionRepository.SaveSMOReturnRoute(addItem);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<SMOReturnRouteTransferDTO>(addItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddSMORoute(HttpContext context, SMORouteReceivingDTO sMORouteReceivingDTO)
        {
            var addItem = _mapper.Map<SMORoute>(sMORouteReceivingDTO);
            var NameExist = _sMORouteAndRegionRepository.GetRouteName(sMORouteReceivingDTO.RouteName);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<SMORouteTransferDTO>(addItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddSMORouteMap(HttpContext context, SMORouteMapReceivingDTO sMORouteMapReceivingDTO)
        {
           // var addItem = _mapper.Map<SMORouteAndStateMap>(sMORouteMapReceivingDTO);
            var addItem = new SMORouteAndStateMap();
            for (int i = 0; i < sMORouteMapReceivingDTO.StateId.Length; i++)
            {
                addItem.Id = 0;
                addItem.SMORegionId = sMORouteMapReceivingDTO.SMORegionId;
                addItem.SMORouteId = sMORouteMapReceivingDTO.SMORouteId;
                addItem.StateId = sMORouteMapReceivingDTO.StateId[i];
                var IdExist = _sMORouteAndRegionRepository.GetSMORouteMapByRouteIdAndStateId(sMORouteMapReceivingDTO.SMORouteId, sMORouteMapReceivingDTO.StateId[i]);
                if (IdExist == null)
                {
                    addItem.CreatedById = context.GetLoggedInUserId();
                    addItem.CreatedAt = DateTime.Now;
                    var savedRank = await _sMORouteAndRegionRepository.SaveSMORouteMap(addItem);

                    if (savedRank == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }

            }
           
            var TransferDTO = _mapper.Map<SMORouteMapTransferDTO>(addItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, "Records Added Successfully");
        }

        public async Task<ApiCommonResponse> DeleteSMORegion(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMORegionById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMORegion(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteSMOReturnRoute(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMOReturnRoute(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteSMORoute(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMORouteById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMORoute(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteSMORouteMap(long id)
        {
            var itemToDelete = await _sMORouteAndRegionRepository.FindSMORouteMapById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _sMORouteAndRegionRepository.DeleteSMORouteMap(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllRouteMapsByRouteId(long routeId)
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllRouteMapsByRouteId(routeId);
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteMapTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS, itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSMORegions()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORegions();
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORegionTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSMOReturnRoutes()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMOReturnRoutes();
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMOReturnRouteTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSMORouteMaps()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORoutesMap();
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteMapTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS, itemTransferDTO);
        }

        //public async Task<ApiCommonResponse> GetAllSMORouteAndRegions()
        //{
        //    var allItems = await _sMORouteAndRegionRepository.FindAllSMORegions();
        //    var allItemsRoute = await _sMORouteAndRegionRepository.FindAllSMORoutes();
        //    if (allItems == null && allItemsRoute == null)
        //    {
        //        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
        //    }
        //    var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteRegionTransferDTO>>(allItems, allItemsRoute);
        //    return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        //}

        public async Task<ApiCommonResponse> GetAllSMORoutes()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORoutes();
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSMORoutesByName(string routeName)
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllSMORoutesByName(routeName);
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSMORoutesWithReturnRoute()
        {
            var allItems = await _sMORouteAndRegionRepository.FindAllRoutesWithReturnRoute();
            if (allItems == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<IEnumerable<SMORouteTransferDTO>>(allItems);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSMORegionById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMORegionById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<SMORegionTransferDTO>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSMOReturnRouteById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<SMOReturnRouteTransferDTO>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSMORouteById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMORouteById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var itemTransferDTO = _mapper.Map<SMORouteTransferDTO>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSMORouteMapById(long id)
        {
            var getItem = await _sMORouteAndRegionRepository.FindSMORouteMapById(id);
            if (getItem == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            var itemTransferDTO = _mapper.Map<SMORouteMapTransferDTO>(getItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, itemTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateSMORegion(HttpContext context, long id, SMORegionReceivingDTO sMOReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMORegionById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.RegionName = sMOReceivingDTO.RegionName;
            itemToUpdate.RegionDescription = sMOReceivingDTO.RegionDescription;
           
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _sMORouteAndRegionRepository.UpdateSMORegion(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var itemTransferDTOs = _mapper.Map<SMORegionTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateSMOReturnRoute(HttpContext context, long id, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMOReturnRouteById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.SMORouteId = sMOReturnRouteReceivingDTO.SMORouteId;
            itemToUpdate.RRecoveryTime = sMOReturnRouteReceivingDTO.RRecoveryTime;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _sMORouteAndRegionRepository.UpdateSMOReturnRoute(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var itemTransferDTOs = _mapper.Map<SMOReturnRouteTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateSMORoute(HttpContext context, long id, SMORouteReceivingDTO sMORouteReceivingDTO)
        {
            var itemToUpdate = await _sMORouteAndRegionRepository.FindSMORouteById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var itemTransferDTOs = _mapper.Map<SMORouteTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,itemTransferDTOs);
        }
    }


}
