namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;

    public class ValidarViabilidadCompletarInfoServicios : ServicioBase<ValidarViabilidadCompletarInfoDto>, IValidarViabilidadCompletarInfoServicios
    {
        private readonly IValidarViabilidadCompletarInfoPersistencia _validarViabilidadCompletarInfoPersistencia;

        public ValidarViabilidadCompletarInfoServicios(IValidarViabilidadCompletarInfoPersistencia validarViabilidadCompletarInfoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _validarViabilidadCompletarInfoPersistencia = validarViabilidadCompletarInfoPersistencia;
        }

        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsulta)
        {
            //_validarViabilidadCompletarInfoPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview()
        {
            return _validarViabilidadCompletarInfoPersistencia.ObtenerValidarViabilidadCompletarInfoPreview();
        }

        protected override ValidarViabilidadCompletarInfoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            ValidarViabilidadCompletarInfoDto infoPersistencia = _validarViabilidadCompletarInfoPersistencia.ObtenerValidarViabilidadCompletarInfo(parametrosConsultaDto);
            return infoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar,
                                                   string usuario)
        {
            _validarViabilidadCompletarInfoPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
