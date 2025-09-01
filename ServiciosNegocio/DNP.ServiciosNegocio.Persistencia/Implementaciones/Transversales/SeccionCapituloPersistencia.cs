namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using Interfaces;
    using Interfaces.Proyectos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity.Core.Objects;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using System.Data.SqlClient;

    public class SeccionCapituloPersistencia : Persistencia, ISeccionCapituloPersistencia
    {
        private readonly IFasePersistencia _fasePersistencia;

        #region Constructor
        /// <summary>
        /// Constructor de SeccionCapituloPersistencia
        /// </summary>
        /// <param name="contextoFactory"></param>
        public SeccionCapituloPersistencia(IContextoFactory contextoFactory, IFasePersistencia fasePersistencia) : base(contextoFactory)
        {            
            _fasePersistencia = fasePersistencia;
        }

        #endregion

        /// <summary>
        /// Método que obtiene el listado de secciones y cápitulos consultando GUID de la tabla [Transversal].[fase]
        /// </summary>
        /// <param name="idMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        public List<SeccionCapituloDto> ObtenerListaCapitulosModificadosByMacroproceso(int idMacroproceso, int IdProyecto, Guid IdInstancia)
        {
            var resultSp = Contexto.UspGetCapitulos(idMacroproceso, IdProyecto, IdInstancia).ToList();

            var capitulos = new List<SeccionCapituloDto>();

            capitulos = resultSp.Select(est => new SeccionCapituloDto()
            {
                SeccionCapituloId = est.SeccionCapituloId.Value,
                SeccionId = est.SeccionId,
                CapituloId = est.CapituloId,
                Macroproceso = est.Macroproceso,
                Instancia = est.Instancia,
                Seccion = est.Seccion,
                Capitulo = est.Capitulo,
                Modificado = est.Modificado,
                Justificacion = est.Justificacion
            }).ToList();

            return capitulos;
        }



        /// <summary>
        /// Método que obtiene el listado de secciones y cápitulos consultando GUID de la tabla [Transversal].[fase]
        /// </summary>
        /// <param name="idMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        public List<SeccionCapituloDto> ObtenerListaCapitulosByMacroproceso(int idMacroproceso,Guid NivelId, Guid FlujoId)
        {
            var capitulos = new List<SeccionCapituloDto>();
            var resultSp = Contexto.UspGetCapitulosByMacroprocesoNivel(idMacroproceso, NivelId, FlujoId).ToList();      

            capitulos = resultSp.Select(est => new SeccionCapituloDto()
            {
                SeccionCapituloId = est.SeccionCapituloId,
                SeccionId = est.SeccionId,
                CapituloId = est.CapituloId,
                Macroproceso = est.Macroproceso,
                Seccion = est.Seccion,
                SeccionModificado = est.SeccionModificado,
                Capitulo = est.Capitulo,
                CapituloModificado = est.CapituloModificado,
                nombreComponente = est.nombreComponente
            }).ToList();

            return capitulos;
        }

        public CapituloModificado ObtenerSeccionCapitulo(string GuiMacroproceso, string nombreCapitulo, string nombreSeccion)
        {
            int capitulo = 2;
            int seccion = 1;
            var capituloEntidad = Contexto.Capitulo.FirstOrDefault(x => x.Nombre == nombreCapitulo);
            if (capituloEntidad != null)
                capitulo = capituloEntidad.Id;

            var seccionEntidad = Contexto.Seccion.FirstOrDefault(x => x.Nombre == nombreSeccion);
            if (seccionEntidad != null)
                seccion = seccionEntidad.Id;

            var faseId = _fasePersistencia.ObtenerFaseByGuid(GuiMacroproceso);
            var macroprocesoSeccion = Contexto.MacroprocesoSeccion.FirstOrDefault(p => p.SeccionId == seccion && p.FaseId == faseId.Id);
            var seccionCapitulo = Contexto.SeccionCapitulos.FirstOrDefault(p => p.CapituloId == capitulo && p.MacroprocesoSeccionId == macroprocesoSeccion.Id);

            var capitulosModificados = new CapituloModificado();
            capitulosModificados.SeccionCapituloId = seccionCapitulo.Id;
            capitulosModificados.CapituloId = capitulo;
            capitulosModificados.SeccionId = seccion;

            return capitulosModificados;
        }

        public bool GuardarJustificacionCambios(CapituloModificado capituloModificados)
        {
            var idActualizacion = Contexto.uspPostActualizaCapituloModificado(
                capituloModificados.ProyectoId,
                capituloModificados.Usuario,
                capituloModificados.Justificacion,
                capituloModificados.InstanciaId,
                capituloModificados.SeccionCapituloId,
                capituloModificados.AplicaJustificacion,
                capituloModificados.Cuenta,
                capituloModificados.Modificado
            );  
            return idActualizacion > 0;
        }

        public RespuestaGeneralDto ValidarSeccionCapitulos(int idMacroproceso, int IdProyecto, Guid IdInstancia)
        {
            RespuestaGeneralDto respuesta = new RespuestaGeneralDto();
            var resultSp = Contexto.UspGetValidarCapitulos(idMacroproceso, IdProyecto, IdInstancia);
            respuesta.Exito = true;
            foreach (var item in resultSp)
            {
                respuesta.Mensaje = respuesta.Mensaje + item;
                respuesta.Exito = false;
            }

            return respuesta;
        }

        public CapituloModificado ObtenerCapitulosModificados(string capitulo, string seccion, string GuiMacroproceso, int idProyecto, Guid guidInstancia)
        {
            CapituloModificado capitulomodificado = new CapituloModificado();
            var seccionCapitulo = ObtenerSeccionCapitulo(GuiMacroproceso, capitulo, seccion);
            var capitulosModificados = Contexto.CapitulosModificados.Where(x => x.InstanciaId == guidInstancia && x.ProyectoId == idProyecto && x.SeccionCapituloId == seccionCapitulo.SeccionCapituloId).FirstOrDefault();
            if (capitulosModificados != null)
            {
                capitulomodificado = new CapituloModificado()
                {
                     ProyectoId = capitulosModificados.ProyectoId.Value,
                     Justificacion = capitulosModificados.Justificacion,
                     Usuario = capitulosModificados.CreatedBy,
                     SeccionCapituloId = capitulosModificados.SeccionCapituloId,
                     SeccionId = capitulosModificados.SeccionCapituloId,
                     Modificado = capitulosModificados.Modificado,
                     InstanciaId = capitulosModificados.InstanciaId,
                };
            }
            return capitulomodificado;
        }

        public List<ErroresProyectoDto> ObtenerErroresProyecto(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia)
        {

            var proyecto = Contexto.uspProyectosPorId(Convert.ToString(idProyecto)).FirstOrDefault();
            var eroresProyecto = new List<ErroresProyectoDto>();
            var eroresProyectoff = new List<ErroresProyectoDto>();
            var erroresSeccionCapitulo = Contexto.UspGetErroresProyecto(GuiMacroproceso, guidInstancia, idProyecto).ToList();
            eroresProyecto = erroresSeccionCapitulo.Select(est => new ErroresProyectoDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            /* Fuentes financación */
            var erroresSeccionCapituloFF = Contexto.UspGetErroresProyectoFuenteFinanciacion(GuiMacroproceso, guidInstancia, idProyecto).ToList();
            eroresProyectoff = erroresSeccionCapituloFF.Select(est => new ErroresProyectoDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            foreach (ErroresProyectoDto item in eroresProyecto)
            {
                if (item.Capitulo == "fuentesdefinanc" && eroresProyectoff.Count() > 0) 
                {
                    item.Errores = eroresProyectoff[0].Errores;
                }
            }

            /* Costos - Se agrega error COST003 Costos vs Fuentes de financicación */
            if (proyecto != null)
            eroresProyecto = GetErroresCostos(eroresProyecto, proyecto.CodigoBpin);
          
            return eroresProyecto.Where(p => p.Errores != null).ToList();
        }

        public List<ErroresProyectoDto> ObtenerErroresSeguimiento(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia)
        {

            //var proyecto = Contexto.uspProyectosPorId(Convert.ToString(idProyecto)).FirstOrDefault();
            
            var eroresProyecto = new List<ErroresProyectoDto>();
            var eroresProyectoff = new List<ErroresProyectoDto>();
            //var erroresSeccionCapitulo = Contexto.UspGetErroresProyecto(GuiMacroproceso, guidInstancia, idProyecto).ToList();
            var erroresSeccionCapitulo = ContextoOnlySP.UspGetErroresSeguimiento(GuiMacroproceso, guidInstancia, idProyecto).ToList();
            eroresProyecto = erroresSeccionCapitulo.Select(est => new ErroresProyectoDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            return eroresProyecto.Where(p => p.Errores != null).ToList();
        }

        private List<ErroresProyectoDto> GetErroresCostos(List<ErroresProyectoDto> eroresProyecto, string CodigoBpin)
        {
            var costosComparacion = Contexto.UspGetCostosPIIPVsFuentesPiip_JSON(CodigoBpin).FirstOrDefault();
            if(costosComparacion != null)
            {
                var listEtapasCostos = JsonConvert.DeserializeObject<VigenciasFuentesCostosDtoJson>(costosComparacion);
                var listaErrores = new List<VigenciasEtapas>();
                if (listEtapasCostos.Etapas.Count > 0)
                {
                    var etapas = listEtapasCostos.Etapas.ToList();
                    var anioActual = DateTime.Now.Year;
                    foreach (var etapa in etapas)
                    {
                        var errores = etapa.Valores.ToList();
                        if (errores.Exists(p => (p.ValorCosto != p.ValorFuentes) && p.Vigencia >= anioActual)) listaErrores.Add(etapa);
                    }

                    if (listaErrores.Count > 0)
                    {
                        var erroresCostosObj = new ErroresJsonCostos();

                        var erroresCostosIndx = eroresProyecto.FindIndex(p => p.Seccion == "recursos" && p.Capitulo == "costosdelasacti");
                        if(erroresCostosIndx != -1 && eroresProyecto[erroresCostosIndx].Errores != null)
                        {
                            erroresCostosObj = JsonConvert.DeserializeObject<ErroresJsonCostos>(eroresProyecto[erroresCostosIndx].Errores);
                        }
                        erroresCostosObj.recursoscostosdelasacti.Add(new ErroresJson()
                        {
                            Error = "COST003",
                            Descripcion = "La sumatoria de costos no coincide con fuentes de financiación",
                            Data = JsonConvert.SerializeObject(listaErrores)
                        });

                        var erroresJson = JsonConvert.SerializeObject(erroresCostosObj);
                        if (erroresCostosIndx != -1) eroresProyecto[erroresCostosIndx].Errores = erroresJson;
                        else
                        {
                            eroresProyecto.Add(new ErroresProyectoDto()
                            {
                                Seccion = "recursos",
                                Capitulo = "costosdelasacti",
                                Errores = erroresJson
                            });
                        }
                    }
                }
            }
            return eroresProyecto;

        }

        public List<ErroresTramiteDto> ObtenerErroresTramite(Guid GuiMacroproceso,  Guid guidInstancia,Guid accionId, string usuarioDNP,bool tieneCDP)
        {
            var eroresTramite = new List<ErroresTramiteDto>();
            var erroresSeccionCapitulo = Contexto.UspGetErroresTramite(GuiMacroproceso, guidInstancia, accionId, usuarioDNP, tieneCDP).ToList();
            eroresTramite = erroresSeccionCapitulo.Select(est => new ErroresTramiteDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            return eroresTramite;
        }

        public List<ErroresTramiteDto> ObtenerErroresViabilidad(Guid GuiMacroproceso, int ProyectoId, Guid NivelId, Guid InstanciaId)
        {
            var eroresTramite = new List<ErroresTramiteDto>();
            var erroresSeccionCapitulo = Contexto.UspGetErroresViabilidad(GuiMacroproceso, ProyectoId, NivelId, InstanciaId).ToList();
            eroresTramite = erroresSeccionCapitulo.Select(est => new ErroresTramiteDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            return eroresTramite;
        }

        public List<SeccionesTramiteDto> ObtenerSeccionesTramite(Guid GuiMacroproceso, Guid guidInstancia)
        {
            var seccionesTramite = new List<SeccionesTramiteDto>();
            var seccionesTramitePestana = Contexto.uspGetSecciones(GuiMacroproceso, guidInstancia).ToList();
            seccionesTramite = seccionesTramitePestana.Select(est => new SeccionesTramiteDto()
            {
                Id = est.Id,
                Nombre = est.Nombre,
                NombreModificado = est.NombreModificado,
                NombrePestana = est.NombrePestana,
                Porcentaje = est.Porcentaje
            }).ToList();

            return seccionesTramite;
        }

        public List<SeccionesTramiteDto> ObtenerSeccionesPorFase(Guid guidInstancia, Guid guidFaseNivel)
        {
            var seccionesTramite = new List<SeccionesTramiteDto>();
            var seccionesTramitePestana = Contexto.uspGetSeccionesPorFase(guidInstancia,guidFaseNivel).ToList();
            seccionesTramite = seccionesTramitePestana.Select(est => new SeccionesTramiteDto()
            {
                Id = est.Id,
                Nombre = est.Nombre,
                NombreModificado = est.NombreModificado,
                NombrePestana = est.NombrePestana,
                Porcentaje = est.Porcentaje
            }).ToList();

            return seccionesTramite;
        }

        public SeccionesCapitulos EliminarCapituloModificado(CapituloModificado capituloModificados)
        {
            var resultado = new SeccionesCapitulos();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    Contexto.uspPostEliminarCapituloModificado(capituloModificados.InstanciaId, capituloModificados.SeccionCapituloId, capituloModificados.ProyectoId, errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = "Capitulo modificado Eliminado Exitosamente!";
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
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

        public List<ErroresPreguntasDto> ObtenerErroresAprobacionRol(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia)
        {

            var proyecto = Contexto.uspProyectosPorId(Convert.ToString(idProyecto)).FirstOrDefault();
            var eroresProyecto = new List<ErroresPreguntasDto>();
            var eroresProyectoff = new List<ErroresPreguntasDto>();
            var erroresSeccionCapitulo = Contexto.UspGetErroresPreguntasAprobacionRol(GuiMacroproceso, guidInstancia, idProyecto).ToList();
            eroresProyecto = erroresSeccionCapitulo.Select(est => new ErroresPreguntasDto()
            {
                Seccion = est.seccion,
                Capitulo = est.capitulo,
                Errores = est.Errores
            }).ToList();

            foreach (ErroresPreguntasDto item in eroresProyecto)
            {
                if (item.Capitulo == "AprobacionRol" && eroresProyectoff.Count() > 0)
                {
                    item.Errores = eroresProyectoff[0].Errores;
                }
            }

            return eroresProyecto;
        }

        public bool FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada capituloModificados)
        {
            
            var idActualizacion = Contexto.uspPostFocalizacionActualizaPoliticasModificadas(
                capituloModificados.ProyectoId,
                capituloModificados.Usuario,
                capituloModificados.Justificacion,
                capituloModificados.InstanciaId,
                capituloModificados.SeccionCapituloId,
                capituloModificados.PoliticaId
            );
            return idActualizacion > 0;
        }

        public List<ErroresProyectoDto> ObtenerErroresProgramacion(Guid guidInstancia, Guid accionId)
        {
            var eroresTramite = new List<ErroresProyectoDto>();
            // var erroresSeccionCapitulo = Contexto.UspGetErroresTramite(GuiMacroproceso, guidInstancia, accionId, usuarioDNP, tieneCDP).ToList();

            var erroresSeccionCapitulo = Contexto.Database.SqlQuery<ErroresProyectoDto>("Transversal.UspGetErroresProgramacion  @InstanciaId, @accionId ",
                                              new SqlParameter("InstanciaId", guidInstancia),
                                              new SqlParameter("accionId", accionId)
                                               ).ToList();

            eroresTramite = erroresSeccionCapitulo.Select(est => new ErroresProyectoDto()
            {
                Seccion = est.Seccion,
                Capitulo = est.Capitulo,
                Errores = est.Errores
            }).ToList();

            return eroresTramite;
        }

    }
}
