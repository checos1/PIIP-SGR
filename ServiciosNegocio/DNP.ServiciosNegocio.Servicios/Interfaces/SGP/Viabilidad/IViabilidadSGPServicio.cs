using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad
{
    public interface IViabilidadSGPServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
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
