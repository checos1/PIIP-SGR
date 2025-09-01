namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Preguntas
{
    using Comunes;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Enum;
    using Dominio.Dto.Preguntas;
    using Interfaces.Preguntas;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Preguntas;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Configuration;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class PreguntasServicio : ServicioBase<ServicioPreguntasDto>, IPreguntasServicio
    {
        private readonly IPreguntasPersistencia _preguntaPersistencia;

        public PreguntasServicio(IPreguntasPersistencia preguntaPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _preguntaPersistencia = preguntaPersistencia;
        }

        public override ServicioPreguntasDto Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        public ServicioPreguntasDto ObtenerPreguntasPreview()
        {
            return _preguntaPersistencia.ObtenerPreguntasPreview();
        }
        protected override ServicioPreguntasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            CuestionarioDto infoCuestionario = null;

            ServicioPreguntasDto preguntas = new ServicioPreguntasDto();
            preguntas.PreguntasEspecificas = new List<PreguntasDto>();
            preguntas.PreguntasGenerales = new List<PreguntasDto>();
            preguntas.AgregarPreguntasRequisitos = new List<AgregarPreguntaRequisito>();

            var nivelId = parametrosConsultaDto.IdNivel;
            var bPin = parametrosConsultaDto.Bpin;
            var instanciaId = parametrosConsultaDto.InstanciaId;
            var formularioId = parametrosConsultaDto.FormularioId;

            var tipoGrilla = ObtenerConfiguracionFormulario(formularioId, parametrosConsultaDto.Token);

            if (tipoGrilla == TipoGrillaPreguntasEnum.Especificas || tipoGrilla == TipoGrillaPreguntasEnum.Generales || tipoGrilla == TipoGrillaPreguntasEnum.TodosLosTipos)
            {
                var result = _preguntaPersistencia.ObtenerAgregarPreguntas();
                preguntas.AgregarPreguntasRequisitos = getAgregarPreguntas(preguntas, result);
            }

            if (tipoGrilla == TipoGrillaPreguntasEnum.Generales || tipoGrilla == TipoGrillaPreguntasEnum.TodosLosTipos)
            {
                preguntas.PreguntasGenerales = _preguntaPersistencia.ObtenerPreguntasGenerales(bPin, nivelId, instanciaId, formularioId, out infoCuestionario);
                if (infoCuestionario != null)
                {
                    preguntas.CodigoBPIN = infoCuestionario.CodigoBPIN;
                    preguntas.Cuestionario = infoCuestionario.Cuestionario;
                    preguntas.CR = infoCuestionario.CR;
                    preguntas.Fase = infoCuestionario.Fase;
                    preguntas.EntidadDestino = infoCuestionario.EntidadDestino;
                    preguntas.ObservacionCuestionario = infoCuestionario.ObservacionCuestionario;
                    preguntas.Fecha = infoCuestionario.Fecha;
                    preguntas.Usuario = infoCuestionario.Usuario;
                    preguntas.AgregarRequisitos = infoCuestionario.AgregarRequisitos;
                    preguntas.CumpleCuestionario = infoCuestionario.CumpleCuestionario;
                }
            }

            if (tipoGrilla == TipoGrillaPreguntasEnum.Especificas || tipoGrilla == TipoGrillaPreguntasEnum.TodosLosTipos)
            {
                preguntas.PreguntasEspecificas = _preguntaPersistencia.ObtenerPreguntasEspecificas(bPin, nivelId, instanciaId, formularioId, out infoCuestionario);
                if (infoCuestionario != null)
                {
                    preguntas.CodigoBPIN = infoCuestionario.CodigoBPIN;
                    preguntas.Cuestionario = infoCuestionario.Cuestionario;
                    preguntas.CR = infoCuestionario.CR;
                    preguntas.Fase = infoCuestionario.Fase;
                    preguntas.EntidadDestino = infoCuestionario.EntidadDestino;
                    preguntas.ObservacionCuestionario = infoCuestionario.ObservacionCuestionario;
                    preguntas.Fecha = infoCuestionario.Fecha;
                    preguntas.Usuario = infoCuestionario.Usuario;
                    preguntas.AgregarRequisitos = infoCuestionario.AgregarRequisitos;
                    preguntas.CumpleCuestionario = infoCuestionario.CumpleCuestionario;
                }
            }

            return preguntas;
        }

        private static List<AgregarPreguntaRequisito> getAgregarPreguntas(ServicioPreguntasDto preguntas, List<AgregarPreguntasDto> result)
        {
            var requisitos = new List<AgregarPreguntaRequisito>();

            foreach (var item in result.Where(p => p.AtributoPadre == null))
            {
                AgregarPreguntaRequisito requisito = requisitos.FirstOrDefault(p => p.Id == item.OpcionId);
                if (requisito == null && item.AtributoId != 6)
                {
                    requisito = new AgregarPreguntaRequisito
                    {
                        Id = item.OpcionId,
                        AtributoPadre = item.AtributoPadre,
                        Name = item.ValorOpcion,
                        Items = new List<AgregarPreguntaSector>()
                    };
                    foreach (var obj in result.Where(p => p.Padre == item.OpcionId))
                    {
                        if (requisito.Items.Count(c => c.Id == obj.OpcionId) == 0)
                        {
                            var sector = new AgregarPreguntaSector
                            {
                                Id = obj.OpcionId,
                                AtributoPadre = obj.AtributoPadre,
                                Name = obj.ValorOpcion,
                                Items = new List<AgregarPreguntaClassificacion>()
                            };
                            requisito.Items.Add(sector);
                            foreach (var obj2 in result.Where(p => p.Padre == sector.Id))
                            {
                                var classificacion = sector.Items.FirstOrDefault(c => c.Id == obj2.OpcionId);
                                if (classificacion == null)
                                {
                                    classificacion = new AgregarPreguntaClassificacion
                                    {
                                        Id = obj2.OpcionId,
                                        AtributoPadre = obj2.AtributoPadre,
                                        Name = obj2.ValorOpcion,
                                        Items = new List<PreguntasDto>()
                                    };
                                    sector.Items.Add(classificacion);
                                }

                                var listaOpciones = result.Where(x => x.PreguntaId == obj2.PreguntaId && x.AtributoId == 6).ToList();
                                List<object> lista = new List<object>();

                                listaOpciones.ForEach(x =>
                                {
                                    var resp = $"\"OpcionId\":\"{x.OpcionId}\",\"ValorOpcion\":\"{x.ValorOpcion}\"";

                                    lista.Add(JsonConvert.DeserializeObject<object>("{" + resp + "}"));
                                });


                                classificacion.Items.Add(new PreguntasDto
                                {
                                    IdPregunta = obj2.PreguntaId,
                                    Pregunta = obj.Pregunta,
                                    Sector = obj.ValorOpcion,
                                    Acuerdo = item.ValorOpcion,
                                    Clasificacion = obj2.ValorOpcion,
                                    OpcionesRespuestas = lista
                                });
                            }
                        }
                    }
                    requisitos.Add(requisito);
                }
            }
            return requisitos;
        }

        public override void Guardar(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                                     bool guardarTemporalmente)
        {
            string mensajeAccion;
            if (guardarTemporalmente)
            {
                GuardadoTemporal(parametrosGuardar, parametrosAuditoria.Usuario);
                mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoTemporal, parametrosGuardar.Contenido);
            }
            else
            {
                GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);
                mensajeAccion = string.Format(ServiciosNegocioRecursos.GuardadoDefinitivo, parametrosGuardar.Contenido);
            }
            GenerarAuditoria(parametrosGuardar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Creacion,
                             mensajeAccion);
        }
        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
            _preguntaPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
        private void GuardadoTemporal(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
            _preguntaPersistencia.GuardarTemporalmente(parametrosGuardar, usuario);
        }

        public ParametrosGuardarDto<ServicioPreguntasDto> ConstruirParametrosGuardar(HttpRequestMessage request)
        {
            var parametrosGuardar = new ParametrosGuardarDto<ServicioPreguntasDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;

            if (request.Headers.Contains("piip-idFormulario"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idFormulario").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.FormularioId = valor;

            return parametrosGuardar;
        }

        private TipoGrillaPreguntasEnum ObtenerConfiguracionFormulario(Guid formularioId, string token)
        {
            try
            {
                TipoGrillaPreguntasEnum tipoGrilla = TipoGrillaPreguntasEnum.TodosLosTipos;
                int totalEspecificas = 0, totalGenerales = 0;

                string url = ConfigurationManager.AppSettings["ApiPiipCore"] + "/api/Formulario/ObtenerFormulariosporId?id=" + formularioId;
                var clienteHttp = new HttpClient();

                clienteHttp.DefaultRequestHeaders.Accept.Clear();
                clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clienteHttp.DefaultRequestHeaders.Add("Authorization", token);

                var respuesta = clienteHttp.GetAsync(url).Result;
                var configuracionFormulario = respuesta.Content.ReadAsAsync<ConfiguracionFormulario>();

                if (configuracionFormulario.Result == null)
                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorFormularioNoEncontrado);

                var disenoFormulario = !string.IsNullOrEmpty(configuracionFormulario.Result.Disenador) ?
                    JsonConvert.DeserializeObject<DisenoFormulario[]>(configuracionFormulario.Result.Disenador) : null;

                if (disenoFormulario != null)
                {
                    foreach (var contenedor in disenoFormulario)
                    {
                        if (contenedor != null && contenedor.Columnas != null && contenedor.Columnas.Count() > 0)
                        {
                            for (int i = 0; i < contenedor.Columnas.Count(); i++)
                            {
                                var especificas = Array.Find(contenedor.Columnas[i], c => c.Id == "PreguntasEspecificas");
                                if (especificas != null) totalEspecificas++;

                                var generales = Array.Find(contenedor.Columnas[i], c => c.Id == "PreguntasGenerales");
                                if (generales != null) totalGenerales++;
                            }
                        }
                    }
                }

                if (totalEspecificas > 0 && totalGenerales > 0)
                    tipoGrilla = TipoGrillaPreguntasEnum.TodosLosTipos;

                if (totalEspecificas > 0 && totalGenerales == 0)
                    tipoGrilla = TipoGrillaPreguntasEnum.Especificas;

                if (totalGenerales > 0 && totalEspecificas == 0)
                    tipoGrilla = TipoGrillaPreguntasEnum.Generales;

                if (totalEspecificas == 0 && totalGenerales == 0)
                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorNoExisteConfiguracionPreguntas);

                return tipoGrilla;
            }
            catch (ServiciosNegocioException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorObtenerConfiguracionFormulario, ex);
            }
        }
    }
}
