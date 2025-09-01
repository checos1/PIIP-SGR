using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IRegionalizacionMetasyRecursosPersistencia
    {
        RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursos(string bpin);
        RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<RegMetasRecursosDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
