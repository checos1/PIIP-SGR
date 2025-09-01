using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class VigenciaFuturaCorrienteFuenteDto
    {
        public int? EtapaId { get; set; }
        public string Etapa { get; set; }
        public int? FuenteId { get; set; }
        public string Fuente { get; set; }
        public string TipoEntidad { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public double? ApropiacionVigente { get; set; }
        public string LabelBotonFuente { get; set; } = "+";
        public bool HabilitaEditarFuente { get; set; } = false;
        public double? ValorTotalVigenciaFutura { get; set; }
        public double? ValorTotalVigenciaFuturaOriginal { get; set; }
        public double? ValorTotalVigenciaFuturas { get; set; }
        public double? ValorTotalVigenciaFuturasOriginal { get; set; }
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<VigenciaFuturaCorrienteFuenteVigenciaDto> Vigencias { get; set; }
    }
}
