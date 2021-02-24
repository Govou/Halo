using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Repository;
using Newtonsoft.Json;

namespace HaloBiz.MyServices.Impl
{
    public class UserProfileServiceImpl : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IMailAdapter _mailAdpater;
        private readonly IModificationHistoryRepository _historyRepo ;
        public UserProfileServiceImpl(IUserProfileRepository userRepo, IMapper mapper, 
            IMailAdapter mailAdapter,
        IModificationHistoryRepository historyRepo )
        {
            this._mapper = mapper;
            this._userRepo = userRepo;
            _mailAdpater = mailAdapter;
            _historyRepo = historyRepo;

        }

        public async Task<ApiResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO)
        {
            if(
                !(userProfileReceivingDTO.Email.Trim().EndsWith("halogen-group.com") || 
                userProfileReceivingDTO.Email.Trim().EndsWith("avanthalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("averthalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("armourxhalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("pshalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("academyhalogen.com") ||
                userProfileReceivingDTO.Email.Trim().EndsWith("armadahalogen.com") )
                )
            {
                return new ApiResponse(400, "Invalid email address");
            }

            var userProfile = _mapper.Map<UserProfile>(userProfileReceivingDTO);
            var savedUserProfile = await _userRepo.SaveUserProfile(userProfile);
            if(savedUserProfile == null)
            {
                return new ApiResponse(500);
            }
            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(userProfile);
            return new ApiOkResponse(userProfileTransferDto);
        }

        public async Task<ApiResponse> FindUserById(long id)
        {
            var userProfile = await _userRepo.FindUserById(id);
            if(userProfile == null)
            {
                return new ApiResponse(404);
            }
            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(userProfile);
            return new ApiOkResponse(userProfileTransferDto);
        }

        public async Task<ApiResponse> FindUserByEmail(string email)
        {
            var userProfile = await _userRepo.FindUserByEmail(email);
            if(userProfile == null)
            {
                return new ApiResponse(404);
            }
            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(userProfile);
            return new ApiOkResponse(userProfileTransferDto);
        }
        

        public async Task<ApiResponse> FindAllUsers()
        {
            var userProfiles = await _userRepo.FindAllUserProfile();
            if(userProfiles == null )
            {
                return new ApiResponse(404);
            }
            var userProfilesTransferDto = _mapper.Map<IEnumerable<UserProfileTransferDTO>>(userProfiles);
            return new ApiOkResponse(userProfilesTransferDto);
        }

        public async  Task<ApiResponse> FindAllUsersNotInAnSBU(long sbuId)
        {
            var users = await _userRepo.FindAllUsersNotInAnProfile(sbuId);
            if(users == null )
            {
                return new ApiResponse(404);
            } 
            return new ApiOkResponse(users);
        }

        public async Task<ApiResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if(userToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {userToUpdate.ToString()} \n" ;
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

            summary += $"Details after change, \n {userToUpdate.ToString()} \n";

            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if(updatedUser == null)
            {
                return new ApiResponse(500);
            }

            // send the sign up mail when the user profile completion hits a 100%.
            if (!updatedUser.SignUpMailSent)
            {
                if (ProfileIs100Percent(updatedUser))
                {
                    string serializedUser = JsonConvert.SerializeObject(updatedUser);
                    RunTask(async () => {
                        await _mailAdpater.SendNewUserSignup(serializedUser);
                    });                                        

                    var superAdmins = await _userRepo.FindAllSuperAdmins();
                    if(superAdmins == null)
                    {
                        return new ApiResponse(500);
                    }

                    var superAdminEmails = superAdmins.Select(x => x.Email).ToArray();
                    var serializedAdminEmails = JsonConvert.SerializeObject(superAdminEmails);

                    RunTask(async () => {
                        await _mailAdpater.AssignRoleToNewUser(serializedUser, serializedAdminEmails);
                    });

                    updatedUser.SignUpMailSent = true;
                    updatedUser = await _userRepo.UpdateUserProfile(updatedUser);
                    if (updatedUser == null)
                    {
                        return new ApiResponse(500);
                    }
                }
            }    
      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "UserProfile",
                ChangeSummary = summary,
                ChangedBy = updatedUser,
                ModifiedModelId = updatedUser.Id
            };

            await _historyRepo.SaveHistory(history);

            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return new ApiOkResponse(userProfileTransferDto);

        }
        public async Task<ApiResponse> AssignUserToSBU(long userId, long SBUId)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if(userToUpdate == null)
            {
                return new ApiResponse(404);
            }
            userToUpdate.SBUId = SBUId;


            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if(updatedUser == null)
            {
                return new ApiResponse(500);
            }

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "UserProfile",
                ChangeSummary = "Assigned user to an SBU",
                ChangedBy = updatedUser,
                ModifiedModelId = updatedUser.Id
            };

            await _historyRepo.SaveHistory(history);

            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return new ApiOkResponse(userProfileTransferDto);

        }

        public async Task<ApiResponse> DetachUserFromSBU(long userId)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if (userToUpdate == null)
            {
                return new ApiResponse(404);
            }
            userToUpdate.SBUId = null;


            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if (updatedUser == null)
            {
                return new ApiResponse(500);
            }

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "UserProfile",
                ChangeSummary = "Detached user from an SBU",
                ChangedBy = updatedUser,
                ModifiedModelId = updatedUser.Id
            };

            await _historyRepo.SaveHistory(history);

            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return new ApiOkResponse(userProfileTransferDto);

        }

        public async Task<ApiResponse> UpdateUserRole(long userId, long roleId)
        {
            var userToUpdate = await _userRepo.FindUserById(userId);
            if (userToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {userToUpdate.ToString()} \n";
            userToUpdate.RoleId = roleId;

            summary += $"Details after change, \n {userToUpdate.ToString()} \n";

            var updatedUser = await _userRepo.UpdateUserProfile(userToUpdate);

            if (updatedUser == null)
            {
                return new ApiResponse(500);
            }

            var serializedUser = JsonConvert.SerializeObject(updatedUser, new JsonSerializerSettings { 
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            RunTask(async () => {
                await _mailAdpater.SendUserAssignedToRoleMail(serializedUser);
            });       

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "UserProfile",
                ChangeSummary = summary,
                ChangedBy = updatedUser,
                ModifiedModelId = updatedUser.Id
            };

            await _historyRepo.SaveHistory(history);

            var userProfileTransferDto = _mapper.Map<UserProfileTransferDTO>(updatedUser);
            return new ApiOkResponse(userProfileTransferDto);
        }


        public async Task<ApiResponse> DeleteUserProfile(long userId)
        {
            var userToDelete = await _userRepo.FindUserById(userId);
            if(userToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(! await _userRepo.RemoveUserProfile(userToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
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
    }
}