using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    public interface ICofinanciacionAgregarServicio
    {
        CofinanciacionProyectoDto ObtenerCofinanciacionAgregar(ParametrosConsultaDto parametrosConsulta);
        CofinanciacionProyectoDto ObtenerCofinanciacionAgregarPreview();
        ParametrosGuardarDto<CofinanciacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, CofinanciacionProyectoDto contenido);
        void Guardar(ParametrosGuardarDto<CofinanciacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
