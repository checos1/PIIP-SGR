namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos
{
    using AutoMapper;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces;
    using Interfaces.Proyectos;
    using Modelo;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Linq;
    using ProyectoConpes = Modelo.ProyectoConpes;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProyectoPersistencia : Persistencia, IProyectoPersistencia
    {
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;
        private readonly IFasePersistencia _fasePersistencia;

        #region Constructor

        public ProyectoPersistencia(IContextoFactory contextoFactory,
            ISeccionCapituloPersistencia seccionCapituloPersistencia,
            IFasePersistencia fasePersistencia) : base(contextoFactory)
        {
            _seccionCapituloPersistencia = seccionCapituloPersistencia;
            _fasePersistencia = fasePersistencia;

        }

        #endregion

        public ProyectoDto ObtenerProyectoPreview()
        {
            return GenerarDummyProyecto();
        }

        public List<ProyectoEntidadDto> ObtenerProyectosEntidad(List<int> idEntidades, List<string> estados)
        {
            var proyectosEntidades = new List<ProyectoEntidadDto>();
            var listadoDesdeBd = Contexto.uspProyectosPorEntidades(string.Join(",", idEntidades), string.Join(",", estados));

            if (listadoDesdeBd == null)
                return proyectosEntidades;
            var resultSp = listadoDesdeBd.ToList();

            proyectosEntidades = resultSp.Select(proy => new ProyectoEntidadDto()
            {
                CodigoBpin = proy.CodigoBpin,
                EntidadId = proy.EntidadId,
                EntidadNombre = proy.EntidadNombre,
                ProyectoId = proy.ProyectoId,
                ProyectoNombre = proy.ProyectoNombre,
                SectorId = proy.SectorId,
                SectorNombre = proy.SectorNombre,
                Estado = proy.Estado,
                EstadoId = proy.EstadoId,
                HorizonteInicio = proy.HorizonteInicio,
                HorizonteFin = proy.HorizonteFin
            }).ToList();


            return proyectosEntidades;
        }

        public List<ProyectoPriorizarDto> ObtenerProyectosPriorizar(String IdUsuarioDNP)
        {
            var result = Contexto.Database.SqlQuery<ProyectoPriorizarDto>("[Proyectos].[uspGetSGR_Proyectos_Priorizar]  @IdUsuarioDNP",
                                                                    new SqlParameter("idUsuarioDNP", (object)IdUsuarioDNP ?? DBNull.Value)
                                                                    ).ToList();

            return result;
        }

        public List<ProyectoEntidadDto> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            var resultSp = Contexto.uspProyectosPorBPIN(string.Join(",", bpins.BPINs)).ToList();

            var proyectos = new List<ProyectoEntidadDto>();

            proyectos = resultSp.Select(proy => new ProyectoEntidadDto()
            {
                CodigoBpin = proy.CodigoBpin,
                EntidadId = proy.EntidadId,
                EntidadNombre = proy.EntidadNombre,
                TipoEntidad = proy.TipoEntidad,
                ProyectoId = proy.ProyectoId,
                ProyectoNombre = proy.ProyectoNombre,
                SectorId = proy.SectorId,
                SectorNombre = proy.SectorNombre,
                TipoEntidadId = proy.TipoEntidadId,
                Estado = proy.Estado,
                DescripcionCR = proy.DescripcionCR,
                FechaCreacion = proy.FechaCreacion,
                EstadoId = proy.EstadoId,
                HorizonteInicio = proy.HorizonteInicio,
                HorizonteFin = proy.HorizonteFin,
                CRTypeId = proy.CRTypeId,
                ResourceGroupId = proy.ResourceGroupId,
                ProyectoSuifId = proy.ProyectoSuifId
            }).ToList();

            return proyectos;
        }

        public List<ProyectoEntidadDto> ObtenerProyectosPorEstados(List<int> idsProyectos, List<string> nombresEstadosProyectos)
        {
            var proyectos = string.Join(",", idsProyectos.ToArray());
            var estados = string.Join(",", nombresEstadosProyectos.ToArray());

            var resultSp = Contexto.uspProyectosPorEstadosValidos(estados, proyectos).ToList();

            var proyectosEstados = new List<ProyectoEntidadDto>();

            foreach (var proyecto in resultSp) proyectosEstados.Add(MapearProyectosEstados(proyecto));

            return proyectosEstados;
        }

        public List<EntidadDto> ObtenerEntidadesPorIds(List<string> idsEntidades)
        {
            var resultSp = Contexto.uspEntidades(string.Join(",", idsEntidades)).ToList();

            var entidades = new List<EntidadDto>();

            entidades = resultSp.Select(ent => new EntidadDto()
            {
                EntidadId = ent.Id,
                EntidadNombre = ent.Name,
                TipoEntidad = ent.TipoEntidad
            }).ToList();

            return entidades;
        }

        public List<ProyectoEntidadDto> ObtenerProyectosPorIds(List<int> ids)
        {
            var resultSp = Contexto.uspProyectosPorId(string.Join(",", ids.ToArray())).ToList();

            var proyectos = new List<ProyectoEntidadDto>();

            proyectos = resultSp.Select(proy => new ProyectoEntidadDto()
            {
                CodigoBpin = proy.CodigoBpin,
                EntidadId = proy.EntidadId,
                EntidadNombre = proy.EntidadNombre,
                TipoEntidad = proy.TipoEntidad,
                ProyectoId = proy.ProyectoId,
                ProyectoNombre = proy.ProyectoNombre,
                SectorId = proy.SectorId,
                SectorNombre = proy.SectorNombre,
                TipoEntidadId = proy.TipoEntidadId,
                Estado = proy.Estado,
                DescripcionCR = proy.DescripcionCR,
                FechaCreacion = proy.FechaCreacion,
                EstadoId = proy.EstadoId,
                HorizonteInicio = proy.HorizonteInicio,
                HorizonteFin = proy.HorizonteFin,
                ValorTotal = proy.ValorTotal,
                TipoProyecto = proy.TipoProyecto
            }).ToList();

            return proyectos;
        }

        public List<CrTypeDto> ObtenerCRType()
        {
            var respuesta = Contexto.VGetCRType.Select(x => new CrTypeDto { Id = x.Id, Description = x.Description }).ToList();

            return respuesta;
        }

        public List<FaseDto> ObtenerFase()
        {
            var respuesta = Contexto.VGetFase.Select(x => new FaseDto { Id = x.Id, NombreFase = x.NombreFase, FaseGUID = x.FaseGUID }).ToList();

            return respuesta;
        }

        public RespuestaGeneralDto MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter id = new ObjectParameter("Id", typeof(int));
            ObjectParameter deleteStatus = new ObjectParameter("Result", typeof(int));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.spDeleteMatrizEntidadDestinoAccion(flujos.FirstOrDefault().EntidadResponsableId, deleteStatus);

                    foreach (var flujo in flujos)
                    {
                        if (!(flujo.EntidadResponsableId > 0 && flujo.EntidadDestinoAccionId > 0))
                        {
                            dbContextTransaction.Rollback();

                            respuesta.Mensaje = "Error al insertar nuevo flujo, EntityTypeCatalogOptionId falta en la entidad " + (flujo.EntidadDestinoAccionId > 0 ? "responsable" : "destino");
                            return respuesta;
                        }

                        Contexto.spInsertMatrizEntidadDestinoAccion(flujo.CRTypeId, flujo.EntidadResponsableId, flujo.EntidadResponsable,
                                                                    flujo.SectorId, flujo.Sector, flujo.RolId, flujo.Rol, flujo.EntidadDestinoAccionId,
                                                                    flujo.EntidadDestinoAccion, flujo.Creado, flujo.Modificado, flujo.CreadoPor,
                                                                    flujo.ModificadoPor, flujo.FaseGuid, flujo.TipoFlujo, id);

                        if (!string.IsNullOrEmpty(id.Value.ToString()))
                        {


                            respuesta.Exito = true;
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            respuesta.Mensaje = "Error al insertar nuevo flujo"; //Convert.ToString(resultado.Value);
                            respuesta.Exito = false;
                            break;
                        }
                    }

                    Contexto.SaveChanges();
                    dbContextTransaction.Commit();

                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }



        public List<MatrizEntidadDestinoAccionDto> ObtenerMatrizFlujo(int entidadResponsableId)
        {
            var resultSp = Contexto.spGetMatrizEntidadDestinoAccion(entidadResponsableId).ToList();

            var flujos = new List<MatrizEntidadDestinoAccionDto>();

            flujos = resultSp.Select(f => new MatrizEntidadDestinoAccionDto()
            {
                Id = f.Id,
                CRTypeId = f.CRTypeId,
                EntidadResponsableId = f.EntidadResponsableId,
                EntidadResponsable = f.EntidadResponsable,
                SectorId = f.SectorId,
                Sector = f.Sector,
                RolId = f.RolId,
                Rol = f.Rol,
                EntidadDestinoAccionId = f.EntidadDestinoAccionId,
                EntidadDestinoAccion = f.EntidadDestinoAccion,
                Creado = f.Creado,
                Modificado = f.Modificado,
                CreadoPor = f.CreadoPor,
                ModificadoPor = f.ModificadoPor,
                FaseGuid = f.FaseGuid,
                TipoFlujo = f.TipoFlujo

            }).ToList();

            return flujos;
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto)
        {
            var result = ContextoOnlySP.uspGetProyectosContracredito(tipoEntidad, idEntidad, idFLujo, idEntidadFiltro, bpin, nombreProyecto);

            List<ProyectoCreditoDto> proyectos = new List<ProyectoCreditoDto>();
            foreach (var p in result)
            {
                ProyectoCreditoDto proyecto = new ProyectoCreditoDto
                {
                    BPIN = p.BPIN,
                    IdEntidad = p.idEntidad.HasValue ? p.idEntidad.Value : 0,
                    IdProyecto = p.idProyecto.HasValue ? p.idProyecto.Value : 0,
                    NombreEntidad = p.nombreEntidad,
                    NombreProyecto = p.nombreProyecto,
                    Sector = p.sector,
                    Programa = p.programa,
                    Subprograma = p.subprograma,
                    MarcaTraslado = p.MarcaTraslado
                };
                proyectos.Add(proyecto);
            }
            //IEnumerable<ProyectoCreditoDto> proyectos = result.Select(p => new ProyectoCreditoDto
            //{
            //    BPIN = p.BPIN,
            //    IdEntidad = p.idEntidad.HasValue ? p.idEntidad.Value : 0,
            //    IdProyecto = p.idProyecto.HasValue ? p.idProyecto.Value : 0,
            //    NombreEntidad = p.nombreEntidad,
            //    NombreProyecto = p.nombreProyecto,
            //    Sector = p.sector,
            //    Programa = p.programa,
            //    Subprograma = p.subprograma,
            //    MarcaTraslado = p.MarcaTraslado
            //});

            return proyectos;
        }


        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto)
        {
            var result = ContextoOnlySP.uspGetProyectosCredito(tipoEntidad, idEntidad, idFLujo, bpin, nombreProyecto);

            IEnumerable<ProyectoCreditoDto> proyectos = result.Select(p => new ProyectoCreditoDto
            {
                BPIN = p.BPIN,
                IdEntidad = p.idEntidad,
                IdProyecto = p.idProyecto,
                NombreEntidad = p.nombreEntidad,
                NombreProyecto = p.nombreProyecto,
                Sector = p.sector,
                Programa = p.programa,
                Subprograma = p.subprograma
            });

            return proyectos;
        }

        #region AuditoriaEntidades

        /// <summary>
        ///     Inserta un nuevo registro de cambio de entidad del proyecto actual
        /// </summary>
        /// <param name="auditoriaEntidad">Información del cambio de entidad del proyecto actual como una instancia de la clase <see cref="AuditoriaEntidadDto"/></param>
        /// <returns>Retorna un valor numérico <see cref="Int16"/> que representa si la transacción fue realizada correctamente</returns>
        /// <exception cref="Exception">Lanzar una nueva excepción si existe una excepción de EntityFramework o uno de procesamiento .NET</exception>
        public short InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad)
        {
            const short GUARDADO = 1, NO_GUARDADO = 0, EXCEPCION = -1;
            var valorRetorno = 0;
            try
            {
                valorRetorno = (int)Contexto.uspPostAuditoriaEntidadProyectos(
                        auditoriaEntidad.EntidadOrigenId,
                        auditoriaEntidad.EntidadOrigen,
                        auditoriaEntidad.EntidadDestinoId,
                        auditoriaEntidad.EntidadDestino,
                        auditoriaEntidad.UsuarioId,
                        auditoriaEntidad.Usuario,
                        auditoriaEntidad.FechaMovimiento,
                        auditoriaEntidad.ProyectoId,
                        auditoriaEntidad.Proyecto.SectorId,
                        auditoriaEntidad.Proyecto.TipoEntidadId
                    ).FirstOrDefault();
            }
            catch (EntityException exception)
            {
                throw new Exception($"ProyectoPersistencia.InsertarAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception exception)
            {
                throw new Exception($"ProyectoPersistencia.InsertarAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? string.Empty}");
            }
            return valorRetorno > 0 ? GUARDADO : ((valorRetorno < 0) ? EXCEPCION : NO_GUARDADO);
        }

        /// <summary>
        ///     Obtiene una lista del historial de cambios de entidades del proyecto actual.
        /// </summary>
        /// <param name="proyectoId">Identificador del proyecto actual</param>
        /// <returns>Retorna una lista de objetos <see cref="List{T}"/> donde <c>T</c> es una instancia de la clase <see cref="AuditoriaEntidadDto"/></returns>
        /// <exception cref="Exception">Lanzar una nueva excepción si existe una excepción de EntityFramework o uno de procesamiento .NET</exception>
        public List<AuditoriaEntidadDto> ObtenerAuditoriaEntidad(int proyectoId)
        {
            var list = new List<AuditoriaEntidadDto>();
            try
            {
                var historialDb = Contexto.uspGetAuditoriaEntidadProyecto(proyectoId).ToList();
                list = historialDb.Select(p => new AuditoriaEntidadDto
                {
                    Identificador = (int)p.Id,
                    EntidadOrigenId = (int)p.IdEntidadOrigen,
                    EntidadOrigen = String.IsNullOrEmpty(p.NombreEntidadOrigen) ? String.Empty : p.NombreEntidadOrigen.ToString(),
                    EntidadDestinoId = (int)p.IdEntidadDestino,
                    EntidadDestino = String.IsNullOrEmpty(p.NombreEntidadDestino) ? String.Empty : p.NombreEntidadDestino.ToString(),
                    FechaMovimiento = (DateTime)p.FechaMovimiento,
                    UsuarioId = p.IdUsuario,
                    Usuario = String.IsNullOrEmpty(p.NombreUsuario) ? String.Empty : p.NombreUsuario.ToString(),
                    Proyecto = new ProyectoEntidadDto
                    {
                        ProyectoId = (int)p.ProyectoId,
                        ProyectoNombre = String.IsNullOrEmpty(p.Proyecto) ? string.Empty : p.Proyecto.ToString(),
                        SectorId = (int)p.SectorId,
                        SectorNombre = p.Sector,
                        TipoEntidadId = p.TipoEntidadId,
                        TipoEntidad = p.TipoEntidad
                    }
                }).ToList();
            }
            catch (EntityException exception)
            {
                throw new Exception($"ProyectoPersistencia.ObtenerAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? string.Empty}");
            }
            catch (Exception exception)
            {
                throw new Exception($"ProyectoPersistencia.ObtenerAuditoriaEntidad: {exception.Message}\n{exception.InnerException?.Message ?? string.Empty}");
            }
            return list;
        }
        #endregion AuditoriaEntidades

        #region MÉTODOS PRIVADOS

        private ProyectoDto GenerarDummyProyecto()
        {
            return new ProyectoDto()
            {
                Id = "380",
                Bpin = "2017184790011",
                Proyecto =
                           "Servicio PROMOCIÓN Y EJECUCIÓN, DEL IX FESTIVAL DE VERANO PLAYA RIO BODOQUERO A REALIZARSE DEL 06 AL 09 DE ENERO DE 2017 EN EL MUNICIPIO MORELIA, CAQUETÁ  Morelia",
                Entidad = "MORELIA",
                VigenciaInicial = 2017,
                VigenciaFinal = 2017,
                Horizonte = "2017 - 2017",
                ValorTotalProyecto = 50000000,
                EstadoBanco = "BankProcess",
                Estado = "En actualización",
            };
        }

        private ProyectoEntidadDto MapearProyectosEstados(uspProyectosPorEstadosValidos_Result proyecto)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspProyectosPorEstadosValidos_Result, ProyectoEntidadDto>());
            return Mapper.Map<ProyectoEntidadDto>(proyecto);
        }

        //OJO PREGUNTA SI SE PUEDE SACAR EL NIVELID
        public CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso, Guid NivelId, Guid FlujoId)
        {

            try
            {
                var infoMacroproceso = _fasePersistencia.ObtenerFaseByGuid(GuiMacroproceso);
                var capituloConpes = new CapituloConpes();
                capituloConpes.Conpes = new List<DocumentoCONPESDto>();
                //var proyectoConpesList = Contexto.UspGetProyectosConpes(proyectoId);

                var proyectoConpesList = Contexto.Database.SqlQuery<DocumentoCONPESDto>("[Transversal].[UspGetProyectosConpesPorInstancia]  @InstanciaId, @proyectoId",
                                                        new SqlParameter("@InstanciaId", InstanciaId),
                                                        new SqlParameter("@proyectoId", proyectoId)
                                                        ).ToList();

                for (var i = 0; i < proyectoConpesList.Count; i++)
                {
                    var conpes = new DocumentoCONPESDto();
                    conpes.id = proyectoConpesList[i].id;
                    conpes.titulo = proyectoConpesList[i].titulo;
                    conpes.proyectoId = proyectoConpesList[i].proyectoId;
                    conpes.numeroCONPES = proyectoConpesList[i].numeroCONPES;
                    conpes.fechaAprobacion = proyectoConpesList[i].fechaAprobacion;
                    conpes.seleccionado = proyectoConpesList[i].seleccionado;

                    capituloConpes.Conpes.Add(conpes);

                }

                //foreach (var item in proyectoConpesList)
                //{
                //    var conpes = new DocumentoCONPESDto();
                //    conpes.id = item.ConpesId;
                //    conpes.titulo = item.NombreConpes;
                //    conpes.proyectoId = item.ProyectoId;
                //    conpes.numeroCONPES = item.NumeroConpes;
                //    conpes.fechaAprobacion = item.FechaAprobacion;
                //    conpes.seleccionado = item.Seleccionado.Value;

                //    capituloConpes.Conpes.Add(conpes);
                //}
                var seccionCapituloList = Contexto.UspGetCapitulosByMacroprocesoNivel(infoMacroproceso.Id, NivelId, FlujoId).ToList();
                var seccionCapitulo = seccionCapituloList.Where(p => p.nombreComponente == "datosgeneralesrelacionconlapl").FirstOrDefault();
                var capitulosModificados = Contexto.CapitulosModificados.Where(x => x.InstanciaId == InstanciaId && x.ProyectoId == proyectoId && x.SeccionCapituloId == seccionCapitulo.SeccionCapituloId).FirstOrDefault();
                if (capitulosModificados != null)
                {
                    capituloConpes.Justificacion = capitulosModificados.Justificacion;
                }
                return capituloConpes;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        public RespuestaGeneralDto ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            //Correccion 24/02/2022 Manuel Diaz
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostActualizaHorizonte(datosHorizonteProyecto.IdProyecto, datosHorizonteProyecto.Mantiene, datosHorizonteProyecto.VigenciaInicio, datosHorizonteProyecto.VigenciaFinal, usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();

                        var capituloModificado = new CapituloModificado()
                        {
                            ProyectoId = datosHorizonteProyecto.IdProyecto,
                            Usuario = usuario,
                            InstanciaId = datosHorizonteProyecto.InstanciaId,
                            SeccionCapituloId = datosHorizonteProyecto.SeccionCapituloId
                        };

                        _seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado);

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

        public RespuestaGeneralDto AdicionarProyectoConpes(CapituloConpes Conpes, string usuario)
        {
            try
            {
                var respuesta = new RespuestaGeneralDto();
                respuesta.Exito = true;
                respuesta.Mensaje = "Ok";
                respuesta.Registros = new List<object>();

                var proyectoConpesList = Contexto.UspPostProyectosConpes(Conpes.ProyectoId, JsonUtilidades.ACadenaJson(Conpes.Conpes), usuario);
                foreach (var item in proyectoConpesList.ToList())
                {
                    var conpes = new DocumentoCONPESDto();
                    conpes.id = item.ConpesId;
                    conpes.titulo = item.NombreConpes;
                    conpes.proyectoId = item.ProyectoId;
                    conpes.numeroCONPES = item.NumeroConpes;
                    conpes.fechaAprobacion = item.FechaAprobacion;
                    conpes.seleccionado = item.Seleccionado.Value;

                    respuesta.Registros.Add(conpes);
                }


                var capituloModificado = new CapituloModificado()
                {
                    InstanciaId = Conpes.InstanciaId,
                    Justificacion = string.IsNullOrEmpty(Conpes.Justificacion) ? null : Conpes.Justificacion,
                    ProyectoId = Conpes.ProyectoId,
                    SeccionCapituloId = Conpes.SeccionCapituloId,
                    Usuario = usuario,
                    AplicaJustificacion = 1
                };

                _seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado);

                return respuesta;
            }
            catch (Exception e)
            {
                var respuesta = new RespuestaGeneralDto();
                respuesta.Exito = false;
                respuesta.Mensaje = e.Message;
                return respuesta;
            }

        }


        public List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId)
        {
            List<DocumentoCONPESDto> lstproyectoconpes = new List<DocumentoCONPESDto>();
            var data = Contexto.ProyectoConpes.FirstOrDefault(x => x.ConpesId == conpesId && x.ProyectoId == proyectoId);
            if (data != null)
            {
                Contexto.ProyectoConpes.Remove(data);
                Contexto.SaveChanges();
            }
            var proyectoConpesList = Contexto.ProyectoConpes.Where(x => x.ProyectoId == proyectoId).ToList();
            foreach (var item in proyectoConpesList)
            {
                var conpes = new DocumentoCONPESDto();
                conpes.id = item.ConpesId;
                conpes.titulo = item.NombreConpes;
                conpes.proyectoId = item.ProyectoId;
                conpes.numeroCONPES = item.NumeroConpes;
                conpes.fechaAprobacion = item.FechaAprobacion;
                conpes.seleccionado = 1;

                lstproyectoconpes.Add(conpes);
            }
            return lstproyectoconpes;
        }

        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin)
        {
            var result = Contexto.UspGetResumenObjetivosProductosActividades_JSON(bpin).SingleOrDefault();

            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<ObjectivosAjusteDto>(result);
            }
            else
            {
                return new ObjectivosAjusteDto();
            }
        }

        public void GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCadenaValor_AjusteCostoActividades(JsonUtilidades.ACadenaJson(producto), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
                {
                    dbContextTransaction.Rollback();
                    string erorr = e.InnerException == null ? e.Message : e.InnerException.Message;
                    throw new ServiciosNegocioException(erorr);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    string erorr = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    throw new ServiciosNegocioException(erorr);
                }
            }
        }

        public void AgregarEntregable(AgregarEntregable[] entregables, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCadenaValor_AgregarEntregable(JsonUtilidades.ACadenaJson(entregables), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public void EliminarEntregable(EntregablesActividadesDto entregable)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCadenaValor_EliminarEntregable(Convert.ToInt32(entregable.EntregableActividadId), resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public ObjectivosAjusteJustificacionDto ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
        {
            var result = Contexto.UspGetResumenObjetivosProductosActividadesJustificacion_JSON(bpin).SingleOrDefault();

            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<ObjectivosAjusteJustificacionDto>(result);
            }
            else
            {
                return new ObjectivosAjusteJustificacionDto();
            }
        }

        public LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto)
        {
            var result = Contexto.upsGetLocalizacionJustificacionProyecto(idProyecto).SingleOrDefault();

            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<LocalizacionJustificacionProyectoDto>(result);
            }
            else
            {
                return new LocalizacionJustificacionProyectoDto();
            }
        }

        public List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(Guid InstanciaId, string BPIN)
        {
            var resultSp = Contexto.UspGetInstanciaProyectoTramite(InstanciaId, BPIN).ToList();

            var proyectos = new List<ProyectoInstanciaDto>();

            proyectos = resultSp.Select(proy => new ProyectoInstanciaDto()
            {
                InstanciaProyecto = proy.id,
                EstadoInstanciaid = proy.estadoInstanciaid,
                ObjetoNegocioId = proy.ObjetoNegocioId,
                InstanciaPadreId = proy.InstanciaPadreId,
                TipoObjeto = proy.tipoObjeto,
                Flujoid = proy.flujoid,

            }).ToList();

            return proyectos;
        }

        public List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            var response = new List<ConfiguracionUnidadMatrizDTO>();

            foreach (var item in dto.ListMatrizEntidad)
            {
                var resultSp = Contexto.spGetMatrizEntidadDestinoAccionUnidad(item.EntidadResponsableId, JsonUtilidades.ACadenaJson(item.ListSectorId),
                    JsonUtilidades.ACadenaJson(item.ListEntidadDestinoId)).ToList();
                ConfiguracionUnidadMatrizDTO datos = new ConfiguracionUnidadMatrizDTO();

                datos.Respuesta = resultSp.Select(proy => new MatrizEntidadUnidadDto()
                {
                    Id = proy.Id,
                    CRTypeId = proy.CRTypeId,
                    EntidadResponsableId = proy.EntidadResponsableId == null ? 0 : proy.EntidadResponsableId.Value,
                    EntidadResponsable = proy.EntidadResponsable,
                    SectorId = proy.SectorId,
                    Sector = proy.Sector,
                    RolId = proy.RolId,
                    Rol = proy.Rol,
                    EntidadDestinoId = proy.EntidadDestinoAccionId,
                    EntidadDestinoAccion = proy.EntidadDestinoAccion,
                    Estado = 0
                }).ToList();

                var resultSpConfiguration = Contexto.spGetTransferConfigurationUnidad(item.EntidadResponsableId).FirstOrDefault();

                datos.TipoFlujo = resultSpConfiguration == null ? 0 : resultSpConfiguration.FlowCatalogId == null ? 0 :
                    resultSpConfiguration.FlowCatalogId.Value;
                datos.FlowId = resultSpConfiguration == null ? Guid.NewGuid() : resultSpConfiguration.FlowId;
                datos.EntidadResponsableId = item.EntidadResponsableId;

                response.Add(datos);
            }

            return response;
        }

        public RespuestaGeneralDto ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            try
            {
                var respuesta = new RespuestaGeneralDto();
                respuesta.Exito = true;
                respuesta.Mensaje = "OK";
                respuesta.Registros = new List<object>();

                foreach (var item in dto.MatrizEntidadUnidad)
                {
                    if (item.Estado == 1)
                    {
                        ObjectParameter idResult = new ObjectParameter("Id", typeof(int));
                        Contexto.spInsertMatrizEntidadDestinoAccionUnidad(item.EntidadResponsableId, item.SectorId, item.RolId, item.EntidadDestinoId, usuario, idResult);
                    }
                    else if (item.Estado == 2)
                    {
                        ObjectParameter result = new ObjectParameter("Result", typeof(int));
                        Contexto.spDeleteMatrizEntidadDestinoAccionUnidad(item.Id, item.EntidadResponsableId, item.SectorId, item.RolId, item.EntidadDestinoId, result);
                    }
                }

                if (dto.TipoFlujo.ToString() == "26953581-213f-952e-bb04-41399c4bd1fa")
                {
                    Contexto.spUpdateTransferConfigurationUnidad(dto.MatrizEntidadUnidad[0].EntidadResponsableId, 4, Guid.Parse("26953581-213f-952e-bb04-41399c4bd1fa"));
                }
                else
                {
                    Contexto.spUpdateTransferConfigurationUnidad(dto.MatrizEntidadUnidad[0].EntidadResponsableId, 4, Guid.Parse("7e76865b-2739-b052-a9f4-e81130bc4ba3"));
                }
                return respuesta;
            }
            catch (Exception e)
            {
                var respuesta = new RespuestaGeneralDto();
                respuesta.Exito = false;
                respuesta.Mensaje = e.Message;
                return respuesta;
            }

        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuario"></param>  
        /// <returns>string</returns> 
        public string ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario)
        {
            var valDev = Contexto.Database.SqlQuery<string>("Proyectos.uspGetValidacionDevolucionPaso @idInstancia, @idAccion, @idAccionDevolucion, @user",
                                                new SqlParameter("idInstancia", instanciaId),
                                                new SqlParameter("idAccion", accionId),
                                                new SqlParameter("idAccionDevolucion", accionDevolucionId),
                                                new SqlParameter("user", usuario)
                                                 ).SingleOrDefault();

            if (valDev is null)
                return "";
            else
                return valDev;
        }

        public string ObtenerProyectosBeneficiarios(string Bpin)
        {
            try
            {
                var listaPoliticas = Contexto.upsGetProyectosBeneficiarios_JSON(Bpin).FirstOrDefault();
                return listaPoliticas;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public string ObtenerProyectosBeneficiariosDetalle(string Json)
        {
            try
            {
                var listaPoliticas = ContextoOnlySP.upsGetProyectosBeneficiariosDetalle_JSON(Json).FirstOrDefault();
                return listaPoliticas;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public string ObtenerJustificacionProyectosBeneficiarios(string Bpin)
        {
            var listaPoliticas = ContextoOnlySP.upsGetJustificacionProyectosBeneficiarios_JSON(Bpin).FirstOrDefault();
            return listaPoliticas;
        }

        public void GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostBeneficiarioTotales(JsonUtilidades.ACadenaJson(beneficiario), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public void GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostBeneficiarioProducto(JsonUtilidades.ACadenaJson(beneficiario), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public void GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostBeneficiarioProductoLocalizacion(JsonUtilidades.ACadenaJson(beneficiario), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public void GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostBeneficiarioProductoLocalizacionCaracterizacion(JsonUtilidades.ACadenaJson(beneficiario), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException e)
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

        public string GetCategoriasSubcategorias_JSON(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
            var listaCategoriaSubcategoria = Contexto.uspGetCategoriaSubcategoria_JSON(padreId, entidadId, esCategoria, esGruposEtnicos).FirstOrDefault();
            return listaCategoriaSubcategoria;
        }

        public List<ProyectoEntidadDto> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros)
        {

            var proyectosEntidades = new List<ProyectoEntidadDto>();
            //var listadoDesdeBd = ContextoOnlySP.UspGetProyectosASelecionar(parametros.flujoid, string.Join(",", parametros.IdsEntidades), parametros.tipoTramiteId).SingleOrDefault();
            var listadoDesdeBd = Contexto.Database.SqlQuery<string>("[Proyectos].[UspGetProyectosASelecionar] @flujoid, @Entidades, @tipoTramiteId, @tipoEntidad, @idUsuarioDNP",
                                    new SqlParameter("@flujoid", parametros.flujoid),
                                    new SqlParameter("@Entidades", (object)string.Join(",", parametros.IdsEntidades) ?? DBNull.Value),
                                    new SqlParameter("@tipoTramiteId", parametros.tipoTramiteId),
                                    new SqlParameter("@tipoEntidad", parametros.tipoEntidad),
                                    new SqlParameter("@idUsuarioDNP", parametros.IdUsuarioDNP)
                                     ).SingleOrDefault();


            if (listadoDesdeBd == "")
                return proyectosEntidades;
            proyectosEntidades = JsonConvert.DeserializeObject<List<ProyectoEntidadDto>>(listadoDesdeBd);

            //proyectosEntidades = resultSp.Select(proy => new ProyectoEntidadDto()
            //{
            //    CodigoBpin = proy.CodigoBpin,
            //    EntidadId = proy.EntidadId,
            //    EntidadNombre = proy.EntidadNombre,
            //    ProyectoId = proy.ProyectoId,
            //    ProyectoNombre = proy.ProyectoNombre,
            //    SectorId = proy.SectorId,
            //    SectorNombre = proy.SectorNombre,
            //    Estado = proy.Estado,
            //    EstadoId = proy.EstadoId,
            //    HorizonteInicio = proy.HorizonteInicio,
            //    HorizonteFin = proy.HorizonteFin
            //}).ToList();


            return proyectosEntidades;

        }


        public string GetResumenReprogramacionPorProductoVigencia_JSON(Guid instanciaId, int proyectoId, int tramiteId)
        {
            //var listaCategoriaSubcategoria = Contexto.uspGetCategoriaSubcategoria_JSON(padreId, entidadId, esCategoria, esGruposEtnicos).FirstOrDefault();
            return null;
        }

        public string uspGetResumenReprogramacionPorVigencia_JSON(Guid instanciaId, int proyectoId, int tramiteId)
        {
            //var listaCategoriaSubcategoria = Contexto.uspGetCategoriaSubcategoria_JSON(padreId, entidadId, esCategoria, esGruposEtnicos).FirstOrDefault();
            return null;
        }


        public RespuestaGeneralDto GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            respuesta.Exito = true;
            respuesta.Mensaje = "OK";
            respuesta.Registros = new List<object>();

            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var rpta = ContextoOnlySP.uspPostReprogramacionPorProductoVigencia(JsonUtilidades.ACadenaJson(reprogramacionValores), usuario, resultado);
                    if (rpta > 0)
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        respuesta = new RespuestaGeneralDto();
                        respuesta.Exito = false;
                        respuesta.Mensaje = "No se guada la información";
                        return respuesta;
                    }
                }
                catch (ServiciosNegocioException e)
                {
                    dbContextTransaction.Rollback();
                    respuesta = new RespuestaGeneralDto();
                    respuesta.Exito = false;
                    respuesta.Mensaje = e.Message;
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Rollback();
                    respuesta = new RespuestaGeneralDto();
                    respuesta.Exito = false;
                    respuesta.Mensaje = ex.Message;
                    return respuesta;
                }
            }
        }
        public SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos)
        {
            SoportesDto soportes = new SoportesDto();
            var result = Contexto.Database.SqlQuery<DocumentosDto>("[Transversal].[uspGetDocumentosProyecto]  @IdProyecto, @Origen, @Vigencia, " +
                "                                                   @Periodo, @TipoDocumento, @Tramite, @Ficha, @ProcesoInterno, @CodigoSubproceso, @NombreDocumento, @Proceso",
                                                                    new SqlParameter("IdProyecto", filtroDocumentos.proyectoId),
                                                                    new SqlParameter("Origen", (object)filtroDocumentos.origen ?? DBNull.Value),
                                                                    new SqlParameter("Vigencia", (object)filtroDocumentos.vigencia ?? DBNull.Value),
                                                                    new SqlParameter("Periodo", (object)filtroDocumentos.periodo ?? DBNull.Value),
                                                                    new SqlParameter("TipoDocumento", (object)filtroDocumentos.tipoDocumento ?? DBNull.Value),
                                                                    new SqlParameter("Tramite", (object)filtroDocumentos.tramite ?? DBNull.Value),
                                                                    new SqlParameter("Ficha", (object)filtroDocumentos.ficha ?? DBNull.Value),
                                                                    new SqlParameter("ProcesoInterno", (object)filtroDocumentos.procesoOrigen ?? DBNull.Value),
                                                                    new SqlParameter("CodigoSubproceso", (object)filtroDocumentos.numeroProceso ?? DBNull.Value),
                                                                    new SqlParameter("NombreDocumento", (object)filtroDocumentos.nombreDocumento ?? DBNull.Value),
                                                                    new SqlParameter("Proceso", (object)filtroDocumentos.proceso ?? DBNull.Value)
                                                                    ).ToList();

            soportes.Documentos = result;
            var resultOrigen = Contexto.Database.SqlQuery<string>("[Transversal].[uspGetOrigenesDocumentos] @Proceso, @ProyectoId",
                new SqlParameter("Proceso", filtroDocumentos.proceso),
                new SqlParameter("ProyectoId", filtroDocumentos.proyectoId)).ToList();

            soportes.Origenes = resultOrigen;

            var resultVigencias = Contexto.Database.SqlQuery<int>("[Transversal].[uspGetVigenciasDocumentos] @Proceso, @ProyectoId",
                new SqlParameter("Proceso", filtroDocumentos.proceso),
                new SqlParameter("ProyectoId", filtroDocumentos.proyectoId)).ToList();

            soportes.Vigencias = resultVigencias;

            var resultPeriodos = Contexto.Database.SqlQuery<string>("[Transversal].[uspGetPeriodosDocumentos] @Proceso, @ProyectoId",
                new SqlParameter("Proceso", filtroDocumentos.proceso),
                new SqlParameter("ProyectoId", filtroDocumentos.proyectoId)).ToList();

            soportes.Periodos = resultPeriodos;

            var resultProcesos = Contexto.Database.SqlQuery<string>("[Transversal].[uspGetProcesosDocumentosMigracion] @ProyectoId",
                new SqlParameter("ProyectoId", filtroDocumentos.proyectoId)).ToList();

            soportes.ProcesosOrigen = resultProcesos;

            var resultTipoDoc = Contexto.Database.SqlQuery<Dominio.Dto.Transversales.TipoDocumento>("[Transversal].[uspGetTiposDocumentos] @Proceso, @ProyectoId",
                new SqlParameter("Proceso", filtroDocumentos.proceso),
                new SqlParameter("ProyectoId", filtroDocumentos.proyectoId)).ToList();

            soportes.TiposDocumento = resultTipoDoc;
            return soportes;
        }

        public PlanNacionalDesarrolloDto ObtenerPND(int idProyecto)
        {
            var result = Contexto.Database.SqlQuery<Dominio.Dto.Proyectos.PlanNacionalDesarrolloDto>("[Proyectos].[spGetPND] @ProyectoId",
                new SqlParameter("@ProyectoId", idProyecto)).ToList();
            return result.FirstOrDefault();
        }
    }
}





