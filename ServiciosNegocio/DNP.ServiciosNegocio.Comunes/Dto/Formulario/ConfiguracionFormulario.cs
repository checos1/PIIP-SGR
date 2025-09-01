using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class ConfiguracionFormulario
    {
        public Guid Id { get; set; }
        public string Disenador { get; set; }
    }
}
