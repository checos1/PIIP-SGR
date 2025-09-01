namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosConsultaProductoDto
    {
        public string Bpin { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid AccionId { get; set; }
    }
}