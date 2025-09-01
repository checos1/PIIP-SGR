using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Priorizacion
{
    public class ProyectoPriorizacionDetalleDto
    {
        public int Id { get; set; }
        public string BPIN { get; set; }
        public int PriorizacionId { get; set; }
        public Guid InstanciaId { get; set; }
        public bool? Priorizado { get; set; }
        public DateTime FechaPriorizacion { get; set; }
        public bool? TieneMetodologiaCierreBrechas { get; set; }
        public int? MagnitudBrecha { get; set; }
        public int? PosicionBrecha { get; set; }
        public String CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class ProyectoPriorizacionDetalleResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
