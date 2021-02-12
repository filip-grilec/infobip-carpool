using System;
using System.Linq;
using Carpool.Contracts.Request;
using FluentValidation;

namespace Carpool.Validators
{
    public class CreateTravelPlanValidator : AbstractValidator<CreateTravelPlanRequest>
    {
        public CreateTravelPlanValidator()
        {
            RuleFor(x => x.StartDateUtc)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .LessThan(x => x.EndDateUtc);

            RuleFor(x => x.CarId)
                .GreaterThan(0);

            RuleFor(x => x.EndLocationId)
                .GreaterThan(0);

            RuleFor(x => x.StartLocationId)
                .GreaterThan(0);
            
            RuleFor(x => x.EmployeeIds.Count())
                .GreaterThan(0);
        }
    }
}