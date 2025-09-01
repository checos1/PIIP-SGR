
namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IDevolverProyectoServicio
    {
        DevolverProyectoDto ObtenerDevolverProyecto(ParametrosConsultaDto parametrosConsulta);
        ParametrosGuardarDto<DevolverProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, DevolverProyectoDto contenido);
        void Guardar(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);

    }
}
