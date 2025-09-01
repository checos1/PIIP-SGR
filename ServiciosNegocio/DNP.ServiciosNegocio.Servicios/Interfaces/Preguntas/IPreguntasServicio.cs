using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IPreguntasServicio
    {
        ServicioPreguntasDto ObtenerPreguntasPreview();
        ServicioPreguntasDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
        ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request);
        ParametrosGuardarDto<ServicioPreguntasDto> ConstruirParametrosGuardar(HttpRequestMessage request);
        void Guardar(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
