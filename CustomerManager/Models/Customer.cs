using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManager.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2 and 100 Characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name Must Be Between 2 and 100 Characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set;}

        [StringLength(30)]
        [Display(Name = "Home Number")]
        public string? HomeNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Work Number")]
        public string? WorkNumber { get; set; }

        [StringLength(30)]
        [Display(Name = "Mobile Phone Number")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Address is Required.")]
        [StringLength(150, ErrorMessage = "Address Should not Exceed 150 Characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email Address is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

    }
}
