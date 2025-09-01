namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Genericos;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;

    [ExcludeFromCodeCoverage]
    public class ProductoServicioMock : IProductosServicio
    {
        public void Guardar(ParametrosGuardarDto<ProyectoDto> parametrosGuardar) {  }
        
        public ProyectoDto ObtenerProductos(ParametrosConsultaServicioFormularioDto parametrosConsultaServicioFormulario)
        {
            return new ProyectoDto();
        }
        public ProyectoDto ObtenerProductosPreview()
        {
            return new ProyectoDto();
        }
        public void Guardar(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente) {  }
        public ParametrosGuardarDto<ProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoDto contenido) { return new ParametrosGuardarDto<ProyectoDto>();}

        public ProyectoDto Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            return new ProyectoDto();
        }

        public ProyectoDto ObtenerProductos(ParametrosConsultaDto parametrosConsulta)
        {
            return new ProyectoDto();
        }
    }
}