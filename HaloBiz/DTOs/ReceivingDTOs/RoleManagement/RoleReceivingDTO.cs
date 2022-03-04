using Auth.PermissionParts;
using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs.RoleManagement
{
    public class RoleReceivingDTO
    {
        public long Id { get; set; } = 0;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<PermissionsDTO> Permissions { get; set; }
    }

    public class PermissionsDTO
    {
        public string Controller { get;  set; }
        public string Action { get;  set; }
        public string Description { get;  set; }
        public string Permission { get;  set; }
    }

    public class RoleTemp : Role
    {
        public IEnumerable<PermissionDisplay> PermissionList { get; set; }

        [MinLength(2, ErrorMessage = "You must choose at least 1 permission for a new role")]  
        /// <summary>
        /// This returns the list of permissions in this role
        /// </summary>
        ///
        public IEnumerable<Permissions> PermissionsInRole => PermissionCode.UnpackPermissionsFromString();

        public RoleTemp() { }

        /// <summary>
        /// This creates the Role with its permissions
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="permissions"></param>
        public RoleTemp(string roleName, string description, ICollection<Permissions> permissions)
        {
            Name = roleName;
            Update(description, permissions);
        }

        public void Update(string description, ICollection<Permissions> permissions)
        {
            Description = description;
            if (permissions == null || !permissions.Any())
                throw new InvalidOperationException("There should be at least one permission associated with a role.");

            PermissionCode = permissions.PackPermissionsIntoString();
        }


    }
}
