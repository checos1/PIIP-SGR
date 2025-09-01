using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Auditoria
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class OpcionCatalogoTipoEntidadServiciosAuditoriaDto
    {
        public string Nombre { get; set; }
        public int? IdTipo { get; set; }
        public int? IdPadre { get; set; }
        public string CodigoEntidad { get; set; }
        public bool? EsActiva { get; set; }

        public bool? CabeceraSector { get; set; }
        public string Orden { get; set; }
        public int? SectorId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public string Mensaje { get; set; }
    }
}
