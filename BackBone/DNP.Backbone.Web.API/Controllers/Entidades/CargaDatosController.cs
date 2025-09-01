using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Office.Excel;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.OleDb;
using System.Data;
using System.Reflection;
using ExcelDataReader;
using DNP.Backbone.Comunes.Utilidades.AutorizacionAttributes;

namespace DNP.Backbone.Web.API.Controllers.Entidades
{
    public class CargaDatosController : Base.BackboneBase
    {

        private readonly IAutorizacionServicios _autorizacionServicios;

        public CargaDatosController(IAutorizacionServicios autorizacionServicios) : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }


        [HttpPost]
        [Route("api/CargarDatos/GuardarDatos")]
        public async Task<IHttpActionResult> GuardarDatos()
        {
            try
            {
                var dynamicDt = new List<dynamic>();
                IExcelDataReader excelReader;
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                var jsonContent = await provider.Contents[1].ReadAsStringAsync();
                var carga = JsonConvert.DeserializeObject<CargaDatosDto>(jsonContent);

                // extraer el nombre del archivo y el contenido del archivo
                Stream stream = new MemoryStream(await provider.Contents[0].ReadAsByteArrayAsync());

                //get fileName
                var filename = provider.Contents[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty);

                // obtener fileName 
                if (filename.EndsWith(".xls"))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (filename.EndsWith(".xlsx"))
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                var dataSet = excelReader.AsDataSet(conf);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    dynamic dyn = new System.Dynamic.ExpandoObject();
                    dynamicDt.Add(dyn);
                    foreach (DataColumn column in dataSet.Tables[0].Columns)
                    {
                        var dic = (IDictionary<string, object>)dyn;
                        column.ColumnName = column.ColumnName.Replace('.', ' ');
                        dic[column.ColumnName] = row[column];
                    }
                }

                #region Guarda en lo SQL y no MongoDB
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarDatos(carga, UsuarioLogadoDto.IdUsuario));
                if (respuesta.Exito)
                {
                    var respuestaMB = _autorizacionServicios.GuardarDatosMongoDB(dynamicDt, respuesta.IdRegistro);
                    if (respuestaMB.Exito)
                        return Ok(respuesta);
                    else
                        return Ok(respuestaMB);
                }
                #endregion

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpPost]
        [Route("api/CargarDatos/EliminarCargaDatos/{id}")]
        public async Task<IHttpActionResult> EliminarCargaDatos([FromUri] int id)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.EliminarCargaDatos(id, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }


        }

        [HttpGet]
        [OpcionAuthorize("EntidadesCargarDatos")]        
        [Route("api/CargaDatos/ObtenerCargaDatosPorTipoYTipoEntidad")]
        public async Task<IHttpActionResult> ObtenerCargaDatosPorTipoYTipoEntidad(string tipoEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerCargaDatosPorTipoYTipoEntidad(tipoEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }


        [HttpGet]
        [Route("api/CargaDatos/ObtenerDatosMongoDb")]
        public async Task<IHttpActionResult> ObtenerDatosMongoDb(string id)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerDatosMongoDb(id));
            //var teste = result["InfoArchivo"].AsBsonArray.ToArray()


            return Ok(result);
        }

    }
}