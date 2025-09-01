using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Castle.Core.Internal;
    using Dominio.Dto.Preguntas;
    using Persistencia.Interfaces.Preguntas;
    using Newtonsoft.Json;
    using Comunes.Dto.Formulario;

    public class PreguntasPersistenciaMock : IPreguntasPersistencia
    {
        private List<object> opcionesRespuesta;
        private List<object> obligaObservacion;
        private object valoresRespuesta;
        private object valoresObserva;
        private void SeteoOpciones()
        {
            valoresRespuesta = JsonConvert.DeserializeObject<object>("[{\"OpcionId\":1,\"Valor\":\"SI\"},{\"OpcionId\":2,\"Valor\":\"NO\"},{\"OpcionId\":3,\"Valor\":\"No Aplica\"}]");
            opcionesRespuesta = new List<object>();
            opcionesRespuesta.Add(valoresRespuesta);
        }
        private void SeteoObligaObservacion()
        {
            valoresObserva = JsonConvert.DeserializeObject<object>("[{\"OpcionId\":1,\"Valor\":\"NO\"},{\"OpcionId\":3,\"Valor\":\"No Aplica\"}]");
            obligaObservacion = new List<object>();
            obligaObservacion.Add(valoresObserva);
        }
        public List<PreguntasDto> ObtenerPreguntasEspecificas(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId,  out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            if (!bPin.IsNullOrEmpty())
            {
                SeteoOpciones();
                SeteoObligaObservacion();
                infoCuestionario = new CuestionarioDto()
                {
                    CodigoBPIN = "0",
                    Cuestionario = 1,
                    CR = 2,
                    Fase = Guid.NewGuid(),
                    EntidadDestino = 636,
                    ObservacionCuestionario = "Observacion cuestionario",
                    CumpleCuestionario = null,
                    Fecha = DateTime.Now,
                    Usuario = "Usuario",
                    AgregarRequisitos = false
                };
                return new List<PreguntasDto>()
                {
                    new PreguntasDto()
                    {
                        IdPregunta= 3,
                        
                        Tipo= "Especifico",
                        Subtipo= "Cultura",
                        Tematica= null,
                        OrdenTematica= null,
                        Pregunta= "Pregunta 3",
                        OrdenPregunta= 1,
                        Explicacion= "Explicacion Pregunta 3",
                        OpcionesRespuestas= opcionesRespuesta,
                        OpcionesRespuestasSeleccionado= "Verdadero",
                        ObligaObservacion= null,
                        ObservacionPregunta= "Observación Pregunta 3",
                        Cabecera= "Cabecera Pregunta 3",
                        Nota= "Nota Pregunta 3",
                        CumpleEn= "Verdadero"
                    }
                };
            }
            else
            {
                return new List<PreguntasDto>();
            }
        }
        public List<PreguntasDto> ObtenerPreguntasGenerales(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            if (bPin.Equals("2017011000236"))
            {
                infoCuestionario = new CuestionarioDto()
                {
                    CodigoBPIN = "0",
                    Cuestionario = 1,
                    CR = 2,
                    Fase = Guid.NewGuid(),
                    EntidadDestino = 636,
                    ObservacionCuestionario = "Observacion cuestionario",
                    CumpleCuestionario = null,
                    Fecha = DateTime.Now,
                    Usuario = "Usuario",
                    AgregarRequisitos = false
                };
                return new List<PreguntasDto>()
                {
                    new PreguntasDto()
                    {
                        IdPregunta= 1,
                        Tipo= "General",
                        Subtipo= null,
                        Tematica= "Tematica 1",
                        OrdenTematica= 1,
                        Pregunta= "Pregunta 1",
                        OrdenPregunta= 1,
                        Explicacion= "Explicacion Pregunta 1",
                        OpcionesRespuestas= opcionesRespuesta,
                        OpcionesRespuestasSeleccionado= "No",
                        ObligaObservacion= obligaObservacion,
                        ObservacionPregunta= "Observación Pregunta 1",
                        Cabecera= "Cabecera Pregunta 1",
                        Nota= "Nota Pregunta 1",
                        CumpleEn= "Si"
                    },
                    new PreguntasDto()
                    {
                        IdPregunta= 2,
                        Tipo= "General",
                        Subtipo= null,
                        Tematica= "Tematica 1",
                        OrdenTematica= 1,
                        Pregunta= "Pregunta 2",
                        OrdenPregunta= 2,
                        Explicacion= "Explicacion Pregunta 2",
                        OpcionesRespuestas= opcionesRespuesta,
                        OpcionesRespuestasSeleccionado= "Verdadero",
                        ObligaObservacion= null,
                        ObservacionPregunta= "Observación Pregunta 2",
                        Cabecera= "Cabecera Pregunta 2",
                        Nota= "Nota Pregunta 2",
                        CumpleEn= "Verdadero"
                    }
                };
            }
            else
            {
                return new List<PreguntasDto>();
            }
        }

        public List<AgregarPreguntasDto> ObtenerAgregarPreguntas()
        {
            return new List<AgregarPreguntasDto>
            {
                new AgregarPreguntasDto
                {
                    AtributoId = 18,
                    Atributo = "Acuerdo",
                    AtributoPadre = null,
                    PreguntaId = 32,
                    Pregunta = "4. Cuando el proyecto se localice en resguardos indígenas o territorios colectivos, o sea presentado por el representante de las comunidades indígenas, negras, afrocolombianas, raizales y palenqueras, debe presentarse certificado suscrito por el secretario de planeación en el cual conste que el plan de vida o plan de etnodesarrollo está en concordancia con el Plan Nacional de Desarrollo y con el plan de desarrollo de las entidades territoriales.",
                    Explicacion =null,
                    ValorOpcion = "Acuerdo 3",
                    OpcionId = 195,
                    Padre = null
                },
                new AgregarPreguntasDto
                {
                    AtributoId = 12,
                    Atributo = "Sector",
                    AtributoPadre = 18,
                    PreguntaId = 32,
                    Pregunta = "4. Cuando el proyecto se localice en resguardos indígenas o territorios colectivos, o sea presentado por el representante de las comunidades indígenas, negras, afrocolombianas, raizales y palenqueras, debe presentarse certificado suscrito por el secretario de planeación en el cual conste que el plan de vida o plan de etnodesarrollo está en concordancia con el Plan Nacional de Desarrollo y con el plan de desarrollo de las entidades territoriales.",
                    Explicacion =null,
                    ValorOpcion = "Sector 3",
                    OpcionId = 196,
                    Padre = 195
                },

                new AgregarPreguntasDto
                {
                    AtributoId = 17,
                    Atributo = "Clasificacion",
                    AtributoPadre = 12,
                    PreguntaId = 32,
                    Pregunta = "4. Cuando el proyecto se localice en resguardos indígenas o territorios colectivos, o sea presentado por el representante de las comunidades indígenas, negras, afrocolombianas, raizales y palenqueras, debe presentarse certificado suscrito por el secretario de planeación en el cual conste que el plan de vida o plan de etnodesarrollo está en concordancia con el Plan Nacional de Desarrollo y con el plan de desarrollo de las entidades territoriales.",
                    Explicacion =null,
                    ValorOpcion = "Clasificación 3",
                    OpcionId = 197,
                    Padre = 196
                }

            };
        }

        public ServicioPreguntasDto ObtenerPreguntasPreview()
        {
            return new ServicioPreguntasDto();
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
        }
        public void GuardarTemporalmente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
        }
    }
}
