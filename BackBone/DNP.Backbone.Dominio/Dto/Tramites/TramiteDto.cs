namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramiteDto
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? FlujoId { get; set; }
        public TipoTramiteDto TipoTramite { get; set; }
        //public int? TipoTramiteId { get { return TipoTramite == null ? null : TipoTramite.Id; } }
        public string NombreTipoTramite { get; set; }
        //public string NombreTipoTramite { get; set; }
        public string Descripcion { get; set; }
        public string NombreTipoEntidad { get; set; }
        public int? TipoEntidadId { get; set; }
        public int? EntidadId { get; set; }
        public decimal? ValorProprio { get; set; }
        public decimal? ValorSGP { get; set; }
        public decimal? ValorNacion { get; set; }
        public int? EstadoId { get; set; }
        public string NombreSector { get; set; }
        public int? SectorId { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreEntidadDestino { get; set; }
        public string DescEstado { get; set; }
        public string IdentificadorCR { get; set; }
        public string NombreObjetoNegocio { get; set; }
        public string Criticidad { get; set; }
        public Guid IdAccion { get; set; }
        public Guid? IdTipoObjeto { get; set; }
        public string IdObjetoNegocio { get; set; }
        public Guid IdInstancia { get; set; }
        public string NombreAccion { get; set; }
        public string Etapa { get; set; }
        public string NumeroTramite { get; set; }
        public int? TramiteId { get; set; }
        public int? TipoTramiteId { get; set; }
        public string NombreSectorTramite { get; set; }
        public string DescripcionTramite { get; set; }
        public DateTime? FechaCreacionTramite { get; set; }
        public string NombreFlujo { get; set; }
        public string Macroproceso { get; set; }
    }

    public class ErroresTramiteDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Errores { get; set; }

    }
}
