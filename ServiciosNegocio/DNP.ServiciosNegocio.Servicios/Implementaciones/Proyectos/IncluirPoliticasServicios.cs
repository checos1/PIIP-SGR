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

    public class IncluirPoliticasServicios : ServicioBase<IncluirPoliticasDto>, IIncluirPoliticasServicios
    {
        private readonly IIncluirPoliticasPersistencia _IncluirPoliticasPersistencia;

        public IncluirPoliticasServicios(IIncluirPoliticasPersistencia incluirPoliticasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _IncluirPoliticasPersistencia = incluirPoliticasPersistencia;
        }

        public IncluirPoliticasDto ObtenerIncluirPoliticas(ParametrosConsultaDto parametrosConsulta)
        {
            _IncluirPoliticasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public IncluirPoliticasDto ObtenerIncluirPoliticasPreview()
        {
            return _IncluirPoliticasPersistencia.ObtenerIncluirPoliticasPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            _IncluirPoliticasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override IncluirPoliticasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _IncluirPoliticasPersistencia.ObtenerIncluirPoliticas(parametrosConsultaDto.Bpin);
        }
    }
}
