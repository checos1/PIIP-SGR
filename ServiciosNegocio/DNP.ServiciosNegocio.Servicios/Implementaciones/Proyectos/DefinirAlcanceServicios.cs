namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Proyectos;
    using Persistencia.Interfaces.Proyectos;
    using Interfaces.Proyectos;

    public class DefinirAlcanceServicios : ServicioBase<AlcanceDto>, IDefinirAlcanceServicios
    {
        private readonly IDefinirAlcancePersistencia _AlcancePersistencia;

        public DefinirAlcanceServicios(IDefinirAlcancePersistencia alcancePersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _AlcancePersistencia = alcancePersistencia;
        }

        public AlcanceDto ObtenerDefinirAlcance(ParametrosConsultaDto parametrosConsultaDto)
        {
            _AlcancePersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override AlcanceDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AlcanceDto infoPersistencia = _AlcancePersistencia.ObtenerDefinirAlcance(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AlcanceDto ObtenerDefinirAlcancePreview()
        {
            return _AlcancePersistencia.ObtenerDefinirAlcancePreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, string usuario)
        {
            _AlcancePersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
