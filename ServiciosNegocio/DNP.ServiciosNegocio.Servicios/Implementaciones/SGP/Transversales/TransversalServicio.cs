using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales;


namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Transversales 
{
    public class TransversalServicioSGP : ITransversalServicioSGP
    {
     

        private readonly ITransversalPersistenciaSGP _objetoPersistencia;

        public TransversalServicioSGP(ITransversalPersistenciaSGP TransversalSGPPersistencia)
        {
            _objetoPersistencia = TransversalSGPPersistencia;
        }

        public EncabezadoSGPDto ObtenerEncabezadoSGP(ParametrosEncabezadoSGP parametros)
        {
            return _objetoPersistencia.ObtenerEncabezadoSGP(parametros);
        }


        

    }
}
