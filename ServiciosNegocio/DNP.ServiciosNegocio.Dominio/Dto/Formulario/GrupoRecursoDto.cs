
namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class GrupoRecursoDto
    {
        public int? GrupoRecursoId { get; set; }
        public string GrupoRecurso { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public List<ObjetivoEspecificoCadenaValorDto> ObjetivoEspecifico { get; set; }

    }
}
