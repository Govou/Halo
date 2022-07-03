using HalobizMigrations.Models;

namespace HaloBiz.DTOs
{
   
    public class ServiceAccountsDTO
    {
        public string Description
        {
            get;
            set;
        }

        public long? ServiceId
        {
            get;
            set;
        }

        public Service Service
        {
            get;
            set;
        }

        public long? AccountId
        {
            get;
            set;
        }

        public Account Account
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }
    }
}
