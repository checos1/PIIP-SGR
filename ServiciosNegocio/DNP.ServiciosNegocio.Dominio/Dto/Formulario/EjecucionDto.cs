namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EjecucionDto
    {
        public int? Mes { get; set; }
        public decimal? ApropiacionInicialMes { get; set; }
        public decimal? ApropiacionVigenteMes { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public int? IdGrupoRecurso { get; set; }
        public string GrupoRecurso { get; set; }
    }
}