using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SeguimientoControl
{
    public class ProgramarActividadesPersistencia : Persistencia, IProgramarActividadesPersistencia
    {
        #region Constructor

        /// <summary>
        /// Constructor de DesagregarEdtPersistencia
        /// </summary>
        /// <param name=\\\\"contextoFactory\\\\"></param>
        public ProgramarActividadesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
           
        }

        #endregion

        #region Get
        public string ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
        {
            var resultSp = Contexto.uspGetProgramarActividades_JSON(ProyectosDto.BPIN).SingleOrDefault();
            return resultSp;
            //return "{\"ProyectoId\":97649,\"BPIN\":\"202100000000007\",\"Objetivos\":[{\"ObjetivoEspecificoId\":699,\"ObjetivoEspecifico\":\"Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL\",\"Productos\":[{\"ProductoId\":1253,\"NombreProducto\":\"1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -1.1.Infraestructura penitenciaria y carcelaria construida -\",\"IndicadorPrincipal\":\"Cupos penitenciarios y carcelarios entregados(nacionales y territoriales) \",\"IndicadorId\":1352,\"UnidadMedidaProducto\":\"Número de cupos\",\"Cantidad\":11284,\"EsAcumulativo\":null,\"EntregablesNivel1\":[{\"ActividadId\":4040,\"DeliverableCatalogId\":8,\"NombreEntregable\":\"Caso 1: Nivel 1 con solo Actividades pero sin registros de actividades -Obra civil 1.1.Infraestructura penitenciaria y carcelaria construida\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":null},{\"ActividadId\":4041,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 pero sin registros en Nivel 2 - Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4041,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":null},{\"ActividadId\":4042,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con dos registros en Nivel 2 sin Actividades -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":6,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"tercer entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"parentId\":4042,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":4,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":5,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial, prueba de VER MAS en nivel 2\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":6,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":7,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 2\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":8,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 3\",\"PadreId\":4042,\"TipoEntregable\":\"Entregable\"}]},{\"ActividadId\":4043,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con Nivel 3 con dos registros en Nivel 2 y Solo en uno nivel 3 sin actividades -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4043,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 3\",\"NombreEntregable\":\"Nivel 3 - Hijo de Caracterización Tramo Vial, prueba de VER MAS en nivel 3\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4043,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":4043,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"}]},{\"ActividadId\":4044,\"DeliverableCatalogId\":7,\"NombreEntregable\":\"Caso 2: Nivel 1 con Nivel 2 con Nivel 3 con dos registros en Nivel 2 y Solo en uno nivel 3 sin actividades -Infraestructura en obra blanca(acabados)\",\"Deliverable\":true,\"Costo\":0,\"CatalogoEntregables\":[{\"DeliverableCatalogId\":4,\"Nivel\":\"Nivel 2\",\"NombreEntregable\":\"Caracterización Tramo Vial\",\"parentId\":4044,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253},{\"DeliverableCatalogId\":5,\"Nivel\":\"Nivel 3\",\"NombreEntregable\":\"Nivel 3 - Hijo de Caracterización Tramo Vial, prueba de VER MAS en nivel 3\",\"parentId\":4,\"EntregableIdPrimerNivel\":9,\"ProductoId\":1253}],\"SeguimientoEntregables\":[{\"IdEntregable\":4,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Caracterización Tramo Vial\",\"PadreId\":4044,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":5,\"Nivel\":\"Nivel 2\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":4044,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":6,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Segundo entregable de Nivel 2 - Caracterización Tramo Vial\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":7,\"Nivel\":\"\",\"DeliverableCatalogId\":2,\"NombreEntregable\":\"Actividad Nivel 3 - Caracterización Tramo Vial\",\"PadreId\":6,\"TipoEntregable\":\"Actividad\"},{\"IdEntregable\":12,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":10,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 2\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"},{\"IdEntregable\":11,\"Nivel\":\"Nivel 3\",\"DeliverableCatalogId\":null,\"NombreEntregable\":\"ENTREGABLE CON NOMBRE PERSONALIZADO 3\",\"PadreId\":5,\"TipoEntregable\":\"Entregable\"}]}]}]}]}";
        }

        public string ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto)
        {
            //var resultSp = Contexto.uspGetReporteActividades_JSON(ProyectosDto.BPIN).SingleOrDefault();
            var resultSp = Contexto.Database.SqlQuery<string>("Productos.uspGetReporteActividades_JSON @BPIN ",
                                 new SqlParameter("BPIN", ProyectosDto.BPIN)
                                 ).SingleOrDefault();
            return resultSp;
        }

        public string ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto)
        {
            var resultSp = ContextoOnlySP.uspGetIndicadoresPoliticas_JSON(ProyectosDto.BPIN).SingleOrDefault();
            return resultSp;
        }

        public List<CalendarioPeriodoDto> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto)
        {
            List<CalendarioPeriodoDto> lista = new List<CalendarioPeriodoDto>();
            var resultSp = ContextoOnlySP.uspGetCalendarioPeriodo(ProyectosDto.BPIN).ToList();
            foreach(var item in resultSp)
            {
                CalendarioPeriodoDto data = new CalendarioPeriodoDto();
                data.CalendarioPeriodoId = item.CalendarioPeriodoId;
                data.FaseId = item.FaseId;
                data.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                data.FechaHasta = item.FechaHasta;
                data.FechaDesde = item.FechaDesde;
                data.Mes = item.Mes;
                data.Vigencia = item.Vigencia;
                lista.Add(data);
            }
            return lista;
        }

        public string ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta)
        {
            var resultSp = ContextoOnlySP.uspGetFocalizacionProgramacionSeguimiento_JSON(parametroConsulta).SingleOrDefault();
            return resultSp;
        }

        public string ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid)
        {
            var resultSp = ContextoOnlySP.uspGetCrucePoliticasSeguimiento_JSON(instanciaid, proyectoid).SingleOrDefault();
            return resultSp;
        }

        public string ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros)
        {
            var resultSp = ContextoOnlySP.uspGetDetalleFocalizacionProgramacionSeguimiento_JSON(parametros).SingleOrDefault();
            return resultSp;
        }


        #endregion

        #region Post       
        public void EditarProgramarActividad(string usuario, ProgramarActividadesDto ProyectosDto)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(ProyectosDto);

                    var result = Contexto.UspPostEditarProgramarActividades(usuario, jsonModel, errorValidacionNegocio);
                    if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
                    {
                        tx.Rollback();
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                    tx.Commit();
                }
            }
            catch (Exception e)
            {
                string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw new ServiciosNegocioException(erorr);
            }
        }

        public void ActividadProgramacionSeguimientoPeriodosValores(string usuario, List<VigenciaEntregableDto> parametros)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            
            try
            {
                List<ProgramacionSeguimientoPeriodosValoresDto> data = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                for(var i = 0; i < parametros.Count; i++)
                {
                    for(var j=0; j < parametros[i].ProgramacionSeguimientoPeriodosValores.Count; j++)
                    {
                        if (parametros[i].ProgramacionSeguimientoPeriodosValores[j].ActividadProgramacionSeguimientoPeriodosId == null)
                        {
                            parametros[i].ProgramacionSeguimientoPeriodosValores[j].PeriodoProyectoId = parametros[i].PeriodoProyectoId;
                        }
                        data.Add(parametros[i].ProgramacionSeguimientoPeriodosValores[j]);
                    }
                }
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(data);

                    var result = Contexto.UspPostEditarActividadProgramacionSeguimientoPeriodosValores(usuario, jsonModel, errorValidacionNegocio);
                    if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
                    {
                        tx.Rollback();
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                    tx.Commit();
                }
            }
            catch (Exception e)
            {
                string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw new ServiciosNegocioException(erorr);
            }
        }

        public void ActividadReporteSeguimientoPeriodosValores(string usuario, ReporteSeguimiento parametros)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                if (parametros.Igual == "1")
                {
                    var listPresupuesto = parametros.CostoPeriodo.Where(x => x.TipoCosto == "Presupuestal").ToList();
                    foreach(var item in listPresupuesto)
                    {
                        foreach(var editar in parametros.CostoPeriodo)
                        {
                            if(editar.Vigencia == item.Vigencia && editar.Mes == item.Mes && editar.TipoCosto == "Flujo de caja")
                            {
                                editar.CostoEjecutadoMes = item.CostoEjecutadoMes;
                                editar.Observaciones = item.Observaciones;
                            }
                        }
                    }
                }

                var jsonCantidadesModel = JsonUtilidades.ACadenaJson(parametros.AvanceCantidades);
                var jsonCostosModel = JsonUtilidades.ACadenaJson(parametros.CostoPeriodo);

                var outParam = new SqlParameter
                {
                    ParameterName = "errorValidacionNegocio",
                    IsNullable = true,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = 8000
                };

                int actividadSeguimientoIdParameter = parametros.ActividadSeguimientoId == null ? 0 : parametros.ActividadSeguimientoId.Value;

                var resultado = Contexto.Database.ExecuteSqlCommand("Productos.UspPostEditarActividadReporteSeguimientoPeriodosValores @usuario,@proyectoId,@jsonActividades,@jsonCosto,@actividadId,@actividadSeguimientoId,@errorValidacionNegocio output ",
                                        new SqlParameter("usuario", usuario),
                                        new SqlParameter("proyectoId", parametros.ProyectoId),
                                        new SqlParameter("jsonActividades", jsonCantidadesModel),
                                        new SqlParameter("jsonCosto", jsonCostosModel),
                                        new SqlParameter("actividadId", parametros.ActividadId),
                                        new SqlParameter("actividadSeguimientoId", actividadSeguimientoIdParameter),
                                        outParam
                                        );

                var resultadoMsg = outParam.Value == DBNull.Value ? string.Empty : outParam.Value.ToString();
                if (!string.IsNullOrEmpty(resultadoMsg))
                {
                    var mensajeError = Convert.ToString(resultadoMsg);
                    throw new ServiciosNegocioException(mensajeError);
                }

                //var result = ContextoOnlySP.UspPostEditarActividadReporteSeguimientoPeriodosValores(usuario, parametros.ProyectoId, jsonCantidadesModel, jsonCostosModel, parametros.ActividadId, parametros.ActividadSeguimientoId, errorValidacionNegocio);
                //if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
                //{
                //    //tx.Rollback();
                //    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                //    throw new ServiciosNegocioException(mensajeError);
                //}
            }
            catch (Exception e)
            {
                string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw new ServiciosNegocioException(erorr);
            }
        }

        public void IndicadorPoliticaSeguimientoPeriodosValores(string usuario, ReporteIndicadorPoliticas parametros)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                var jsonModel = JsonUtilidades.ACadenaJson(parametros.ReporteIndicadores);

                var result = ContextoOnlySP.UspPostEditarIndicadorPoliticasSeguimientoPeriodosValores(usuario, parametros.ProyectoId, parametros.PoliticaId, parametros.FuenteId,
                    parametros.DimensionId, parametros.LocalizacionId, jsonModel, errorValidacionNegocio);
                if (!string.IsNullOrEmpty(Convert.ToString(errorValidacionNegocio.Value)))
                {
                    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                    throw new ServiciosNegocioException(mensajeError);
                }
            }
            catch (Exception e)
            {
                string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                throw new ServiciosNegocioException(erorr);
            }
        }

        public string GuardarFocalizacionProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionProgramacionSeguimientoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostProductoCategoriaSeguimiento(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string GuardarCruceProgramacionSeguimiento(ParametrosGuardarDto<FocalizacionCrucePoliticaSeguimientoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostCrucePoliticasSeguimiento(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
        #endregion

        #region Delete

        #endregion
    }
}

