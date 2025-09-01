using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Entidades
{
    public class EntidadController : Base.BackboneBase
    {

        private readonly IAutorizacionServicios _autorizacionServicios;

        public EntidadController(IAutorizacionServicios autorizacionServicios) : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }


        [HttpGet]
        [Route("api/Entidad/Obtener")]
        public async Task<IHttpActionResult> ObtenerEntidades()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadesNegocio(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerSectores")]
        public async Task<IHttpActionResult> ObtenerSectores()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerSectoresNegocio(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerDepartamentos")]
        public async Task<IHttpActionResult> ObtenerDepartamentos()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerDepartamentos(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }        

        [HttpGet]
        [Route("api/Entidad/ObtenerEntidadesPorTipoEntidad")]
        public async Task<IHttpActionResult> ObtenerEntidadesPorTipoEntidad(string tipoEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadesPorTipoEntidad(tipoEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }
        
        [HttpGet]
        [Route("api/Entidad/ObtenerListadoEntidadesXUsuarioAutenticado")]
        public async Task<IHttpActionResult> ObtenerListadoEntidadesXUsuarioAutenticado()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoEntidadesXUsuarioAutenticado(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado")]
        public async Task<IHttpActionResult> ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(string tipoEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(tipoEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerListadoPerfilesXEntidadBanco")]
        public async Task<IHttpActionResult> ObtenerListadoPerfilesXEntidad(string idEntidad, int resourceGroupId)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoPerfilesXEntidadBanco(idEntidad, UsuarioLogadoDto.IdUsuario, resourceGroupId));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerListadoPerfilesXEntidad")]
        public async Task<IHttpActionResult> ObtenerListadoPerfilesXEntidad(string idEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoPerfilesXEntidad(idEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerEntidadesPorUnidadesResponsables")]
        public async Task<IHttpActionResult> ObtenerEntidadesPorUnidadesResponsables()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadesPorUnidadesResponsables(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerSectoresParaEntidades")]
        public async Task<IHttpActionResult> ObtenerSectoresParaEntidades()
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerSectoresParaEntidades(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerEntidadPorEntidadId")]
        public async Task<IHttpActionResult> ObtenerEntidadPorEntidadId([FromUri] Guid idEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadPorEntidadId(idEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }


        [HttpGet]
        [Route("api/Entidad/ObtenerSubEntidadesPorEntidadId")]
        public async Task<IHttpActionResult> ObtenerSubEntidadesPorEntidadId(Guid idEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerSubEntidadesPorEntidadId(idEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        //
        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        /// 

        [Route("api/Entidad/ObtenerExcelEntidades")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcelEntidades(List<EntidadFiltroDto> entidadesFiltro)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var _result = new ExcelDto
                {
                    Mensaje = "No hay ningún resultado",
                    Reporte = "Información del entidades",
                    Columnas = new List<string> { "Entidades", "Tipo Entidad" },
                    Data = entidadesFiltro.Select(x => new { NombreCompleto = x.NombreCompleto + (x.CabezaSector ? " - Cabeza Sector" : string.Empty), x.TipoEntidad }).ToList()
                };
                result.Content = ExcelUtilidades.ObtenerExcellComum(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Entidad/GuardarEntidad")]
        public async Task<IHttpActionResult> GuardarEntidad(EntidadNegocioDto entidadDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarEntidad(entidadDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Entidad/ActualizarUnidadResponsable")]
        public async Task<IHttpActionResult> ActualizarUnidadResponsable(EntidadNegocioDto entidadDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.ActualizarUnidadResponsable(entidadDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerCRType")]
        public async Task<IHttpActionResult> ObtenerCRType()
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerCRType(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Entidad/ObtenerFase")]
        public async Task<IHttpActionResult> ObtenerFase()
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerFase(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpGet]
        [Route("api/Entidad/ObtenerMatrizFlujo")]
        public async Task<IHttpActionResult> ObtenerMatrizFlujo(int entidadResponsableId)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerMatrizFlujo(entidadResponsableId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpPost]
        [Route("api/Entidad/EliminarEntidad/{idEntidad}")]
        public async Task<IHttpActionResult> EliminarEntidad([FromUri] Guid idEntidad)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.EliminarEntidad(idEntidad, UsuarioLogadoDto.IdUsuario));
                if (!respuesta.Exito)
                {
                    return Content(HttpStatusCode.Conflict, respuesta);
                }

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Entidad/MantenimientoMatrizFlujo")]
        public async Task<IHttpActionResult> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.MantenimientoMatrizFlujo(flujos, UsuarioLogadoDto.IdUsuario));
                if (!respuesta.Exito)
                {
                    return Content(HttpStatusCode.Conflict, respuesta);
                }

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpPost]
        [Route("api/Entidad/ObtenerEntidadesConRoleVisualizador")]
        public async Task<IHttpActionResult> ObtenerEntidadesConRoleVisualizador(string usuarioDNP)
        {
            try
            {
                List<EntidadNegocioDto> entidades = new List<EntidadNegocioDto>();
                if (!string.IsNullOrEmpty(usuarioDNP))
                {
                    var result = await _autorizacionServicios.ObtenerEntidadesConRoleVisualizador(usuarioDNP);
                    result.ForEach(x => { 
                        if(!entidades.Exists(z=> z.Id == x.Id))
                            entidades.Add(x); });
                }
                return Ok(entidades);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}