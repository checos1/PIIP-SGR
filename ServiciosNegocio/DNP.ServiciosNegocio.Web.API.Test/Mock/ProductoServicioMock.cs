namespace DNP.ServiciosNegocio.Web.API.Test.Mock
{
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Proyectos;
    using System.Net.Http;
    using Dominio.Dto.Genericos;
    using ServiciosNegocio.Servicios.Interfaces.Formulario;

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



    }
}