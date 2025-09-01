using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DNP.Backbone.Servicios.Validadores
{
    public class AlertasReglasValidator : AbstractValidator<AlertasReglasDto>
    {
        private readonly IDictionary<TipoColumna, Func<string, bool>> _validators;
        private readonly IDictionary<TipoColumna, int[]> _tipoColumnasCondicionales;

        public AlertasReglasValidator()
        {
            int[] condicionalesNumericas = new[] { 1, 2, 3, 4, 5, 6 };
            int[] condicionalesTexto = new[] { 1, 6, 7 };

            _validators = new Dictionary<TipoColumna, Func<string, bool>>
            {
                { TipoColumna.Int,      valor => Regex.IsMatch(valor, @"^\d+$") },
                { TipoColumna.String,   valor => !string.IsNullOrWhiteSpace(valor) },
                { TipoColumna.Money,    valor => Regex.IsMatch(valor,  @"^\d{1,3}(?:\,\d{3})*.\d{2}$") },
                { TipoColumna.Datetime, valor => DateTime.TryParse(valor, out var date) }
            };

            _tipoColumnasCondicionales = new Dictionary<TipoColumna, int[]>
            {
                { TipoColumna.Int,  condicionalesNumericas },
                { TipoColumna.String, condicionalesTexto },
                { TipoColumna.Money, condicionalesNumericas },
                { TipoColumna.Datetime, condicionalesNumericas }
            };

            RuleFor(x => x.Condicional)
                .NotEmpty()
                .WithMessage("Ingrese un operador lógico para el condicional");
            RuleFor(x => x.MapColumnasUnoId)
                .NotEmpty()
                .WithMessage("Seleccione una columna para el condicional");
            RuleFor(x => x.Valor != null || x.MapColumnasDosId != null)
                .NotEmpty()
                .WithMessage("Ingrese un valor para el condicional. Un valor o columna para comparar");

            RuleFor(x => x.Valor)
                .Must((regla, valor) => regla.MapColumnasDosId != null || _validators[regla.MapColumnasUno.TipoColumna](valor))
                .WithMessage("Ingrese un valor válido para el tipo de columna seleccionado en condicional");

            RuleFor(x => x)
                .Must((regla) => regla.Valor != null || regla.MapColumnasUno?.TipoColumna == regla.MapColumnasDos?.TipoColumna)
                .WithMessage("El tipo de columna uno debe ser el mismo que el tipo de columna dos");

            RuleFor(x => x)
                .Must((regla) => regla.Condicional != null && _tipoColumnasCondicionales[regla.MapColumnasUno.TipoColumna].Contains(regla.Condicional ?? 0))
                .WithMessage("El operador seleccionado para el tipo de columna no es válido en condicional");
        }
    }
}
