using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IDatosBasicosSGRServicio
    {
        DatosBasicosSGRDto ObtenerDatosBasicosSGR(ParametrosConsultaDto parametrosConsulta);
        DatosBasicosSGRDto ObtenerDatosBasicosSGRPreview();
        ParametrosGuardarDto<DatosBasicosSGRDto> ConstruirParametrosGuardado(HttpRequestMessage request, DatosBasicosSGRDto contenido);
        void Guardar(ParametrosGuardarDto<DatosBasicosSGRDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
