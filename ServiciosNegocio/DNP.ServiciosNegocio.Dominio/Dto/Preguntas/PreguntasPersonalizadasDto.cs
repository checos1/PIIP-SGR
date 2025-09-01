using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Preguntas
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ServicioPreguntasPersonalizadasDto : CuestionarioDto
    {
        public Guid InstanciaId { get; set; }
        public int SeccionCapituloId { get; set; }
        public List<TematicaDto> PreguntasEspecificas { get; set; }
        public List<TematicaDto> PreguntasGenerales { get; set; }
        public List<AgregarPreguntaRequisito> AgregarPreguntasRequisitos { get; set; }
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TematicaDto
    {
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public List<PreguntasPersonalizadasDto> Preguntas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class infoTematicaDto
    {
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class OpcionDto
    {
        public int? OpcionId { get; set; }
        public string ValorOpcion { get; set; }
    }
}
