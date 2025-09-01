namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TipoDocumentoTramiteDto
    {
        public int Id { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public int? TipoTramiteId { get; set; }
        public bool? Obligatorio { get; set; }
        public int? IdRol { get; set; }
        public string RolId { get; set; }
    }
}
