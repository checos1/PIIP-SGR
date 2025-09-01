using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionesFlujosDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public Guid IdFormulario { get; set; }
        public int PeriodoValidez { get; set; }
        public string ServicioValidacion { get; set; }
        public string ServicioAlmacenamiento { get; set; }
        public Guid IdNotificacion { get; set; }
        public Guid? IdAccionPadre { get; set; }
        public string IdEstadoInicial { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public Guid IdFlujoAnidado { get; set; }
        public bool TipoParalela { get; set; }
        public Guid? IdScope { get; set; }
        public TipoAccion TipoAccion { get; set; }
        public DateTime? FechaLimite { get; set; }
        public Guid? FlujoId { get; set; }
        public int CantidadAcciones { get; set; }
        public FlujoDto FlujoAnidado { get; set; }
        public Guid? RolId { get; set; }
        public List<Guid> RolesIds { get; set; }
        public Guid? EstadoAccionId { get; set; }
        public Guid AccionInstanciaId { get; set; }
        public int? EstadoAccionPorInstanciaId { get; set; }
        public EnrutamientoDto Enrutamiento { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Guid? IdInstancia { get; set; }
        public Guid? IdAccion { get; set; }
        public bool? EsObligatoria { get; set; }
        public AccionesPorInstanciaDto AccionPorInstancia { get; set; }
        public List<List<AccionesFlujosDto>> AccionesParalelas { get; set; }
        public int? EnrutamientoId { get; set; }
        public int IndiceCreacion { get; set; }
        public string Ventana { get; set; }
        public Guid? IdNivel { get; set; }
        public string DescripcionNivel { get; set; }
        public bool RequiereInfoNivelAnterior { get; set; }
        public bool? VisualizarCumple { get; set; }
        public bool? VisualizaEnviarSubpaso { get; set; }
        public List<AutorizacionAccionesDto> Usuarios { get; set; }
        public List<AccionesFlujosSubPasosDto> SubPasos { get; set; }
    }
}
