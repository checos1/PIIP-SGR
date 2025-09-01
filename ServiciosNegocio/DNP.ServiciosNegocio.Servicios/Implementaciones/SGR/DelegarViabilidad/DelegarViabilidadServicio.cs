using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.DelegarViabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
using System;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.DelegarViabilidad
{
    public class DelegarViabilidadServicio : IDelegarViabilidadServicio
    {
        private readonly IDelegarViabilidadPersistencia _delegarViabilidadPersistencia;
        public DelegarViabilidadServicio(IDelegarViabilidadPersistencia delegarViabilidadPersistencia)
        {
            _delegarViabilidadPersistencia = delegarViabilidadPersistencia;
        }
        public string SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId)
        {
            return _delegarViabilidadPersistencia.SGR_DelegarViabilidad_ObtenerProyecto(bpin, instanciaId);
        }

        public string SGR_DelegarViabilidad_ObtenerEntidades(string bpin)
        {
            return _delegarViabilidadPersistencia.SGR_DelegarViabilidad_ObtenerEntidades(bpin);
        }

        public Task<ReponseHttp> SGR_DelegarViabilidad_Registrar(DelegarViabilidadDto json, string usuario)
        {
            try
            {
                _delegarViabilidadPersistencia.SGR_DelegarViabilidad_Registrar(json, usuario);

                return Task.FromResult(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }
    }
}
