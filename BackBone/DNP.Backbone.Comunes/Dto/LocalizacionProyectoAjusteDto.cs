using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
	public class LocalizacionProyectoAjusteDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string Accion { get; set; }
        public string Justificacion { get; set; }
        public int SeccionCapituloId { get; set; }
        public Guid InstanciaId { get; set; }
        public List<LocalizacionAjusteDto> NuevaLocalizacion { get; set; }
    }
}
