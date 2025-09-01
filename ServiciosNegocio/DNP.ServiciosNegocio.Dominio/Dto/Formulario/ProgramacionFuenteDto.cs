using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class ProgramacionFuenteDto
    {
        public int Id { get; set; }
        public double? ValorSolicititado { get; set; }
        public List<RegionalizacionRecursosDto> RegionalizacionRecursos { get; set; }
    }
}
