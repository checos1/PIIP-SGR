
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosNegocio.Comunes.Dto;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
       public interface IPoliticasTransversalesCrucePoliticasServicios
    {
        PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticas(ParametrosConsultaDto parametrosConsultaDto);
        PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasPreview();
        void Guardar(ParametrosGuardarDto<PoliticasTCrucePoliticasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticasTCrucePoliticasDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticasTCrucePoliticasDto contenido);
    }
}
