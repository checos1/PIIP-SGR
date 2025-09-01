namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Comunes;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Dto;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
    public class FuenteFinanciacionProyectoServicioMock : IFuenteFinanciacionServicios
    {
        ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> IFuenteFinanciacionServicios.ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoFuenteFinanciacionDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<ProyectoFuenteFinanciacionDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idInstanciaFlujo"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idInstanciaFlujo"));

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idAccion"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idAccion"));

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;

        }

        public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        {
            throw new NotImplementedException();
        }

        RespuestaGeneralDto IFuenteFinanciacionServicios.EliminarPoliticasProyecto(int proyectoId, int politicaId)
        {
            throw new NotImplementedException();
        }

        void IFuenteFinanciacionServicios.Guardar(ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {

        }

        string IFuenteFinanciacionServicios.GuardarCategoriasPoliticaTransversalesAjustes(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        string IFuenteFinanciacionServicios.GuardarPoliticasTransversalesAjustes(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        ProyectoFuenteFinanciacionDto IFuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin.Equals("2017011000042"))
            {

                return new ProyectoFuenteFinanciacionDto()
                {
                    BPIN = "2017011000042",
                    ValorTotalProyecto = 1103874997,
                    CR = 1,
                    FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                                          {
                                              new FuenteFinanciacionDto()
                                              {
                                                  EjecucionId = null,
                                                  OtraEntidad = "",
                                                  EntidadId = 3,
                                                  Vigencia = 2017,
                                                  TipoRecursoId = 1,
                                                  TipoEntidadId = 1,
                                                  Mes = 1,
                                                  Obligacion = null,
                                                  GrupoRecurso =" PGN",
                                                  EtapaId = 1,
                                                  Pago = null,
                                                  Solicitado = 100,
                                                  Compromiso = 100,
                                                  ApropiacionVigente = 100,
                                                  ApropiacionInicial = 100,
                                                  ProgramacionId = 100,
                                                  FuenteId = 1
                                              }
                                          }
                };
            }

            return null;
        }

        ProyectoFuenteFinanciacionDto IFuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyectoPreview()
        {
            return new ProyectoFuenteFinanciacionDto()
            {

                BPIN = "2017011000149",
                ValorTotalProyecto = Convert.ToDecimal(4579958855.04),
                CR = 2,
                FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                {
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 1,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 2,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                    EjecucionId = 3,
                    OtraEntidad = "",
                    EntidadId = 3,
                    Vigencia = 2017,
                    TipoRecursoId = 1,
                    TipoEntidadId = 1,
                    Mes = 1,
                    Obligacion = null,
                    GrupoRecurso =" PGN",
                    EtapaId = 1,
                    Pago = null,
                    Solicitado = 100,
                    Compromiso = 100,
                    ApropiacionVigente = 100,
                    ApropiacionInicial = 100,
                    ProgramacionId = 100,
                    FuenteId = 1
                },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 4,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    }

                }

            };
        }

        string IFuenteFinanciacionServicios.ObtenerPoliticasTransversalesAjustes(string Bpin)
        {
            throw new NotImplementedException();
        }

        string IFuenteFinanciacionServicios.ObtenerPoliticasTransversalesCategorias(string Bpin)
        {
            throw new NotImplementedException();
        }

        string IFuenteFinanciacionServicios.ObtenerPoliticasTransversalesResumen(string Bpin)
        {
            return new ProyectoFuenteFinanciacionDto()
            {

                BPIN = "2017011000149",
                ValorTotalProyecto = Convert.ToDecimal(4579958855.04),
                CR = 2,
                FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                {
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 1,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 2,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                    EjecucionId = 3,
                    OtraEntidad = "",
                    EntidadId = 3,
                    Vigencia = 2017,
                    TipoRecursoId = 1,
                    TipoEntidadId = 1,
                    Mes = 1,
                    Obligacion = null,
                    GrupoRecurso =" PGN",
                    EtapaId = 1,
                    Pago = null,
                    Solicitado = 100,
                    Compromiso = 100,
                    ApropiacionVigente = 100,
                    ApropiacionInicial = 100,
                    ProgramacionId = 100,
                    FuenteId = 1
                },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 4,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    }

                }

            }.ToString();
        }

        public string ObtenerPoliticasCategoriasIndicadores(string Bpin)
        {
            return new ProyectoFuenteFinanciacionDto()
            {

                BPIN = "2017011000149",
                ValorTotalProyecto = Convert.ToDecimal(4579958855.04),
                CR = 2,
                FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                {
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 1,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 2,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                    EjecucionId = 3,
                    OtraEntidad = "",
                    EntidadId = 3,
                    Vigencia = 2017,
                    TipoRecursoId = 1,
                    TipoEntidadId = 1,
                    Mes = 1,
                    Obligacion = null,
                    GrupoRecurso =" PGN",
                    EtapaId = 1,
                    Pago = null,
                    Solicitado = 100,
                    Compromiso = 100,
                    ApropiacionVigente = 100,
                    ApropiacionInicial = 100,
                    ProgramacionId = 100,
                    FuenteId = 1
                },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 4,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    }

                }

            }.ToString();
        }

        public string ModificarCategoriasIndicadores(string Bpin)
        {
            return new ProyectoFuenteFinanciacionDto()
            {

                BPIN = "2017011000149",
                ValorTotalProyecto = Convert.ToDecimal(4579958855.04),
                CR = 2,
                FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                {
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 1,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 2,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    },
                    new FuenteFinanciacionDto()
                    {
                    EjecucionId = 3,
                    OtraEntidad = "",
                    EntidadId = 3,
                    Vigencia = 2017,
                    TipoRecursoId = 1,
                    TipoEntidadId = 1,
                    Mes = 1,
                    Obligacion = null,
                    GrupoRecurso =" PGN",
                    EtapaId = 1,
                    Pago = null,
                    Solicitado = 100,
                    Compromiso = 100,
                    ApropiacionVigente = 100,
                    ApropiacionInicial = 100,
                    ProgramacionId = 100,
                    FuenteId = 1
                },
                    new FuenteFinanciacionDto()
                    {
                        EjecucionId = 4,
                        OtraEntidad = "",
                        EntidadId = 3,
                        Vigencia = 2017,
                        TipoRecursoId = 1,
                        TipoEntidadId = 1,
                        Mes = 1,
                        Obligacion = null,
                        GrupoRecurso =" PGN",
                        EtapaId = 1,
                        Pago = null,
                        Solicitado = 100,
                        Compromiso = 100,
                        ApropiacionVigente = 100,
                        ApropiacionInicial = 100,
                        ProgramacionId = 100,
                        FuenteId = 1
                    }

                }

            }.ToString();
        }

        public ResultadoProcedimientoDto ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            return new ResultadoProcedimientoDto();
        }

        public RespuestaGeneralDto EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId)
        {
            throw new NotImplementedException();
        }

        public string ObtenerCrucePoliticasAjustes(string bPIN)
        {
            return string.Empty;
        }

        public RespuestaGeneralDto GuardarCrucePoliticasAjustes(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string name)
        {
            return new RespuestaGeneralDto();
        }

        string IFuenteFinanciacionServicios.ObtenerPoliticasSolicitudConcepto(string Bpin)
        {
            List<Dominio.Dto.Focalizacion.Politicas> politicas = new List<Dominio.Dto.Focalizacion.Politicas>();

            Dominio.Dto.Focalizacion.Politicas politicaVictimas = new Dominio.Dto.Focalizacion.Politicas()
            {
                PoliticaId=20,
                Politica = "Víctimas",
            };
            Dominio.Dto.Focalizacion.Politicas politicaEquidad = new Dominio.Dto.Focalizacion.Politicas()
            {
                PoliticaId = 7,
                Politica = "Equidad de la mujer",
            };

            politicas.Add(politicaVictimas);
            politicas.Add(politicaEquidad);

            return politicas.ToArray().ToString();
        }

        public string FocalizacionSolicitarConceptoDT(ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>> parametrosGuardar, string name)
        {
            return "Politca FocalizacionSolicitarConceptoDT";
        }

        string IFuenteFinanciacionServicios.ObtenerDireccionesTecnicasPoliticasFocalizacion()
        {
            var json = "[{'Id':1,'EntityTypeCatalogOptionId':10010146,'PolicitTargetingId':4},{'Id':2,'EntityTypeCatalogOptionId':10010151,'PolicitTargetingId':7},{'Id':3,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':9},{'Id':4,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':10},{'Id':5,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':11},{'Id':6,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':12},{'Id':7,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':13},{'Id':8,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':14},{'Id':9,'EntityTypeCatalogOptionId':10010206,'PolicitTargetingId':20}]";
            return json;
        }

        string IFuenteFinanciacionServicios.ObtenerResumenSolicitudConcepto(string Bpin)
        {
            var json = "[{'BPIN':'202200000000136','ProyectoId':97986,'Resumen':[{'PoliticaId':4,'Politica':'Construcción de Paz','DireccionTecnica':'Sub. De Derechos humanos y Paz','Descripcion':'Concepto tomado desde el front','Fecha':'09/24/2022 21:33','Aprobacion':'Pendiente'},{'PoliticaId':11,'Politica':'Grupos étnicos - comunidades negras','DireccionTecnica':'Sub. De Derechos humanos y Paz','Descripcion':'Concepto tomado desde el front','Fecha':'09/24/2022 21:33','Aprobacion':'Pendiente'}]}]";
            return json;
        }

        public string ObtenerPreguntasEnvioPoliticaSubDireccion(Guid instanciaid, int proyectoid, string usuarioDNP, Guid nivelid)
        {
            var json = "{'ProyectoId':97990,'BPIN':'202200000000135','Politicas':[{'PoliticaId':13,'NombrePolitica':'Grupos étnicos - comunidades raizales','EnviosSubdireccion':[{'EnvioPoliticaSubDireccionIdAgrupa':40,'NombreEntidadFormulador':'','NombreUsuarioFormulador':'Belinda:Benitez','Correo':'BelindaBenitez@yopmail.com','FechaEnvio':'2022-10-18T09:28:21.680','Descripcion':'Para hacer la solicitud, es necesario haber sustentado en la sección \'Justificación\', las modificaciones realizada en la política.\n\nSi requiere completar la justificación de la modificación, diríjase a la pestaña \'Soportes\' para adjuntar un documento','UsuarioFormulador':'CC50995617','IdUsuarioDNP':'CC50995617','NombreUsuarioEnvio':'Belinda:Benitez','EntityTypeCatalogOptionId':10010146,'NombreEntityTypeCatalogOption':'Sub. De Derechos humanos y Paz','Preguntas':[{'PreguntaId':3849,'Pregunta':'¿Valida la desagregación de EDT, programación de actividades y productos?','NombreRol':'ConceptoPolitica','OpcionesRespuesta':'[{\'OpcionId\':1,\'ValorOpcion\':\'SI\'},{\'OpcionId\':2,\'ValorOpcion\':\'NO\'}]','Respuesta':'1','ObligaObservacion':null,'ObservacionPregunta':'Probando 123','Tipo':null}]}]}]}";
            return json;
        }

        public string GuardarPreguntasEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string name)
        {
            return "OK";
        }

        public string GuardarRespuestaEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string name)
        {
            return "OK";
        }

    }
}
