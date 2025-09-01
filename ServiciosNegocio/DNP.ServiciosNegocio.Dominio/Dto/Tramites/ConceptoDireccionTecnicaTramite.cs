using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ConceptoDireccionTecnicaTramite
    {
        public int? FaseId { get; set; }
        public int? ProjectId { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? FormularioId { get; set; }
        public int? CR { get; set; }
        public bool? AgregarRequisitos { get; set; }
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool? Cumple { get; set; }
        public bool? Definitivo { get; set; }
        public int? PreguntaId { get; set; }
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public string ObservacionPregunta { get; set; }
        public List<object> OpcionesRespuesta { get; set; }
        public string NombreRol { get; set; }
        public string NombreNivel { get; set; }
        public int? CuestionarioProyectoId { get; set; }
        public int TramiteId { get; set; }
        public int EsPreguntaAbierta { get; set; }

    }
}
