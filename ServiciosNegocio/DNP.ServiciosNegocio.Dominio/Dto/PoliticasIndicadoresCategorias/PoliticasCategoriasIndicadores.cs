using DNP.ServiciosNegocio.Dominio.Dto.PoliticasCategoriasIndicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias
{
	public class PoliticasCategoriasIndicadores
	{
		public int ProyectoId { get; set; }
		public string Bpin { get; set; }
		public List<PoliticasCategorias> Politicas { get; set; }
	}
}