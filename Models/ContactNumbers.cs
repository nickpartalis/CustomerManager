using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManager.Models
{
    public class ContactNumbers
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [Display(Name = "Home Number")]
        public string? HomeNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Work Number")]
        public string? WorkNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Mobile Phone Number")]
        public string? MobileNumber { get; set; }


        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
