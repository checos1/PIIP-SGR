using System;

namespace DNP.Backbone.Dominio.Dto.Priorizacion
{
    public class PriorizacionDatosBasicosDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public string Recurso { get; set; }
        public string Fase { get; set; }
        public Nullable<decimal> ValorProyecto { get; set; }
    }
}
