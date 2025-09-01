namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEstadoServicio
    {
        Task<List<EstadoDto>> ConsultarEstados();
        
    }
}
