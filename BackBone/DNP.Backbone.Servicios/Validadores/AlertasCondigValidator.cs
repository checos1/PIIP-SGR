using DNP.Backbone.Comunes.Dto;
using FluentValidation;

namespace DNP.Backbone.Servicios.Validadores
{
    public class AlertasCondigValidator : AbstractValidator<AlertasConfigDto>
    {
        public AlertasCondigValidator()
        {
            RuleFor(x => x.NombreAlerta)
                .NotEmpty()
                .WithMessage("Ingrese un nombre para la alerta");
            RuleFor(x => x.TipoAlerta)
                .NotEmpty()
                .WithMessage("Seleccione el tipo de alerta");
            RuleFor(x => x.MensajeAlerta)
                .NotEmpty()
                .WithMessage("Ingrese un mensaje para la alerta");
            RuleFor(x => x.Classificacion)
                .NotEmpty()
                .WithMessage("Seleccione la clasificación de la alerta");
            RuleFor(x => x.AlertasReglasDtos)
                .NotEmpty()
                .WithMessage("La alerta debe tener al menos un condicional");
            
            RuleForEach(x => x.AlertasReglasDtos).SetValidator(new AlertasReglasValidator());
        }
    }
}
