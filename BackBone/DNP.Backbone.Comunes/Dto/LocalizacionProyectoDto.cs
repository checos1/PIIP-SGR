using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
	public class LocalizacionProyectoDto
	{
        public LocalizacionProyectoDto()
        {
        }

        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<LocalizacionDto> Localizacion { get; set; }
        public List<LocalizacionNuevaDto> NuevaLocalizacion { get; set; }
    }
}
