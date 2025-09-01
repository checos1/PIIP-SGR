using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class LocalizacionJustificacionProyectoDto
	{

        public LocalizacionJustificacionProyectoDto()
        {
        }

        public int? ProyectoId { get; set; }
        public List<LocalizacionFirmeDto> ProyectosLocalizacionFirme { get; set; }
        public List<LocalizacionNuevaDto> ListadoNuevos { get; set; }
        public List<LocalizacionBorradaDto> ListadoBorrados { get; set; }
        public List<LocalizacionVerificacionDto> VerificacionColumnas { get; set; }
    }
}

