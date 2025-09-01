namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    using DNP.Backbone.Dominio.Dto.Tramites;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class NegocioVerificacionOcadPazDto
    {
        public Guid IdInstancia { get; set; }
        public Guid? IdTipoObjeto { get; set; }
        public string NombreObjetoNegocio { get; set; }
        public string IdObjetoNegocio { get; set; }
        public int IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreEntidadDestino { get; set; }
        public string TipoEntidad { get; set; }
        public Guid? IdAccion { get; set; }
        public string NombreAccion { get; set; }
        public string AgrupadorEntidad { get; set; }
        public List<Guid> roles { get; set; }
        public string Criticidad { get; set; }
        public int? SectorId { get; set; }
        public string EstadoProyecto { get; set; }
        public int? TipoEntidadId { get; set; }
        public string DescripcionCR { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionStr { get; set; }
        public DateTime? FechaFin { get; set; }
        public string FechaFinStr { get; set; }
        public int? ProyectoId { get; set; }
        public int? EstadoProyectoId { get; set; }
        public string SectorNombre { get; set; }
        public string Horizonte { get { return $"{HorizonteInicio} - {HorizonteFin}"; } }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public string TipoProyecto { get; set; }
        public string Operacion { get; set; }
        public TramiteDto TramiteDto { get; set; }
        public string NombreFlujo { get; set; }
        public int? EstadoInstanciaId { get; set; }
        public string EstadoInstancia { get; set; }
        public Guid? FlujoId { get; set; }
        public int? ProyectoTramiteId { get; set; }
        public string Etapa { get; set; }
        public Guid? grupoId { get; set; }
        public bool? GruposPermitidos { get; set; }
        public string CodigoProceso { get; set; }
        public Guid? IdNivel { get; set; }
        public DateTime? FechaPaso { get; set; }
        public string FechaPasoStr { get; set; }
        public DateTime? FechaFinPaso { get; set; }
        public string FechaFinPasoStr { get; set; }
        public string Macroproceso { get; set; }
        public string CodigoTramite { get; set; }
        public Guid? InstanciaPadreId { get; set; }
        public int? CRTypeId { get; set; }
        public int? ResourceGroupId { get; set; }
        public bool? AplicaAccion { get; set; }
        public int? UsuarioEncargadoOcadPazId { get; set; }
        public string UsuarioEncargadoAsignadoOcadPaz { get; set; }
        public Guid? IdRol { get; set; }
        public string AsignadoA { get; set; }
        public string CorreoAsignadoA { get; set; }
        public string Cumple { get; set; }

    }
}
