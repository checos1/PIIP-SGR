using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
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
