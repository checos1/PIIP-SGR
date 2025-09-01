namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Preguntas
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Preguntas;

    public interface IPreguntasPersistencia
    {
        List<PreguntasDto> ObtenerPreguntasEspecificas (string bPin, Guid nivelId, Guid instanciaId, Guid formularioId, out CuestionarioDto infoCuestionario);
        List<PreguntasDto> ObtenerPreguntasGenerales (string bPin, Guid nivelId, Guid instanciaId, Guid formularioId, out CuestionarioDto infoCuestionario);
        List<AgregarPreguntasDto> ObtenerAgregarPreguntas ();
        ServicioPreguntasDto ObtenerPreguntasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario);
        void GuardarTemporalmente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario);
    }
}
