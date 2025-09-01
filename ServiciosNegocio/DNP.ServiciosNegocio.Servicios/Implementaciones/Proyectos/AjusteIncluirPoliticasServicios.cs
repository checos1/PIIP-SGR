using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos
{
    public class AjusteIncluirPoliticasServicios : ServicioBase<IncluirPoliticasDto>, IAjusteIncluirPoliticasServicios
    {
        private readonly IAjusteIncluirPoliticasPersistencia _ajusteIncluirPoliticasPersistencia;

    public AjusteIncluirPoliticasServicios(IAjusteIncluirPoliticasPersistencia ajusteIncluirPoliticasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
    {
            _ajusteIncluirPoliticasPersistencia = ajusteIncluirPoliticasPersistencia;
    }

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticas(ParametrosConsultaDto parametrosConsulta)
        {
            _ajusteIncluirPoliticasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public IncluirPoliticasDto ObtenerAjusteIncluirPoliticasPreview()
        {
            return _ajusteIncluirPoliticasPersistencia.ObtenerAjusteIncluirPoliticasPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            _ajusteIncluirPoliticasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override IncluirPoliticasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _ajusteIncluirPoliticasPersistencia.ObtenerAjusteIncluirPoliticas(parametrosConsultaDto.Bpin);
        }
    }
}
