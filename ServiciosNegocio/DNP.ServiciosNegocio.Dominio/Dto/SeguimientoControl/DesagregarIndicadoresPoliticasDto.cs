using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl
{
    public class ConsultaIndicadoresPoliticas
    {
        public string BPIN { get; set; }

        public ConsultaIndicadoresPoliticas(string bpin)
        {
            BPIN = bpin;
        }

        public static string ObtenerNombreCortoIndicadores(string nombre)
        {
            var maximoCaracteres = 70;
            var nombreCorto = nombre != null && nombre.Length > maximoCaracteres ? nombre.Substring(0, maximoCaracteres) + "..." : nombre;
            return nombreCorto;
        }
    }

    public class DesagregarIndicadoresPoliticasDto
    {
        public string BPIN { get; set; }
        public List<PoliticasDto> Politicas { get; set; }
        public static DesagregarIndicadoresPoliticasDto CrearListadoObtenerIndicadoresPoliticas(string listadoJson, List<CalendarioPeriodoDto> periodos)
        {
            var model = JsonConvert.DeserializeObject<DesagregarIndicadoresPoliticasDto>(listadoJson);

            /* Construye listado de Secciones obligatorias*/
            for (var a = 0; a < model.Politicas.Count; a++)
            {
                model.Politicas[a].NombrePoliticaCorto = ConsultaIndicadoresPoliticas.ObtenerNombreCortoIndicadores(model.Politicas[a].NombrePolitica);

                for (var i = 0; i < model.Politicas[a].Fuentes.Count; i++)
                {
                    model.Politicas[a].Fuentes[i].NombreFuenteCorto = ConsultaIndicadoresPoliticas.ObtenerNombreCortoIndicadores(model.Politicas[a].Fuentes[i].NombreFuente);

                    for (var j = 0; j < model.Politicas[a].Fuentes[i].Categorias.Count; j++)
                    {
                        model.Politicas[a].Fuentes[i].Categorias[j].NombreCategoriaCorto = 
                            ConsultaIndicadoresPoliticas.ObtenerNombreCortoIndicadores(model.Politicas[a].Fuentes[i].Categorias[j].NombreCategoria);

                        model.Politicas[a].Fuentes[i].Categorias[j].NombreSubCategoriaCorto =
                                ConsultaIndicadoresPoliticas.ObtenerNombreCortoIndicadores(model.Politicas[a].Fuentes[i].Categorias[j].NombreSubCategoria);
                        var reporteCategorias = new List<ReporteIndicadoresDto>();
                        foreach (var item in periodos)
                        {
                            var valorCompromisoData = model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias == null ? 0 :
                                model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias.Where(x => x.Vigencia == item.Vigencia
                            && x.PeriodoPeriodicidadId == item.PeriodosPeriodicidadId
                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId
                            && x.PoliticaId == model.Politicas[a].PoliticaId
                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId
                            && x.TipoValorId == 6).Sum(x => x.Valor);
                            var valorObligacionData = model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias == null ? 0 : 
                                model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias.Where(x => x.Vigencia == item.Vigencia
                            && x.PeriodoPeriodicidadId == item.PeriodosPeriodicidadId
                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId
                            && x.PoliticaId == model.Politicas[a].PoliticaId
                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId
                            && x.TipoValorId == 7).Sum(x => x.Valor);
                            var valorPagosData = model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias == null ? 0 : 
                                model.Politicas[a].Fuentes[i].Categorias[j].ProgramacionCategorias.Where(x => x.Vigencia == item.Vigencia
                            && x.PeriodoPeriodicidadId == item.PeriodosPeriodicidadId
                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId
                            && x.PoliticaId == model.Politicas[a].PoliticaId
                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId
                            && x.TipoValorId == 8).Sum(x => x.Valor);
                            
                            var reporteCategoriasData = new ReporteIndicadoresDto();
                            reporteCategoriasData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                            reporteCategoriasData.FaseId = item.FaseId;
                            reporteCategoriasData.Vigencia = item.Vigencia;
                            reporteCategoriasData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                            reporteCategoriasData.FechaDesde = item.FechaDesde;
                            reporteCategoriasData.FechaHasta = item.FechaHasta;
                            reporteCategoriasData.Mes = item.Mes;
                            reporteCategoriasData.IndicadorId = 0;
                            reporteCategoriasData.ValorCompromisos = valorCompromisoData ?? 0;
                            reporteCategoriasData.ValorObligaciones = valorObligacionData ?? 0;
                            reporteCategoriasData.ValorPagos = valorPagosData ?? 0;
                            reporteCategorias.Add(reporteCategoriasData);

                            var reporteCategoriasDataSeguimiento = new ReporteIndicadoresDto();
                            reporteCategoriasDataSeguimiento.CalendarioPeriodoId = item.CalendarioPeriodoId;
                            reporteCategoriasDataSeguimiento.FaseId = item.FaseId;
                            reporteCategoriasDataSeguimiento.Vigencia = item.Vigencia;
                            reporteCategoriasDataSeguimiento.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                            reporteCategoriasDataSeguimiento.FechaDesde = item.FechaDesde;
                            reporteCategoriasDataSeguimiento.FechaHasta = item.FechaHasta;
                            reporteCategoriasDataSeguimiento.Mes = item.Mes;
                            reporteCategoriasDataSeguimiento.IndicadorId = 1;
                            reporteCategorias.Add(reporteCategoriasDataSeguimiento);
                        }

                        for (var m = 0; m < model.Politicas[a].Fuentes[i].Categorias[j].Indicadores.Count; m++)
                        {
                            model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].NombreIndicadorCorto =
                                ConsultaIndicadoresPoliticas.ObtenerNombreCortoIndicadores(model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].NombreIndicador);
                            

                            for (var l = 0; l < model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones.Count; l++)
                            {
                                var reporteIndicadores = new List<ReporteIndicadoresDto>();
                                foreach (var item in periodos)
                                {
                                    var reporteIndicadoresData = new ReporteIndicadoresDto();
                                    reporteIndicadoresData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    reporteIndicadoresData.FaseId = item.FaseId;
                                    reporteIndicadoresData.Vigencia = item.Vigencia;
                                    reporteIndicadoresData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    reporteIndicadoresData.FechaDesde = item.FechaDesde;
                                    reporteIndicadoresData.FechaHasta = item.FechaHasta;
                                    reporteIndicadoresData.Mes = item.Mes;
                                    reporteIndicadoresData.IndicadorId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId;
                                    reporteIndicadoresData.PoliticaId = model.Politicas[a].PoliticaId;
                                    reporteIndicadoresData.FuenteId = model.Politicas[a].Fuentes[i].FuenteId;
                                    reporteIndicadoresData.LocalizacionId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].LocalizacionId;
                                    reporteIndicadores.Add(reporteIndicadoresData);
                                }

                                model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreAgrupacion = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreAgrupacion == "NA" ?
                                    "N/A" : model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreAgrupacion;
                                model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreMunicipio = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreMunicipio == "NA" ?
                                    "N/A" : model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreMunicipio;
                                model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreTipoAgrupacion = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreTipoAgrupacion == "NA" ?
                                    "N/A" : model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].NombreTipoAgrupacion;
                                
                                for (var n = 0; n < model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias.Count; n++)
                                {
                                    //double totalVigente = 0;
                                    double totalCompromisos = 0;
                                    double totalObligaciones = 0;
                                    double totalPagos = 0;
                                    //double valorVigente = 0;
                                    double valorCompromisos = 0;
                                    double valorObligaciones = 0;
                                    double valorPagos = 0;
                                    List<DetalleProgramacionVigenciaIndicadoresDto> reporteProgramacionIndicadores = new List<DetalleProgramacionVigenciaIndicadoresDto>();

                                    for (var p = 0; p < model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad.Count; p++)
                                    {
                                        //var valorVigenteEntidad = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].ProgramacionIndicadores.Where(
                                        //            x => x.TipoValorId == 3 && x.PeriodoPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id).FirstOrDefault();
                                        var valorCompromisoEntidad = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].ProgramacionIndicadores.Where(
                                            x => x.TipoValorId == 6 
                                            && x.PeriodoPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id
                                            && x.PoliticaId == model.Politicas[a].PoliticaId 
                                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId 
                                            && x.IndicadorId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId
                                            && x.LocalizacionId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].LocalizacionId
                                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId).FirstOrDefault();
                                        var valorObligacionEntidad = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].ProgramacionIndicadores.Where(
                                            x => x.TipoValorId == 7 
                                            && x.PeriodoPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id
                                            && x.PoliticaId == model.Politicas[a].PoliticaId 
                                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId 
                                            && x.IndicadorId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId
                                            && x.LocalizacionId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].LocalizacionId
                                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId).FirstOrDefault();
                                        var valorPagoEntidad = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].ProgramacionIndicadores.Where(
                                            x => x.TipoValorId == 8 
                                            && x.PeriodoPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id
                                            && x.PoliticaId == model.Politicas[a].PoliticaId 
                                            && x.FuenteId == model.Politicas[a].Fuentes[i].FuenteId 
                                            && x.IndicadorId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId
                                            && x.LocalizacionId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].LocalizacionId
                                            && x.DimensionId == model.Politicas[a].Fuentes[i].Categorias[j].CategoriaId).FirstOrDefault();
                                        DetalleProgramacionVigenciaIndicadoresDto reporteProgramacionIndicadoresData = new DetalleProgramacionVigenciaIndicadoresDto();
                                        reporteProgramacionIndicadoresData.PeriodoPeriodicidadId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id;
                                        reporteProgramacionIndicadoresData.Mes = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Mes;
                                        reporteProgramacionIndicadoresData.Vigencia = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].Vigencia;
                                        reporteProgramacionIndicadoresData.PeriodoProyectoId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodoProyectoId;
                                        //reporteProgramacionIndicadoresData.ValorVigente = valorVigenteEntidad == null ? 0 : valorVigenteEntidad.Valor ?? 0;
                                        reporteProgramacionIndicadoresData.ValorCompromisos = valorCompromisoEntidad == null ? 0 : valorCompromisoEntidad.Valor ?? 0;
                                        reporteProgramacionIndicadoresData.ValorObligaciones = valorObligacionEntidad == null ? 0 : valorObligacionEntidad.Valor ?? 0;
                                        reporteProgramacionIndicadoresData.ValorPagos = valorPagoEntidad == null ? 0 : valorPagoEntidad.Valor ?? 0;

                                        reporteProgramacionIndicadoresData.PoliticaId = model.Politicas[a].PoliticaId;
                                        reporteProgramacionIndicadoresData.FuenteId = model.Politicas[a].Fuentes[i].FuenteId;
                                        reporteProgramacionIndicadoresData.DimensionId = model.Politicas[a].Fuentes[i].Categorias[j].SubCategoriaId;
                                        reporteProgramacionIndicadoresData.IndicadorId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId;
                                        reporteProgramacionIndicadoresData.LocalizacionId = model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].LocalizacionId;
                                        reporteProgramacionIndicadoresData.ObservacionValores = valorCompromisoEntidad == null ? string.Empty : valorCompromisoEntidad.ObservacionValores;

                                        reporteProgramacionIndicadores.Add(reporteProgramacionIndicadoresData);

                                        //totalVigente += valorVigenteEntidad == null ? 0 : valorVigenteEntidad.Valor ?? 0;
                                        totalCompromisos += valorCompromisoEntidad == null ? 0 : valorCompromisoEntidad.Valor ?? 0;
                                        totalObligaciones += valorObligacionEntidad == null ? 0 : valorObligacionEntidad.Valor ?? 0;
                                        totalPagos += valorPagoEntidad == null ? 0 : valorPagoEntidad.Valor ?? 0;

                                        foreach (var item in reporteIndicadores)
                                        {
                                            if (item.PeriodosPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id
                                                    && item.Vigencia == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].Vigencia
                                                    && item.Mes == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Mes
                                                    && item.IndicadorId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].IndicadorId
                                                    && item.PoliticaId == model.Politicas[a].PoliticaId && item.FuenteId == model.Politicas[a].Fuentes[i].FuenteId)
                                            {
                                                //item.ValorVigente = valorVigenteEntidad == null ? 0 : valorVigenteEntidad.Valor ?? 0;
                                                //item.ValorVigenteAnterior = item.ValorVigente;
                                                item.ValorCompromisos = valorCompromisoEntidad == null ? 0 : valorCompromisoEntidad.Valor ?? 0;
                                                item.ValorCompromisosAnterior = item.ValorCompromisos;
                                                item.ValorObligaciones = valorObligacionEntidad == null ? 0 : valorObligacionEntidad.Valor ?? 0;
                                                item.ValorObligacionesAnterior = item.ValorObligaciones;
                                                item.ValorPagos = valorPagoEntidad == null ? 0 : valorPagoEntidad.Valor ?? 0;
                                                item.ValorPagosAnterior = item.ValorPagos;
                                                item.Observaciones = valorCompromisoEntidad == null ? string.Empty : valorCompromisoEntidad.ObservacionValores;
                                                item.ObservacionesAnterior = item.Observaciones;
                                            }
                                        }

                                        foreach (var item in reporteCategorias)
                                        {
                                            if (item.PeriodosPeriodicidadId == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Id
                                                    && item.Vigencia == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].Vigencia
                                                    && item.Mes == model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].PeriodosPeriodicidad[p].Mes
                                                    && item.IndicadorId==1)
                                            {
                                                item.ValorCompromisos = item.ValorCompromisos  + (valorCompromisoEntidad == null ? 0 : valorCompromisoEntidad.Valor ?? 0);
                                                item.ValorObligaciones = item.ValorObligaciones + (valorObligacionEntidad == null ? 0 : valorObligacionEntidad.Valor ?? 0);
                                                item.ValorPagos = item.ValorPagos + (valorPagoEntidad == null ? 0 : valorPagoEntidad.Valor ?? 0);
                                            }
                                        }
                                    }
                                    model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].ProgramacionVigenciaIndicadores = reporteProgramacionIndicadores;
                                    //model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].TotalVigencia = totalVigente;
                                    model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].TotalCompromisos = totalCompromisos;
                                    model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].TotalObligaciones = totalObligaciones;
                                    model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].Vigencias[n].TotalPagos = totalPagos;
                                }
                                model.Politicas[a].Fuentes[i].Categorias[j].Indicadores[m].Localizaciones[l].ReporteIndicadores = reporteIndicadores;
                            }
                        }
                        model.Politicas[a].Fuentes[i].Categorias[j].ReporteCategorias = reporteCategorias;
                    }
                    
                }
            }
                
            return model;
        }

    }

    public class PoliticasDto
    {
        public int PoliticaId { get; set; }
        public string NombrePolitica { get; set; }
        public string NombrePoliticaCorto { get; set; }
        public int LocalizacionId { get; set; }
        public List<FuentesDto> Fuentes { get; set; }
    }

    public class FuentesDto
    {
        public int FuenteId { get; set; }
        public string NombreFuente { get; set; }
        public string NombreFuenteCorto { get; set; }
        public List<CategoriasDto> Categorias { get; set; }
    }

    public class CategoriasDto
    {
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreCategoriaCorto { get; set; }
        public int SubCategoriaId { get; set; }
        public string NombreSubCategoria { get; set; }
        public string NombreSubCategoriaCorto { get; set; }
        public List<ReporteIndicadoresDto> ReporteCategorias { get; set; }
        public List<DetalleProgramacionIndicadoresDto> ProgramacionCategorias { get; set; }
        public List<IndicadoresPoliticasDto> Indicadores { get; set; }
    }
    public class LocalizacionesIndicadoresDto
    {
        public int LocalizacionId { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreMunicipio { get; set; }
        public string NombreAgrupacion { get; set; }
        public string NombreTipoAgrupacion { get; set; }
        public List<VigenciaIndicadoresDto> Vigencias { get; set; }
        public List<ReporteIndicadoresDto> ReporteIndicadores { get; set; }
    }

    public class IndicadoresPoliticasDto
    {
        public int IndicadorId { get; set; }
        public string CodigoIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public string NombreIndicadorCorto { get; set; }
        
        public List<LocalizacionesIndicadoresDto> Localizaciones { get; set; }
        
    }

    public class VigenciaIndicadoresDto
    {
        public int Vigencia { get; set; }
        public int PeriodoProyectoId { get; set; }
        public double TotalVigencia { get; set; }
        public double TotalCompromisos { get; set; }
        public double TotalObligaciones { get; set; }
        public double TotalPagos { get; set; }
        public string UsuarioDNP { get; set; }
        public List<DetalleProgramacionIndicadoresDto> ProgramacionIndicadores { get; set; }
        public List<DetalleProgramacionVigenciaIndicadoresDto> ProgramacionVigenciaIndicadores { get; set; }
        public List<PeriodosPeriodicidadDto> PeriodosPeriodicidad { get; set; }
    }

    public class DetalleProgramacionIndicadoresDto
    {
        public int PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int IndicadorId { get; set; }
        public int? LocalizacionId { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? PeriodoPeriodicidadId { get; set; }
        public string ObservacionPeriodo { get; set; }
        public int? TipoValorId { get; set; }
        public string NombreTipoValor { get; set; }
        public double? Valor { get; set; }
        public string ObservacionValores { get; set; }
        public int Vigencia { get; set; }
    }
    public class DetalleProgramacionVigenciaIndicadoresDto
    {
        public int PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int IndicadorId { get; set; }
        public int? LocalizacionId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? PeriodoPeriodicidadId { get; set; }
        public string ObservacionPeriodo { get; set; }
        public int? TipoValorId { get; set; }
        public string NombreTipoValor { get; set; }
        public double ValorVigente { get; set; }
        public double ValorCompromisos { get; set; }
        public double ValorObligaciones { get; set; }
        public double ValorPagos { get; set; }
        public string ObservacionValores { get; set; }
        public int Vigencia { get; set; }
        public string Mes { get; set; }
    }

    public class ReporteIndicadoresDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? SeguimientoPeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public double ValorVigente { get; set; }
        public double ValorVigenteAnterior { get; set; }
        public double ValorCompromisos { get; set; }
        public double ValorCompromisosAnterior { get; set; }
        public double ValorObligaciones { get; set; }
        public double ValorObligacionesAnterior { get; set; }
        public double ValorPagos { get; set; }
        public double ValorPagosAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public int? IndicadorId { get; set; }
        public int? PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int? LocalizacionId { get; set; }
    }
}
