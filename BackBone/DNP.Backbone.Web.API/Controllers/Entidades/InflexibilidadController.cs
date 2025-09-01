using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using Newtonsoft.Json;
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
    public class InflexibilidadController : Base.BackboneBase
    {

        private readonly IAutorizacionServicios _autorizacionServicios;

        public InflexibilidadController(IAutorizacionServicios autorizacionServicios) : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }

        /// <summary>
        /// Obtener lista de inflexbilidades por entidad
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inflexibilidad/ObtenerInflexibilidadPorEntidadId")]
        public async Task<IHttpActionResult> ObtenerInflexibilidadPorEntidadId([FromUri] Guid idEntidad, [FromBody] InflexibilidadFiltroDto filtro)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerInflexibilidadPorEntidadId(idEntidad, filtro, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        /// <summary>
        /// Guardar inflexibilidad
        /// </summary>
        /// <param name="inflexibilidadDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inflexibilidad/GuardarInflexibilidad")]
        public async Task<IHttpActionResult> GuardarInflexibilidad(InflexibilidadDto inflexibilidadDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarInflexibilidad(inflexibilidadDto, UsuarioLogadoDto.IdUsuario));
                if (respuesta is null)
                    return Ok(new RespuestaGeneralDto { Exito = false, Mensaje = "Error al insertar el registro" });

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Guardar Pagos inflexibilidad
        /// </summary>
        /// <param name="inflexibilidadDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inflexibilidad/GuardarInflexibilidadPagos")]
        public async Task<IHttpActionResult> GuardarInflexibilidadPagos(List<InflexibilidadPagosDto> lista)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarInflexibilidadPagos(lista, UsuarioLogadoDto.IdUsuario));
                if (respuesta is null)
                    return Ok(new RespuestaGeneralDto { Exito = false, Mensaje = "Error al insertar el registro" });

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Actualiza la información del pago de inflexibilidad, colocando el identificador del archivo
        /// </summary>
        /// <param name="pago"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inflexibilidad/ActualizarIdArchivoInflexibilidadPagos")]
        public async Task<IHttpActionResult> ActualizarIdArchivoInflexibilidadPagos(InflexibilidadPagosDto pago)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.ActualizarIdArchivoInflexibilidadPagos(pago, UsuarioLogadoDto.IdUsuario));
                if (respuesta is null)
                    return Ok(new RespuestaGeneralDto { Exito = false, Mensaje = "Error al asociar el archivo" });

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener lista de inflexbilidades por entidad
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Inflexibilidad/ObtenerInflexibilidadPagos")]
        public async Task<IHttpActionResult> ObtenerInflexibilidadPagos(int idInflexibilidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerInflexibilidadPagos(idInflexibilidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        


        /// <summary>
        /// Eliminar inflexibilidad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inflexibilidad/EliminarInflexibilidad/{id}")]
        public async Task<IHttpActionResult> EliminarInflexibilidad([FromUri] int id)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.EliminarInflexibilidad(id, UsuarioLogadoDto.IdUsuario));
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

        /// <summary>
        /// Api para obtención de datos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        /// 

        [Route("api/Inflexibilidad/ObtenerExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcel(List<InflexibilidadDto> dto)
        {
            try
            {

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var _result = new ExcelDto
                {
                    Mensaje = "No hay ningún resultado",
                    Reporte = "Información del entidades",
                    Columnas = dto.FirstOrDefault().Columnas,
                    ColumnasHeader = dto.FirstOrDefault().ColumnasHeader,
                    Data = dto.Select(x => new
                    {
                        x.NombreInflexibilidad,
                        x.PeriodoExcel,
                        x.ValorTotal,
                        x.ValorPagado,
                        x.Estado
                    }).ToList()
                };

                result.Content = ExcelUtilidades.ObtenerExcellInflexibilidad(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

     
    }
}