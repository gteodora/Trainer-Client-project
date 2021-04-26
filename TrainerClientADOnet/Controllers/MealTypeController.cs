using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Controllers
{
    [Authorize(Roles = Utility.TRAINER_ROLE)]
    public class MealTypeController : Controller
    {
        MealTypeDAO mealTypeDAO = new MealTypeDAO();
        // GET: MealType
        public ActionResult Index()
        {
            var clients = mealTypeDAO.GetAll();
            return View(clients);
        }

        // GET: MealType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MealType/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
               int id= mealTypeDAO.Create(name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: MealType/Edit/5
        public ActionResult Edit(int id)
        {
            var model = mealTypeDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: MealType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                int number = mealTypeDAO.Edit(id, name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: MealType/Delete/5
        public ActionResult Delete(int id)
        {
            var model = mealTypeDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: MealType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                mealTypeDAO.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
