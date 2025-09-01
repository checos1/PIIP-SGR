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
    using ServiciosNegocio.Dominio.Dto.Poblacion;

    public class CuantificacionLocalizacionServiciosMock: ICuantificacionLocalizacionServicios
    {

        public PoblacionDto ObtenerCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "2017761220016")
            {
                return new PoblacionDto()
                {
                    Vigencias = new List<PoblacionVigenciasDto>()
                                       {
                                           new PoblacionVigenciasDto()
                                       }


                };
            }

            return null;
        }

        public PoblacionDto ObtenerCuantificacionLocalizacionPreview()
        {
            return new PoblacionDto();
        }


        public ParametrosGuardarDto<PoblacionDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, PoblacionDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<PoblacionDto>();

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

        public void Guardar(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {

        }


    }
}
