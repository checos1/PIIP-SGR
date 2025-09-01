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
    public class FuenteCofinanciacionServicioMock : IFuenteCofinanciacionServicio
    {
        public ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, FuenteCofinanciacionProyectoDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<FuenteCofinanciacionProyectoDto>();

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

        public void Guardar(ParametrosGuardarDto<FuenteCofinanciacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {

        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyecto(ParametrosConsultaDto parametrosConsulta)
        {
            if (parametrosConsulta.Bpin.Equals("202000000000005"))
            {
                return new FuenteCofinanciacionProyectoDto()
                {
                    ProyectoId = 72210,
                    CodigoBPIN = "202000000000005",
                    CR = 2,
                    Cofinanciacion = new List<FuenteCofinanciacionDto> {
                        new FuenteCofinanciacionDto()
                        {
                           CofinanciadorId = 10,
                           TipoCofinanciadorId = 2,
                           TipoCofinanciador = "Rubro",
                           Cofinanciador = "RF-2021-MA"
                        }
                    },
                };
            }

            return null;
        }

        public FuenteCofinanciacionProyectoDto ObtenerFuenteCofinanciacionProyectoPreview()
        {
            return new FuenteCofinanciacionProyectoDto()
            {
                ProyectoId = 72210,
                CodigoBPIN = "202000000000005",
                CR = 2,
                Cofinanciacion = new List<FuenteCofinanciacionDto> {
                    new FuenteCofinanciacionDto()
                    {
                        CofinanciadorId = 11,
                        TipoCofinanciadorId = 1,
                        TipoCofinanciador = "Proyecto",
                        Cofinanciador = "2017011000129"
                    }
                },
            };
        }
    }
}
