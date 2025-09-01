using DNP.ServiciosNegocio.Dominio.Dto.Acciones;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Acciones
{
    public interface IEjecucionAccionTransaccionalServicios
    {
        string Usuario { get; set; }
        bool EjecutarAccion(AccionFormularioDto accion);
     
    }
}
