using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    public class LocalizacionProyectoAjusteDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string Accion { get; set; }
        public string Justificacion { get; set; }
        public int SeccionCapituloId { get; set; }
        public Guid InstanciaId { get; set; }
        public List<LocalizacionNuevaDto> NuevaLocalizacion { get; set; }
    }

}
