using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;

namespace DNP.ServiciosWBS.Test.Mocks
{
    public class RegionalizacionMetasyRecursosMock
    {
        public RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return new RegMetasRecursosDto()
            {
                BPIN = "202000000000005"
            };

        }
        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<RegMetasRecursosDto> parametrosGuardar, string usuario)
        {
        }
        public RegMetasRecursosDto DiligenciarRegionalizacionMetasyRecursosPreview()
        {
            return new RegMetasRecursosDto()
            {
                BPIN = "202000000000005"
            };
        }
    }
}

