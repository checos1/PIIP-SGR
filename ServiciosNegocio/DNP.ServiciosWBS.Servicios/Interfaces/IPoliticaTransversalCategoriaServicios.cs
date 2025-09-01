namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IPoliticaTransversalCategoriaServicios
    {
        PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoria(ParametrosConsultaDto parametrosConsultaDto);
        PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoriaPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTCategoriasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTCategoriasDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTCategoriasDto contenido);
    }
}
