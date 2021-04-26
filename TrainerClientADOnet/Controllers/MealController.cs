using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Models.DTO;
using TrainerClientADOnet.Utilities;

namespace TrainerClientADOnet.Controllers
{
    public class MealController : Controller
    {
        MealDAO mealDAO = new MealDAO();
        static MealDTO[][] mealDescriptions = new MealDTO[3][];

        // GET: Training/Details/5
        [Authorize(Roles = Utility.CLIENT_ROLE)]
        public ActionResult Details(int id)
        {
            var meal = mealDAO.GetById(id);
            if (meal != null)
            {
                return View(meal);
            }
            else
            {
                return View("Error");
            }
        }

        //ok
        // GET: Meal/Create
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Create(int id, int mealTypeId, DateTime date, int programId, DateTime monday)
        {
            var meal = GetById(id, mealTypeId);  
            if (meal != null)
            {
                ViewBag.SelectedMonday = monday;
                return View(meal);
            }
            else
            {
                return View("Error");
            }
        }
        //OK-treba da ga potrazi u onom nizu i kada ga nadje, treba njega vratiti
        private MealDTO GetById(int id, int mealTypeId)
        {
            for (int j = 0; j < 7; j++)
            {
                if (((MealDTO)(mealDescriptions[mealTypeId-1][j])).Id == id) { return mealDescriptions[mealTypeId-1][j]; }
            }
            return null;
        }

        //OK
        // POST: Meal/Create
        [HttpPost]
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Create(DateTime date, int programId, DateTime monday, FormCollection collection)
        {
            try
            {
                int mealTypeId = int.Parse(collection["MealTypeId"]);
                string description = collection["Description"];
                int recurring = int.Parse(collection["Recurring"]);

                DateTime startTime = date;
                //ovo sedmicno gleda
                for (int j = 1; j <= recurring; j++)
                {
                    //U istom danu da nadodaje meal
                    int newId = mealDAO.Create(programId, mealTypeId, startTime, description);

                    startTime = startTime.AddDays(7);
                }

                DateTime sunday = monday.AddDays(6);
                return RedirectToAction("MealPlanForProgram", new { programId = programId, monday = monday, sunday = sunday });
            }
            catch
            {
                return View();
            }
        }

        // GET: Meal/Edit/5
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Edit(int id, DateTime monday)
        {
          
                var meal = mealDAO.GetById(id);
                if (meal != null)
                {
                meal.StartTime = meal.StartTime.Date;
                ViewBag.SelectedMonday = monday;
                return View(meal);
                }
                else
                {
                    return View("Error");
                }
        }

        //OK
        // POST: Meal/Edit/5
        [HttpPost]
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Edit(int id,  DateTime monday, int mealTypeId, FormCollection collection)
        {
            try
            {
                int programID= int.Parse(collection["ProgramId"]);
                string description = collection["Description"];
                int number = mealDAO.Edit(id, description);
                return RedirectToAction("MealPlanForProgram", new { programId = programID, monday = monday });
            }
            catch
            {
                return View("Error");
            }
        }


        //NADJEM sve obroke koji imaju ovaj program id - iz BAZE  I  pripadaju opsegu datuma u ovoj sedmici
        // GET: Meal/MealPlanForProgram/5
        public ActionResult MealPlanForProgram(int programId, DateTime monday)
        {
            var sunday = monday.AddDays(6);
            var model = mealDAO.getByProgram(programId, monday, sunday);
            model = model.Where(t => t.StartTime.Date >= monday.Date && t.StartTime.Date <= sunday).ToList();
            var orderedMeals = PutMealsInOrder(model, monday);
            ViewBag.Meals = orderedMeals;
            ViewBag.SelectedMonday = monday;
            return View();
        }
        /*reda obroke tako da meals[1] je dorucak; meals[2] je rucak...
         a dane postavlja tako da krece od ponedeljka-nedelje. tj/ [0]-[6]*/
        private MealDTO[][] PutMealsInOrder(List<MealDTO> model, DateTime monday)
        {
            MealDTO emptyMeal = new MealDTO();
            
            for (int i = 0; i < 3; i++)
            {
                mealDescriptions[i] = new MealDTO[7];
                for (int j = 0; j < 7; j++)
                {
                    emptyMeal = getMealById(i);
                    emptyMeal.StartTime = getDayOfTheWeekByIdAndWeek(j, monday);  //getDayOfTheWeekById(j);
                    //popunjavanje podacima iz baze
                    mealDescriptions[i][j] = (MealDTO)model.Where(m => i + 1 == m.MealTypeId)
                    .FirstOrDefault(m => m.StartTime.DayOfWeek == getDayOfTheWeekForId(j));
                    // inicijalizacija niza            
                    if (mealDescriptions[i][j] == null)
                    {
                        mealDescriptions[i][j] = emptyMeal;
                    }
                }
            }
            return mealDescriptions;
        }

        private DateTime getDayOfTheWeekByIdAndWeek(int j, DateTime monday)
        {

            if (j == 0) //pon
                return monday;
            else if (j == 1)
                return monday.AddDays(1);
            else if (j == 2)
                return monday.AddDays(2);
            else if (j == 3)
                return monday.AddDays(3);
            else if (j == 4)
                return monday.AddDays(4);
            else if (j == 5)
                return monday.AddDays(5);
            else //if (j == 6)
                return monday.AddDays(6); ;


        
            

        }

        private MealDTO getMealById(int i)  //NOT GOOD, jer je hard kodovano (ako se ubaci i obrok u bazu, vise treba da se popravi...)
        {
            if (i == 0)
                return new MealDTO("breakfast");
            else if (i == 1)
                return new MealDTO("lunch");
            else //if (i == 2)
                return new MealDTO("dinner");
        }
        private DateTime getDayOfTheWeekById(int j)
        {
            if (j == 0) //pon
                return Utility.GetThisWeekMonday();
            else if (j == 6)
                return Utility.GetThisWeekSunday();
            else if (j == 1)
                return Utility.GetThisWeekTuesday();
            else if (j == 2)
                return Utility.GetThisWeekWednesday();
            else if (j == 3)
                return Utility.GetThisWeekThursday();
            else if (j == 4)
                return Utility.GetThisWeekFriday();
            else //if (j == 5)
                return Utility.GetThisWeekSaturday();
        }
        private DayOfWeek getDayOfTheWeekForId(int j)
        {
            if (j == 0) //pon
                return DayOfWeek.Monday;
            else if (j == 6)
                return DayOfWeek.Sunday;
            else if (j == 1)
                return DayOfWeek.Tuesday;
            else if (j == 2)
                return DayOfWeek.Wednesday;
            else if (j == 3)
                return DayOfWeek.Thursday;
            else if (j == 4)
                return DayOfWeek.Friday;
            else //if (j == 5)
                return DayOfWeek.Saturday;
        }


    }
}