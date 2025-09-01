namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public interface ICadenaValorServicios
    {
        CadenaValorDto ObtenerCadenaValor(ParametrosConsultaDto parametrosConsulta);
        CadenaValorDto ObtenerCadenaValorPreview();
        void Guardar(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<CadenaValorDto> ConstruirParametrosGuardado(HttpRequestMessage request, CadenaValorDto contenido);
    }
}
