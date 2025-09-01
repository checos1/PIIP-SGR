namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CodigoPresupuestalDto
    {
        public int Id { get; set; }
        public int EntidadId { get; set; }
        public int ProyectoId { get; set; }
        public int TramiteId { get; set; }
        public string CodigoPresupuestal { get; set; }
        public string CodigoEntidad { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public int Consecutivo { get; set; }
    }
}
