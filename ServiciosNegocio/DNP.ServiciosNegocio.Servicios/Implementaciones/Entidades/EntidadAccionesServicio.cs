namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades
{
    using Dominio.Dto.Entidades;
    using Interfaces.Entidades;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Entidades;

    public class EntidadAccionesServicio : IEntidadAccionesServicio
    {
        private readonly IEntidadAccionesPersistencia _entidadAccionesPersistencia;

        public EntidadAccionesServicio(IEntidadAccionesPersistencia entidadAccionesPersistencia, IAuditoriaServicios auditoriaServicios)
        {
            _entidadAccionesPersistencia = entidadAccionesPersistencia;
        }

        public EntidadAcciones ObtenerEntidadesAcciones(EntidadAccionesEntrada parametrosConsulta)
        {
            return ObtenerDefinitivo(parametrosConsulta);
        }
        private EntidadAcciones ObtenerDefinitivo(EntidadAccionesEntrada parametrosConsulta)
        {
            var listadoRol = parametrosConsulta.ListadoRoles;
            var bPin = parametrosConsulta.Bpin;
            EntidadAcciones respuesta = new EntidadAcciones();
            var listadosp = _entidadAccionesPersistencia.ObtenerEntidadesDestino(bPin, listadoRol, parametrosConsulta.InstanciaId);
            respuesta.ListadoEntidadDestino = listadosp;
            return respuesta;
        }
    }
}
