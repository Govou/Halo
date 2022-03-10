using System;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.ContactDTO
{
    public class LeadQuoteClass
    {

		
		public string Rcnumber
		{
			get;
			set;
		}

                           
        public string ReferenceNo
        {
			get;set;
        }


		public string LogoUrl
		{
			get;
			set;
		}


		public string Email
		{
			get;
			set;
		}


		public long? SuspectId
		{
			get;
			set;
		}

		public long? CustomerId
		{
			get;
			set;
		}



		public long? LeadId
		{
			get;
			set;
		}

		

		public bool IsDeleted
		{
			get;
			set;
		}

		public DateTime CreatedAt
		{
			get;
			set;
		}

		


		public virtual Quote Quote
		{
			get;
			set;
		}
	}
}
