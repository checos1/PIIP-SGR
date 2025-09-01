using System;
using System.Threading.Tasks;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SGR.CTUS;

namespace DNP.Backbone.Servicios.Interfaces.SGR
{
    public interface ISGRCtusServicios
    {
        Task<ConceptoCTUSDto> SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto obj, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto obj, string usuarioDNP);
        Task<UsuarioEncargadoCTUSDto> SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto obj, string usuarioDNP);

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="usuarioDnp"></param> 
        /// <returns>int</returns> 
        Task<bool> SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string usuarioDnp);
    }
}
