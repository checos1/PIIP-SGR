using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.DiligenciarFuentes;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.DiligenciarFuentes
{
    public class DiligenciarFuentesPersistencia : Persistencia, IDiligenciarFuentesPersistencia
    {

        public DiligenciarFuentesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
    {
        Mapper.Reset();

    }

        public DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesAgregar(string bpin)
        {
            throw new System.NotImplementedException();
        }
    }
}
