

namespace DNP.CargaArchivos.Web.API.Controllers.CargaArchivo
{
    using Dominio.Dto.CargaArchivo;
    using Servicios.Interfaces.CargaArchivo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Excepciones;
    using Swashbuckle.Swagger.Annotations;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CargaArchivoController : ApiController
    {

        private readonly ICargaArchivo _cargaArchivoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CargaArchivoController(ICargaArchivo cargaArchivoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _cargaArchivoServicio = cargaArchivoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/CargaArchivo")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Formulario Carga Archivo Dummy", typeof(FormularioCargaArchivoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar()
        {
            var result = await Task.Run(() => _cargaArchivoServicio.ConsultarCargaArchivo());
            if (result != null)
                return Ok(result);

            throw new CargaArchivosException(string.Format(ServiciosNegocioRecursos.RespuestaSinResultados));
        }
    }
}
