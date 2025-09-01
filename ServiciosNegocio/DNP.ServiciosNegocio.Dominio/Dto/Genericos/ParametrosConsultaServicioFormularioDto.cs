using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Genericos
{
    [ExcludeFromCodeCoverage]
    public class ParametrosConsultaServicioFormularioDto
    {
        public Guid AccionId { get; set; }
        public Guid InstanciaId { get; set; }
        public string Json { get; set; }
    }
}
