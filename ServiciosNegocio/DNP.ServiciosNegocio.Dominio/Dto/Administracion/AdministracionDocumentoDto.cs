using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Administracion
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics.CodeAnalysis;


    [ExcludeFromCodeCoverage]
    public class AdministracionDocumentoDto
    {
        public string Id { get; set; }
        public string NombreDocumento { get; set; }
        public string Codigo { get; set; }
        public Boolean? Activo { get; set; }
        public string ModificadoPor { get; set; }
    }
}
