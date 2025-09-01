using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    public class SeguridadServicio : ISeguridadServicio
    {

        private readonly ISeguridadPersistencia _SeguridadPersistencia;

        #region Constructor


        public SeguridadServicio(ISeguridadPersistencia SeguridadPersistencia)
        {
            _SeguridadPersistencia = SeguridadPersistencia;
        }

        #endregion

        public string PermisosAccionPaso(AccionFlujoDto accionFlujoDto)
        {
            return _SeguridadPersistencia.PermisosAccionPaso(accionFlujoDto);
        }
    }
}
