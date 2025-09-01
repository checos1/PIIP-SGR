using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Focalizacion
{
    public class FocalizacionPoliticaTFuentesDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<FuentesFinanciacion> FuentesFinanciacion { get; set; }
    }
    public class FuentesFinanciacion
    {
        public int? FuenteId { get; set; }
        public int? EtapaId { get; set; }
        public string Etapa { get; set; }
        public int? FinaciadorId { get; set; }
        public string Finaciador { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? RecursoId { get; set; }
        public string Recurso { get; set; }
        public string Texto { get; set; }
        public List<Politicas> Politicas { get; set; }
    }
    public class Politicas
    {
        public int? FocalizacionPoliticaId { get; set; }
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public int? Categoria { get; set; }
        public int? Indicador { get; set; }
        public int? CrucePolitica { get; set; }
    }
}
