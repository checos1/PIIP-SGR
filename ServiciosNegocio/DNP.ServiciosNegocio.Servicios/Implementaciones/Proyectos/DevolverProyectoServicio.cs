
namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos
{

    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Persistencia.Interfaces.Proyectos;
    using Interfaces.Proyectos;
    using Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes;
    using Persistencia.Interfaces.Genericos;

    public class DevolverProyectoServicio : ServicioBase<DevolverProyectoDto>, IDevolverProyectoServicio
    {
        private readonly IDevolverProyectoPersistencia _devolverProyectoPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;


        public DevolverProyectoServicio(IDevolverProyectoPersistencia devolverProyectoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _devolverProyectoPersistencia = devolverProyectoPersistencia;
        }

        public DevolverProyectoDto ObtenerDevolverProyecto(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _devolverProyectoPersistencia.ObtenerDevolverProyecto(parametrosConsultaDto.Bpin);
        }        

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, string usuario)
        {
            _devolverProyectoPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override DevolverProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            DevolverProyectoDto infoPersistencia = _devolverProyectoPersistencia.ObtenerDevolverProyecto(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

    }
}
