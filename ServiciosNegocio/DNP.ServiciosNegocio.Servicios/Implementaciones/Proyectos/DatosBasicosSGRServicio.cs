using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class DatosBasicosSGRServicio : ServicioBase<DatosBasicosSGRDto>, IDatosBasicosSGRServicio
    {
        private readonly IDatosBasicosSGRPersistencia _datosBasicosSGRPersistencia;

        public DatosBasicosSGRServicio(IDatosBasicosSGRPersistencia datosBasicosSGRPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _datosBasicosSGRPersistencia = datosBasicosSGRPersistencia;
        }



        public DatosBasicosSGRDto ObtenerDatosBasicosSGR (ParametrosConsultaDto parametrosConsulta)
        {
            return _datosBasicosSGRPersistencia.ObtenerDatosBasicosSGR(parametrosConsulta.Bpin);
        }

        public DatosBasicosSGRDto ObtenerDatosBasicosSGRPreview()
        {
            return _datosBasicosSGRPersistencia.ObtenerDatosBasicosSGRPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosBasicosSGRDto> parametrosGuardar, string usuario)
        {
            _datosBasicosSGRPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override DatosBasicosSGRDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            DatosBasicosSGRDto infoPersistencia = _datosBasicosSGRPersistencia.ObtenerDatosBasicosSGR(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

       
    }
}
