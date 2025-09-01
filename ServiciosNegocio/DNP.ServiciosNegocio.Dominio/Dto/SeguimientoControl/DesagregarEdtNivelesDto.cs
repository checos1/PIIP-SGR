using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl
{
    public class ConsultaObjetivosProyecto
    {
        public string BPIN { get; set; }

        public ConsultaObjetivosProyecto(string bpin)
        {
            BPIN = bpin;
        }

        public static string ObtenerNombreCorto(string nombre)
        {
            var maximoCaracteres = 70;
            var nombreCorto = nombre != null && nombre.Length > maximoCaracteres ? nombre.Substring(0, maximoCaracteres) + "..." : nombre;
            return nombreCorto;
        }
    }

    public class CalendarioPeriodoDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
    }

    public class DesagregarEdtNivelesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Objetivo> Objetivos { get; set; }

        public static DesagregarEdtNivelesDto CrearListadoObjProdNiveles(string listadoJson)
        {
            var model = JsonConvert.DeserializeObject<DesagregarEdtNivelesDto>(listadoJson);

            /* Construye listado de Secciones obligatorias*/
            for (var i = 0; i < model.Objetivos.Count; i++)
            {
                model.Objetivos[i].ObjetivoEspecificoCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(model.Objetivos[i].ObjetivoEspecifico);
                model.Objetivos[i].IdCompuesto = "Objetivo-" + model.Objetivos[i].ObjetivoEspecificoId;
                for (var j = 0; j < model.Objetivos[i].Productos.Count; j++)
                {
                    var RequisitosObligatoriosListado = new List<List<RequisitosObligatorios>>();
                    double costoProductoAcumualdo = 0;
                    if (model.Objetivos[i].Productos[j].EntregablesNivel1 != null)
                    {
                        for (var k = 0; k < model.Objetivos[i].Productos[j].EntregablesNivel1.Count; k++)
                        {
                            var contenidoEntregableNv1 = model.Objetivos[i].Productos[j].EntregablesNivel1[k];
                            contenidoEntregableNv1.IndexObjetivo = (i + 1);
                            contenidoEntregableNv1.IndexProducto = (j + 1);
                            contenidoEntregableNv1.IndexNivel1 = (k + 1);

                            var nivelesObligatoriosNv1 = CrearRquisitosObligatorios(contenidoEntregableNv1);
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios = nivelesObligatoriosNv1;
                            
                            var niveles = CrearJerarquia(contenidoEntregableNv1, contenidoEntregableNv1.CatalogoEntregables, ref costoProductoAcumualdo);
                            
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].Consecutivo = contenidoEntregableNv1.IndexObjetivo + "." + contenidoEntregableNv1.IndexProducto + "." + contenidoEntregableNv1.IndexNivel1;
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados = niveles.ListadoNiveles;
                            //model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios.Exists(p => p.Nombre == "Nivel 2");
                            //if(model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 == false)
                                model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados.Exists(p => p.NivelEntregable == "Nivel 2");
                            //model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios.Exists(p => p.Nombre == "Nivel 3");
                            //if (model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 == false)
                                model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados.Exists(p => p.NivelEntregable == "Nivel 3");

                            RequisitosObligatoriosListado.Add(niveles.EntergableNivel1.RequisitosObligatorios);

                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(contenidoEntregableNv1.NombreEntregable);
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].IdCompuesto = "nivel-1-" + contenidoEntregableNv1.ActividadId;
                        }
                    }

                    if (model.Objetivos[i].Productos[j].Actividades != null)
                    {
                        for (var k = 0; k < model.Objetivos[i].Productos[j].Actividades.Count; k++)
                        {
                            var contenidoEntregableNv1 = model.Objetivos[i].Productos[j].Actividades[k];
                            contenidoEntregableNv1.IndexObjetivo = (i + 1);
                            contenidoEntregableNv1.IndexProducto = (j + 1);
                            contenidoEntregableNv1.IndexNivel1 = (k + 1);
                            model.Objetivos[i].Productos[j].Actividades[k].Consecutivo = contenidoEntregableNv1.IndexObjetivo + "." + contenidoEntregableNv1.IndexProducto + "." + contenidoEntregableNv1.IndexNivel1;
                            model.Objetivos[i].Productos[j].Actividades[k].TotalVigencia = 0;
                            for (var p=0;p < model.Objetivos[i].Productos[j].Actividades[k].Vigencias.Count; p++)
                            {
                                decimal totalVigencia = 0;
                                var listDetalle = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                for (var q = 0; q < model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                {
                                    var detalle = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalle.IdPeriodosPeriodicidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalle.Mes = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalle.Vigencia = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia;
                                    detalle.PeriodoProyectoId = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodo = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalle.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                    detalle.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                    detalle.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalle.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                    detalle.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad?.Valor == null ? 0 : seguimientoValoresEntidad.Valor.Value;
                                    detalle.ValorAnterior = detalle.Valor;
                                    totalVigencia += detalle.Valor;
                                    listDetalle.Add(detalle);
                                }
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValores = listDetalle;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigencia = totalVigencia;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaAnterior = totalVigencia;
                                model.Objetivos[i].Productos[j].Actividades[k].TotalVigencia = model.Objetivos[i].Productos[j].Actividades[k].TotalVigencia + totalVigencia;
                            }
                            model.Objetivos[i].Productos[j].Actividades[k].TotalVigenciaAnterior = model.Objetivos[i].Productos[j].Actividades[k].TotalVigencia;
                        }
                    }
                    
                    /* Valida requisitos obligatorios por producto*/
                    var RequisitosObligatorios = ObtenerRequisitosObligatorios(RequisitosObligatoriosListado);
                    var RequisitosObligatoriosProducto = RequisitosObligatorios.OrderBy(p => p.Position).GroupBy(p => p.Nombre).ToList();
                    var RequisitosGenerales = CrearRequisitosGenerales();
                    for (int m = 0; m < RequisitosGenerales.Count; m++)
                    {
                        var requisito = RequisitosGenerales[m].Nombre;
                        model.Objetivos[i].Productos[j].RequisitosObligatorios.Add(new RequisitosObligatorios()
                        {
                            Nombre = requisito,
                            RequisitoCumple = (RequisitosObligatorios.Exists(p => p.Nombre == requisito))
                        });
                    }

                    model.Objetivos[i].Productos[j].NombreProductoCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(model.Objetivos[i].Productos[j].NombreProducto);
                    model.Objetivos[i].Productos[j].IdCompuesto = "Producto-" + model.Objetivos[i].Productos[j].ProductoId;
                }
            };
            return model;
        }

        public static DesagregarEdtNivelesDto CrearListadoObjProdNivelesXReporte(string listadoJson, List<CalendarioPeriodoDto> periodos)
        {
            var model = JsonConvert.DeserializeObject<DesagregarEdtNivelesDto>(listadoJson);

            /* Construye listado de Secciones obligatorias*/
            for (var i = 0; i < model.Objetivos.Count; i++)
            {
                model.Objetivos[i].ObjetivoEspecificoCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(model.Objetivos[i].ObjetivoEspecifico);
                model.Objetivos[i].IdCompuesto = "Objetivo-" + model.Objetivos[i].ObjetivoEspecificoId;
                for (var j = 0; j < model.Objetivos[i].Productos.Count; j++)
                {
                    var RequisitosObligatoriosListado = new List<List<RequisitosObligatorios>>();
                    double costoProductoAcumualdo = 0;
                    if (model.Objetivos[i].Productos[j].EntregablesNivel1 != null)
                    {
                        for (var k = 0; k < model.Objetivos[i].Productos[j].EntregablesNivel1.Count; k++)
                        {
                            var contenidoEntregableNv1 = model.Objetivos[i].Productos[j].EntregablesNivel1[k];
                            contenidoEntregableNv1.IndexObjetivo = (i + 1);
                            contenidoEntregableNv1.IndexProducto = (j + 1);
                            contenidoEntregableNv1.IndexNivel1 = (k + 1);

                            var nivelesObligatoriosNv1 = CrearRquisitosObligatorios(contenidoEntregableNv1);
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios = nivelesObligatoriosNv1;

                            var niveles = CrearJerarquiaReportes(contenidoEntregableNv1, contenidoEntregableNv1.CatalogoEntregables, periodos, ref costoProductoAcumualdo);
                            
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].Consecutivo = contenidoEntregableNv1.IndexObjetivo + "." + contenidoEntregableNv1.IndexProducto + "." + contenidoEntregableNv1.IndexNivel1;
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados = niveles.ListadoNiveles;
                            //model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios.Exists(p => p.Nombre == "Nivel 2");
                            //if(model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 == false)
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel2 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados.Exists(p => p.NivelEntregable == "Nivel 2");
                            //model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].RequisitosObligatorios.Exists(p => p.Nombre == "Nivel 3");
                            //if (model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 == false)
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].Nivel3 = model.Objetivos[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados.Exists(p => p.NivelEntregable == "Nivel 3");


                            RequisitosObligatoriosListado.Add(niveles.EntergableNivel1.RequisitosObligatorios);

                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(contenidoEntregableNv1.NombreEntregable);
                            model.Objetivos[i].Productos[j].EntregablesNivel1[k].IdCompuesto = "nivel-1-" + contenidoEntregableNv1.ActividadId;
                        }
                    }

                    if (model.Objetivos[i].Productos[j].Actividades != null)
                    {
                        for (var k = 0; k < model.Objetivos[i].Productos[j].Actividades.Count; k++)
                        {
                            var contenidoEntregableNv1 = model.Objetivos[i].Productos[j].Actividades[k];
                            contenidoEntregableNv1.IndexObjetivo = (i + 1);
                            contenidoEntregableNv1.IndexProducto = (j + 1);
                            contenidoEntregableNv1.IndexNivel1 = (k + 1);
                            model.Objetivos[i].Productos[j].Actividades[k].Consecutivo = contenidoEntregableNv1.IndexObjetivo + "." + contenidoEntregableNv1.IndexProducto + "." + contenidoEntregableNv1.IndexNivel1;
                            var valorCosto = model.Objetivos[i].Productos[j].Actividades[k].CostoTotal / (model.Objetivos[i].Productos[j].Actividades[k].CantidadTotal == null ? 1 : 
                                model.Objetivos[i].Productos[j].Actividades[k].CantidadTotal.Value == 0 ? 1 : model.Objetivos[i].Productos[j].Actividades[k].CantidadTotal.Value);
                            var avanceCantidades = new List<AvanceCantidadesDto>();
                            foreach (var item in periodos)
                            {
                                var existe = model.Objetivos[i].Productos[j].Actividades[k].Vigencias.Where(x => x.Vigencia == item.Vigencia).FirstOrDefault();
                                if(existe != null)
                                {
                                    var avanceCantidadesData = new AvanceCantidadesDto();
                                    avanceCantidadesData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    avanceCantidadesData.FaseId = item.FaseId;
                                    avanceCantidadesData.Vigencia = item.Vigencia;
                                    avanceCantidadesData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    avanceCantidadesData.FechaDesde = item.FechaDesde;
                                    avanceCantidadesData.FechaHasta = item.FechaHasta;
                                    avanceCantidadesData.Mes = item.Mes;
                                    avanceCantidadesData.ActividadProgramacionSeguimientoId = model.Objetivos[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId;
                                    avanceCantidades.Add(avanceCantidadesData);
                                }
                            }

                            var costoPeriodo = new List<CostoPeriodoDto>();
                            foreach (var item in periodos)
                            {
                                var existe = model.Objetivos[i].Productos[j].Actividades[k].Vigencias.Where(x => x.Vigencia == item.Vigencia).FirstOrDefault();
                                if (existe != null)
                                {
                                    var costoPeriodoData = new CostoPeriodoDto();
                                    costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    costoPeriodoData.FaseId = item.FaseId;
                                    costoPeriodoData.Vigencia = item.Vigencia;
                                    costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    costoPeriodoData.FechaDesde = item.FechaDesde;
                                    costoPeriodoData.FechaHasta = item.FechaHasta;
                                    costoPeriodoData.Mes = item.Mes;
                                    costoPeriodoData.ActividadProgramacionSeguimientoId = model.Objetivos[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId;
                                    costoPeriodoData.TipoCosto = "Presupuestal";
                                    costoPeriodo.Add(costoPeriodoData);
                                }
                            }

                            foreach (var item in periodos)
                            {
                                var existe = model.Objetivos[i].Productos[j].Actividades[k].Vigencias.Where(x => x.Vigencia == item.Vigencia).FirstOrDefault();
                                if (existe != null)
                                {
                                    var costoPeriodoData = new CostoPeriodoDto();
                                    costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    costoPeriodoData.FaseId = item.FaseId;
                                    costoPeriodoData.Vigencia = item.Vigencia;
                                    costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    costoPeriodoData.FechaDesde = item.FechaDesde;
                                    costoPeriodoData.FechaHasta = item.FechaHasta;
                                    costoPeriodoData.Mes = item.Mes;
                                    costoPeriodoData.ActividadProgramacionSeguimientoId = model.Objetivos[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId;
                                    costoPeriodoData.TipoCosto = "Flujo de caja";
                                    costoPeriodo.Add(costoPeriodoData);
                                }
                            }
                            for (var p = 0; p < model.Objetivos[i].Productos[j].Actividades[k].Vigencias.Count; p++)
                            {
                                decimal acumaladoVigenciaCE = 0;
                                double acumaladoVigenciaCP = 0;
                                double acumaladoVigenciaCFC = 0;
                                decimal totalVigenciaCE = 0;
                                double totalVigenciaCP = 0;
                                double totalVigenciaCFC = 0;
                                decimal totalBaseCE = 0;
                                double totalBaseCP = 0;
                                double totalBaseCFC = 0;
                                var listDetalleCE = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                var listDetalleCP = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                var listDetalleCFC = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                for (var q = 0; q < model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                {
                                    var detalleCE = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCE.IdPeriodosPeriodicidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCE.Mes = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCE.Vigencia = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia;
                                    detalleCE.PeriodoProyectoId = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodo = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCE.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                    detalleCE.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                    detalleCE.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCE.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                    
                                    acumaladoVigenciaCE = acumaladoVigenciaCE + (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                    if(q==0)
                                        totalVigenciaCE = 0;
                                    else
                                        totalVigenciaCE = acumaladoVigenciaCE;

                                    detalleCE.ValorCantidadEjecutada = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor;
                                    detalleCE.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.ValorBase;
                                    detalleCE.ValorCantidadEjecutadaAnterior = detalleCE.ValorCantidadEjecutada;
                                    detalleCE.Observaciones = seguimientoValoresEntidad?.Observaciones;
                                    
                                    totalBaseCE += detalleCE.Valor;
                                    listDetalleCE.Add(detalleCE);

                                    var detalleCP = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCP.IdPeriodosPeriodicidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCP.Mes = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCP.Vigencia = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia;
                                    detalleCP.PeriodoProyectoId = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidadCP = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodoCP = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCP.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCP == null ? seguimientoValoresEntidadSinPeriodoCP?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoId;
                                    detalleCP.PeriodosPeriodicidadId = seguimientoValoresEntidadCP?.PeriodosPeriodicidadId;
                                    detalleCP.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCP.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                    
                                    acumaladoVigenciaCP = acumaladoVigenciaCP + (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                    if (q == 0)
                                        totalVigenciaCP = 0;
                                    else
                                        totalVigenciaCP = acumaladoVigenciaCP;

                                    detalleCP.ValorCostoPresupuestal = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor;
                                    detalleCP.ValorCostoPresupuestalAnterior = detalleCP.ValorCostoPresupuestal;
                                    detalleCP.ValorCantidad = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.ValorBase  * (valorCosto == null ? 0 : valorCosto.Value);
                                    detalleCP.Observaciones = seguimientoValoresEntidadCP?.Observaciones;
                                    
                                    totalBaseCP += detalleCP.ValorCantidad;
                                    listDetalleCP.Add(detalleCP);

                                    var detalleCFC = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCFC.IdPeriodosPeriodicidad = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCFC.Mes = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCFC.Vigencia = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia;
                                    detalleCFC.PeriodoProyectoId = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidadCFC = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodoCFC = model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                        .Where(x => x.PeriodoProyectoId == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCFC.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCFC == null ? seguimientoValoresEntidadSinPeriodoCFC?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoId;
                                    detalleCFC.PeriodosPeriodicidadId = seguimientoValoresEntidadCFC?.PeriodosPeriodicidadId;
                                    detalleCFC.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCFC.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                    
                                    acumaladoVigenciaCFC = acumaladoVigenciaCFC + (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                    if (q == 0)
                                        totalVigenciaCFC = 0;
                                    else
                                        totalVigenciaCFC = acumaladoVigenciaCFC;

                                    detalleCFC.ValorCostoFlujoCaja = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor;
                                    detalleCFC.ValorCostoFlujoCajaAnterior = detalleCFC.ValorCostoFlujoCaja;
                                    detalleCFC.ValorCantidad = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                    detalleCFC.Observaciones = seguimientoValoresEntidadCFC?.Observaciones;
                                    
                                    totalBaseCFC += detalleCFC.ValorCantidad;
                                    listDetalleCFC.Add(detalleCFC);
                                    foreach(var item in avanceCantidades)
                                    {
                                        if(item.PeriodosPeriodicidadId == detalleCE.IdPeriodosPeriodicidad &&
                                                item.Vigencia == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia
                                                && item.Mes == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCE.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CantidadProgramadaVigencia = detalleCE.Valor;
                                            item.CantidadAcumuladaMeses = totalVigenciaCE == 0 ? 0 : totalVigenciaCE - (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                            item.CantidadEjecutadaMes = detalleCE.ValorCantidadEjecutada;
                                            item.CantidadEjecutadaMesAnterior = item.CantidadEjecutadaMes;
                                            item.Observaciones = detalleCE.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }

                                    foreach (var item in costoPeriodo)
                                    {
                                        if (item.PeriodosPeriodicidadId == detalleCP.IdPeriodosPeriodicidad && item.TipoCosto == "Presupuestal" &&
                                                item.Vigencia == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia
                                                && item.Mes == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCP.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CostoProgramadoVigencia = detalleCP.ValorCantidad;
                                            item.CostoAcumuladoMeses = totalVigenciaCP == 0 ? 0 : totalVigenciaCP - (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                            item.CostoEjecutadoMes = detalleCP.ValorCostoPresupuestal;
                                            item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                            item.Observaciones = detalleCP.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }

                                    foreach (var item in costoPeriodo)
                                    {
                                        if (item.PeriodosPeriodicidadId == detalleCFC.IdPeriodosPeriodicidad && item.TipoCosto == "Flujo de caja" &&
                                                item.Vigencia == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].Vigencia
                                                && item.Mes == model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCFC.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CostoProgramadoVigencia = detalleCFC.ValorCantidad;
                                            item.CostoAcumuladoMeses = totalVigenciaCFC == 0 ? 0 : totalVigenciaCFC - (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                            item.CostoEjecutadoMes = detalleCFC.ValorCostoFlujoCaja;
                                            item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                            item.Observaciones = detalleCFC.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }
                                }
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCE = listDetalleCE;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCP = listDetalleCP;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCFC = listDetalleCFC;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCE = totalVigenciaCE;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCEAnterior = totalVigenciaCE;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCP = totalVigenciaCP;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCPAnterior = totalVigenciaCP;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCFC = totalVigenciaCFC;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalVigenciaCFCAnterior = totalVigenciaCFC;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalBaseCE = totalBaseCE;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalBaseCP = totalBaseCP;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                                model.Objetivos[i].Productos[j].Actividades[k].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                            }
                            model.Objetivos[i].Productos[j].Actividades[k].AvanceCantidades = avanceCantidades;
                            model.Objetivos[i].Productos[j].Actividades[k].CostoPeriodo = costoPeriodo;
                        }
                    }

                    /* Valida requisitos obligatorios por producto*/
                    var RequisitosObligatorios = ObtenerRequisitosObligatorios(RequisitosObligatoriosListado);
                    var RequisitosObligatoriosProducto = RequisitosObligatorios.OrderBy(p => p.Position).GroupBy(p => p.Nombre).ToList();
                    var RequisitosGenerales = CrearRequisitosGenerales();
                    for (int m = 0; m < RequisitosGenerales.Count; m++)
                    {
                        var requisito = RequisitosGenerales[m].Nombre;
                        model.Objetivos[i].Productos[j].RequisitosObligatorios.Add(new RequisitosObligatorios()
                        {
                            Nombre = requisito,
                            RequisitoCumple = (RequisitosObligatorios.Exists(p => p.Nombre == requisito))
                        });
                    }

                    model.Objetivos[i].Productos[j].NombreProductoCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(model.Objetivos[i].Productos[j].NombreProducto);
                    model.Objetivos[i].Productos[j].IdCompuesto = "Producto-" + model.Objetivos[i].Productos[j].ProductoId;
                }
            };
            return model;
        }

        private static JerarquiaNiveles CrearJerarquia(EntregablesNivel1 entregableNv1, List<CatalogoEntregable> CatalogoEntregables, ref double acumulado)
        {
            var conteo = new List<ListadoCantidadObligatorios>();
            var entregablesRegistrados = new List<SeguimientoEntregable>();
            if (entregableNv1.SeguimientoEntregables != null)
            {
                // Crea Nivel 2 o Actividades
                var niveles2 = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == null).ToList();
                var tipoEntregableNvl2 = niveles2.GroupBy(r => r.NivelEntregable).Select(r => r.Key).FirstOrDefault();
                var indxRequisitoNvl2 = entregableNv1.RequisitosObligatorios.FindIndex(p => p.TipoEntregable == tipoEntregableNvl2);
                double valornivel2 = 0;
                DateTime? fechainicialNivel1 = null;
                DateTime? fechaFinalNivel1 = null;
                for (int i = 0; i < niveles2.Count; i++)
                {
                    if(fechainicialNivel1 == null)
                        fechainicialNivel1 = niveles2[i].FechaInicio;
                    if(fechainicialNivel1> niveles2[i].FechaInicio)
                        fechainicialNivel1 = niveles2[i].FechaInicio;
                    if (fechaFinalNivel1 == null)
                        fechaFinalNivel1 = niveles2[i].FechaFin;
                    if (fechaFinalNivel1 < niveles2[i].FechaFin)
                        fechaFinalNivel1 = niveles2[i].FechaFin;
                    DateTime? fechainicialNivel2 = null;
                    DateTime? fechaFinalNivel2 = null;
                    if (fechainicialNivel2 == null)
                        fechainicialNivel2 = niveles2[i].FechaInicio;
                    if (fechainicialNivel2 > niveles2[i].FechaInicio)
                        fechainicialNivel2 = niveles2[i].FechaInicio;
                    if (fechaFinalNivel2 == null)
                        fechaFinalNivel2 = niveles2[i].FechaFin;
                    if (fechaFinalNivel2 < niveles2[i].FechaFin)
                        fechaFinalNivel2 = niveles2[i].FechaFin;
                    var consecutivoNivel2 = entregableNv1.IndexObjetivo + "." + entregableNv1.IndexProducto + "." + entregableNv1.IndexNivel1 + "." + (i+1);
                    niveles2[i].Consecutivo = consecutivoNivel2;
                    niveles2[i].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(niveles2[i].NombreEntregable);
                    niveles2[i].IdCompuesto = "nivel-2-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId;
                    niveles2[i].Descendencia.Add(new DescendenciaModel() {  ActividadSeguimientoId = Convert.ToInt32(niveles2[i].SeguimientoEntregableId) });
                    /*if (CatalogoEntregables == null)
                        niveles2[i].ContieneSiguienteNivel = false;
                    else
                        niveles2[i].ContieneSiguienteNivel = CatalogoEntregables.Exists(p => p.Nivel == "Nivel 3" && p.DeliverableCatalogPadreId == niveles2[i].EntregableCatalogId);*/
                    niveles2[i].TotalVigencia = 0;
                    if (niveles2[i].Vigencias != null)
                    {
                        for (var p = 0; p < niveles2[i].Vigencias.Count; p++)
                        {
                            decimal totalVigencia = 0;
                            var listDetalle = new List<ProgramacionSeguimientoPeriodosValoresDto>();

                            if(niveles2[i].Vigencias[p].PeriodosPeriodicidad != null)
                            {
                                for (var q = 0; q < niveles2[i].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                {
                                    var detalle = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalle.IdPeriodosPeriodicidad = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalle.Mes = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalle.Vigencia = niveles2[i].Vigencias[p].Vigencia;
                                    detalle.PeriodoProyectoId = niveles2[i].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidad = niveles2[i].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                        .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodo = niveles2[i].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                        .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalle.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                    detalle.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                    detalle.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalle.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                    detalle.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad?.Valor == null ? 0 : seguimientoValoresEntidad.Valor.Value;
                                    detalle.ValorAnterior = detalle.Valor;
                                    totalVigencia += detalle.Valor;
                                    listDetalle.Add(detalle);
                                }
                            }
                            
                            niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValores = listDetalle;
                            niveles2[i].Vigencias[p].TotalVigencia = totalVigencia;
                            niveles2[i].Vigencias[p].TotalVigenciaAnterior = totalVigencia;
                            niveles2[i].TotalVigencia = niveles2[i].TotalVigencia + totalVigencia;
                        }
                    }
                    niveles2[i].TotalVigenciaAnterior = niveles2[i].TotalVigencia;
                    double valornivel3 = 0;
                    // Crea Nivel 3 o Actividades
                    var niveles3 = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == niveles2[i].SeguimientoEntregableId).ToList();
                    for (int j = 0; j < niveles3.Count; j++)
                    {
                        if (fechainicialNivel1 > niveles3[j].FechaInicio)
                            fechainicialNivel1 = niveles3[j].FechaInicio;
                        if (fechaFinalNivel1 < niveles3[j].FechaFin)
                            fechaFinalNivel1 = niveles3[j].FechaFin;
                        if (fechainicialNivel2 > niveles3[j].FechaInicio)
                            fechainicialNivel2 = niveles3[j].FechaInicio;
                        if (fechaFinalNivel2 < niveles3[j].FechaFin)
                            fechaFinalNivel2 = niveles3[j].FechaFin;
                        DateTime? fechainicialNivel3 = null;
                        DateTime? fechaFinalNivel3 = null;
                        if (fechainicialNivel3 == null)
                            fechainicialNivel3 = niveles2[i].FechaInicio;
                        if (fechainicialNivel3 > niveles2[i].FechaInicio)
                            fechainicialNivel3 = niveles2[i].FechaInicio;
                        if (fechaFinalNivel3 == null)
                            fechaFinalNivel3 = niveles2[i].FechaFin;
                        if (fechaFinalNivel3 < niveles2[i].FechaFin)
                            fechaFinalNivel3 = niveles2[i].FechaFin;
                        var consecutivoNivel3 = consecutivoNivel2 + "." + (j + 1);
                        niveles2[i].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(niveles3[j].SeguimientoEntregableId) });
                        niveles3[j].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(niveles3[j].SeguimientoEntregableId) });
                        niveles3[j].Consecutivo = consecutivoNivel3;

                        niveles3[j].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(niveles3[j].NombreEntregable);
                        niveles3[j].IdCompuesto = "nivel-3-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId + "" + niveles3[j].SeguimientoEntregableId;
                        niveles3[j].TotalVigencia = 0;
                        if (niveles3[j].Vigencias != null)
                        {
                            for (var p = 0; p < niveles3[j].Vigencias.Count; p++)
                            {
                                decimal totalVigencia = 0;
                                var listDetalle = new List<ProgramacionSeguimientoPeriodosValoresDto>();

                                if (niveles3[j].Vigencias[p].PeriodosPeriodicidad != null)
                                {
                                    for (var q = 0; q < niveles3[j].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                    {
                                        var detalle = new ProgramacionSeguimientoPeriodosValoresDto();
                                        detalle.IdPeriodosPeriodicidad = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                        detalle.Mes = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                        detalle.Vigencia = niveles3[j].Vigencias[p].Vigencia;
                                        detalle.PeriodoProyectoId = niveles3[j].Vigencias[p].PeriodoProyectoId;
                                        var seguimientoValoresEntidad = niveles3[j].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                            .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId &&
                                            x.PeriodosPeriodicidadId == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                        var seguimientoValoresEntidadSinPeriodo = niveles3[j].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                            .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                        detalle.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                        detalle.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                        detalle.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                        detalle.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                        detalle.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad?.Valor == null ? 0 : seguimientoValoresEntidad.Valor.Value;
                                        detalle.ValorAnterior = detalle.Valor;
                                        totalVigencia += detalle.Valor;
                                        listDetalle.Add(detalle);
                                    }
                                }

                                niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValores = listDetalle;
                                niveles3[j].Vigencias[p].TotalVigencia = totalVigencia;
                                niveles3[j].Vigencias[p].TotalVigenciaAnterior = totalVigencia;
                                niveles3[j].TotalVigencia = niveles3[j].TotalVigencia + totalVigencia;
                            }
                        }
                        niveles3[j].TotalVigenciaAnterior = niveles3[j].TotalVigencia;
                        // Actividades
                        var actividades = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == niveles3[j].SeguimientoEntregableId).ToList();
                        double valorActividades = 0;
                        for (int iac = 0; iac < actividades.Count; iac++)
                        {
                            if (fechainicialNivel1 > actividades[iac].FechaInicio)
                                fechainicialNivel1 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel1 < actividades[iac].FechaFin)
                                fechaFinalNivel1 = actividades[iac].FechaFin;
                            if (fechainicialNivel2 > actividades[iac].FechaInicio)
                                fechainicialNivel2 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel2 < actividades[iac].FechaFin)
                                fechaFinalNivel2 = actividades[iac].FechaFin;
                            if (fechainicialNivel3 > actividades[iac].FechaInicio)
                                fechainicialNivel3 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel3 < actividades[iac].FechaFin)
                                fechaFinalNivel3 = actividades[iac].FechaFin;
                            var consecutivoNivel4 = consecutivoNivel3 + "." + (iac + 1);
                            niveles2[i].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            niveles3[j].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            actividades[iac].Consecutivo = consecutivoNivel4;
                            actividades[iac].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            actividades[iac].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(actividades[iac].NombreEntregable);
                            actividades[iac].IdCompuesto = "Actividades-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId + "" + niveles3[j].SeguimientoEntregableId + "" + actividades[iac].SeguimientoEntregableId;

                            actividades[iac].TotalVigencia = 0;
                            valorActividades = valorActividades + (actividades[iac].CostoTotal == null ? 0 : actividades[iac].CostoTotal.Value);
                            if (actividades[iac].Vigencias != null)
                            {
                                for (var p = 0; p < actividades[iac].Vigencias.Count; p++)
                                {
                                    decimal totalVigencia = 0;
                                    var listDetalle = new List<ProgramacionSeguimientoPeriodosValoresDto>();

                                    if (actividades[iac].Vigencias[p].PeriodosPeriodicidad != null)
                                    {
                                        for (var q = 0; q < actividades[iac].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                        {
                                            var detalle = new ProgramacionSeguimientoPeriodosValoresDto();
                                            detalle.IdPeriodosPeriodicidad = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                            detalle.Mes = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                            detalle.Vigencia = actividades[iac].Vigencias[p].Vigencia;
                                            detalle.PeriodoProyectoId = actividades[iac].Vigencias[p].PeriodoProyectoId;
                                            var seguimientoValoresEntidad = actividades[iac].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                                .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId &&
                                                x.PeriodosPeriodicidadId == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                            var seguimientoValoresEntidadSinPeriodo = actividades[iac].Vigencias[p].ActividadProgramacionSeguimientoPeriodosValores
                                                .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                            detalle.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                            detalle.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                            detalle.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                            detalle.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;
                                            detalle.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad?.Valor == null ? 0 : seguimientoValoresEntidad.Valor.Value;
                                            detalle.ValorAnterior = detalle.Valor;
                                            totalVigencia += detalle.Valor;
                                            listDetalle.Add(detalle);
                                        }
                                    }

                                    actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValores = listDetalle;
                                    actividades[iac].Vigencias[p].TotalVigencia = totalVigencia;
                                    actividades[iac].Vigencias[p].TotalVigenciaAnterior = totalVigencia;
                                    actividades[iac].TotalVigencia = actividades[iac].TotalVigencia + totalVigencia;
                                }
                            }
                            actividades[iac].TotalVigenciaAnterior = actividades[iac].TotalVigencia;


                        }
                        niveles3[j].CostoTotal = valorActividades == 0 ? niveles3[j].CostoTotal : valorActividades;
                        valornivel3 = valornivel3 + (niveles3[j].CostoTotal == null ? 0 : niveles3[j].CostoTotal.Value);
                        niveles3[j].Hijos = actividades;
                        niveles3[j].FechaInicio = fechainicialNivel3;
                        niveles3[j].FechaFin = fechaFinalNivel3;
                    }
                    niveles2[i].CostoTotal = valornivel3;
                    valornivel2 = valornivel2 + (niveles2[i].CostoTotal == null ? 0 : niveles2[i].CostoTotal.Value);
                    niveles2[i].Hijos = niveles3;
                    niveles2[i].ContieneSiguienteNivel = niveles2[i].Hijos.Exists(p => p.NivelEntregable == "Nivel 3");
                    niveles2[i].FechaInicio = fechainicialNivel2;
                    niveles2[i].FechaFin = fechaFinalNivel2;
                }
                entregablesRegistrados = niveles2;
                acumulado = valornivel2;
                entregableNv1.FechaInicio = fechainicialNivel1;
                entregableNv1.FechaFin = fechaFinalNivel1;
            }


            return new JerarquiaNiveles() { ListadoNiveles = entregablesRegistrados, EntergableNivel1 = entregableNv1 };
        }

        private static JerarquiaNiveles CrearJerarquiaReportes(EntregablesNivel1 entregableNv1, List<CatalogoEntregable> CatalogoEntregables, List<CalendarioPeriodoDto> periodos, ref double acumulado)
        {
            var conteo = new List<ListadoCantidadObligatorios>();
            var entregablesRegistrados = new List<SeguimientoEntregable>();
            if (entregableNv1.SeguimientoEntregables != null)
            {
                // Crea Nivel 2 o Actividades
                var niveles2 = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == null).ToList();
                var tipoEntregableNvl2 = niveles2.GroupBy(r => r.NivelEntregable).Select(r => r.Key).FirstOrDefault();
                var indxRequisitoNvl2 = entregableNv1.RequisitosObligatorios.FindIndex(p => p.TipoEntregable == tipoEntregableNvl2);
                double valornivel2 = 0;
                DateTime? fechainicialNivel1 = null;
                DateTime? fechaFinalNivel1 = null;
                for (int i = 0; i < niveles2.Count; i++)
                {
                    if (fechainicialNivel1 == null)
                        fechainicialNivel1 = niveles2[i].FechaInicio;
                    if (fechainicialNivel1 > niveles2[i].FechaInicio)
                        fechainicialNivel1 = niveles2[i].FechaInicio;
                    if (fechaFinalNivel1 == null)
                        fechaFinalNivel1 = niveles2[i].FechaFin;
                    if (fechaFinalNivel1 < niveles2[i].FechaFin)
                        fechaFinalNivel1 = niveles2[i].FechaFin;
                    DateTime? fechainicialNivel2 = null;
                    DateTime? fechaFinalNivel2 = null;
                    if (fechainicialNivel2 == null)
                        fechainicialNivel2 = niveles2[i].FechaInicio;
                    if (fechainicialNivel2 > niveles2[i].FechaInicio)
                        fechainicialNivel2 = niveles2[i].FechaInicio;
                    if (fechaFinalNivel2 == null)
                        fechaFinalNivel2 = niveles2[i].FechaFin;
                    if (fechaFinalNivel2 < niveles2[i].FechaFin)
                        fechaFinalNivel2 = niveles2[i].FechaFin;
                    var consecutivoNivel2 = entregableNv1.IndexObjetivo + "." + entregableNv1.IndexProducto + "." + entregableNv1.IndexNivel1 + "." + (i + 1);
                    niveles2[i].Consecutivo = consecutivoNivel2;
                    niveles2[i].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(niveles2[i].NombreEntregable);
                    niveles2[i].IdCompuesto = "nivel-2-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId;
                    niveles2[i].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(niveles2[i].SeguimientoEntregableId) });
                    /*if (CatalogoEntregables == null)
                        niveles2[i].ContieneSiguienteNivel = false;
                    else
                        niveles2[i].ContieneSiguienteNivel = CatalogoEntregables.Exists(p => p.Nivel == "Nivel 3" && p.DeliverableCatalogPadreId == niveles2[i].EntregableCatalogId);*/
                    
                    if (niveles2[i].Vigencias != null)
                    {
                        var valorCosto = niveles2[i].CostoTotal / (niveles2[i].CantidadTotal == null ? 1 : niveles2[i].CantidadTotal.Value == 0 ? 1 : niveles2[i].CantidadTotal.Value);
                        var avanceCantidades = new List<AvanceCantidadesDto>();
                        foreach (var item in periodos)
                        {
                            var avanceCantidadesData = new AvanceCantidadesDto();
                            avanceCantidadesData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                            avanceCantidadesData.FaseId = item.FaseId;
                            avanceCantidadesData.Vigencia = item.Vigencia;
                            avanceCantidadesData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                            avanceCantidadesData.FechaDesde = item.FechaDesde;
                            avanceCantidadesData.FechaHasta = item.FechaHasta;
                            avanceCantidadesData.Mes = item.Mes;
                            avanceCantidadesData.ActividadProgramacionSeguimientoId = niveles2[i].ActividadProgramacionSeguimientoId;
                            avanceCantidades.Add(avanceCantidadesData);
                        }

                        var costoPeriodo = new List<CostoPeriodoDto>();
                        foreach (var item in periodos)
                        {
                            var costoPeriodoData = new CostoPeriodoDto();
                            costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                            costoPeriodoData.FaseId = item.FaseId;
                            costoPeriodoData.Vigencia = item.Vigencia;
                            costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                            costoPeriodoData.FechaDesde = item.FechaDesde;
                            costoPeriodoData.FechaHasta = item.FechaHasta;
                            costoPeriodoData.Mes = item.Mes;
                            costoPeriodoData.ActividadProgramacionSeguimientoId = niveles2[i].ActividadProgramacionSeguimientoId;
                            costoPeriodoData.TipoCosto = "Presupuestal";
                            costoPeriodo.Add(costoPeriodoData);
                        }

                        foreach (var item in periodos)
                        {
                            var costoPeriodoData = new CostoPeriodoDto();
                            costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                            costoPeriodoData.FaseId = item.FaseId;
                            costoPeriodoData.Vigencia = item.Vigencia;
                            costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                            costoPeriodoData.FechaDesde = item.FechaDesde;
                            costoPeriodoData.FechaHasta = item.FechaHasta;
                            costoPeriodoData.Mes = item.Mes;
                            costoPeriodoData.ActividadProgramacionSeguimientoId = niveles2[i].ActividadProgramacionSeguimientoId;
                            costoPeriodoData.TipoCosto = "Flujo de caja";
                            costoPeriodo.Add(costoPeriodoData);
                        }
                        for (var p = 0; p < niveles2[i].Vigencias.Count; p++)
                        {
                            decimal totalVigenciaCE = 0;
                            double totalVigenciaCP = 0;
                            double totalVigenciaCFC = 0;
                            decimal totalBaseCE = 0;
                            double totalBaseCP = 0;
                            double totalBaseCFC = 0;
                            decimal acumaladoVigenciaCE = 0;
                            double acumaladoVigenciaCP = 0;
                            double acumaladoVigenciaCFC = 0;
                            var listDetalleCE = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                            var listDetalleCP = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                            var listDetalleCFC = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                            for (var q = 0; q < niveles2[i].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                            {
                                var detalleCE = new ProgramacionSeguimientoPeriodosValoresDto();
                                detalleCE.IdPeriodosPeriodicidad = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                detalleCE.Mes = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                detalleCE.Vigencia = niveles2[i].Vigencias[p].Vigencia;
                                detalleCE.PeriodoProyectoId = niveles2[i].Vigencias[p].PeriodoProyectoId;
                                var seguimientoValoresEntidad = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId &&
                                    x.PeriodosPeriodicidadId == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                var seguimientoValoresEntidadSinPeriodo = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                detalleCE.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                detalleCE.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                detalleCE.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                detalleCE.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                acumaladoVigenciaCE = acumaladoVigenciaCE + (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                if (q == 0)
                                    totalVigenciaCE = 0;
                                else
                                    totalVigenciaCE = acumaladoVigenciaCE;

                                detalleCE.ValorCantidadEjecutada = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor;
                                detalleCE.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.ValorBase;
                                detalleCE.ValorCantidadEjecutadaAnterior = detalleCE.ValorCantidadEjecutada;
                                detalleCE.Observaciones = seguimientoValoresEntidad?.Observaciones;
                                totalBaseCE += detalleCE.Valor;
                                listDetalleCE.Add(detalleCE);

                                var detalleCP = new ProgramacionSeguimientoPeriodosValoresDto();
                                detalleCP.IdPeriodosPeriodicidad = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                detalleCP.Mes = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                detalleCP.Vigencia = niveles2[i].Vigencias[p].Vigencia;
                                detalleCP.PeriodoProyectoId = niveles2[i].Vigencias[p].PeriodoProyectoId;
                                var seguimientoValoresEntidadCP = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId &&
                                    x.PeriodosPeriodicidadId == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                var seguimientoValoresEntidadSinPeriodoCP = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                detalleCP.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCP == null ? seguimientoValoresEntidadSinPeriodoCP?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoId;
                                detalleCP.PeriodosPeriodicidadId = seguimientoValoresEntidadCP?.PeriodosPeriodicidadId;
                                detalleCP.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosId;
                                detalleCP.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                acumaladoVigenciaCP = acumaladoVigenciaCP + (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                if (q == 0)
                                    totalVigenciaCP = 0;
                                else
                                    totalVigenciaCP = acumaladoVigenciaCP;

                                detalleCP.ValorCostoPresupuestal = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor;
                                detalleCP.ValorCostoPresupuestalAnterior = detalleCP.ValorCostoPresupuestal;
                                detalleCP.ValorCantidad = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                detalleCP.Observaciones = seguimientoValoresEntidadCP?.Observaciones;
                                totalBaseCP += detalleCP.ValorCantidad;
                                listDetalleCP.Add(detalleCP);

                                var detalleCFC = new ProgramacionSeguimientoPeriodosValoresDto();
                                detalleCFC.IdPeriodosPeriodicidad = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                detalleCFC.Mes = niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                detalleCFC.Vigencia = niveles2[i].Vigencias[p].Vigencia;
                                detalleCFC.PeriodoProyectoId = niveles2[i].Vigencias[p].PeriodoProyectoId;
                                var seguimientoValoresEntidadCFC = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId &&
                                    x.PeriodosPeriodicidadId == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                var seguimientoValoresEntidadSinPeriodoCFC = niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                    .Where(x => x.PeriodoProyectoId == niveles2[i].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                detalleCFC.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCFC == null ? seguimientoValoresEntidadSinPeriodoCFC?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoId;
                                detalleCFC.PeriodosPeriodicidadId = seguimientoValoresEntidadCFC?.PeriodosPeriodicidadId;
                                detalleCFC.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosId;
                                detalleCFC.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                acumaladoVigenciaCFC = acumaladoVigenciaCFC + (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                if (q == 0)
                                    totalVigenciaCFC = 0;
                                else
                                    totalVigenciaCFC = acumaladoVigenciaCFC;

                                detalleCFC.ValorCostoFlujoCaja = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor;
                                detalleCFC.ValorCostoFlujoCajaAnterior = detalleCFC.ValorCostoFlujoCaja;
                                detalleCFC.ValorCantidad = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                detalleCFC.Observaciones = seguimientoValoresEntidadCFC?.Observaciones;
                                totalBaseCFC += detalleCFC.ValorCantidad;
                                listDetalleCFC.Add(detalleCFC);
                                foreach (var item in avanceCantidades)
                                {
                                    if (item.PeriodosPeriodicidadId == detalleCE.IdPeriodosPeriodicidad && item.Vigencia == niveles2[i].Vigencias[p].Vigencia
                                                && item.Mes == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCE.ActividadProgramacionSeguimientoId)
                                    {
                                        item.CantidadProgramadaVigencia = detalleCE.Valor;
                                        item.CantidadAcumuladaMeses = totalVigenciaCE == 0 ? 0 : totalVigenciaCE - (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                        item.CantidadEjecutadaMes = detalleCE.ValorCantidadEjecutada;
                                        item.CantidadEjecutadaMesAnterior = item.CantidadEjecutadaMes;
                                        item.Observaciones = detalleCE.Observaciones ?? "";
                                        item.ObservacionesAnterior = item.Observaciones;
                                    }
                                }

                                foreach (var item in costoPeriodo)
                                {
                                    if (item.PeriodosPeriodicidadId == detalleCP.IdPeriodosPeriodicidad && item.TipoCosto == "Presupuestal" && 
                                        item.Vigencia == niveles2[i].Vigencias[p].Vigencia
                                                && item.Mes == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCP.ActividadProgramacionSeguimientoId)
                                    {
                                        item.CostoProgramadoVigencia = detalleCP.ValorCantidad;
                                        item.CostoAcumuladoMeses = totalVigenciaCP == 0 ? 0 : totalVigenciaCP - (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                        item.CostoEjecutadoMes = detalleCP.ValorCostoPresupuestal;
                                        item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                        item.Observaciones = detalleCP.Observaciones ?? "";
                                        item.ObservacionesAnterior = item.Observaciones;
                                    }
                                }

                                foreach (var item in costoPeriodo)
                                {
                                    if (item.PeriodosPeriodicidadId == detalleCFC.IdPeriodosPeriodicidad && item.TipoCosto == "Flujo de caja" &&
                                        item.Vigencia == niveles2[i].Vigencias[p].Vigencia
                                                && item.Mes == niveles2[i].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCFC.ActividadProgramacionSeguimientoId)
                                    {
                                        item.CostoProgramadoVigencia = detalleCFC.ValorCantidad;
                                        item.CostoAcumuladoMeses = totalVigenciaCFC == 0 ? 0 : totalVigenciaCFC - (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                        item.CostoEjecutadoMes = detalleCFC.ValorCostoFlujoCaja;
                                        item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                        item.Observaciones = detalleCFC.Observaciones ?? "";
                                        item.ObservacionesAnterior = item.Observaciones;
                                    }
                                }
                            }
                            niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCE = listDetalleCE;
                            niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCP = listDetalleCP;
                            niveles2[i].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCFC = listDetalleCFC;
                            niveles2[i].Vigencias[p].TotalVigenciaCE = totalVigenciaCE;
                            niveles2[i].Vigencias[p].TotalVigenciaCEAnterior = totalVigenciaCE;
                            niveles2[i].Vigencias[p].TotalVigenciaCP = totalVigenciaCP;
                            niveles2[i].Vigencias[p].TotalVigenciaCPAnterior = totalVigenciaCP;
                            niveles2[i].Vigencias[p].TotalVigenciaCFC = totalVigenciaCFC;
                            niveles2[i].Vigencias[p].TotalVigenciaCFCAnterior = totalVigenciaCFC;
                            niveles2[i].Vigencias[p].TotalBaseCE = totalBaseCE;
                            niveles2[i].Vigencias[p].TotalBaseCP = totalBaseCP;
                            niveles2[i].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                            niveles2[i].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                        }
                        niveles2[i].AvanceCantidades = avanceCantidades;
                        niveles2[i].CostoPeriodo = costoPeriodo;
                    }

                    // Crea Nivel 3 o Actividades
                    double valornivel3 = 0;
                    var niveles3 = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == niveles2[i].SeguimientoEntregableId).ToList();
                    for (int j = 0; j < niveles3.Count; j++)
                    {
                        if (fechainicialNivel1 > niveles3[j].FechaInicio)
                            fechainicialNivel1 = niveles3[j].FechaInicio;
                        if (fechaFinalNivel1 < niveles3[j].FechaFin)
                            fechaFinalNivel1 = niveles3[j].FechaFin;
                        if (fechainicialNivel2 > niveles3[j].FechaInicio)
                            fechainicialNivel2 = niveles3[j].FechaInicio;
                        if (fechaFinalNivel2 < niveles3[j].FechaFin)
                            fechaFinalNivel2 = niveles3[j].FechaFin;
                        DateTime? fechainicialNivel3 = null;
                        DateTime? fechaFinalNivel3 = null;
                        if (fechainicialNivel3 == null)
                            fechainicialNivel3 = niveles2[i].FechaInicio;
                        if (fechainicialNivel3 > niveles2[i].FechaInicio)
                            fechainicialNivel3 = niveles2[i].FechaInicio;
                        if (fechaFinalNivel3 == null)
                            fechaFinalNivel3 = niveles2[i].FechaFin;
                        if (fechaFinalNivel3 < niveles2[i].FechaFin)
                            fechaFinalNivel3 = niveles2[i].FechaFin;
                        var consecutivoNivel3 = consecutivoNivel2 + "." + (j + 1);
                        niveles2[i].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(niveles3[j].SeguimientoEntregableId) });
                        niveles3[j].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(niveles3[j].SeguimientoEntregableId) });
                        niveles3[j].Consecutivo = consecutivoNivel3;

                        niveles3[j].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(niveles3[j].NombreEntregable);
                        niveles3[j].IdCompuesto = "nivel-3-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId + "" + niveles3[j].SeguimientoEntregableId;

                        
                        if (niveles3[j].Vigencias != null)
                        {
                            var valorCosto = niveles3[j].CostoTotal / (niveles3[j].CantidadTotal == null ? 1 : niveles3[j].CantidadTotal.Value == 0 ? 1 : niveles3[j].CantidadTotal.Value );
                            var avanceCantidades = new List<AvanceCantidadesDto>();
                            foreach (var item in periodos)
                            {
                                var avanceCantidadesData = new AvanceCantidadesDto();
                                avanceCantidadesData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                avanceCantidadesData.FaseId = item.FaseId;
                                avanceCantidadesData.Vigencia = item.Vigencia;
                                avanceCantidadesData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                avanceCantidadesData.FechaDesde = item.FechaDesde;
                                avanceCantidadesData.FechaHasta = item.FechaHasta;
                                avanceCantidadesData.Mes = item.Mes;
                                avanceCantidadesData.ActividadProgramacionSeguimientoId = niveles3[j].ActividadProgramacionSeguimientoId;
                                avanceCantidades.Add(avanceCantidadesData);
                            }

                            var costoPeriodo = new List<CostoPeriodoDto>();
                            foreach (var item in periodos)
                            {
                                var costoPeriodoData = new CostoPeriodoDto();
                                costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                costoPeriodoData.FaseId = item.FaseId;
                                costoPeriodoData.Vigencia = item.Vigencia;
                                costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                costoPeriodoData.FechaDesde = item.FechaDesde;
                                costoPeriodoData.FechaHasta = item.FechaHasta;
                                costoPeriodoData.Mes = item.Mes;
                                costoPeriodoData.ActividadProgramacionSeguimientoId = niveles3[j].ActividadProgramacionSeguimientoId;
                                costoPeriodoData.TipoCosto = "Presupuestal";
                                costoPeriodo.Add(costoPeriodoData);
                            }

                            foreach (var item in periodos)
                            {
                                var costoPeriodoData = new CostoPeriodoDto();
                                costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                costoPeriodoData.FaseId = item.FaseId;
                                costoPeriodoData.Vigencia = item.Vigencia;
                                costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                costoPeriodoData.FechaDesde = item.FechaDesde;
                                costoPeriodoData.FechaHasta = item.FechaHasta;
                                costoPeriodoData.Mes = item.Mes;
                                costoPeriodoData.ActividadProgramacionSeguimientoId = niveles3[j].ActividadProgramacionSeguimientoId;
                                costoPeriodoData.TipoCosto = "Flujo de caja";
                                costoPeriodo.Add(costoPeriodoData);
                            }
                            for (var p = 0; p < niveles3[j].Vigencias.Count; p++)
                            {
                                decimal totalVigenciaCE = 0;
                                double totalVigenciaCP = 0;
                                double totalVigenciaCFC = 0;
                                decimal totalBaseCE = 0;
                                double totalBaseCP = 0;
                                double totalBaseCFC = 0;
                                decimal acumaladoVigenciaCE = 0;
                                double acumaladoVigenciaCP = 0;
                                double acumaladoVigenciaCFC = 0;
                                var listDetalleCE = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                var listDetalleCP = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                var listDetalleCFC = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                for (var q = 0; q < niveles3[j].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                {
                                    var detalleCE = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCE.IdPeriodosPeriodicidad = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCE.Mes = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCE.Vigencia = niveles3[j].Vigencias[p].Vigencia;
                                    detalleCE.PeriodoProyectoId = niveles3[j].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidad = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodo = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCE.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                    detalleCE.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                    detalleCE.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCE.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                    acumaladoVigenciaCE = acumaladoVigenciaCE + (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                    if (q == 0)
                                        totalVigenciaCE = 0;
                                    else
                                        totalVigenciaCE = acumaladoVigenciaCE;

                                    detalleCE.ValorCantidadEjecutada = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor;
                                    detalleCE.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.ValorBase;
                                    detalleCE.ValorCantidadEjecutadaAnterior = detalleCE.ValorCantidadEjecutada;
                                    detalleCE.Observaciones = seguimientoValoresEntidad?.Observaciones;
                                    totalBaseCE += detalleCE.Valor;
                                    listDetalleCE.Add(detalleCE);

                                    var detalleCP = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCP.IdPeriodosPeriodicidad = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCP.Mes = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCP.Vigencia = niveles3[j].Vigencias[p].Vigencia;
                                    detalleCP.PeriodoProyectoId = niveles3[j].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidadCP = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodoCP = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCP.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCP == null ? seguimientoValoresEntidadSinPeriodoCP?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoId;
                                    detalleCP.PeriodosPeriodicidadId = seguimientoValoresEntidadCP?.PeriodosPeriodicidadId;
                                    detalleCP.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCP.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                    acumaladoVigenciaCP = acumaladoVigenciaCP + (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                    if (q == 0)
                                        totalVigenciaCP = 0;
                                    else
                                        totalVigenciaCP = acumaladoVigenciaCP;

                                    detalleCP.ValorCostoPresupuestal = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor;
                                    detalleCP.ValorCostoPresupuestalAnterior = detalleCP.ValorCostoPresupuestal;
                                    detalleCP.ValorCantidad = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                    detalleCP.Observaciones = seguimientoValoresEntidadCP?.Observaciones;
                                    totalBaseCP += detalleCP.ValorCantidad;
                                    listDetalleCP.Add(detalleCP);

                                    var detalleCFC = new ProgramacionSeguimientoPeriodosValoresDto();
                                    detalleCFC.IdPeriodosPeriodicidad = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                    detalleCFC.Mes = niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                    detalleCFC.Vigencia = niveles3[j].Vigencias[p].Vigencia;
                                    detalleCFC.PeriodoProyectoId = niveles3[j].Vigencias[p].PeriodoProyectoId;
                                    var seguimientoValoresEntidadCFC = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId &&
                                        x.PeriodosPeriodicidadId == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                    var seguimientoValoresEntidadSinPeriodoCFC = niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                        .Where(x => x.PeriodoProyectoId == niveles3[j].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                    detalleCFC.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCFC == null ? seguimientoValoresEntidadSinPeriodoCFC?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoId;
                                    detalleCFC.PeriodosPeriodicidadId = seguimientoValoresEntidadCFC?.PeriodosPeriodicidadId;
                                    detalleCFC.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosId;
                                    detalleCFC.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                    acumaladoVigenciaCFC = acumaladoVigenciaCFC + (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                    if (q == 0)
                                        totalVigenciaCFC = 0;
                                    else
                                        totalVigenciaCFC = acumaladoVigenciaCFC;

                                    detalleCFC.ValorCostoFlujoCaja = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor;
                                    detalleCFC.ValorCostoFlujoCajaAnterior = detalleCFC.ValorCostoFlujoCaja;
                                    detalleCFC.ValorCantidad = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                    detalleCFC.Observaciones = seguimientoValoresEntidadCFC?.Observaciones;
                                    totalBaseCFC += detalleCFC.ValorCantidad;
                                    listDetalleCFC.Add(detalleCFC);
                                    foreach (var item in avanceCantidades)
                                    {
                                        if (item.PeriodosPeriodicidadId == detalleCE.IdPeriodosPeriodicidad && item.Vigencia == niveles3[j].Vigencias[p].Vigencia
                                                && item.Mes == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCE.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CantidadProgramadaVigencia = detalleCE.Valor;
                                            item.CantidadAcumuladaMeses = totalVigenciaCE == 0 ? 0 : totalVigenciaCE - (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                            item.CantidadEjecutadaMes = detalleCE.ValorCantidadEjecutada;
                                            item.CantidadEjecutadaMesAnterior = item.CantidadEjecutadaMes;
                                            item.Observaciones = detalleCE.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }

                                    foreach (var item in costoPeriodo)
                                    {
                                        if (item.PeriodosPeriodicidadId == detalleCP.IdPeriodosPeriodicidad && item.TipoCosto == "Presupuestal" && 
                                            item.Vigencia == niveles3[j].Vigencias[p].Vigencia
                                                && item.Mes == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCP.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CostoProgramadoVigencia = detalleCP.ValorCantidad;
                                            item.CostoAcumuladoMeses = totalVigenciaCP == 0 ? 0 : totalVigenciaCP - (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                            item.CostoEjecutadoMes = detalleCP.ValorCostoPresupuestal;
                                            item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                            item.Observaciones = detalleCP.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }

                                    foreach (var item in costoPeriodo)
                                    {
                                        if (item.PeriodosPeriodicidadId == detalleCFC.IdPeriodosPeriodicidad && item.TipoCosto == "Flujo de caja" &&
                                            item.Vigencia == niveles3[j].Vigencias[p].Vigencia
                                                && item.Mes == niveles3[j].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCFC.ActividadProgramacionSeguimientoId)
                                        {
                                            item.CostoProgramadoVigencia = detalleCFC.ValorCantidad;
                                            item.CostoAcumuladoMeses = totalVigenciaCFC == 0 ? 0 : totalVigenciaCFC - (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                            item.CostoEjecutadoMes = detalleCFC.ValorCostoFlujoCaja;
                                            item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                            item.Observaciones = detalleCFC.Observaciones ?? "";
                                            item.ObservacionesAnterior = item.Observaciones;
                                        }
                                    }
                                }
                                niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCE = listDetalleCE;
                                niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCP = listDetalleCP;
                                niveles3[j].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCFC = listDetalleCFC;
                                niveles3[j].Vigencias[p].TotalVigenciaCE = totalVigenciaCE;
                                niveles3[j].Vigencias[p].TotalVigenciaCEAnterior = totalVigenciaCE;
                                niveles3[j].Vigencias[p].TotalVigenciaCP = totalVigenciaCP;
                                niveles3[j].Vigencias[p].TotalVigenciaCPAnterior = totalVigenciaCP;
                                niveles3[j].Vigencias[p].TotalVigenciaCFC = totalVigenciaCFC;
                                niveles3[j].Vigencias[p].TotalVigenciaCFCAnterior = totalVigenciaCFC;
                                niveles3[j].Vigencias[p].TotalBaseCE = totalBaseCE;
                                niveles3[j].Vigencias[p].TotalBaseCP = totalBaseCP;
                                niveles3[j].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                                niveles3[j].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                            }
                            niveles3[j].AvanceCantidades = avanceCantidades;
                            niveles3[j].CostoPeriodo = costoPeriodo;
                        }
                        
                        // Actividades
                        var actividades = entregableNv1.SeguimientoEntregables.Where(p => p.SeguimientoEntregablePadreId == niveles3[j].SeguimientoEntregableId).ToList();
                        double valorActividades = 0;
                        for (int iac = 0; iac < actividades.Count; iac++)
                        {
                            if (fechainicialNivel1 > actividades[iac].FechaInicio)
                                fechainicialNivel1 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel1 < actividades[iac].FechaFin)
                                fechaFinalNivel1 = actividades[iac].FechaFin;
                            if (fechainicialNivel2 > actividades[iac].FechaInicio)
                                fechainicialNivel2 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel2 < actividades[iac].FechaFin)
                                fechaFinalNivel2 = actividades[iac].FechaFin;
                            if (fechainicialNivel3 > actividades[iac].FechaInicio)
                                fechainicialNivel3 = actividades[iac].FechaInicio;
                            if (fechaFinalNivel3 < actividades[iac].FechaFin)
                                fechaFinalNivel3 = actividades[iac].FechaFin;
                            var consecutivoNivel4 = consecutivoNivel3 + "." + (iac + 1);
                            niveles2[i].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            niveles3[j].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            actividades[iac].Consecutivo = consecutivoNivel4;
                            actividades[iac].Descendencia.Add(new DescendenciaModel() { ActividadSeguimientoId = Convert.ToInt32(actividades[iac].SeguimientoEntregableId) });
                            actividades[iac].NombreEntregableCorto = ConsultaObjetivosProyecto.ObtenerNombreCorto(actividades[iac].NombreEntregable);
                            actividades[iac].IdCompuesto = "Actividades-" + entregableNv1.ActividadId + "" + niveles2[i].SeguimientoEntregableId + "" + niveles3[j].SeguimientoEntregableId + "" + actividades[iac].SeguimientoEntregableId;
                            
                            valorActividades = valorActividades + (actividades[iac].CostoTotal == null ? 0 : actividades[iac].CostoTotal.Value);
                            if (actividades[iac].Vigencias != null)
                            {
                                var valorCosto = actividades[iac].CostoTotal / (actividades[iac].CantidadTotal == null ? 1 : actividades[iac].CantidadTotal.Value == 0 ? 1 : actividades[iac].CantidadTotal.Value);
                                var avanceCantidades = new List<AvanceCantidadesDto>();
                                foreach (var item in periodos)
                                {
                                    var avanceCantidadesData = new AvanceCantidadesDto();
                                    avanceCantidadesData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    avanceCantidadesData.FaseId = item.FaseId;
                                    avanceCantidadesData.Vigencia = item.Vigencia;
                                    avanceCantidadesData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    avanceCantidadesData.FechaDesde = item.FechaDesde;
                                    avanceCantidadesData.FechaHasta = item.FechaHasta;
                                    avanceCantidadesData.Mes = item.Mes;
                                    avanceCantidadesData.ActividadProgramacionSeguimientoId = actividades[iac].ActividadProgramacionSeguimientoId;
                                    avanceCantidades.Add(avanceCantidadesData);
                                }

                                var costoPeriodo = new List<CostoPeriodoDto>();
                                foreach (var item in periodos)
                                {
                                    var costoPeriodoData = new CostoPeriodoDto();
                                    costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    costoPeriodoData.FaseId = item.FaseId;
                                    costoPeriodoData.Vigencia = item.Vigencia;
                                    costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    costoPeriodoData.FechaDesde = item.FechaDesde;
                                    costoPeriodoData.FechaHasta = item.FechaHasta;
                                    costoPeriodoData.Mes = item.Mes;
                                    costoPeriodoData.ActividadProgramacionSeguimientoId = actividades[iac].ActividadProgramacionSeguimientoId;
                                    costoPeriodoData.TipoCosto = "Presupuestal";
                                    costoPeriodo.Add(costoPeriodoData);
                                }

                                foreach (var item in periodos)
                                {
                                    var costoPeriodoData = new CostoPeriodoDto();
                                    costoPeriodoData.CalendarioPeriodoId = item.CalendarioPeriodoId;
                                    costoPeriodoData.FaseId = item.FaseId;
                                    costoPeriodoData.Vigencia = item.Vigencia;
                                    costoPeriodoData.PeriodosPeriodicidadId = item.PeriodosPeriodicidadId;
                                    costoPeriodoData.FechaDesde = item.FechaDesde;
                                    costoPeriodoData.FechaHasta = item.FechaHasta;
                                    costoPeriodoData.Mes = item.Mes;
                                    costoPeriodoData.ActividadProgramacionSeguimientoId = actividades[iac].ActividadProgramacionSeguimientoId;
                                    costoPeriodoData.TipoCosto = "Flujo de caja";
                                    costoPeriodo.Add(costoPeriodoData);
                                }
                                for (var p = 0; p < actividades[iac].Vigencias.Count; p++)
                                {
                                    decimal totalVigenciaCE = 0;
                                    double totalVigenciaCP = 0;
                                    double totalVigenciaCFC = 0;
                                    decimal totalBaseCE = 0;
                                    double totalBaseCP = 0;
                                    double totalBaseCFC = 0;
                                    decimal acumaladoVigenciaCE = 0;
                                    double acumaladoVigenciaCP = 0;
                                    double acumaladoVigenciaCFC = 0;
                                    var listDetalleCE = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                    var listDetalleCP = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                    var listDetalleCFC = new List<ProgramacionSeguimientoPeriodosValoresDto>();
                                    for (var q = 0; q < actividades[iac].Vigencias[p].PeriodosPeriodicidad.Count; q++)
                                    {
                                        var detalleCE = new ProgramacionSeguimientoPeriodosValoresDto();
                                        detalleCE.IdPeriodosPeriodicidad = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                        detalleCE.Mes = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                        detalleCE.Vigencia = actividades[iac].Vigencias[p].Vigencia;
                                        detalleCE.PeriodoProyectoId = actividades[iac].Vigencias[p].PeriodoProyectoId;
                                        var seguimientoValoresEntidad = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId &&
                                            x.PeriodosPeriodicidadId == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                        var seguimientoValoresEntidadSinPeriodo = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCantidadEjecutada
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                        detalleCE.ActividadProgramacionSeguimientoId = seguimientoValoresEntidad == null ? seguimientoValoresEntidadSinPeriodo?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidad?.ActividadProgramacionSeguimientoId;
                                        detalleCE.PeriodosPeriodicidadId = seguimientoValoresEntidad?.PeriodosPeriodicidadId;
                                        detalleCE.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosId;
                                        detalleCE.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidad?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                        acumaladoVigenciaCE = acumaladoVigenciaCE + (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                        if (q == 0)
                                            totalVigenciaCE = 0;
                                        else
                                            totalVigenciaCE = acumaladoVigenciaCE;

                                        detalleCE.ValorCantidadEjecutada = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor;
                                        detalleCE.Valor = seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.ValorBase;
                                        detalleCE.ValorCantidadEjecutadaAnterior = detalleCE.ValorCantidadEjecutada;
                                        detalleCE.Observaciones = seguimientoValoresEntidad?.Observaciones;
                                        totalBaseCE += detalleCE.Valor;
                                        listDetalleCE.Add(detalleCE);

                                        var detalleCP = new ProgramacionSeguimientoPeriodosValoresDto();
                                        detalleCP.IdPeriodosPeriodicidad = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                        detalleCP.Mes = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                        detalleCP.Vigencia = actividades[iac].Vigencias[p].Vigencia;
                                        detalleCP.PeriodoProyectoId = actividades[iac].Vigencias[p].PeriodoProyectoId;
                                        var seguimientoValoresEntidadCP = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId &&
                                            x.PeriodosPeriodicidadId == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                        var seguimientoValoresEntidadSinPeriodoCP = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoPresupuestal
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                        detalleCP.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCP == null ? seguimientoValoresEntidadSinPeriodoCP?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoId;
                                        detalleCP.PeriodosPeriodicidadId = seguimientoValoresEntidadCP?.PeriodosPeriodicidadId;
                                        detalleCP.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosId;
                                        detalleCP.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCP?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                        acumaladoVigenciaCP = acumaladoVigenciaCP + (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                        if (q == 0)
                                            totalVigenciaCP = 0;
                                        else
                                            totalVigenciaCP = acumaladoVigenciaCP;

                                        detalleCP.ValorCostoPresupuestal = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor;
                                        detalleCP.ValorCostoPresupuestalAnterior = detalleCP.ValorCostoPresupuestal;
                                        detalleCP.ValorCantidad = seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                        detalleCP.Observaciones = seguimientoValoresEntidadCP?.Observaciones;
                                        totalBaseCP += detalleCP.ValorCantidad;
                                        listDetalleCP.Add(detalleCP);

                                        var detalleCFC = new ProgramacionSeguimientoPeriodosValoresDto();
                                        detalleCFC.IdPeriodosPeriodicidad = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id;
                                        detalleCFC.Mes = actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes;
                                        detalleCFC.Vigencia = actividades[iac].Vigencias[p].Vigencia;
                                        detalleCFC.PeriodoProyectoId = actividades[iac].Vigencias[p].PeriodoProyectoId;
                                        var seguimientoValoresEntidadCFC = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId &&
                                            x.PeriodosPeriodicidadId == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Id).FirstOrDefault();
                                        var seguimientoValoresEntidadSinPeriodoCFC = actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja
                                            .Where(x => x.PeriodoProyectoId == actividades[iac].Vigencias[p].PeriodoProyectoId).FirstOrDefault();
                                        detalleCFC.ActividadProgramacionSeguimientoId = seguimientoValoresEntidadCFC == null ? seguimientoValoresEntidadSinPeriodoCFC?.ActividadProgramacionSeguimientoId : seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoId;
                                        detalleCFC.PeriodosPeriodicidadId = seguimientoValoresEntidadCFC?.PeriodosPeriodicidadId;
                                        detalleCFC.ActividadProgramacionSeguimientoPeriodosId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosId;
                                        detalleCFC.ActividadProgramacionSeguimientoPeriodosValoresId = seguimientoValoresEntidadCFC?.ActividadProgramacionSeguimientoPeriodosValoresId;

                                        acumaladoVigenciaCFC = acumaladoVigenciaCFC + (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                        if (q == 0)
                                            totalVigenciaCFC = 0;
                                        else
                                            totalVigenciaCFC = acumaladoVigenciaCFC;

                                        detalleCFC.ValorCostoFlujoCaja = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor;
                                        detalleCFC.ValorCostoFlujoCajaAnterior = detalleCFC.ValorCostoFlujoCaja;
                                        detalleCFC.ValorCantidad = seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.ValorBase * (valorCosto == null ? 0 : valorCosto.Value);
                                        detalleCFC.Observaciones = seguimientoValoresEntidadCFC?.Observaciones;
                                        totalBaseCFC += detalleCFC.ValorCantidad;
                                        listDetalleCFC.Add(detalleCFC);
                                        foreach (var item in avanceCantidades)
                                        {
                                            if (item.PeriodosPeriodicidadId == detalleCE.IdPeriodosPeriodicidad && item.Vigencia == actividades[iac].Vigencias[p].Vigencia
                                                && item.Mes == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes && 
                                                item.ActividadProgramacionSeguimientoId == detalleCE.ActividadProgramacionSeguimientoId)
                                            {
                                                item.CantidadProgramadaVigencia = detalleCE.Valor;
                                                item.CantidadAcumuladaMeses = totalVigenciaCE == 0 ? 0 : totalVigenciaCE - (seguimientoValoresEntidad == null ? 0 : seguimientoValoresEntidad.Valor);
                                                item.CantidadEjecutadaMes = detalleCE.ValorCantidadEjecutada;
                                                item.CantidadEjecutadaMesAnterior = item.CantidadEjecutadaMes;
                                                item.Observaciones = detalleCE.Observaciones ?? "";
                                                item.ObservacionesAnterior = item.Observaciones;
                                            }
                                        }

                                        foreach (var item in costoPeriodo)
                                        {
                                            if (item.PeriodosPeriodicidadId == detalleCP.IdPeriodosPeriodicidad && item.TipoCosto == "Presupuestal" && 
                                                item.Vigencia == actividades[iac].Vigencias[p].Vigencia
                                                && item.Mes == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCP.ActividadProgramacionSeguimientoId)
                                            {
                                                item.CostoProgramadoVigencia = detalleCP.ValorCantidad;
                                                item.CostoAcumuladoMeses = totalVigenciaCP == 0 ? 0 : totalVigenciaCP - (seguimientoValoresEntidadCP == null ? 0 : seguimientoValoresEntidadCP.Valor);
                                                item.CostoEjecutadoMes = detalleCP.ValorCostoPresupuestal;
                                                item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                                item.Observaciones = detalleCP.Observaciones ?? "";
                                                item.ObservacionesAnterior = item.Observaciones;
                                            }
                                        }

                                        foreach (var item in costoPeriodo)
                                        {
                                            if (item.PeriodosPeriodicidadId == detalleCFC.IdPeriodosPeriodicidad && item.TipoCosto == "Flujo de caja" && 
                                                item.Vigencia == actividades[iac].Vigencias[p].Vigencia
                                                && item.Mes == actividades[iac].Vigencias[p].PeriodosPeriodicidad[q].Mes &&
                                                item.ActividadProgramacionSeguimientoId == detalleCFC.ActividadProgramacionSeguimientoId)
                                            {
                                                item.CostoProgramadoVigencia = detalleCFC.ValorCantidad;
                                                item.CostoAcumuladoMeses = totalVigenciaCFC == 0 ? 0 : totalVigenciaCFC - (seguimientoValoresEntidadCFC == null ? 0 : seguimientoValoresEntidadCFC.Valor);
                                                item.CostoEjecutadoMes = detalleCFC.ValorCostoFlujoCaja;
                                                item.CostoEjecutadoMesAnterior = item.CostoEjecutadoMes;
                                                item.Observaciones = detalleCFC.Observaciones ?? "";
                                                item.ObservacionesAnterior = item.Observaciones;
                                            }
                                        }
                                    }
                                    actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCE = listDetalleCE;
                                    actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCP = listDetalleCP;
                                    actividades[iac].Vigencias[p].ProgramacionSeguimientoPeriodosValoresCFC = listDetalleCFC;
                                    actividades[iac].Vigencias[p].TotalVigenciaCE = totalVigenciaCE;
                                    actividades[iac].Vigencias[p].TotalVigenciaCEAnterior = totalVigenciaCE;
                                    actividades[iac].Vigencias[p].TotalVigenciaCP = totalVigenciaCP;
                                    actividades[iac].Vigencias[p].TotalVigenciaCPAnterior = totalVigenciaCP;
                                    actividades[iac].Vigencias[p].TotalVigenciaCFC = totalVigenciaCFC;
                                    actividades[iac].Vigencias[p].TotalVigenciaCFCAnterior = totalVigenciaCFC;
                                    actividades[iac].Vigencias[p].TotalBaseCE = totalBaseCE;
                                    actividades[iac].Vigencias[p].TotalBaseCP = totalBaseCP;
                                    actividades[iac].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                                    actividades[iac].Vigencias[p].TotalBaseCFC = totalBaseCFC;
                                }
                                actividades[iac].AvanceCantidades = avanceCantidades;
                                actividades[iac].CostoPeriodo = costoPeriodo;
                            }
                            

                        }
                        niveles3[j].CostoTotal = valorActividades == 0 ? niveles3[j].CostoTotal : valorActividades;
                        valornivel3 = valornivel3 + (niveles3[j].CostoTotal == null ? 0 : niveles3[j].CostoTotal.Value);
                        niveles3[j].Hijos = actividades;
                        niveles3[j].FechaInicio = fechainicialNivel3;
                        niveles3[j].FechaFin = fechaFinalNivel3;
                    }
                    niveles2[i].CostoTotal = valornivel3;
                    valornivel2 = valornivel2 + (niveles2[i].CostoTotal == null ? 0 : niveles2[i].CostoTotal.Value);
                    niveles2[i].Hijos = niveles3;
                    niveles2[i].ContieneSiguienteNivel = niveles2[i].Hijos.Exists(p => p.NivelEntregable == "Nivel 3");
                    niveles2[i].FechaInicio = fechainicialNivel2;
                    niveles2[i].FechaFin = fechaFinalNivel2;
                }

                entregablesRegistrados = niveles2;
                acumulado = valornivel2;
                entregableNv1.FechaInicio = fechainicialNivel1;
                entregableNv1.FechaFin = fechaFinalNivel1;
            }


            return new JerarquiaNiveles() { ListadoNiveles = entregablesRegistrados, EntergableNivel1 = entregableNv1 };
        }

        private static List<RequisitosObligatorios> CrearRquisitosObligatorios(EntregablesNivel1 entregable)
        {
            var listadoObligatorios = new List<string>();
            listadoObligatorios = (entregable.CatalogoEntregables != null) ? entregable.CatalogoEntregables.OrderBy(p => p.Nivel).GroupBy(p => p.Nivel).Select(p => p.Key).ToList() : new List<string>();

            var index = 2;
            foreach (var requisitos in listadoObligatorios)
            {
                if (requisitos != "Actividad")
                {
                    entregable.RequisitosObligatorios.Add(new RequisitosObligatorios()
                    {
                        Nombre = requisitos,
                        TipoEntregable = requisitos,
                        RequisitoCumple = false,
                        Position = index
                    });
                    index++;
                }
            }

            entregable.RequisitosObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Nivel 1",
                TipoEntregable = "Nivel 1",
                RequisitoCumple = true,
                Position = 1
            });

            entregable.RequisitosObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Actividades",
                TipoEntregable = "Actividad",
                RequisitoCumple = false,
                Position = entregable.RequisitosObligatorios.Count + 1
            });

            return entregable.RequisitosObligatorios;
        }

        private static List<RequisitosObligatorios> CrearRequisitosGenerales()
        {
            var listadoObligatorios = new List<RequisitosObligatorios>();

            listadoObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Nivel 1",
                TipoEntregable = "Nivel 1",
                RequisitoCumple = true,
                Position = 1
            });

            listadoObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Nivel 2",
                TipoEntregable = "Nivel 2",
                RequisitoCumple = true,
                Position = 2
            });

            listadoObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Nivel 3",
                TipoEntregable = "Nivel 3",
                RequisitoCumple = true,
                Position = 3
            });


            listadoObligatorios.Add(new RequisitosObligatorios()
            {
                Nombre = "Actividades",
                TipoEntregable = "Actividad",
                RequisitoCumple = false,
                Position = 4
            });

            return listadoObligatorios;
        }

        private static List<RequisitosObligatorios> ObtenerRequisitosObligatorios(List<List<RequisitosObligatorios>> requisitos)
        {
            var listadoObligatorios = new List<RequisitosObligatorios>();
            for (int i = 0; i < requisitos.Count; i++)
            {
                for (int j = 0; j < requisitos[i].Count; j++)
                {
                    listadoObligatorios.Add(requisitos[i][j]);
                }
            }
            return listadoObligatorios;
        }
    }

    public class CatalogoEntregable
    {
        public int DeliverableCatalogId { get; set; }
        public string Nivel { get; set; }
        public string NombreEntregable { get; set; }
        public int parentId { get; set; }
        public int EntregableIdPrimerNivel { get; set; }
        public int ClassificacionId { get; set; }
        public int ClasificacionPadreId { get; set; }
        public int DeliverableCatalogPadreId { get; set; }
        public int UnidadMedidaId { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int? Padreid { get; set; }
        public string UnidadMedida { get; set; }
    }

    public class EntregablesNivel1: ProgramarActividadesDto
    {
        public int? DeliverableCatalogId { get; set; }
        public int IndexObjetivo { get; set; }
        public int IndexProducto { get; set; }
        public int IndexNivel1 { get; set; }
        public bool Deliverable { get; set; }
        public bool Nivel2 { get; set; }
        public bool Nivel3 { get; set; }
        public double Costo { get; set; }
        public string NumeroEntregableNivel1 { get; set; }
        public List<RequisitosObligatorios> RequisitosObligatorios { get; set; }
        public List<CatalogoEntregable> CatalogoEntregables { get; set; }
        public List<SeguimientoEntregable> SeguimientoEntregables { get; set; }
        public List<SeguimientoEntregable> NivelesRegistrados { get; set; }

        public EntregablesNivel1()
        {
            RequisitosObligatorios = new List<RequisitosObligatorios>();
            SeguimientoEntregables = new List<SeguimientoEntregable>();
            NivelesRegistrados = new List<SeguimientoEntregable>();
        }
    }

    public class RequisitosObligatorios
    {
        public string Nombre { get; set; }
        public string TipoEntregable { get; set; }
        public bool RequisitoCumple { get; set; }
        public int Position { get; set; }
    }

    public class Objetivo
    {
        public string IdCompuesto { get; set; }
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string ObjetivoEspecificoCorto { get; set; }
        public string NumeroObjetivo { get; set; }
        public List<Producto> Productos { get; set; }
    }

    public class Producto
    {
        public string IdCompuesto { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string NombreProductoCorto { get; set; }
        public string IndicadorPrincipal { get; set; }
        public int IndicadorId { get; set; }
        public string UnidadMedidaProducto { get; set; }
        public double Cantidad { get; set; }
        public double CostoProducto { get; set; }
        public string EsAcumulativo { get; set; }
        public string NumeroProducto { get; set; }
        public List<EntregablesNivel1> EntregablesNivel1 { get; set; }
        public List<EntregablesNivel1> Actividades { get; set; }
        public List<RequisitosObligatorios> RequisitosObligatorios { get; set; }
        public Producto()
        {
            RequisitosObligatorios = new List<RequisitosObligatorios>();
            EntregablesNivel1 = new List<EntregablesNivel1>();
            Actividades = new List<EntregablesNivel1>();
        }
    }

    public class SeguimientoEntregable : ProgramarActividadesDto
    {
        public int? SeguimientoEntregableIdPredecesora { get; set; }
        public string NivelEntregable { get; set; }
        public int? EntregableCatalogId { get; set; }
        public string CodigoCatalogoEntregable { get; set; }
        public int? SeguimientoEntregablePadreId { get; set; }
        public string TipoEntregable { get; set; }
        public string NumeroEntregableNivel2 { get; set; }
        public string NumeroEntregableNivel3 { get; set; }
        public string NumeroActividad { get; set; }

        public List<SeguimientoEntregable> Hijos { get; set; }
        public List<DescendenciaModel> Descendencia { get; set; }
        public bool HijosCumplenObligatorio { get; set; }
        public bool ContieneSiguienteNivel { get; set; }
        public SeguimientoEntregable()
        {
            Hijos = new List<SeguimientoEntregable>();
            Descendencia = new List<DescendenciaModel>();
        }
    }

    public class ReponseHttp
    {
        public bool Status { get; set; }
        public string Message { get; set; }

    }

    public class RegistroModel
    {
        public string Tipo { get; set; }
        public List<RegistroEntregable> NivelesNuevos { get; set; }
    }

    public class DescendenciaModel
    {
        public int ActividadSeguimientoId { get; set; }
    }

    public class RegistroEntregable
    {
        public int? DeliverableCatalogId { get; set; }
        public string Nivel { get; set; }
        public string NombreEntregable { get; set; }
        public int ProductoId { get; set; }
        public int? Padreid { get; set; }
        public int ActividadId { get; set; }
        public int? UnidadMedidaId { get; set; }
        public int ActividadSeguimientoId { get; set; }
    }

    public class JerarquiaNiveles
    {
        public List<SeguimientoEntregable> ListadoNiveles { get; set; }
        public EntregablesNivel1 EntergableNivel1 { get; set; }
    }

    public class ConteoObligatorios
    {
        public int? IdEntregable { get; set; }
        public int CantidadAgrupada { get; set; }
        public string Nivel { get; set; }
    }

    public class ListadoCantidadObligatorios
    {
        public int Index { get; set; }
        public List<ConteoObligatorios> ConteoObligatorios { get; set; }
        public string Nivel { get; set; }
    }

    public class ProgramarActividadesDto
    {
        public int? ActividadId { get; set; }
        public int? SeguimientoEntregableId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PredecesoraId { get; set; }
        public int? SeguimientoEntregablePredecesoraId { get; set; }
        public int ProyectoId { get; set; }
        public string Tipo { get; set; }
        public string Bpin { get; set; }
        public string TipoSigla { get; set; }
        public string Consecutivo { get; set; }
        public string NombreEntregableCorto { get; set; }
        public string NombreEntregable { get; set; }
        public string IdCompuesto { get; set; }
        public double? CostoTotal { get; set; }
        public double? CostoUnitario { get; set; }
        public int? CantidadTotal { get; set; }
        public float? DuracionOptimista { get; set; }
        public float? DuracionPesimista { get; set; }
        public float? DuracionProbable { get; set; }
        public float? DuracionPromedio { get; set; }
        public string UnidadMedida { get; set; }
        public int? UnidadMedidaId { get; set; }
        public string NombreActividad { get; set; }
        public string NombrePredecesora { get; set; }
        public int? PosPosicion { get; set; }
        public int? Adelanto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool HabilitaEditar { get; set; }
        public decimal TotalVigencia { get; set; }
        public decimal TotalVigenciaAnterior { get; set; }
        public string NumeroActividad { get; set; }
        public List<VigenciaEntregableDto> Vigencias { get; set; }
        public List<AvanceCantidadesDto> AvanceCantidades { get; set; }
        public List<CostoPeriodoDto> CostoPeriodo { get; set; }
    }

    public class AvanceCantidadesDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public decimal CantidadProgramadaVigencia { get; set; }
        public decimal CantidadAcumuladaMeses { get; set; }
        public decimal CantidadEjecutadaMes { get; set; }
        public decimal CantidadEjecutadaMesAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
    }

    public class CostoPeriodoDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public double CostoProgramadoVigencia { get; set; }
        public double CostoAcumuladoMeses { get; set; }
        public double CostoEjecutadoMes { get; set; }
        public double CostoEjecutadoMesAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public string TipoCosto { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
    }
}
