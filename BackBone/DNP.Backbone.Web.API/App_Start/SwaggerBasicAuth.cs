using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Web.API
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Swashbuckle.Swagger;

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
            if (operation == null)
                return;

            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            var parameter = new Parameter
            {
                description = "The authorization token",
                @in = "header",
                name = "Authorization",
                required = true,
                type = "string"
            };

            if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                parameter.required = false;

            operation.parameters.Add(parameter);
        }
    }
}