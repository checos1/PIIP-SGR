using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica
{
	public class IndicadoresPoliticaDto
	{
		public string BPIN { get; set; }
		public string Politica { get; set; }
		public string Categoria1 { get; set; }
		public string Categoria2 { get; set; }
		public string Indicador { get; set; }
		public List<LocalizacionesDto> Localizaciones { get; set; }
	}

	public class LocalizacionesDto
	{
		public string Departamento { get; set; }
		public string Municipio { get; set; }
		public string TipoAgrupacion { get; set; }
		public string Agrupacion { get; set; }
		public string Localizacion { get; set; }

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
