using FluentValidation;
 using Ots.Schema;
 
 namespace Ots.Api.Impl.Validation;
 
 public class EftTransactionValidator : AbstractValidator<EftTransactionRequest>
 {
     public EftTransactionValidator()
     {
         RuleFor(x => x.Description).MinimumLength(2).MaximumLength(250);
         RuleFor(x => x.Amount).GreaterThan(0);
         RuleFor(x => x.AccountId).GreaterThan(0);
         RuleFor(x => x.ReceiverName).MinimumLength(2).MaximumLength(250);
         RuleFor(x => x.ReveiverIban).MinimumLength(2).MaximumLength(250);
     }
 }