using System;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class AssigneeDTO
    {
		

		public string Name
		{
			get;
			set;
		}

		public long? TaskId
		{
			get;
			set;
		}

		

		public bool IsActive
		{
			get;
			set;
		}

		public long TaskAssigneeId
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
