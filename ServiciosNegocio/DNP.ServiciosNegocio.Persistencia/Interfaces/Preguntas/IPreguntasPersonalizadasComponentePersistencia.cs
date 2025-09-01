using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Preguntas
{
    public interface IPreguntasPersonalizadasComponentePersistencia
    {
        List<TematicaDto> ObtenerPreguntasEspecificasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasGeneralesComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario);
        List<AgregarPreguntasDto> ObtenerAgregarPreguntas();
        void GuardarDefinitivamenteCustomSGR(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario);
        ConceptosPreviosEmitidosDto ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto);

    }
}
