using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceAssignmentDetailsRepository
    {
        //Armed escort
        Task<ArmedEscortServiceAssignmentDetail> SaveEscortServiceAssignmentdetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);
        Task<ArmedEscortServiceAssignmentDetailReplacement> ReplaceArmedEscortServiceAssignmentdetail(ArmedEscortServiceAssignmentDetailReplacement serviceAssignmentDetail);

        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailById(long Id);
        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByAssignmentId(long Id);
        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceId(long resourceId);
        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId);
        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceId2(long? resourceId);

        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetails();
        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllUniqueEscortServiceAssignmentDetails();
        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllNoneHeldEscortServiceAssignmentDetails();

        //MasterServiceAssignment GetName(string name);
        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetailsByAssignmentId(long assignmentId);

        Task<ArmedEscortServiceAssignmentDetail> UpdateEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> DeleteEscortServiceAssignmentDetailByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateEscortServiceAssignmentDetailForEndJourneyByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateArmedEscortServiceAssignmentDetailHeldByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);

        //Commander
        Task<CommanderServiceAssignmentDetail> SaveCommanderServiceAssignmentdetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);
        Task<CommanderServiceAssignmentDetailReplacement> ReplaceCommanderServiceAssignmentdetail(CommanderServiceAssignmentDetailReplacement serviceAssignmentDetail);

        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailById(long Id);
        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByAssignmentId(long Id);
        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceId(long resourceId);
        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId);
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindCommanderServiceAssignmentDetailByTiedVehicleResourceIdAndAssignmentId(long? tiedResourceId, long assId);
        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceId2(long? resourceId);

        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetails();
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllUniqueCommanderServiceAssignmentDetails();
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllNoneHeldCommanderServiceAssignmentDetails();
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetailsByAssignmentId(long assignmentId);
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetailsByProfileId(long profileId);

        //MasterServiceAssignment GetName(string name);

        Task<CommanderServiceAssignmentDetail> UpdateCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> DeleteCommanderServiceAssignmentDetailByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateCommanderServiceAssignmentDetailForEndJourneyByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateCommanderServiceAssignmentDetailHeldByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail);
       // Task<CommanderServiceAssignmentDetail> UpdateCommanderServiceAssignmentDetailForVehicleReplacementByAssignmentIdAndResourceId(CommanderServiceAssignmentDetail serviceAssignmentDetail);

        //Pilot
        Task<PilotServiceAssignmentDetail> SavePilotServiceAssignmentdetail(PilotServiceAssignmentDetail serviceAssignmentDetail);
        Task<PilotServiceAssignmentDetailReplacement> ReplacePilotServiceAssignmentdetail(PilotServiceAssignmentDetailReplacement serviceAssignmentDetail);

        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailById(long Id);
        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByAssignmentId(long Id);
        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceId(long resourceId);
        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId);
        Task<IEnumerable<PilotServiceAssignmentDetail>> FindPilotServiceAssignmentDetailByTiedVehicleResourceIdAndAssignmentId(long? tiedResourceId, long assId);
        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceId2(long? resourceId);

        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetails();
        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllUniquePilotServiceAssignmentDetails();
        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllNoneHeldPilotServiceAssignmentDetails();

        //MasterServiceAssignment GetName(string name);
        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetailsByAssignmentId(long assignmentId);

        Task<PilotServiceAssignmentDetail> UpdatePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeletePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> DeletePilotServiceAssignmentDetailByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdatePilotServiceAssignmentDetailForEndJourneyByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdatePilotServiceAssignmentDetailHeldByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail);

        //Task<PilotServiceAssignmentDetail> UpdatePilotServiceAssignmentDetailForVehicleReplacementByAssignmentIdAndResourceId(PilotServiceAssignmentDetail serviceAssignmentDetail);

        //Vehicle
        Task<VehicleServiceAssignmentDetail> SaveVehicleServiceAssignmentdetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);
        Task<VehicleAssignmentDetailReplacement> ReplaceVehicleServiceAssignmentdetail(VehicleAssignmentDetailReplacement serviceAssignmentDetail);

        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailById(long Id);
        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByAssignmentId(long Id);
        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId(long resourceId);
        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long AssId);
        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId_(long? resourceId);
        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId2(long? resourceId);

        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetails();
        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllUniqueVehicleServiceAssignmentDetails();
        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllNoneHeldVehicleServiceAssignmentDetails();
        List<VehicleServiceAssignmentDetail> FindAllNoneHeldVehicleServiceAssignmentDetails2();
        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetailsByAssignmentId(long assignmentId);

        //MasterServiceAssignment GetName(string name);

        Task<VehicleServiceAssignmentDetail> UpdateVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> DeleteVehicleServiceAssignmentDetailByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateVehicleServiceAssignmentDetailForEndJourneyByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail);
        Task<bool> UpdateVehicleServiceAssignmentDetailHeldByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail);

        //Passenger
        Task<Passenger> SavePassenger(Passenger passenger);

        Task<Passenger> FindPassengerById(long Id);
        Task<Passenger> FindPassengerByAssignmentId(long Id);

        Task<IEnumerable<Passenger>> FindAllPassengers();

        Task<IEnumerable<Passenger>> FindAllPassengersByAssignmentId(long assignmentId);
        //MasterServiceAssignment GetName(string name);

        Task<Passenger> UpdatePassenger(Passenger passenger);

        Task<bool> DeletePassenger(Passenger passenger);
        Task<bool> DeletePassengerByAssignmentId(Passenger passenger);

        Task<IEnumerable<Contract>> FindAllContracts();
    }
}
