using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{
    public interface IAjusteIncluirPoliticasPersistencia
    {
        IncluirPoliticasDto ObtenerAjusteIncluirPoliticas(string bpin);
        IncluirPoliticasDto ObtenerAjusteIncluirPoliticasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
