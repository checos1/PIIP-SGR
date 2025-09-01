namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
	using System.Diagnostics.CodeAnalysis;

	[ExcludeFromCodeCoverage]
	public class ResultadoProcedimientoDto
	{
		public bool Exito { get; set; }
		public string Mensaje { get; set; }
	}
}
