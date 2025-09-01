using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class IndicadorProductoAgregarDto
    {
        public string Bpin { get; set; }
        public List<ObjetivosIndicadorProductoAgregarDto> Objetivos { get; set; }
    }
}
