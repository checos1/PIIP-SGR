using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Enrutamiento
{
    [ExcludeFromCodeCoverage]
    public class ReglaEnrutamiento
    {
        public int Regla { get; set; }
        public string Descripcion { get; set; }
    }
}
