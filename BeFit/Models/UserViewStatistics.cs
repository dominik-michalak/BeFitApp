namespace BeFitApp.Models
{
    public class UserViewStatistics
    {
        public string ExerciseTypeName { get; set; }
        public int TimesPerformed { get; set; }
        public int TotalRepetitions { get; set; }
        public decimal AverageWeight { get; set; }
        public decimal MaxWeight { get; set; }
    }
}
