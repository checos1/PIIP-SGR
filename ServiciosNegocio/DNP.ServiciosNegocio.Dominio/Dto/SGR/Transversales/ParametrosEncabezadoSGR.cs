using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    [ExcludeFromCodeCoverage]
    public class ParametrosEncabezadoSGR
    {
        public Guid IdInstancia { get; set; }
        public Guid IdFlujo { get; set; }
        public Guid IdNivel { get; set; }
        public string IdProyectoStr { get; set; }
        public int IdProyecto { get; set; }
        public string Tramite { get; set; }
    }
}
