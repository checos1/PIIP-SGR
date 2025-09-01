namespace DNP.Backbone.Servicios.Implementaciones
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using Comunes;
    using Comunes.Enums;
    using DNP.Backbone.Comunes.Utilidades;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using Interfaces;
    using Newtonsoft.Json;

    public class ClienteHttpServicios : IClienteHttpServicios
    {
        private static readonly string _token;

        static ClienteHttpServicios()
        {
            var secret = ConfigurationManager.AppSettings["JwtSecret-Servicio"];

            var claims = new List<Claim>();
            claims.Add(new Claim("Tipo", "servicio"));
            claims.Add(new Claim("Nombre-Servicio", "DNP.Backbone.Web.API"));

            var tokenManager = new JwtTokenManager(secret);
            _token = tokenManager.EscribirToken(claims, DateTime.UtcNow.AddYears(5));
        }

        public async Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, string parametros, object peticion, string usuarioDnp, bool readCustomHttpCodes = false, IPrincipal principal = null, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
         {
            using (var client = new HttpClient())
            {
                if (useBearerToken)
                {
                    GetAuthorizationApiArchivos(client, "Bearer", tokenBearerJWT);
                }
                else
                {
                    if (useJWTAuth)
                    {
                        AsignarAutorizacionJWT(client, usuarioDnp);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Authorization = AsignarAutorizacionBasica(usuarioDnp);
                    }
                }

                //var principal = HttpContext.Current.User;

   
                if (principal != null)
                {
                    var menus = ClaimIdentityUtilidades.ObtenerMenus(principal);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Backbone-Menus", string.Join(",", menus));

                    var entidades = ClaimIdentityUtilidades.ObtenerDictionaryEntidadesOpciones(principal);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Backbone-Entidades", string.Join(",", entidades.Select(x => x.Key)));
                }

                HttpResponseMessage response;
                switch (metodoServicio)
                {
                    case MetodosServiciosWeb.Get:
                        response = client.GetAsync(endPoint + uriMetodo + parametros).Result;
                        break;

                    case MetodosServiciosWeb.GetAsync:
                        {
                            response = client.GetAsync(endPoint + uriMetodo + parametros).Result;
                            return await response.Content.ReadAsStringAsync();
                        }

                    case MetodosServiciosWeb.Post:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PostAsync(endPoint + uriMetodo, content).Result;

                            break;
                        }
                    case MetodosServiciosWeb.PostJson:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            jsondata = WebUtility.UrlEncode(jsondata);
                            response = client.PostAsync(endPoint + uriMetodo +"?json=" + jsondata, null).Result;
                            break;
                        }
                    case MetodosServiciosWeb.PostAsync:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PostAsync(endPoint + uriMetodo, content).Result;
                            return await response.Content.ReadAsStringAsync();
                        }
                    case MetodosServiciosWeb.Put:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PutAsync(endPoint + uriMetodo, content).Result;

                            break;
                        }
                    case MetodosServiciosWeb.Delete:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.DeleteAsync(endPoint + uriMetodo + parametros).Result;
                            return await response.Content.ReadAsStringAsync();
                        }
                    default:
                        response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ReasonPhrase = BackboneRecursos.MetodoServicioNoDisponible
                        };
                        break;
                }

                if (readCustomHttpCodes || response?.IsSuccessStatusCode == true)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }

        public async Task<string> ConsumirServicio(MetodosServiciosWeb metodoServicio, string endPoint, string uriMetodo, object peticion, string usuarioDnp, bool readCustomHttpCodes = false, IPrincipal principal = null, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            using (var client = new HttpClient())
            {
                if (useBearerToken)
                {
                    GetAuthorizationApiArchivos(client, "Bearer", tokenBearerJWT);
                }
                else
                {
                    if (useJWTAuth)
                    {
                        AsignarAutorizacionJWT(client, usuarioDnp);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Authorization = AsignarAutorizacionBasica(usuarioDnp);
                    }
                }

                //var principal = HttpContext.Current.User;

                if (principal != null)
                {
                    var menus = ClaimIdentityUtilidades.ObtenerMenus(principal);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Backbone-Menus", string.Join(",", menus));

                    var entidades = ClaimIdentityUtilidades.ObtenerDictionaryEntidadesOpciones(principal);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Backbone-Entidades", string.Join(",", entidades.Select(x => x.Key)));
                }

                HttpResponseMessage response;
                switch (metodoServicio)
                {
                    case MetodosServiciosWeb.Get:
                        response = client.GetAsync(endPoint + uriMetodo).Result;
                        break;

                    case MetodosServiciosWeb.GetAsync:
                        {
                            response = client.GetAsync(endPoint + uriMetodo).Result;
                            return await response.Content.ReadAsStringAsync();
                        }

                    case MetodosServiciosWeb.Post:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PostAsync(endPoint + uriMetodo, content).Result;

                            break;
                        }
                    case MetodosServiciosWeb.PostAsync:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PostAsync(endPoint + uriMetodo, content).Result;
                            return await response.Content.ReadAsStringAsync();
                        }
                    case MetodosServiciosWeb.Put:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.PutAsync(endPoint + uriMetodo, content).Result;

                            break;
                        }
                    case MetodosServiciosWeb.Delete:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            response = client.DeleteAsync(endPoint + uriMetodo).Result;
                            return await response.Content.ReadAsStringAsync();
                        }
                    default:
                        response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ReasonPhrase = BackboneRecursos.MetodoServicioNoDisponible
                        };
                        break;
                }

                if (readCustomHttpCodes || response?.IsSuccessStatusCode == true)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }

       

        /// <summary> Obtiene HttpResponseMessage para metodos post 
        /// <param name="url">Url del api</param>
        /// <param name="body">Body request del api</param>
        /// /// <param name="usuarioDNP">usuario DNP</param>
        /// /// <param name="useJWTAuth">tipo autorización</param>
        /// <returns>Gets the collection of <see cref="HttpResponseMessage"/></returns>
        public Task<HttpResponseMessage> PostRequestApiMultiContent(string url, MultipartFormDataContent body, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            HttpResponseMessage httpResponseMessage;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                if (useBearerToken)
                {
                    GetAuthorizationApiArchivos(client, "Bearer", tokenBearerJWT);
                }
                else
                {
                    if (useJWTAuth)
                    {
                        AsignarAutorizacionJWT(client, usuarioDNP);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Authorization = AsignarAutorizacionBasica(usuarioDNP);
                    }
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.PostAsync("/Archivos", body).GetAwaiter().GetResult();
            }

            return Task.FromResult(httpResponseMessage);
        }

        /// <summary> Obtiene byte[] para metodo get
        /// <param name="url">Url del api</param>
        /// /// <param name="usuarioDNP">usuario DNP</param>
        /// /// <param name="useJWTAuth">tipo autorización</param>
        /// <returns>Gets the collection of <see cref="byte[]"/></returns>
        public Task<byte[]> RequestApiByteArray(MetodosServiciosWeb metodoServicio, string url, string parametros, object peticion, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "")
        {
            HttpResponseMessage httpResponseMessage = null;

            using (HttpClient client = new HttpClient())
            {
                if (useBearerToken)
                {
                    GetAuthorizationApiArchivos(client, "Bearer", tokenBearerJWT);
                }
                else
                {
                    if (useJWTAuth)
                    {
                        AsignarAutorizacionJWT(client, usuarioDNP);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Authorization = AsignarAutorizacionBasica(usuarioDNP);
                    }
                }

                switch (metodoServicio)
                {
                    case MetodosServiciosWeb.Get:
                            httpResponseMessage = client.GetAsync(url + parametros).Result;
                            return Task.FromResult(ConvertUtilidades.StreamToByteArray(httpResponseMessage.Content.ReadAsStreamAsync().Result));
                    case MetodosServiciosWeb.Post:
                        {
                            var jsondata = JsonConvert.SerializeObject(peticion);
                            var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                            httpResponseMessage = client.PostAsync(url, content).Result;
                            return Task.FromResult(ConvertUtilidades.StreamToByteArray(httpResponseMessage.Content.ReadAsStreamAsync().Result));
                        }
                }                
            }

            return Task.FromResult(ConvertUtilidades.StreamToByteArray(httpResponseMessage.Content.ReadAsStreamAsync().Result));
        }

        private AuthenticationHeaderValue AsignarAutorizacionBasica(string usuarioDnp)
        {
            //TODO: Pendiente. Consultar información del usuario con su contraseña para armar el AuthenticationHeaderValue
            var byteArray = Encoding.ASCII.GetBytes(usuarioDnp + ": password1234");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private void AsignarAutorizacionJWT(HttpClient client, string usuarioDnp)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization-Type", "JWT");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Application-User", usuarioDnp);
        }

        private void GetAuthorizationApiArchivos(HttpClient client, string typeAuth, string tokenAuth)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(typeAuth, tokenAuth);
        }
    }

}
