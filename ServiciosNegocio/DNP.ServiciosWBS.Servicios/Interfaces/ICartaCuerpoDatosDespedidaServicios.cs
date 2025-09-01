namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Tramites;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICartaCuerpoDatosDespedidaServicios
    {
        DatosConceptoDespedidaDto ObtenerCuerpoDatosDespedida(ParametrosConsultaDto parametrosConsultaDto);
        DatosConceptoDespedidaDto ObtenerCuerpoDatosDespedidaPreview();
        void Guardar(ParametrosGuardarDto<DatosConceptoDespedidaDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<DatosConceptoDespedidaDto> ConstruirParametrosGuardado(HttpRequestMessage request, DatosConceptoDespedidaDto contenido);
    }
}
