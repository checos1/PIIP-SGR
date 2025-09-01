using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion
{
    public class FocalizacionCategoriasDto
    {
        public int ProyectoId { get; set; }        
        public int PoliticaId { get; set; }
        public int CategoriaId { get; set; }        
        public int LocalizacionId { get; set; }
        public string Vigencia { get; set; }
        public decimal MetaCategoria { get; set; }
        public int PersonasCategoria { get; set; }
        public decimal MetaIndicadorSecundario { get; set; }
    }
}
