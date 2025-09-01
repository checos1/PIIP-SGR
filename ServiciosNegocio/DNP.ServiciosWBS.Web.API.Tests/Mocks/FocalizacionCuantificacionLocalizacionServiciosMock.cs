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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;

    public class FocalizacionCuantificacionLocalizacionServiciosMock : IFocalizacionCuantificacionLocalizacionServicios
    {

        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017761220016")
            {
                return new FocalizacionCuantificacionLocalizacionDto()
                {
                    Focalizacion = new List<FocalizacionCuantificacionDto>()
                    {
                        new FocalizacionCuantificacionDto()
                    }   
                };
            }

            return null;
        }


        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview()
        {
            return new FocalizacionCuantificacionLocalizacionDto();
        }


        public ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, FocalizacionCuantificacionLocalizacionDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto>();

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

        public void Guardar(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {

        }

    }
}
