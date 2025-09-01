using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{

    public class PreguntasSeguimientoProyectoDto
    {
        public int tramiteId { get; set; }
        public int proyectoId { get; set; }
        public int tipoTramiteId { get; set; }
        public Guid nivelId { get; set; }
        public Guid instanciaId { get; set; }
        public Guid faseId { get; set; }
        public List<RespuestasSeguimientoProyectoDto> lstRespuestas { get; set; }
    }

}
