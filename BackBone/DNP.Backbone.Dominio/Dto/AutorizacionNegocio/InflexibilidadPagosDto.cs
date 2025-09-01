using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class InflexibilidadPagosDto 
    {
        public int Id{ get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double Valor { get; set; }
        public int IdInflexibilidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Guid? IdArchivoBlob { get; set; }
        public string ContentType { get; set; }
    }
}
