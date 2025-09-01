using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class DisenoFormulario
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("columns")]
        public Columna[][] Columnas { get; set; }
    }
    
    [ExcludeFromCodeCoverage]
    public class Columna
    {
        [JsonProperty("key")]
        public string Id { get; set; }
    }
 }
