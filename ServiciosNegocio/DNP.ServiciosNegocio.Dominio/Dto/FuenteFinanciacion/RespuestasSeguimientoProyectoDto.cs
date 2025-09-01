using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class RespuestasSeguimientoProyectoDto
    {
        public int ProyectoId { get; set; }
        public int idPregunta { get; set; }
        public int idRespuesta { get; set; }
        public string valor { get; set; }
    }
}
