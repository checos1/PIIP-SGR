using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public class InstanciaResultado
    {
        public bool Exitoso { get; set; }
        public string MensajeOperacion { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? AccionPorInstanciaId { get; set; }

    }
}
