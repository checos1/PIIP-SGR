namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;

    public class ValidarViabilidadCompletarInfoServiciosMock : IValidarViabilidadCompletarInfoServicios
    {
        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017011000236")
            {
                var auxTematicas = new List<TematicaDto>();
                var auxMensajesError = new List<MensajeErrorDto>();



                auxMensajesError.Add(new MensajeErrorDto()
                {
                    MensajeError = "Para la vigencia 2017, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"


                });
                auxMensajesError.Add(new MensajeErrorDto()
                {
                    MensajeError = "Para la  vigencia 2018, la sumatoria del valor solicitado de 20,600,000.00 de las fuentes de financiación Territorial es diferente a la sumatoria del valor solicitado de 0.00 de las actividades ActisTerritorial"


                });
                auxTematicas.Add(new TematicaDto()
                {
                    Tematica = " Focalización x Actividades Actis",
                    MensajesError = auxMensajesError.ToList()
                });
                auxTematicas.Add(new TematicaDto()
                {
                    Tematica = " Focalización x Actividades Actis2",
                    MensajesError = auxMensajesError.ToList()
                });
                return new ValidarViabilidadCompletarInfoDto()
                {
                    Mensaje = "Mensaje Prueba",

                    Tematicas = auxTematicas
                };
            }

            return null;
        }
        public ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview()
        {
            return new ValidarViabilidadCompletarInfoDto();
        }
        public void Guardar(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            
        }
        public ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, ValidarViabilidadCompletarInfoDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idInstanciaFlujo"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idInstanciaFlujo"));

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idAccion"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idAccion"));

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }
    }
}
