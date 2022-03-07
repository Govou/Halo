using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class TaskOwnerShipDTO
    {
		
		public long Id
		{
			get;
			set;
		}

		public long? TaskOwnerId
		{
			get;
			set;
		}

		

		public DateTime DatePicked
		{
			get;
			set;
		}

		public DateTime TimePicked
		{
			get;
			set;
		}

		public ICollection<Task> Tasks
		{
			get;
			set;
		}

		public bool IsDeleted
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
