using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales
{
    public interface ITransversalServicioSGP
    {        
        EncabezadoSGPDto ObtenerEncabezadoSGP(ParametrosEncabezadoSGP parametros);        
    }
}
