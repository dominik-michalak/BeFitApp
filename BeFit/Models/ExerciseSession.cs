using System.ComponentModel.DataAnnotations;

namespace BeFitApp.Models
{
    public class ExerciseSession : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Start date and time is required")]
        [Display(Name = "Start Date and Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End date and time is required")]
        [Display(Name = "End Date and Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [MaxLength(500)]
        [Display(Name = "Session Notes")]
        public string? Notes { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Intensity must be between 1 to 10")]
        [Display(Name = "Intensity Level (1-10)")]
        public int IntensityLevel { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }

        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [Display(Name = "Created By User")]
        public virtual AppUser? CreatedBy { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDateTime <= StartDateTime)
            {
                yield return new ValidationResult(
                    "End date and time must be after start date and time.",
                    new[] { nameof(EndDateTime) });
            }
        }
    }
}
