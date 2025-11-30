using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFitApp.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Height")]
        public String Height { get; set; } = "0";

        [Required]
        [Column(TypeName = "int")]
        public int Weight { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [DisplayName("Address")]
        public String Address { get; set; }
    }
}
