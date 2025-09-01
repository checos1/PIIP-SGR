using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion
{
    public class DatosReprogramacionDto
    {
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<ValoresReprogramacion> ValoresReprogramacion { get; set; }
    }

    public class ValoresReprogramacion
    {
        public int? Vigencia { get; set; }
        public decimal? ReprogramadoNacion { get; set; }
        public decimal? ReprogramadoPropios { get; set; }
        public bool? ValoresReprogramado { get; set; }
    }
}
