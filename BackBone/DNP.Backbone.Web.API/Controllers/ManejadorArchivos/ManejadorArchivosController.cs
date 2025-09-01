using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.ManejadorArchivos;
using DNP.Backbone.Dominio.Dto.Transversales;
using DNP.Backbone.Servicios.Implementaciones.ManejadorArchivos;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.ManejadorArchivos;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.ManejadorArchivos
{
    public class ManejadorArchivosController : Base.BackboneBase
    {
        private readonly IManejadorArchivosServicios _manejadorArchivosServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="priorizacionServicios">Instancia de servicios de priorizacion</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        public ManejadorArchivosController(ManejadorArchivosServicios manejadorArchivosServicios,
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _manejadorArchivosServicios = manejadorArchivosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/manejadorArchivos/ObtenerArchivoInfo")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerArchivoInfo(string Id, string coleccion)
        {
            try
            {
                var result = await Task.Run(() => _manejadorArchivosServicios.ObtenerArchivoInfo(Id, coleccion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/manejadorArchivos/ObtenerListadoArchivos/{coleccion}")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivos(string coleccion, [FromBody] Dictionary<string, object> filtros)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.ObtenerListadoArchivos(coleccion, filtros, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/ValidarArchivosSgr/{coleccion}")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarArchivoSgr(string coleccion, [FromBody] Dictionary<string, object> filtros)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.ValidarArchivoSgr(coleccion, filtros, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/GuardarArchivoRepositorio")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarArchivoRepositorio([FromBody] ArchivoInfoBsonDto parametros)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.GuardarArchivoRepositorio(parametros, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/CambiarEstadoDataArchivo/{coleccion}/{idArchivo}/{status}")]
        [HttpDelete]
        public async Task<IHttpActionResult> CambiarEstadoDataArchivo(string coleccion, string idArchivo, string status)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.CambiarEstadoDataArchivo(coleccion, idArchivo, status, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/ObtenerArchivoBytes")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerArchivoBytes(string coleccion, string IdArchivoBlob)
        {
            try
            {
                var result = await Task.Run(() => _manejadorArchivosServicios.ObtenerArchivoBytes(coleccion, IdArchivoBlob, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/manejadorArchivos/EliminarArchivo/{coleccion}/{idArchivo}/{status}")]
        [HttpDelete]
        public async Task<IHttpActionResult> EliminarArchivo(string coleccion, string idArchivo, string status)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.EliminarArchivo(coleccion, idArchivo, status, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/ActualizarArchivo/{coleccion}/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> ActualizarArchivo(string coleccion, string id, [FromBody] ArchivoInfoViewModelDto archivo)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.ActualizarArchivo(coleccion, id, archivo, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/CargarArchivos")]
        [HttpPost]
        public async Task<IHttpActionResult> CargarArchivos()
        {
            List<Dictionary<string, object>> lstFile = new List<Dictionary<string, object>>();
            //foreach (var item in HttpContext.Current.Request.Files)
            //{
            Dictionary<string, object> file = new Dictionary<string, object>();
            file.Add("FileName", HttpContext.Current.Request.Files[0].FileName);
            file.Add("ContentType", HttpContext.Current.Request.Files[0].ContentType);
            file.Add("ContentLength", HttpContext.Current.Request.Files[0].ContentLength);
            file.Add("InputStream", HttpContext.Current.Request.Files[0].InputStream);
            lstFile.Add(file);
            //}

            Stream fileStream = HttpContext.Current.Request.Files[0].InputStream;

            ArchivoViewModelDto archivo = new ArchivoViewModelDto
            {
                FormFile = lstFile,
                Nombre = HttpContext.Current.Request.Params["Nombre"],
                Coleccion = HttpContext.Current.Request.Params["Coleccion"],
                Status = HttpContext.Current.Request.Params["Status"],
                Metadatos = HttpContext.Current.Request.Params["Metadatos"]
            };
            var formFile = HttpContext.Current.Request.Params["FormFile"];
            var result = await Task.Run(() => _manejadorArchivosServicios.CargarArchivos(archivo, fileStream, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/manejadorArchivos/GetAuthorizationApiArchivos")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAuthorizationApiArchivos([FromBody] UsuariosArchivosLoginDto parametros)
        {
            var result = await Task.Run(() => _manejadorArchivosServicios.GetAuthorizationApiArchivos(parametros, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        /// <summary>
        ///  Obtiene un content result de un archivo binario de un Zip con los archivos
        /// </summary>
        /// <param name="datos">Listado de documentos a descargar <see cref="DocumentosDto"/>.</param>
        /// <returns></returns>
        [Route("api/manejadorArchivos/DescargarDocumentosProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> DescargarDocumentosProyectosAsync(List<DocumentosDto> datos)
        {
            try
            {

                var result = await Task.Run(() => _manejadorArchivosServicios.ObtenerArchivoDocumentos(datos, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }
    }
}