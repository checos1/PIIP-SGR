namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Preguntas
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Preguntas;

    public interface IPreguntasPersonalizadasPersistencia
    {
        List<TematicaDto> ObtenerPreguntasEspecificas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasGenerales(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, out CuestionarioDto infoCuestionario);
        List<AgregarPreguntasDto> ObtenerAgregarPreguntas();
        DatosGeneralesProyectosDto ObtenerDatosGeneralesProyecto(int? pProyectoId, Guid pNivelId);
        ConfiguracionEntidadDto ObtenerConfiguracionEntidades(int? pProyectoId, Guid pNivelId);
        void GuardarDefinitivamente(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario);
        void DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia);
    }
}
