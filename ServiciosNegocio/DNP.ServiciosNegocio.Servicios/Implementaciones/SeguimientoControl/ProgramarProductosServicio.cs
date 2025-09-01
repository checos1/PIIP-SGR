using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl
{
    public class ProgramarProductosServicio: IProgramarProductosServicio //xxxxx
    {
        private readonly IProgramarProductosPersistencia _ProgramarProductosPersistencia;

        #region Constructor

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public ProgramarProductosServicio(IProgramarProductosPersistencia ProgramarProductosPersistencia)
        {
            _ProgramarProductosPersistencia = ProgramarProductosPersistencia;
        }

        #endregion

        public string ObtenerListadoObjProdNiveles(string bpin)
        {
            return _ProgramarProductosPersistencia.ObtenerListadoObjProdNiveles(bpin);
        }

        public string GuardarProgramarProducto(ParametrosGuardarDto<ProgramarProductoDto> parametrosGuardar, string usuario)
        {
            return _ProgramarProductosPersistencia.GuardarProgramarProducto(parametrosGuardar, usuario);
        }
    }
}
