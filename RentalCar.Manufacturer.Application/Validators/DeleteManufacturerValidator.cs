using FluentValidation;
using RentalCar.Manufacturer.Application.Commands.Request;

namespace RentalCar.Manufacturer.Application.Validators;

public class DeleteManufacturerValidator : AbstractValidator<DeleteManufacturerRequest>
{
    public DeleteManufacturerValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Informe o código");
    }
}

