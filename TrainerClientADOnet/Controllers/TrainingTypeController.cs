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
    public class TrainingTypeController : Controller
    {
        TrainingTypeDAO trainingTypeDAO = new TrainingTypeDAO();

        // GET: TrainingType
        public ActionResult Index()
        {
            var trainingTypes = trainingTypeDAO.GetAll();
            return View(trainingTypes);
        }

        // GET: TrainingType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingType/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                int id = trainingTypeDAO.Create(name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingType/Edit/5
        public ActionResult Edit(int id)
        {
            var model = trainingTypeDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: TrainingType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                int number=trainingTypeDAO.Edit(id, name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: TrainingType/Delete/5
        public ActionResult Delete(int id)
        {
            var model = trainingTypeDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: TrainingType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                trainingTypeDAO.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }



    }
}
