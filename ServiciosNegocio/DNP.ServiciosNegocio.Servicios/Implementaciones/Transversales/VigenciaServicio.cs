using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    public class VigenciaServicio : IVigenciaServicio
    {
        private readonly IVigenciaPersistencia _vigenciaPersistencia;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="vigenciaPersistencia">Instancia de persistencia de vigencia</param>        
        public VigenciaServicio(IVigenciaPersistencia vigenciaPersistencia)
        {
            _vigenciaPersistencia = vigenciaPersistencia;
        }
        public List<int?> ObtenerVigencias()
        {
            return _vigenciaPersistencia.ObtenerVigencias();
        }
    }
}
