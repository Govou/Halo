
using System;
using System.ComponentModel.DataAnnotations;

namespace Auth.PermissionParts
{
    public enum Permissions : short
    {
        NotSet = 0, //error or empty state condition

        //The list of permissions on the system
        [Display(GroupName = "Branch", Name = "Get", Description = "Can view branch info")]
        Branch_Get = 0x1,
        [Display(GroupName = "Branch", Name = "Post", Description = "Can create a branch entry")]
        Branch_Post = 0x2,
        [Display(GroupName = "Branch", Name = "Put", Description = "Can update a branch entry")]
        Branch_Put = 0x3,
        [Display(GroupName = "Branch", Name = "Delete", Description = "Can delete a branch entry")]
        Branch_Delete = 0x4,



        //[Display(GroupName = "CardSetup", Name = "Read", Description = "Can read card setups")]
        //CardSetupRead = 0x7,
        [Display(GroupName = "CardSetup", Name = "Create", Description = "Can create a card setup entry")]
        CardSetupCreate = 0x8,
        [Display(GroupName = "CardSetup", Name = "Update", Description = "Can update a card setup entry")]
        CardSetupUpdate = 0x9,
        [Display(GroupName = "CardSetup", Name = "Delete", Description = "Can delete a card setup entry")]
        CardSetupDelete = 0x10,
        //[Display(GroupName = "CardSetup", Name = "UnDelete", Description = "Can undo the delete a card setup entry")]
        //CardSetupUndelete = 0x11,


        //[Display(GroupName = "ClientCardSetup", Name = "Read", Description = "Can read client card setups")]
        //ClientCardSetupRead = 0x13,
        [Display(GroupName = "ClientCardSetup", Name = "Create", Description = "Can create a client card setup entry")]
        ClientCardSetupCreate = 0x14,
        [Display(GroupName = "ClientCardSetup", Name = "Update", Description = "Can update a client card setup entry")]
        ClientCardSetupUpdate = 0x15,
        [Display(GroupName = "ClientCardSetup", Name = "Delete", Description = "Can delete a client card setup entry")]
        ClientCardSetupDelete = 0x16,
        //[Display(GroupName = "ClientCardSetup", Name = "UnDelete", Description = "Can undo the delete a client card setup entry")]
        //ClientCardSetupUndelete = 0x17,       

        //[Display(GroupName = "CardAccount", Name = "Read", Description = "Can read card accounts")]
        //CardAccountRead = 0x19,
        [Display(GroupName = "CardAccount", Name = "Create", Description = "Can create a card account entry")]
        CardAccountCreate = 0x20,
        [Display(GroupName = "CardAccount", Name = "Update", Description = "Can update a card account entry")]
        CardAccountUpdate = 0x21,
        [Display(GroupName = "CardAccount", Name = "Delete", Description = "Can delete a card account entry")]
        CardAccountDelete = 0x22,
        //[Display(GroupName = "CardAccount", Name = "UnDelete", Description = "Can undo the delete a card account entry")]
        //CardAccountUndelete = 0x23,


        //[Display(GroupName = "Card", Name = "Read", Description = "Can read cards")]
        //CardRead = 0x25,
        [Display(GroupName = "Card", Name = "Create", Description = "Can create a card entry")]
        CardCreate = 0x26,
        [Display(GroupName = "Card", Name = "Update", Description = "Can update a card entry")]
        CardUpdate = 0x27,
        [Display(GroupName = "Card", Name = "Delete", Description = "Can delete a card entry")]
        CardDelete = 0x28,
        //[Display(GroupName = "Card", Name = "UnDelete", Description = "Can undo the delete a card entry")]
        //CardUndelete = 0x29,


        //[Display(GroupName = "CardHotlistCode", Name = "Read", Description = "Can read card hotlist codes")]
        //CardHotlistCodeRead = 0x31,
        [Display(GroupName = "CardHotlistCode", Name = "Create", Description = "Can create a card hotlist code entry")]
        CardHotlistCodeCreate = 0x32,
        [Display(GroupName = "CardHotlistCode", Name = "Update", Description = "Can update a card hotlist code entry")]
        CardHotlistCodeUpdate = 0x33,
        [Display(GroupName = "CardHotlistCode", Name = "Delete", Description = "Can delete a card hotlist code entry")]
        CardHotlistCodeDelete = 0x34,
        //[Display(GroupName = "CardHotlistCode", Name = "UnDelete", Description = "Can undo the delete a card hotlist code entry")]
        //CardHotlistCodeUndelete = 0x35,


        [Display(GroupName = "Role", Name = "RoleCreate", Description = "Can create roles with their permissions")]
        RoleCreate = 0x37,
        [Display(GroupName = "Role", Name = "RoleUpdate", Description = "Can update a role along with its permissions")]
        RoleUpdate = 0x38,
        [Display(GroupName = "Role", Name = "Delete", Description = "Can delete a role entry")]
        RoleDelete = 0x39,
        //[Display(GroupName = "Role", Name = "UnDelete", Description = "Can undo the delete a role entry")]
        //RoleUndelete = 0x40,

        //[Display(GroupName = "CardOverride", Name = "Read", Description = "Can read card overrides")]
        //CardOverrideRead = 0x41,
        [Display(GroupName = "CardOverride", Name = "Create", Description = "Can create a card override ")]
        CardOverrideCreate = 0x42,
        [Display(GroupName = "CardOverride", Name = "Update", Description = "Can update a card override ")]
        CardOverrideUpdate = 0x43,
        [Display(GroupName = "CardOverride", Name = "Delete", Description = "Can delete a card override ")]
        CardOverrideDelete = 0x44,
        //[Display(GroupName = "CardOverride", Name = "UnDelete", Description = "Can undo the delete a card override ")]
        //CardOverrideUndelete = 0x45,

        [Display(GroupName = "PendingApprovals", Name = "Approver", Description = "Can approve any new record or change of record")]
        Approver = 0x46,

        [Display(GroupName = "User", Name = "Create", Description = "Can create a user")]
        UserCreate = 0x47,
        [Display(GroupName = "User", Name = "EnableDisable", Description = "Can disable or enable a user")]
        UserAccessChange = 0x48,
        [Display(GroupName = "User", Name = "Roles", Description = "Manage user roles")]
        UserRoles = 0x49,

    }

}