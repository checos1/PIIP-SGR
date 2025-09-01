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
    public sealed class OpcionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _opcion;


        public OpcionAuthorizeAttribute(string opciones)
        {
            _opcion = opciones.Split('|');
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);
            if (!authorized)
            {
                return false;
            }

            var entidades = ClaimIdentityUtilidades.ObtenerDictionaryEntidadesOpciones(actionContext.RequestContext.Principal);
            var permissiones = new List<string>();

            for (int i = 0; i < _opcion.Length; i++)
            {
                if (entidades.Values.Any(x => x.Any(y => y == _opcion[i])))
                    permissiones.Add(_opcion[i]);
            }

            if (permissiones.Count() == _opcion.Length)
                return true;
            else
                return false;

        }
    }
}
