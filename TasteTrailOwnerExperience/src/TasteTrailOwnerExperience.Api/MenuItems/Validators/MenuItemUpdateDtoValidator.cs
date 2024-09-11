using FluentValidation;
using TasteTrailOwnerExperience.Core.MenuItems.Dtos;

namespace TasteTrailOwnerExperience.Api.MenuItems.Validators;

public class MenuItemUpdateDtoValidator : AbstractValidator<MenuItemUpdateDto>
{
    public MenuItemUpdateDtoValidator()
    {
        RuleFor(mi => mi.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(mi => mi.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(mi => mi.Description)
            .MaximumLength(500);

        RuleFor(mi => mi.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(10000);
    }
}