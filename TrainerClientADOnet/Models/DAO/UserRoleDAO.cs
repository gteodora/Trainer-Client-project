using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TrainerClientADOnet.Models.DTO;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Net;

namespace TrainerClientADOnet.Models.DAO
{
    public class UserRoleDAO :Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context = new ApplicationDbContext();

        public List<UserRoleDTO> GetAll()
        {
            var usersWithRoles = (from user in context.Users
                                  from userRole in user.Roles
                                  join role in context.Roles on userRole.RoleId equals
                                  role.Id
                                  select new UserRoleDTO()
                                  {
                                      Id=user.Id,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      Name = role.Name
                                  }).ToList();
            return usersWithRoles;
        }

        public async Task<ActionResult> Create(string roleId, string email, string username, string password, string roleName, bool emailConfirmed, bool phoneNumberConfirmed, bool twoFactorEnabled, bool lockoutEnabled, int accessFailedCount)
        {
            var db = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new ApplicationUserManager(userStore);

            var user = new ApplicationUser { UserName = username, Email = email, EmailConfirmed=emailConfirmed, PhoneNumberConfirmed=phoneNumberConfirmed, TwoFactorEnabled=twoFactorEnabled, LockoutEnabled=lockoutEnabled, AccessFailedCount=accessFailedCount  };
            if (user != null) 
                { 
                        var result = await userManager.CreateAsync(user, password);
                var id = user.Id;
                    if (result.Succeeded)
                    {
                        result = userManager.AddToRole(user.Id, "Trener");
                    
                    //todo: posalji e-mail ulogovanom korisniku:
                    SendEmailWithCredentials(email, password);
                        return RedirectToAction("Index", "UserRole");
                    }
                    else AddErrors(result);
                }
            return View("Error");
        }

        private void SendEmailWithCredentials(string email, string password)
        {
            /* try
             {

                       *MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("teodoragalic96@gmail.com");
                            mail.To.Add("comtrade@mailinator.com");
                            mail.Subject = "Test Mail- TESTIRANJE SLANJA KREDENCIJALA";
                            mail.Body = "This is for testing SMTP mail from GMAIL" +
                                email + "   " + password;

                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("Teodora", "teodorag96");
                            SmtpServer.EnableSsl = true;

                            SmtpServer.Send(mail);
                            */
            /*         
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("teodoragalic96@gmail.com", "password"),
                EnableSsl = true,
            };

            smtpClient.Send("teodoragalic96@gmail.com", "comtrade@mailinator.com", "subject", "body");
             */

            MailAddress from = new MailAddress("galateica@hotmail.com");
                MailAddress to = new MailAddress("comtrade@mailinator.com");
                MailMessage message = new MailMessage(from, to);
                // message.Subject = "Using the SmtpClient class.";
                message.Subject = "Using the SmtpClient class.";
                message.Body = @"Using this feature, you can send an email message from an application very easily.";
                // Add a carbon copy recipient.
                MailAddress copy = new MailAddress("Notifications@contoso.com");
                message.CC.Add(copy);
                SmtpClient client = new SmtpClient("smtp.live.com");
            // Include credentials if the server requires them.
            client.EnableSsl = true;
                client.Credentials = (ICredentialsByHost)CredentialCache.DefaultNetworkCredentials;
                Console.WriteLine("Sending an email message to {0} using the SMTP host {1}.",
                     to.Address, client.Host);
                try
                {
                    client.Send(message);
                }
                catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        client.Send(message);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deliver message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in RetryIfBusy(): {0}",
                        ex.ToString());
            }
        }

        internal void Edit(string roleId, string email)
        {
            throw new NotImplementedException();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public void Delete(string id)
        {
            var user = context.Users.Where(d => d.Id.Equals(id)).FirstOrDefault();
            context.Users.Remove(user);
            context.SaveChanges();
        }


        public object GetAll(string idRole)
        {
                        var usersWithRoles = (from user in context.Users
                                              from userRole in user.Roles
                                              join role in context.Roles
                                              on userRole.RoleId equals role.Id
                                            where userRole.RoleId.Equals(idRole)
                                              select new UserRoleDTO()
                                  {
                                      Id=user.Id,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      Name = role.Name,
                                      RoleId=role.Id
                                  }).ToList();
            return usersWithRoles;
        }

    }
}