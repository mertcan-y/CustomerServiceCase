using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.Database.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [RegularExpression(pattern:"^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$",ErrorMessage = "Email wrong")]
        public string Email { get; set; }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name).Length(3, 15);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Surname).Length(3, 25);
        }
    }
}
