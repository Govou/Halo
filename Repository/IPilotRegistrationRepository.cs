﻿using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IPilotRegistrationRepository
    {
        Task<PilotProfile> SavePilot(PilotProfile pilotProfile);

        Task<PilotProfile> FindPilotById(long Id);

        Task<IEnumerable<PilotProfile>> FindAllPilots();

        //PilotProfile GetPilotName(string Name);

        Task<PilotProfile> UpdatePilot(PilotProfile pilotProfile);

        Task<bool> DeletePilot(PilotProfile pilotProfile);

        //Tie
        Task<PilotSMORoutesResourceTie> SavePilotTie(PilotSMORoutesResourceTie pilotProfileTie);

        Task<PilotSMORoutesResourceTie> FindPilotTieById(long Id);

        Task<IEnumerable<PilotSMORoutesResourceTie>> FindAllPilotTies();

        Task<bool> DeletePilotTie(PilotSMORoutesResourceTie pilotProfileTie);
        PilotSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long RouteId, long RegionId);
    }
}
