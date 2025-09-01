using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Preguntas;
using DNP.Backbone.Dominio.Dto.SGR;
using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
using DNP.Backbone.Servicios.Interfaces.Preguntas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class PreguntasServiciosMock : IPreguntasPersonalizadasServicios
    {
        private readonly IPreguntasPersonalizadasServicios _preguntasServicios;

        public Task<RespuestaGeneralDto> DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadas(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadasCustomSGR(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ConceptosPreviosEmitidosDto> ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracionEntidadDto> ObtenerConfiguracionEntidades(int? ProyectoId, Guid NivelId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DatosGeneralesProyectosDto> ObtenerDatosGeneralesProyecto(int? ProyectoId, Guid NivelId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp)
        {
            return Task.FromResult(new ServicioPreguntasPersonalizadasDto());
        }

        public Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp)
        {
            return Task.FromResult(new ServicioPreguntasPersonalizadasDto());
        }
    }
}
