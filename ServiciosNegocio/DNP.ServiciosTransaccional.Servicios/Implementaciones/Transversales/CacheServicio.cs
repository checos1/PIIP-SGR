using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Transversales
{
    public class CacheServicio:ICacheServicio
    {
        public async Task<ProyectoDto> ObtenerProyecto(string bpin)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyecto"];
            var usuario = ConfigurationManager.AppSettings["UsuarioGenericoServiciosNegocio"];
            var parametros = $"?bpin={bpin}";
            
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes(usuario + ": password1234");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = client.GetAsync(endPoint + uriMetodo + parametros).Result;

                var result = await response.Content.ReadAsStringAsync();

                if (result?.Contains(ServiciosNegocioRecursos.SinResultados) != true) return null;

                return JsonConvert.DeserializeObject<ProyectoDto>(result);
            }
        }
    }
}
