using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class CofinanciacionProyectoDto
    {
        public int? ProyectoId { get; set; }
        public string CodigoBPIN { get; set; }
        public int? CR { get; set; }
        public List<CofinanciacionDto> Cofinanciacion { get; set; }
    }
}
