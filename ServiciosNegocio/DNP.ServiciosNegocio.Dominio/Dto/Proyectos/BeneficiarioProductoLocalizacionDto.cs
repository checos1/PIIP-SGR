using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class BeneficiarioProductoLocalizacionDto
    {
        public int ProyectoId { get; set; }
        public int ProductoId { get; set; }
        public int LocalizacionId { get; set; }
        public List<DetalleVigencias> DetalleVigencias { get; set; }
    }

    public class DetalleVigencias
    {
        public int PeriodoProyectoId { get; set; }
        public int ValorActual { get; set; }
    }
}
