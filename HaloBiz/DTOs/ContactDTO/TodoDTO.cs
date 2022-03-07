using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ContactDTO
{

	public class TodoDTO
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

		public DateTime DueDate
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public long? GoalId
		{
			get;
			set;
		}

		public List<PMResponsible> Responsible { get; set; }
	}
}
