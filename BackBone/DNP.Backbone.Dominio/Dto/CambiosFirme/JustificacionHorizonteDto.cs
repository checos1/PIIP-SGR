using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto
{
	public class JustificacionHorizontenDto
	{
		public int HorizonteId { get; set; }
		public int Periodo { get; set; }
		public int Vigencia { get; set; }
		public string Estado { get; set; }
		public int? VigenciaFirme { get; set; }

	}

	public class JustificacionHorizonteCambiosDto
	{
		public List<JustificacionHorizontenDto> Vigencia { get; set; }
		public List<JustificacionHorizontenDto> VigenciaFirme { get; set; }

	}
}
