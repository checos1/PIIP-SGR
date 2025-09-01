namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
	using System.Collections.Generic;

	public interface ILocalizacionServicios
    {
        LocalizacionProyectoDto ObtenerLocalizacion(ParametrosConsultaDto parametrosConsultaDto);
        LocalizacionProyectoDto ObtenerLocalizacionProyectos(string bpin);
        LocalizacionProyectoDto ObtenerLocalizacionPreview();
        void Guardar(ParametrosGuardarDto<LocalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<LocalizacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, LocalizacionProyectoDto contenido);
        ResultadoProcedimientoDto GuardarLocalizacion(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario);
    }
}
