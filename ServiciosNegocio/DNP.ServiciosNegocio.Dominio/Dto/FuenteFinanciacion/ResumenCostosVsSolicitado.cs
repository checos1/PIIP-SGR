using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
  
    public class ResumenCostosVsSolicitado
    {
        public int vigencia { get; set; }
        public decimal PreinversioCostos { get; set; }
        public decimal PreinversionSolicitado { get; set; }
        public decimal InversionCostos { get; set; }
        public decimal InversionSolicitado { get; set; }
        public decimal OperacionCostos { get; set; }
        public decimal OperacionSolicitado { get; set; }


        
    }
}
