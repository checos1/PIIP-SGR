namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramiteFuentePresupuestalDto
    {
        public int IdFuente { get; set; }
        public int IdProyectoFuenteTramite { get; set; }
        public int IdProyectoTramite { get; set; }
        public double ValorContracreditoCSF { get; set; }
        public double ValorContracreditoSSF { get; set; }
        public double ValorInicial { get; set; }
        public double Valorvigente { get; set; }
        public string Accion { get; set; }
        public int IdTipoValorInicial { get; set; }
        public int IdTipoValorVigente { get; set; }
        public int IdTipoValorContracreditoCSF { get; set; }
        public int IdTipoValorContracreditoSSF { get; set; }
        public int IdTramite { get; set; }
        public int IdProyecto { get; set; }
    }   
}
