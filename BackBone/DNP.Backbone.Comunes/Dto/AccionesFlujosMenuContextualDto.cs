
namespace DNP.Backbone.Comunes.Dto
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionesFlujosMenuContextualDto
    {
        public Guid Id { get; set; }
        public Guid? AccionInstanciaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Guid IdFormulario { get; set; }
        public string ModificadoPor { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Estado { get; set; }
        public string OrdenVisualizacion { get; set; }
        public FlujoMenuContextualDto FlujoAnidado { get; set; }
        public Guid? RolId { get; set; }
        public List<RolAutorizacionDto> Roles { get; set; }
        public TipoAccion TipoAccion { get; set; }
        public List<AccionesParalelasFlujoDto> AccionesParalelas { get; set; }
        public string Ventana { get; set; }
        public Guid? IdNivel { get; set; }
        public string DescripcionNivel { get; set; }
        public List<AutorizacionAccionesDto> Usuarios { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Cedula { get; set; }
        public string NombreUsuario { get; set; }
        public string Entidad { get; set; }
        public int? IdEntidad { get; set; }
        public bool RequiereInfoNivelAnterior { get; set; }
        public bool? VisualizarCumple { get; set; }
        public bool? VisualizaEnviarSubpaso { get; set; }
        public int? EstadoAccionPorInstanciaId { get; set; }
        public List<AccionesFlujosSubPasosDto> SubPasos { get; set; }
    }
}
