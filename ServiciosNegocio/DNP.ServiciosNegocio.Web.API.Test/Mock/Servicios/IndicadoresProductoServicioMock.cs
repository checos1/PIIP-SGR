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
    using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
    using DNP.ServiciosNegocio.Servicios.Interfaces.CadenaValor;
    using Dominio.Dto.FuenteFinanciacion;
    using Newtonsoft.Json;

    public class IndicadoresProductoServicioMock : IIndicadoresProductoServicio
    {
        public IndicadorResponse ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            throw new NotImplementedException();
        }

        public IndicadorResponse EliminarIndicadorProducto(int indicadorId, string usuario)
        {
            throw new NotImplementedException();
        }

        public IndicadorResponse GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public List<IndicadorCapituloModificadoDto> IndicadoresValidarCapituloModificado(string bpin)
        {
            try
            {
                var indicadoresCapituloModificados = new List<IndicadorCapituloModificadoDto>();
                string jsonString = "[{'CodigoIndicador':'120600101','EsPrincipal':false,'TipoIndicador':'S','EsAcumulable':false,'IndicadorAcumula':'No Acumulativo','NombreIndicador':'Cupos penitenciarios y carcelarios entregados en Establecimientos de Reclusión del Orden Nacional (ERON)','Ajuste':'Nuevo','MetaEnFirme':0.00,'MetaEnAjuste':0.00,'Diferencia':0.00,'Mensaje':'Se añade el indicador secundario:  Código: 120600101. Acumulativo: NO. Indicador: Cupos penitenciarios y carcelarios entregados en Establecimientos de Reclusión del Orden Nacional (ERON)','ClaseCSS':'utitulo-grilla'},{'CodigoIndicador':'120600102','EsPrincipal':false,'TipoIndicador':'S','EsAcumulable':false,'IndicadorAcumula':'No Acumulativo','NombreIndicador':'Cupos  carcelarios entregados en Establecimientos del Orden Territorial ','Ajuste':'Nuevo','MetaEnFirme':0.00,'MetaEnAjuste':0.00,'Diferencia':0.00,'Mensaje':'Se añade el indicador secundario:  Código: 120600102. Acumulativo: NO. Indicador: Cupos  carcelarios entregados en Establecimientos del Orden Territorial ','ClaseCSS':'utitulo-grilla'},{'CodigoIndicador':'120600103','EsPrincipal':false,'TipoIndicador':'S','EsAcumulable':false,'IndicadorAcumula':'No Acumulativo','NombreIndicador':'Laboratorio de criminalística  construidos y dotados en los Establecimientos de Reclusión del Orden Nacional y/o territorial ','Ajuste':'Nuevo','MetaEnFirme':0.00,'MetaEnAjuste':0.00,'Diferencia':0.00,'Mensaje':'Se añade el indicador secundario:  Código: 120600103. Acumulativo: NO. Indicador: Laboratorio de criminalística  construidos y dotados en los Establecimientos de Reclusión del Orden Nacional y/o territorial ','ClaseCSS':'utitulo-grilla'},{'CodigoIndicador':'120600104','EsPrincipal':false,'TipoIndicador':'S','EsAcumulable':false,'IndicadorAcumula':'No Acumulativo','NombreIndicador':'Laboratorio de criminalística dotados en los Establecimientos de Reclusión del Orden Nacional y/o territorial ','Ajuste':'Nuevo','MetaEnFirme':0.00,'MetaEnAjuste':0.00,'Diferencia':0.00,'Mensaje':'Se añade el indicador secundario:  Código: 120600104. Acumulativo: NO. Indicador: Laboratorio de criminalística dotados en los Establecimientos de Reclusión del Orden Nacional y/o territorial ','ClaseCSS':'utitulo-grilla'},{'CodigoIndicador':'120600600','EsPrincipal':true,'TipoIndicador':'P','EsAcumulable':true,'IndicadorAcumula':'Acumulativo','NombreIndicador':'Boletines de información penitenciaria y carcelaria elaborados','Ajuste':'Modificado','MetaEnFirme':2.00,'MetaEnAjuste':2.00,'Diferencia':0.00,'Mensaje':'El Indicador secundario: Boletines de información penitenciaria y carcelaria elaborados con código : 120600600 se cambia de No Acumulativo a : Acumulativo','ClaseCSS':'uTexto-grilla'}]";

                indicadoresCapituloModificados = JsonConvert.DeserializeObject<List<IndicadorCapituloModificadoDto>>(jsonString);

                return indicadoresCapituloModificados;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public IndicadorProductoDto ObtenerIndicadoresProducto(string bpin)
        {
            throw new NotImplementedException();
        }

        public Dominio.Dto.CadenaValor.RegionalizacionDto RegionalizacionGeneral(string bpin)
        {
            throw new NotImplementedException();
        }

        public RespuestaGeneralDto GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuario)
        {
            return new RespuestaGeneralDto();
        }

        public RespuestaGeneralDto GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string name)
        {
            return new RespuestaGeneralDto();
        }

        public string ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin)
        {
            return string.Empty;
        }
        public string ObtenerSeccionPoliticaFocalizacionDT(string bpin)
        {
            return string.Empty;
        }
    }
}
