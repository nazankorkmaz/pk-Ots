using FluentValidation;
 using Ots.Schema;
 
 namespace Ots.Api.Impl.Validation;
 
 public class AccountValidator : AbstractValidator<AccountRequest>
 {
     public AccountValidator()
     {
         RuleFor(x => x.CustomerId).GreaterThan(0);
         RuleFor(x => x.CurrencyCode).MinimumLength(3).MaximumLength(3);
         RuleFor(x => x.Name).MinimumLength(2).MaximumLength(50);
     }
 }