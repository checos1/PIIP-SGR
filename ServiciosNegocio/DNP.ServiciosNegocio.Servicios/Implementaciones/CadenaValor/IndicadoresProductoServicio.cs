using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using DNP.ServiciosNegocio.Persistencia.Interfaces.CadenaValor;
using DNP.ServiciosNegocio.Servicios.Interfaces.CadenaValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.CadenaValor
{
    public class IndicadoresProductoServicio : IIndicadoresProductoServicio
    {
        private readonly IIndicadoresProductoPersistencia _indicadoresProductoPersistencia;

        public IndicadoresProductoServicio(IIndicadoresProductoPersistencia indicadoresProductoPersistencia)
        {
            _indicadoresProductoPersistencia = indicadoresProductoPersistencia;
        }

        public IndicadorProductoDto ObtenerIndicadoresProducto(string bpin)
        {
            return _indicadoresProductoPersistencia.ObtenerIndicadoresProducto(bpin);
        }

        public IndicadorResponse GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario)
        {
            return _indicadoresProductoPersistencia.GuardarIndicadoresSecundarios(parametrosGuardar, usuario);
        }

        public IndicadorResponse EliminarIndicadorProducto(int indicadorId, string usuario)
        {
            return _indicadoresProductoPersistencia.EliminarIndicadorProducto(indicadorId, usuario);
        }

        public IndicadorResponse ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            return _indicadoresProductoPersistencia.ActualizarMetaAjusteIndicador(Indicador, usuario);
        }

        public List<IndicadorCapituloModificadoDto> IndicadoresValidarCapituloModificado(string bpin)
        {
            return _indicadoresProductoPersistencia.IndicadoresValidarCapituloModificado(bpin);
        }

        public RegionalizacionDto RegionalizacionGeneral(string bpin)
        {
            return _indicadoresProductoPersistencia.RegionalizacionGeneral(bpin);
        }
        public RespuestaGeneralDto GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuario)
        {
            return _indicadoresProductoPersistencia.GuardarRegionalizacionFuentesFinanciacionAjustes(regionalizacionFuenteAjuste, usuario);
        }

        public RespuestaGeneralDto GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            return _indicadoresProductoPersistencia.GuardarFocalizacionCategoriasAjustes(focalizacionCategoriasAjuste, usuario);
        }

        public string ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin)
        {
            return _indicadoresProductoPersistencia.ObtenerDetalleAjustesJustificaionRegionalizacion(bpin);
        }
        public string ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin)
        {
            return _indicadoresProductoPersistencia.ObtenerSeccionOtrasPoliticasFacalizacionPT(bpin);
        }
        public string ObtenerSeccionPoliticaFocalizacionDT(string bpin)
        {
            return _indicadoresProductoPersistencia.ObtenerSeccionPoliticaFocalizacionDT(bpin);
        }
    }
}
