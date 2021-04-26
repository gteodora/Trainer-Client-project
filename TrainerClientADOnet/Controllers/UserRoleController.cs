using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Controllers
{
    [Authorize(Roles = Utility.ADMIN_ROLE)]
    public class UserRoleController : Controller
    {
        UserRoleDAO userRoleDAO = new UserRoleDAO();

        //ok
        // GET: UserRole
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userRoles = userRoleDAO.GetAll();
            return View(userRoles);
        }

        //ok
        // GET: UserRole/Details/5
        public ActionResult Details(string id)
        {
            var userRoles = userRoleDAO.GetAll();
            var userRole = userRoles.FirstOrDefault(c => c.Id.Equals(id));
            if (userRole != null)
            {
                return View(userRole);
            }
            else
            {
                return View("Error");
            }
        }

        //ok
        // GET: UserRole/Create
        public ActionResult Create()
        {
            UserRoleDTO userRoleDTO = new UserRoleDTO();
            userRoleDTO.Email = "@gmail.com";
            userRoleDTO.PasswordHash = "Teodora!7";
            userRoleDTO.EmailConfirmed = false;
            userRoleDTO.PhoneNumberConfirmed = false;
            userRoleDTO.TwoFactorEnabled = false;
            userRoleDTO.LockoutEnabled = false;
            userRoleDTO.AccessFailedCount = 0;
            userRoleDTO.UserName = "@gmail.com";
            //password, stamp
            return View(userRoleDTO);
        }

       //ok
       // POST: UserRole/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                //trebam dodati i ostale podatke ako unosi klijenta u moju bazu!!!!  ->admin unosi ADMINe i TRENERe
                string roleId = collection["RoleId"];
                string email = collection["Email"];
                string password = collection["Password"];

                string userName = email;  
                string roleName = collection["Name"]; //ne trebam
                bool emailConfirmed = false;
                bool phoneNumberConfirmed = false;
                bool twoFactorEnabled = false;
                bool lockoutEnabled = false;
                int accesFailedCount = 0;
                //ovo gore dobro pokupi i sada trebam da ovog klijenta posaljem da kreira Identity i onda taj vraceni ID->upisem tog korisnika!!!
                await userRoleDAO.Create( roleId, email, userName,  password, roleName, emailConfirmed, phoneNumberConfirmed, twoFactorEnabled, lockoutEnabled, accesFailedCount);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        //ok
        // GET: UserRole/Edit/5
        public ActionResult Edit(string id)
        {
            var userRoles = userRoleDAO.GetAll();
            var userRole = userRoles.FirstOrDefault(c => c.Id.Equals(id));
            if (userRole != null)
            {
                return View(userRole);
            }
            else
            {
                return View("Error");
            }
        }

        //todo userrole edit
        // POST: UserRole/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                string roleId = collection["RoleId"];
                string email = collection["Email"];
                userRoleDAO.Edit(roleId, email);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //ok
        // GET: UserRole/Delete/5
        public ActionResult Delete(string id)
        {
            var userRoles = userRoleDAO.GetAll();
            var userRole = userRoles.FirstOrDefault(c => c.Id.Equals(id));
            if (userRole != null)
            {
                return View(userRole);
            }
            else
            {
                return View("Error");
            }
        }

        //ok
        // POST: UserRole/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                userRoleDAO.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        //ok
        public ActionResult UsersFromSelectedRole(string idRole)
        {
            var userRoles = userRoleDAO.GetAll(idRole);
            return View(userRoles);
        }


    }
}
