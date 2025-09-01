namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EstadoDto
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Codigo { get; set; }

    }
}
