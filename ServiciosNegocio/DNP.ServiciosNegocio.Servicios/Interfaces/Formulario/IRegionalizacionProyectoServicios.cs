using System.Net.Http;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Formulario
{
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IRegionalizacionProyectoServicios
    {
        RegionalizacionProyectoDto ObtenerRegionalizacion(ParametrosConsultaDto parametrosConsulta);
        RegionalizacionProyectoDto ObtenerRegionalizacionPreview();
        void Guardar(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<RegionalizacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, RegionalizacionProyectoDto contenido);
    }
}
