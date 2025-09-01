namespace DNP.ServiciosNegocio.Servicios.Interfaces.Requisitos
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Requisitos;

    public interface IRequisitosServicio
    {
        ServicioAgregarRequisitosDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
        ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request);
        void Guardar(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<ServicioAgregarRequisitosDto> ConstruirParametrosGuardar(HttpRequestMessage request);
    }
}
