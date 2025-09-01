using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Persistencia.Interfaces;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System.Data.SqlClient;

namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Tramites
{
    public class TramitePersistencia : Persistencia, ITramitePersistencia
    {
        public TramitePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public DetalleCartaConceptoDto GetRadicadoEntradaORFEO(int? tramiteId) 
        {
            var response = Contexto.uspGetDetalleCartaConcepto(tramiteId).FirstOrDefault();

            return response == null ? null : new DetalleCartaConceptoDto {
                ExpedienteId = response.ExpedienteId,
                RadicadoEntrada = response.RadicadoEntrada,
                RadicadoSalida  = response.RadicadoSalida,
                FaseId = response.FaseId,
                TramiteId = response.TramiteId
            };
        }

        public string GetUsuarioDestinoORFEO(int? tramiteId, string idUsuarioDNP)
        {
            return Contexto.upsGetTramiteEnvioSubDireccion1(tramiteId, idUsuarioDNP).FirstOrDefault()?.ModificadoPor ?? null;
        }

        public int PostActualizarCartaRadicado(int tramiteId, string usuarioDnp ,string radicadoEntrada = "", string radicadoSalida = "", string expedienteId = "")
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            return Contexto.UspPostActualizarRadicadoCarta(tramiteId, radicadoEntrada, radicadoSalida, usuarioDnp, expedienteId, errorValidacionNegocio);
        }

        public List<ServiciosNegocio.Comunes.Dto.Tramites.TramiteProyectoDto> GetTramiteProyectos(int tramiteId)
        {
            var result = Contexto.uspGetProyectosTramite(tramiteId);

            return result.Select(w => new ServiciosNegocio.Comunes.Dto.Tramites.TramiteProyectoDto
            {
                BPIN = w.BPIN,
                EntidadId = w.EntidadId,
                Estado = w.Estado,
                NombreEntidad = w.NombreEntidad,
                NombreProyecto = w.NombreProyecto,
                Sector = w.Sector,
                TipoProyecto =  w.TipoProyecto                
            }).ToList();
        }

        public List<AnalistaResponsableDto> ObtenerAnalistaResponsablePorSector(int sectorId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            var result = Contexto.uspGetAnalistaResponsablePorSector(sectorId);

            return result.Select(w => new AnalistaResponsableDto
            {
                IdUsuario = w.IdUsuario.Value,
                Cuenta = w.Cuenta
            }).ToList();
        }

        public string EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            var result = Contexto.uspPostEliminarMarcaPervioVigencia_Proyectos(bpin, vigencia);

            return result.FirstOrDefault();
        }


        public ResponseDto<bool> ActualizarCargueMasivo(string numeroProceso, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResponseDto<bool>();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostActualizarCargueMasivo(numeroProceso,  usuario, errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Estado = true;
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Estado = false;
                        resultado.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return resultado;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ConsultarCargueExcel(string numeroProceso)
        {
            var resultSp = Contexto.uspGetConsultarCargueExcel_JSON(numeroProceso).FirstOrDefault();
            return resultSp;
        }

        public List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            List<DatosUsuarioDto> lista = new List<DatosUsuarioDto>();
            lista = Contexto.UspGetObtenerUsuariosPorInstanciaPadre(InstanciaId).ToList()
                .ConvertAll(x => new DatosUsuarioDto
                {
                    UsuarioDnp = x.IdUsuarioDNP,
                    Cuenta = x.Correo,
                    NombreUsuario = x.NombreUsuario
                });

            return lista;
        }

        public List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin)
        {
            List<CodigoPresupuestal_Proyecto> lista = new List<CodigoPresupuestal_Proyecto>();
            lista = Contexto.uspGetDatosMarcaPervioVigencia_Proyectos(Bpin).ToList()
                .ConvertAll(x => new CodigoPresupuestal_Proyecto
                {
                    Id  =    x.proyectoId,
                    NombreProyecto = x.NombreProyecto,
                    Bpin = x.BPIN,
                    CodigoEntidad = x.CodigoEntidad,
                    NombreEntidad = x.NombreEntidad,
                    CodigoSubprograma = x.CodigoSubPrograma,
                    Subprograma = x.NombreSubPrograma,
                    CodigoPrograma = x.CodigoPrograma,
                    Programa = x.NombrePrograma,
                    UsuarioEnvio = x.UsuarioEnvio,
                    NombreFuente = x.NombreFuente,
                    ValorVigente = x.ValorVigente,
                    CodigoProceso = x.CodigoProceso,
                    InstanciaId = x.InstanciaId == null ? new Guid("00000000-0000-0000-0000-000000000000") : x.InstanciaId.Value
                });
            return lista;

       
        }

        public ResponseDto<bool> ActualizaCampoRemitenteConcepto(int tramiteId, string usuarioDNP)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResponseDto<bool>();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var result = Contexto.uspPostActualizaCampoRemitenteConcepto(tramiteId, usuarioDNP);

                    dbContextTransaction.Commit();
                    resultado.Estado = true;
                    return resultado;
                   
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public DetalleTramiteDto ObtenerDetalleTramiteRadicado(string numeroTramite)
        {
            var detalleTramite = Contexto.uspGetObtenerDetalleTramite(numeroTramite).FirstOrDefault();



            return new DetalleTramiteDto()
            {
                TramiteId = detalleTramite.TramiteId,
                SectorId = detalleTramite.SectorId.Value,
                NombreSector = detalleTramite.NombreSector,
                EntidadId = detalleTramite.EntidadId.Value,
                NombreEntidad = detalleTramite.NombreEntidad,
                TipoTramiteId = detalleTramite.TipoTramiteId.Value,
                TipoTramite = detalleTramite.TipoTramite,
                CodigoTipoTramite = detalleTramite.CodigoTipoTramite,
                Descripcion = detalleTramite.Descripcion,
                DescripcionInstancia = detalleTramite.DescripcionInstancia,
                InstanciaId = detalleTramite.InstanciaId,
                CodigoDocumental = detalleTramite.CodigoDocumental,
                PDF = detalleTramite.PDF
            };



        }

        public int ObtenerDependenciaByEntidadOrfeoId(int EntidadOrfeoId)
        {
             var detalleTramite = Contexto.uspGetDependenciaByOrfeoEntidadId(EntidadOrfeoId).FirstOrDefault();

            return detalleTramite.Value;
        }


    }
}
