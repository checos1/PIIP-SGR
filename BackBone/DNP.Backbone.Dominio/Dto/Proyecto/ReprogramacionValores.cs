using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
	public class ReprogramacionValores
	{

		public int ProyectoId { get; set; }
		public int TramiteId { get; set; }
		public int ProductoId { get; set; }
		public int PeriodoProyectoId { get; set; }
		public decimal ReprogramadoNacion { get; set; }
		public decimal ReprogramadoPropios { get; set; }
	}
}
