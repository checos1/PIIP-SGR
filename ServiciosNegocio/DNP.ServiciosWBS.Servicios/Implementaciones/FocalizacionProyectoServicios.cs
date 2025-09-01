namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;

    public class FocalizacionProyectoServicios : ServicioBase<FocalizacionProyectoDto>, IFocalizacionProyectoServicios
    {
        private readonly IFocalizacionProyectoPersistencia _focalizacionPersistencia;

        public FocalizacionProyectoServicios(IFocalizacionProyectoPersistencia focalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionPersistencia = focalizacionPersistencia;
        }

        public FocalizacionProyectoDto ObtenerFocalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            _focalizacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public FocalizacionProyectoDto ObtenerFocalizacionPreview()
        {
            return _focalizacionPersistencia.ObtenerFocalizacionPreview();
        }

        protected override FocalizacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FocalizacionProyectoDto infoPersistencia = _focalizacionPersistencia.ObtenerFocalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar,
                                                   string usuario)
        {
            _focalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
