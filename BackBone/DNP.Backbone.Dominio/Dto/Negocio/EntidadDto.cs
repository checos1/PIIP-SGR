using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Negocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EntidadDto
    {
        public AtributosEntidadDto AtributosEntidad { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int EntityTypeId { get; set; }
        public string Name { get; set; }
        public int Id{ get; set; }
    }
}
