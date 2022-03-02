using System;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Helpers;
//using Microsoft.AspNetCore.Http;
using HaloBiz.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HaloBiz.Model;
using HalobizMigrations.Models.Halobiz;
using HaloBiz.DTOs.ContactDTO;
using System.Linq;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Visitors;
using HalobizMigrations.Models.ProjectManagement;
using System.Collections.Generic;

namespace HaloBiz.MyServices.Impl
{
    public class ContactServiceImpl:IContactServiceImpl
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ContactServiceImpl> _logger;
        private readonly IMapper _mapper;

        public ContactServiceImpl(HalobizContext context, ILogger<ContactServiceImpl> logger, IMapper mapper)
        {
         
        this._mapper = mapper;
        this._logger = logger;
        this._context = context;
        
       }

        public async Task<ApiCommonResponse> AddNewContact(HttpContext context,Contactdto contactDTO)
        {


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                    {
                    //Check if the user with the same lastname,email or phonenumber already exist
                    var contactInstance = await _context.Contacts.FirstOrDefaultAsync(x => x.Email  == contactDTO.Email && x.IsDeleted == false
                                                                                      || x.LastName == contactDTO.LastName && x.IsDeleted == false
                                                                                      || x.Mobile == contactDTO.Mobile && x.IsDeleted == false);
                    if (contactInstance != null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE,null,"A user with the same Email,LastName or Mobile number already exists"); ;
                    }

                    //Map Dto to constact, add the logged in user info and save the contact
                    var newContact = _mapper.Map<Contact>(contactDTO);

                    newContact.CreatedById = context.GetLoggedInUserId();
                    newContact.IsDeleted = false;
                    newContact.CreatedAt = DateTime.Now;
                    newContact.Mobile = contactDTO.Mobile.Replace("+234-", "0");
                    newContact.Mobile2 = contactDTO.Mobile2.Replace("+234-", "0");

                    var contactEntity = await _context.Contacts.AddAsync(newContact);
                    await _context.SaveChangesAsync();
                    var savedContact = contactEntity.Entity;
                    await transaction.CommitAsync();
                    
                    return CommonResponse.Send(ResponseCodes.SUCCESS, savedContact);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }


        public async Task<ApiCommonResponse> disableContact(HttpContext httpContext,long contactId)
        {
            var contact = await _context.Contacts.Where(x => x.IsDeleted == false && x.Id == contactId).FirstOrDefaultAsync();
            if(contact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            else
            {
                contact.IsDeleted = true;
                _context.Contacts.Update(contact);
                _context.SaveChanges();
                return CommonResponse.Send(ResponseCodes.SUCCESS);

            }



        }


        public async Task<ApiCommonResponse> updateContact(HttpContext httpContext, long contactId, Contactdto contactdto)
        {
            var contactToUpdate = await _context.Contacts.Where(x => x.IsDeleted == false && x.Id == contactId).FirstOrDefaultAsync();
            if (contactToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
               
            }
            else
            {
                var summary = $"Initial details before change, \n {contactToUpdate.ToString()} \n";
                contactToUpdate.DateOfBirth = contactdto.DateOfBirth;
                contactToUpdate.Email = contactdto.Email;
                contactToUpdate.Email2 = contactdto.Email2;
                contactToUpdate.Facebook = contactdto.Facebook;
                contactToUpdate.FirstName = contactdto.FirstName;
                contactToUpdate.Gender = contactdto.Gender;
                contactToUpdate.Instagram = contactdto.Instagram;
                contactToUpdate.IsDeleted = contactdto.IsDeleted;
                contactToUpdate.LastName = contactdto.LastName;
                contactToUpdate.MiddleName = contactdto.MiddleName;
                contactToUpdate.Mobile = contactdto.Mobile;
                contactToUpdate.Mobile2 = contactdto.Mobile2;
                contactToUpdate.ProfilePicture = contactdto.ProfilePicture;
                contactToUpdate.Title = contactdto.Title;
                contactToUpdate.Twitter = contactdto.Twitter;

                var mappedUpdatedContact = _mapper.Map<Contact>(contactToUpdate);

                var updatedContacts = _context.Contacts.Update(mappedUpdatedContact).Entity;
                await _context.SaveChangesAsync();

                summary += $"Details after change, \n {updatedContacts.ToString()} \n";

                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "Contact",
                    ChangeSummary = summary,
                    ChangedById = httpContext.GetLoggedInUserId(),
                    ModifiedModelId = updatedContacts.Id
                };

                await _context.ModificationHistories.AddAsync(history);
                await _context.SaveChangesAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, updatedContacts);
            }
        }

        

        public async Task<ApiCommonResponse> GetAllContact(HttpContext httpContext)
        {
            
            var contact = await _context.Contacts.Where(x => x.IsDeleted == false && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            if (contact == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, contact);
            }
            
        }

        public async Task<ApiCommonResponse> GetAllSuspect()
        {

            var suspect = await _context.Suspects.Where(x => x.IsDeleted == false).ToListAsync();
            if (suspect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, suspect);
            }

        }


        public async Task<ApiCommonResponse> AttachToSuspect(HttpContext httpContext,SuspectContactDTO suspectContactDTO)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    
                    //Check if the user with the same lastname,email or phonenumber already exist
                    var checkIfCntactExist = await _context.SuspectContacts.Where(x => x.IsDeleted == false
                                                             && x.SuspectId == suspectContactDTO.SuspectId
                                                             && x.ContactId == suspectContactDTO.ContactId)
                                                            .FirstOrDefaultAsync();

                    if (checkIfCntactExist != null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE,null,"This contact has already been attached to this suspect");
                    }

                    //Map Dto to constact, add the logged in user info and save the contact
                    var newContact = _mapper.Map<SuspectContact>(suspectContactDTO);
                    newContact.IsDeleted = false;
                    var contactEntity = await _context.SuspectContacts.AddAsync(newContact);
                    await _context.SaveChangesAsync();
                    var savedContact = contactEntity.Entity;
                    await transaction.CommitAsync();


                    return CommonResponse.Send(ResponseCodes.SUCCESS, savedContact);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }


        }

        public async Task<ApiCommonResponse> GetContactsAttachedToSuspect(long suspectId)
        {

            var suspect = await _context.SuspectContacts.Where(x => x.IsDeleted == false && x.SuspectId == suspectId)
                                                              .Include(x=>x.Contact)
                                                              .Include(x=>x.Suspect)
                                                              .ToListAsync();
            if (suspect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, suspect);
            }

            //_context.Meetings
        }

        public async Task<ApiCommonResponse> GetMeeting(long suspectId)
        {

            var meetings = await _context.Meetings.Where(x => x.IsActive == true && x.SuspectId == suspectId)
                                                              .Include(x => x.ContactsInvolved.Where(x=>x.IsActive == true))
                                                              .Include(x => x.StaffsInvolved.Where(x=>x.IsActive == true))
                                                              .ToListAsync();
            

            if (meetings == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                var meetingArray = new List<Meeting>();
                foreach (var meeting in meetings)
                {
                    meeting.ContactsInvolved = meeting.ContactsInvolved.GroupBy(p => p.ContactId)
                              .Select(result => result.First())
                              .ToArray();
                    meeting.StaffsInvolved = meeting.StaffsInvolved.GroupBy(p => p.StaffId)
                              .Select(result => result.First())
                              .ToArray();

                    meetingArray.Add(meeting);
                }
                return CommonResponse.Send(ResponseCodes.SUCCESS, meetingArray);
            }

            //_context.Meetings
        }


        public async Task<ApiCommonResponse> createMeeting(HttpContext httpContext,MeetingDTO meetingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfCntactExist = await _context.Suspects.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == meetingDTO.SuspectId);
                    if (checkIfCntactExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    var checkIfNameExist = await _context.Meetings.FirstOrDefaultAsync(x => x.IsActive == true && x.Caption.Trim() == meetingDTO.Caption.Trim());

                    if(checkIfNameExist != null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE,null,"Please use another meeting caption,the current one already exist");
                    }

                    var meetingInstance = new Meeting();
                    meetingInstance.Caption = meetingDTO.Caption;
                    meetingInstance.CreatedAt = DateTime.Now;
                    meetingInstance.CreatedById = httpContext.GetLoggedInUserId();
                    meetingInstance.Description = meetingDTO.Description;
                    meetingInstance.EndDate = meetingDTO.EndDate;
                    meetingInstance.IsActive = true;
                    meetingInstance.MeetingType = meetingDTO.MeetingType;
                    meetingInstance.Reason = meetingDTO.Reason;
                    meetingInstance.StartDate = meetingDTO.StartDate;
                    meetingInstance.SuspectId = meetingDTO.SuspectId;

                    var contactEntity = await _context.Meetings.AddAsync(meetingInstance);
                    await _context.SaveChangesAsync();
                    var savedMeeting = contactEntity.Entity;

                    if(meetingDTO.ContactsInvolved != null)
                    {
                        var contactList = new List<MeetingContact>();
                        foreach (var contact in meetingDTO.ContactsInvolved)
                        {
                            var contactInstance = new MeetingContact();
                            contactInstance.ContactId = contact.ContactId;
                            contactInstance.CreatedById = httpContext.GetLoggedInUserId();
                            contactInstance.IsActive = true;
                            contactInstance.MeetingId = savedMeeting.Id;
                            contactInstance.Name = contact.Name;
                            contactInstance.ProfilePicture = contact.ProfilePicture;
                            contactList.Add(contactInstance);
                        }
                        await _context.MeetingContacts.AddRangeAsync(contactList);
                        await _context.SaveChangesAsync();
                    }
                   


                    if (meetingDTO.StaffsInvolved != null)
                    {
                        var staffList = new List<MeetingStaff>();
                        foreach (var contact in meetingDTO.StaffsInvolved)
                        {
                            var staffInstance = new MeetingStaff();
                            staffInstance.StaffId = contact.StaffId;
                            staffInstance.CreatedById = httpContext.GetLoggedInUserId();
                            staffInstance.IsActive = true;
                            staffInstance.MeetingId = savedMeeting.Id;
                            staffInstance.StaffName = contact.StaffName;
                            staffInstance.StaffProfileImg = contact.StaffProfileImg;
                            staffList.Add(staffInstance);
                        }
                        await _context.MeetingStaffs.AddRangeAsync(staffList);
                        await _context.SaveChangesAsync();
                    }
                   
                    await transaction.CommitAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, savedMeeting);

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }


        public async Task<ApiCommonResponse> updateMeeting(HttpContext httpContext, long meetingId,MeetingDTO meetingDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var meetingInstance = await _context.Meetings.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == meetingId);
                    if (meetingInstance == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    meetingInstance.Caption = meetingDTO.Caption;
                    meetingInstance.Description = meetingDTO.Description;
                    meetingInstance.EndDate = meetingDTO.EndDate;
                    meetingInstance.MeetingType = meetingDTO.MeetingType;
                    meetingInstance.Reason = meetingDTO.Reason;
                    meetingInstance.StartDate = meetingDTO.StartDate;
                    var contactEntity = _context.Meetings.Update(meetingInstance);
                    await _context.SaveChangesAsync();
                    var editedMeeting = contactEntity.Entity;
                    await transaction.CommitAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, editedMeeting);

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }


        public async Task<ApiCommonResponse> AddmoreStaff(HttpContext httpContext, long meetingId, List<MeetingStaff> meetingDTO)
        {

            var getStaffsById = await _context.MeetingStaffs.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();

            if(getStaffsById != null)
            {
                var staffArray = new List<MeetingStaff>();
                foreach (var user in meetingDTO)
                {
                    if (getStaffsById.Any(cus => cus.StaffId == user.StaffId))
                    {
                        continue;
                    }
                    else
                    {
                        var staffInstance = new MeetingStaff();
                        staffInstance.CreatedAt = DateTime.Now;
                        staffInstance.CreatedById = httpContext.GetLoggedInUserId();
                        staffInstance.IsActive = true;
                        staffInstance.MeetingId = meetingId;
                        staffInstance.StaffId = user.StaffId;
                        staffInstance.StaffName = user.StaffName;
                        staffInstance.StaffProfileImg = user.StaffProfileImg;
                        staffArray.Add(staffInstance);
                    }
                }

                await _context.MeetingStaffs.AddRangeAsync(staffArray);
                await _context.SaveChangesAsync();
                var remainStaff = await _context.MeetingStaffs.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();
                var groupedResult = remainStaff.GroupBy(p => p.StaffId)
                          .Select(result => result.First())
                          .ToArray();
                return CommonResponse.Send(ResponseCodes.SUCCESS, groupedResult, "Successffully saved");
            }

           

            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");

        }

        public async Task<ApiCommonResponse> AddmoreContact(HttpContext httpContext, long meetingId, List<MeetingContact> meetingDTO)

        {
            var getConstactsId = await _context.MeetingContacts.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();

            if (getConstactsId != null)
            {
                var contactArray = new List<MeetingContact>();
                foreach (var user in meetingDTO)
                {
                    if (getConstactsId.Any(cus => cus.ContactId == user.ContactId))
                    {
                        continue;
                    }
                    else
                    {
                        var contactInstance = new MeetingContact();
                        contactInstance.CreatedAt = DateTime.Now;
                        contactInstance.CreatedById = httpContext.GetLoggedInUserId();
                        contactInstance.IsActive = true;
                        contactInstance.MeetingId = meetingId;
                        contactInstance.ContactId = user.ContactId;
                        contactInstance.Name = user.Name;
                        contactInstance.ProfilePicture = user.ProfilePicture;
                        contactArray.Add(contactInstance);
                    }

                }

                await _context.MeetingContacts.AddRangeAsync(contactArray);
                await _context.SaveChangesAsync();
                var remainContacts = await _context.MeetingContacts.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();
                var groupedResult = remainContacts.GroupBy(p => p.ContactId)
                         .Select(result => result.First())
                         .ToArray();
                return CommonResponse.Send(ResponseCodes.SUCCESS, groupedResult, "Successfully saved Contact");
               
            }

            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");

        }

        public async Task<ApiCommonResponse> AddmoreResponsible(HttpContext httpContext, long todoId, List<PMResponsible> responsibles)
        {
            if (responsibles != null)
            {

                var responsiblesArray = new List<PMResponsible>();
                foreach (var responsible in responsibles)
                {
                    var responsibleInstance = new PMResponsible();
                    responsibleInstance.CreatedAt = DateTime.Now;
                    responsibleInstance.CreatedById = httpContext.GetLoggedInUserId();
                    responsibleInstance.IsActive = true;
                    responsibleInstance.ToDoId = todoId;
                    responsibleInstance.Fullname = responsible.Fullname;
                    responsibleInstance.ImageUrl = responsible.ImageUrl;
                    responsiblesArray.Add(responsibleInstance);
                }

                await _context.PMResponsibles.AddRangeAsync(responsiblesArray);
                await _context.SaveChangesAsync();
                var remainResponsible = await _context.PMResponsibles.Where(x => x.IsActive == true && x.ToDoId == todoId).ToListAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, remainResponsible, "Successfully saved");

            }

            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");

        }





        public async Task<ApiCommonResponse> removeContact(HttpContext httpContext, long meetingId,long contactId)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfContactExist = await _context.MeetingContacts.FirstOrDefaultAsync(x => x.IsActive == true && x.MeetingId == meetingId && x.ContactId == contactId);
                    if (checkIfContactExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    checkIfContactExist.IsActive = false;
                    _context.MeetingContacts.Update(checkIfContactExist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var remainContacts = await _context.MeetingContacts.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();
                    var groupedResult = remainContacts.GroupBy(p => p.ContactId)
                        .Select(result => result.First())
                        .ToArray();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, groupedResult, "Successfully removed Contact");

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        public async Task<ApiCommonResponse> removeResponsible(HttpContext httpContext, long todoId, long responsibleId)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfResponsibleExist = await _context.PMResponsibles.FirstOrDefaultAsync(x => x.IsActive == true && x.ToDoId == todoId && x.Id == responsibleId);
                    if (checkIfResponsibleExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    checkIfResponsibleExist.IsActive = false;
                    _context.PMResponsibles.Update(checkIfResponsibleExist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var remainResponsible = await _context.PMResponsibles.Where(x => x.IsActive == true && x.ToDoId == todoId).ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, remainResponsible,"Successfully removed user");

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        public async Task<ApiCommonResponse> removeStaff(HttpContext httpContext, long meetingId, long staffId)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfStaffExist = await _context.MeetingStaffs.Where(x => x.IsActive == true && x.MeetingId == meetingId && x.StaffId == staffId).FirstOrDefaultAsync();
                    if (checkIfStaffExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null,"This user ws not found");
                    }

                    checkIfStaffExist.IsActive = false;
                    _context.MeetingStaffs.Update(checkIfStaffExist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var remainStaff = await _context.MeetingStaffs.Where(x => x.IsActive == true && x.MeetingId == meetingId).ToListAsync();
                    var groupedResult = remainStaff.GroupBy(p => p.StaffId)
                       .Select(result => result.First())
                       .ToArray();
                    return CommonResponse.Send(ResponseCodes.SUCCESS,groupedResult,"Successfully removed staff");

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }


        public async Task<ApiCommonResponse> removeMeeting(HttpContext httpContext, long meetingId)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfMeetingExist = await _context.Meetings.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == meetingId);
                    if (checkIfMeetingExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    checkIfMeetingExist.IsActive = false;
                    _context.Meetings.Update(checkIfMeetingExist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS);

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }



        public async Task<ApiCommonResponse> detachContact(HttpContext httpContext,long suspectid,long contactId)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var checkIfCntactExist = await _context.SuspectContacts.FirstOrDefaultAsync(x => x.IsDeleted == false && x.SuspectId == suspectid && x.ContactId == contactId);
                    if (checkIfCntactExist == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    checkIfCntactExist.IsDeleted = true;
                    _context.SuspectContacts.Update(checkIfCntactExist);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS);

                    //_context.Meetings
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        public async Task<ApiCommonResponse> CreateGoal(HttpContext httpContext, long suspectid, GoalDTO goalDTO)
        {
            var goalToBeSaved = new Goal();
            goalToBeSaved.Caption = goalDTO.Caption;
            goalToBeSaved.Description = goalDTO.Description;
            goalToBeSaved.IsActive = true;
            goalToBeSaved.SuspectId = suspectid;
            goalToBeSaved.CreatedAt = DateTime.Now;
            goalToBeSaved.CreatedById = httpContext.GetLoggedInUserId();

            var goalEntity = await _context.Goals.AddAsync(goalToBeSaved);
            await _context.SaveChangesAsync();
            var savedGoal = goalEntity.Entity;

            return CommonResponse.Send(ResponseCodes.SUCCESS, savedGoal, "Successfully saved Goal");
        }


        public async Task<ApiCommonResponse> updateGoal(HttpContext httpContext, long goalId, GoalDTO goalDTO)
        {

            var goalToBeUpdated = await _context.Goals.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == goalId);

            goalToBeUpdated.Description = goalDTO.Description;
            goalToBeUpdated.Caption = goalDTO.Caption;

            var goalEntity = _context.Goals.Update(goalToBeUpdated);
            await _context.SaveChangesAsync();
            var editGoal = goalEntity.Entity;
            return CommonResponse.Send(ResponseCodes.SUCCESS, editGoal);
        }

        public async Task<ApiCommonResponse> changeTodoStatus(HttpContext httpContext, long todoId)
        {

            var todoToUpdate = await _context.ToDos.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == todoId);

            todoToUpdate.Status = true;

            var todoEntity = _context.ToDos.Update(todoToUpdate);
            await _context.SaveChangesAsync();
            var editTodo = todoEntity.Entity;
            return CommonResponse.Send(ResponseCodes.SUCCESS, editTodo);
        }





        public async Task<ApiCommonResponse> getContactsForSuspectsById(HttpContext httpContext, long suspectId)
        {

            var getContactsById = await _context.SuspectContacts.Where(x => x.IsDeleted == false && x.SuspectId == suspectId)
                                                   .Include(x => x.Contact)
                                                   .ToListAsync();

            var contacts = new List<Contact>();
            if(getContactsById != null)
            {
                foreach(var contact in getContactsById)
                {
                   if(contact.Contact.IsDeleted == false)
                    {
                        contacts.Add(contact.Contact);
                    }

                }

                return CommonResponse.Send(ResponseCodes.SUCCESS, contacts);
            }

            return CommonResponse.Send(ResponseCodes.FAILURE, null,"No value was found");
        }


        public async Task<ApiCommonResponse> removeGoal(HttpContext httpContext, long goalId)
        {

            var goalToBeUpdated = await _context.Goals.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == goalId);

            goalToBeUpdated.IsActive = false;
            var goalEntity = _context.Goals.Update(goalToBeUpdated);
            await _context.SaveChangesAsync();
            var editGoal = goalEntity.Entity;
            return CommonResponse.Send(ResponseCodes.SUCCESS, editGoal);
        }

        public async Task<ApiCommonResponse> getGoalsBySuspectId(HttpContext httpContext, long suspectId)
        {

            var goalTobeFound = await _context.Goals.Where(x => x.IsActive == true && x.SuspectId == suspectId)
                                                    .Include(x => x.ToDos.Where(x => x.IsActive == true))
                                                    .ToListAsync();

            if(goalTobeFound == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, goalTobeFound);
            }
            
        }

        public async Task<ApiCommonResponse> CreateTodo(HttpContext httpContext, long goalId, TodoDTO todoDTO)
        {
            var todoToBeSaved = new ToDo();
            todoToBeSaved.Caption = todoDTO.Caption;
            todoToBeSaved.Description = todoDTO.Description;
            todoToBeSaved.IsActive = true;
            todoToBeSaved.GoalId = goalId;
            todoToBeSaved.CreatedAt = DateTime.Now;
            todoToBeSaved.CreatedById = httpContext.GetLoggedInUserId();
            todoToBeSaved.DueDate = todoDTO.DueDate;
            todoToBeSaved.Status = false;
            var todoEntity = await _context.ToDos.AddAsync(todoToBeSaved);
            await _context.SaveChangesAsync();
            if (todoDTO.Responsible != null)
            {
                var responsibleArray = new List<PMResponsible>();
                foreach(var responsible in todoDTO.Responsible)
                {
                    var responsibleInstance = new PMResponsible();
                    responsibleInstance.CreatedAt = DateTime.Now;
                    responsibleInstance.CreatedById = httpContext.GetLoggedInUserId();
                    responsibleInstance.Fullname = responsible.Fullname;
                    responsibleInstance.ImageUrl = responsible.ImageUrl;
                    responsibleInstance.IsActive = true;
                    responsibleInstance.ToDoId = todoToBeSaved.Id;

                    responsibleArray.Add(responsibleInstance);
                }
                await _context.PMResponsibles.AddRangeAsync(responsibleArray);
                await _context.SaveChangesAsync();

            }

            
            var savedTodo = todoEntity.Entity;

            return CommonResponse.Send(ResponseCodes.SUCCESS, savedTodo, "Successfully saved Todo");
        }

        public async Task<ApiCommonResponse> UpdateTodo(HttpContext httpContext, long todoId, TodoDTO todoDTO)
        {
            var todoToBeUpdated = await _context.ToDos.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == todoId);

            if(todoToBeUpdated == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not find Todo");
            }

            todoToBeUpdated.Caption = todoDTO.Caption;
            todoToBeUpdated.Description = todoDTO.Description;
            todoToBeUpdated.DueDate = todoDTO.DueDate;
            todoToBeUpdated.Status = todoDTO.Status;

            var todoUpdatedeEntity = _context.ToDos.Update(todoToBeUpdated);
            await _context.SaveChangesAsync();
            var editTodo = todoUpdatedeEntity.Entity;
            return CommonResponse.Send(ResponseCodes.SUCCESS, editTodo);
        }

        public async Task<ApiCommonResponse> removeTodo(HttpContext httpContext, long todoId)
        {

            var todoToBeUpdated = await _context.ToDos.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == todoId);

            todoToBeUpdated.IsActive = false;
            var todoToBeSaved = _context.ToDos.Update(todoToBeUpdated);
            await _context.SaveChangesAsync();
            var editTodo = todoToBeSaved.Entity;
            return CommonResponse.Send(ResponseCodes.SUCCESS, editTodo);
        }

        public async Task<ApiCommonResponse> getTodoByGoalId(HttpContext httpContext, long goalId)
        {

            var todoToBeFound = await _context.ToDos.Where(x => x.IsActive == true && x.GoalId == goalId)
                                                    .Include(x=>x.Responsibles.Where(x=>x.IsActive == true))
                                                     .ToListAsync();

            if (todoToBeFound == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS, todoToBeFound);
            }

        }


        public async Task<ApiCommonResponse> getDashBoardForSuspect(HttpContext httpContext, long suspectId)
        {

            var getSuspectContactsById = await _context.SuspectContacts.Where(x => x.IsDeleted == false && x.SuspectId == suspectId)
                                                       .Include(x => x.Contact)
                                                       .ToListAsync();

            var getMeetingsBySuspectId = await _context.Meetings.Where(x => x.IsActive == true && x.SuspectId == suspectId)
                                                        .Include(x => x.ContactsInvolved.Where(x => x.IsActive == true))
                                                        .Include(x => x.StaffsInvolved.Where(x => x.IsActive == true))
                                                        .ToListAsync();

            var getGoalAndTodo = await _context.Goals.Where(x => x.IsActive == true && x.SuspectId == suspectId)
                                                      .Include(x => x.ToDos.Where(x => x.IsActive == true && x.DueDate.Date == DateTime.Now.Date))
                                                              .ThenInclude(x => x.Responsibles.Where(x => x.IsActive == true))
                                                      .ToListAsync();

            var suspectContactsArray = new List<SuspectContact>();

            if (getSuspectContactsById.Any())
            {
                foreach (var contact in getSuspectContactsById)
                {
                    if (contact.IsDeleted == false)
                    {
                        suspectContactsArray.Add(contact);
                    }

                }
            }


            var todoList = new List<ToDo>();
            if(getGoalAndTodo != null)
            {
                foreach (var goal in getGoalAndTodo)
                {
                    todoList.AddRange(goal.ToDos);
                }

            }

            var getDistinctTodo = todoList.GroupBy(p => p.Id)
                              .Select(result => result.First())
                              .ToArray();


               
            var valueToBeExposed = new DashBoardSummary
            {
                Meeting = getMeetingsBySuspectId,
                Goal = getGoalAndTodo,
                Suspects = getSuspectContactsById,
                MeetingDueToday = getMeetingsBySuspectId.Where(x => x.EndDate.Date == DateTime.Now.Date).ToList(),
                TodoDueToday = getDistinctTodo

            };

            return CommonResponse.Send(ResponseCodes.SUCCESS,valueToBeExposed,"This value was successfully processed");


        }

        public async Task<ApiCommonResponse> GetLeadClassificationsData(HttpContext httpContext)
        {


            var suspectsInConcern = await _context.Suspects.AsNoTracking()
                .Where(x=>x.IsDeleted == false)
                .Include(x => x.LeadOrigin)
                .Include(x=>x.CreatedBy)
                .Include(x => x.GroupType)
                //.Include(x => x.Branch)
                //.Include(x => x.Office)
                //.Include(x => x.State)
                //.Include(x => x.Lga)
                //.Include(x => x.Industry)
                //.Include(x => x.LeadType)
                .Include(x => x.SuspectQualifications.Where(x => !x.IsDeleted && x.IsActive))
                .ThenInclude(x => x.ServiceQualifications)
                .ThenInclude(x => x.Service)
                .ToListAsync();

            if(suspectsInConcern == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Suspect was found");
            }

            var leadInstance = new LeadsClassificationData();

            leadInstance.UnqualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count == 0).ToList();
           
            leadInstance.LeadsInQualification = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x => x.AuthorityCompleted == false)).ToList();

            leadInstance.QualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x=>x.AuthorityCompleted == true)).ToList();


            return CommonResponse.Send(ResponseCodes.SUCCESS, leadInstance, "Suspect was successfully retrieved");

        }


        public async Task<ApiCommonResponse> GetLeadClassificationsDataById(HttpContext httpContext,long CreatedById)
        {


            var suspectsInConcern = await _context.Suspects.AsNoTracking()
                .Where(x => x.IsDeleted == false && x.CreatedById == CreatedById)
                .Include(x => x.LeadOrigin)
                .Include(x => x.CreatedBy)
                .Include(x => x.GroupType)
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.State)
                .Include(x => x.Lga)
                .Include(x => x.Industry)
                .Include(x => x.LeadType)
                .Include(x => x.SuspectQualifications.Where(x => !x.IsDeleted && x.IsActive))
                .ThenInclude(x => x.ServiceQualifications)
                .ThenInclude(x => x.Service)
                .ToListAsync();

            if (suspectsInConcern == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Suspect was found");
            }

            var leadInstance = new LeadsClassificationData();

            leadInstance.UnqualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count == 0).ToList();

            leadInstance.LeadsInQualification = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x => x.AuthorityCompleted == false)).ToList();

            leadInstance.QualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x => x.AuthorityCompleted == true)).ToList();


            return CommonResponse.Send(ResponseCodes.SUCCESS, leadInstance, "Suspect was successfully retrieved");

        }

        public async Task<ApiCommonResponse> GetLeadClassificationsDataByDates(HttpContext httpContext, DateTime startDate,DateTime endDate)
        {


            var suspectsInConcern = await _context.Suspects.AsNoTracking()
                .Where(x => x.IsDeleted == false && x.CreatedAt.Date <= startDate.Date && x.CreatedAt.Date >= endDate.Date)
                .Include(x => x.LeadOrigin)
                .Include(x => x.CreatedBy)
                .Include(x => x.GroupType)
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.State)
                .Include(x => x.Lga)
                .Include(x => x.Industry)
                .Include(x => x.LeadType)
                .Include(x => x.SuspectQualifications.Where(x => !x.IsDeleted && x.IsActive))
                .ThenInclude(x => x.ServiceQualifications)
                .ThenInclude(x => x.Service)
                .ToListAsync();

            if (suspectsInConcern == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Suspect was found");
            }

            var leadInstance = new LeadsClassificationData();

            leadInstance.UnqualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count == 0).ToList();

            leadInstance.LeadsInQualification = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x => x.AuthorityCompleted == false)).ToList();

            leadInstance.QualifiedLeads = suspectsInConcern.Where(x => x.SuspectQualifications.Count > 0 && x.SuspectQualifications.Any(x => x.AuthorityCompleted == true)).ToList();


            return CommonResponse.Send(ResponseCodes.SUCCESS, leadInstance, "Suspect was successfully retrieved");

        }

        public async Task<ApiCommonResponse> GetLeadsOpportunityData(HttpContext httpContext)
        {


          

            var getAllLeads = await _context.Leads.AsNoTracking()
                           .Where(x => x.IsDeleted == false)
                           .Include(x => x.CreatedBy)
                           .Include(x => x.GroupType)
                           .Include(x => x.Suspect)
                             .ThenInclude(x => x.SuspectQualifications)
                                   .ThenInclude(x => x.ServiceQualifications)
                                        
                           .ToListAsync();

                         

            if (getAllLeads == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Suspect was found");
            }


            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllLeads, "Suspect was successfully retrieved");

        }

        public async Task<ApiCommonResponse> GetLeadsOpportunityDataByCreatedId(HttpContext httpContext,long createdById)
        {




            var getAllLeads = await _context.Leads
                           .Where(x => x.IsDeleted == false && x.CreatedById == createdById)
                           .Include(x => x.GroupType)
                           .Include(x => x.Suspect)
                             .ThenInclude(x => x.SuspectQualifications)
                                   .ThenInclude(x => x.ServiceQualifications)
                                     .ThenInclude(x => x.Service)
                           .ToListAsync();



            if (getAllLeads == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Suspect was found");
            }

            var leadInstance = new LeadsOpportunityData();

            leadInstance.Initiate = getAllLeads.Where(x => x.LeadCaptureStatus == false).ToList();
            leadInstance.DealCapture = getAllLeads.Where(x => x.LeadCaptureStatus == true).ToList();
            leadInstance.Closed = getAllLeads.Where(x => x.LeadClosureStatus == true).ToList();
            leadInstance.Negotiations = getAllLeads.Where(x => x.LeadCaptureDocumentUrl != null && x.LeadCaptureStatus == true).ToList();
            leadInstance.Dropped = getAllLeads.Where(x => x.IsLeadDropped == true).ToList();
            leadInstance.Conversion = getAllLeads.Where(x => x.LeadConversionStatus == true).ToList();
            leadInstance.ConversionRatio = (long)Math.Round((double)(100 * leadInstance.Conversion.Count) / getAllLeads.Count);

            return CommonResponse.Send(ResponseCodes.SUCCESS, leadInstance, "Suspect was successfully retrieved");

        }


        public async Task<ApiCommonResponse> getContractByLeadId(long Id)
        {


            var contract = await _context.Contracts
                                           .Where(x => x.IsDeleted == false && x.CustomerDivisionId == Id)
                                           .Include(x=>x.CreatedBy)
                                            .Include(x => x.ContractServices)
                                            .ToListAsync();



            if (contract == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No Contract was found");
            }

            
           

            return CommonResponse.Send(ResponseCodes.SUCCESS, contract, "Contract was successfully retrieved");

        }


    }
}
