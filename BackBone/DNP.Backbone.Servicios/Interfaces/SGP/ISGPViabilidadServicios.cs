namespace DNP.Backbone.Servicios.Interfaces.SGP
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.SGP;
    using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
    using DNP.Backbone.Dominio.Dto.SGP.Transversal;
    using System;

    public interface ISGPViabilidadServicios
    {
        Task<ParametroDto> SGPTransversalLeerParametro(string parametro, string usuarioDNP);
        Task<List<LstAcuerdoSectorClasificadorDto>> SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId, string usuarioDNP);

        Task<string> SGPAcuerdoGuardarProyecto(AcuerdoSectorClasificadorSGPDto obj, string usuarioDNP);

        Task<List<ListaDto>> SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP);

        Task<LeerInformacionGeneralViabilidadDto> SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode);
        Task<List<LeerParametricasViabilidadDto>> SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGPViabilidadGuardarInformacionBasica(InformacionBasicaViabilidadSGPDto obj, string usuarioDNP);
        Task<string> GuardarProyectoViabilidadInvolucradosSGP(ProyectoViabilidadInvolucradosSGPDto objProyectoViabilidadInvolucradosDto, string usuarioDNP);
        Task<string> EliminarProyectoViabilidadInvolucradosSGP(int id, string usuarioDnp);
        Task<List<ProyectoViabilidadInvolucradosSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp);
        Task<List<ProyectoViabilidadInvolucradosFirmaSGPDto>> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp);
        Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId, string usuarioDnp);
        Task<EntidadDestinoResponsableFlujoSgpDto> SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId, string usuarioDnp);
    }
}
