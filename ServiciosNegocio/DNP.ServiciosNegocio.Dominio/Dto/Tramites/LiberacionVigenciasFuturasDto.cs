using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class LiberacionVigenciasFuturasDto
    {
        public int tramiteProyectoId {get; set;} 
        public int tramiteId {get; set;} 
        public string creadoPor {get; set;} 
        public System.DateTime? vigenciaDesde {get; set;} 
        public System.DateTime? vigenciaHasta { get; set; }
    }
}
