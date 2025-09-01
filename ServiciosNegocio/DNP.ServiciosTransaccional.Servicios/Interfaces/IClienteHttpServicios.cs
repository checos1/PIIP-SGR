using DNP.ServiciosNegocio.Comunes.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces
{
    public interface IClienteHttpServicios
    {
        Task<string> ConsumirServicio(
            MetodosServiciosWeb metodoServicio,
            string endPoint,
            string uriMetodo,
            string parametros,
            object peticion,
            string usuarioDnp,
            bool readCustomHttpCodes = false,
            bool useJWTAuth = false
            , bool useBearerToken = false, 
            string tokenBearerJWT = ""
        );

        Task<HttpResponseMessage> PostRequestApiMultiContent(string url, MultipartFormDataContent body, string usuarioDNP, bool useJWTAuth = false, bool useBearerToken = false, string tokenBearerJWT = "");
    }
}
