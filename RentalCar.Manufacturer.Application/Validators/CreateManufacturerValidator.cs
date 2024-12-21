using FluentValidation;
using RentalCar.Manufacturer.Application.Commands.Request;

namespace RentalCar.Manufacturer.Application.Validators;

public class CreateManufacturerValidator : AbstractValidator<CreateManufacturerRequest>
{
    public CreateManufacturerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Informe o nome");
        
        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Informe o telefone")
            .Length(9,25).WithMessage("O telefone deve ter entre 9 - 25 digitos");
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Informe o e-mail")
            .EmailAddress().WithMessage("Informe um e-mail válido");
    }
}

