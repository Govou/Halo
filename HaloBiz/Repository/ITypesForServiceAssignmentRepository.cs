using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ITypesForServiceAssignmentRepository
    {
        //Trip
        Task<TripType> SaveTripType(TripType tripType);

        Task<TripType> FindTripTypeById(long Id);

        Task<IEnumerable<TripType>> FindAllTripTypes();

        TripType GetName(string name);

        Task<TripType> UpdateTripType(TripType tripType);

        Task<bool> DeleteTripType(TripType tripType);

        //Passenger
        Task<PassengerType> SavePassengerType(PassengerType passengerType);

        Task<PassengerType> FindPassengerTypeById(long Id);

        Task<IEnumerable<PassengerType>> FindAllPassengerTypes();

        PassengerType GetPassengerName(string name);

        Task<PassengerType> UpdatePassengerType(PassengerType passengerType);

        Task<bool> DeletePassengerType(PassengerType passengerType);


        //Source
        Task<SourceType> SaveSourceType(SourceType sourceType);

        Task<SourceType> FindSourceTypeById(long Id);

        Task<IEnumerable<SourceType>> FindAllSourceTypes();

        SourceType GetSourceName(string name);

        Task<SourceType> UpdateSourceType(SourceType sourceType);

        Task<bool> DeleteSourceType(SourceType sourceType);


        //ReleaseType
        Task<ReleaseType> SaveReleaseType(ReleaseType ReleaseType);

        Task<ReleaseType> FindReleaseTypeById(long Id);

        Task<IEnumerable<ReleaseType>> FindAllReleaseTypes();

        ReleaseType GetReleaseName(string name);

        Task<ReleaseType> UpdateReleaseType(ReleaseType releaseType);

        Task<bool> DeleteReleaseType(ReleaseType releaseType);
    }
}
