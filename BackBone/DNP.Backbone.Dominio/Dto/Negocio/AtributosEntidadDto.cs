using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Negocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AtributosEntidadDto
    {
        public string ModificadoPor { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int? SectorId { get; set; }
        public string Orden { get; set; }
        public bool CabeceraSector { get; set; }
        public int Id { get; set; }

    }
}
