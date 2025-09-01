using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    public interface IFuenteCofinanciacionServicio
    {
        FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(ParametrosConsultaDto parametrosConsulta);
        FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview();
        ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, FuenteCofinanciacionProyectoDto contenido);
        void Guardar(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
