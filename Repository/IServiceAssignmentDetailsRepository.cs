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

        Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailById(long Id);

        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetails();

        //MasterServiceAssignment GetName(string name);
        Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetailsByAssignmentId(long assignmentId);

        Task<ArmedEscortServiceAssignmentDetail> UpdateEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail);

        //Commander
        Task<CommanderServiceAssignmentDetail> SaveCommanderServiceAssignmentdetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);

        Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailById(long Id);

        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetails();
        Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetailsByAssignmentId(long assignmentId);

        //MasterServiceAssignment GetName(string name);

        Task<CommanderServiceAssignmentDetail> UpdateCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail);

        //Pilot
        Task<PilotServiceAssignmentDetail> SavePilotServiceAssignmentdetail(PilotServiceAssignmentDetail serviceAssignmentDetail);

        Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailById(long Id);

        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetails();

        //MasterServiceAssignment GetName(string name);
        Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetailsByAssignmentId(long assignmentId);

        Task<PilotServiceAssignmentDetail> UpdatePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeletePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail);

        //Vehicle
        Task<VehicleServiceAssignmentDetail> SaveVehicleServiceAssignmentdetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);

        Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailById(long Id);

        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetails();
        Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetailsByAssignmentId(long assignmentId);

        //MasterServiceAssignment GetName(string name);

        Task<VehicleServiceAssignmentDetail> UpdateVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);

        Task<bool> DeleteVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail);

        //Passenger
        Task<Passenger> SavePassenger(Passenger passenger);

        Task<Passenger> FindPassengerById(long Id);

        Task<IEnumerable<Passenger>> FindAllPassengers();

        Task<IEnumerable<Passenger>> FindAllPassengersByAssignmentId(long assignmentId);
        //MasterServiceAssignment GetName(string name);

        Task<Passenger> UpdatePassenger(Passenger passenger);

        Task<bool> DeletePassenger(Passenger passenger);
    }
}
