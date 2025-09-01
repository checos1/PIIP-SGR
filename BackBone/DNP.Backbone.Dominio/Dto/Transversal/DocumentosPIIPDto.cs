using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    public class DocumentosPIIPDto
    {
        public string Origen { get; set; }
        public string OrigenCompleto { get; set; }
        public string Vigencia { get; set; }
        public string NombreProceso { get; set; }
        public string NombreFlujo { get; set; }
        public string CodigoProceso { get; set; }
        public string Paso { get; set; }
        public Guid AccionId { get; set; }
        public Guid InstanciaId { get; set; }
        public string ObjetoNegocioId { get; set; }
        public Guid? IdNivel { get; set; }
        public int ProyectoId { get; set; }
        public string Periodo { get; set; }
    }
}
