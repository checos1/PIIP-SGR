using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    public class RespuestaEnvioPoliticaSubDireccionAjustes
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int PoliticaId { get; set; }
        public string IdUsuarioDNP { get; set; }
    }
}
