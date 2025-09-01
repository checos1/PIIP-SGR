using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using System;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad
{
    public interface ICTUSPersistencia
    {
        ConceptoCTUSDto SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId);
        ResultadoProcedimientoDto SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto json, string usuario);
        ResultadoProcedimientoDto SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto json, string usuario);
        UsuarioEncargadoCTUSDto SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId);
        ResultadoProcedimientoDto SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto json, string usuario);
        RolApruebaCTUSDto SGR_CTUS_LeerRolDirectorProyectoCtus(int proyectoId, Guid instanciaPadreId);
        string ValidarInstanciaCTUSNoFinalizada(int idProyecto);

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="user"></param>   
        /// <returns>bool</returns> 
        bool SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string user);
    }
}
