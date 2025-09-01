using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class RegionalizacionProyectoDto
    {
        public string Bpin { get; set; }
        public List<VigenciaRegionalizacionDto> Vigencias { get; set; }
    }
}
