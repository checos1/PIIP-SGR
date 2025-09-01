using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using System;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad
{
    public interface IViabilidadServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        LeerInformacionGeneralViabilidadDto SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode);
        string SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGR_Viabilidad_GuardarInformacionBasica(string json, string usuario);
        ResultadoProcedimientoDto SGR_Viabilidad_FirmarUsuario(string json, string usuario);
        ResultadoProcedimientoDto SGR_Viabilidad_EliminarFirmaUsuario(string json, string usuario);
        string SGR_Viabilidad_ObtenerPuntajeProyecto(Guid instanciaId, int entidadId);
        ResultadoProcedimientoDto SGR_Viabilidad_GuardarPuntajeProyecto(string json, string usuario);
    }
}
