using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad
{
    using System;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IPreguntasPersonalizadasSGPServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        ServicioPreguntasPersonalizadasDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles);
        ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponenteSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles);
        DatosGeneralesProyectosDto ObtenerDatosGeneralesProyecto(int? pProyectoId, Guid pNivelId);
        void Guardar(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        void GuardarCustomSGP(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request);
        ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> ConstruirParametrosGuardar(HttpRequestMessage request);
        ConfiguracionEntidadDto ObtenerConfiguracionEntidades(int? pProyectoId, Guid pNivelId);
        void DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia);        
    }
}
