using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IProgramarProductosServicio
    {
        Task<string> ObtenerListadoObjProdNiveles(string bpin, string usuarioDNP);
        Task<string> GuardarProgramarProducto(ProgramarProductoDto programarProducto);        
    }
}
