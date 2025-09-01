namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Beneficiarios;
    using DNP.Backbone.Dominio.Dto.CadenaValor;
    using DNP.Backbone.Dominio.Dto.CostoActividades;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;
    using DNP.Backbone.Dominio.Dto.Transversales;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    public class ProyectoServiciosMock : IProyectoServicios
    {
        public Task<ProyectoDto> ObtenerInfoPDF(InstanciaProyectoDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuarioDNP"></param>    
        public Task<string> ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new ProyectoDto());
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new ProyectoDto { GruposEntidades = new List<GrupoEntidadProyectoDto> { new GrupoEntidadProyectoDto() } });
        }

        public Task<IEnumerable<ProyectoResumenDto>> ObtenerMonitoreoProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            var resumenProyectosDtoMock = new List<ProyectoResumenDto>();
            resumenProyectosDtoMock.Add(new ProyectoResumenDto
            {
                //ProyectoNombre = "Prueba Ok"
            });
            return Task.FromResult(resumenProyectosDtoMock.AsEnumerable());
        }

        Task<ProyectoResumenDto> IProyectoServicios.ObtenerMonitoreoProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new ProyectoResumenDto { GruposEntidades = new List<GrupoEntidadProyectoResumenDto>{ new GrupoEntidadProyectoResumenDto() }  });
        }

        public Task<InstanciaDto> ActivarInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new InstanciaDto());
        }

        public Task<InstanciaDto> PausarInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new InstanciaDto());
        }

        public Task<InstanciaDto> DetenerInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new InstanciaDto());
        }
        public Task<InstanciaDto> CancelarInstanciaMisProcesos(ProyectoParametrosDto datosConsulta, string token)
        {
            return Task.FromResult(new InstanciaDto());
        }

        public Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            IEnumerable<ProyectoCreditoDto> ret = new List<ProyectoCreditoDto>();
            (ret as List<ProyectoCreditoDto>).Add(new ProyectoCreditoDto { BPIN = "223323" });
            return Task.FromResult(ret);
        }

        public Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            IEnumerable<ProyectoCreditoDto> ret = new List<ProyectoCreditoDto>();
            (ret as List<ProyectoCreditoDto>).Add(new ProyectoCreditoDto { BPIN = "223323" });
            return Task.FromResult(ret);
        }

        public Task<RespuestaGeneralDto> GuardarProyectos(ParametroProyectoTramiteDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP)
        {
            return Task.FromResult(new ProyectoDto());
        }
        
        public Task<ProyectoDto> ObtenerProyectosTodos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProyectoDto> ObtenerProyectosConsolaProcesos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ObtenerTokenMGA(string bpin, Dominio.Dto.UsuarioLogadoDto usuarioLogeado, string tipoUsuario, string tokenAutorizacion)
        {
            if (string.IsNullOrEmpty(bpin) || string.IsNullOrEmpty(tokenAutorizacion) || string.IsNullOrEmpty(tipoUsuario) || string.IsNullOrEmpty(usuarioLogeado.IdUsuario))
                return null;

            return Task.FromResult("OK");
        }

		public Task<RespuestaGeneralDto> actualizarHorizonte(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp)
		{
			throw new System.NotImplementedException();
		}

        public Task<string> ObtenerProyectosBpin(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        Task<IndicadorProductoDto> IProyectoServicios.ObtenerIndicadoresProducto(string bpin, string tokenAutorizacion, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<IndicadorResponse> IProyectoServicios.GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }
        Task<IndicadorResponse> IProyectoServicios.EliminarIndicadorProducto(int indicadorId, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<ObjectivosAjusteDto> IProyectoServicios.ObtenerResumenObjetivosProductosActividades(string bpin, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<ReponseHttp> IProyectoServicios.GuardarCostoActividades(ProductoAjusteDto producto, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<IndicadorResponse> IProyectoServicios.ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<string> IProyectoServicios.AgregarEntregable(AgregarEntregable[] entregables, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<string> IProyectoServicios.EliminarEntregable(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<List<InstanciaDto>> IProyectoServicios.DevolverInstanciasHijas(ProyectoParametrosDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        Task<List<IndicadorCapituloModificadoDto>> IProyectoServicios.IndicadoresValidarCapituloModificado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegionalizacionDto> RegionalizacionGeneral(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(new RegionalizacionDto());
        }

        public Task<RespuestaGeneralDto> GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuarioDNP)
        {
            return Task.FromResult(new RespuestaGeneralDto());
        }

        Task<ObjectivosAjusteJustificacionDto> IProyectoServicios.ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

		public Task<LocalizacionJustificacionProyectoDto> ObtenerJustificacionLocalizacionProyecto(int proyectoId, string usuarioDNP)
		{
            LocalizacionJustificacionProyectoDto ret = new LocalizacionJustificacionProyectoDto();
                return Task.FromResult(ret);
        }

        public Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuarioDNP)
        {
            return Task.FromResult(new RespuestaGeneralDto());
        }

        public Task<string> ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<List<ProyectoInstanciaDto>> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IProyectoServicios.ObtenerProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        Task<string> IProyectoServicios.ObtenerJustificacionProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IProyectoServicios.GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            if (beneficiario == null || string.IsNullOrWhiteSpace(beneficiario.BPIN) || beneficiario.NumeroPersonalAjuste < 0 || beneficiario.ProyectoId < 0)
            {
                throw new System.NotImplementedException();
            }

            return Task.FromResult("OK");
        }

        Task<string> IProyectoServicios.GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuarioDNP)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.InterventionLocalizationTypeId < 0 || beneficiario.PersonasBeneficiaros <= 0)
            {
                throw new System.NotImplementedException();
            }

            return Task.FromResult("OK");
        }

        Task<string> IProyectoServicios.GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId <= 0)
            {
                throw new System.NotImplementedException();
            }

            return Task.FromResult("OK");
        }

        Task<string> IProyectoServicios.GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            if (beneficiario == null || beneficiario.ProductoId <= 0 || beneficiario.ProyectoId <= 0 || beneficiario.LocalizacionId <= 0 || beneficiario.Vigencia <= 0)
            {
                throw new System.NotImplementedException();
            }

            return Task.FromResult("OK");
        }

        public Task<string> ObtenerSeccionPoliticaFocalizacionDT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuarioDNP)
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
                respuesta.Mensaje = "Se guardó la información";
            }
            return Task.FromResult(respuesta);

        }
        public Task<SoportesDto> ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos, string usuarioDNP)
        {
            SoportesDto soportes = new SoportesDto();
            soportes.Documentos.Add(new DocumentosDto() { Id = 1, Descripcion = "Documento" });
            return Task.FromResult(soportes);
        }

        Task<string> IProyectoServicios.ObtenerProyectosBeneficiariosDetalle(string json, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }
        public Task<PlanNacionalDesarrolloDto> ObtenerPND(int idProyecto, string usuarioPND)
        {
            var pnd = new PlanNacionalDesarrolloDto();
            return Task.FromResult(pnd);
        }

        public Task<ProyectoVerificacionOcadPazDto> ObtenerProyectosVerificacionOcadPazSgr(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroVerificacionOcadPazSgrDto proyectoFiltroDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
