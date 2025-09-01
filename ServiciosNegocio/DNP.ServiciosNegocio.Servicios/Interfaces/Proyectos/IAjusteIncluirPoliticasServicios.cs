using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Net.Http;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
    public interface IAjusteIncluirPoliticasServicios
    {
        IncluirPoliticasDto ObtenerAjusteIncluirPoliticas(ParametrosConsultaDto parametrosConsulta);
        IncluirPoliticasDto ObtenerAjusteIncluirPoliticasPreview();
        void Guardar(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<IncluirPoliticasDto> ConstruirParametrosGuardado(HttpRequestMessage request, IncluirPoliticasDto contenido);
    }
}
