namespace DNP.Backbone.Servicios.Interfaces.Administracion
{
    using System.Threading.Tasks;
    using DNP.Backbone.Dominio.Dto.Administracion;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;

    public interface IEjecutorServicio
    {
        Task<EjecutorDto> ConsultarEjecutor(string nit, string usuarioDnp);
        Task<bool> GuardarEjecutor(EjecutorDto Obj, string UsuarioDNP);
    }

    
}
