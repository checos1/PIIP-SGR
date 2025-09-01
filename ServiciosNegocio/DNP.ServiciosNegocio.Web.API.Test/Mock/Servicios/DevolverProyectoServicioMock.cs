namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Comunes;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Dto;
    using Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;

    public class DevolverProyectoServicioMock: IDevolverProyectoServicio
    {

        public ParametrosGuardarDto<DevolverProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, DevolverProyectoDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<DevolverProyectoDto>();

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

        public void Guardar(ParametrosGuardarDto<DevolverProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {

        }

        public DevolverProyectoDto ObtenerDevolverProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin.Equals("2017011000042"))
            {
                return new DevolverProyectoDto()
                {
                    Bpin = "2017011000042",
                    Observacion = "",
                    DevolverId = true,
                    EstadoDevolver = 7 
                };
            }
            return null;
        }

    }
}
