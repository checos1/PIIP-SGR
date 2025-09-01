namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Tramite
{
    using System;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using Interfaces.Transversales;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using Persistencia.Interfaces.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Tramite;

    public class TramiteProyectoSGPServicio : ServicioBase<DatosTramiteProyectosDto>, ITramiteProyectoSGPServicio
    {
        private readonly ICacheServicio _cacheServicio;
        private readonly ITramiteProyectoSGPPersistencia _tramiteProyectoSGPPersistencia;

        public TramiteProyectoSGPServicio(ICacheServicio cacheServicio, ITramiteProyectoSGPPersistencia tramiteProyectoSGPPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cacheServicio = cacheServicio;
            _tramiteProyectoSGPPersistencia = tramiteProyectoSGPPersistencia;
        }


        public string ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            return _tramiteProyectoSGPPersistencia.ObtenerProyectosTramiteNegocio(TramiteId);
        }

        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            return _tramiteProyectoSGPPersistencia.GuardarProyectosTramiteNegocio(datosTramiteProyectosDto, usuario);
        }

        protected override DatosTramiteProyectosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosTramiteProyectosDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ValidacionProyectosTramiteNegocio(int TramiteId)
        {
            return _tramiteProyectoSGPPersistencia.ValidacionProyectosTramiteNegocio(TramiteId);
        }
    }
}
