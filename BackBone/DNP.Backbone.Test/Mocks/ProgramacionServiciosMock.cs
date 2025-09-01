using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Nivel;
using DNP.Backbone.Dominio.Dto.Programacion;
using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Enums;
using DNP.Backbone.Servicios.Interfaces.Nivel;
using DNP.Backbone.Servicios.Interfaces.Programacion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.Tramites;

namespace DNP.Backbone.Test.Mocks
{
    public class ProgramacionServiciosMock : IProgramacionServicios
    {
        public Task<RespuestaGeneralDto> CrearPeriodo(string tipoEntidad, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<RespuestaGeneralDto> EditarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<RespuestaGeneralDto> EliminarProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<RespuestaGeneralDto> EliminarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<RespuestaGeneralDto> GuardarProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true, IdRegistro = "1" });
        }

        public Task<RespuestaGeneralDto> GuardarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true, IdRegistro = "1" });
        }

        public Task<RespuestaGeneralDto> IniciarProceso(string tipoEntidad, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<RespuestaGeneralDto> GuardarConfiguracionMensaje(dynamic configuracionMensaje, string usuarioDnp)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = true });
        }

        public Task<IEnumerable<ProgramacionDto>> ObtenerProgramaciones(string tipoEntidad, string usuarioDnp)
        {
            IEnumerable<ProgramacionDto> result = new List<ProgramacionDto>() { new ProgramacionDto() };
            return Task.FromResult(result);
        }

        public Task<IEnumerable<ProgramacionDto>> ObtenerProgramaciones(string tipoEntidad, Guid? capituloId, DateTime? fechaInicio, DateTime? fechaFin, EstadoProceso? estado, string usuarioDnp)
        {
            IEnumerable<ProgramacionDto> result = new List<ProgramacionDto>() { new ProgramacionDto() };
            return Task.FromResult(result);
        }

        public Task<IEnumerable<ProgramacionExcepcionDto>> ObtenerProgramacionExcepciones(int idProgramacion, string usuarioDnp)
        {
            IEnumerable<ProgramacionExcepcionDto> result = new List<ProgramacionExcepcionDto>() { new ProgramacionExcepcionDto() };
            return Task.FromResult(result);
        }

        public Task<IEnumerable> ObtenerTipoEstadoProceso()
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int TramiteId, string origen, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return Task.FromResult(jsonString);
        }

        public Task<string> ObtenerDatosProgramacionDetalle(int TramiteProyectoId, string origen, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return Task.FromResult(jsonString);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionFuentes(ProgramacionFuenteDto programacionFuente, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }


        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto programacionIniciativa, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        Task<string> IProgramacionServicios.ObtenerCargaMasivaCreditos(string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ValidarCargaMasivaCreditos(dynamic validarCargaMasivaCreditos, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<ReponseHttp> IProgramacionServicios.RegistrarCargaMasivaCreditos(dynamic registrarCargaMasivaCreditos, string usuarioDnp)
        {
            ReponseHttp result = new ReponseHttp();
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ValidarCargaMasivaCuotas(dynamic validarCargaMasivaCuotas, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<ReponseHttp> IProgramacionServicios.RegistrarCargaMasivaCuotas(dynamic registrarCargaMasivaCuotas, string usuarioDnp)
        {
            ReponseHttp result = new ReponseHttp();
            return Task.FromResult(result);
        }

        Task<RespuestaGeneralDto> IProgramacionServicios.ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";
            return Task.FromResult(response);

        }

        Task<string> IProgramacionServicios.ConsultarProyectoGenerarPresupuestal(int sectorId, int entidadId, string proyectoId, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ObtenerProgramacionSectores(int sectorId, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ObtenerProgramacionEntidadesSector(int sectorId, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<string> IProgramacionServicios.ObtenerCalendarioProgramacion(Guid FlujoId, string usuarioDnp)
        {
            string result = string.Empty;
            return Task.FromResult(result);
        }

        Task<TramitesResultado> IProgramacionServicios.RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> proyectoSinPresupuestalDto, string usuarioDNP)
        {
            TramitesResultado result = new TramitesResultado() { Exito = true };
            return Task.FromResult(result);
        }

        async Task<RespuestaGeneralDto> IProgramacionServicios.RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> calendarioProgramacionDto, string usuarioDNP)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto
            {
                Exito = true,
                Mensaje = "OK"
            };

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<string> ValidarConsecutivoPresupuestal(dynamic validarConsecutivoPresupuestal, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }
        public async Task<RespuestaGeneralDto> GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto programacionRegionalizacionDto, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<string> ConsultarPoliticasTransversalesProgramacion(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<RespuestaGeneralDto> AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto objIncluirPoliticasDto, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<string> ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<RespuestaGeneralDto> EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<RespuestaGeneralDto> AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public async Task<RespuestaGeneralDto> EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }
        public async Task<string> ObtenerCrucePoliticasProgramacion(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }
        public async Task<string> PoliticasSolicitudConceptoProgramacion(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }
        public async Task<RespuestaGeneralDto> GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }
        public async Task<RespuestaGeneralDto> SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }
        public async Task<string> ObtenerResumenSolicitudConceptoProgramacion(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public Task<string> ObtenerDatosProgramacionProducto(int TramiteId, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':2290,'Productos':[{'ProductCatalogId':1213,'NombreProducto':'Documentos de lineamientos técnicos','indicador':'Documentos de lineamientos técnicos realizados','unidadMedida':'Número de documentos','RecursoSolicitado':5192479878.00,'MetaSolicitada':1.0000,'MetaProgramada':111.0000,'RecursoProgramado':1236.0000},{'ProductCatalogId':2615,'NombreProducto':'Documentos de planeación','indicador':'Documentos de planeación realizados','unidadMedida':'Número de documentos','RecursoSolicitado':979097400.00,'MetaSolicitada':4.0000,'MetaProgramada':2615.0000,'RecursoProgramado':2615.0000},{'ProductCatalogId':863,'NombreProducto':'Estación de peaje construida','indicador':'Estación de peaje construida','unidadMedida':'Número de estaciones de peajess','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4911,'NombreProducto':'Intersección construida','indicador':'Intersección construida a nivel o desnivel en la red vial primaria','unidadMedida':'Número de intersecciones','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4910,'NombreProducto':'Obras para la reducción del riesgo','indicador':'Obras para la reducción del riesgo construidas','unidadMedida':'Número de obras','RecursoSolicitado':45589494659.00,'MetaSolicitada':20.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':865,'NombreProducto':'Peaje con servicio de administración','indicador':'Peaje con servicio de administración','unidadMedida':'Número de estaciones de peajess','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':856,'NombreProducto':'Puente con mantenimiento','indicador':'Puentes de la red primaria con mantenimiento','unidadMedida':'Número de puentes','RecursoSolicitado':647181911.00,'MetaSolicitada':1.0000,'MetaProgramada':856.0000,'RecursoProgramado':856.0000},{'ProductCatalogId':849,'NombreProducto':'Puente construido','indicador':'Puente construido para el mejoramiento de la red vial primaria','unidadMedida':'Número de puentes','RecursoSolicitado':30000000000.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':853,'NombreProducto':'Puente rehabilitado','indicador':'Puentes rehabilitados en vía primaria','unidadMedida':'Número de puentes','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':1211,'NombreProducto':'Servicio de educación informal en seguridad en Servicio de transporte','indicador':'Personas capacitadas','unidadMedida':'Número de personas','RecursoSolicitado':141425180.00,'MetaSolicitada':150.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4929,'NombreProducto':'Servicio de Información Geográfica - SIG','indicador':'Sistema de información geográfica actualizado con información para la gestión de riesgos','unidadMedida':'Número de sistemas','RecursoSolicitado':1370736360.00,'MetaSolicitada':0.2500,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4854,'NombreProducto':'Servicio de operación de túneles','indicador':'Túneles con servicio de operación','unidadMedida':'Número de túneles','RecursoSolicitado':4556000000.00,'MetaSolicitada':1.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':862,'NombreProducto':'Servicio de operación de vías primarias','indicador':'Vía primaria en operación','unidadMedida':'Kilómetros de vías primarias','RecursoSolicitado':4737042472.00,'MetaSolicitada':1.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':2630,'NombreProducto':'Servicios de operación de vías primarias','indicador':'Vías en operación ','unidadMedida':'Kilómetros de vías','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':861,'NombreProducto':'Sitio crítico estabilizado','indicador':'Sitio crítico estabilizado','unidadMedida':'Número de sitios críticos','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':857,'NombreProducto':'Túnel con mantenimiento','indicador':'Túnel de vía primaria con mantenimiento','unidadMedida':'Número de túneles','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':847,'NombreProducto':'Túnel construido','indicador':'Túnel construido para el mejoramiento de la red primaria','unidadMedida':'Número de túneles','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':860,'NombreProducto':'Túnel habilitado','indicador':'Túnel de vía primaria con mantenimiento de emergencia','unidadMedida':'Número de túneles','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4908,'NombreProducto':'Vía habilitada por emergencia','indicador':'Vía primaria habilitada por emergencia','unidadMedida':'Kilómetros de vías primarias','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':855,'NombreProducto':'Vía primaria mantenida','indicador':'Vía primaria con mantenimiento','unidadMedida':'Kilómetros de vías primarias','RecursoSolicitado':83982371017.00,'MetaSolicitada':293.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':840,'NombreProducto':'Vía primaria mejorada','indicador':'Vía primaria mejorada','unidadMedida':'Kilómetros de vías primarias','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':852,'NombreProducto':'Vía primaria rehabilitada','indicador':'Vía primaria rehabilitada','unidadMedida':'Kilómetros de vías primarias','RecursoSolicitado':52420501320.00,'MetaSolicitada':8.9600,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4859,'NombreProducto':'Viaducto con mantenimiento','indicador':'Viaductos mantenidos','unidadMedida':'Número de viaductos','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':845,'NombreProducto':'Viaducto construido','indicador':'Viaductos construidos para el mejoramiento de vía primaria','unidadMedida':'Número de viaductos','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000},{'ProductCatalogId':4858,'NombreProducto':'Viaductos rehabilitados','indicador':'Viaductos rehabilitados','unidadMedida':'Número de viaductos','RecursoSolicitado':0.00,'MetaSolicitada':0.0000,'MetaProgramada':0.0000,'RecursoProgramado':0.0000}]}";

            return Task.FromResult(jsonString);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionProducto(ProgramacionProductoDto programacionProducto, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public Task<InboxTramite> ObtenerInboxProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            return Task.FromResult(new InboxTramite());
        }

        public async Task<string> ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<RespuestaGeneralDto> BorrarTramiteProyecto(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var resultado = new RespuestaGeneralDto();


            if (objIncluirPoliticasDto != null && objIncluirPoliticasDto.DatosDimension.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return Task.FromResult(resultado);
        }

        public Task<RespuestaGeneralDto> EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var resultado = new RespuestaGeneralDto();

            if (objIncluirPoliticasDto != null && objIncluirPoliticasDto.DimensionId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return Task.FromResult(resultado);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosInclusion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }
        public Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var resultado = new RespuestaGeneralDto();


            if (objIncluirPoliticasDto != null && objIncluirPoliticasDto.DatosDimension.Count != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return Task.FromResult(resultado);
        }

        public async Task<string> ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }


        public async Task<string> ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<RespuestaGeneralDto> RegistrarCargaMasivaSaldos(int TipoCargueId, string usuarioDnp)
        {
            RespuestaGeneralDto rta = new RespuestaGeneralDto();
            rta.Exito = true;
            string result = string.Empty;
            return await Task.FromResult(rta);
        }

        public async Task<string> ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<string> ObtenerCargaMasivaSaldos(string TipoCargue, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<string> ObtenerTipoCargaMasiva(string TipoCargue, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }

        public async Task<RespuestaGeneralDto> ValidarCargaMasiva(dynamic jsonListaRegistros, string usuarioDnp)
        {
            RespuestaGeneralDto result = new RespuestaGeneralDto();
            result.Exito = true;
            return await Task.FromResult(result);
        }


       

       

        public async Task<string> ObtenerDetalleCargaMasivaSaldos(int? CargueId, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }


        public async Task<string> ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio, string usuarioDnp)
        {
            string result = string.Empty;
            return await Task.FromResult(result);
        }
        public Task<RespuestaGeneralDto> GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuarioDnp)
        {
            var resultado = new RespuestaGeneralDto();
            if (proyectoId != 0 && politicaId != 0)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "Los parametros vienen sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return Task.FromResult(resultado);
        }

    }
}
