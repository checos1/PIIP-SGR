using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl
{
    public interface IProgramarProductosPersistencia
    {
        string ObtenerListadoObjProdNiveles(string bpin);
        string GuardarProgramarProducto(ParametrosGuardarDto<ProgramarProductoDto> parametrosGuardar, string usuario);
        //void EditarProgramarActividad(string usuario, ProgramarProductoDto ProyectosDto);
    }
}
