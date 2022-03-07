using System;
namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class UploadedRequirement
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

		public bool IsActive
		{
			get;
			set;
		}

		public long? DeliverableId
		{
			get;
			set;
		}

		

		public string Extension
		{
			get;
			set;
		}

		public long? RequirementId
		{
			get;
			set;
		}


		public string DocUrl
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
