using System;
namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class AssignDeliverableDTO
    {
		
		public long Id
		{
			get;
			set;
		}

		public string Caption
		{
			get;
			set;
		}

		public string Alias
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Priority
		{
			get;
			set;
		}

		public string ConfidenceLevel
		{
			get;
			set;
		}

		public long? DeliverableAssigneeId
		{
			get;
			set;
		}

		public DeliverableUser? DeliverableUser
		{
			get;
			set;
		}


		public long? DeliverableId
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public DateTime DueDate
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
