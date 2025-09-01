using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class SGPTramiteServiciosMock : ISGPTramiteServicios
    {
        private readonly ISGPTramiteServicios _tramiteServicios;

        public Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            DNP.Backbone.Dominio.Dto.InstanciaResultado rta = new DNP.Backbone.Dominio.Dto.InstanciaResultado();
            if (instanciaTramite == new Guid("00000000-0000-0000-0000-000000000000") || instanciaTramite == null)
                rta.Exitoso = false;
            else
                rta.Exitoso = true;
            return Task.FromResult(rta);
        }

        //TramiteSGP - Información Presupuestal

        public Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
        {
            if (tramiteId == 0)
                return null;
            List<ProyectoTramiteFuenteDto> response = new List<ProyectoTramiteFuenteDto>();
            ProyectoTramiteFuenteDto proyecto = new ProyectoTramiteFuenteDto();
            proyecto.BPIN = "200001";
            proyecto.NombreProyecto = "Prueba 1";
            proyecto.ValorTotalNacion = 100;
            proyecto.ValorTotalPropios = 200;
            proyecto.Operacion = "Operacion 1";
            proyecto.Id = 1;
            proyecto.ListaFuentes = new List<FuenteFinanciacionDto>();
            FuenteFinanciacionDto fuente = new FuenteFinanciacionDto();
            fuente.FuenteId = 1;
            fuente.NombreCompleto = "Fuente 1";
            fuente.GrupoRecurso = "CSF";
            fuente.ApropiacionInicial = 1000;
            fuente.ApropiacionVigente = 2000;
            proyecto.ListaFuentes.Add(fuente);
            response.Add(proyecto);
            return Task.FromResult(response);
        }

        public Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
        {
            if (tramiteId == 0)
                return null;
            List<ProyectoTramiteFuenteDto> response = new List<ProyectoTramiteFuenteDto>();
            ProyectoTramiteFuenteDto proyecto = new ProyectoTramiteFuenteDto();
            proyecto.BPIN = "200001";
            proyecto.NombreProyecto = "Prueba 1";
            proyecto.ValorTotalNacion = 100;
            proyecto.ValorTotalPropios = 200;
            proyecto.Operacion = "Operacion 1";
            proyecto.Id = 1;
            proyecto.ListaFuentes = new List<FuenteFinanciacionDto>();
            FuenteFinanciacionDto fuente = new FuenteFinanciacionDto();
            fuente.FuenteId = 1;
            fuente.NombreCompleto = "Fuente 1";
            fuente.GrupoRecurso = "CSF";
            fuente.ApropiacionInicial = 1000;
            fuente.ApropiacionVigente = 2000;
            proyecto.ListaFuentes.Add(fuente);
            response.Add(proyecto);
            return Task.FromResult(response);
        }

        public Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pTramiteProyectoId, int? pProyectoRequisitoId, string usuarioDnp, bool isCDP)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerTiposValorPorEntidadSgp(int IdEntidad, int IdTipoEntidad, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public  Task<string> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public  Task<string> ObtenerDatosAdicionSgp(int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }


        public  Task<string> EiliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

    }
}
