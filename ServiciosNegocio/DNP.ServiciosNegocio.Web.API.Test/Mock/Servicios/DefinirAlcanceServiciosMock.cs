namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Proyectos;

    public class DefinirAlcanceServiciosMock : IDefinirAlcanceServicios
    {
        public AlcanceDto ObtenerDefinirAlcance(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin == "202000000000005")
            {
                return new AlcanceDto()
                {
                    DescripcionGeneralProyectoNueva = new List<ProyectoNuevaDto>()
                                        {
                                           new ProyectoNuevaDto()
                                        }
                };
            }

            return null;
        }

        public AlcanceDto ObtenerDefinirAlcancePreview()
        {
            return new AlcanceDto();
        }

        public ParametrosGuardarDto<AlcanceDto> ConstruirParametrosGuardado(
            HttpRequestMessage request, AlcanceDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<AlcanceDto>();

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

        public void Guardar(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria,
                            bool guardarTemporalmente)
        {

        }
    }
}
