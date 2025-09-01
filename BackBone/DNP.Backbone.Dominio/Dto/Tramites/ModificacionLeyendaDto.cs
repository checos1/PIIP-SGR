namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ModificacionLeyendaDto
    {
        public int? Id { get; set; }
        public int? TramiteProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string Justificacion { get; set; }
        public bool? ErrorAritmetico { get; set; }
        public bool? ErrorTranscripcion { get; set; }
        //consulta
        public string BPIN { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public string NombreProyectoOriginal { get; set; }
        public string CodigoPresupuestal { get; set; }
        public string NombreProyectoModificacion { get; set; }
        //saber si es solo nombre
        public int? tipoUpdate { get; set; }
    }
}
