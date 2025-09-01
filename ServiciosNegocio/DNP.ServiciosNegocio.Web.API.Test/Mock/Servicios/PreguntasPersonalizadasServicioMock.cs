using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class PreguntasPersonalizadasServicioMock : IPreguntasPersonalizadasServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public ParametrosConsultaDto ConstruirParametrosConsulta(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> ConstruirParametrosGuardar(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public void DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia)
        {
            throw new NotImplementedException();
        }

        public void Guardar(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            throw new NotImplementedException();
        }

        public void GuardarCustomSGR(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            throw new NotImplementedException();
        }

        public ServicioPreguntasPersonalizadasDto Obtener(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        public ConceptosPreviosEmitidosDto ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto)
        {
            ConceptosPreviosEmitidosDto dto = new ConceptosPreviosEmitidosDto();
            dto.TotalConceptosEmitidos = 1;
            return dto;
        }

        public ConfiguracionEntidadDto ObtenerConfiguracionEntidades(int? pProyectoId, Guid pNivelId)
        {
            throw new NotImplementedException();
        }

        public DatosGeneralesProyectosDto ObtenerDatosGeneralesProyecto(int? pProyectoId, Guid pNivelId)
        {
            throw new NotImplementedException();
        }

        public ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles)
        {
            throw new NotImplementedException();
        }

        public ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            throw new NotImplementedException();
        }

        public ServicioPreguntasPersonalizadasDto ObtenerPreguntasPersonalizadasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            throw new NotImplementedException();
        }
    }
}
