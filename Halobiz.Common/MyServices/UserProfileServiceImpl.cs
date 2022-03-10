using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.Repository;
using Newtonsoft.Json;
using System.Security.Claims;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.Helpers;

namespace Halobiz.Common.MyServices
{
    public interface IUserProfileService
    {
        Task<ApiCommonResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiCommonResponse> FindUserById(long id);
        Task<ApiCommonResponse> FindUserByEmail(string email);
        Task<ApiCommonResponse> FindAllUsers();
        Task<ApiCommonResponse> FindAllUsersNotInAnSBU(long sbuId);
        Task<ApiCommonResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiCommonResponse> DeleteUserProfile(long userId);
        Task<ApiCommonResponse> UpdateUserRole(HttpContext context, long userId, List<RoleReceivingDTO> roles);
        Task<ApiCommonResponse> AssignUserToSBU(long userId, long SBUId);
        Task<ApiCommonResponse> DetachUserFromSBU(long id);
        Task<ApiCommonResponse> FetchAllUserProfilesWithEscalationLevelConfiguration();
    }
    public class UserProfileServiceImpl : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepo;
       // private readonly IMailAdapter _mailAdpater;
       // private readonly IModificationHistoryRepository _historyRepo ;
        private readonly HalobizContext _context;
        public UserProfileServiceImpl(IUserProfileRepository userRepo, 
           // IMailAdapter mailAdapter,
            HalobizContext context
          //  IModificationHistoryRepository historyRepo
          )
        {
            _userRepo = userRepo;
            //_mailAdpater = mailAdapter;
           // _historyRepo = historyRepo;
            _context = context;

        }

        public async Task<ApiCommonResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO)
        {
            if (
                !(userProfileReceivingDTO.Email.Trim().EndsWith("halogen-group.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("avanthalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("averthalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("armourxhalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("pshalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("academyhalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("armadahalogen.com"))
                )
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Invalid email address");
            }

            if (userProfileReceivingDTO.ImageUrl.Length > 255)
            {
                userProfileReceivingDTO.ImageUrl = userProfileReceivingDTO.ImageUrl.Substring(0, 255);
            }

            //todo fix mapping
            // var userProfile = Mapping.Mapper.Map<UserProfile>(userProfileReceivingDTO);
            //var userProfile = (UserProfile) userProfileReceivingDTO;
            //var savedUserProfile = await _userRepo.SaveUserProfile(userProfile);
            //if(savedUserProfile == null)
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            //}
            // var userProfileTransferDto = Mapping.Mapper.Map<UserProfileTransferDTO>(userProfile);
            //return CommonResponse.Send(ResponseCodes.SUCCESS, userProfile);
            return CommonResponse.Send(ResponseCodes.SUCCESS);

        }

        public async Task<ApiCommonResponse> FindUserById(long id)
        {
            var userProfile = await _userRepo.FindUserById(id);
            if (userProfile == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            //var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(userProfile);
            return CommonResponse.Send(ResponseCodes.SUCCESS, userProfile);
        }

        public async Task<ApiCommonResponse> FindUserByEmail(string email)
        {
            var userProfile = await _userRepo.FindUserByEmail(email);
            if (userProfile == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

          

           // var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(userProfile);
            return CommonResponse.Send(ResponseCodes.SUCCESS, userProfile);
        }


        public async Task<ApiCommonResponse> FindAllUsers()
        {
            var userProfiles = await _userRepo.FindAllUserProfile();
            if (userProfiles == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            //var userProfilesTransferDto = Mapping.Mapper.Map<IEnumerable<UserProfileTransferDTO>>(userProfiles);
            return CommonResponse.Send(ResponseCodes.SUCCESS, userProfiles);
        }

        public async Task<ApiCommonResponse> FindAllUsersNotInAnSBU(long sbuId)
        {
            var users = await _userRepo.FindAllUsersNotInAnProfile(sbuId);
            if (users == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, users);
        }

        public async Task<ApiCommonResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if (userToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var summary = $"Initial details before change, \n {userToUpdate.ToString()} \n";
            userToUpdate.Address = userProfileReceivingDTO.Address;
            userToUpdate.AltEmail = userProfileReceivingDTO.AltEmail;
            userToUpdate.AltMobileNumber = userProfileReceivingDTO.AltMobileNumber;
            userToUpdate.Email = userProfileReceivingDTO.Email;
            userToUpdate.FacebookHandle = userProfileReceivingDTO.FacebookHandle;
            userToUpdate.FirstName = userProfileReceivingDTO.FirstName;
            userToUpdate.ImageUrl = userProfileReceivingDTO.ImageUrl;
            userToUpdate.InstagramHandle = userProfileReceivingDTO.InstagramHandle;
            userToUpdate.LastName = userProfileReceivingDTO.LastName;
            userToUpdate.LinkedInHandle = userProfileReceivingDTO.LinkedInHandle;
            userToUpdate.MobileNumber = userProfileReceivingDTO.MobileNumber;
            userToUpdate.TwitterHandle = userProfileReceivingDTO.TwitterHandle;
            userToUpdate.StaffId = userProfileReceivingDTO.StaffId;
            userToUpdate.DateOfBirth = Convert.ToDateTime(userProfileReceivingDTO.DateOfBirth);
            userToUpdate.CodeName = userProfileReceivingDTO.CodeName;
            userToUpdate.OtherName = userProfileReceivingDTO.OtherName;

            summary += $"Details after change, \n {userToUpdate} \n";

            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if (updatedUser == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            // send the sign up mail when the user profile completion hits a 100%.
            if (!updatedUser.SignUpMailSent.Value)
            {
                if (ProfileIs100Percent(updatedUser))
                {
                    string serializedUser = JsonConvert.SerializeObject(updatedUser, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    //RunTask(async () => {
                    //    await _mailAdpater.SendNewUserSignup(serializedUser);
                    //});

                    //var superAdminEmails = superAdmins.Select(x => x.Email).ToArray();
                    //var serializedAdminEmails = JsonConvert.SerializeObject(superAdminEmails);

                    //RunTask(async () => {
                    //    await _mailAdpater.AssignRoleToNewUser(serializedUser, serializedAdminEmails);
                    //});

                    updatedUser.SignUpMailSent = true;
                    updatedUser = await _userRepo.UpdateUserProfile(updatedUser);
                    if (updatedUser == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }
            }


            //ModificationHistory history = new ModificationHistory(){
            //    ModelChanged = "UserProfile",
            //    ChangeSummary = summary,
            //    ChangedBy = updatedUser,
            //    ModifiedModelId = updatedUser.Id
            //};

            //await _historyRepo.SaveHistory(history);

            //var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return CommonResponse.Send(ResponseCodes.SUCCESS, updatedUser);

        }
        public async Task<ApiCommonResponse> AssignUserToSBU(long userId, long SBUId)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if (userToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            userToUpdate.Sbuid = SBUId;


            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if (updatedUser == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            //ModificationHistory history = new ModificationHistory(){
            //    ModelChanged = "UserProfile",
            //    ChangeSummary = "Assigned user to an SBU",
            //    ChangedBy = updatedUser,
            //    ModifiedModelId = updatedUser.Id
            //};

            //await _historyRepo.SaveHistory(history);

            //var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return CommonResponse.Send(ResponseCodes.SUCCESS, updatedUser);

        }

        public async Task<ApiCommonResponse> DetachUserFromSBU(long userId)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if (userToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            userToUpdate.Sbuid = null;


            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if (updatedUser == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            //ModificationHistory history = new ModificationHistory()
            //{
            //    ModelChanged = "UserProfile",
            //    ChangeSummary = "Detached user from an SBU",
            //    ChangedBy = updatedUser,
            //    ModifiedModelId = updatedUser.Id
            //};

            //await _historyRepo.SaveHistory(history);

            //var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return CommonResponse.Send(ResponseCodes.SUCCESS, updatedUser);

        }

        public async Task<ApiCommonResponse> UpdateUserRole(HttpContext context, long userId, List<RoleReceivingDTO> roles)
        {
            try
            {
                var userToUpdate = await _userRepo.FindUserById(userId);
                if (userToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "User not found");
                }

                var thisUserId = long.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out long userIdClaim) ?
                userIdClaim : 31;
                if (thisUserId == userId)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You cannot add/update role for yourself");
                }

                //remove all the role assignment for this user
                var oldUserRoles = _context.UserRoles.Where(x => x.UserId == userId);
                _context.RemoveRange(oldUserRoles);
                await _context.SaveChangesAsync();

                List<UserRole> userRoles = new List<UserRole>();

                foreach (var item in roles)
                {
                    userRoles.Add(new UserRole { UserId = userId, RoleId = item.Id, CreatedById = thisUserId });
                }

                //ModificationHistory history = new ModificationHistory()
                //{
                //    ModelChanged = "UserRole",
                //    ChangeSummary = "Changes to roles",
                //    ChangedBy = $"{thisUserId}",
                //    ModifiedModelId = updatedUser.Id
                //};

                //await _historyRepo.SaveHistory(history);

                await _context.UserRoles.AddRangeAsync(userRoles);
                await _context.SaveChangesAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, userRoles);
            }
            catch (Exception ex)
            {
                var p = ex.StackTrace;
            }

            return null;

        }

        //public async Task<ApiCommonResponse> UpdateUserRole(long userId, long roleId)
        //{
        //    var userToUpdate = await _userRepo.FindUserById(userId);
        //    if (userToUpdate == null)
        //    {
        //        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
        //    }
        //    var summary = $"Initial details before change, \n {userToUpdate} \n";
        //    userToUpdate.RoleId = roleId;

        //    summary += $"Details after change, \n {userToUpdate} \n";

        //    var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

        //    if (updatedUser == null)
        //    {
        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
        //    }

        //    var serializedUser = JsonConvert.SerializeObject(updatedUser, new JsonSerializerSettings { 
        //         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });

        //    RunTask(async () => {
        //        await _mailAdpater.SendUserAssignedToRoleMail(serializedUser);
        //    });       

        //    ModificationHistory history = new ModificationHistory()
        //    {
        //        ModelChanged = "UserProfile",
        //        ChangeSummary = summary,
        //        ChangedBy = updatedUser,
        //        ModifiedModelId = updatedUser.Id
        //    };

        //    await _historyRepo.SaveHistory(history);

        //    var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
        //    return CommonResponse.Send(ResponseCodes.SUCCESS,userProfileTransferDto);
        //}


        public async Task<ApiCommonResponse> DeleteUserProfile(long userId)
        {
            var userToDelete = await _userRepo.FindUserById(userId);
            if (userToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _userRepo.RemoveUserProfile(userToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        private bool ProfileIs100Percent(UserProfile userProfile)
        {
            return !string.IsNullOrWhiteSpace(userProfile.Address) &&
                !string.IsNullOrWhiteSpace(userProfile.AltEmail) &&
                !string.IsNullOrWhiteSpace(userProfile.AltMobileNumber) &&
                !string.IsNullOrWhiteSpace(userProfile.CodeName) &&
                userProfile.DateOfBirth != default(DateTime) &&
                !string.IsNullOrWhiteSpace(userProfile.Email) &&
                !string.IsNullOrWhiteSpace(userProfile.FirstName) &&
                !string.IsNullOrWhiteSpace(userProfile.ImageUrl) &&
                !string.IsNullOrWhiteSpace(userProfile.LastName) &&
                !string.IsNullOrWhiteSpace(userProfile.MobileNumber) &&
                !string.IsNullOrWhiteSpace(userProfile.OtherName);
        }

        private void RunTask(Action action)
        {
            Task.Run(action);
        }

        public async Task<ApiCommonResponse> FetchAllUserProfilesWithEscalationLevelConfiguration()
        {
            var resultObject = await _userRepo.FetchAllUserProfilesWithEscalationLevelConfiguration();
            return CommonResponse.Send(ResponseCodes.SUCCESS, resultObject);
        }
    }
}