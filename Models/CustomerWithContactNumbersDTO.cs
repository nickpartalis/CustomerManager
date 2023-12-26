using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Models
{
    public class CustomerWithContactNumbersDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string? HomeNumber { get; set; }
        public string? WorkNumber { get; set; }
        public string? MobileNumber { get; set; }
    }
}
