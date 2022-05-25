using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class OnlineLocationFavoriteTransferDTO
    {
        

        public long Id { get; set; }
        public long? OnlineProfileId { get; set; }
        public long? ClientId { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public string LocationGeometry { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string LocationFullAddress { get; set; }
        public string LocationStreetAddress { get; set; }
       
        public Lga LocationLGA { get; set; }
        public long? LocationLGAId { get; set; }
        
        public State LocationState { get; set; }
        public long? LocationStateId { get; set; }
       
        public OnlineProfile OnlineProfile { get; set; }
       
      
        public CustomerDivision Client { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
