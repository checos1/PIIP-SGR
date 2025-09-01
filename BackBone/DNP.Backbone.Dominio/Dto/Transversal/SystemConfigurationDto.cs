using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.Backbone.Dominio.Dto.Transversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class SystemConfigurationDto
    {
        public string VariableKey { get; set; }
        public List<ValoresConfiguracionDto> Valores { get; set; }
    }
    public class ValoresConfiguracionDto
    {
        public string Valor { get; set; }
    }
}

