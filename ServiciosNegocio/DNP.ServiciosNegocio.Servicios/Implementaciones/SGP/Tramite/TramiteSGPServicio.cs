using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Tramite;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Tramite
{
    public class TramiteSGPServicio : ServicioBase<DatosTramiteProyectosDto>, ITramiteSGPServicio
    {
        private readonly ICacheServicio _cacheServicio;
        private readonly ITramiteSGPPersistencia _tramiteSGPPersistencia;

        public TramiteSGPServicio(ICacheServicio cacheServicio, ITramiteSGPPersistencia tramitesProyectoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cacheServicio = cacheServicio;
            _tramiteSGPPersistencia = tramitesProyectoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosTramiteProyectosDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        protected override DatosTramiteProyectosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
        {
            return _tramiteSGPPersistencia.ActualizaEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, usuario);
        }

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            return _tramiteSGPPersistencia.EliminarProyectoTramiteNegocio(TramiteId, ProyectoId);
        }

        public TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario)
        {
            return _tramiteSGPPersistencia.GuardarTramiteInformacionPresupuestal(parametrosGuardar, usuario);
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            return _tramiteSGPPersistencia.ObtenerListaProyectosFuentes(tramiteId);
        }

        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
        {
            return _tramiteSGPPersistencia.GuardarFuentesTramiteProyectoAprobacion(fuentesTramiteProyectoAprobacion, usuario);
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            return _tramiteSGPPersistencia.ObtenerListaProyectosFuentesAprobado(tramiteId);
        }

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario)
        {
            return _tramiteSGPPersistencia.GuardarTramiteTipoRequisito(parametrosGuardar, usuario);
        }

        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            return _tramiteSGPPersistencia.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, isCDP);
        }
        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo)
        {
            return _tramiteSGPPersistencia.ObtenerProyectosContracredito(tipoEntidad, idEntidad, idFLujo);
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo)
        {
            return _tramiteSGPPersistencia.ObtenerProyectosCredito(tipoEntidad, idEntidad, idFLujo);
        }

        public string ObtenerTiposValorPorEntidad(int IdEntidad, int IdTipoEntidad)
        {
            return _tramiteSGPPersistencia.ObtenerTiposValorPorEntidad(IdEntidad, IdTipoEntidad);
        }

        public RespuestaGeneralDto GuardarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteSGPPersistencia.GuardarDatosAdicionSgp(parametrosGuardar, usuario);
        }

        public RespuestaGeneralDto eliminarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteSGPPersistencia.eliminarDatosAdicionSgp(parametrosGuardar, usuario);
        }

        public string ObtenerDatosAdicionSgp(int tramiteId)
        {
            return _tramiteSGPPersistencia.ObtenerDatosAdicionSgp(tramiteId);
        }
    }
}
