namespace DNP.Backbone.Servicios.Interfaces.SGR
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.SGR;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using DNP.Backbone.Dominio.Dto.SGR.Transversal;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;

    public interface ISGRViabilidadServicios
    {
        Task<ParametroDto> SGR_Transversal_LeerParametro(string parametro, string usuarioDNP);
        Task<List<ListaParametrosDto>> SGR_Transversal_LeerListaParametros(string usuarioDNP);
        Task<List<LstAcuerdoSectorClasificadorDto>> SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId, string usuarioDNP);

        Task<string> SGR_Acuerdo_GuardarProyecto(AcuerdoSectorClasificadorDto obj, string usuarioDNP);

        Task<List<ListaDto>> SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP);

        Task<LeerInformacionGeneralViabilidadDto> SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode);
        Task<List<LeerParametricasViabilidadDto>> SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_Viabilidad_GuardarInformacionBasica(InformacionBasicaViabilidadDto obj, string usuarioDNP);
        Task<string> SGR_Viabilidad_ObtenerPuntajeProyecto(System.Guid instanciaId, int entidadId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_Viabilidad_GuardarPuntajeProyecto(string puntajesProyecto, string usuarioDNP);
    }
}
