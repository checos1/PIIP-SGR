namespace DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class RelacionPlanificacionDto
    {
        public int Id { get; set; }
        public string NumeroConpes { get; set; }
        public string NombreConpes { get; set; }
        public string Estado { get; set; }

    }
}
