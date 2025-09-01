using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.SGP.Tramite
{
    public class TramiteSGPServicioMock: ITramiteSGPServicio
    {
        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo)
        {
            throw new NotImplementedException();
        }

        public string ObtenerTiposValorPorEntidad(int IdEntidad, int IdTipoEntidad)
        {
            throw new NotImplementedException();
        }

        public RespuestaGeneralDto GuardarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public RespuestaGeneralDto eliminarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public string ObtenerDatosAdicionSgp(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public string ObtenerCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }
        public string ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }

    }
}
