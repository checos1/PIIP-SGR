using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramitesProyectosDto
    {
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public Guid? InstanciaId { get; set; }
        public int EntidadId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class JustificacionTematicaDto
    {
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public List<JustificacionTramiteProyectoDto> justificaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class JustificacionTramiteProyectoDto
    {
        public Guid NivelId { get; set; }
        public Guid InstanciaId { get; set; }
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public int? JustificacionId { get; set; }
        public int? JustificacionPreguntaId { get; set; }
        public int? OrdenJustificacionPregunta { get; set; }
        public string JustificacionPregunta { get; set; }
        public string JustificacionRespuesta { get; set; }
        public string ObservacionPregunta { get; set; }
        public string OpcionesRespuesta { get; set; }
        public string ObservacionRespuesta { get; set; }
        public string NombreRol { get; set; }
        public string NombreNivel { get; set; }
        public int? CuestionarioId { get; set; }
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        // estos campos se adicionaron por le PBI 28341
        public string Usuario { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Paso { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta  { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class TramitesValoresProyectoDto
    {
        public Nullable<decimal> DecretoNacion { get; set; }
        public Nullable<decimal> DecretoPropios { get; set; }
        public Nullable<decimal> VigenteNacion { get; set; }
        public Nullable<decimal> VigentePropios { get; set; }
        public Nullable<decimal> DisponibleNacion { get; set; }
        public Nullable<decimal> DisponiblePropios { get; set; }
        public Nullable<decimal> VigenciaFuturaNacion { get; set; }
        public Nullable<decimal> VigenciaFuturaPropios { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class TramitesResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public byte[] Byte64 { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class JustificacionPasoDto
    {
        public string Paso { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public List<JustificacionTramiteProyectoDto> justificaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProyectoJustificacioneDto
    {
        public int ProyectoId { get; set;  }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public List<JustificacionPasoDto> ListaJustificacionPaso { get; set; }
    }

}
