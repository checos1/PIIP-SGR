using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Acciones;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Acciones
{
    using System.Configuration;
    using Comunes.Autorizacion;

    public class EjecutarAccionFormularioController : ApiController
    {
        private readonly IEjecucionAccionTransaccionalServicios _accionTransaccionalServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public EjecutarAccionFormularioController(IEjecucionAccionTransaccionalServicios accionTransaccionalServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _accionTransaccionalServicios = accionTransaccionalServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        // TODO: Verificar el cambio de nombre de método y routing de acuerdo a correo de Jimmy
        [Route("api/Accion/EjecutarAccion")]
        [HttpPost]
        public async Task<IHttpActionResult> EjecutarAccion(AccionFormularioDto accion)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ejecutarAccion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            string usuario = RequestContext.Principal.Identity.Name;
            _accionTransaccionalServicios.Usuario = usuario;
            var result = await Task.Run(() => _accionTransaccionalServicios.EjecutarAccion(accion));
            return Ok(result);
        }
    }
}
