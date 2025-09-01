namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    using System.Collections.Generic;

    public class ProyectoFuenteFinanciacionDto
    {
        public string BPIN { get; set; }
        public int? CR { get; set; }
        public decimal? ValorTotalProyecto { get; set; }
        public List<FuenteFinanciacionDto> FuentesFinanciacion { get; set; }
    }
}
