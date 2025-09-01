using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion
{
    public class FuentesAprobacionServicio : ServicioBase<PreguntasSeguimientoProyectoDto>, IFuentesAprobacionServicio
    {
        private readonly IFuentesAprobacionPersistencia _IFuentesSeguimientoProyectoPersistencia;

        public FuentesAprobacionServicio(IFuentesAprobacionPersistencia fuentesSeguimientoProyectoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _IFuentesSeguimientoProyectoPersistencia = fuentesSeguimientoProyectoPersistencia;
        }

        public string ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            return _IFuentesSeguimientoProyectoPersistencia.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override PreguntasSeguimientoProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        public string GuardarPreguntasAprobacionRol(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario)
        {
            return _IFuentesSeguimientoProyectoPersistencia.GuardarPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto, usuario);
        }

        public string ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            return _IFuentesSeguimientoProyectoPersistencia.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto);
        }

        public string GuardarPreguntasAprobacionJefe(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario)
        {
            return _IFuentesSeguimientoProyectoPersistencia.GuardarPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuario);
        }

    }
}
