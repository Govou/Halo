using System;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.ContactDTO
{
    public class SuspectContactDTO
    {
		public long? SuspectId
		{
			get;
			set;
		}

		public Suspect Suspect
		{
			get;
			set;
		}

		public long? ContactId
		{
			get;
			set;
		}

		public Contact Contact
		{
			get;
			set;
		}

		public ContactPriority ContactPriority
		{
			get;
			set;
		}

		public ContactDesignation ContactDesignation
		{
			get;
			set;
		}

		public ContactQualification ContactQualification
		{
			get;
			set;
		}
	}
}
