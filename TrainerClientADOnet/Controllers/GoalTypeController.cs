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
    public class GoalTypeController : Controller
    {
        GoalTypeDAO goalTypesDAO = new GoalTypeDAO();

        // GET: GoalType
        public ActionResult Index()
        {
            var goalTypes = goalTypesDAO.GetAll();
            return View(goalTypes);
        }


        // GET: GoalType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoalType/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                int id = goalTypesDAO.Create(name);
                return RedirectToAction("Index");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GoalType/Edit/5
        public ActionResult Edit(int id)
        {
            var model = goalTypesDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: GoalType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                int number = goalTypesDAO.Edit(id, name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: GoalType/Delete/5
        public ActionResult Delete(int id)
        {
            var model = goalTypesDAO.getById(id);
            if (model!=null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: GoalType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                goalTypesDAO.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
