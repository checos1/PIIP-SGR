using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Poblacion
{

    [ExcludeFromCodeCoverage]
    public class PoblacionVigenciasDto
    {
        public int? Vigencia { get; set; }
        public List<PoblacionVigenciaLocalizacion> Localizacion { get; set; }
    }
}
