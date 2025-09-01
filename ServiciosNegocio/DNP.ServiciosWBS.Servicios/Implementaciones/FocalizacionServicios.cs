namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

    public class FocalizacionServicios : ServicioBase<FocalizacionProyectoDto>, IFocalizacionServicios
    {
        private readonly IFocalizacionPersistencia _focalizacionPersistencia;

        public FocalizacionServicios(IFocalizacionPersistencia focalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPersistencia = focalizacionPersistencia;
        }

        public FocalizacionProyectoDto ObtenerProyectoFocalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public FocalizacionProyectoDto ObtenerProyectoFocalizacionPreview()
        {
            return _focalizacionPersistencia.ObtenerFocalizacionProyectoPreview();
        }

        protected override FocalizacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FocalizacionProyectoDto infoPersistencia = _focalizacionPersistencia.ObtenerFocalizacionProyecto(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar,
                                                   string usuario)
        {
            _focalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
