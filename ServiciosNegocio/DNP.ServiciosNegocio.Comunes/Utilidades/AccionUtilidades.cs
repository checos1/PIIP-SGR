using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DNP.ServiciosNegocio.Comunes.Interfaces;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Properties;
using Newtonsoft.Json.Linq;

namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.

    public class AccionUtilidades : IAccionUtilidades
    {

        public bool ExisteInstancia(Guid idInstancia, string idUsuario)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes(idUsuario + ": pwd");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var response = client.GetAsync(ConfigurationManager.AppSettings["apiPiipCore"] + "api/Flujo/ObtenerInstanciaPorId?idInstancia=" + idInstancia);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    var respuesta = JToken.Parse(jsonResponse.Result);
                    if (respuesta.HasValues)
                    {
                        foreach (var token in (JObject)respuesta)
                        {
                            var llave = token.Key;
                            var valor = token.Value;
                            if (llave == "Id" && valor.ToString() != Guid.Empty.ToString())
                                return true;
                        }
                    }

                    throw new AccionException(Resources.ErrorInstanciaFlujo);
                }
            }
            catch (Exception ex)
            {
                throw new AccionException(ex.Message);
            }
        }
        
        public bool ExisteAccionActiva(Guid idInstanciaAccion, string idUsuario)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    bool accionExiste = false;
                    bool accionEstaActiva = false;
                    var byteArray = Encoding.ASCII.GetBytes(idUsuario + ": pwd");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var response = client.GetAsync(ConfigurationManager.AppSettings["apiPiipCore"] + "api/Flujo/ObtenerAccionPorInstanciaIdAccion?idInstanciaAccion=" + idInstanciaAccion);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    var respuesta = JToken.Parse(jsonResponse.Result);
                    if (respuesta.HasValues)
                    {
                        foreach (var token in (JObject)respuesta)
                        {
                            var llave = token.Key;
                            var valor = token.Value;
                            if (llave == "Id" && valor.ToString() != Guid.Empty.ToString())
                                accionExiste = true;
                            if (llave == "EstadoAccionPorInstanciaId" && valor.ToString() != Guid.Empty.ToString())
                            {
                                accionEstaActiva = EstadoAccionPorInstanciaEnum.PorIniciar == (EstadoAccionPorInstanciaEnum)Convert.ToInt32(valor);
                            }
                        }
                        if (!accionExiste)
                            throw new AccionException(Resources.ErrorAccionInexistente);
                        if (!accionEstaActiva)
                            throw new AccionException(Resources.ErrorEstadoInstanciaAccion);

                        return true;
                    }

                    throw new AccionException(Resources.ErrorAccionInexistente);
                }
            }
            catch (Exception ex)
            {
                throw new AccionException(ex.Message);
            }
        }
    }
}
