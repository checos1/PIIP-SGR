namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos
{
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using Interfaces;
    using Interfaces.TramitesProyectos;
    using Modelo;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    public class TramitesProyectosPersistencia : Persistencia, ITramitesProyectosPersistencia
    {
        #region Constructor

        public TramitesProyectosPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto,
                                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();


            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostAgregarProyectos(JsonUtilidades.ACadenaJson(datosTramiteProyectosDto),
                                                         usuario,
                                                         null,
                                                         null,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

                //return resultado;

            }
        }

        public TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar,
                                                          string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostActualizaValoresInfomracionPresupuestal(JsonUtilidades.ACadenaJson(parametrosGuardar),
                                                         usuario,
                                                         null,
                                                         null,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar,
                                                             string usuario)
        {
            var resultado = new TramitesResultado();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            bool? isCDP = null;
            if (parametrosGuardar != null)
            {
                if (parametrosGuardar.Count > 0)
                {
                    var tipoRequisito = parametrosGuardar.First().IdTipoRequisito;
                    if (tipoRequisito == 1) isCDP = true;
                    else if (tipoRequisito == 2) isCDP = false;
                }

            }

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    if (parametrosGuardar.Count > 1 || (parametrosGuardar.Count == 1 && parametrosGuardar.First().Descripcion != "BorrarTodo"))
                    {
                        var sinIdProyectoTramite = parametrosGuardar.Where(a => a.IdProyectoTramite == 0);
                        if (sinIdProyectoTramite.Count() != 0)
                        {
                            var tramiteId = parametrosGuardar.First().IdTramite;
                            var proyectoId = parametrosGuardar.First().IdProyecto;
                            var tramiteProyectoCollection = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == tramiteId && tramiteProyecto.ProyectoId == proyectoId).ToArray();
                            if (tramiteProyectoCollection.Length == 1)
                            {
                                var corregida = sinIdProyectoTramite.Select(tramiteProyecto =>
                                {
                                    tramiteProyecto.IdProyectoTramite = tramiteProyectoCollection.First().Id;
                                    return tramiteProyecto;
                                });
                                parametrosGuardar = parametrosGuardar.Except(sinIdProyectoTramite).ToList();
                                parametrosGuardar.AddRange(corregida);
                            }
                        }

                        if (isCDP.HasValue)
                        {
                            if (isCDP.Value) Contexto.uspPostActualizaValoresTipoRequisito(JsonUtilidades.ACadenaJson(parametrosGuardar), usuario, null, null, errorValidacionNegocio);
                            else Contexto.uspPostActualizaValoresTipoRequisitoCRP(JsonUtilidades.ACadenaJson(parametrosGuardar), usuario, null, null, errorValidacionNegocio);
                        }
                        else throw new Exception("tipo de requisito no soportado");

                        if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                        {
                            dbContextTransaction.Commit();
                            resultado.Exito = true;
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
                    else if (parametrosGuardar.Count == 1 && parametrosGuardar.First().Descripcion == "BorrarTodo")
                    {
                        var tramiteId = parametrosGuardar.First().IdTramite;
                        var proyectoId = parametrosGuardar.First().IdProyecto;
                        var tipoRequisitoId = isCDP.HasValue && isCDP.Value ? 1 : 2;
                        var tramiteProyectoId = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == tramiteId && tramiteProyecto.ProyectoId == proyectoId).FirstOrDefault()?.Id ?? 0;
                        var tramiteProyectoRequisito = Contexto.ProyectosRequisitos.Where(proyectoRequisitos => proyectoRequisitos.Proyectos.Id == tramiteProyectoId && proyectoRequisitos.TipoRequisitoId == tipoRequisitoId).ToArray();
                        if (tramiteProyectoRequisito.Length == 1) Contexto.ProyectosRequisitos.Remove(tramiteProyectoRequisito[0]);
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                    }
                    return resultado;
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

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostEliminarProyectos(TramiteId, ProyectoId, errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

                //return resultado;

            }
        }


        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            var result = ContextoOnlySP.uspGetProyectosTramiteNegocio(TramiteId);
            //IEnumerable<ProyectosEnTramiteDto> proyectos = null;
            IEnumerable<ProyectosEnTramiteDto> proyectos = result.Select(p => new ProyectosEnTramiteDto
            {
                Sector = p.Sector,
                NombreEntidad = p.NombreEntidad,
                BPIN = p.BPIN,
                NombreProyecto = p.NombreProyecto,
                ProyectoId = p.ProyectoId,
                EntidadId = p.EntidadId,
                TipoProyecto = p.TipoProyecto,
                Estado = p.Estado,
                EstadoActualizacion = p.EstadoActualizacion,
                TramiteId = p.TramiteId,
                ValorMontoProyectoNacion = p.ValorMontoProyectoNacion,
                ValorMontoProyectoPropios = p.ValorMontoProyectoPropios,
                ValorMontoTramiteNacion = p.ValorMontoTramiteNacion,
                ValorMontoTramitePropios = p.ValorMontoTramitePropios,
                Programa = p.Programa,
                SubPrograma = p.SubPrograma,
                CodigoPresupuestal = p.CodigoPresupuestal,
                ValorMontoTramiteNacionSSF= p.ValorMontoTramiteNacionSSF,
                ValorMontoProyectoNacionSSF= p.ValorMontoProyectoNacionSSF

            });

            return proyectos;
        }

        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramite(int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId)
        {
            if (Rol != new Guid() || Rol.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                var rta = Contexto.uspGetTipoDocumentosSoportePorRol(TipoTramiteId, Rol, tramiteId, nivelId).ToList();
                return rta.ConvertAll(x => new TipoDocumentoTramiteDto
                {
                    Id = x.id,
                    TipoDocumentoId = x.id,
                    TipoDocumento = x.TipoDocumento,
                    TipoTramiteId = x.TipoTramiteId,
                    Obligatorio = x.Obligatorio == 1 ? true:false,
                    faseId = x.FaseId,
                    RolId = x.IdRol
                });


            }
            else
            {

                return (from TDT in Contexto.TipoDocumentoTramite
                        join TTR in Contexto.TipoDocumentoTramiteRol on TDT.Id equals TTR.TipoDocumentoTramite.Id
                        join TD in Contexto.TipoDocumento on TDT.TipoDocumento.Id equals TD.Id
                        where TDT.TipoTramiteId == TipoTramiteId //&& TTR.IdRol == Rol
                        select new TipoDocumentoTramiteDto
                        {
                            Id = TDT.Id,
                            TipoDocumentoId = TD.Id,
                            TipoDocumento = TD.Descripcion,
                            TipoTramiteId = TDT.TipoTramiteId,
                            Obligatorio = TDT.Obligatorio,
                        }).ToList();
            }
        }

        public void ActualizarInstanciaProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {
            var proyectoTramite = Contexto.Proyectos.FirstOrDefault(i => i.TramiteId == DatosInstanciasProyecto.TramiteId && i.ProyectoId == DatosInstanciasProyecto.ProyectoId);

            if (proyectoTramite == null) return;
            // ReSharper disable once PossibleNullReferenceException
            proyectoTramite.FlujoId = DatosInstanciasProyecto.FlujoId;
            proyectoTramite.InstanciaId = DatosInstanciasProyecto.InstanciaId;
            proyectoTramite.FechaModificacion = DateTime.Now;
            proyectoTramite.ModificadoPor = usuario;
            Contexto.SaveChanges();

        }

        public IEnumerable<JustificacionTramiteProyectoDto> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            var result = Contexto.uspGetPreguntasJustificacionTramite(TramiteId, ProyectoId, TipoTramiteId, IdNivel);
            IEnumerable<JustificacionTramiteProyectoDto> justificaciones = result.Select(j => new JustificacionTramiteProyectoDto
            {
                TramiteId = j.TramiteId,
                ProyectoId = j.ProyectoId,
                JustificacionPreguntaId = j.JustificacionPreguntaId,
                JustificacionPregunta = j.JustificacionPregunta,
                OrdenJustificacionPregunta = j.OrdenJustificacionPregunta,
                JustificacionRespuesta = j.JustificacionRespuesta,
                ObservacionPregunta = j.ObservacionPregunta,
                Tematica = j.Tematica,
                OrdenTematica = j.OrdenTematica,
                NombreRol = j.NombreRol,
                NombreNivel = j.NombreNivel,
                CuestionarioId = j.CuestionarioId,
                Cuenta = j.cuenta,
                NombreUsuario = j.NombreUsuario,
                FechaEnvio = j.FechaCreacion,
                OpcionesRespuesta = j.OpcionesRespuesta
            });
            return justificaciones;
        }

        public TramitesResultado GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarRespuestaJustificacionTramite(JsonUtilidades.ACadenaJson(justificacionTramiteProyectoDto),
                                                         usuario,
                                                         errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                //return resultado;
            }

        }

        public IEnumerable<FuentePresupuestalDto> ObtenerFuentesInformacionPresupuestal()
        {
            var result = Contexto.uspGetFuentesInformacionPresupuestal().ToList();
            //IEnumerable<ProyectosEnTramiteDto> proyectos = null;
            IEnumerable<FuentePresupuestalDto> fuentes = result.Select(p => new FuentePresupuestalDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Codigo = p.Codigo,
                Origen = p.Origen
            });

            return fuentes;
        }

        public IEnumerable<TipoRequisitoDto> ObtenerTiposRequisito()
        {
            var result = Contexto.uspGetTiposRequisito().ToList();
            //IEnumerable<ProyectosEnTramiteDto> proyectos = null;
            IEnumerable<TipoRequisitoDto> tr = result.Select(p => new TipoRequisitoDto
            {
                Id = p.Id,
                TipoRequisito = p.TipoRequisito,
                FaseId = p.FaseId.HasValue ? p.FaseId.Value : 0,
                Descripcion = p.Descripcion
            });

            return tr;
        }

        public IEnumerable<ProyectoFuentePresupuestalDto> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto)
        {
            List<Dominio.Dto.Tramites.ProyectoFuentePresupuestalDto> listatmp = new List<Dominio.Dto.Tramites.ProyectoFuentePresupuestalDto>();
            var result = Contexto.uspGetProyectoFuentePresupuestalPorTramite(pProyectoId, pTramiteId, pTipoProyecto).ToList();

            listatmp = result
                .Select(x => new { x.Id, x.TramiteProyectoId, x.FuenteId })
                .Distinct()
                .ToList().ConvertAll(item => new Dominio.Dto.Tramites.ProyectoFuentePresupuestalDto
                {
                    Id = item.Id.HasValue ? item.Id.Value : 0,
                    TramiteProyectoId = item.TramiteProyectoId.HasValue ? item.TramiteProyectoId.Value : 0,
                    FuenteId = item.FuenteId.HasValue ? item.FuenteId.Value : 0,
                }).ToList();


            foreach (var item in listatmp)
            {
                item.ListaFuentes = result
                    .Where(x => x.FuenteId == item.FuenteId && x.Id == item.Id)
                .Select(x => new
                {
                    x.FuenteId,
                    x.Nombre,
                    x.Origen,
                    x.TramiteProyectoId,
                    x.Accion,
                    x.ValorInicialCSF,
                    x.ValorInicialSSF,
                    x.ValorVigenteCSF,
                    x.ValorVigenteSSF,
                    x.ValorDisponibleCSF,
                    x.ValorDisponibleSSF,
                    x.ValorContracreditoCSF,
                    x.ValorContracreditoSSF,
                    x.idTipoValorContracreditoCSF,
                    x.idTipoValorContracreditoSSF,
                })
                .Distinct()
                .ToList().ConvertAll(f => new FuentePresupuestalDto
                {
                    Id = f.FuenteId.HasValue ? f.FuenteId.Value : 0,
                    Nombre = f.Nombre,
                    Origen = f.Origen,
                    TramiteProyectoId = f.TramiteProyectoId.HasValue ? f.TramiteProyectoId.Value : 0,
                    Accion = f.Accion,
                    ValorInicialCSF = f.ValorInicialCSF,
                    ValorInicialSSF = f.ValorInicialSSF,
                    ValorVigenteCSF = f.ValorVigenteCSF,
                    ValorVigenteSSF = f.ValorVigenteSSF,
                    ValorDisponibleCSF = f.ValorDisponibleCSF,
                    ValorDisponibleSSF = f.ValorDisponibleSSF,
                    ValorContracreditoCSF = f.ValorContracreditoCSF,
                    ValorContracreditoSSF = f.ValorContracreditoSSF,
                    idTipoValorContracreditoCSF = f.idTipoValorContracreditoCSF.HasValue ? f.idTipoValorContracreditoCSF.Value : 0,
                    idTipoValorContracreditoSSF = f.idTipoValorContracreditoSSF.HasValue ? f.idTipoValorContracreditoSSF.Value : 0
                }).ToList();

            }

            return listatmp.AsEnumerable<Dominio.Dto.Tramites.ProyectoFuentePresupuestalDto>();

        }

        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            List<ProyectoRequisitoDto> listatmp = new List<ProyectoRequisitoDto>();

            if (isCDP)
            {
                var result = Contexto.uspGetProyectoRequisitosPorTramite(pProyectoId, pTramiteId).ToList();
                if (result.Count != 0)
                    listatmp = result
                        .Select(x => new { x.Id, x.TramiteProyectoId, x.Descripcion, x.Numero, x.Fecha, x.UnidadEjecutora, x.TipoRequisitoId })
                        .Distinct()
                        .ToList().ConvertAll(item => new ProyectoRequisitoDto
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            TramiteProyectoId = item.TramiteProyectoId,
                            Numero = item.Numero,
                            Fecha = item.Fecha,
                            UnidadEjecutora = item.UnidadEjecutora,
                            TipoRequisitoId = item.TipoRequisitoId
                        }).ToList();

                foreach (var item in listatmp)
                {
                    item.ListaTiposRequisito = result
                         .Where(x => x.Id == item.Id)
                    .Select(x => new { x.TipoRequisitoId, x.TipoRequisitoDescripcion, x.TipoRequisito })
                    .Distinct()
                    .ToList().ConvertAll(f => new TipoRequisitoDto
                    {
                        Id = f.TipoRequisitoId,
                        TipoRequisito = f.TipoRequisito,
                        Descripcion = f.TipoRequisitoDescripcion,
                    }).ToList();

                }

                foreach (var item in listatmp)
                {
                    foreach (var itemfuente in item.ListaTiposRequisito)
                    {
                        itemfuente.ListaValores = result
                             .Where(x => x.TipoRequisitoId == itemfuente.Id && x.Id == item.Id)
                        .Select(x => new { x.proyectorequisitovalorid, x.TipoRequisitoId, x.TipoValorId, x.TipoValor, x.Valor })
                        .Distinct()
                        .ToList().ConvertAll(v => new ProyectoRequisitoValoresDto
                        {
                            Id = v.proyectorequisitovalorid,
                            ProyectosRequisitoId = v.TipoRequisitoId,
                            TipoValor = new TipoValorDto { Id = v.TipoValorId, TipoValorFuente = v.TipoValor },
                            Valor = v.Valor
                        }).ToList();
                    }
                }
            }
            else
            {
                var result = Contexto.uspGetProyectoRequisitosPorTramiteCRP(pProyectoId, pTramiteId).ToList();
                if (result.Count != 0)
                    listatmp = result
                        .Select(x => new { x.Id, x.TramiteProyectoId, x.Descripcion, x.Numero, x.NumeroContrato, x.Fecha, x.UnidadEjecutora, x.TipoRequisitoId })
                        .Distinct()
                        .ToList().ConvertAll(item => new ProyectoRequisitoDto
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            TramiteProyectoId = item.TramiteProyectoId,
                            TipoRequisitoId = item.TipoRequisitoId,
                            Numero = item.Numero,
                            NumeroContrato = item.NumeroContrato,
                            Fecha = item.Fecha,
                            UnidadEjecutora = item.UnidadEjecutora
                        }).ToList();

                foreach (var item in listatmp)
                {
                    item.ListaTiposRequisito = result
                         .Where(x => x.Id == item.Id)
                    .Select(x => new { x.TipoRequisitoId, x.TipoRequisitoDescripcion, x.TipoRequisito })
                    .Distinct()
                    .ToList().ConvertAll(f => new TipoRequisitoDto
                    {
                        Id = f.TipoRequisitoId,
                        TipoRequisito = f.TipoRequisito,
                        Descripcion = f.TipoRequisitoDescripcion,
                    }).ToList();

                }

                foreach (var item in listatmp)
                {
                    foreach (var itemfuente in item.ListaTiposRequisito)
                    {
                        itemfuente.ListaValores = result
                             .Where(x => x.TipoRequisitoId == itemfuente.Id && x.Id == item.Id)
                        .Select(x => new { x.proyectorequisitovalorid, x.TipoRequisitoId, x.TipoValorId, x.TipoValor, x.Valor })
                        .Distinct()
                        .ToList().ConvertAll(v => new ProyectoRequisitoValoresDto
                        {
                            Id = v.proyectorequisitovalorid,
                            ProyectosRequisitoId = v.TipoRequisitoId,
                            TipoValor = new TipoValorDto { Id = v.TipoValorId, TipoValorFuente = v.TipoValor },
                            Valor = v.Valor
                        }).ToList();
                    }
                }

            }

            return listatmp.AsEnumerable<ProyectoRequisitoDto>();

        }


        public TramitesResultado ActualizarValoresProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {

            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostActualizarProyectoValores(DatosInstanciasProyecto.TramiteId,
                                                        DatosInstanciasProyecto.ProyectoId,
                                                        DatosInstanciasProyecto.EntidadId,
                                                        DatosInstanciasProyecto.TipoRolId,
                                                        DatosInstanciasProyecto.ValorMontoNacionEnTramite,
                                                        DatosInstanciasProyecto.ValorMontoPropiosEnTramite,
                                                        DatosInstanciasProyecto.TipoProyecto,
                                                         usuario,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                //return resultado;
            }
        }



        public TramitesResultado ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto,
                                                                   string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();


            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostValidarEnviarDatosTramiteNegocio(JsonUtilidades.ACadenaJson(datosTramiteProyectosDto),
                                                         usuario,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

                //return resultado;

            }
        }


        public TramitesResultado ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto datosTramiteProyectosDto,
                                                                string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();


            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostValidarEnviarDatosAprobacionTramiteNegocio(JsonUtilidades.ACadenaJson(datosTramiteProyectosDto),
                                                         usuario,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

                //return resultado;

            }
        }

        public IEnumerable<JustificacionTematicaDto> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            List<JustificacionTematicaDto> listadoRetorno = new List<JustificacionTematicaDto>();
            var result = Contexto.uspGetPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel);

            if (result == null)
                return listadoRetorno;
            var listaResultadoSp = result.ToList();

            foreach (var tematica in listaResultadoSp)
            {
                if (listadoRetorno.Where(x => x.Tematica == tematica.Tematica).Count() == 0)
                {
                    JustificacionTematicaDto tamaticaDto = new JustificacionTematicaDto();
                    tamaticaDto.Tematica = tematica.Tematica;
                    tamaticaDto.OrdenTematica = tematica.OrdenTematica;
                    listadoRetorno.Add(tamaticaDto);
                }
            }

            foreach (var tematica in listadoRetorno)
            {
                tematica.justificaciones = new List<JustificacionTramiteProyectoDto>();
                foreach (var j in listaResultadoSp.Where(x => x.Tematica == tematica.Tematica))
                {
                    JustificacionTramiteProyectoDto justificacion = new JustificacionTramiteProyectoDto();


                    justificacion.TramiteId = j.TramiteId;
                    justificacion.ProyectoId = j.ProyectoId;
                    justificacion.JustificacionPreguntaId = j.JustificacionPreguntaId;
                    justificacion.JustificacionPregunta = j.JustificacionPregunta;
                    justificacion.OrdenJustificacionPregunta = j.OrdenJustificacionPregunta;
                    justificacion.JustificacionRespuesta = j.JustificacionRespuesta;
                    justificacion.ObservacionPregunta = j.ObservacionPregunta;
                    justificacion.OpcionesRespuesta = j.OpcionesRespuesta;
                    justificacion.Tematica = j.Tematica;
                    justificacion.OrdenTematica = j.OrdenTematica;
                    justificacion.NombreRol = j.NombreRol;
                    justificacion.NombreNivel = j.NombreNivel;
                    justificacion.CuestionarioId = j.CuestionarioId;
                    justificacion.Usuario = j.usuario;
                    justificacion.FechaEnvio = j.FechaEnvio;
                    justificacion.Paso = j.paso;
                    justificacion.NombreUsuario = j.NombreUsuario;
                    justificacion.Cuenta = j.Cuenta;

                    tematica.justificaciones.Add(justificacion);
                }
            }


            return listadoRetorno;
        }


        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId)
        {
            var result = Contexto.uspGetObtenerProyectosTramiteNegocioAprobacion(TramiteId, TipoRolId);
            IEnumerable<ProyectosEnTramiteDto> proyectos = result.Select(p => new ProyectosEnTramiteDto
            {
                Sector = p.Sector,
                NombreEntidad = p.NombreEntidad,
                BPIN = p.BPIN,
                NombreProyecto = p.NombreProyecto,
                ProyectoId = p.ProyectoId,
                EntidadId = p.EntidadId,
                TipoProyecto = p.TipoProyecto,
                EstadoActualizacion = p.Estado,
                TramiteId = p.TramiteId,
                Programa = p.Programa,
                SubPrograma = p.Subprograma,
                CodigoPresupuestal = p.CodigoPresupuestal,
                ValorMontoProyectoNacion = p.ValorMontoProyectoNacion,
                ValorMontoProyectoPropios = p.ValorMontoProyectoPropios,
                ValorMontoTramiteNacion = p.ValorMontoTramiteNacion,
                ValorMontoTramitePropios = p.ValorMontoTramitePropios,
                ValorMontoAprobadosNacion = p.ValorMontoAprobadosNacion,
                ValorMontoAprobadosPropios = p.ValorMontoAprobadosPropios
            });

            return proyectos;
        }

        public IEnumerable<FuentesTramiteProyectoAprobacionDto> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto)
        {
            var result = Contexto.uspGetObtenerFuentesTramiteProyectoAprobacion(proyectoId, tramiteId, pTipoProyecto);
            IEnumerable<FuentesTramiteProyectoAprobacionDto> proyectos = result.Select(p => new FuentesTramiteProyectoAprobacionDto
            {
                FuenteId = p.FuenteId,
                NombreFuente = p.Nombre,
                TipoAccion = p.Accion,
                ProyectoId = p.ProyectoId,
                TramiteId = p.TramiteId,
                Origen = p.Origen,
                ValorInicialCSF = p.ValorInicialCSF,
                ValorInicialSSF = p.ValorInicialSSF,
                ValorVigenteCSF = p.ValorVigenteCSF,
                ValorVigenteSSF = p.ValorVigenteSSF,
                ValorSolicitadoCSF = p.ValorContracreditoCSF,
                ValorSolicitadoSSF = p.ValorContracreditoSSF,
                ValorAprobadoCSF = p.ValorAprobadoCSF,
                ValorAprobadoSSF = p.ValorAprobadoSSF
            });

            return proyectos;
        }


        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostGuardarFuentesTramiteProyectoAprobacion(JsonUtilidades.ACadenaJson(fuentesTramiteProyectoAprobacion),
                                                         usuario,
                                                         errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                //return resultado;
            }

        }


        public CodigoPresupuestalDto ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            CodigoPresupuestalDto codigoPresupuestal = null;
            var result = Contexto.uspGetConsultarCodigoPresupuestal(entidadId, tramiteId, proyectoId).FirstOrDefault();

            if (result != null)
            {
                codigoPresupuestal = new CodigoPresupuestalDto
                {
                    CodigoPresupuestal = result.CodigoPresupuestal,
                    ProyectoId = result.ProyectoId.HasValue ? result.ProyectoId.Value : 0,
                    CodigoEntidad = result.CodigoEntidad,
                    EntidadId = result.EntityTypeCatalogOptionId.HasValue ? result.EntityTypeCatalogOptionId.Value : 0,
                    Programa = result.Programa,
                    Subprograma = result.Subprograma
                };
            }


            return codigoPresupuestal;
        }

        public TramitesResultado ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostActualizaCodigoPresupuestal(JsonUtilidades.ACadenaJson(new CodigoPresupuestalDto { TramiteId = tramiteId, ProyectoId = proyectoId, EntidadId = entidadId }),
                                                         usuario,
                                                         errorValidacionNegocio);

                    if (errorValidacionNegocio.Value == null || string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                    return resultado;
                    //throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                //return resultado;
            }
        }

        public bool CrearAlcanceTramite(AlcanceTramiteDto data)
        {
            var proyectoActualList = Contexto.Proyectos.AsNoTracking().Where(x => x.TramiteId == data.TramiteId).ToList();
            foreach (var proyectoActual in proyectoActualList)
            {
                int proyectoAnteriorId = proyectoActual.Id;
                proyectoActual.Id = 0;
                proyectoActual.TramiteId = data.NuevoTramiteId;
                proyectoActual.InstanciaId = data.NuevaInstanciaId;
                proyectoActual.CreadoPor = data.Usuario;
                proyectoActual.FechaCreacion = DateTime.Now;
                proyectoActual.ModificadoPor = null;
                proyectoActual.FechaModificacion = null;
                Contexto.Proyectos.Add(proyectoActual);

                var proyectosValoresActual = Contexto.ProyectosValores.AsNoTracking().Where(x => x.Proyectos.Id == proyectoAnteriorId).ToList();
                foreach (var item in proyectosValoresActual)
                {
                    item.Id = 0;
                    item.Proyectos = proyectoActual;
                    item.CreadoPor = data.Usuario;
                    item.FechaCreacion = DateTime.Now;
                    item.ModificadoPor = null;
                    item.FechaModificacion = null;
                }
                if (proyectosValoresActual.Count() > 0)
                {
                    Contexto.ProyectosValores.AddRange(proyectosValoresActual);
                }

                var proyectosRequisitosActual = Contexto.ProyectosRequisitos.AsNoTracking().Where(x => x.Proyectos.Id == proyectoAnteriorId).ToList();
                foreach (var item in proyectosRequisitosActual)
                {
                    int proyectoRequisitoAnteriorId = item.Id;
                    item.Id = 0;
                    item.Proyectos = proyectoActual;
                    item.CreadoPor = data.Usuario;
                    item.FechaCreacion = DateTime.Now;
                    item.ModificadoPor = null;
                    item.FechaModificacion = null;
                    Contexto.ProyectosRequisitos.Add(item);
                    var ProyectosRequisitosValoresActual = Contexto.ProyectosRequisitosValores.AsNoTracking().Where(x => x.ProyectosRequisitos.Id == proyectoRequisitoAnteriorId).ToList();
                    foreach (var valor in ProyectosRequisitosValoresActual)
                    {
                        valor.Id = 0;
                        valor.ProyectosRequisitos = item;
                        valor.CreadoPor = data.Usuario;
                        valor.FechaCreacion = DateTime.Now;
                        valor.ModificadoPor = null;
                        valor.FechaModificacion = null;
                    }
                    if (ProyectosRequisitosValoresActual.Count() > 0)
                    {
                        Contexto.ProyectosRequisitosValores.AddRange(ProyectosRequisitosValoresActual);
                    }

                }

                var justificacionRespuestaActual = Contexto.JustificacionRespuesta.AsNoTracking().Where(x => x.TramiteId == data.TramiteId).ToList();
                foreach (var item in justificacionRespuestaActual)
                {
                    int justificacionRespuestaAnteriorId = item.Id;
                    item.Id = 0;
                    item.TramiteId = data.NuevoTramiteId;
                    item.CreadoPor = data.Usuario;
                    item.FechaCreacion = DateTime.Now;
                    item.ModificadoPor = null;
                    item.FechaModificacion = null;
                    Contexto.JustificacionRespuesta.Add(item);
                    var justificacionRespuestaDetalleActual = Contexto.JustificacionRespuestaDetalle.AsNoTracking().Where(x => x.JustificacionRespuesta.Id == justificacionRespuestaAnteriorId).ToList();
                    foreach (var valor in justificacionRespuestaDetalleActual)
                    {
                        valor.Id = 0;
                        valor.JustificacionRespuesta = item;
                        valor.CreadoPor = data.Usuario;
                        valor.FechaCreacion = DateTime.Now;
                        valor.ModificadoPor = null;
                        valor.FechaModificacion = null;
                    }
                    if (justificacionRespuestaDetalleActual.Count() > 0)
                    {
                        Contexto.JustificacionRespuestaDetalle.AddRange(justificacionRespuestaDetalleActual);
                    }

                }

                var proyectoFuentesPresupuestalActual = Contexto.ProyectoFuentesPresupuestal.AsNoTracking().Where(x => x.Proyectos.Id == proyectoAnteriorId).ToList();
                foreach (var item in proyectoFuentesPresupuestalActual)
                {
                    int proyectoFuentesPresupuestalAnteriorId = item.Id;
                    item.Id = 0;
                    item.Proyectos = proyectoActual;
                    item.CreadoPor = data.Usuario;
                    item.FechaCreacion = DateTime.Now;
                    item.ModificadoPor = null;
                    item.FechaModificacion = null;
                    Contexto.ProyectoFuentesPresupuestal.Add(item);
                    var proyectoFuentesPresupuestalValoresActual = Contexto.ProyectoFuentesPresupuestalValores.AsNoTracking().Where(x => x.ProyectoFuentesPresupuestal.Id == proyectoFuentesPresupuestalAnteriorId).ToList();
                    foreach (var valor in proyectoFuentesPresupuestalValoresActual)
                    {
                        valor.Id = 0;
                        valor.ProyectoFuentesPresupuestal = item;
                        valor.CreadoPor = data.Usuario;
                        valor.FechaCreacion = DateTime.Now;
                        valor.ModificadoPor = null;
                        valor.FechaModificacion = null;
                    }
                    if (proyectoFuentesPresupuestalValoresActual.Count() > 0)
                    {
                        Contexto.ProyectoFuentesPresupuestalValores.AddRange(proyectoFuentesPresupuestalValoresActual);
                    }

                }
                Contexto.SaveChanges();

            }
            return true;
        }
        public ResponseDto<EnvioSubDireccionDto> GuardarSolicitarConcepto(EnvioSubDireccionDto concepto)
        {
            ResponseDto<EnvioSubDireccionDto> respuesta = new Comunes.Dto.Tramites.ResponseDto<EnvioSubDireccionDto>();
            try
            {
                ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
                Contexto.uspPostGuardarSolicitarConcepto(JsonUtilidades.ACadenaJson(concepto), resultado);
                if (string.IsNullOrEmpty(resultado.Value.ToString()))
                {
                    respuesta.Estado = true;
                    respuesta.Data = concepto;
                    return respuesta;
                }
                else
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = Convert.ToString(resultado.Value);
                    return respuesta;

                }
            }
            catch (ServiciosNegocioException se)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = se.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }

        public List<EnvioSubDireccionDto> ObtenerSolicitarConcepto(int tramiteid)
        {
            try
            {
                var EnvioSubDireccionDto_ = Contexto.uspGetObtenerSolicitarConcepto_JSON(tramiteid).SingleOrDefault();
                if (EnvioSubDireccionDto_ != null)
                {
                    return JsonConvert.DeserializeObject<List<EnvioSubDireccionDto>>(EnvioSubDireccionDto_);
                }
                else
                {
                    return new List<EnvioSubDireccionDto>();
                }
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public List<TramitesProyectosDto> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario)
        {
            List<TramitesProyectosDto> resultado = new List<TramitesProyectosDto>();
            resultado = Contexto.Proyectos.Where(x => x.ProyectoId == proyectoId && x.EntidadId == entidadId)
                .ToList()
                .ConvertAll(x => new TramitesProyectosDto
                {
                    TramiteId = x.TramiteId,
                    InstanciaId = x.InstanciaId,
                    ProyectoId = x.Id,
                    EntidadId = x.EntidadId.HasValue ? x.EntidadId.Value : 0
                });
            return resultado;
        }



        public TramitesValoresProyectoDto ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuario)
        {
            TramitesValoresProyectoDto tp = new TramitesValoresProyectoDto();
            var result = Contexto.uspGetObtenerValoresProyectos(tramiteId, proyectoId, entidadId).FirstOrDefault();

            if (result != null)
            {
                tp.DecretoNacion = result.DecretoNacion;
                tp.DecretoPropios = result.DecretoPropios;
                tp.DisponibleNacion = result.DisponibleNacion;
                tp.DisponiblePropios = result.DisponiblePropios;
                tp.VigenciaFuturaNacion = result.VigenciaFuturaNacion;
                tp.VigenciaFuturaPropios = result.VigenciaFuturaPropios;
                tp.VigenteNacion = result.VigenteNacion;
                tp.VigentePropios = result.VigentePropios;
            }


            return tp;

        }

        public List<ConceptoDireccionTecnicaTramite> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {

            List<ConceptoDireccionTecnicaTramite> vConceptoDireccionTecnicaTramite = new List<ConceptoDireccionTecnicaTramite>();
            vConceptoDireccionTecnicaTramite = Contexto.uspGetPreguntasConceptoDireccionTecnicaTramite(tramiteId, nivelid).ToList()
                .ConvertAll(x => new ConceptoDireccionTecnicaTramite
                {
                    FaseId = x.FaseId,
                    ProjectId = x.ProjectId,
                    InstanciaId = x.InstanciaId,
                    FormularioId = x.FormularioId,
                    CR = x.CR,
                    AgregarRequisitos = x.AgregarRequisitos,
                    Usuario = x.Usuario,
                    Fecha = x.Fecha,
                    Observaciones = x.Observaciones,
                    Cumple = x.Cumple,
                    Definitivo = x.Definitivo,
                    PreguntaId = x.PreguntaId,
                    Pregunta = x.Pregunta,
                    Respuesta = x.Respuesta,
                    ObservacionPregunta = x.ObservacionPregunta,
                    OpcionesRespuesta = !string.IsNullOrEmpty(x.OpcionesRespuesta) ? JsonConvert.DeserializeObject<List<object>>(x.OpcionesRespuesta) : null,
                    NombreRol = x.NombreRol,
                    NombreNivel = x.NombreNivel,
                    CuestionarioProyectoId = x.CuestionarioProyectoId,
                    TramiteId = x.TramiteId.Value,
                    EsPreguntaAbierta = x.EsPreguntaAbierta.Value,
                });

            return vConceptoDireccionTecnicaTramite;

        }

        public TramitesResultado GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite, string usuario)
        {

            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPreguntasConceptoDireccionTecnicaTramite(JsonUtilidades.ACadenaJson(lConceptoDireccionTecnicaTramite), usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

        public PlantillaCarta ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite)
        {
            PlantillaCarta resultado = new PlantillaCarta();
            var lista = Contexto.uspGetPlantillaCarta(nombreSeccion, tipoTramite);
            List<uspGetPlantillaCarta_Result1> listatmp = lista.ToList();

            var resultadotmp = listatmp.Select(x => new { x.Id, x.TipoTramiteId }).Distinct().Take(1).ToList()
                .ConvertAll(f => new PlantillaCarta
                {
                    Id = f.Id,
                    IipoTramiteId = f.TipoTramiteId
                });

            if (resultadotmp != null)
            {
                resultado = resultadotmp[0];

                resultado.PlantillaSecciones = listatmp.Select(y => new { y.PantillaCartaSeccionId, y.NombreSeccion, y.Id, y.TipoTramiteId }).Distinct().ToList()
                    .ConvertAll(
                            j => new PlantillaCartaSecciones
                            {
                                Id = j.PantillaCartaSeccionId,
                                PlantillaCartaId = j.Id,
                                NombreSeccion = j.NombreSeccion,

                            });
                foreach (var item in resultado.PlantillaSecciones)
                {
                    item.PlantillaSeccionCampos = listatmp.Where(x => x.PantillaCartaSeccionId == item.Id && x.Id == item.PlantillaCartaId)
                                .Distinct().ToList()
                                .ConvertAll(c => new Campo
                                {
                                    Id = c.PlantillaCartaSeccionCampoId,
                                    PlantillaCartaSeccionId = c.PantillaCartaSeccionId,
                                    NombreCampo = c.NombreCampo,
                                    TipoCampo = new TipoCampo
                                    {
                                        TipoCampoId = c.TipoCampoId,
                                        Formato = c.FormatoCampo,
                                        Longitud = c.LongitudCampo,
                                        Nombre = c.NombreTipoCampo
                                    },
                                    TituloCampo = c.TituloCampo,
                                    TextoDefecto = c.TextoDefecto,
                                    Editable = c.Editable,
                                    Orden = c.Orden,
                                    ConsultaSql = c.ConsultaSql,
                                    ValorDefecto = c.TextoDefecto
                                });
                }
                return resultado;
            }
            else
                return new PlantillaCarta();
        }

        public List<Carta> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId)
        {

            var lista = Contexto.uspGetDatosCartaPorPlantillaSeccion(plantillaSeccionId, tramiteId).ToList();
            var resultado = lista.Select(x => new { x.Id, x.NombreFase, x.RadicadoEntrada, x.RadicadoSalida, x.TramiteId, x.NumeroTramite, x.Entidad }).Distinct().Take(1).ToList()
                .ConvertAll(x => new Carta
                {
                    Id = x.Id,
                    TramiteId = x.TramiteId,
                    Proceso = x.NumeroTramite,
                    Entidad = x.Entidad,
                    RadicadoEntrada = x.RadicadoEntrada,
                    ListaCartaSecciones = lista.Select(y => new { y.PlantillaCartaSeccionId, y.Id }).Distinct().ToList()
                   .ConvertAll(z => new CartaSecciones
                   {
                       Id = z.PlantillaCartaSeccionId,
                       CartaId = z.Id,
                       ListaCartaCampos = lista.Where(w => w.PlantillaCartaSeccionId == z.PlantillaCartaSeccionId && w.Id == z.Id).Distinct().ToList()
                       .ConvertAll(w => new CartaCampo
                       {
                           Id = w.CartaConceptoCampoId.HasValue ? w.CartaConceptoCampoId.Value : 0,
                           CartaConceptoSeccionId = w.CartaConceptoId,
                           DatoValor = w.DatoValor,
                           NombreCampo = w.NombreCampo,
                           TipoCampo = new TipoCampo
                           {
                               Tipo = w.Tipo,
                               Formato = w.Formato,
                               Longitud = w.Longitud
                           },
                           PlantillaCartaCampoId = w.PlantillaCartaCampoId.HasValue ? w.PlantillaCartaCampoId.Value : 0
                       })
                   })

                });

            var firmada = Contexto.UspGetObtenerFirmaCartaConcepto(tramiteId).FirstOrDefault();
            bool firmado = false;
            try
            {
                if (firmada != null && firmada.ToUpper().Equals("FIRMADA"))
                    firmado = true;
                else
                    firmado = false;
            }
            catch { firmado = false; }

            if (resultado != null && resultado.Count > 0)// && (firmada == null || firmada.FirstOrDefault().ToUpper().Equals("FIRMADA")))
                resultado[0].Firmada = firmado;
            //else
            //    resultado[0].Firmada = firmado;
            return resultado;
        }

        public List<Carta> ObtenerDatosCartaPorSeccionDespedia(int plantillaSeccionId, int tramiteId)
        {

            var lista = Contexto.uspGetDatosCartaPorPlantillaSeccionDespedida(plantillaSeccionId, tramiteId).ToList();
            var resultado = lista.Select(x => new { x.Id, x.NombreFase, x.RadicadoEntrada, x.RadicadoSalida, x.TramiteId, x.NumeroTramite, x.Entidad }).Distinct().Take(1).ToList()
                .ConvertAll(x => new Carta
                {
                    Id = x.Id,

                    TramiteId = x.TramiteId,
                    Proceso = x.NumeroTramite,
                    Entidad = x.Entidad,
                    ListaCartaSecciones = lista.Select(y => new { y.PlantillaCartaSeccionId, y.Id }).Distinct().ToList()
                   .ConvertAll(z => new CartaSecciones
                   {
                       Id = z.PlantillaCartaSeccionId,
                       CartaId = z.Id,
                       ListaCartaCampos = lista.Where(w => w.PlantillaCartaSeccionId == z.PlantillaCartaSeccionId && w.Id == z.Id).Distinct().ToList()
                       .ConvertAll(w => new CartaCampo
                       {
                       })
                   })

                });

            return resultado;
        }

        //Alejo
        //public List<DatosConceptoDespedidaDto> ObtenerDatosCartaConceptoDespedida(int tramiteId)
        //{
        //    try
        //    {
        //        var ConceptoDto = Contexto.uspGetDatosCartaConceptoDespedida_JSON(tramiteId).SingleOrDefault();
        //        return JsonConvert.DeserializeObject<DatosConceptoDespedidaDto>(ConceptoDto);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
        //    }

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DatosConceptoDespedidaDto ObtenerDatosCartaConceptoDespedidaPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<DatosConceptoDespedidaDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaDatosCartaConceptoDespedida);
        }



        public UsuarioTramite VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite)
        {



            return usuarioTramite;
        }

        public TramitesResultado ActualizarCartaDatosIniciales(Carta datosIniciales, string usuario)
        {

            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    List<CartaCampo> listaCampos = datosIniciales.ListaCartaSecciones[0].ListaCartaCampos;
                    Contexto.uspPostActualizaCartaDatosIniciales(datosIniciales.Id, datosIniciales.ListaCartaSecciones[0].Id, datosIniciales.TramiteId, datosIniciales.TipoId, JsonUtilidades.ACadenaJson(listaCampos), usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

            return respuesta;
        }

        public TramitesResultado ActualizarCartaDatosDespedida(Carta datosDespedida, string usuario)
        {

            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    List<CartaCampo> listaCampos = datosDespedida.ListaCartaSecciones[0].ListaCartaCampos;
                    Contexto.uspPostActualizaCartaDatosIniciales(datosDespedida.Id, datosDespedida.ListaCartaSecciones[0].Id, datosDespedida.TramiteId, datosDespedida.TipoId, JsonUtilidades.ACadenaJson(listaCampos), usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

            return respuesta;
        }

        public List<UsuarioTramite> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite)
        {
            List<UsuarioTramite> lista = new List<UsuarioTramite>();
            lista = Contexto.UspGetUsauriosRegistrados(numeroTramite).ToList()
                .ConvertAll(x => new UsuarioTramite
                {
                    ActivoUsuario = x.ActivoUsuario.HasValue ? x.ActivoUsuario.Value : false,
                    Cargo = x.Cargo,
                    Email = x.EMAIL,
                    NombreUsuario = x.NombreUsuario,
                    IDUsuarioDNP = x.IDUsuarioDNP
                });

            return lista;
        }

        public TramitesResultado CargarFirma(FileToUploadDto parametros)
        {
            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(parametros.FileAsBase64)));
                    var registro = Contexto.CartaConceptoFirma.Where(x => x.Usuario == parametros.UsuarioId && x.Activo).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.Firma = ImageToByteArray(img);
                        registro.ModificadoPor = parametros.UsuarioId;
                        registro.FechaModificacion = DateTime.Now;
                        Contexto.SaveChanges();
                    }
                    else
                    {
                        CartaConceptoFirma cartafirma = new CartaConceptoFirma();
                        cartafirma.Activo = true;
                        cartafirma.CreadoPor = parametros.UsuarioId;
                        cartafirma.FechaCreacion = DateTime.Now;
                        cartafirma.Rol = parametros.RolId.ToString();
                        cartafirma.Firma = ImageToByteArray(img);
                        cartafirma.Usuario = parametros.UsuarioId;
                        Contexto.CartaConceptoFirma.Add(cartafirma);
                        Contexto.SaveChanges();
                    }

                    respuesta.Exito = true;
                    dbContextTransaction.Commit();
                }
                catch (ServiciosNegocioException)
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

            return respuesta;
        }

        public TramitesResultado ValidarSiExisteFirmaUsuario(string idUsuario)
        {
            var respuesta = new TramitesResultado();
            var registro = Contexto.CartaConceptoFirma.Where(x => x.Usuario == idUsuario && x.Activo).FirstOrDefault();
            if (registro != null && registro.Id > 0)
            {
                respuesta.Exito = true;
                respuesta.Byte64 = registro.Firma;
            }
            else
                respuesta.Exito = false;


            return respuesta;
        }

        public TramitesResultado Firmar(int tramiteId, string radicadoSalida, string usuario)
        {
            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var x = Contexto.UspPostFirmaCartaConcepto(tramiteId, radicadoSalida, usuario, resultado);
                    if (x != null && x.FirstOrDefault() == 1)
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

            return respuesta;
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public List<CuerpoConceptoCDP> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId)
        {
            List<CuerpoConceptoCDP> resultado = new List<CuerpoConceptoCDP>();
            var lista = Contexto.uspGetDatosCartaConceptoCuerpoCDP(tramiteId).ToList()
                .ConvertAll(x => new CuerpoConceptoCDP
                {
                    CDP = x.CDP,
                    FechaCDP = x.FechaCDP,
                    ValorCDP = x.ValorCDP
                });
            return lista;
        }
        public List<CuerpoConceptoAutorizacion> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId)
        {
            List<CuerpoConceptoAutorizacion> resultado = new List<CuerpoConceptoAutorizacion>();
            // var CartaConcepto_ = ConsultarCartaConcepto(tramiteId);
            var lista = Contexto.uspGetDatosCartaConceptoCuerpoAutorizacion(tramiteId).ToList();

            resultado = lista
                .ConvertAll(x => new CuerpoConceptoAutorizacion
                {
                    EntidadId = x.EntidadId,
                    Entidad = x.Entidad,
                    TramiteId = x.TramiteId,
                    ProyectoId = x.ProyectoId,
                    NombreProyecto = x.NombreProyecto,
                    Recurso = x.Recurso,
                    ProgramaId = x.ProgramaId,
                    ProgramaCodigo = x.ProgramaCodigo,
                    Programa = x.Programa,
                    SubprogramaId = x.SubprogramaId,
                    SubProgramaCodigo = x.SubProgramaCodigo,
                    SubPrograma = x.SubPrograma,
                    TipoProyecto = x.TipoProyecto,
                    Valor = string.Join("", x.Valor.Split(',')),
                    CodigoPresupuestal = x.CodigoPresupuestal,
                    //FechaRadicacion = CartaConcepto_ == null ? DateTime.Now : CartaConcepto_.FechaCreacion,
                    //NumeroRadicacion = CartaConcepto_ == null ? "0" : CartaConcepto_.RadicadoEntrada
                });
            return resultado;
        }

        public CartaConcepto ConsultarCartaConcepto(int tramiteid)
        {
            DateTime fechaRadicacion = DateTime.Now;
            var carta = Contexto.CartaConcepto.FirstOrDefault(x => x.TramiteId == tramiteid);
            return carta;
        }

        public Carta ConsultarCarta(int tramiteid)
        {
            DateTime fechaRadicacion = DateTime.Now;
            Carta carta = new Carta();
            var cartaTmp = Contexto.CartaConcepto.FirstOrDefault(x => x.TramiteId == tramiteid);
            if (cartaTmp != null)
            {
                carta.Id = cartaTmp.Id;
                carta.TramiteId = cartaTmp.TramiteId;
                carta.RadicadoEntrada = cartaTmp.RadicadoEntrada;
                carta.RadicadoSalida = cartaTmp.RadicadoSalida;
            }
            return carta;
        }

        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
        {

            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostActualizaEstadoAjusteProyecto(objetoNegocioId, tramiteId, observacion, tipoDevolucion, usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

        public int TramiteEnPasoUno(Guid InstanciaId)
        {
            int resultado = 0;
            var rta = Contexto.uspGetTramiteEnPasoUno(InstanciaId).FirstOrDefault();
            if (rta != null)
                resultado = rta.Existe;

            return resultado;
        }

        public List<TramiteConpes> ObtenerConpesTramite(int tramiteId)
        {
            return Contexto.TramiteConpes.Where(item => item.TramiteId == tramiteId).ToList();
        }

        public void GuardarConpesTramites(int tramiteId, string usuario, List<TramiteConpesDto> conpesModel)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var tx = Contexto.Database.BeginTransaction())
            {
                var jsonModel = JsonUtilidades.ACadenaJson(conpesModel);

                var result = Contexto.UspPostAgregarConpesTramite(tramiteId, usuario, jsonModel, errorValidacionNegocio);
                if (!string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                {
                    tx.Rollback();
                    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                    throw new ServiciosNegocioException(mensajeError);
                }

                tx.Commit();
            }
        }

        public void RemoverConpesTramites(RemoverAsociacionConpesTramiteDto conpesModel)
        {
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var query = Contexto.TramiteConpes.FirstOrDefault(x => x.TramiteId == conpesModel.TramiteId && x.NumeroConpes == conpesModel.numeroConpes);
            if (query == null)
            {
                throw new ServiciosNegocioException("No se encontraron registros que eliminar");
            }

            Contexto.TramiteConpes.Remove(query);
            Contexto.SaveChanges();
        }

        public PeriodoPresidencial ObtenerPeriodoPresidencialActual()
        {
            var currentYear = DateTime.Now;
            var x = Contexto.PeriodoPresidencial
                .ToList();

            return x.FirstOrDefault(w => w.FechaInicial <= currentYear && w.FechaFinal >= currentYear);
        }

        public string EliminarAsociacionVFO(EliminacionAsociacionDto tramiteFiltroDto)
        {
            try
            {
                if (!int.TryParse(tramiteFiltroDto.TramiteId, out int tramiteId)) return "Id Tramite no válido";
                if (!int.TryParse(tramiteFiltroDto.ProyectoId, out int proyectoId)) return "Id Proyecto no válido";

                var tramiteProyectoCollection = Contexto.Proyectos.Where(a => a.TramiteId == tramiteId && a.ProyectoId == proyectoId).ToArray();
                if (tramiteProyectoCollection.Length == 0) return "Error al Eliminar asociación el tramite no contiene ningun proyecto asociado";
                if (tramiteProyectoCollection.Length != 1) return "Error al Eliminar asociación el tramite contiene mas de un proyecto asociado";
                Contexto.Proyectos.Remove(tramiteProyectoCollection[0]);
                Contexto.SaveChanges();

                return "Desasociación Exitosa";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramite(string bpin, int tramiteId)
        {
            List<proyectoAsociarTramite> resultado = new List<proyectoAsociarTramite>();
            var lista = Contexto.uspGetObtenerProyectosParaAsociarTramite(bpin, tramiteId).ToList();

            resultado = lista
                .ConvertAll(x => new proyectoAsociarTramite
                {
                    ProyectoId = x.ProyectoId,
                    BPIN = x.BPIN,
                    PeriodoProyectoId = x.PeriodoProyectoId,
                    NombreProyecto = x.NombreProyecto,
                    Accion = x.Accion,
                    TipoProyecto = x.TipoProyecto,
                    EntidadId = x.EntidadId,
                    NombreEntidad = x.NombreEntidad
                });
            return resultado;
        }

        public string AsociarProyectoVFO(proyectoAsociarTramite proyectoDto, string usuario)
        {
            try
            {
                var respuesta = new TramitesResultado();
                var tramiteProyectoCollection = Contexto.Proyectos.Where(a => a.TramiteId == proyectoDto.TramiteId && a.Estado == true).ToArray();
                if (tramiteProyectoCollection.Length > 0) return "Error al asociar el proyecto al tramite, ya existe una asociación";



                using (var dbContextTransaction = Contexto.Database.BeginTransaction())
                {
                    try
                    {
                        Proyectos proyecto = new Proyectos
                        {
                            TramiteId = proyectoDto.TramiteId,
                            ProyectoId = Convert.ToInt32(proyectoDto.ProyectoId),
                            EntidadId = proyectoDto.EntidadId,
                            PeriodoProyectoId = proyectoDto.PeriodoProyectoId,
                            Accion = proyectoDto.Accion,
                            Estado = true,
                            TipoProyecto = proyectoDto.TipoProyecto,
                            NombreProyecto = proyectoDto.NombreProyecto,
                            FechaCreacion = DateTime.Now,
                            CreadoPor = usuario
                        };

                        Contexto.Proyectos.Add(proyecto);
                        Contexto.SaveChanges();
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta.Exito.ToString();
                    }
                    catch (ServiciosNegocioException)
                    {
                        dbContextTransaction.Rollback();
                        respuesta.Mensaje = "Se genero una excepción interna en la base de datos al intentar registrar la asociación";
                        return respuesta.Mensaje;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return ex.Message;
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoTramite(int TramiteId)
        {
            var result = Contexto.uspGetObtenerProyectoTramiteVigencia(TramiteId).FirstOrDefault();
            //IEnumerable<ProyectosEnTramiteDto> proyectos = null;
            DatosProyectoTramiteDto proyecto;
            if (result != null)
            {
                proyecto = new DatosProyectoTramiteDto
                {
                    Sector = result.Sector,
                    NombreEntidad = result.NombreEntidad,
                    BPIN = result.BPIN,
                    NombreProyecto = result.NombreProyecto,
                    ProyectoId = result.ProyectoId,
                    EntidadId = result.EntidadId,
                    CodigoPresupuestal = result.CodigoPresupuestal,
                    VigenciaInicial = result.VigenciaInicial,
                    VigenciaFinal = result.VigenciaFinal,
                    ValorInicialNacion = result.ValorInicialNacion,
                    ValorInicialPropios = result.ValorInicialPropios,
                    ValorVigenteNacion = result.ValorVigenteNacion,
                    ValorVigentePropios = result.ValorVigentePropios,
                    ValorDisponibleNacion = result.ValorDisponibleNacion,
                    ValorDisponiblePropios = result.ValorDisponiblePropios,
                    ValorVigenciaFuturaNacion = result.ValorVigenciaFuturaNacion,
                    ValorVigenciaFuturaPropios = result.ValorVigenciaFuturaPropios,
                    TramiteId = result.TramiteId,
                    FechaFinal=result.FechaFinal

                };
            }
            else
            {
                proyecto = new DatosProyectoTramiteDto();
            }

            return proyecto;
        }

        public CartaConcepto ObtenerDetalleCartaConcepto(int tramiteId)
        {
            return Contexto.CartaConcepto.FirstOrDefault(w => w.TramiteId == tramiteId);
        }


        #region Vigencias Futuras

        public InformacionPresupuestalVlrConstanteDto ObtenerInformacionPresupuestalVlrConstanteVF(int TramiteId)
        {
            try
            {
                var UbicacionesDto = Contexto.uspGetVigenciaFuturaVlrConstante_JSON(TramiteId).SingleOrDefault();
                return JsonConvert.DeserializeObject<InformacionPresupuestalVlrConstanteDto>(UbicacionesDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ObtenerDatosCronograma(Guid instanciaId)
        {
            var listadoCronogramas = Contexto.UspGetActividadesCronograma_JSON(instanciaId).SingleOrDefault();
            return listadoCronogramas;
        }

        public IEnumerable<JustificacionPasoDto> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            List<JustificacionPasoDto> listadoRetorno = new List<JustificacionPasoDto>();
            var result = Contexto.uspGetPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel);

            if (result == null)
                return listadoRetorno;
            var listaResultadoSp = result.ToList();

            foreach (var paso in listaResultadoSp)
            {
                if (listadoRetorno.Where(x => x.Paso == paso.paso).Count() == 0)
                {
                    if (paso.JustificacionRespuesta != null)
                    {
                        JustificacionPasoDto PasoDto = new JustificacionPasoDto();
                        PasoDto.Paso = paso.paso;
                        PasoDto.NombreUsuario = paso.NombreUsuario;
                        PasoDto.Cuenta = paso.Cuenta;
                        PasoDto.FechaEnvio = paso.FechaEnvio;
                        listadoRetorno.Add(PasoDto);
                    }
                }
            }

            foreach (var pasos in listadoRetorno)
            {
                pasos.justificaciones = new List<JustificacionTramiteProyectoDto>();
                foreach (var j in listaResultadoSp.Where(x => x.paso == pasos.Paso))
                {
                    if (j.JustificacionRespuesta != null)
                    {
                        JustificacionTramiteProyectoDto justificacion = new JustificacionTramiteProyectoDto();
                        justificacion.TramiteId = j.TramiteId;
                        justificacion.ProyectoId = j.ProyectoId;
                        justificacion.JustificacionPreguntaId = j.JustificacionPreguntaId;
                        justificacion.JustificacionPregunta = j.JustificacionPregunta;
                        justificacion.OrdenJustificacionPregunta = j.OrdenJustificacionPregunta;
                        justificacion.JustificacionRespuesta = j.JustificacionRespuesta;
                        justificacion.ObservacionPregunta = j.ObservacionPregunta;
                        justificacion.OpcionesRespuesta = j.OpcionesRespuesta;
                        justificacion.Tematica = j.Tematica;
                        justificacion.OrdenTematica = j.OrdenTematica;
                        justificacion.NombreRol = j.NombreRol;
                        justificacion.NombreNivel = j.NombreNivel;
                        justificacion.CuestionarioId = j.CuestionarioId;
                        justificacion.Usuario = j.usuario;
                        justificacion.FechaEnvio = j.FechaEnvio;
                        justificacion.Paso = j.paso;
                        justificacion.NombreUsuario = j.NombreUsuario;
                        justificacion.Cuenta = j.Cuenta;

                        pasos.justificaciones.Add(justificacion);
                    }
                }
            }


            return listadoRetorno;
        }

        public InformacionPresupuestalValoresDto ObtenerInformacionPresupuestalValores(int TramiteId)
        {
            try
            {
                var UbicacionesDto = Contexto.uspGetAprobacionVigenciasFuturas_JSON(TramiteId).SingleOrDefault();
                return JsonConvert.DeserializeObject<InformacionPresupuestalValoresDto>(UbicacionesDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario)
        {
            string resultado = string.Empty;
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    var result = Contexto.uspPostAprobacionVigenciasFuturas(JsonUtilidades.ACadenaJson(informacionPresupuestalValoresDto), usuario, errorValidacionNegocio).SingleOrDefault();


                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";
                        dbContextTransaction.Commit();
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado = mensajeError;
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

        #endregion Vigencias Futuras

        public List<EnvioSubDireccionDto> ObtenerSolicitarConceptoPorTramite(int tramiteId)
        {
            List<EnvioSubDireccionDto> lista = new List<EnvioSubDireccionDto>();
            lista = Contexto.uspGetObtenerSolicitarConceptoPorTramite(tramiteId).ToList()
                .ConvertAll(x => new EnvioSubDireccionDto
                {
                    Enviado = x.Enviado.HasValue ? x.Enviado.Value : false,
                    Id = x.Id.HasValue ? x.Id.Value : 0,
                    IdUsuarioDNP = x.IdUsuarioDNP,
                    NombreEntidad = x.Entidad,
                    NombreUsuarioDNP = x.NombreUsuario,
                    Correo = x.Correo,
                    Activo = x.Activo.HasValue ? x.Activo.Value : false,
                    FechaCreacion = x.Fecha.HasValue ? x.Fecha.Value : DateTime.MinValue,
                    EntityTypeCatalogOptionId = !string.IsNullOrEmpty(x.EntityTypeCatalogOptionId) ? Convert.ToInt32(x.EntityTypeCatalogOptionId) : 0,
                    ParentId = !string.IsNullOrEmpty(x.ParentId) ? Convert.ToInt32(x.ParentId) : 0,
                    IdUsuarioDNPQueEnvia = x.IdUsuarioDNPQueEnvia,
                    NombreUsuarioQueEnvia = x.NombreUsuarioQueEnvia,
                    FechaEntrega = x.FechaEntrega.HasValue ? x.FechaEntrega.Value : DateTime.MinValue
                });

            return lista;
        }

        public List<TramiteDeflactoresDto> GetTramiteDeflactores()
        {

            try
            {
                var deflactores = new List<TramiteDeflactoresDto>();
                string jsonString = Contexto.uspGetDeflactores().SingleOrDefault();

                deflactores = JsonConvert.DeserializeObject<List<TramiteDeflactoresDto>>(jsonString);

                return deflactores;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public List<Dominio.Dto.Tramites.TramiteProyectoDto> GetProyectoTramite(int ProyectoId, int TramiteId)
        {

            try
            {
                var proyectoTramite = new List<Dominio.Dto.Tramites.TramiteProyectoDto>();
                string jsonString = Contexto.uspGetProyectoTramite(ProyectoId, TramiteId).SingleOrDefault();

                proyectoTramite = JsonConvert.DeserializeObject<List<Dominio.Dto.Tramites.TramiteProyectoDto>>(jsonString);

                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ActualizaVigenciaFuturaProyectoTramite(Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto, string usuario)
        {
            string resultado = string.Empty;
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    var result = Contexto.uspPostActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto.ProyectoId, tramiteProyectoDto.TramiteId, tramiteProyectoDto.EsConstante, tramiteProyectoDto.AnioBase, usuario, errorValidacionNegocio).SingleOrDefault();


                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";
                        dbContextTransaction.Commit();
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado = mensajeError;
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

        public VigenciaFuturaCorrienteDto GetFuentesFinanciacionVigenciaFuturaCorriente(string Bpin)
        {
            try
            {
                var proyectoTramite = new VigenciaFuturaCorrienteDto();
                string jsonString = Contexto.FuentesFinanciacionVigenciaFuturaCorriente_JSON(Bpin).SingleOrDefault();
                proyectoTramite = JsonConvert.DeserializeObject<VigenciaFuturaCorrienteDto>(jsonString);
                bool cumple = false;

                if (proyectoTramite.ValorTotalVigente > proyectoTramite.ValorPorcentaje)
                {
                    cumple = true;
                }

                int AnioActual = DateTime.Now.Year;

                proyectoTramite.cumple = cumple;
                proyectoTramite.AnioInicio = proyectoTramite.AñoInicio;
                proyectoTramite.AnioFin = proyectoTramite.AñoFin;
                double? valorTotalVigenteFutura = 0;
                foreach (var item in proyectoTramite.Fuentes)
                {
                    double? valorTotalVigenciaFutura = 0;
                    double? valorTotalVigenciaFuturas = 0;
                    item.Vigencias = item.Vigencias.OrderBy(x => x.Vigencia).ToList();
                    foreach (var item2 in item.Vigencias)
                    {
                        item2.ValorVigenteFuturaOriginal = item2.ValorVigenteFutura;
                        valorTotalVigenciaFutura = valorTotalVigenciaFutura + item2.ValorVigenteFutura;

                        if (item2.Vigencia > AnioActual)
                        {
                            valorTotalVigenciaFuturas = valorTotalVigenciaFuturas + item2.ValorVigenteFutura;
                        }
                    }
                    item.ValorTotalVigenciaFutura = valorTotalVigenciaFutura;
                    item.ValorTotalVigenciaFuturaOriginal = valorTotalVigenciaFutura;
                    item.ValorTotalVigenciaFuturas = valorTotalVigenciaFuturas;
                    item.ValorTotalVigenciaFuturasOriginal = valorTotalVigenciaFuturas;

                    valorTotalVigenteFutura = valorTotalVigenteFutura + item.ValorTotalVigenciaFuturas;
                }
                proyectoTramite.ValorTotalVigenteFutura = valorTotalVigenteFutura;

                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public VigenciaFuturaConstanteDto GetFuentesFinanciacionVigenciaFuturaCoonstante(string Bpin, int AnioBase)
        {
            try
            {
                var proyectoTramite = new VigenciaFuturaConstanteDto();
                string jsonString = Contexto.FuentesFinanciacionVigenciaFuturaConstante_JSON(Bpin, AnioBase).SingleOrDefault();
                proyectoTramite = JsonConvert.DeserializeObject<VigenciaFuturaConstanteDto>(jsonString);
                bool cumple = false;

                if (proyectoTramite.ValorTotalVigente > proyectoTramite.ValorPorcentaje)
                {
                    cumple = true;
                }

                int AnioActual = DateTime.Now.Year;

                proyectoTramite.cumple = cumple;
                proyectoTramite.AnioInicio = proyectoTramite.AñoInicio;
                proyectoTramite.AnioFin = proyectoTramite.AñoFin;

                double? valorTotalVigenteFutura = 0;
                foreach (var item in proyectoTramite.Fuentes)
                {
                    double? valorTotalVigenciaFutura = 0;
                    double? valorTotalVigenciaFuturaCorriente = 0;
                    double? valorTotalVigenciaFuturas = 0;
                    item.Vigencias = item.Vigencias.OrderBy(x => x.Vigencia).ToList();
                    foreach (var item2 in item.Vigencias)
                    {
                        item2.ValorVigenteFuturaOriginal = item2.ValorVigenteFutura;
                        item2.ValorVigenteFuturaCorrienteOriginal = item2.ValorVigenteFuturaCorriente;
                        valorTotalVigenciaFutura = valorTotalVigenciaFutura + item2.ValorVigenteFutura;
                        valorTotalVigenciaFuturaCorriente = valorTotalVigenciaFuturaCorriente + item2.ValorVigenteFuturaCorriente;

                        if (item2.Vigencia > AnioActual)
                        {
                            valorTotalVigenciaFuturas = valorTotalVigenciaFuturas + item2.ValorVigenteFuturaCorriente;
                        }
                    }
                    item.ValorTotalVigenciaFutura = valorTotalVigenciaFutura;
                    item.ValorTotalVigenciaFuturaOriginal = valorTotalVigenciaFutura;
                    item.ValorTotalVigenciaFuturaCorriente = valorTotalVigenciaFuturaCorriente;
                    item.ValorTotalVigenciaFuturaCorrienteOriginal = valorTotalVigenciaFuturaCorriente;

                    item.ValorTotalVigenciaFuturas = valorTotalVigenciaFuturas;
                    item.ValorTotalVigenciaFuturasOriginal = valorTotalVigenciaFuturas;

                    valorTotalVigenteFutura = valorTotalVigenteFutura + item.ValorTotalVigenciaFuturas;
                }
                proyectoTramite.ValorTotalVigenteFutura = valorTotalVigenteFutura;

                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public AccionDto ObtenerAccionActualyFinal(int tramiteId, string bpin)
        {
            AccionDto respuesta = new AccionDto();
            try
            {
                var result = Contexto.uspGetAccionActualyFinal(tramiteId, bpin).SingleOrDefault();


                if (result != null)
                {
                    respuesta.IdAccionActual = result.AccionFlujoActualId.Value;
                    respuesta.IdAccionFinal = result.AccionFlujoFinalId.Value;
                    respuesta.NombreAccionActual = result.AccionFlujoActualNombre;
                    respuesta.NombreAccionFinal = result.AccionFlujoFinalNombre;
                    respuesta.IdFlujo = result.FlujoId.Value;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return respuesta;


        }
        public int EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid))
        {
            AccionDto respuesta = new AccionDto();
            var errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            try
            {
                var result = Contexto.uspPostEliminarPermisosAccionesUsuario(usuarioDestino, tramiteId, aliasNivel, InstanciaId, errorValidacionNegocio).SingleOrDefault();


                if (result != null)
                {
                    return result.HasValue ? result.Value : 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;


        }


        public VigenciaFuturaResponse ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(fuente);
                    var result = Contexto.uspPostActualizarVigenciaFuturaFuente(FuenteJson,
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(fuente);
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

        public VigenciaFuturaResponse ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(prod);
                    var result = Contexto.uspPostActualizarVigenciaFuturaProducto(FuenteJson,
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(prod);
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

        public TramitesResultado EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario)
        {

            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostEnviarRespuestaConceptoDireccionTecnicaTramite(tramiteId, usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
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

        public List<TramiteModalidadContratacionDto> ObtenerModalidadesContratacion(int? mostrar)
        {
            List<TramiteModalidadContratacionDto> lista = new List<TramiteModalidadContratacionDto>();
            try
            {
                var result = Contexto.UspGetModalidadesContratacion(mostrar).ToList();
                foreach (var item in result)
                {
                    lista.Add(new TramiteModalidadContratacionDto() { Id = item.Id, Nombre = item.Nombre });
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public ActividadPreContractualDto ActualizarActividadesCronograma(ActividadPreContractualDto actividades, string usuario)
        {

            try
            {
                ObjectParameter CronogramaId = new ObjectParameter("CronogramaId", typeof(int));
                ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

                var respuestaPreContractual = new List<ProyectoActividadCronogramaDto>();
                var respuestaContractual = new List<ProyectoActividadCronogramaDto>();

                if (actividades.ActividadesPreContractuales != null)
                {

                    //Actualiza precontractuales
                    foreach (var actividad in actividades.ActividadesPreContractuales)
                    {
                        var resultado = Contexto.UspPostEliminarActividadesCronograma(
                            actividad.CronogramaId,
                            actividades.ProyectoId,
                            actividades.TramiteId,
                            actividad.Actividad,
                            actividad.ActividadPreContractualId,
                            actividad.ModalidadContratacionId,
                            actividad.FechaInicial,
                            actividad.FechaFinal,
                            usuario,
                            false,
                            errorValidacionNegocio,
                            CronogramaId);
                        if (actividad.CronogramaId != CronogramaId.Value as int?)
                        {
                            actividad.CronogramaId = CronogramaId.Value as int?;
                        }
                        respuestaPreContractual.Add(actividad);
                    }
                }

                if (actividades.ActividadesContractuales != null)
                {

                    //Actualiza precontractuales
                    foreach (var actividad in actividades.ActividadesContractuales)
                    {
                        var resultado = Contexto.UspPostEliminarActividadesCronograma(
                            actividad.CronogramaId,
                            actividades.ProyectoId,
                            actividades.TramiteId,
                            actividad.Actividad,
                            actividad.ActividadPreContractualId,
                            actividad.ModalidadContratacionId,
                            actividad.FechaInicial,
                            actividad.FechaFinal,
                            usuario,
                            false,
                            errorValidacionNegocio,
                            CronogramaId);
                        if (actividad.CronogramaId != CronogramaId.Value as int?)
                        {
                            actividad.CronogramaId = CronogramaId.Value as int?;
                        }
                        respuestaContractual.Add(actividad);
                    }
                }

                if (actividades.EliminarActividadesContractuales != null)
                {

                    //Actualiza precontractuales
                    foreach (var actividad in actividades.EliminarActividadesContractuales)
                    {
                        var resultado = Contexto.UspPostEliminarActividadesCronograma(
                            actividad.CronogramaId,
                            actividades.ProyectoId,
                            actividades.TramiteId,
                            actividad.Actividad,
                            actividad.ActividadPreContractualId,
                            actividad.ModalidadContratacionId,
                            actividad.FechaInicial,
                            actividad.FechaFinal,
                            usuario,
                            true,
                            errorValidacionNegocio,
                            CronogramaId);
                        if (actividad.CronogramaId != CronogramaId.Value as int?)
                        {
                            actividad.CronogramaId = CronogramaId.Value as int?;
                        }
                        respuestaContractual.Add(actividad);
                    }
                }

                return new ActividadPreContractualDto()
                {
                    ProyectoId = actividades.ProyectoId,
                    ActividadesPreContractuales = respuestaPreContractual,
                    ActividadesContractuales = respuestaContractual
                };
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public ActividadPreContractualDto ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades)
        {

            try
            {
                var jsonString = Contexto.upsGetActividadesPreContratuales_JSON(ModalidadContratacionId, ProyectoId, TramiteId, eliminarActividades).SingleOrDefault();

                var actividades = JsonConvert.DeserializeObject<ActividadPreContractualDto>(jsonString);

                //return actividadesCronograma.Where(a => a.ModalidadContratacionId == ModalidadContratacionId && a.TramiteProyectoId == TramiteProyectoId).ToList(); 
                return actividades;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public ProductosConstantesVF GetProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase)
        {
            try
            {
                var productosConstantesVF = new ProductosConstantesVF();
                string jsonString = Contexto.ProductosVigenciaFuturaConstante_JSON(Bpin, TramiteId, AnioBase).SingleOrDefault();
                productosConstantesVF = JsonConvert.DeserializeObject<ProductosConstantesVF>(jsonString);

                int pagina = 1;
                int conteo = 0;
                double? TotalesVigenciaFuturaConstante = 0;

                if (productosConstantesVF.Deflactores != null)
                {
                    foreach (var item in productosConstantesVF.Deflactores)
                    {
                        if (conteo % 4 == 0)
                        {
                            pagina += 1;
                        }
                        item.Pagina = pagina;
                        conteo += 1;

                        foreach (var objetivos in productosConstantesVF.ResumenObjetivos)
                        {
                            foreach (var productos in objetivos.Productos)
                            {
                                double? ValorTotalVigenteFuturaProducto = 0;
                                double? ValorTotalVigenteFuturaProductoCorriente = 0;
                                foreach (var vigencias in productos.Vigencias)
                                {
                                    ValorTotalVigenteFuturaProducto += vigencias.VigenciaFuturaSolicitada;
                                    ValorTotalVigenteFuturaProductoCorriente += (vigencias.VigenciaFuturaSolicitada * vigencias.Deflactor);
                                    if (vigencias.Vigencia == item.AnioConstante)
                                    {
                                        item.ValorTotalVigenteFutura += vigencias.VigenciaFuturaSolicitada;
                                        item.ValorTotalVigenteFuturaCorriente += (vigencias.VigenciaFuturaSolicitada * vigencias.Deflactor);
                                    }
                                }
                                productos.ValorTotalVigenteFuturaProducto = ValorTotalVigenteFuturaProducto;
                                productos.ValorTotalVigenteFuturaProductoOriginal = ValorTotalVigenteFuturaProducto;
                                productos.ValorTotalVigenteFuturaProductoCorriente = ValorTotalVigenteFuturaProductoCorriente;
                                productos.ValorTotalVigenteFuturaProductoCorrienteOriginal = ValorTotalVigenteFuturaProductoCorriente;
                            }
                        }

                        TotalesVigenciaFuturaConstante += item.ValorTotalVigenteFutura;
                        productosConstantesVF.TotalesVigenciaFuturaConstante = TotalesVigenciaFuturaConstante;

                        foreach (var detalle in productosConstantesVF.DetalleObjetivos)
                        {
                            foreach (var detalleprod in detalle.Productos)
                            {
                                foreach (var detallevig in detalleprod.Vigencias)
                                {
                                    detallevig.TotalVigenciasFuturaSolicitadaOriginal = detallevig.TotalVigenciasFuturaSolicitada;
                                }
                            }
                        }

                    }
                }

                return productosConstantesVF;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public ProductosCorrientesVF GetProductosVigenciaFuturaCorriente(string Bpin, int TramiteId)
        {
            try
            {
                var productosCorrientesVF = new ProductosCorrientesVF();
                string jsonString = Contexto.ProductosVigenciaFuturaCorriente_JSON(Bpin).SingleOrDefault();
                productosCorrientesVF = JsonConvert.DeserializeObject<ProductosCorrientesVF>(jsonString);

                int pagina = 1;
                int conteo = 0;
                double? TotalesVigenciaFuturaCorriente = 0;

                if (productosCorrientesVF != null && productosCorrientesVF.AniosVigencias != null)
                {
                    foreach (var item in productosCorrientesVF.AniosVigencias)
                    {
                        if (conteo % 4 == 0)
                        {
                            pagina += 1;
                        }
                        item.Pagina = pagina;
                        conteo += 1;

                        foreach (var objetivos in productosCorrientesVF.ResumenObjetivos)
                        {
                            foreach (var productos in objetivos.Productos)
                            {
                                double? ValorTotalVigenteFuturaProducto = 0;
                                double? ValorTotalVigenteFuturaProductoCorriente = 0;
                                foreach (var vigencias in productos.Vigencias)
                                {
                                    ValorTotalVigenteFuturaProducto += vigencias.VigenciaFuturaSolicitada;
                                    ValorTotalVigenteFuturaProductoCorriente += vigencias.VigenciaFuturaSolicitada;
                                    if (vigencias.Vigencia == item.AnioConstante)
                                    {
                                        item.ValorTotalVigenteFutura += vigencias.VigenciaFuturaSolicitada;
                                    }
                                }
                                productos.ValorTotalVigenteFuturaProducto = ValorTotalVigenteFuturaProducto;
                                productos.ValorTotalVigenteFuturaProductoOriginal = ValorTotalVigenteFuturaProducto;

                            }
                        }

                        TotalesVigenciaFuturaCorriente += item.ValorTotalVigenteFutura;
                        productosCorrientesVF.TotalesVigenciaFuturaCorriente = TotalesVigenciaFuturaCorriente;
                        foreach (var detalle in productosCorrientesVF.DetalleObjetivos)
                        {
                            foreach (var detalleprod in detalle.Productos)
                            {
                                foreach (var detallevig in detalleprod.Vigencias)
                                {
                                    detallevig.TotalVigenciasFuturaSolicitadaOriginal = detallevig.TotalVigenciasFuturaSolicitada;
                                }
                            }
                        }

                    }
                }

                return productosCorrientesVF;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId)
        {
            var roltmp = new Guid(rolId);
            var niveltmp = new Guid(nivelId);
            if (roltmp != new Guid())
            {
                var rta = Contexto.uspGetTipoDocumentosSoportePorNivel(tipoTramiteId, niveltmp, roltmp).ToList();
                return rta.ConvertAll(x => new TipoDocumentoTramiteDto
                {
                    Id = x.Id,
                    TipoDocumentoId = x.Id,
                    TipoDocumento = x.TipoDocumento,
                    TipoTramiteId = x.TipoTramiteId,
                    Obligatorio = x.Obligatorio
                });


            }
            else
            {

                return (from TDT in Contexto.TipoDocumentoTramite
                        join TTR in Contexto.TipoDocumentoTramiteRol on TDT.Id equals TTR.TipoDocumentoTramite.Id
                        join TD in Contexto.TipoDocumento on TDT.TipoDocumento.Id equals TD.Id
                        where TDT.TipoTramiteId == tipoTramiteId //&& TTR.IdRol == Rol
                        select new TipoDocumentoTramiteDto
                        {
                            Id = TDT.Id,
                            TipoDocumentoId = TD.Id,
                            TipoDocumento = TD.Descripcion,
                            TipoTramiteId = TDT.TipoTramiteId,
                            Obligatorio = TDT.Obligatorio
                        }).ToList();
            }
        }

        public IEnumerable<DatosUsuarioDto> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia)
        {
            List<DatosUsuarioDto> lista;
            var rta = Contexto.uspGetDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia).ToList();
            lista = rta.ConvertAll(x => new DatosUsuarioDto
            {
                IdUsuario = x.IdUsuario.HasValue ? x.IdUsuario.Value : new Guid(),
                NombreUsuario = x.NombreUsuario,
                Cuenta = x.Cuenta,
                EntidadId = x.EntidadId,
                Entidad = x.Entidad,
                RolId = x.rolId.HasValue ? x.rolId.Value : new Guid()
            }) ;
            return lista;
        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramiteLeyenda(string bpin, int tramiteId)
        {
            List<proyectoAsociarTramite> resultado = new List<proyectoAsociarTramite>();
            var lista = Contexto.uspGetObtenerProyectosParaAsociarTramiteLeyenda(bpin, tramiteId).ToList();

            resultado = lista
                .ConvertAll(x => new proyectoAsociarTramite
                {
                    ProyectoId = x.ProyectoId,
                    BPIN = x.BPIN,
                    PeriodoProyectoId = x.PeriodoProyectoId,
                    NombreProyecto = x.NombreProyecto,
                    Accion = x.Accion,
                    TipoProyecto = x.TipoProyecto
                });
            return resultado;
        }

        public ModificacionLeyendaDto ObtenerModificacionLeyenda(int tramiteId, int ProyectoId)
        {
            ModificacionLeyendaDto resultado = new ModificacionLeyendaDto();
            var tramiteProyectoId = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == tramiteId && tramiteProyecto.ProyectoId == ProyectoId).FirstOrDefault()?.Id;


            var lista = Contexto.uspGetObtenerModificacionProyectoTramiteLeyenda(tramiteProyectoId).ToList();

            resultado = lista
                .ConvertAll(x => new ModificacionLeyendaDto
                {
                    BPIN = x.BPIN,
                    CodigoPresupuestal = x.CodigoPresupuestal,
                    NombreProyecto = x.NombreProyectoOriginal,
                    NombreProyectoOriginal = x.NombreProyectoOriginal,
                    NombreProyectoModificacion = x.NombreProyectoModificacion,
                    ErrorAritmetico = x.ErrorAritmetico,
                    ErrorTranscripcion = x.ErrorTranscripcion,
                    Justificacion = x.Justificacion,
                    Programa = x.Programa,
                    Subprograma = x.Subprograma,
                    TramiteProyectoId = tramiteProyectoId
                }).FirstOrDefault();
            return resultado;
        }

        public string ActualizaModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario)
        {
            string resultado = string.Empty;
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    var result = Contexto.uspPostModificacionLeyenda(modificacionLeyendaDto.TramiteProyectoId,modificacionLeyendaDto.tipoUpdate, modificacionLeyendaDto.NombreProyectoModificacion,
                                                                        modificacionLeyendaDto.Justificacion, modificacionLeyendaDto.ErrorAritmetico, modificacionLeyendaDto.ErrorTranscripcion,
                                                                        usuario, errorValidacionNegocio).SingleOrDefault();


                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";
                        dbContextTransaction.Commit();
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado = mensajeError;
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

        public List<EntidadCatalogoDTDto> ObtenerListaDireccionesDNP(Guid IdEntidad)
        {
            List<EntidadCatalogoDTDto> resultado = new List<EntidadCatalogoDTDto>();
            try
            {
                resultado = Contexto.uspGetObtenerDireccionesTecnicas(IdEntidad)
                    .ToList()
                    .ConvertAll(et => new EntidadCatalogoDTDto
                    {
                        EntityTypeCatalogOptionId = et.Id,
                        Name = et.Name,
                        EntityTypeId = et.EntityTypeId,
                        ParentId = et.ParentId,
                        Code = et.Code,
                        IsActive = et.IsActive
                    }).OrderBy(x => x.Name).ToList();

                return resultado;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<EntidadCatalogoDTDto> ObtenerListaSubdireccionesPorParentId(int IdEntityType)
        {
            List<EntidadCatalogoDTDto> resultado = new List<EntidadCatalogoDTDto>();
            try
            {
                resultado = Contexto.uspGetObtenerSubdireccionesTecnicas(IdEntityType)
                    .ToList()
                    .ConvertAll(et => new EntidadCatalogoDTDto
                    {
                        EntityTypeCatalogOptionId = et.Id,
                        Name = et.Name,
                        EntityTypeId = et.EntityTypeId,
                        ParentId = et.ParentId,
                        Code = et.Code,
                        IsActive = et.IsActive
                    }).OrderBy(x => x.Name).ToList();

                return resultado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TramitesResultado BorrarFirma(FileToUploadDto parametros)
        {
            var respuesta = new TramitesResultado();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var registro = Contexto.CartaConceptoFirma.Where(x => x.Usuario == parametros.UsuarioId &&  x.Activo).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.Activo = false;
                        registro.ModificadoPor = parametros.UsuarioId;
                        registro.FechaModificacion = DateTime.Now;
                        Contexto.SaveChanges();
                        respuesta.Exito = true;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "No existe firma para el usuario";
                    }

                    respuesta.Exito = true;
                    dbContextTransaction.Commit();
                }
                catch (ServiciosNegocioException)
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

            return respuesta;
        }

        public ProyectosCartaDto ObtenerProyectosCartaTramite(int tramiteId)
        {
            ProyectosCartaDto resultado = new ProyectosCartaDto();

            var lista = Contexto.uspGetProyectosCartaTramiteVfo(tramiteId).ToList();

            resultado = lista
                .ConvertAll(x => new ProyectosCartaDto
                {
                    Bpin = x.Bpin,
                    CodigoEntidad = x.CodigoEntidad,
                    CodigoPrograma= x.CodigoPrograma,
                    CodigoSubprograma = x.CodigoSubprograma,
                    ConsecutivoCodigoPresupuestal = x.ConsecutivoCodigoPresupuestal,
                    Entidad = x.Entidad,
                    NombreProyecto = x.NombreProyecto,
                    Programa = x.Programa,
                    Subprogramal = x.Subprogramal,
                    esConstante = x.esConstante
                }).FirstOrDefault();
            return resultado;
        }

        public DetalleCartaConceptoALDto ObtenerDetalleCartaAL(int tramiteId)
        {
            DetalleCartaConceptoALDto resultado = new DetalleCartaConceptoALDto();

            var lista = Contexto.uspGetDetalleCartaAclaracionLeyendaProyecto(tramiteId).ToList();

            resultado = lista
                .ConvertAll(x => new DetalleCartaConceptoALDto
                {
                    Aclaracion = x.Aclaracion,
                    NombreActual = x.NombreActual
                }).FirstOrDefault();
            return resultado;
        }

        public int ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId)
        {

            try
            {
                var resultado =   Contexto.uspGetDevolucionAmpliarConcepto(ProyectoId, TramiteId);

                if (resultado == null)
                    return 0;

                return Convert.ToInt32(resultado);

            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId)
        {
            var result = Contexto.uspGetObtenerDatosProyectoConceptoPorInstancia(instanciaId).FirstOrDefault();
            DatosProyectoTramiteDto proyecto;
            if (result != null)
            {
                proyecto = new DatosProyectoTramiteDto
                {
                    Sector = result.Sector,
                    EntidadDestino = result.EntidadDestino,
                    BPIN = result.BPIN,
                    NombreProyecto = result.NombreProyecto,
                    ProyectoId = result.ProyectoId.HasValue ? result.ProyectoId.Value : 0,
                    EstadoProyecto = result.EstadoProyecto,
                    MacroProceso = result.MacroProceso,
                    FechaInicioProceso = result.FechaInicioProceso,
                    EstadoProceso = result.EstadoProceso,
                    NombrePaso = result.NombrePaso,
                    FechaInicioPaso = result.FechaInicioPaso,
                    Proceso = result.Proceso
                };
            }
            else
            {
                proyecto = new DatosProyectoTramiteDto();
            }

            return proyecto;
        }

        public List<TramiteLiberacionVfDto> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var liberaciones = new List<TramiteLiberacionVfDto>();
                string jsonString = Contexto.uspGetLiberacionVF(ProyectoId, TramiteId).SingleOrDefault();

                if (string.IsNullOrEmpty(jsonString))
                    return null;

                liberaciones = JsonConvert.DeserializeObject<List<TramiteLiberacionVfDto>>(jsonString);

                decimal TotalCorrientesAutorizadosNacion = 0;
                decimal TotalCorrientesAutorizadosPropios = 0;
                decimal TotalCorrientesUtilizadosNacion = 0;
                decimal TotalCorrientesUtilizadosPropios = 0;

                decimal TotalConstantesAutorizadosNacion = 0;
                decimal TotalConstantesAutorizadosPropios = 0;
                decimal TotalConstantesUtilizadosNacion = 0;
                decimal TotalConstantesUtilizadosPropios = 0;

                decimal TotalConstantesCorrientesAutorizadosNacion = 0;
                decimal TotalConstantesCorrientesAutorizadosPropios = 0;

                foreach (var item in liberaciones)
                {
                    foreach (var item2 in item.TramitesALiberar)
                    {
                        item2.CodigoAutorizacionOriginal = item2.CodigoAutorizacion;
                        item2.FechaAutorizacionOriginal = item2.FechaAutorizacion;
                        item2.EstadoAutorizacionOriginal = item2.EstadoAutorizacion;

                        if (string.IsNullOrEmpty(item2.CodigoAutorizacion) || item2.CodigoAutorizacion == "0")
                        {
                            item.HabilitaValoresSolicitados = false;
                        }

                        if (!item2.EsConstante)
                        {
                            foreach (var item3 in item2.ValoresCorrientesAutorizaLiberacion)
                            {
                                item3.UtilizadoNacionOriginal = item3.UtilizadoNacion;
                                item3.UtilizadoPropiosOriginal = item3.UtilizadoPropios;

                                TotalCorrientesAutorizadosNacion += item3.AprobadoNacion;
                                TotalCorrientesAutorizadosPropios += item3.AprobadoPropios;
                                TotalCorrientesUtilizadosNacion += item3.UtilizadoNacion;
                                TotalCorrientesUtilizadosPropios += item3.UtilizadoPropios;
                            }

                            item2.TotalCorrientesAutorizadosNacion = TotalCorrientesAutorizadosNacion;
                            item2.TotalCorrientesAutorizadosPropios = TotalCorrientesAutorizadosPropios;
                            item2.TotalCorrientesUtilizadosNacion = TotalCorrientesUtilizadosNacion;
                            item2.TotalCorrientesUtilizadosPropios = TotalCorrientesUtilizadosPropios;
                        }

                        if (item2.EsConstante)
                        {
                            foreach (var item3 in item2.ValoresConstantesAutorizaLiberacion)
                            {
                                item3.UtilizadoConstanteNacionOriginal = item3.UtilizadoConstanteNacion;
                                item3.UtilizadoConstantePropiosOriginal = item3.UtilizadoConstantePropios;

                                TotalConstantesAutorizadosNacion += item3.AprobadoConstanteNacion;
                                TotalConstantesAutorizadosPropios += item3.AprobadoConstantePropios;
                                TotalConstantesUtilizadosNacion += item3.UtilizadoConstanteNacion;
                                TotalConstantesUtilizadosPropios += item3.UtilizadoConstantePropios;

                                TotalConstantesCorrientesAutorizadosNacion += item3.AprobadoCorrienteNacion;
                                TotalConstantesCorrientesAutorizadosPropios += item3.AprobadoCorrientePropios;
                            }

                            item2.TotalConstantesAutorizadosNacion = TotalConstantesAutorizadosNacion;
                            item2.TotalConstantesAutorizadosPropios = TotalConstantesAutorizadosPropios;
                            item2.TotalConstantesUtilizadosNacion = TotalConstantesUtilizadosNacion;
                            item2.TotalConstantesUtilizadosPropios = TotalConstantesUtilizadosPropios;

                            item2.TotalConstantesCorrientesAutorizadosNacion = TotalConstantesCorrientesAutorizadosNacion;
                            item2.TotalConstantesCorrientesAutorizadosPropios = TotalConstantesCorrientesAutorizadosPropios;
                        }

                    }
                }

                return liberaciones;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public VigenciaFuturaResponse InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(autorizacion);
                    var result = Contexto.uspPostSetAutorizacionVigenciaFutura(FuenteJson,
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(autorizacion);
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        return resultado;
                        //throw new ServiciosNegocioException(mensajeError);
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

        public VigenciaFuturaResponse InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(autorizacion);
                    var result = Contexto.uspPostSetAutorizacionVigenciasFuturasValores(FuenteJson,
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(autorizacion);
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        return resultado;
                        //throw new ServiciosNegocioException(mensajeError);
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

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            List<ProyectoTramiteFuenteDto> lista = new List<ProyectoTramiteFuenteDto>();
            try
            {

                var listatmp = Contexto.uspGetProyectoFuentePresupuestalPorTramite_Traslado(tramiteId).ToList();
                //List<uspGetProyectoFuentePresupuestalPorTramite_Traslado_Result> rta = listatmp;
                foreach (var item in listatmp)
                {
                    if(lista.Where(x => x.ProyectoId == item.proyectoId).FirstOrDefault() == null)
                    {
                        ProyectoTramiteFuenteDto prop = new ProyectoTramiteFuenteDto();
                        prop.ProyectoId = item.proyectoId.Value;
                        prop.BPIN = item.Bpin;
                        prop.NombreProyecto = item.NombreProyecto;
                        prop.Operacion = item.tipoProyecto;
                        prop.TramiteProyectoId = item.TramiteProyectoId.HasValue ? item.TramiteProyectoId.Value : 0;
                        prop.ValorTotalNacion = item.ValorTotalNacion.HasValue ? item.ValorTotalNacion.Value : 0;
                        prop.ValorTotalPropios = item.ValorTotalPropios.HasValue ? item.ValorTotalPropios.Value : 0;
                        lista.Add(prop);
                    }
                }
               

                foreach (var item in lista)
                {
                    item.ListaFuentes = new List<FuenteFinanciacionDto>();
                    var listafuenteTmp = listatmp.ToList().Where(x => x.proyectoId == item.ProyectoId);
                    foreach (var itemFuente in listafuenteTmp)
                    {
                        FuenteFinanciacionDto fte = new FuenteFinanciacionDto
                        {
                            FuenteId = itemFuente.FuenteId,
                            NombreCompleto = itemFuente.Nombre,
                            GrupoRecurso = itemFuente.Origen,
                            TipoValorContracreditoCSF = itemFuente.idTipoValorContracreditoCSF.HasValue ? itemFuente.idTipoValorContracreditoCSF.Value : 0,
                            TipoValorContracreditoSSF = itemFuente.idTipoValorContracreditoSSF.HasValue ? itemFuente.idTipoValorContracreditoSSF.Value : 0,
                            ValorIncialCSF = itemFuente.ValorInicialCSF,
                            ValorIncialSSF = itemFuente.ValorInicialSSF,
                            ValorVigenteSSF = itemFuente.ValorVigenteSSF,
                            ValorVigenteCSF = itemFuente.ValorVigenteCSF,
                            ValorContracreditoCSF = itemFuente.ValorContracreditoCSF,
                            ValorContracreditoSSF = itemFuente.ValorContracreditoSSF
                        };
                        item.ListaFuentes.Add(fte);
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);

            }

        }
        public List<EntidadesAsociarComunDto> ObtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto)
        {
            try
            {
                var entidades = Contexto.uspGetObtenerEntidadesParaAsociar(InstanciaId, AccionTramiteProyecto).ToList();
                var listaEntidades = new List<EntidadesAsociarComunDto>();
                foreach(var entity in entidades)
                {
                    var entidad = new EntidadesAsociarComunDto { Id = entity.Id, NombreEntidad = entity.NombreEntidad };
                    listaEntidades.Add(entidad);
                }
                return listaEntidades;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public int ValidacionPeriodoPresidencial(int TramiteId)
        {
            ObjectParameter resultado = new ObjectParameter("Resultado", typeof(int));
            try
            {
                var entidades = Contexto.uspGetValidacion_excedePeriodoPresidencial_Resultado(TramiteId, resultado);

                return Convert.ToInt16(resultado.Value);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar,
                                                       string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostGuardarMontosTramite(JsonUtilidades.ACadenaJson(parametrosGuardar),
                                                         usuario,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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

        public TramitesResultado GuardarTramiteInformacionPresupuestalPorProyectos(List<ProyectoTramiteFuenteDto> parametrosGuardar,
                                                         string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new TramitesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostActualizaValoresInfomracionPresupuestal(JsonUtilidades.ACadenaJson(parametrosGuardar),
                                                         usuario,
                                                         null,
                                                         null,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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

        public List<DatosProyectoTramiteDto> ObtenerDatosProyectosPorTramite(int TramiteId)
        {
            List<DatosProyectoTramiteDto> proyectos = new List<DatosProyectoTramiteDto>();

            try
            {
                proyectos = Contexto.uspGetObtenerProyectoTramiteVigencia(TramiteId)
                    .ToList()
                    .ConvertAll(x => new DatosProyectoTramiteDto
                    {
                        Sector = x.Sector,
                        NombreEntidad = x.NombreEntidad,
                        BPIN = x.BPIN,
                        NombreProyecto = x.NombreProyecto,
                        ProyectoId = x.ProyectoId,
                        EntidadId = x.EntidadId,
                        CodigoPresupuestal = x.CodigoPresupuestal,
                        VigenciaInicial = x.VigenciaInicial,
                        VigenciaFinal = x.VigenciaFinal,
                        ValorInicialNacion = x.ValorInicialNacion,
                        ValorInicialPropios = x.ValorInicialPropios,
                        ValorVigenteNacion = x.ValorVigenteNacion,
                        ValorVigentePropios = x.ValorVigentePropios,
                        ValorDisponibleNacion = x.ValorDisponibleNacion,
                        ValorDisponiblePropios = x.ValorDisponiblePropios,
                        ValorVigenciaFuturaNacion = x.ValorVigenciaFuturaNacion,
                        ValorVigenciaFuturaPropios = x.ValorVigenciaFuturaPropios,
                        TramiteId = x.TramiteId,
                        FechaFinal = x.FechaFinal
                    });

                return proyectos;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<ResumenLiberacionVfDto> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var resumen = new List<ResumenLiberacionVfDto>();
                string jsonString = Contexto.uspGetResumenAutorizacionesVerificadasVF(ProyectoId, TramiteId).SingleOrDefault();

                if (string.IsNullOrEmpty(jsonString))
                    return resumen;

                resumen = JsonConvert.DeserializeObject<List<ResumenLiberacionVfDto>>(jsonString);

                decimal TotalAutorizadosNacion = 0;
                decimal TotalAutorizadosPropios = 0;
                decimal TotalUtilizadosNacion = 0;
                decimal TotalUtilizadosPropios = 0;

                foreach (var item in resumen)
                {
                    foreach (var item2 in item.ValoresAutorizadosUtilizados)
                    {
                        TotalAutorizadosNacion += item2.AprobadosNacion;
                        TotalAutorizadosPropios += item2.AprobadosNPropios;
                        TotalUtilizadosNacion += item2.UtilizadoNacion;
                        TotalUtilizadosPropios += item2.UtilizadoPropios;
                    }
                    item.TotalAutorizadosNacion = TotalAutorizadosNacion;
                    item.TotalAutorizadosPropios = TotalAutorizadosPropios;
                    item.TotalUtilizadosNacion = TotalUtilizadosNacion;
                    item.TotalUtilizadosPropios = TotalUtilizadosPropios;
                }

                return resumen;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public ValoresUtilizadosLiberacionVfDto ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var resumen = new ValoresUtilizadosLiberacionVfDto();
                Guid InstanciaId = new Guid();
                string jsonString = ContextoOnlySP.uspGetValoresUtilizadosPorProducto(TramiteId, ProyectoId, InstanciaId).SingleOrDefault();

                if (string.IsNullOrEmpty(jsonString))
                    return null;

                resumen = JsonConvert.DeserializeObject<ValoresUtilizadosLiberacionVfDto>(jsonString);

                decimal TotalValorSolicitadoR_ = 0;
                decimal TotalValorTotalUtilizadosR_ = 0;
                decimal TotalValorUtilizadoPorProductosR_ = 0;

                if (resumen.TramitesVerificadosCorrientes != null)
                {
                    foreach (var item in resumen.TramitesVerificadosCorrientes)
                    {
                        foreach (var item2 in item.ResumenValoresCorrientes)
                        {
                            TotalValorSolicitadoR_ += item2.ValorSolicitado;
                            TotalValorTotalUtilizadosR_ += item2.ValorTotalUtilizados;
                            TotalValorUtilizadoPorProductosR_ += item2.ValorUtilizadoPorProductos;
                        }

                        item.TotalValorSolicitado = TotalValorSolicitadoR_;
                        item.TotalValorTotalUtilizados = TotalValorTotalUtilizadosR_;
                        item.TotalValorUtilizadoPorProductos = TotalValorUtilizadoPorProductosR_;

                        if (item.DetalleObjetivosCorrientes != null)
                        {
                            foreach (var item2 in item.DetalleObjetivosCorrientes)
                            {
                                foreach (var item3 in item2.DetalleProductosCorrientes)
                                {
                                    decimal TotalValorSolicitado_ = 0;
                                    decimal TotalValorTotalAprobado_ = 0;
                                    decimal TotalValorUtilizado_ = 0;
                                    item3.TramiteLiberarId = item.TramiteLiberarId;
                                    foreach (var item4 in item3.Vigencias)
                                    {
                                        item4.ValorUtilizadoOriginal = item4.ValorUtilizado;
                                        TotalValorSolicitado_ += item4.ValorSolicitado;
                                        TotalValorTotalAprobado_ += item4.ValorAprobado;
                                        TotalValorUtilizado_ += item4.ValorUtilizado;
                                    }

                                    item3.TotalValorSolicitado = TotalValorSolicitado_;
                                    item3.TotalValorTotalAprobado = TotalValorTotalAprobado_;
                                    item3.TotalValorUtilizado = TotalValorUtilizado_;
                                }
                            }
                        }
                    }
                }

                decimal TotalValorSolicitadoCorrientes = 0;
                decimal TotalValorSolicitadoConstantes = 0;
                decimal TotalTotalValorUtilizadoCorrientes = 0;
                decimal TotalTotalValorUtilizadoConstantes = 0;
                decimal TotalValorUtilizadoPorProductosCorrientes = 0;
                decimal TotalValorUtilizadoPorProductosConstantes = 0;

                if (resumen.TramitesVerificadosConstantes != null)
                {
                    foreach (var item in resumen.TramitesVerificadosConstantes)
                    {
                        foreach (var item2 in item.ResumenValoresConstantes)
                        {
                            TotalValorSolicitadoCorrientes += item2.ValorSolicitadoCorrientes;
                            TotalValorSolicitadoConstantes += item2.ValorSolicitadoConstantes;
                            TotalTotalValorUtilizadoCorrientes += item2.TotalValorUtilizadoCorrientes;
                            TotalTotalValorUtilizadoConstantes += item2.TotalValorUtilizadoConstantes;
                            TotalValorUtilizadoPorProductosCorrientes += item2.ValorUtilizadoPorProductosCorrientes;
                            TotalValorUtilizadoPorProductosConstantes += item2.ValorUtilizadoPorProductosConstantes;
                        }

                        item.TotalValorSolicitadoCorrientes = TotalValorSolicitadoCorrientes;
                        item.TotalValorSolicitadoConstantes = TotalValorSolicitadoConstantes;
                        item.TotalTotalValorUtilizadoCorrientes = TotalTotalValorUtilizadoCorrientes;
                        item.TotalTotalValorUtilizadoConstantes = TotalTotalValorUtilizadoConstantes;
                        item.TotalValorUtilizadoPorProductosCorrientes = TotalValorUtilizadoPorProductosCorrientes;
                        item.TotalValorUtilizadoPorProductosConstantes = TotalValorUtilizadoPorProductosConstantes;

                        if (item.DetalleObjetivosConstantes != null)
                        {
                            foreach (var item2 in item.DetalleObjetivosConstantes)
                            {
                                foreach (var item3 in item2.DetalleProductosConstantes)
                                {
                                    decimal TotalValorSolicitadoCorriente = 0;
                                    decimal TotalValorTotalAprobadoCorriente = 0;
                                    decimal TotalValorUtilizadoCorriente = 0;
                                    decimal TotalValorSolicitadoConstante = 0;
                                    decimal TotalValorTotalAprobadoConstante = 0;
                                    decimal TotalValorUtilizadoConstante = 0;

                                    item3.TramiteLiberarId = item.TramiteLiberarId;
                                    foreach (var item4 in item3.Vigencias)
                                    {
                                        item4.ValorUtilizadoConstantesOriginal = item4.ValorUtilizadoConstantes;
                                        TotalValorSolicitadoCorriente += item4.ValorSolicitadoCorriente;
                                        TotalValorTotalAprobadoCorriente += item4.ValorAprobadoCorrientes;
                                        TotalValorUtilizadoCorriente += item4.ValorUtilizadoCorrientes;
                                        TotalValorSolicitadoConstante += item4.ValorSolicitadoConstante;
                                        TotalValorTotalAprobadoConstante += item4.ValorAprobadoConstantes;
                                        TotalValorUtilizadoConstante += item4.ValorUtilizadoConstantes;
                                    }

                                    item3.TotalValorSolicitadoCorriente = TotalValorSolicitadoCorriente;
                                    item3.TotalValorTotalAprobadoCorriente = TotalValorTotalAprobadoCorriente;
                                    item3.TotalValorUtilizadoCorriente = TotalValorUtilizadoCorriente;
                                    item3.TotalValorSolicitadoConstante = TotalValorSolicitadoConstante;
                                    item3.TotalValorTotalAprobadoConstante = TotalValorTotalAprobadoConstante;
                                    item3.TotalValorUtilizadoConstante = TotalValorUtilizadoConstante;
                                }
                            }
                        }
                    }
                }

                return resumen;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public List<tramiteVFAsociarproyecto> ObtenerTramitesVFparaLiberar(int proyectoId)
        {
            List<tramiteVFAsociarproyecto> resultado = new List<tramiteVFAsociarproyecto>();
            var lista = Contexto.UspGetTramitesVFaLiberar(proyectoId.ToString()).ToList();

            resultado = lista
                .ConvertAll(x => new tramiteVFAsociarproyecto
                {
                    Id=x.Id,
                    NumeroTramite=x.NumeroTramite,
                    Descripcion=x.Descripcion,
                    ObjContratacion=x.ObjContratacion,
                    tipotramiteId=x.tipotramiteId,
                    fecha = x.fecha
                });
            return resultado;
        }

        public string GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuario)
        {
            string resultado = string.Empty;
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    var result = ContextoOnlySP.uspPostLiberacionVF(liberacionVigenciasFuturasDto.tramiteProyectoId, liberacionVigenciasFuturasDto.tramiteId, usuario,
                                                                      liberacionVigenciasFuturasDto.vigenciaDesde, liberacionVigenciasFuturasDto.vigenciaHasta, errorValidacionNegocio);

                    if (result.SingleOrDefault().Value > 0)
                    {
                        resultado = "OK";
                        dbContextTransaction.Commit();
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }

        public IEnumerable<ProyectoJustificacioneDto> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
       {
            List<ProyectoJustificacioneDto> lista = new List<ProyectoJustificacioneDto>();

            var result = Contexto.uspGetPreguntasJustificacionTramite_Proyectos(TramiteId, TipoTramiteId, IdNivel);

            foreach (var item in result.AsQueryable())
            {
                int pProyecto = item.ProyectoId.HasValue ? item.ProyectoId.Value : 0;
                if (lista.FindIndex(x => x.ProyectoId == pProyecto) < 0)
                {
                    ProyectoJustificacioneDto proyectotmp = new ProyectoJustificacioneDto();
                    proyectotmp.ProyectoId = pProyecto;
                    proyectotmp.BPIN = item.BPIN;
                    proyectotmp.NombreProyecto = item.NombreProyecto;
                    proyectotmp.ListaJustificacionPaso = new List<JustificacionPasoDto>();
                    lista.Add(proyectotmp);

                }
                ProyectoJustificacioneDto proyecto = lista.FirstOrDefault(x => x.ProyectoId == item.ProyectoId);
                if (proyecto != null && proyecto.ProyectoId > 0)
                {
                    var paso = proyecto.ListaJustificacionPaso.FirstOrDefault(x => x.Paso == item.paso);
                    if (paso == null)
                    {
                        JustificacionPasoDto PasoDto = new JustificacionPasoDto();
                        PasoDto.Paso = item.paso;
                        PasoDto.NombreUsuario = item.NombreUsuario;
                        PasoDto.Cuenta = item.Cuenta;
                        PasoDto.FechaEnvio = item.FechaEnvio;
                        PasoDto.justificaciones = new List<JustificacionTramiteProyectoDto>();
                        proyecto.ListaJustificacionPaso.Add(PasoDto);
                    }
                    int index = proyecto.ListaJustificacionPaso.FindIndex(x => x.Paso == item.paso);
                    JustificacionTramiteProyectoDto j = proyecto.ListaJustificacionPaso[index].justificaciones.FirstOrDefault(x => x.JustificacionPreguntaId == item.JustificacionPreguntaId);
                    if (j == null || j.JustificacionPreguntaId < 0)
                    {
                        j = new JustificacionTramiteProyectoDto();
                        j.TramiteId = item.TramiteId;
                        j.ProyectoId = item.ProyectoId;
                        j.JustificacionPreguntaId = item.JustificacionPreguntaId;
                        j.JustificacionPregunta = item.JustificacionPregunta;
                        j.OrdenJustificacionPregunta = item.OrdenJustificacionPregunta;
                        j.JustificacionRespuesta = item.JustificacionRespuesta;
                        j.ObservacionPregunta = item.ObservacionPregunta;
                        j.Tematica = item.Tematica;
                        j.OrdenTematica = item.OrdenTematica;
                        j.NombreRol = item.NombreRol;
                        j.NombreNivel = item.NombreNivel;
                        j.CuestionarioId = item.CuestionarioId;
                        j.Cuenta = item.Cuenta;
                        j.NombreUsuario = item.NombreUsuario;
                        j.FechaEnvio = item.FechaEnvio;
                        j.OpcionesRespuesta = item.OpcionesRespuesta;
                        j.Paso = item.paso;
                        proyecto.ListaJustificacionPaso[index].justificaciones.Add(j);
                    }
                }

            }
          
           
            return lista;
        }

        public int TramiteAjusteEnPasoUno(int tramiteId, int proyectoId)
        {
            int resultado = 0;
            var rta = ContextoOnlySP.uspGetTramiteAjusteEnPasoUno(tramiteId, proyectoId).FirstOrDefault();
            if (rta != null)
                resultado = rta.HasValue ? rta.Value : 0;

            return resultado;
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            List<ProyectoTramiteFuenteDto> lista = new List<ProyectoTramiteFuenteDto>();
            try
            {

                var listatmp = Contexto.uspGetProyectoFuentePresupuestalAprobadoPorTramite_Traslado(tramiteId).ToList();
                //List<uspGetProyectoFuentePresupuestalPorTramite_Traslado_Result> rta = listatmp;
                foreach (var item in listatmp)
                {
                    if (lista.Where(x => x.ProyectoId == item.proyectoId).FirstOrDefault() == null)
                    {
                        ProyectoTramiteFuenteDto prop = new ProyectoTramiteFuenteDto();
                        prop.ProyectoId = item.proyectoId.Value;
                        prop.BPIN = item.Bpin;
                        prop.NombreProyecto = item.NombreProyecto;
                        prop.Operacion = item.tipoProyecto;
                        prop.TramiteProyectoId = item.TramiteProyectoId.HasValue ? item.TramiteProyectoId.Value : 0;
                        prop.ValorTotalNacion = item.ValorTotalNacion.HasValue ? item.ValorTotalNacion.Value : 0;
                        prop.ValorTotalPropios = item.ValorTotalPropios.HasValue ? item.ValorTotalPropios.Value : 0;
                        lista.Add(prop);
                    }
                }


                foreach (var item in lista)
                {
                    item.ListaFuentes = new List<FuenteFinanciacionDto>();
                    var listafuenteTmp = listatmp.ToList().Where(x => x.proyectoId == item.ProyectoId);
                    foreach (var itemFuente in listafuenteTmp)
                    {
                        FuenteFinanciacionDto fte = new FuenteFinanciacionDto
                        {
                            FuenteId = itemFuente.FuenteId,
                            NombreCompleto = itemFuente.Nombre,
                            GrupoRecurso = itemFuente.Origen,
                            TipoValorAprobadoCSF = itemFuente.idTipoValorAprobadoCSF.HasValue ? itemFuente.idTipoValorAprobadoCSF.Value : 0,
                            TipoValorAprobadoSSF = itemFuente.idTipoValorAprobadoSSF.HasValue ? itemFuente.idTipoValorAprobadoSSF.Value : 0,
                            ValorIncialCSF = itemFuente.ValorInicialCSF,
                            ValorIncialSSF = itemFuente.ValorInicialSSF,
                            ValorVigenteSSF = itemFuente.ValorVigenteSSF,
                            ValorVigenteCSF = itemFuente.ValorVigenteCSF,
                            ValorContracreditoCSF = itemFuente.ValorSolicitadoCSF,
                            ValorContracreditoSSF = itemFuente.ValorSolicitadoSSF,
                            ValorAprobadoCSF = itemFuente.ValorAprobadoCSF,
                            ValorAprobadoSSF = itemFuente.ValorAprobadoSSF
                        };
                        item.ListaFuentes.Add(fte);
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);

            }

        }

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(productosCorrientes);
                    var result = ContextoOnlySP.uspPostActualizarLiberacionVFProducto(FuenteJson, usuario, errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(productosCorrientes);
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        return resultado;
                        //throw new ServiciosNegocioException(mensajeError);
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

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosCosntantes, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var FuenteJson = JsonUtilidades.ACadenaJson(productosCosntantes);
                    var result = ContextoOnlySP.uspPostActualizarLiberacionVFProducto(FuenteJson, usuario, errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(productosCosntantes);
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        return resultado;
                        //throw new ServiciosNegocioException(mensajeError);
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


        public List<EntidadesAsociarComunDto> ObtenerEntidadTramite(string numeroTramite)
        {
            try
            {
                var entidades = ContextoOnlySP.uspGetObtenerEntidadesTramite(numeroTramite).ToList();
                var listaEntidades = new List<EntidadesAsociarComunDto>();
                foreach (var entity in entidades)
                {
                    var entidad = new EntidadesAsociarComunDto { Id = entity.Id, NombreEntidad = entity.NombreEntidad, CabezaSector = entity.CabezaSector };
                    listaEntidades.Add(entidad);
                }
                return listaEntidades;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public VigenciaFuturaResponse EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar)
        {

            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var result = ContextoOnlySP.uspPostEliminarLiberacionVF(tramiteEliminar.tramiteId);

                    if (result > 0)
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = "Exito";
                        return resultado;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }


        public List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            List<DatosUsuarioDto> lista = new List<DatosUsuarioDto>();
            lista = ContextoOnlySP.UspGetObtenerUsuariosPorInstanciaPadre(InstanciaId).ToList()
                .ConvertAll(x => new DatosUsuarioDto
                {
                    UsuarioDnp = x.IdUsuarioDNP,
                    Cuenta = x.Correo,
                    NombreUsuario = x.NombreUsuario
                });

            return lista;
        }

        public List<CalendarioPeriodoDto> ObtenerCalendartioPeriodo(string bpin)
        {
            List<CalendarioPeriodoDto> lista = new List<CalendarioPeriodoDto>();
            lista = ContextoOnlySP.uspGetCalendarioPeriodo(bpin).ToList()
                .ConvertAll(x => new CalendarioPeriodoDto
                {
                    FaseId = x.FaseId,
                    PeriodosPeriodicidadId = x.PeriodosPeriodicidadId.HasValue ? x.PeriodosPeriodicidadId.Value : 0,
                    FechaHasta = x.FechaHasta,
                    FechaDesde = x.FechaDesde,
                    Mes = x.Mes
                });

            return lista;
        }

        public string ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId)
        {
            string jsonString = ContextoOnlySP.uspGetPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId).SingleOrDefault();

            return jsonString;
        }

        public PresupuestalProyectosAsociadosDto ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId)
        {
            try
            {
                var resumen = new PresupuestalProyectosAsociadosDto();
                string jsonString = ContextoOnlySP.uspGetPresupuestalProyectosAsociados(TramiteId, InstanciaId).SingleOrDefault();

                if (string.IsNullOrEmpty(jsonString))
                    return resumen;

                resumen = JsonConvert.DeserializeObject<PresupuestalProyectosAsociadosDto>(jsonString);

                if (resumen.ProyectosAsociados != null)
                {
                    foreach (var item in resumen.ProyectosAsociados)
                    {
                        foreach (var item2 in item.DetalleFuentes)
                        {
                            item2.ValorIncorporarCSFOriginal = item2.ValorIncorporarCSF;
                            item2.ValorIncorporarSSFOriginal = item2.ValorIncorporarSSF;
                            item2.ValorIncorporarAprobadoCSFOriginal = item2.ValorIncorporarAprobadoCSF;
                            item2.ValorIncorporarAprobadoSSFOriginal = item2.ValorIncorporarAprobadoSSF;
                        }
                    }
                }

                if (resumen.ProyectosAportantes != null)
                {
                    foreach (var item in resumen.ProyectosAportantes)
                    {                       
                        foreach (var item2 in item.DetalleFuentes)
                        {
                            item2.ValorIncorporarCSFOriginal = item2.ValorIncorporarCSF;
                            item2.ValorIncorporarSSFOriginal = item2.ValorIncorporarSSF;
                            item2.ValorIncorporarAprobadoCSFOriginal = item2.ValorIncorporarAprobadoCSF;
                            item2.ValorIncorporarAprobadoSSFOriginal = item2.ValorIncorporarAprobadoSSF;
                        }
                    }
                }

                return resumen;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public OrigenRecursosDto GetOrigenRecursosTramite(int TramiteId)
        {
            try
            {
                var resumen = new List<OrigenRecursosDto>();
                string jsonString = ContextoOnlySP.uspGetOrigenRecursos(TramiteId).SingleOrDefault();

                if (string.IsNullOrEmpty(jsonString))
                    return resumen.FirstOrDefault();

                resumen = JsonConvert.DeserializeObject<List<OrigenRecursosDto>>(jsonString);

                return resumen.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public VigenciaFuturaResponse SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var result = ContextoOnlySP.uspPostSetOrigenRecursos(JsonUtilidades.ACadenaJson(origenRecurso), usuario, errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public int ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {

            try
            {
                //var modalidad = ContextoOnlySP.uspGetObtenerModalidadContratacionVigenciasFuturas(ProyectoId, TramiteId).FirstOrDefault();

                var modalidad = Contexto.Database.SqlQuery<int>("Tramites.uspGetObtenerModalidadContratacionVigenciasFuturas    @ProyectoId, @TramiteId",
                                                              new SqlParameter("ProyectoId", ProyectoId),
                                                              new SqlParameter("TramiteId", TramiteId)
                                                              ).FirstOrDefault();

                return modalidad;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public List<TramiteRVFAutorizacion> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId)
        {
            List<TramiteRVFAutorizacion> resultado = new List<TramiteRVFAutorizacion>();
            bpin = bpin == "busquedacompleta" ? "" : bpin;
            resultado = ContextoOnlySP.uspGetObtenerAutorizacionesParaReprogramacion(bpin, tramiteId).ToList()
                .ConvertAll(x => new TramiteRVFAutorizacion
                {
                    Id = x.Id,
                    NumeroTramite = x.NumeroTramite,
                    CodigoAutorizacion = x.CodigoAutorizacion,
                    Descripcion = x.Descripcion,
                    TramiteLiberarId = x.TramiteLiberarId,
                    FechaAutorizacion = x.FechaAutorizacion

                });

            return resultado;
        }

        public string AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuario)
        {
            try
            {
                var respuesta = new TramitesResultado();
                var tramiteProyectoId = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == reprogramacionDto.TramiteId && tramiteProyecto.ProyectoId == reprogramacionDto.ProyectoId && tramiteProyecto.EntidadId == reprogramacionDto.EntidadId).FirstOrDefault()?.Id;
                var tramiteProyectoCollection = Contexto.Reprogramacion.Where(a => a.TramiteProyectoId == tramiteProyectoId && a.AutorizacionVigenciasFuturasId == reprogramacionDto.Id).ToArray();
                if (tramiteProyectoCollection.Length > 0) return "Error al asociar la autorización, ya existe una asociación";




                using (var dbContextTransaction = Contexto.Database.BeginTransaction())
                {
                    try
                    {
                        Reprogramacion reprogramacion = new Reprogramacion
                        {
                            TramiteProyectoId = Convert.ToInt32(tramiteProyectoId),
                            AutorizacionVigenciasFuturasId = Convert.ToInt32(reprogramacionDto.Id),
                            FechaCreacion = DateTime.Now,
                            CreadoPor = usuario

                        };

                        Contexto.Reprogramacion.Add(reprogramacion);
                        Contexto.SaveChanges();
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta.Exito.ToString();
                    }
                    catch (ServiciosNegocioException)
                    {
                        dbContextTransaction.Rollback();
                        respuesta.Mensaje = "Se genero una excepción interna en la base de datos al intentar registrar la autorización";
                        return respuesta.Mensaje;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return ex.Message;
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public TramiteRVFAutorizacion ObtenerAutorizacionAsociada(int tramiteId)
        {
            TramiteRVFAutorizacion resultado = new TramiteRVFAutorizacion();
            var result = ContextoOnlySP.uspGetObtenerAutorizacionAsociada(tramiteId).FirstOrDefault();

            if (result != null)
            {
                resultado.Id = result.Id;
                resultado.NumeroTramite = result.NumeroTramite;
                resultado.CodigoAutorizacion = result.CodigoAutorizacion;
                resultado.Descripcion = result.Descripcion;
                resultado.TramiteLiberarId = result.TramiteLiberarId;
                resultado.FechaAutorizacion = result.FechaAutorizacion;
                resultado.ReprogramacionId = result.ReprogramacionId;
            }

            return resultado;
        }

        public VigenciaFuturaResponse EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar)
        {
            var resultado = new VigenciaFuturaResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var result = ContextoOnlySP.uspPostEliminarAsociacionReprogramacion(tramiteEliminar.Id, tramiteEliminar.AutorizacionVigenciasFuturasId);

                    if (result > 0)
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = "Exito";
                        return resultado;
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }
    }
}