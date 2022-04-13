using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class JourneyStartandStopRepositoryImpl: IJourneyStartandStopRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<JourneyStartandStopRepositoryImpl> _logger;
        public JourneyStartandStopRepositoryImpl(HalobizContext context, ILogger<JourneyStartandStopRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteJouneyIncident(JourneyIncident incident)
        {
            incident.IsDeleted = true;
            _context.JourneyIncidents.Update(incident);
            return await SaveChanges();
        }

        public async Task<bool> DeleteJouneyIncidentPic(JourneyIncidentPicture incidentPic)
        {
            incidentPic.IsDeleted = true;
            _context.JourneyIncidentPictures.Update(incidentPic);
            return await SaveChanges();
        }

        public async Task<bool> DeleteJouneyStart(ArmadaJourneyStart journeyStart)
        {
            journeyStart.IsDeleted = true;
            _context.ArmadaJourneyStarts.Update(journeyStart);
            return await SaveChanges();
        }

        public async Task<bool> DeleteJouneyStop(ArmadaJourneyStop journeyStop)
        {
            journeyStop.IsDeleted = true;
            _context.ArmadaJourneyStops.Update(journeyStop);
            return await SaveChanges();
        }

        public async Task<bool> DeleteJourneyLeadCommander(JourneyLeadCommander leadCommander)
        {
            leadCommander.IsDeleted = true;
            _context.JourneyLeadCommanders.Update(leadCommander);
            return await SaveChanges();
        }

        public async Task<bool> DeleteJourneyNote(JourneyNote journeyNote)
        {
            journeyNote.IsDeleted = true;
            _context.JourneyNotes.Update(journeyNote);
            return await SaveChanges();
        }

        public async Task<IEnumerable<FeedbackMaster>> FindAllFeedbackMasters()
        {
            return await _context.FeedbackMasters.Where(jr => jr.IsDeleted == false).Include(x=>x.JourneyStart).Include(x=>x.ArmedEscortFeedbackDetails.Where(x=>x.IsDeleted == false))
            .Include(x => x.CommanderFeedbackDetails.Where(x => x.IsDeleted == false)).Include(x => x.PilotFeedbackDetails.Where(x => x.IsDeleted == false))
            .Include(x => x.VehicleFeedbackDetails.Where(x => x.IsDeleted == false))
            .Include(ct => ct.ServiceAssignment).OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<JourneyIncidentPicture>> FindAllJouneyIncidentPics()
        {
            return await _context.JourneyIncidentPictures.Where(jr => jr.IsDeleted == false)
            .Include(ct => ct.JourneyIncident).OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<JourneyIncidentPicture>> FindAllJouneyIncidentPicsByIncidentId(long incidentId)
        {
            return await _context.JourneyIncidentPictures.Where(jr => jr.IsDeleted == false && jr.JourneyIncidentId == incidentId)
           .Include(ct => ct.JourneyIncident).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<JourneyIncident>> FindAllJouneyIncidents()
        {
            return await _context.JourneyIncidents.Where(jr => jr.IsDeleted == false)
           .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<JourneyIncident>> FindAllJouneyIncidentsByJourneyStartId(long journeyStartId)
        {
            return await _context.JourneyIncidents.Where(jr => jr.IsDeleted == false && jr.JourneyStartId== journeyStartId)
         .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
         .ToListAsync();
        }

        public async Task<IEnumerable<JourneyLeadCommander>> FindAllJouneyLeadCommanders()
        {
            return await _context.JourneyLeadCommanders.Where(jr => jr.IsDeleted == false && jr.IsActive == true)
           .Include(ct=>ct.LeadCommander)
          .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
          .ToListAsync();
        }

        public async Task<IEnumerable<JourneyNote>> FindAllJouneyNotesByStartId(long journeyStartId)
        {
            return await _context.JourneyNotes.Where(jr => jr.IsDeleted == false && jr.JourneyStartId == journeyStartId)
           .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<ArmadaJourneyStart>> FindAllJouneyStarts()
        {
            return await _context.ArmadaJourneyStarts.Where(jr => jr.IsDeleted == false)
           .Include(ct => ct.ServiceAssignment).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<ArmadaJourneyStop>> FindAllJouneyStops()
        {

            return await _context.ArmadaJourneyStops.Where(jr => jr.IsDeleted == false)
           .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<ArmadaJourneyStop>> FindAllJouneyStopsByStartId(long startId)
        {
            return await _context.ArmadaJourneyStops.Where(jr => jr.IsDeleted == false && jr.JourneyStartId == startId)
         .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
         .ToListAsync();
        }

        public async Task<IEnumerable<JourneyNote>> FindAllJourneyNotes()
        {
            return await _context.JourneyNotes.Where(jr => jr.IsDeleted == false )
           .Include(ct => ct.JourneyStart).OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public Task<FeedbackDetail> FindFeedbackDetailById(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<FeedbackMaster> FindFeedbackMasterByAssignmentId(long assignId)
        {
            return await _context.FeedbackMasters.Include(r => r.ServiceAssignment)
          .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == assignId && aer.IsDeleted == false);
        }

        public async Task<FeedbackMaster> FindFeedbackMasterById(long Id)
        {
            return await _context.FeedbackMasters.Include(r => r.ServiceAssignment)
            .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<GeneralFeedbackDetail> FindGenralFeedbackByAssignmentId(long assignId)
        {
            //return await _context.GeneralFeedbackDetails.Include(r => r.FeedbackMaster)
            //.FirstOrDefaultAsync(aer => aer.FeedbackMasterId == fMasterId && aer.IsDeleted == false);
            return await _context.GeneralFeedbackDetails.Include(r => r.FeedbackMaster)
          .FirstOrDefaultAsync(aer => aer.FeedbackMaster.ServiceAssignmentId == assignId && aer.IsDeleted == false);
        }

        public async Task<JourneyIncident> FindJourneyIncidentById(long Id)
        {
            return await _context.JourneyIncidents.Include(r => r.JourneyStart)
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<JourneyIncidentPicture> FindJourneyIncidentPicById(long Id)
        {
            return await _context.JourneyIncidentPictures.Include(r => r.JourneyIncident)
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<JourneyLeadCommander> FindJourneyLeadCommanderById(long Id)
        {
            return await _context.JourneyLeadCommanders.Include(r => r.JourneyStart)
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false && aer.IsActive == true);
        }

        public async Task<JourneyNote> FindJourneyNoteById(long Id)
        {
            return await _context.JourneyNotes.Include(r => r.JourneyStart)
             .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ArmadaJourneyStart> FindJourneyStartByAssignmentId(long assignId)
        {
            return await _context.ArmadaJourneyStarts.Include(r => r.ServiceAssignment)
            .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == assignId && aer.IsDeleted == false);
        }

        public async Task<ArmadaJourneyStart> FindJourneyStartById(long Id)
        {
            return await _context.ArmadaJourneyStarts.Include(r => r.ServiceAssignment)
            .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ArmadaJourneyStop> FindJourneyStopById(long Id)
        {
            return await _context.ArmadaJourneyStops.Include(r => r.JourneyStart)
            .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<bool> RelinquishLeadCommander(JourneyLeadCommander leadCommander)
        {
            leadCommander.IsActive = false;
            _context.JourneyLeadCommanders.Update(leadCommander);
            return await SaveChanges();
        }

        public async Task<ArmedEscortFeedbackDetail> SaveArmedEscortFeedback(ArmedEscortFeedbackDetail feedback)
        {
            var savedEntity = await _context.ArmedEscortFeedbackDetails.AddAsync(feedback);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderFeedbackDetail> SaveCommanderFeedback(CommanderFeedbackDetail feedback)
        {
            var savedEntity = await _context.CommanderFeedbackDetails.AddAsync(feedback);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<FeedbackDetail> SaveFeedbackDetail(FeedbackDetail feedbackDetail)
        {
            //var savedEntity = await _context.FeedbackDetails.AddAsync(feedbackDetail);

            //if (await SaveChanges())
            //{
            //    return savedEntity.Entity;
            //}
            return null;
        }

        public async Task<FeedbackMaster> SaveFeedbackMaster(FeedbackMaster feedbackMaster)
        {
            var savedEntity = await _context.FeedbackMasters.AddAsync(feedbackMaster);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<GeneralFeedbackDetail> SaveGeneralFeedback(GeneralFeedbackDetail feedback)
        {
            var savedEntity = await _context.GeneralFeedbackDetails.AddAsync(feedback);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<JourneyIncident> SaveJourneyIncident(JourneyIncident incident)
        {
            var savedEntity = await _context.JourneyIncidents.AddAsync(incident);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<JourneyIncidentPicture> SaveJourneyIncidentPic(JourneyIncidentPicture incidentPic)
        {
            var savedEntity = await _context.JourneyIncidentPictures.AddAsync(incidentPic);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<JourneyLeadCommander> SaveJourneyLeadCommander(JourneyLeadCommander leadCommander)
        {
            var savedEntity = await _context.JourneyLeadCommanders.AddAsync(leadCommander);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<JourneyNote> SaveJourneyNote(JourneyNote journeyNote)
        {
            var savedEntity = await _context.JourneyNotes.AddAsync(journeyNote);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmadaJourneyStart> SaveJourneyStart(ArmadaJourneyStart journeyStart)
        {
            var savedEntity = await _context.ArmadaJourneyStarts.AddAsync(journeyStart);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmadaJourneyStop> SaveJourneyStop(ArmadaJourneyStop journeyStop)
        {
            var savedEntity = await _context.ArmadaJourneyStops.AddAsync(journeyStop);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotFeedbackDetail> SavePilotFeedback(PilotFeedbackDetail feedback)
        {
            var savedEntity = await _context.PilotFeedbackDetails.AddAsync(feedback);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleFeedbackDetail> SaveVehicleFeedback(VehicleFeedbackDetail feedback)
        {
            var savedEntity = await _context.VehicleFeedbackDetails.AddAsync(feedback);

            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateCancelJouneyStart(ArmadaJourneyStart journeyStart)
        {
            journeyStart.IsJourneyCancelled = true;
            _context.ArmadaJourneyStarts.Update(journeyStart);
            return await SaveChanges();
        }

        public async Task<bool> UpdateCServiceAssignmentOnJouneyStart(MasterServiceAssignment masterServiceAssignment)
        {
            masterServiceAssignment.SAExecutionStatus = 1;
            _context.MasterServiceAssignments.Update(masterServiceAssignment);
            return await SaveChanges();
        }

        public async Task<bool> UpdateCServiceAssignmentOnJouneyStartForEndJourney(MasterServiceAssignment masterServiceAssignment)
        {
            masterServiceAssignment.AssignmentStatus = "Closed";
            masterServiceAssignment.SAExecutionStatus = 2;
            _context.MasterServiceAssignments.Update(masterServiceAssignment);
            return await SaveChanges();
        }

        public async Task<ArmadaJourneyStart> UpdateEndJouneyStart(ArmadaJourneyStart journeyStart)
        {
            var updatedEntity = _context.ArmadaJourneyStarts.Update(journeyStart);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
            //journeyStart.JourneyEndDatetime = DateTime.UtcNow;
            //_context.ArmadaJourneyStarts.Update(journeyStart);
            //return await SaveChanges();
        }

        public async Task<JourneyIncident> UpdateJourneyIncident(JourneyIncident incident)
        {
            var updatedEntity = _context.JourneyIncidents.Update(incident);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<JourneyNote> UpdateJourneyNote(JourneyNote journeyNote)
        {
            var updatedEntity = _context.JourneyNotes.Update(journeyNote);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public Task<ArmadaJourneyStart> UpdateJourneyStartForStop(ArmadaJourneyStart journeyStart)
        {
            throw new NotImplementedException();
        }

        public async Task<ArmadaJourneyStop> UpdateJourneyStop(ArmadaJourneyStop journeyStop)
        {

            var updatedEntity = _context.ArmadaJourneyStops.Update(journeyStop);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateStopJouneyStart(ArmadaJourneyStart journeyStart)
        {
            journeyStart.IsJourneyStopped = true;
            _context.ArmadaJourneyStarts.Update(journeyStart);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.InnerException.Message);
                return false;
            }
        }
    }
}
