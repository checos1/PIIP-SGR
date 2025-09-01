using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
using DNP.ServiciosWBS.Servicios.Interfaces;
using DNP.ServiciosWBS.Servicios.Interfaces.Transversales;
using System;

namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    public class DiligenciarFuentesServicios : ServicioBase<DiligenciarFuentesProyectoDto>, IDiligenciarFuentes
    {
        private readonly IDiligenciarFuentesPersistencia _diligenciarFuentesPersistencia;

        public DiligenciarFuentesServicios(IDiligenciarFuentesPersistencia diligenciarFuentesPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _diligenciarFuentesPersistencia = diligenciarFuentesPersistencia;
        }

        public DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentes(ParametrosConsultaDto parametrosConsulta)
        {
            _diligenciarFuentesPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);           
        }

        public DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesPreview()
        {
            return _diligenciarFuentesPersistencia.ObtenerDiligenciarFuentesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, string usuario)
        {
            _diligenciarFuentesPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override DiligenciarFuentesProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _diligenciarFuentesPersistencia.ObtenerDiligenciarFuentes(parametrosConsultaDto.Bpin);
        }
    }
}
