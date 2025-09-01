using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    public interface ISeguridadServicio
    {
        string PermisosAccionPaso(AccionFlujoDto accionFlujoDto);
    }
}
