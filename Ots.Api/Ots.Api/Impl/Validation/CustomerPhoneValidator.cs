using FluentValidation;
 using Ots.Api.Domain;
 using Ots.Schema;
 
 namespace Ots.Api.Impl.Validation;
 
 public class CustomerPhoneValidator : AbstractValidator<CustomerPhoneRequest>
 {
     public CustomerPhoneValidator()
     {
         RuleFor(x => x.CustomerId).GreaterThan(0);
         RuleFor(x => x.PhoneNumber).MinimumLength(12).MaximumLength(12)./*Matches("[0-9]+").*/WithMessage("Please enter a valid phone number");
         RuleFor(x => x.CountryCode).MinimumLength(3).MaximumLength(3);        
     }
 }