using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFitApp.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Exercise type is required")]
        [Display(Name = "Exercise Type")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Exercise Type")]
        public virtual ExerciseType? ExerciseTypeNav { get; set; }

        [Required(ErrorMessage = "Exercise session is required")]
        [Display(Name = "Exercise Session")]
        public int ExerciseSessionId { get; set; }
        [Display(Name = "Exercise Session")]
        public virtual ExerciseSession? ExerciseSessionNav { get; set; }

        [Required(ErrorMessage = "Sets are required")]
        [Range(1, 100, ErrorMessage = "Sets must be between 1 and 100")]
        [Display(Name = "Sets")]
        public int Sets { get; set; }

        [Required(ErrorMessage = "Repetitions are required")]
        [Range(1, 100, ErrorMessage = "Repetitions must be between 1 and 100")]
        [Display(Name = "Repetitions")]
        public int Repetitions { get; set; }

        [Range(0, 1000, ErrorMessage = "Weight must be between 0 and 1000")]
        [Display(Name = "Weight (kg)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(6,2)")]
        public decimal? Weight { get; set; }

        [MaxLength(300)]
        [Display(Name = "Exercise Notes")]
        public string? Notes { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Dificulty must be between 1 to 10")]
        [Display(Name = "Dificulty Level  (1-10)")]
        public int DificultyLevel { get; set; }
    }
}
