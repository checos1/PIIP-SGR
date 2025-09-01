using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    public class ParametrosCrearNotificacionFlujoDto
    {
        public string IdUsuarioDNP { get; set; }
        public string NombreNotificacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsManual { get; set; }
        public int Tipo { get; set; }
        public string ContenidoNotificacion { get; set; }
        public string NombreArchivo { get; set; }
    }
}
