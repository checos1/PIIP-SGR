using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosWBS.Persistencia.Interfaces.Transversales
{
    public interface ISeccionCapituloPersistencia
    {
        bool GuardarJustificacionCambios(CapituloModificado capituloModificados);
        CapituloModificado ObtenerSeccionCapitulo(string GuiMacroproceso, string nombreCapitulo, string nombreSeccion);
    }
}
