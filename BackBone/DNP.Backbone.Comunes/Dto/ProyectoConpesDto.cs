using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ProyectoConpesDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int ConpesId { get; set; }
        public string NumeroConpes { get; set; }
        public string NombreConpes { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime VigenciaDesde { get; set; }
        public DateTime VigenciaHasta { get; set; }
    }
}
