using System.ComponentModel.DataAnnotations;

namespace BeFitApp.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Exercise Type name must be between 2 and 100 characters.")]
        [Display(Name = "Exercise Type Name")]
        public string ExerciseName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
        [Display(Name = "Exercise Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Categroy is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category must be between 2 and 100 characters.")]
        [Display(Name = "Exercise Category")]
        public string Category { get; set; }

        [Display(Name = "Requires Equipment")]
        public bool RequiresEquipment { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
