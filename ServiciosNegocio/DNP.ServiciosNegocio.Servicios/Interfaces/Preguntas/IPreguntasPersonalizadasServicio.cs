using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas
{
    using System;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IPreguntasPersonalizadasServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        ServicioPreguntasPersonalizadasDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles);
        DatosGeneralesProyectosDto ObtenerDatosGeneralesProyecto(int? pProyectoId, Guid pNivelId);
        void Guardar(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        void GuardarCustomSGR(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request);
        ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> ConstruirParametrosGuardar(HttpRequestMessage request);
        ConfiguracionEntidadDto ObtenerConfiguracionEntidades(int? pProyectoId, Guid pNivelId);
        void DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia);
        ConceptosPreviosEmitidosDto ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto);
    }
}
