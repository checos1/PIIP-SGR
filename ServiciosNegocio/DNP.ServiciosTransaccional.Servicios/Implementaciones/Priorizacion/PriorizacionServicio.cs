using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Dto;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Priorizacion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Priorizacion
{
    public class PriorizacionServicio : IPriorizacionServicio
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string MOTOR_FLUJO_ENDPOINT = ConfigurationManager.AppSettings["ApiMotorFlujos"];
        private readonly string SERVICIO_NEGOCIO = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly string SERVICIO_PIIP_CORE = ConfigurationManager.AppSettings["ApiPiipCore"];
        

        public PriorizacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<ResponseDto<bool>> RegistrarInstanciaPriorizacion(string usuarioDNP, ObjetoNegocio objetoNegocio)
        {
            var resultado = new ResponseDto<bool>();

            var crearInstanciaUrl = ConfigurationManager.AppSettings["uriRegistroInstanciaPriorizacion"].ToString();
            //Obtener parametros consumo servicio flujos
            //Tipo Tramite, FlujoId, ObjetoId etc...
            var datosPriorizacion = await ConsultarDatosPriorizacion(objetoNegocio, usuarioDNP);

            try
            {
                //var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                //    MetodosServiciosWeb.Post,
                //    MOTOR_FLUJO_ENDPOINT,
                //    crearInstanciaUrl,
                //    string.Empty,
                //    datosPriorizacion,
                //    usuarioDNP,
                //    useJWTAuth: false
                //);

                var result = new ResponseDto<bool>();
                if (datosPriorizacion.FlujoId != "00000000-0000-0000-0000-000000000000" && datosPriorizacion.FlujoId != null)
                {

                    var crearInstancia = EjecutarFlujoPIIP(objetoNegocio.ObjetoNegocioId, usuarioDNP, datosPriorizacion.FlujoId, "");

                    if (!crearInstancia)
                    {
                        throw new Exception("No fue posible crear la instancia de priorizacioón");
                    }

                    result = new ResponseDto<bool>() { Mensaje = "Instancia de priorización creada" };
                }
                else
                {
                    result = new ResponseDto<bool>() { Mensaje = "Instancia ya creada." };
                }

                //var resultModel = JsonConvert.DeserializeObject<InstanciaPriorizacionDto>(jsonResponse);
                //var result = new ResponseDto<bool>() { Mensaje= "Instancia de priorización creada"};
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return resultado;

        }

        public async Task<InstanciaPriorizacionDto> ConsultarDatosPriorizacion(ObjetoNegocio objetoNegocio, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerRegistroPriorizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, SERVICIO_NEGOCIO, uriMetodo, string.Empty, objetoNegocio, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<InstanciaPriorizacionDto>(respuesta);

        }
        private Boolean EjecutarFlujoPIIP(string projectBpin, string usuario, string flujoId, string AccionPorInstanciaId)
        {
            try
            {
                if (projectBpin == null)
                {
                    return false;
                }

               if (projectBpin.Length == 0)
                {
                    return false;
                }

                //string URL = "https://as-formularios-api-uat.azurewebsites.net/api/Flujo/CrearInstancia";
                var crearInstanciaUrl = ConfigurationManager.AppSettings["uriRegistroInstanciaPriorizacion"].ToString();
                string URL = $"{SERVICIO_PIIP_CORE}/{crearInstanciaUrl}";

               InstanciaFlujoPIIPDto objInstanciaFlujoPIIP = new InstanciaFlujoPIIPDto()
                {
                    FlujoId = flujoId,
                    ObjetoId = projectBpin.ToString(),
                    UsuarioId = usuario,
                    RolId = "1DD225F4-5C34-4C55-B11D-E5856A68839B",
                    TipoObjetoId = "BC154CBA-50A5-4209-81CE-7C0FF0AEC2CE",
                    ListaEntidades = new List<string>() { "0" }
                };
                String jsonString = JsonConvert.SerializeObject(objInstanciaFlujoPIIP);
                var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

               var returnCode = "false";

               using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Basic Q0M4NTQ3NDcwNTou");
                    var response = client.PostAsync(URL,data).Result;

                   if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        JArray jsonArray = JArray.Parse(responseString);
                        dynamic info = JObject.Parse(jsonArray[0].ToString());
                        AccionPorInstanciaId = info.AccionPorInstanciaId;
                        returnCode = "true";
                    }
               }
                
               return returnCode == "true" ? true : false;
            }
            catch (Exception ex)
            {
                throw;
            }
       }
    }
}
