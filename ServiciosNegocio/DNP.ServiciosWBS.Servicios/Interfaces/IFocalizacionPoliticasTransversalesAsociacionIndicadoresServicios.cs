using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
using System.Net.Http;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios
    {
        PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(ParametrosConsultaDto parametrosConsulta);
        PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTIndicadoresDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTIndicadoresDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTIndicadoresDto contenido);
    }
}
