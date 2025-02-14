using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Timetable.Models;
using Timetable.Models.CSCM.DynamicTimeTable;

namespace Timetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TimeTableCSCM timeTable= new TimeTableCSCM();
            return View(timeTable);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult GenerateTimeTable(TimeTableCSCM timeTable)
        {
            if (!ModelState.IsValid)
                return View("Index", timeTable);

            if (timeTable.Subjects.Sum(s => s.AssignedHours) != timeTable.TotalHours)
            {
                ViewBag.Error = "Total assigned hours must match total hours for the week.";
                return View("Index", timeTable);
            }

            ViewBag.TimeTable = GenerateTable(timeTable);
            return View("Index", timeTable);
        }

        private List<List<string>> GenerateTable(TimeTableCSCM model)
        {
            var subjectPool = new List<string>();
            foreach (var subject in model.Subjects)
                subjectPool.AddRange(Enumerable.Repeat(subject.SubjectName, subject.AssignedHours));

            var timetable = new List<List<string>>();
            int index = 0;

            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < model.WorkingDays; j++)
                {
                    if (index < subjectPool.Count)
                        row.Add(subjectPool[index++]);
                }
                timetable.Add(row);
            }

            return timetable;
        }
    }
}
