namespace DNP.Backbone.Servicios.Interfaces.Preguntas
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Dominio.Dto.Preguntas;

    public interface IPreguntasPersonalizadasServicios
    {
        Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, string usuarioDnp);
        Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp);
        Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp);
        Task<DatosGeneralesProyectosDto> ObtenerDatosGeneralesProyecto(int? ProyectoId, Guid NivelId, string usuarioDnp);
        Task<ConfiguracionEntidadDto> ObtenerConfiguracionEntidades(int? ProyectoId, Guid NivelId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadas(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp); 
        Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadasCustomSGR(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp); 
        Task<RespuestaGeneralDto> DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia, string usuarioDnp);
        Task<ConceptosPreviosEmitidosDto> ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto, string usuarioDnp);
    }
}
