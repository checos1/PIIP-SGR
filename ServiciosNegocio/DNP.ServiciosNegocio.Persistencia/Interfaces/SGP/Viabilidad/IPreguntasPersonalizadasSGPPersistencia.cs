using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad
{
    public interface IPreguntasPersonalizadasSGPPersistencia
    {
        List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGPCustom(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGPCustomSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGPCustom(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGPCustomSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<AgregarPreguntasDto> ObtenerAgregarPreguntas();
        void GuardarPreguntasPersonalizadasCustomSGP(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario);        
    }
}
