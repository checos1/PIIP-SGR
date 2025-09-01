using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    public class FocalizacionCategoriasAjusteDto
    {
        public int ProyectoId { get; set; }
        public string Bpin { get; set; }
        public int PoliticaId { get; set; }
        public int CategoriaId { get; set; }
        public int FuenteId { get; set; }
        public int ProductoId { get; set; }
        public int LocalizacionId { get; set; }
        public string Vigencia { get; set; }
        public decimal TotalFuene { get; set; }
        public decimal TotalCostoProducto { get; set; }
        public decimal EnAjuste { get; set; }
        public decimal MetaCategoria { get; set; }
        public int PersonasCategoria { get; set; }
        public decimal MetaIndicadorSecundario { get; set; }
    }
}
