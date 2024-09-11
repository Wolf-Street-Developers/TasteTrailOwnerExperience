using FluentValidation;
using TasteTrailOwnerExperience.Core.Venues.Dtos;

namespace TasteTrailOwnerExperience.Api.Venues.Validators;

public class VenueUpdateDtoValidator : AbstractValidator<VenueUpdateDto>
{
    public VenueUpdateDtoValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200);
        
        RuleFor(v => v.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(v => v.Description)
            .MaximumLength(500);

        RuleFor(v => v.Email)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(v => v.ContactNumber)
            .MaximumLength(15);

        RuleFor(v => v.AveragePrice)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(10000);
    }
}