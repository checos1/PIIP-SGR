using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ProyectosEntidadesDto
    {
        public int SectorId { get; set; }
        public string SectorNombre { get; set; }
        public int EntidadId { get; set; }
        public string EntidadNombre { get; set; }
        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; }
        public string CodigoBpin { get; set; }
        public string Estado { get; set; }
        public int? EstadoId { get; set; }
        public string AgrupadorEntidad { get; set; }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
    }
}
