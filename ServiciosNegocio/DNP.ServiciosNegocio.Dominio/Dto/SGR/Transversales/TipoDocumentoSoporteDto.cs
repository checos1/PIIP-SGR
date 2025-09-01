namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TipoDocumentoSoporteDto
    {
        public int Id { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string TipoDocumento { get; set; }
        public bool Obligatorio { get; set; }
        public string CampoValidacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
    }

}
