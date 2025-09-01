using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Focalizacion
{
    public class FocalizacionSolicitarConceptoDto
    {
        public int Id { get; set; }
        public Guid InstanciaId { get; set; }
        public int ProyectoId { get; set; }
        public int PoliticaId { get; set; }
        public string Descripcion { get; set; }
        public string IdUsuarioDNP { get; set; }
        public bool Activo { get; set; }
        public bool Enviado { get; set; }
        public int EntityTypeCatalogOptionId { get; set; }
        public string CreadoPor { get; set; }
    }
}
