using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Preguntas
{
    public class ServicioPreguntasPersonalizadasDto : CuestionarioDto
    {
        public Guid InstanciaId { get; set; }
        public int SeccionCapituloId { get; set; }
        public List<TematicaDto> PreguntasEspecificas { get; set; }
        public List<TematicaDto> PreguntasGenerales { get; set; }
    }

    public class PreguntasPersonalizadasDto
    {
        public int? IdPregunta { get; set; }
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public string Pregunta { get; set; }
        public int? OrdenPregunta { get; set; }
        public string Explicacion { get; set; }
        public string TipoPregunta { get; set; }
        public List<object> OpcionesRespuesta { get; set; }
        public string AyudaOpcionesRespuesta { get; set; }
        public string Respuesta { get; set; }
        public List<object> ObligaObservacion { get; set; }
        public string ObservacionPregunta { get; set; }
        public string AyudaObservacion { get; set; }
        public string Cabecera { get; set; }
        public string Nota { get; set; }
        public string CumpleEn { get; set; }
        public string IdPreguntaPadre { get; set; }
        public string HabilitaEn { get; set; }
        public string Sector { get; set; }
        public string Acuerdo { get; set; }
        public string Clasificacion { get; set; }
    }

    public class TematicaDto
    {
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public List<PreguntasPersonalizadasDto> Preguntas { get; set; }
    }

    public class OpcionDto
    {
        public int? OpcionId { get; set; }
        public string ValorOpcion { get; set; }
    }

    public class CuestionarioDto
    {
        public string CodigoBPIN { get; set; }
        public int? Cuestionario { get; set; }
        public int? CR { get; set; }
        public Guid? Fase { get; set; }
        public int? EntidadDestino { get; set; }
        public string ObservacionCuestionario { get; set; }
        public bool? Definitivo { get; set; }
        public bool? CumpleCuestionario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; }
        public bool? AgregarRequisitos { get; set; }
    }
}
