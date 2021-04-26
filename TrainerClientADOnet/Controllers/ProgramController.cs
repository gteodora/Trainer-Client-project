using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;
using PagedList;

namespace TrainerClientADOnet.Controllers
{
    public class ProgramController : Controller
    {
        ProgramDAO programDAO = new ProgramDAO();
        GoalTypeDAO goalTypeDAO = new GoalTypeDAO();
        ProgramGoalTypeDAO programGoalTypeDAO = new ProgramGoalTypeDAO();

        //OK
        // GET: Program/Details/5
        public ActionResult Details(int id)
        {
            var program = programGoalTypeDAO.GetById(id);
            if (program != null)
            {
                program.ProgramType = Utility.setProgramType(program.IsMeal);
                return View(program);
            }
            else
            {
                return View("Error");
            }
        }

       
        //OK
        // GET: Program/Edit/5
        public ActionResult Edit(int id, int? clientId)
        {
            var program = programGoalTypeDAO.GetById(id);
            if (program != null)
            {
                return View(program);
            }
            else
            {
                return View("Error");
            }
        }

        //todo:edit program
        // POST: Program/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, int? clientId, FormCollection collection)
        {
            var program = programGoalTypeDAO.GetById(id);
            try
            {
                //string name = collection["Name"];
               // int number = programDAO.Edit(id, name);

                //return RedirectToAction("Index");
                return RedirectToAction("AllProgramsOfClient", new { clientId = program.ClientId });
            }
            catch
            {
                return View("Error");
            }
        }

        //OK
        // GET: Program/Delete/5
        public ActionResult Delete(int id, int? clientId)
        {
            var program = programGoalTypeDAO.GetById(id);
            if (program != null)
            {
                program.ProgramType = Utility.setProgramType(program.IsMeal);
                return View(program);
            }
            else
            {
                return View("Error");
            }
        }

        // POST: Program/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int? clientId, FormCollection collection)
        {
            var program = programGoalTypeDAO.GetById(id);
            try
            {
                programDAO.Delete(id);

                return RedirectToAction("AllProgramsOfClient", new { clientId = program.ClientId });
            }
            catch
            {
                return View("Error");
            }
        }

        //OK
        // GET: Program/AllProgramsOfClient/5
        public ActionResult AllProgramsOfClient(string clientId, int? page, int? goalTypeId)   //za slucaj da ide na 2.stranicu da i dalje zna da je uljucena pretraga iz combo boxa
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ProgramGoalTypeDTO> pro = null;
            List<ProgramGoalTypeDTO> programs = programGoalTypeDAO.GetAllOfClient(clientId);
           // List<ProgramGoalTypeDTO> selectedPrograms=programs.Where(p=>p.Is)
            pro = programs.ToPagedList(pageIndex, pageSize);
            return View(pro);
        }

        //OK
        // GET: Client/CreateProgramForClient/5
        public ActionResult CreateProgramForClient(string clientId)
        {
            ProgramDTO program = new ProgramDTO();
            program.ClientId = clientId;
            program.StartTime = DateTime.Today;  
            program.EndTime = DateTime.Today;
            ViewBag.goalsItems = goalTypeDAO.GetAll();
            return View(program);
        }

        //ok
        // POST: Client/CreateProgramForClient/5
        [HttpPost]
        public ActionResult CreateProgramForClient(string clientId, FormCollection collection)  
        {
            try
            {
                
                int goalTypeId = int.Parse(collection["GoalTypeId"]);    
                DateTime startTime = DateTime.Parse(collection["StartTime.Date"]);
                DateTime endTime = DateTime.Parse(collection["EndTime.Date"]);
                string description = collection["Description"];
                bool isMeal= bool.Parse(collection["IsMeal"]);

                int id = programDAO.Create(clientId, goalTypeId, startTime, endTime, description, isMeal); 

               // return RedirectToAction("AllProgramsOfClient", new { clientId=client_id });
                return RedirectToAction("Details", new { id = id });   
            }
            catch
            {
                return View("Error");
            }
        }

    }
}
