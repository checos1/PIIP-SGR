using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IGestionSeguimientoServicio
    {
        Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(GestionProyectoDto proyecto);

        Task<List<TransversalSeguimientoDto>> UnidadesMedida(string usuario);
    }
}
