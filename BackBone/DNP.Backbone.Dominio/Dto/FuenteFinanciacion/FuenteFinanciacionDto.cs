namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteFinanciacionDto
    {
        public int? FuenteId { get; set; }
        public int? Vigencia { get; set; }
        public int? Mes { get; set; }
        public int? EtapaId { get; set; }
        public string GrupoRecurso { get; set; }
        public int? TipoEntidadId { get; set; }
        public string TipoEntidad { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public string OtraEntidad { get; set; }
        public int? TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public string NombreCompleto { get; set; }
        public int? ProgramacionId { get; set; }
        public decimal? Solicitado { get; set; }
        public decimal? ApropiacionInicial { get; set; }
        public int? EjecucionId { get; set; }
        public decimal? ApropiacionVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public int TipoValorContracreditoCSF { get; set; }
        public int TipoValorContracreditoSSF { get; set; }
        public decimal? ValorContracreditoCSF { get; set; }
        public decimal? ValorContracreditoSSF { get; set; }
        public int TipoValorAprobadoCSF { get; set; }
        public int TipoValorAprobadoSSF { get; set; }
        public decimal? ValorAprobadoCSF { get; set; }
        public decimal? ValorAprobadoSSF { get; set; }

        public decimal? ValorIncialCSF { get; set; }
        public decimal? ValorIncialSSF { get; set; }
        public decimal? ValorVigenteCSF { get; set; }
        public decimal? ValorVigenteSSF { get; set; }
    }
}
