using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Transversales
{
    public interface ITransversalPersistenciaSGP
    {
        
        EncabezadoSGPDto ObtenerEncabezadoSGP(ParametrosEncabezadoSGP parametros);

    }
}
