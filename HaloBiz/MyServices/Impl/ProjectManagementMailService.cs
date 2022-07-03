using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.EnumResponse;
using HalobizMigrations.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MimeKit;
using MimeKit.Text;

namespace HaloBiz.MyServices.Impl
{
    public class ProjectManagementMailService : IProjectManagementMailService
    {

        private readonly  IWebHostEnvironment _hostEnvironment;
        private readonly HalobizContext _context;
        private readonly MailSettings _appSettings;
        public ProjectManagementMailService(IWebHostEnvironment hostEnvironment,HalobizContext context,IOptions<MailSettings> appSettings)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _appSettings = appSettings.Value;
        }


        public async Task<ApiCommonResponse> BuildMail(ProjectManagementOption projectManagementOption,long requestId)
        {

            var getProjectManagementTemplate = Path.Combine(_hostEnvironment.WebRootPath,
                "ProjectManagementTemplate/ProjectManagementNotificationTemplate.html");

            var response = new ApiCommonResponse();
            switch (projectManagementOption)
            {
                case ProjectManagementOption.WorkspaceCreation:
                    response = await SendWorkspaceNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.ProjectCreation:
                    response = await SendProjectNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.TaskCreation:
                    response = await SendTaskNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.DeliverableMovement:
                    response = await SendDeliverableMovementNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.ProjectComment:
                    response = await SendProjectCommentNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.DeliverableApproval:
                    response = await SendDeliverableApprovalNotification(getProjectManagementTemplate, requestId);
                    break;
                case ProjectManagementOption.TaskComment:
                    response = await SendTaskCommentNotification(getProjectManagementTemplate, requestId);
                    break;
             }
            return CommonResponse.Send(ResponseCodes.SUCCESS, response);
        }
        
        public async Task<ApiCommonResponse> SendWorkspaceNotification(string path, long requestId)
        {
            var getCurrentWorkspace = await _context.Workspaces.AsNoTracking()
                .Include(x => x.ProjectCreators)
                .ThenInclude(x=>x.ProjectCreatorProfile)
                .Include(x=>x.CreatedBy)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Workspace Notification.";
                var createdAt = getCurrentWorkspace.CreatedAt.ToString("D");
                var projectCreators = new List<string>();
                var privacySetting = getCurrentWorkspace.IsPublic == true
                    ? "Workspace is public"
                    : "Workspace is private";
                var projectCreatorTitle = getCurrentWorkspace.ProjectCreators.Any() ? 
                    "<div><strong>Assigned Project Creators: <strong></div><br>"
                    : "";
                var firstText = "<div>A new workspace has just been created, Please see Workspace details below.</div><br>";
                var workspaceDetails = $"<div><div><strong>Workspace Details<strong></div><br>" +
                                       $"<div>Workspace caption: {getCurrentWorkspace.Caption}</div><br>" +
                                       $"<div>Workspace description : {getCurrentWorkspace.Description}</div><br>" +
                                       $"<div>Workspace private setting : {privacySetting}<div>" +
                                       $"</div><br>";
                var finalMessage = "<div>You are getting this message because you are either " +
                                   "the Workspace creator or an assigned project creator on this workspace</div><br>" +
                                   "<div>As an assigned project creator you can go ahead and create projects within this workspace.</div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentWorkspace.CreatedBy.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                if (getCurrentWorkspace.ProjectCreators.Any())
                {
                    foreach (var destination in getCurrentWorkspace.ProjectCreators)
                    {
                        projectCreators.Add($"<div>{destination.ProjectCreatorProfile.FirstName} " +
                                            $"{destination.ProjectCreatorProfile.LastName}({destination.ProjectCreatorProfile.Email})</div><br>");
                        email.Cc.Add(MailboxAddress.Parse(destination.ProjectCreatorProfile.Email));
                    }
                }
                var body  = firstText + workspaceDetails +  projectCreatorTitle + String.Join("",projectCreators) + "<br>" + finalMessage;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        }
        
         public async Task<ApiCommonResponse> SendProjectNotification(string path, long requestId)
         {
            var getCurrentProject = await _context.Projects.AsNoTracking()
                .Include(x=>x.Workspace)
                .Include(x => x.Watchers)
                .ThenInclude(x=>x.ProjectWatcher)
                .Include(x=>x.CreatedBy)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Project Notification.";
                var createdAt = getCurrentProject.CreatedAt.ToString("D");
                List<string> watchers = new List<string>();
                watchers.Clear();
                var projectCreatorTitle = getCurrentProject.Watchers.Any() ? "<div><strong>Assigned Project Creators: <strong></div><br>" : "";
                var firstText = "<div>A new project has just been created, Please see Project details below.</div><br>";
                var projectDetails = $"<div><div><strong>Project Details<strong></div><br>" +
                                       $"<div>Project caption: {getCurrentProject.Caption}</div><br>" +
                                       $"<div>Project description : {getCurrentProject.Description}</div><br>" +
                                       $"<div>Project workspace : {getCurrentProject.Workspace.Caption}<div>" +
                                       $"</div><br>";
                var finalMessage = "<div>You are getting this message because you are either " +
                                   "the Workspace creator or an assigned project creator on this workspace</div><br>" +
                                   "<div>As an assigned project creator you can go ahead and create projects within this workspace.</div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentProject.CreatedBy.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                if (getCurrentProject.Watchers.Any())
                {
                    foreach (var destination in getCurrentProject.Watchers)
                    {
                        watchers.Add($"<div>{destination.ProjectWatcher.FirstName} " +
                                            $"{destination.ProjectWatcher.LastName}({destination.ProjectWatcher.Email})</div><br>");
                        email.Cc.Add(MailboxAddress.Parse(destination.ProjectWatcher.Email));
                    }
                }
                var body  = firstText + projectDetails +  projectCreatorTitle + String.Join("",watchers) + "<br>" + finalMessage;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        }
        
         public async Task<ApiCommonResponse> SendTaskNotification(string path, long requestId)
         {
            var getCurrentTask = await _context.Tasks.AsNoTracking()
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Project)
                .Include(x => x.TaskAssignees)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Task Notification.";
                var createdAt = getCurrentTask.CreatedAt.ToString("D");
                var assignees = new List<string>();
                var assigneesTitle = getCurrentTask.TaskAssignees.Any() ?
                    "<div><strong>Assignees: <strong></div><br>" 
                    :"";
                var firstText = "<div>A new Task has just been created, Please see Task details below.</div><br>";
                var taskDetails = $"<div><div><strong>Task Details<strong></div><br>" +
                                       $"<div>Task caption: {getCurrentTask.Caption}</div><br>" +
                                       $"<div>Task description : {getCurrentTask.Description}</div><br>" +
                                       $"<div>Project Start date  : {getCurrentTask.TaskStartDate:d}</div><br>" +
                                       $"<div>Project End date : {getCurrentTask.TaskEndDate:d}</div><br>" +
                                       $"<div>Project Duration : {Math.Round((getCurrentTask.TaskEndDate.Date - getCurrentTask.TaskStartDate.Date).TotalHours)}Hr(s)</div><br>" +
                                       $"<div>Parent Project : {getCurrentTask.Project.Caption}<div>" +
                                       $"</div><br>";
                var finalMessage = "<div>You are getting this message because you are either an " +
                                   "assignee of this tasks or you are the task creator<div><br> " +
                                   "<div>As one of the assignees of this task, you are expected to take " +
                                   "ownership of this task and commence work on it</div><br>" +
                                   "<div>Kindly logon to the Halobiz Suit of Solutions, navigate to the project" +
                                   " management Module and go to Task management sub module to commence work.</div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentTask.CreatedBy.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                if (getCurrentTask.TaskAssignees.Any())
                {
                    foreach (var destination in getCurrentTask.TaskAssignees)
                    {
                        var currentUser =
                            await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == destination.TaskAssigneeId);
                        assignees.Add($"<div>{currentUser.FirstName} " +
                                            $"{currentUser.LastName}({currentUser.Email})</div><br>");
                        email.Cc.Add(MailboxAddress.Parse(currentUser.Email));
                    }
                }
                var body  = firstText + taskDetails + assigneesTitle  + String.Join("",assignees) + "<br>" + finalMessage;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        }
         
          public async Task<ApiCommonResponse> SendDeliverableMovementNotification(string path, long requestId)
        {
            var getCurrentDeliverable = await _context.Deliverables.AsNoTracking()
                .Include(x=>x.Workspace)
                .Include(x=>x.Task)
                .ThenInclude(x=>x.TaskOwnership)
                .ThenInclude(x=>x.TaskOwner)
                .Include(x=>x.Status)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();

            var getDeliverableAssignee = await _context.AssignTasks.AsNoTracking()
                .Include(x => x.DeliverableAssignee)
                .FirstOrDefaultAsync(x => x.DeliverableId == getCurrentDeliverable.Id);
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Deliverable Status Notification.";
                var createdAt = getCurrentDeliverable.CreatedAt.Date.ToString("D");
                var firstText = $"<div>Please be informed that the deliverable <strong>{getCurrentDeliverable.Caption}</strong> , " +
                                "has just been moved along its inherent " +
                                "status flow, kindly see movement details below</div><br>";
                var deliverableDetails = $"<div><div><strong>Status flow movement Details: <strong></div><br>" +
                                         $"<div>New Status: {getCurrentDeliverable.Status.Caption}</div><br>" +
                                         $"<div>Datetime of movement : {DateTime.Now:D}</div></div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentDeliverable.Task.TaskOwnership.TaskOwner.Email));
                email.To.Add(MailboxAddress.Parse(getDeliverableAssignee.DeliverableAssignee.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                var body  = firstText + deliverableDetails;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        }
          
           public async Task<ApiCommonResponse> SendDeliverableApprovalNotification(string path, long requestId)
          {
            var getCurrentDeliverable = await _context.Deliverables.AsNoTracking()
                .Include(x=>x.Task)
                .ThenInclude(x=>x.TaskOwnership)
                .ThenInclude(x=>x.TaskOwner)
                .Include(x=>x.Status)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Deliverable Approval Notification.";
                var createdAt = getCurrentDeliverable.CreatedAt.ToString("D");
                var firstText = $"<div>Please be informed that the deliverable <strong> {getCurrentDeliverable.Caption} </strong>  has just been " +
                                "submitted for your approval.</div><br>";
                var deliverableDetails = $"<div><div>Kindly logon to the Halobiz Suit of Solutions, navigate to the Project<br>" +
                                         $"management Module and go to Deliverable approval module</div><br>" +
                                         $"<div>to view, approve or decline completion of the said deliverable.</div></div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentDeliverable.Task.TaskOwnership.TaskOwner.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                var body  = firstText + deliverableDetails + "<br>";
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        } 
           
        public async Task<ApiCommonResponse> SendProjectCommentNotification(string path, long requestId)
          {
            var getCurrentProjectComment = await _context.ProjectComments.AsNoTracking()
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Project)
                .ThenInclude(x=>x.CreatedBy)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Project Comment Notification.";
                var createdAt = getCurrentProjectComment.CreatedAt.ToString("D");
                var firstText = $"<div>A new message has been left by <strong> {getCurrentProjectComment.CreatedBy.FirstName} {getCurrentProjectComment.CreatedBy.LastName} </strong> on project " +
                                $" <strong>{getCurrentProjectComment.Project.Caption}. </strong></div><br>";
                var projectCommentDetails = $"<div><div>Kindly logon to the Halobiz Suit of Solutions, navigate to the Project" +
                                         " management Module and go to Project Management structure to view comments." +
                                         "</div></div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentProjectComment.CreatedBy.Email));
                email.To.Add(MailboxAddress.Parse(getCurrentProjectComment.Project.CreatedBy.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                var body  = firstText + projectCommentDetails;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        } 
        
         public async Task<ApiCommonResponse> SendTaskCommentNotification(string path, long requestId)
          {
            var getCurrentTaskComment = await _context.TaskComments.AsNoTracking()
                .Include(x=>x.CreatedBy)
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();

            var getTask = await _context.Tasks.AsNoTracking()
                .Include(x => x.TaskOwnership)
                .ThenInclude(x => x.TaskOwner)
                .Include(x => x.CreatedBy)
                .Include(x => x.TaskAssignees)
                .Where(x => x.Id == getCurrentTaskComment.ProjectId)
                .FirstOrDefaultAsync();
            try
            {
                var email = new MimeMessage(); 
                email.Subject = "New Task Comment Notification.";
                var createdAt = getCurrentTaskComment.CreatedAt.ToString("D");
                var firstText = $"<div>A new message has been left by <strong>{getCurrentTaskComment.CreatedBy.FirstName} {getCurrentTaskComment.CreatedBy.LastName}</strong> on Task " +
                                $"<strong>{getTask.Caption}</strong></div><br>";
                var taskCommentDetails = $"<div>Kindly logon to the Halobiz Suit of Solutions, navigate to the Project " +
                                            "management Module and go to Task Management module to view comments.</div>";
                
                email.To.Add(MailboxAddress.Parse(getCurrentTaskComment.CreatedBy.Email));
                email.To.Add(MailboxAddress.Parse(getTask.TaskOwnership.TaskOwner.Email));
                email.From.Add(MailboxAddress.Parse($"noreply@halogen-group.com"));
                if (getTask.TaskAssignees.Any())
                {
                    foreach (var destination in getTask.TaskAssignees)
                    {
                        var currentUser =
                            await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == destination.TaskAssigneeId);
                        email.Cc.Add(MailboxAddress.Parse(currentUser.Email));
                    }
                }
                var body  = firstText + taskCommentDetails;
                string mailText = File.ReadAllText(path)
                    .Replace("[subject]", email.Subject)
                    .Replace("[date]", createdAt)
                    .Replace("[message]", body);
                email.Body = new TextPart(TextFormat.Html) { Text = mailText };
                
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The workspace email was successfully sent");

            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, e, e.Message);
            }
        } 
         
       
        
    }
}