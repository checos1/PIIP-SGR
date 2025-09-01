using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.Swagger;

namespace DNP.ServiciosNegocio.Web.API
{
    using System.Web.Http.Description;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public class SwaggerBasicAuth : IOperationFilter
    {
        public string Name { get; private set; }

        public SwaggerBasicAuth()
        {
            Name = "basic";
        }

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var basicAuthDict = new Dictionary<string, IEnumerable<string>> { { Name, new List<string>() } };
            operation.security = new IDictionary<string, IEnumerable<string>>[] { basicAuthDict };
        }
    }
}