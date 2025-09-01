using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DNP.Backbone.Comunes.Utilidades.AutorizacionAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class EntidadEOpcionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _opcion;

        public EntidadEOpcionAuthorizeAttribute(string opcion)
        {
            _opcion = opcion;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);
            if (!authorized)
            {
                return false;
            }

            var entidades = ClaimIdentityUtilidades.ObtenerDictionaryEntidadesOpciones(actionContext.RequestContext.Principal);

            var requestIdEntidad = actionContext.ActionArguments["IdEntidad"];
            if (requestIdEntidad == null)
            {
                throw new Exception("No fue posible localizar la entidad");
            }

            var entidad = entidades[requestIdEntidad.ToString()];
            
            return entidad.Any(x => x == _opcion);
        }
    }
}
