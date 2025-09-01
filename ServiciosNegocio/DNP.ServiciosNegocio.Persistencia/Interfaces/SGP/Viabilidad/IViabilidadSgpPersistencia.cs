using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using System;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad
{

    public interface IViabilidadSgpPersistencia
    {
        string SGPTransversalLeerParametro(string Parametro);
        LeerInformacionGeneralViabilidadDto SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode);
        string SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGPViabilidadGuardarInformacionBasica(string json, string usuario);
        ResultadoProcedimientoDto SGPViabilidadFirmarUsuario(string json, string usuario);
        IEnumerable<ProyectoViabilidadInvolucradosDto> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId);
        IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId);
        ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradosSGP(int id);
        ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucradosSGP(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario);
        EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId);
        EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId);
    }
}
