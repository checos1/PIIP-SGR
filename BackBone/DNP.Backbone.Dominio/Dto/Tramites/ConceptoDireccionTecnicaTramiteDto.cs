using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class ConceptoDireccionTecnicaTramiteDto
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
        public int EsConcepto { get; set; }
    }
}
