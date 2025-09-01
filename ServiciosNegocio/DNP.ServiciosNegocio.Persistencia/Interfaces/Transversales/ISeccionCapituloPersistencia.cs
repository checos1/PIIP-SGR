using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales
{
    public interface ISeccionCapituloPersistencia
    {
        List<SeccionCapituloDto> ObtenerListaCapitulosModificadosByMacroproceso(int idMacroproceso, int IdProyecto, Guid IdInstancia);
        List<SeccionCapituloDto> ObtenerListaCapitulosByMacroproceso(int idMacroproceso,Guid NivelId, Guid FlujoId);
        bool GuardarJustificacionCambios(CapituloModificado capituloModificados);
        CapituloModificado ObtenerSeccionCapitulo(string GuiMacroproceso, string nombreCapitulo, string nombreSeccion);
        RespuestaGeneralDto ValidarSeccionCapitulos(int idMacroproceso, int IdProyecto, Guid IdInstancia);
        CapituloModificado ObtenerCapitulosModificados(string capitulo, string seccion, string guiMacroproceso, int idProyecto, Guid guidInstancia);
        List<ErroresProyectoDto> ObtenerErroresProyecto(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia);
        List<ErroresProyectoDto> ObtenerErroresSeguimiento(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia);
        List<ErroresTramiteDto> ObtenerErroresTramite(Guid GuiMacroproceso, Guid guidInstancia, Guid accionId, string usuarioDNP,bool tieneCDP);
        List<ErroresTramiteDto> ObtenerErroresViabilidad(Guid GuiMacroproceso, int ProyectoId, Guid NivelId, Guid InstanciaId);
        List<SeccionesTramiteDto> ObtenerSeccionesTramite(Guid GuiMacroproceso, Guid guidInstancia);
        List<SeccionesTramiteDto> ObtenerSeccionesPorFase(Guid guidInstancia, Guid guidFaseNivel);
        SeccionesCapitulos EliminarCapituloModificado(CapituloModificado capituloModificados);
        List<ErroresPreguntasDto> ObtenerErroresAprobacionRol(Guid GuiMacroproceso, int idProyecto, Guid guidInstancia);
        bool FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada capituloModificados);
        List<ErroresProyectoDto> ObtenerErroresProgramacion(Guid guidInstancia, Guid accionId);
    }
}
