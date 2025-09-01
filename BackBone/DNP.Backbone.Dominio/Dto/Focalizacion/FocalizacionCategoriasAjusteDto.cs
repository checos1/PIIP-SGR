using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Focalizacion
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
	public class CategoriasIndicadoresDto
	{
		public int ProyectoId { get; set; }
		public string BPIN { get; set; }
		public List<PoliticasCategoriasIndicadoresDto> Politicas { get; set; }
	}

	public class PoliticasCategoriasIndicadoresDto
	{
		public int PoliticaId { get; set; }
		public string Politica { get; set; }
		public List<ListaCategoriasDto> Categorias { get; set; }

	}

	public class ListaCategoriasDto
	{
		public int Categoria1Id { get; set; }
		public string Categoria1 { get; set; }
		public int Categoria2Id { get; set; }
		public string Categoria2 { get; set; }
		public int FocalizacionId { get; set; }
		public List<IndicadoresCategoriaDto> ListaIndicadores { get; set; }
	}
	public class IndicadoresCategoriaDto
	{
		public int IndicadorId { get; set; }
		public string Indicador { get; set; }
		public int ProyectoId { get; set; }
		public int CategoriaId { get; set; }
		public int FocalizacionIndicadorId { get; set; }
		public string Accion { get; set; }
	}
}
