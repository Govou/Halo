using System;
using System.Collections.Generic;
using HalobizMigrations.Models;
using HalobizMigrations.Models.ProjectManagement;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class DeliverableWithStatusDTO
    {
		
		public long Id
		{
			get;
			set;
		}

		public string Alias
		{
			get;
			set;
		}

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

		public bool IsPicked
		{
			get;
			set;
		}

		public bool IsApproved
		{
			get;
			set;
		}

		public bool IsDeclined
		{
			get;
			set;
		}

		public decimal Budget
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

		public DateTime? DatePicked
		{
			get;
			set;
		}

		public ICollection<PMUploadedRequirement> UploadedRequirement
		{
			get;
			set;
		}

		public long? TaskId
		{
			get;
			set;
		}

		public Task Task
		{
			get;
			set;
		}

		public long TimeEstimate
		{
			get;
			set;
		}

		public ICollection<Dependency> Dependencies
		{
			get;
			set;
		}

		public ICollection<DeliverableAssignee> DeliverableAssignees
		{
			get;
			set;
		}

		public ICollection<Balance> Balances
		{
			get;
			set;
		}

		public ICollection<PMNote> Notes
		{
			get;
			set;
		}

		public ICollection<Picture> Pictures
		{
			get;
			set;
		}

		public ICollection<Video> Videos
		{
			get;
			set;
		}

		public ICollection<Document> Documents
		{
			get;
			set;
		}

		public ICollection<PMRequirement> Requirements
		{
			get;
			set;
		}

		public DependentType? DependentType
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public long? AssignTaskId
		{
			get;
			set;
		}

		public AssignTask AssignTask
		{
			get;
			set;
		}

		public long CreatedById
		{
			get;
			set;
		}

		public StatusFlow StatusFlow { get; set; }
		public UserProfile userProfile { get; set; }

		public ICollection<PMIllustration> PMIllustrations { get; set; }

		public UserProfile CreatedBy
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
