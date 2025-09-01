namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos
{

    using Interfaces;
    using Interfaces.Proyectos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Modelo;
    using System.Text;
    using System.Threading.Tasks;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System.Data.Entity.Core.Objects;
    using Newtonsoft.Json.Linq;
    using Comunes.Utilidades;
    using System.Net;
    using System.IO;
    using System.Web.Configuration;
    using Newtonsoft.Json;

    public class DevolverProyectoPersistencia : Persistencia, IDevolverProyectoPersistencia
    {

        public DevolverProyectoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public DevolverProyectoDto ObtenerDevolverProyecto(string bpin)
        {
            try
            {
                var devolverProyectoDto = new DevolverProyectoDto();
                IEnumerable<uspGetDevolverProyectoMga_Result> devolverProyectoList = Contexto.uspGetDevolverProyectoMga(bpin).ToList();

                devolverProyectoDto = MapearDevolverProyectoDto(devolverProyectoList.ToList());

                return devolverProyectoDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(e.Message);
            }
        }

        private DevolverProyectoDto MapearDevolverProyectoDto(List<uspGetDevolverProyectoMga_Result> listadoDesdeBd)
        {
            var devolverProyecto = new DevolverProyectoDto();

            listadoDesdeBd.GroupBy(o =>
            new
            {
                o.BPIN,
                o.ProyectoId,
                o.Observacion,
                o.DevolverId,
                o.EstadoDevolver
            }).ToList().ForEach(
                ob =>
                {
                    devolverProyecto.Bpin = ob.Key.BPIN;
                    devolverProyecto.ProyectoId = ob.Key.ProyectoId;
                    devolverProyecto.Observacion = ob.Key.Observacion;
                    devolverProyecto.DevolverId = ob.Key.DevolverId;
                    devolverProyecto.EstadoDevolver = ob.Key.EstadoDevolver;
                });

            return devolverProyecto;
        }


        public void GuardarDefinitivamente(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    JObject json = JObject.Parse(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido));
                    Contexto.uspPostDevolverProyecto(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        ActualizarEstadoProyecto(parametrosGuardar, json, usuario);

                        Contexto.InsertMissingProjectComment(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, errorValidacionNegocio);

                        if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                        {
                            Contexto.SaveChanges();
                            dbContextTransaction.Commit();
                            return;
                        }
                        else
                        {
                            var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                            throw new ServiciosNegocioException(mensajeError);
                        }
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private Boolean ActualizarEstadoProyecto(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, JObject json, string usuario)
        {
            try
            {
                string domain = System.Configuration.ConfigurationManager.AppSettings["DominioMgaWeb"];
                string URL = domain + "api/ProjectExternalService/UpdateProjectStatusAndComment";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Basic QWRtb25QSUlQOjE4MzEwMTc3");


                CambioEstadoDto objCambioEstado = new CambioEstadoDto()
                {
                    ProjectId = parametrosGuardar.Contenido.ProyectoId.ToString(),
                    Comment = parametrosGuardar.Contenido.Observacion.ToString(),
                    ProjectStatus = parametrosGuardar.Contenido.EstadoDevolver.ToString(),
                    BPIN = parametrosGuardar.Contenido.Bpin.ToString(),
                    CreatedBy = usuario,
                    CreatedByRole = "Formulador Oficial"
                };

                String jsonString = JsonConvert.SerializeObject(objCambioEstado);

                var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

                streamWriter.Write(jsonString);
                streamWriter.Flush();
                streamWriter.Close();


                var returnCode = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    returnCode = result.ToString().ToUpper();
                }

                return returnCode == "TRUE" ? true : false;
            }
            catch (Exception exp)
            {
                throw new ServiciosNegocioException(exp.Message);
                //return false;
            }

        }
    }
}
