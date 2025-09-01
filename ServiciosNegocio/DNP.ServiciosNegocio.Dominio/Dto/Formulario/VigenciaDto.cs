using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class VigenciaDto
    {
        public long? Vigencia { get; set; }
        public decimal? GranTotalPorVigencia { get; set; }
        public List<ObjetivoEspecificoCadenaValorDto> ObjetivosEspecificos { get; set; }
        public List<ProblemaCentralDto> ProblemaCentral { get; set; }
        public List<FuenteDto> Fuente { get; set; }
    }
}