using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;
using System.Net.Http;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IRegionalizacionMetasyRecursosServicios
    {
        RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServicios(ParametrosConsultaDto parametrosConsulta);
        RegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServiciosPreview();
        ParametrosGuardarDto<RegMetasRecursosDto> ConstruirParametrosGuardado(HttpRequestMessage request, RegMetasRecursosDto contenido);
        void Guardar(ParametrosGuardarDto<RegMetasRecursosDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
