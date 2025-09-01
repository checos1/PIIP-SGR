using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite
{
    public interface ITramiteProyectoSGPServicio
    {
        ParametrosGuardarDto<DatosTramiteProyectosDto> ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto contenido);
        string ObtenerProyectosTramiteNegocio(int TramiteId);
        TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        string ValidacionProyectosTramiteNegocio(int TramiteId);
    }
}
