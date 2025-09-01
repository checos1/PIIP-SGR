using DNP.ServiciosNegocio.Dominio.Dto.Entidades;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Entidades
{
    public interface IEntidadAccionesServicio
    {
        EntidadAcciones ObtenerEntidadesAcciones(EntidadAccionesEntrada parametrosConsulta);
    }
}
