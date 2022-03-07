using System;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.ContactDTO
{
    public class ContactResponse
    {
		public long Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string ProfilePicture
		{
			get;
			set;
		}

		public Gender Gender
		{
			get;
			set;
		}

		public DateTime? DateOfBirth
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string Email2
		{
			get;
			set;
		}

		public string Mobile
		{
			get;
			set;
		}

		public string Mobile2
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string MiddleName
		{
			get;
			set;
		}

		public string Twitter
		{
			get;
			set;
		}

		public string Instagram
		{
			get;
			set;
		}

		public string Facebook
		{
			get;
			set;
		}

		public ContactPriority Priority
		{
			get;
			set;
		}

		public ContactDesignation Designation
		{
			get;
			set;
		}

		public long? CustomerDivisionId
		{
			get;
			set;
		}

		
		public CustomerDivision CustomerDivision
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
