using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class CustomerDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Street name must be between 2 and 50 characters.")]
        public string StreetName { get; set; } = string.Empty;

        [Required(ErrorMessage = "House number is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "House number must be between 1 and 10 characters.")]
        public string HouseNumber { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Apartment number cannot be longer than 10 characters.")]
        public string ApartmentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Postal code must be in the format XX-XXX.")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Town is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Town name must be between 2 and 50 characters.")]
        public string Town { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Phone number is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CustomerDTO), "ValidateDateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth >= DateTime.Now)
            {
                return new ValidationResult("Date of birth must be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
