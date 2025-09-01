using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    [ExcludeFromCodeCoverage]
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                List<ErrorFormato> listaErrores = new List<ErrorFormato>();
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        string er = string.Format("{0}", key.ToString().Substring(key.ToString().IndexOf('.') + 1));
                        if (!string.IsNullOrEmpty(er))
                            listaErrores.Add(new ErrorFormato { Error = er });
                    }
                }

                if (listaErrores.Any())
                {
                    var errorFormatoInvalido = new ErrorValidacionFormatoInvalido
                    {
                        Mensaje = "No fue posible guardar, existen campos con formato inválido",
                        ListaErrores = listaErrores
                    };

                    throw new HttpResponseException(
                        actionContext.Request.CreateErrorResponse(
                            HttpStatusCode.BadRequest, 
                            new Exception(JsonUtilidades.ACadenaJson(errorFormatoInvalido))));
                }
            }
        }
    }

    internal class ErrorFormato
    {
        public string Error { get; internal set; }
    }

    internal class ErrorValidacionFormatoInvalido
    {
        public string Mensaje { get; internal set; }
        public List<ErrorFormato> ListaErrores { get; internal set; }
    }
}
