using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.ServiciosNegocio.Dominio.Dto.PoliticasCategoriasIndicadores
{
	public class PoliticasCategorias
	{
		public int PoliticaId { get; set; }
		public string Politica { get; set; }
		public int Categoria1Id { get; set; }
		public string Categoria1 { get; set; }
		public int Categoria2Id { get; set; }
		public string Categoria2 { get; set; }
		public int IndicadorId { get; set; }
		public string Indicador { get; set; }
	}
}