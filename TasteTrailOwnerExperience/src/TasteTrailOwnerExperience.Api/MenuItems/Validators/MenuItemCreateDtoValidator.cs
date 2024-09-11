using FluentValidation;
using TasteTrailOwnerExperience.Core.MenuItems.Dtos;

namespace TasteTrailOwnerExperience.Api.MenuItems.Validators;

public class MenuItemCreateDtoValidator : AbstractValidator<MenuItemCreateDto>
{
    public MenuItemCreateDtoValidator()
    {
        RuleFor(mi => mi.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(mi => mi.Description)
            .MaximumLength(500);

        RuleFor(mi => mi.Price)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(10000);

        RuleFor(mi => mi.MenuId)
            .NotEmpty()
            .GreaterThan(0);
    }
}