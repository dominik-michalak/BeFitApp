using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFitApp.Models
{
    public class MonthlyFee
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000)]
        public decimal Fee { get; set; }

        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string? Remarks { get; set; }

        public string? Status { get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}