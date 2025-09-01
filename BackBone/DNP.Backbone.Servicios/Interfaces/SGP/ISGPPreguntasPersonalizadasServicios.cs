namespace DNP.Backbone.Servicios.Interfaces.SGP
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Dominio.Dto.Preguntas;

    public interface ISGPPreguntasPersonalizadasServicios
    {
        Task<ServicioPreguntasPersonalizadasDto> ConsultarPreguntasSGP(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, string usuarioDnp);
        Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasSGPComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp);
        Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasSGPComponenteSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp);
        Task<DatosGeneralesProyectosDto> ObtenerDatosGeneralesProyecto(int? ProyectoId, Guid NivelId, string usuarioDnp);
        Task<ConfiguracionEntidadDto> ObtenerConfiguracionEntidades(int? ProyectoId, Guid NivelId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadas(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp); 
        Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadasSGP(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp); 
        Task<RespuestaGeneralDto> DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia, string usuarioDnp);        
    }
}
