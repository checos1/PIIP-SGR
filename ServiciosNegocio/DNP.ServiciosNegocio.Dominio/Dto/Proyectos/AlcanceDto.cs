using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class AlcanceDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public DateTime FechaInicioEtapaInversion { get; set; }
        public List<ProyectoNuevaDto> DescripcionGeneralProyectoNueva { get; set; }
    }
}
