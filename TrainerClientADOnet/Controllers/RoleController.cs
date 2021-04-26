using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TrainerClientADOnet.Models;
using TrainerClientADOnet.Models.DAO;

namespace TrainerClientADOnet.Controllers
{
    public class RoleController : Controller
    {
        RoleDAO roleDAO = new RoleDAO();
       
        //OK
        // GET: Role
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var roles = roleDAO.GetAll();
            return View(roles);
        }

        //OK
        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        //ok
        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string Name = collection["Name"];
                roleDAO.Create(Name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // todo: edit role

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //OK
        // GET: Role/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        
        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                roleDAO.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        //ok
        // GET: Role/Users/5
        public ActionResult Users(string id)
        {
            return RedirectToAction("UsersFromSelectedRole", "UserRole", new { idRole = id });
        }
    }
}
