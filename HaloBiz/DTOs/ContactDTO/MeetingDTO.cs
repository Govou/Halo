using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.ContactDTO
{
    public class MeetingDTO
    {
		

		public string Caption
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public MeetingType MeetingType
		{
			get;
			set;
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public string Reason
		{
			get;
			set;
		}

		public long? SuspectId
		{
			get;
			set;
		}

		

		public bool IsActive
		{
			get;
			set;
		}

		public ICollection<MeetingStaff> StaffsInvolved
		{
			get;
			set;
		}

		public ICollection<MeetingContact> ContactsInvolved
		{
			get;
			set;
		}

		public long CreatedById
		{
			get;
			set;
		}

		

		public DateTime CreatedAt
		{
			get;
			set;
		}

		public DateTime UpdatedAt
		{
			get;
			set;
		}
	}
}
