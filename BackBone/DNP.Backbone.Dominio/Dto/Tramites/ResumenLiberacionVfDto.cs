using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class ResumenLiberacionVfDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public decimal TotalValoresUtilizados { get; set; }
        public List<ResumenLiberacionVfValoresDto> ValoresAutorizadosUtilizados { get; set; }
        public decimal TotalAutorizadosNacion { get; set; }
        public decimal TotalAutorizadosPropios { get; set; }
        public decimal TotalUtilizadosNacion { get; set; }
        public decimal TotalUtilizadosPropios { get; set; }
    }

    public class ResumenLiberacionVfValoresDto
    {
        public int Vigencia { get; set; }
        public decimal AprobadosNacion { get; set; }
        public decimal AprobadosNPropios { get; set; }
        public decimal UtilizadoNacion { get; set; }
        public decimal UtilizadoPropios { get; set; }
    }
}
