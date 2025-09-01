using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
	public class HorizonteProyectoDto
	{
		public int IdProyecto { get; set; }
		public string VigenciaInicio { get; set; }
		public string VigenciaFinal { get; set; }
		public int Mantiene { get; set; }
		public int SeccionCapituloId { get; set; }
		public string Usuario { get; set; }
		public string GuiMacroproceso { get; set; }
		public Guid InstanciaId { get; set; }
	}
}
