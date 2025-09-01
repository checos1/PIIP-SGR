using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class RegionalizacionIndicadorDto
    {
        public string Bpin { get; set; }
        public List<VigenciaObjetivoProductoDto> Vigencias { get; set; }
    }
}
