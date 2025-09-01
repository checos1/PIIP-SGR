using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ActividadCadenaValorDto
    {
        public int? Id { get; set; }
        public  int? ActividadId { get; set; }
        public string NombreActividad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Etapa{ get; set; }
        public int? TipoInsumoId { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public string Observacion { get; set; }
    }
}
