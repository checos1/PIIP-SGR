namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesPoliticaTransversalCategoriaServicios
    {
        AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoria(ParametrosConsultaDto parametrosConsultaDto);
        AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoriaPreview();
        void Guardar(ParametrosGuardarDto<AjustesPoliticaTCategoriasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AjustesPoliticaTCategoriasDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesPoliticaTCategoriasDto contenido);
    }
}
