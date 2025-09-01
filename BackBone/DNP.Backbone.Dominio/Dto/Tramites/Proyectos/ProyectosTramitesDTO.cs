using DNP.Backbone.Dominio.Dto.Proyecto;
using System;
using System.Collections.Generic;


namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    public class ProyectosTramitesDTO
    {
        public string Mensaje { get; set; }
        public string NombreTramite { get; set; }
        public string Idtramite { get; set; }
        public List<NegocioDto> ListaProyectos { get; set; }
    }

    public class JustificacionTematicaDto
    {
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public List<JustificacionTramiteProyectoDto> justificaciones { get; set; }
    }

    public class JustificacionTramiteProyectoDto
    {
        public string NivelId { get; set; }
        public string InstanciaId { get; set; }
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public int? JustificacionId { get; set; }
        public int? JustificacionPreguntaId { get; set; }
        public int? OrdenJustificacionPregunta { get; set; }
        public string JustificacionPregunta { get; set; }
        public string JustificacionRespuesta { get; set; }
        public string ObservacionPregunta { get; set; }
        public string ObservacionRespuesta { get; set; }
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public string NombreRol { get; set; }
        public string NombreNivel { get; set; }
        public int? CuestionarioId { get; set; }
        
        // estos campos se adicionaron por le PBI 28341
        public string Usuario { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Paso { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta { get; set; }

        //estos campos se adicionaron por le PBI 31339
        public string OpcionesRespuesta { get; set; }
    }

    public class JustificacionPasoDto
    {
        public string Paso { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public List<JustificacionTramiteProyectoDto> justificaciones { get; set; }
    }

    public class ProyectoJustificacioneDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreCorto { get; set; }
        public List<JustificacionPasoDto> ListaJustificacionPaso { get; set; }
    }

}
