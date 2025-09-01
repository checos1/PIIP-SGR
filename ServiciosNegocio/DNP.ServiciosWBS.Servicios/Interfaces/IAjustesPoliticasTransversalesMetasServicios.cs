namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using DNP.ServiciosNegocio.Comunes.Dto;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;    

    public interface IAjustesPoliticasTransversalesMetasServicios
    {        
        AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetas(ParametrosConsultaDto parametrosConsulta);
        AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetasPreview();
        void Guardar(ParametrosGuardarDto<AjustesPoliticaTMetasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AjustesPoliticaTMetasDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesPoliticaTMetasDto contenido);
    }
}
