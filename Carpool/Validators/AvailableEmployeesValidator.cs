using System;
using Carpool.Contracts.Request;
using FluentValidation;

namespace Carpool.Validators
{
    public class AvailableEmployeesValidator : AbstractValidator<AvailableEmployeesRequest>
    {
        public AvailableEmployeesValidator()
        {
            RuleFor(x => x.StartDateUtc)
                .GreaterThan(DateTime.UtcNow.Date);

            RuleFor(x => x.EndDateUtc)
                .GreaterThan(x => x.StartDateUtc);
        }
    }
}