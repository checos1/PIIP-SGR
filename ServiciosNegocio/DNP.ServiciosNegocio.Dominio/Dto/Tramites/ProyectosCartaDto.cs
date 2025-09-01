namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProyectosCartaDto
    {
        public string NombreProyecto { get; set; }
        public string Bpin { get; set; }
        public string CodigoPrograma { get; set; }
        public string Programa { get; set; }
        public string CodigoSubprograma { get; set; }
        public string Subprogramal { get; set; }
        public string CodigoEntidad { get; set; }
        public string Entidad { get; set; }
        public long? ConsecutivoCodigoPresupuestal { get; set; }
        public bool? esConstante { get; set; }
    }
}
