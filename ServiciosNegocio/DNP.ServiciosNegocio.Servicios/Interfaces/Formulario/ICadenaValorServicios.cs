using DNP.ServiciosNegocio.Dominio.Dto.Formulario;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Formulario
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface ICadenaValorServicios
    {
        CadenaValorDto ObtenerCadenaValor(ParametrosConsultaDto parametrosConsulta);
        object ObtenerCadenaValorPreview();
        void Guardar(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<CadenaValorDto> ConstruirParametrosGuardado(HttpRequestMessage request, CadenaValorDto contenido);
    }
}
