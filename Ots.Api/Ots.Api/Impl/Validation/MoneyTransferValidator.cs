using FluentValidation;
 using Ots.Schema;
 
 namespace Ots.Api.Impl.Validation;
 
 public class MoneyTransferValidator : AbstractValidator<MoneyTransferRequest>
 {
     public MoneyTransferValidator()
     {
         RuleFor(x => x.Description).MinimumLength(2).MaximumLength(250);
         RuleFor(x => x.Amount).GreaterThan(0);
         RuleFor(x => x.FromAccountId).GreaterThan(0);
         RuleFor(x => x.ToAccountId).GreaterThan(0);
         RuleFor(x => x.ToAccountId).NotEqual(x => x.FromAccountId).WithMessage("From and To account cannot be the same");    
     }
 }