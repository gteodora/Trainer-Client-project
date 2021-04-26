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
    public class TrainingController : Controller
    {
        TrainingDAO trainingDAO = new TrainingDAO();
        public const int NUMBER_OF_TRAINING_ROWS = 14;  //8-9, 9-10, ..., 20-21, 21-22   //22-8=14  mogucih satnica u toku dana
        public const int NUMBER_OF_DAYS = 7;
        static TrainingDTO[][] trainingDescriptions = new TrainingDTO[NUMBER_OF_TRAINING_ROWS][];
        static ClientTrainingDTO[][] trainerTrainingDescriptions = new ClientTrainingDTO[NUMBER_OF_TRAINING_ROWS][];

        //ok: za klijenta
        // GET: Training/Details/5
        [Authorize(Roles = Utility.CLIENT_ROLE)]
        public ActionResult Details(int id, DateTime monday)
        {
            var training = trainingDAO.GetById(id);
            if (training != null)
            {
                ViewBag.SelectedMonday = monday;
                return View(training);
            }
            else
            {
                return View("Error");
            }
        }

        //OK
        // GET: Training/Create
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Create(int id, DateTime monday, DateTime startTime, DateTime endTime, int trainingTypeId, int programId)
        {
            TrainingDTO training = GetById(id);  
            if (training != null)
            {
                ViewBag.SelectedMonday = monday;
                return View(training);
            }
            else
            {
                return View("Error");
            }
        }

        // GET: Training/CreateWithoutClientAndProgram
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult CreateWithoutClientAndProgram(int id, DateTime monday, DateTime startTime, DateTime endTime, int trainingTypeId)
        {
            TrainingDTO training = GetById(id);
            if (training != null)
            {
                ViewBag.SelectedMonday = monday;
                training.TrainingTypeId = 2;
                return View(training);
            }
            else
            {
                return View("Error");
            }
        }
        // POST: Training/CreateWithoutClientAndProgram
        [HttpPost]
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult CreateWithoutClientAndProgram( DateTime monday, FormCollection collection)
        {
            try
            {
                int trainingTypeId = int.Parse(collection["TrainingTypeId"]);
                DateTime startTimeWeek = DateTime.Parse(collection["StartTime"]);
                DateTime endTime;
                string description = collection["Description"];
                int duration = int.Parse(collection["Duration"]);
                int recurring = int.Parse(collection["Recurring"]);


                //ovo sedmicno gleda
                for (int j = 1; j <= recurring; j++)
                {
                    DateTime startTime = startTimeWeek;
                    //U istom danu da nadodaje trening
                    if (duration >= 1)
                    {
                        for (int i = 1; i <= duration; i++)
                        {
                            endTime = startTime.AddHours(1);
                            int newId = trainingDAO.CreateWithoutClientAndProgram( trainingTypeId, startTime, endTime, description);
                            startTime = startTime.AddHours(1);
                        }

                    }
                    startTimeWeek = startTimeWeek.AddDays(7);
                }

                DateTime sunday = monday.AddDays(6);
                return RedirectToAction("AllTrainerTrainings", new {  monday = monday });
            }
            catch
            {
                return View("Error");
            }
        }
        //OK - treba da trening potrazi po Id-ju u nizu trainingDescriptions[][] i kada ga nadje, treba ga vratiti
        public static TrainingDTO GetById(int id)
        {
            for(int i = 0; i < NUMBER_OF_TRAINING_ROWS; i++) 
            { 
                for (int j = 0; j < NUMBER_OF_DAYS; j++)
                {
                    if (((TrainingDTO)(trainingDescriptions[i][j])).Id == id) { return trainingDescriptions[i][j]; }
                }
            }
            return null;
        }

        //OK
        // POST: Training/Create
        [HttpPost]
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Create(int programId, DateTime monday, FormCollection collection)
        {
            try
            {
                int trainingTypeId = int.Parse(collection["TrainingTypeId"]);
                DateTime startTimeWeek = DateTime.Parse(collection["StartTime"]);
                DateTime endTime;
                string description = collection["Description"];
                int duration = int.Parse(collection["Duration"]);
                int recurring = int.Parse(collection["Recurring"]);


                //ovo sedmicno gleda
                for (int j = 1; j <= recurring; j++)
                {
                    DateTime startTime = startTimeWeek;
                    //U istom danu da nadodaje trening
                    if (duration >= 1)
                    {
                        for (int i = 1; i <= duration; i++)
                        {
                            endTime = startTime.AddHours(1);
                            int newId = trainingDAO.Create(programId, trainingTypeId, startTime, endTime, description);
                            startTime = startTime.AddHours(1);
                        }

                    }
                    startTimeWeek = startTimeWeek.AddDays(7);
                }

                DateTime sunday = monday.AddDays(6);
                return RedirectToAction("TrainingPlanForProgram", new { programId = programId, monday = monday });
            }
            catch
            {
                return View("Error");
            }
        }

        //OK
        // GET: Training/Edit/5
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Edit(int id, DateTime monday, DateTime startTime, DateTime endTime)
        {

            var training = trainingDAO.GetById(id);
            if (training != null)
            {
                ViewBag.SelectedMonday = monday;
                return View(training);
            }
            else
            {
                return View("Error");
            }
        }
       
        // POST: Training/Edit/5
        [HttpPost]
        [Authorize(Roles = Utility.TRAINER_ROLE)]
        public ActionResult Edit(int id, DateTime monday, FormCollection collection)
        {
            try
            {
                TrainingDTO trainingBeforeEditing = trainingDAO.GetById(id);
                int programID = int.Parse(collection["ProgramId"]);
                int trainingTypeId = int.Parse(collection["TrainingTypeId"]);
                DateTime startTime = DateTime.Parse(collection["StartTime"]);
                //DateTime endTime = DateTime.Parse(collection["EndTime"]);
                string description = collection["Description"];
                int newDuration = int.Parse(collection["Duration"]);

                List<TrainingDTO> trainings = trainingDAO.getTrainingsByDescriptionTypeDay(trainingBeforeEditing.ProgramId, trainingBeforeEditing.Description, trainingBeforeEditing.StartTime, trainingBeforeEditing.TrainingTypeId);
                int previousDuration= trainings.Count();
                trainings.OrderBy(t=>t.StartTime.Hour); //sortiram treninge po redu
                
                
                if (newDuration > previousDuration) //dodajem novi trening/nove treninge
                {
                    DateTime lastStartTime = trainings.Last().StartTime; //od zadnjeg nejgovo startno vrijeme
                    DateTime lastEndTime = lastStartTime.AddHours(1);
                    trainings.ForEach(training => trainingDAO.Edit(training.Id, trainingTypeId, description));
                    for (int i = newDuration - previousDuration; i > 0; --i)
                    {
                        lastStartTime = lastStartTime.AddHours(1);
                        lastEndTime = lastEndTime.AddHours(1);
                        trainingDAO.Create(programID, trainingTypeId, lastStartTime, lastEndTime, description);
                    }
                }
                else if (newDuration < previousDuration)
                {//brisem zadnji trening/zadnje treninge
                    TrainingDTO trainingForDelete = null;
                    trainings.ForEach(training => trainingDAO.Edit(training.Id, trainingTypeId, description));
                    for (int i = previousDuration - newDuration; i > 0; --i)
                    {
                        trainingForDelete = trainings.Last();
                        trainings.Remove(trainingForDelete);
                        trainingDAO.Delete(trainingForDelete.Id);
                    }
                }
                else
                {
                    //za svaki trening iznad, editujem preostala polja: TIP TRENINGA i OPIS
                    trainings.ForEach(training => trainingDAO.Edit(training.Id, trainingTypeId, description));
                }

                //int number = trainingDAO.Edit(id, trainingTypeId, startTime, endTime ,description);
                DateTime sunday = monday.AddDays(6);
                return RedirectToAction("TrainingPlanForProgram", new { programId = programID, monday = monday, sunday = sunday });
            }
            catch
            {
                return View("Error");
            }
        }

        
        //NADJEM sve treninge koji imaju ovaj program id - iz BAZE  I  pripadaju opsegu datuma u ovoj sedmici
        // GET: Meal/MealPlanForProgram/5
        public ActionResult TrainingPlanForProgram(int programId, DateTime monday)
        {
            var sunday = monday.AddDays(6);
            var model = trainingDAO.getByProgram(programId, monday, sunday);
            model = model.Where(t => t.StartTime.Date >= monday.Date && t.EndTime.Date <= sunday).ToList();
            var orderedTrainings = PutTrainingsInOrder(model, monday, sunday);
            ViewBag.Tranings = orderedTrainings;
            ViewBag.SelectedMonday = monday;
            return View();
        }

        /*reda treninge tako da traings[1] je 7-8; traings[2] je 8-9...
a dane postavlja tako da krece od ponedeljka-nedelje. tj/ [0]-[6]*/
        private TrainingDTO[][] PutTrainingsInOrder(List<TrainingDTO> model, DateTime monday, DateTime sunday)
        {
            TrainingDTO emptyTraining = new TrainingDTO();
            for (int i = 0; i < NUMBER_OF_TRAINING_ROWS; i++)
            {
                trainingDescriptions[i] = new TrainingDTO[7];
                for (int j = 0; j < NUMBER_OF_DAYS; j++)
                {
                    emptyTraining = (TrainingDTO)getTrainingByIAndJ(i, j, false, monday, sunday); //setuje starttime i endtime
                    emptyTraining.TrainingTypeId=0;
                    //popunjavanje podacima iz baze
                    if (model != null)
                    {
                      
                        trainingDescriptions[i][j] = (TrainingDTO)(model.Where((m) => predicate(m, i))    // Func<TrainingDTO, int, bool> checkDoesMHasTimeLikeI = (m, ii) => predicate(m, ii);
                            .FirstOrDefault(m => m.StartTime.DayOfWeek == getDayOfTheWeekForId(j)));
                    }
                    // inicijalizacija niza odgovarajucim elementom     
                    if (trainingDescriptions[i][j] == null)
                    {
                        trainingDescriptions[i][j] = emptyTraining;
                    }
                }
            }
            return trainingDescriptions;
        }
        
        private bool predicate(TrainingDTO m, int i)
        {
            if (check(m,i))
            {
                return true;
            }
            else return false;
        }

        private bool check(TrainingDTO m, int i)
        {
            if ((i == 0 && m.StartTime.Hour == 8 && m.EndTime.Hour == 9) ||
                 (i == 1 && m.StartTime.Hour == 9 && m.EndTime.Hour == 10) ||
                 (i == 2 && m.StartTime.Hour == 10 && m.EndTime.Hour == 11) ||
                 (i == 3 && m.StartTime.Hour == 11 && m.EndTime.Hour == 12) ||
                 (i == 4 && m.StartTime.Hour == 12 && m.EndTime.Hour == 13) ||
                 (i == 5 && m.StartTime.Hour == 13 && m.EndTime.Hour == 14) ||
                 (i == 6 && m.StartTime.Hour == 14 && m.EndTime.Hour == 15) ||
                 (i == 7 && m.StartTime.Hour == 15 && m.EndTime.Hour == 16) ||
                 (i == 8 && m.StartTime.Hour == 16 && m.EndTime.Hour == 17) ||
                 (i == 9 && m.StartTime.Hour == 17 && m.EndTime.Hour == 18) ||
                 (i == 10 && m.StartTime.Hour == 18 && m.EndTime.Hour == 19) ||
                 (i == 11 && m.StartTime.Hour == 19 && m.EndTime.Hour == 20) ||
                 (i == 12 && m.StartTime.Hour == 20 && m.EndTime.Hour == 21) ||
                 (i == 13 && m.StartTime.Hour == 21 && m.EndTime.Hour == 22))
                return true;
            else return false;
        }

        //OK
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
        //OK
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
        //OK
        private object getTrainingByIAndJ(int i, int j, bool isItClientTraining, DateTime monday, DateTime sunday)
        {
            var training=new TrainingDTO();
            if (isItClientTraining)
            {
                training = new ClientTrainingDTO();
            }
            DateTime dateByJ = getDayOfTheWeekById(j);
            if (i == 0)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j,monday,sunday), 8, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 9, 0, 0);
            }
            else if (i == 1)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 9,0,0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 10,0,0);
            }
            else if (i == 2)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 10, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 11, 0, 0);
            }
            else if (i == 3)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 11, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 12, 0, 0);
            }
            else if (i == 4)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 12, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 13, 0, 0);
            }
            else if (i == 5)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 13, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 14, 0, 0);
            }
            else if (i == 6)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 14, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 15, 0, 0);
            }
            else if (i == 7)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 15, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 16, 0, 0);
            }
            else if (i == 8)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 16, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 17, 0, 0);
            }
            else if (i == 9)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 17, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 18, 0, 0);
            }
            else if (i == 10)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 18, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 19, 0, 0);
            }
            else if (i == 11)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 19, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 20, 0, 0);
            }
            else if (i == 12)
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 20, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 21, 0, 0);
            }
            else
            {
                training.StartTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 21, 0, 0);
                training.EndTime = Utility.ChangeTime(getDayOfTheWeekByIdAndGetDateByWeek(j, monday, sunday), 22, 0, 0);
            }
            return training;
        }

        private DateTime getDayOfTheWeekByIdAndGetDateByWeek(int j, DateTime monday, DateTime sunday)
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
            else //if (j == 5)
                return sunday;
        }


        //NADJEM sve treninge koji imaju ovaj training type(SA TRENEROM) iz BAZE  I  pripadaju opsegu datuma u ovoj sedmici
        // GET: Trainining/AllTrainerTrainings
        public ActionResult AllTrainerTrainings( DateTime monday)
        {
            var sunday = monday.AddDays(6);
            var model = trainingDAO.getTrainerTrainingsInPickedWeek(monday);
            model = model.Where(t => t.StartTime.Date >= monday.Date && t.EndTime.Date <= sunday).ToList();
            var orderedTrainings = PutTrainerTrainingsInOrder(model, monday, sunday);     // ViewBag.Tranings = orderedTrainings;
            ViewBag.Tranings = orderedTrainings;
            ViewBag.SelectedMonday = monday;
            return View();
        }

        //ovdje ce prvo genrisati samostalne treninge, ostalo je isto za datume i id i program id,  jedino NOVINA: ime i prezime i id klijenta je 0
        private object PutTrainerTrainingsInOrder(List<ClientTrainingDTO> model, DateTime monday, DateTime sunday)
        {
             ClientTrainingDTO emptyTraining = new ClientTrainingDTO();
             for (int i = 0; i < NUMBER_OF_TRAINING_ROWS; i++)
             {
                 trainingDescriptions[i] = new ClientTrainingDTO[7];
                 for (int j = 0; j < NUMBER_OF_DAYS; j++)
                 {
                     emptyTraining = (ClientTrainingDTO)getTrainingByIAndJ(i, j, true, monday, sunday);
                    emptyTraining.TrainingTypeId = 1; //samostalno
                    //budu nule:
                    //emptyTraining.ProgramId = 0;
                   // emptyTraining.Id = 0;
                     //popunjavanje podacima iz baze
                     if (model != null)
                     {

                         trainingDescriptions[i][j] = (ClientTrainingDTO)(model.Where((m) => predicate(m, i))    // Func<TrainingDTO, int, bool> checkDoesMHasTimeLikeI = (m, ii) => predicate(m, ii);
                             .FirstOrDefault(m => m.StartTime.DayOfWeek == getDayOfTheWeekForId(j)));
                     }
                     // inicijalizacija niza odgovarajucim elementom     
                     if (trainingDescriptions[i][j] == null)
                     {
                         trainingDescriptions[i][j] = emptyTraining;
                     }
                 }
             }
             return trainingDescriptions;
            
        }


        // GET: MealType/Delete/5
        public ActionResult Delete(int id, DateTime monday, DateTime sunday)
        {
            var model = trainingDAO.getById(id);
            if (model != null)
            {
                return View(model);
            }
            else return View("Error");
        }

        // POST: MealType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int programId, DateTime monday, DateTime sunday, FormCollection collection)
        {
            try
            {
              //  int programId = int.Parse(collection["ProgramId"]);
               // DateTime monday = DateTime.Parse(collection["StartTime"]);
               // DateTime sunday = DateTime.Parse(collection["EndTime"]);
                trainingDAO.Delete(id);

                return RedirectToAction("TrainingPlanForProgram", new { programId = programId, monday = monday, sunday = sunday });
            }
            catch
            {
                return View("Error");
            }
        }


    }
    }
