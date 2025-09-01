using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Preguntas
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AgregarPreguntasDto
    {
        public int AtributoId { get; set; }
        public string Atributo { get; set; }
        public Nullable<int> AtributoPadre { get; set; }
        public int PreguntaId { get; set; }
        public string Pregunta { get; set; }
        public string Explicacion { get; set; }
        public int OpcionId { get; set; }
        public string ValorOpcion { get; set; }
        public int? Padre { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PreguntasDto
    {
        public int? IdPregunta { get; set; }
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public string Tematica { get; set; }
        public int? OrdenTematica { get; set; }
        public string Pregunta { get; set; }
        public int? OrdenPregunta { get; set; }
        public string Explicacion { get; set; }
        public List<object> OpcionesRespuestas { get; set; }
        public string OpcionesRespuestasSeleccionado { get; set; }
        public List<object> ObligaObservacion { get; set; }
        public string ObservacionPregunta { get; set; }
        public string Cabecera { get; set; }
        public string Nota { get; set; }
        public string CumpleEn { get; set; }
        public string Sector { get; set; }
        public string Acuerdo { get; set; }
        public string Clasificacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ServicioPreguntasDto : CuestionarioDto
    {
        public List<PreguntasDto> PreguntasEspecificas { get; set; }
        public List<PreguntasDto> PreguntasGenerales { get; set; }
        public List<AgregarPreguntaRequisito> AgregarPreguntasRequisitos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AgregarPreguntaBase
    {
        public int Id { get; set; }
        public int? AtributoPadre { get; set; }
        public string Name { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AgregarPreguntaSector : AgregarPreguntaBase
    {
        public List<AgregarPreguntaClassificacion> Items { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AgregarPreguntaClassificacion : AgregarPreguntaBase
    {
        public List<PreguntasDto> Items { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AgregarPreguntaRequisito : AgregarPreguntaBase
    {
        public List<AgregarPreguntaSector> Items { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
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
