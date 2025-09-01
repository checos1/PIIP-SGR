namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
    using Dominio.Dto.Proyectos;
    using ServiciosNegocio.Servicios.Interfaces.Proyectos;
    public class ProyectoServicioMock : IProyectoServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public Task<ProyectoDto> ObtenerProyecto(string bPin, string tokenAutorizacion)
        {
            return bPin.Equals("2017761220016") ? Task.FromResult(new ProyectoDto()) : Task.FromResult<ProyectoDto>(null);
        }

        public Task<ProyectoDto> ObtenerProyectoPreview()
        {
            return Task.FromResult(new ProyectoDto());
        }
        public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorEntidadesYEstados(ParametrosProyectosDto parametros)
        {
            if (parametros.IdsEntidades.Contains(636) && parametros.NombresEstadosProyectos.Contains("Disponible"))
                return Task.FromResult(new List<ProyectoEntidadDto>() { new ProyectoEntidadDto() });

            return Task.FromResult<List<ProyectoEntidadDto>>(null);
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            return Task.FromResult(new List<ProyectoEntidadDto>() { new ProyectoEntidadDto() });
        }

        public Task<List<EntidadDto>> ConsultarEntidadesPorIds(List<string> idsEntidades)
        {
            return Task.FromResult(new List<EntidadDto>() { new EntidadDto() });
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosPorIds(List<int> ids)
        {
            return Task.FromResult(new List<ProyectoEntidadDto>() { new ProyectoEntidadDto() });
        }

        public Task<List<CrTypeDto>> ObtenerCRType()
        {
            return Task.FromResult(new List<CrTypeDto>() { new CrTypeDto() });
        }

        public Task<List<FaseDto>> ObtenerFase()
        {
            return Task.FromResult(new List<FaseDto>() { new FaseDto() });
        }
        public Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId)
        {
            return Task.FromResult(new List<MatrizEntidadDestinoAccionDto>() { new MatrizEntidadDestinoAccionDto() });
        }

        public Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            return Task.FromResult(new RespuestaGeneralDto { Exito = false });
        }

        Task<ProyectoDto> IProyectoServicio.ObtenerProyecto(string bPin, string tokenAutorizacion)
        {
            return Task.FromResult(new ProyectoDto { Bpin = "323232" });
        }

        Task<ProyectoDto> IProyectoServicio.ObtenerProyectoPreview()
        {
            return Task.FromResult(new ProyectoDto { Bpin = "323232" });
        }

        Task<List<ProyectoEntidadDto>> IProyectoServicio.ConsultarProyectosPorEntidadesYEstados(ParametrosProyectosDto parametros)
        {
            return Task.FromResult(new List<ProyectoEntidadDto>());
        }

        Task<List<ProyectoEntidadDto>> IProyectoServicio.ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            throw new System.NotImplementedException();
        }

        Task<List<EntidadDto>> IProyectoServicio.ConsultarEntidadesPorIds(List<string> idsEntidades)
        {
            throw new System.NotImplementedException();
        }

        Task<List<ProyectoEntidadDto>> IProyectoServicio.ConsultarProyectosPorIds(List<int> ids)
        {
            return Task.FromResult(new List<ProyectoEntidadDto>());
        }

        Task<List<CrTypeDto>> IProyectoServicio.ObtenerCRType()
        {
            throw new System.NotImplementedException();
        }

        Task<List<FaseDto>> IProyectoServicio.ObtenerFase()
        {
            throw new System.NotImplementedException();
        }

        Task<RespuestaGeneralDto> IProyectoServicio.MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            throw new System.NotImplementedException();
        }

        Task<List<MatrizEntidadDestinoAccionDto>> IProyectoServicio.ObtenerMatrizFlujo(int entidadResponsableId)
        {
            throw new System.NotImplementedException();
        }

        Task<RespuestaGeneralDto> IProyectoServicio.InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad)
        {
            throw new System.NotImplementedException();
        }

        Task<RespuestaGeneralDto> IProyectoServicio.ObtenerAuditoriaEntidad(int proyectoId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto)
        {
            List<ProyectoCreditoDto> proyectos = new List<ProyectoCreditoDto>();
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121212", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121232", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121234", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });

            return proyectos;
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto)
        {
            List<ProyectoCreditoDto> proyectos = new List<ProyectoCreditoDto>();
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121212", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121232", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });
            proyectos.Add(new ProyectoCreditoDto { BPIN = "12121234", IdEntidad = 41, IdProyecto = 2, NombreEntidad = "Entidad", NombreProyecto = "Proyecto", Sector = "Sector" });

            return proyectos;
        }

        public CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso, string NivelId, string FlujiId)
        {
            throw new NotImplementedException();
        }

        public Task<HorizonteProyectoDto> ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            throw new NotImplementedException();
        }

        Task<RespuestaGeneralDto> IProyectoServicio.ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes Conpes, string usuario)
        {
            throw new NotImplementedException();
        }

        public List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId)
        {
            throw new NotImplementedException();
        }

        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin)
        {
            throw new NotImplementedException();
        }

        Task<ReponseHttp> IProyectoServicio.GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario)
        {
            throw new NotImplementedException();
        }

        void IProyectoServicio.AgregarEntregable(AgregarEntregable[] entregables, string usuario)
        {
            throw new NotImplementedException();
        }

        void IProyectoServicio.EliminarEntregable(EntregablesActividadesDto entregable)
        {
            throw new NotImplementedException();
        }

        ObjectivosAjusteJustificacionDto IProyectoServicio.ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
        {
            throw new NotImplementedException();
        }

        public LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto)
        {
            throw new NotImplementedException();
        }



        public List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN)
        {
            throw new NotImplementedException();
        }

        string IProyectoServicio.ObtenerProyectosBeneficiarios(string bpin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuario"></param> 
        /// </summary> 
        string IProyectoServicio.ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario)
        {
            throw new NotImplementedException();
        }

        string IProyectoServicio.ObtenerJustificacionProyectosBeneficiarios(string bpin)
        {
            return string.Empty;
        }

        void IProyectoServicio.GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario)
        {
            if (beneficiario == null || string.IsNullOrWhiteSpace(beneficiario.BPIN) || beneficiario.NumeroPersonalAjuste < 0 || beneficiario.ProyectoId < 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoServicio.GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.InterventionLocalizationTypeId < 0 || beneficiario.PersonasBeneficiaros <= 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoServicio.GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId < 0)
            {
                throw new NotImplementedException();
            }
        }

        void IProyectoServicio.GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId < 0)
            {
                throw new NotImplementedException();
            }
        }

        public List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            throw new NotImplementedException();
        }

        public string GetCategoriasSubcategorias_JSON(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros)
        {
            List<ProyectoEntidadDto> lProyectos = new List<ProyectoEntidadDto>();
            ProyectoEntidadDto proyecto = new ProyectoEntidadDto()
            {
                SectorId = 33,
                SectorNombre= "Cultura",
                EntidadId = 177,
                EntidadNombre ="MINISTERIO DE CULTURA - GESTION GENERAL",
                ProyectoId=98071,
                ProyectoNombre="Prestación de servicio de apoyo profesional para el fortalecimiento del sistema municipal de cultura \"hagamos de la cultura un empre Bogotá",
                CodigoBpin ="202200000000180",
                Estado="En Ejecucion",
                EstadoId=6,
                HorizonteInicio=2022,
                HorizonteFin=2026
            };
            lProyectos.Add(proyecto);
            proyecto = new ProyectoEntidadDto()
            {
                SectorId = 12,
                SectorNombre = "Justicia y del derecho",
                EntidadId = 159,
                EntidadNombre = "INSTITUTO NACIONAL PENITENCIARIO Y CARCELARIO - INPEC",
                ProyectoId = 98149,
                ProyectoNombre = "Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional",
                CodigoBpin = "202200000000219",
                Estado = "En Ejecucion",
                EstadoId = 6,
                HorizonteInicio = 2022,
                HorizonteFin = 2025
            };
            lProyectos.Add(proyecto);



            return Task.FromResult( lProyectos);

        }

        public Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario)
        {
            RespuestaGeneralDto respuesta = new RespuestaGeneralDto();
            respuesta.Exito = false;
            if (reprogramacionValores != null && reprogramacionValores[0].ProyectoId == 0)
            {
                respuesta.Mensaje = "No se guada la información";

            }
            else
            {
                respuesta.Exito = true;
                respuesta.Mensaje = "No se guada la información";
            }
            return Task.FromResult(respuesta);

        }
        public SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos)
        {
            SoportesDto soportes = new SoportesDto();
            return soportes;
        }

        public string ObtenerProyectosBeneficiariosDetalle(string json)
        {
            return string.Empty;
        }

        public PlanNacionalDesarrolloDto ObtenerPND(int idProyecto)
        {
            return new PlanNacionalDesarrolloDto();
        }

        Task<List<ProyectoPriorizarDto>> IProyectoServicio.ObtenerProyectosPriorizar(string IdUsuarioDNP)
        {
            return Task.FromResult(new List<ProyectoPriorizarDto> { });
        }
    }

}

