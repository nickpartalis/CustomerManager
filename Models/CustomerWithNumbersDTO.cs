using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Models
{
    public class CustomerWithNumbersDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string? HomeNumber { get; set; }
        public string? WorkNumber { get; set; }
        public string? MobileNumber { get; set; }

        public class Validator : AbstractValidator<CustomerWithNumbersDTO>
        {
            public Validator()
            {
                RuleFor(dto => dto.FirstName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("First name must not be empty.")
                    .Length(2, 100).WithMessage("First name must have between 2 and 100 characters.")
                    .Matches(@"^[A-Za-zα-ωΑ-Ωά-ώΆ-Ώ\s]*$").WithMessage("Names should only contain letters.");

                RuleFor(dto => dto.LastName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Last name must not be empty.")
                    .Length(2, 100).WithMessage("Last name must have between 2 and 100 characters.")
                    .Matches(@"^[A-Za-zα-ωΑ-Ωά-ώΆ-Ώ\s]*$").WithMessage("Names should only contain letters.");

                RuleFor(dto => dto.Address)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Address must not be empty.")
                    .Length(3, 150).WithMessage("Address must have between 3 and 150 characters.");

                RuleFor(dto => dto.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Email must not be empty and must be a valid email address.");

                RuleFor(dto => dto.HomeNumber)
                    .NotEmpty()
                    .When(dto => string.IsNullOrEmpty(dto.WorkNumber) && string.IsNullOrEmpty(dto.MobileNumber))
                    .WithMessage("At least one contact number must be provided.");

                RuleFor(dto => dto.WorkNumber)
                    .NotEmpty()
                    .When(dto => string.IsNullOrEmpty(dto.HomeNumber) && string.IsNullOrEmpty(dto.MobileNumber))
                    .WithMessage("At least one contact number must be provided.");

                RuleFor(dto => dto.MobileNumber)
                    .NotEmpty()
                    .When(dto => string.IsNullOrEmpty(dto.HomeNumber) && string.IsNullOrEmpty(dto.WorkNumber))
                    .WithMessage("At least one contact number must be provided.");
            }
        }
    }

}
