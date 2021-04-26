using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TrainerClientADOnet.Models;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;
using PagedList;

namespace TrainerClientADOnet.Controllers
{
    [Authorize(Roles = Utility.TRAINER_ROLE)]
    public class ClientController : Controller
    {
        ClientDAO clientDAO = new ClientDAO();
        ProgramDAO programDAO = new ProgramDAO();
        UserRoleDAO userRoleDAO = new UserRoleDAO();

        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)   // ReplaceWhitespace(,sWhitespace)
        {
            return sWhitespace.Replace(input, replacement);
        }

        //OK
        // GET: Client
        public ActionResult Index( int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ClientDTO> pro = null;
            List<ClientDTO> clients  = clientDAO.GetAll();
            pro = clients.ToPagedList(pageIndex, pageSize);
            return View(pro);
        }

        //OK
        // GET: Client/Details/5
        public ActionResult Details(string id)
        {
            var clients = clientDAO.GetAll();
            char[] charsToTrim = { ' ' };
            var client = clients.FirstOrDefault(c => c.Id.TrimEnd(charsToTrim).Equals(id));
            if (client != null)
            {
                return View(client);
            }
            else
            {
                return View("Error");
            }
        }

        //OK
        // GET: Client/Create
        public ActionResult Create()
        {
            ClientUserRoleDTO client = new ClientUserRoleDTO();
            client.RegistrationDate = DateTime.Now; 

            client.Email = "@mailinator.com";
            client.PasswordHash = "Teodora!7";
            client.EmailConfirmed = false;
            client.PhoneNumberConfirmed = false;
            client.TwoFactorEnabled = false;
            client.LockoutEnabled = false;
            client.AccessFailedCount = 0;
            client.UserName = "@mailinator.com";
            return View(client);
        }

        //ok
        // POST: Client/Create
        [HttpPost]
        public async Task<ActionResult> Create(ClientUserRoleDTO client)
        {
            var errors = ModelState
.Where(x => x.Value.Errors.Count > 0)
.Select(x => new { x.Key, x.Value.Errors })
.ToArray();
            if (ModelState.IsValid)
            {
                try
                {
                    string roleId = GetRoleIdOfClient();
                    string email = client.Email;
                    string password = client.Password;
                    string userName = email;
                   // string roleName = collection["Name";
                    bool emailConfirmed = false;
                    bool phoneNumberConfirmed = false;
                    bool twoFactorEnabled = false;
                    bool lockoutEnabled = false;
                    int accesFailedCount = 0;
                    //ovo gore dobro pokupi i sada trebam da ovog klijenta posaljem da kreira Identity i onda taj vraceni ID->upisem tog korisnika!!!
                    //////////////////////
                    string firstName = client.FirstName;
                    string lastName = client.LastName;
                    DateTime registrationDate = client.RegistrationDate;
                    DateTime dateOfBirth = client.DateOfBirth;
                    string description = client.Description;
                  
                    //////////////////////////
                   await clientDAO.Create(firstName, lastName, dateOfBirth, registrationDate, roleId, email, userName, password, emailConfirmed, phoneNumberConfirmed, twoFactorEnabled, lockoutEnabled, accesFailedCount, description);

                    

                    return RedirectToAction("Index");
                    //return RedirectToAction("Details", new { id = id });


                }
                catch(Exception e)
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Create");
            }
        }

        private string GetRoleIdOfClient()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var clientRole = context.Roles.Where(d => d.Name.Equals(Utility.CLIENT_ROLE)).FirstOrDefault();
            return clientRole.Id;
        }

        //OK
        // GET: Client/Edit/5
        public ActionResult Edit(string id)
        {
            var clients = clientDAO.GetAll();
            clients.ForEach(c => Regex.Replace(c.Id, @"s", ""));
                   char[] charsToTrim = {' '};
            var client = clients.FirstOrDefault(c => c.Id.TrimEnd(charsToTrim).Equals(id));
            if (client != null)
            {
                return View(client);
            }
            else
            {
                return View("Error");
            }
        }

        //ok
        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                string firstName = collection["FirstName"];
                string lastName = collection["LastName"];
                DateTime birthDate = DateTime.Parse(collection["DateOfBirth.Date"]);
                DateTime registrationDate = DateTime.Parse(collection["RegistrationDate.Date"]);
                string description = collection["Description"];
                int number = clientDAO.Edit(id, firstName, lastName, birthDate, registrationDate, description);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        //OK
        // GET: Client/Delete/5
        public ActionResult Delete(string id)
        {
            var clients = clientDAO.GetAll();

            char[] charsToTrim = { ' ' };
            var client = clients.FirstOrDefault(c => c.Id.TrimEnd(charsToTrim).Equals(id));

            if (client != null)
            {
                return View(client);
            }
            else
            {
                return View("Error");
            }
        }

        //ok
        // POST: Client/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                userRoleDAO.Delete(id); //brisanje iz Identity-ja
                clientDAO.Delete(id);  //brisanje iz baze

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

    }
}
