using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    public class CargaDatosDto
    {
        public int Id { get; set; }
        public decimal ValorContraCredito { get; set; }
        public decimal ValorCredito { get; set; }
        public int TipoCargaDatosId { get; set; }
        public string TipoEntidad { get; set; }
        public string Entidad { get; set; }
        public Guid? IdEntidad { get; set; }
        public Guid? IdArchivo { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
    }
}
