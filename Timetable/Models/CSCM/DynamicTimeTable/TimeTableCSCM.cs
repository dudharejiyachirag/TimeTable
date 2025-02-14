using System.ComponentModel.DataAnnotations;

namespace Timetable.Models.CSCM.DynamicTimeTable
{
    public class TimeTableCSCM
    {
        [Required, Range(1, 7)]
        public int WorkingDays { get; set; }

        [Required, Range(1, 8)]
        public int SubjectsPerDay { get; set; }

        [Required, Range(1, 20)]
        public int TotalSubjects { get; set; }

        public int TotalHours => WorkingDays * SubjectsPerDay;

        public List<SubjectHoursModel> Subjects { get; set; } = new List<SubjectHoursModel>();
    }

    public class SubjectHoursModel
    {
        public string SubjectName { get; set; }
        public int AssignedHours { get; set; }
    }
}

