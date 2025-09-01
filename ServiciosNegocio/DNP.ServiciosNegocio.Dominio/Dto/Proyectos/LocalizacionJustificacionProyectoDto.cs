using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
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
