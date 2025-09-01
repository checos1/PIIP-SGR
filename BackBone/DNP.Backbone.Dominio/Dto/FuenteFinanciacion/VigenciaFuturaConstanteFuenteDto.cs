using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.FuenteFinanciacion
{
    public class VigenciaFuturaConstanteFuenteDto
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
        public double? ValorTotalVigenciaFuturaCorriente { get; set; }
        public double? ValorTotalVigenciaFuturaCorrienteOriginal { get; set; }
        public double? ValorTotalVigenciaFuturas { get; set; }
        public double? ValorTotalVigenciaFuturasOriginal { get; set; }
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<VigenciaFuturaConstanteFuenteVigenciaDto> Vigencias { get; set; }
    }
}
