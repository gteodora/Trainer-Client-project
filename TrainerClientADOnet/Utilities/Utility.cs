using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainerClientADOnet.Models;
using TrainerClientADOnet.Models.DAO;
using TrainerClientADOnet.Models.DTO;

namespace TrainerClientADOnet.Utilities
{
    public class Utility
    {
        public const string TRAINER_ROLE = "Trener"; //ovako je u bazi
        public const string ADMIN_ROLE = "Admin";
        public const string CLIENT_ROLE = "Klijent";
        public const string TRAINER_TRAININGS_COLOR = "#ffebcc";
        public const string PROGRAM_TYPE_MEAL = "Meal";
        public const string PROGRAM_TYPE_TRAINING = "Training";

        private static GoalTypeDAO goalTypeDAO = new GoalTypeDAO();
        private static TrainingTypeDAO trainingTypeDAO = new TrainingTypeDAO();
        private static MealTypeDAO mealTypeDAO = new MealTypeDAO();
        private static RoleDAO roleDAO = new RoleDAO();
        private static ProgramDAO programDAO = new ProgramDAO(); 
            private static ClientDAO clientDAO = new ClientDAO();
        private static TrainingDAO trainingDAO = new TrainingDAO();
        
        public static int TrainerTrainingType { get; internal set; }

        private Utility()
        {
            TrainerTrainingType = 2;  //todo: metoda koja dobavlja vriejdnost
        }

        //todo: rijesiti se ovih metoda
        public static DateTime GetThisWeekMonday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 1); //+1 da dobijem ponedeljak :)
        }

        public static DateTime GetThisWeekSunday()
        {
            if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                return DateTime.UtcNow;   // [0,6] npr. 3(srijeda)+7-3-1=6       //4+7
            else
            {
                return DateTime.UtcNow.AddDays(7 - (int)(DateTime.UtcNow.DayOfWeek));
            }
        }

        public static DateTime GetThisWeekTuesday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 2); //+1 da dobijem ponedeljak :)
        }

        public static DateTime GetThisWeekWednesday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 3); //+1 da dobijem ponedeljak :)
        }

        public static DateTime GetThisWeekThursday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 4); //+1 da dobijem ponedeljak :)
        }

        public static DateTime GetThisWeekFriday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 5); //+1 da dobijem ponedeljak :)
        }

        public static DateTime GetThisWeekSaturday()
        {
            return DateTime.UtcNow.AddDays((-(int)DateTime.UtcNow.DayOfWeek) + 6); //+1 da dobijem ponedeljak :)
        }

        public static string setProgramType(bool isMeal)
        {
            if (isMeal)
            {
                return PROGRAM_TYPE_MEAL;
            }
            else
            {
                return PROGRAM_TYPE_TRAINING;
            }
        }

        public static List<SelectListItem> returnSelectListOfProgramTypes()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = Utility.PROGRAM_TYPE_TRAINING,
                Value = false.ToString()
            });
            listItems.Add(new SelectListItem
            {
                Text = Utility.PROGRAM_TYPE_MEAL,
                Value = true.ToString(),
                Selected = true
            });
            return listItems;
        }

        public static List<SelectListItem> returnSelectListOfGoalTypes()
        {
            List<SelectListItem> goalTypesItems = new List<SelectListItem>();
            foreach (GoalTypeDTO item in goalTypeDAO.GetAll())
            {
                goalTypesItems.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return goalTypesItems;
        }
        
 public static List<SelectListItem> returnSelectListOfClients()
        {
            List<SelectListItem> goalTypesItems = new List<SelectListItem>();
            foreach (ClientDTO item in clientDAO.GetAll())
            {
                goalTypesItems.Add(new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                });
            }
            return goalTypesItems;
        }

        public static DateTime ChangeTime( DateTime datetime, int hours, int minutes, int seconds)
        {
            return new DateTime(
                datetime.Year,
                datetime.Month,
                datetime.Day,
                hours,
                minutes,
                seconds,
                datetime.Millisecond,
                datetime.Kind
                );
        }

        public static List<SelectListItem> returnSelectListOfTrainingTypes()
        {
            List<SelectListItem> typesItems = new List<SelectListItem>();
            foreach (TrainingTypeDTO item in trainingTypeDAO.GetAll())
            {
                if (("Sa trenerom").Equals(item.Name.TrimEnd()))
                {
                    typesItems.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    typesItems.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });
                }
            }
            return typesItems;
        }

        internal static DateTime getMondayFromTrainingDate()
        {
            throw new NotImplementedException();
        }

        public static List<SelectListItem> returnSelectListOfMealTypes()
        {
            List<SelectListItem> typesItems = new List<SelectListItem>();
            foreach (MealTypeDTO item in mealTypeDAO.GetAll())
            {
                typesItems.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return typesItems;
        }

public static List<SelectListItem> returnSelectListOfRoleTypes()
        {
            List<SelectListItem> typesItems = new List<SelectListItem>();
            foreach (RoleDTO item in roleDAO.GetAll())
            {
                //TODO: zbog EDIT-a: dodati provjeru da li je ovaj roleId isti kao i roleId od klijenta(parametar metode)
                //kreirati metodu koja za userId vraca njegov roleId, pa ovaj svaki iz liste provjeriti je li isti tome i onda to oznaciti sa Selected = true
                typesItems.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return typesItems;
        }
        
            public static List<SelectListItem> returnSelectListOfRoleTypesForAdmin()
        {
            List<SelectListItem> typesItems = new List<SelectListItem>();

            typesItems.Add(new SelectListItem
            {
                Text = Utility.ADMIN_ROLE,
                Value = GetRoleIdOfRole(Utility.ADMIN_ROLE),
            });

            typesItems.Add(new SelectListItem
            {
                Text = Utility.TRAINER_ROLE,
                Value = GetRoleIdOfRole(Utility.TRAINER_ROLE),
                Selected = true
            });

            return typesItems;
        }

        private static string GetRoleIdOfRole(string nameOfRole)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var role = context.Roles.Where(d => d.Name.Equals(nameOfRole)).FirstOrDefault();
            return role.Id;
        }

        public static string getClientIdForProgramId(string programId)
        {
            ProgramDTO program = programDAO.getById(programId);
            return program.ClientId;
        }

        public static string GetFullNameByProgramId(int programId)
        {
           return clientDAO.getClientFullNameByProgramId(programId);
        }
        public static int GetMaxTrainingDurationTime(int programId, string description, DateTime startTime, int trainingTypeId)
        {
            //dohvatim sve treninge taj dan, koji poslije pocinju, sortiram pa provjerim koji od tih treninga je prvi koji ima vece startTime od zadnjeg
            //i onda uymem prvi i oduymem rayliku startTime od ovog prvog ispod i ovog gore i dobijem max vrijeme trajanja ovog treninga
            
                List<TrainingDTO> trainingsByDay = trainingDAO.getTrainingsByProgramDay(programId, startTime);
                if (trainingsByDay != null) //prilikom kreiranaj u danu kada nema nijednog treninga, za svaki slucaj
                {
                    trainingsByDay = trainingsByDay.FindAll(t => t.StartTime.Hour > startTime.Hour);
                    trainingsByDay = trainingsByDay.OrderBy(t => t.StartTime).ToList();

                    TrainingDTO lastTraining = (trainingDAO.getTrainingsByDescriptionTypeDay(programId, description, startTime, trainingTypeId)).OrderBy(t => t.StartTime).Last();
                    TrainingDTO firstTrainingAfterThis = trainingsByDay.FirstOrDefault(t => t.StartTime >= lastTraining.EndTime);
                    if (firstTrainingAfterThis != null)
                    {
                        int firstTrainingHourAfterThis = firstTrainingAfterThis.StartTime.Hour;
                        return firstTrainingHourAfterThis - startTime.Hour;
                    }
                }
            
            return 22 - startTime.Hour; 
        }

        public static int GetMaxTrainingDurationTimeCreate(DateTime startTime, int programId)
        {
            List<TrainingDTO> trainingsByDay = trainingDAO.getTrainingsByProgramDay(programId, startTime);
            if (trainingsByDay != null) {//prilikom kreiranaj u danu kada nema nijednog treninga, za svaki slucaj
                trainingsByDay = trainingsByDay.FindAll(t => t.StartTime.Hour > startTime.Hour);
                if (trainingsByDay != null)
                {
                    trainingsByDay = trainingsByDay.OrderBy(t => t.StartTime).ToList();
                    TrainingDTO firstTrainingAfterThis = trainingsByDay.FirstOrDefault();
                    if (firstTrainingAfterThis != null) 
                    { 
                    int firstTrainingHourAfterThis = firstTrainingAfterThis.StartTime.Hour;
                    return firstTrainingHourAfterThis - startTime.Hour;
                    }
                }
            }
            return 22 - startTime.Hour; 
        }

        //ako je trening sa ovim osobinama, ali MANJEG SATA od ovog startTime vec upisan, vrati true 
        public static bool IsWrittenAlreadyThisTraining(int programId, string description, DateTime startTime, int trainingTypeId)
        {
            List<TrainingDTO> trainings = trainingDAO.getTrainingsByDescriptionTypeDay(programId, description,  startTime,  trainingTypeId);
           trainings = trainings.FindAll(t => t.StartTime.Hour < startTime.Hour);
            //TrainingDTO training = training.Find(t => startTime.Hour - t.StartTime.Hour == 1);
            if (trainings.Any(t => /*startTime.Hour<t.StartTime.Hour */ (startTime.Hour - t.StartTime.Hour) == 1)) //ako postoji trening sa MANJIM startTime.Hour on je VEC UPISAN.       //startTime.Hour - t.StartTime.Hour <= 5
            {
                return true;
            }
            return false;
        }

       public static DateTime previousMonday(DateTime monday)
        {
            return monday.AddDays(-7);
        }

        public static DateTime nextMonday(DateTime monday)
        {
            return monday.AddDays(7);
        }
     
 
        //todo-trajanje treninga
        //Vraca 
        public static int GetTrainingDuration(int programId, string description, DateTime startTime, int trainingTypeId)
        {
            List<TrainingDTO> trainings = trainingDAO.getTrainingsByDescriptionTypeDay(programId, description, startTime, trainingTypeId);

            return trainings.Count();
        }

        public static int GetMaxTrainingReccuringTimeCreate(DateTime startTime, int programId)
        {
            return 5; //todo
        }

        public static int GetMaxTrainingDurationTimeCreate2(DateTime startTime)
        {
            return 5; //todo
        }
        public static int GetMaxTrainingReccuringTimeCreate2(DateTime startTime)
        {
            return 5; //todo
        }

        public static List<SelectListItem> returnSelectListProgramsOfClient(string clientId)
        {
            var programs = programDAO.GetAllByClient(clientId);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(ProgramGoalTypeDTO item in programs)
            {
                list.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Description
                });
            }
            return list;
        }
    }
}