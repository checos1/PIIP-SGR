using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.CadenaValor
{
    public interface IIndicadoresProductoPersistencia
    {
        IndicadorProductoDto ObtenerIndicadoresProducto(string bpin);
        IndicadorResponse GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario);
        IndicadorResponse EliminarIndicadorProducto(int indicadorId, string usuario);
        IndicadorResponse ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuario);
        List<IndicadorCapituloModificadoDto> IndicadoresValidarCapituloModificado(string bpin);
        RegionalizacionDto RegionalizacionGeneral(string bpin);
        RespuestaGeneralDto GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuario);
        RespuestaGeneralDto GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario);
        string ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin);
        string ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin);
        string ObtenerSeccionPoliticaFocalizacionDT(string bpin);
    }
}