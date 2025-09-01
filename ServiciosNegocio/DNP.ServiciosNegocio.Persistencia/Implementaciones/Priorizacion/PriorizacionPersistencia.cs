using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Priorizacion;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Priorizacion
{
    public class PriorizacionPersistencia : Persistencia, IPriorizacionPersistencia
    {
        #region Constructor

        public PriorizacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        public List<PriorizacionDatosBasicosDto> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            var resultSp = Contexto.uspGetProyectosDatosBasicos(string.Join(",", bpins.BPINs)).ToList();

            var priorizacionDatosBasicos = new List<PriorizacionDatosBasicosDto>();

            priorizacionDatosBasicos = resultSp.Select(prio => new PriorizacionDatosBasicosDto()
            {
                ProyectoId = prio.ProyectoId,
                BPIN = prio.BPIN,
                NombreProyecto = prio.NombreProyecto,
                Recurso = prio.Recurso,
                Fase = prio.Fase,
                ValorProyecto = prio.ValorProyecto,
            }).ToList();

            return priorizacionDatosBasicos;
        }
        public InstanciaPriorizacionDto ObtenerRegistroPriorizacion(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var jsonString = Contexto.uspGetDatosInstanciaPriorizacion(objetoNegocio.ObjetoNegocioId).SingleOrDefault();

                var datosInstancia = JsonConvert.DeserializeObject<InstanciaPriorizacionDto>(jsonString);

                return datosInstancia;
            }
            catch (Exception)
            {
                throw ;
            }

        }

        //public IEnumerable<PriorizacionProyectoDto> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var priorizacionProyecto = Contexto.Database.SqlQuery<PriorizacionProyectoDto>("[Proyectos].[uspGetSGR_Priorizacion_Proyecto] @InstanciaId",
        //        new SqlParameter("@InstanciaId", instanciaId)
        //    ).ToList();

        //    return priorizacionProyecto;
        //}

        //public IEnumerable<PriorizacionProyectoDto> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var aprobacionProyecto = Contexto.Database.SqlQuery<PriorizacionProyectoDto>("[Proyectos].[uspGetSGR_Aprobacion_Proyecto] @InstanciaId",
        //        new SqlParameter("@InstanciaId", instanciaId)
        //    ).ToList();

        //    return aprobacionProyecto;
        //}

        public string ObtenerFuentesSGR(string bpin, Nullable<Guid> instanciaId)
        {
            var listaFuentes = Contexto.uspGetFuentesSGR_JSON(bpin, instanciaId).FirstOrDefault();
            return listaFuentes;
        }

        public void RegistrarViabilidadFuentesSGR(List<EtapaSGRDto> json, string usuario)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(json);

                    var result = Contexto.uspPostFuentesSGRAgregar(jsonModel, usuario, errorValidacionNegocio);
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

        public string ObtenerFuentesNoSGR(string bpin, Nullable<Guid> instanciaId)
        {
            var listaFuentes = Contexto.uspGetFuentesNoSGR_JSON(bpin, instanciaId).FirstOrDefault();
            return listaFuentes;
        }

        public void RegistrarViabilidadFuentesNoSGR(List<EtapaNoSGRDto> json, string usuario)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(json);

                    var result = Contexto.uspPostFuentesNoSGRAgregar(jsonModel, usuario, errorValidacionNegocio);
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

        public string ObtenerResumenFuentesCostos(string bpin, Nullable<Guid> instanciaId)
        {
            var listaFuentes = Contexto.uspGetResumenFuentesCostosSGR_JSON(bpin, instanciaId).FirstOrDefault();
            return listaFuentes;
        }

        public void RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto json, string usuario)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            try
            {
                using (var tx = Contexto.Database.BeginTransaction())
                {
                    var jsonModel = JsonUtilidades.ACadenaJson(json);

                    var result = Contexto.uspPostFuentesNoSGRCofinanciadorAgregar(jsonModel, usuario, errorValidacionNegocio);
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

        public string ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, Nullable<int> vigencia, Nullable<int> vigenciaFuente)
        {
            var listaFuentes = Contexto.uspGetDatosAdicionalesCofinanciadorNoSGR_JSON(bpin, vigencia, vigenciaFuente).FirstOrDefault();
            return listaFuentes;
        }

        public ProyectoPriorizacionDetalleResultado GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            var respuesta = new ProyectoPriorizacionDetalleResultado();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "Resultado",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var jsonConsulta = Contexto.Database.SqlQuery<string>("[Proyectos].[uspPostProyectoPermisosPriorizacionSGR] @BPIN,@InstanciaId, @PriorizacionId, @Usuario, @Resultado output",
                                                       new SqlParameter("BPIN", proyectoPriorizacionDetalleDto.BPIN),
                                                       new SqlParameter("InstanciaId", proyectoPriorizacionDetalleDto.InstanciaId),
                                                       new SqlParameter("PriorizacionId", proyectoPriorizacionDetalleDto.PriorizacionId),
                                                       new SqlParameter("Usuario", usuario),
                                                       outParam
                                                       ).SingleOrDefault();

                    if (outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = jsonConsulta;
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }


                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }
    }
}
