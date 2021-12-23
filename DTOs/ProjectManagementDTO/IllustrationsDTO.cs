using System;
namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class IllustrationsDTO
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

		public string IllustrationImage
		{
			get;
			set;
		}

		public long? TaskOrDeliverableId
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
