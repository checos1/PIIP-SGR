namespace DNP.Backbone.Web.API
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.Util;
    using DNP.Autorizacion.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto;
    using Newtonsoft.Json;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
    using DNP.Backbone.Dominio.Dto.Flujos;
    using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
    using System.Collections;

    [ExcludeFromCodeCoverage]
    public static class ExcelUtilidades
    {
        public static ByteArrayContent ObtenerExcell(InboxDto _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Proyectos");
            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasProyectos(ColumnasVisibles);
            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre proyectos por entidad y sectores", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.GruposEntidades == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.GruposEntidades)
                {
                    foreach (var entidad in item.ListaEntidades)
                    {
                        foreach (var obj in entidad.ObjetosNegocio)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;
                            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoId"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.Numeric);
                                cellInfoA.SetCellValue((double)obj.ProyectoId.GetValueOrDefault());
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "IdObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.IdObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.SectorNombre);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }

                            if (esColumnaPorNombre(ColumnasVisibles, "NombreEntidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "DescripcionCR"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.DescripcionCR);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "Horizonte"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.Horizonte);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "Criticidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.Criticidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.EstadoProyecto);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            num++;
                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);


            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellProyecto(DNP.Backbone.Dominio.Dto.Proyecto.ProyectoDto _result, ProyectoFiltroDto proyectoFiltroDto)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Proyectos");
            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasProyectos(ColumnasVisibles);
            var laPrimeraFila = 6;


            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            if (_result.GruposEntidades.Count > 0)
                addInfo(ref excel, ref sheet, reporte: "Mis Procesos - Proyectos", fuente: $"{proyectoFiltroDto.Macroproceso} / {_result.GruposEntidades.FirstOrDefault().TipoEntidad}");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.GruposEntidades == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.GruposEntidades)
                {
                    foreach (var entidad in item.ListaEntidades)
                    {
                        foreach (var obj in entidad.ObjetosNegocio)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;
                            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.SectorNombre);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreEntidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoId"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.Numeric);
                                cellInfoA.SetCellValue((double)obj.ProyectoId.GetValueOrDefault());
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "IdObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.IdObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.EstadoProyecto);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreFlujo);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "CodigoProceso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.CodigoProceso);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoInstancia"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.EstadoInstancia);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "FechaCreacion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.HasValue ? obj.FechaCreacion.Value.ToString("dd-MM-yyyy hh:mm:ss") : String.Empty);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreAccion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "FechaPaso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaPaso.HasValue ? obj.FechaPaso.Value.ToString("dd-MM-yyyy hh:mm:ss") : String.Empty);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            sheet.AutoSizeColumn(num);
                            num++;

                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);


            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);

            addImagem(ref excel, ref sheet);
            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }


        public static ByteArrayContent ObtenerExcellProyectoConsola(DNP.Backbone.Dominio.Dto.Proyecto.ProyectoDto _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Proyectos");
            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasProyectosConsola(ColumnasVisibles);
            var laPrimeraFila = 6;


            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            if (_result.GruposEntidades.Count > 0)
                addInfo(ref excel, ref sheet, reporte: "Consola de procesos - Proyectos", fuente: $"{_result.GruposEntidades.FirstOrDefault().TipoEntidad}");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.GruposEntidades == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.GruposEntidades)
                {
                    foreach (var entidad in item.ListaEntidades)
                    {
                        foreach (var obj in entidad.ObjetosNegocio)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;
                            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.SectorNombre);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreEntidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoId"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.Numeric);
                                cellInfoA.SetCellValue((double)obj.ProyectoId.GetValueOrDefault());
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "IdObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.IdObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.EstadoProyecto);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreObjetoNegocio"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreObjetoNegocio);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "Macroproceso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.Macroproceso);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreFlujo);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "CodigoProceso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.CodigoProceso);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoInstancia"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.EstadoInstancia);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "FechaCreacion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.HasValue ? obj.FechaCreacion.Value.ToString("dd-MM-yyyy hh:mm:ss") : String.Empty);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreAccion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "FechaPaso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaPaso.HasValue ? obj.FechaPaso.Value.ToString("dd-MM-yyyy hh:mm:ss") : String.Empty);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            sheet.AutoSizeColumn(num);
                            num++;

                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);


            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);

            addImagem(ref excel, ref sheet);
            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }


        /// <summary>
        ///  Obtiene un arreglo de bytes del excel generado. Consola/Proyectos
        /// </summary>
        /// <param name="datos">Información a presentar en el excel generado. <see cref="DNP.Backbone.Dominio.Dto.Proyecto.ProyectoDto"/></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] ObtenerExcelConsolaProyecto(DNP.Backbone.Dominio.Dto.Proyecto.ProyectoDto datos)
        {
            var listaBytes = new List<byte>();

            // diccionario de columna-propiedad
            var columnaPropiedades = new Dictionary<String, String>
                {
                    { "Sector", "SectorNombre"},
                    { "Entidad", "NombreEntidad"},
                    { "ID", "ProyectoId"},
                    { "BPIN", "IdObjetoNegocio"},
                    { "Estado", "EstadoProyecto"},
                    { "Nombre del proyecto", "NombreObjetoNegocio"},
                    { "Horizonte", "Horizonte"}
                };
            var numeroFila = 6;
            try
            {
                IWorkbook excel = new XSSFWorkbook();
                ISheet pestania = excel.CreateSheet("Proyectos");

                
                addColumnas(ref excel, ref pestania, datos.ColumnasVisibles.ToList(), numeroFila);
                addInfo(ref excel, ref pestania, reporte: "Información sobre proyectos por entidad y sectores", fuente: "BACKBONE");

                #region Estilos
                IFont fontCelda = excel.CreateFont();
                fontCelda.IsBold = false;
                fontCelda.FontName = "Calibri";
                fontCelda.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
                ICellStyle estiloCelda = excel.CreateCellStyle();
                estiloCelda.SetFont(fontCelda);
                estiloCelda.BorderBottom = BorderStyle.None;
                estiloCelda.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                estiloCelda.FillPattern = FillPattern.SolidForeground;

                IFont fontCeldaAlternativa = excel.CreateFont();
                fontCeldaAlternativa.IsBold = false;
                fontCeldaAlternativa.FontName = "Calibri";
                fontCeldaAlternativa.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
                ICellStyle estiloCeldaAlternativa = excel.CreateCellStyle();
                estiloCeldaAlternativa.SetFont(fontCeldaAlternativa);
                estiloCeldaAlternativa.BorderBottom = BorderStyle.None;
                estiloCeldaAlternativa.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                estiloCeldaAlternativa.FillPattern = FillPattern.SolidForeground;
                #endregion Estilos

                if (datos.GruposEntidades == null)
                {
                    IRow row = pestania.CreateRow(numeroFila++);
                    row.CreateCell(0).SetCellValue(datos.Mensaje);
                }
                else
                {
                    datos.GruposEntidades.SelectMany(p => p.ListaEntidades.SelectMany(q => q.ObjetosNegocio)).ToList().ForEach(proyecto =>
                    {
                        numeroFila++;

                        IRow fila = pestania.CreateRow(numeroFila);

                        /// obtener las propiedades del objecto actual 'proyecto' <see cref="Dominio.Dto.Proyecto.NegocioDto"/>
                        var propiedadLista = proyecto.GetType().GetProperties().Select((p, index) => new
                        {
                            Index = index,
                            Propiedad = p.Name,
                            Valor = p.GetValue(proyecto)
                        }).ToList();

                        var columnaPropiedadLista = columnaPropiedades.Where(p => datos.ColumnasVisibles.Contains(p.Key)).Select(p => p.Value).ToList();
                        propiedadLista = propiedadLista.Where(p => columnaPropiedadLista.Contains(p.Propiedad)).Select(p => new
                        {
                            Index = columnaPropiedadLista.IndexOf(p.Propiedad),
                            p.Propiedad,
                            p.Valor
                        }).OrderBy(p => p.Index).ToList();

                        var numeroColumna = 0;

                        propiedadLista.ForEach(columna =>
                        {
                            ICell celda = fila.CreateCell(numeroColumna++, ObtenerTipoCelda(columna.Valor));
                            EstablecerValorCelda(celda, columna.Valor);
                            pestania.AutoSizeColumn(numeroColumna);
                            if (numeroFila % 2 != 0)
                                celda.CellStyle = estiloCelda;
                            else
                                celda.CellStyle = estiloCeldaAlternativa;

                        });// fin: columnas valores
                    });// fin: proyectos;
                }
                
                pestania.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, numeroFila, 0, datos.ColumnasVisibles.Length - 1));
                pestania.CreateFreezePane(0, 7, 0, 7);
                
                numeroFila++;
                IRow rowInvalid = pestania.CreateRow(numeroFila);
                var lt = numeroFila + 20;

                while (numeroFila <= lt)
                {
                    numeroFila++;
                    rowInvalid = pestania.CreateRow(numeroFila);
                    rowInvalid.CreateCell(0).SetCellValue(" ");
                    rowInvalid.CreateCell(1).SetCellValue(" ");
                }
                addImagem(ref excel, ref pestania);

                using (MemoryStream ms = new MemoryStream())
                {
                    excel.Write(ms);
                    listaBytes = ms.ToArray().ToList();
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"ExcelUtilidades.ObtenerExcelConsolaProyecto => { exception.Message }\\n {exception.InnerException?.Message ?? String.Empty}");
            }

            return listaBytes.ToArray();
        }

        /// <summary>
        ///  Obtiene un arreglo de bytes del excel generado. MensajeNotificación con los datos proporcionados
        ///  como una lista <see cref="List{T}"/> donde <c>T</c> es una instancia de la clase <see cref="UsuarioNotificacionDto"/>
        /// </summary>
        /// <param name="datos">Lista de datos de tipo <see cref="UsuarioNotificacionDto"/></param>
        /// <returns></returns>
        public static byte[] ObtenerExcelNotificaciones(IEnumerable datos)
        {

            var listaBytes = new List<byte>();

            // obtener la lista de propiedades de cada objeto de la lista de objetos actual
            var columnaPropiedades = datos.Cast<Object>().SelectMany(p => /*obtener propiedades*/p.GetType().GetProperties()).Distinct().Select(p => p.Name).ToList();

            var numeroFila = 6;
            try
            {
                IWorkbook excel = new XSSFWorkbook();
                ISheet pestania = excel.CreateSheet("Notificaciones");

                
                addColumnas(ref excel, ref pestania, columnaPropiedades, numeroFila);
                addInfo(ref excel, ref pestania, reporte: "Lista de notificaciones del usuario", fuente: "BACKBONE");

                #region Estilos
                IFont fontCelda = excel.CreateFont();
                fontCelda.IsBold = false;
                fontCelda.FontName = "Calibri";
                fontCelda.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
                ICellStyle estiloCelda = excel.CreateCellStyle();
                estiloCelda.SetFont(fontCelda);
                estiloCelda.BorderBottom = BorderStyle.None;
                estiloCelda.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                estiloCelda.FillPattern = FillPattern.SolidForeground;

                IFont fontCeldaAlternativa = excel.CreateFont();
                fontCeldaAlternativa.IsBold = false;
                fontCeldaAlternativa.FontName = "Calibri";
                fontCeldaAlternativa.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
                ICellStyle estiloCeldaAlternativa = excel.CreateCellStyle();
                estiloCeldaAlternativa.SetFont(fontCeldaAlternativa);
                estiloCeldaAlternativa.BorderBottom = BorderStyle.None;
                estiloCeldaAlternativa.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                estiloCeldaAlternativa.FillPattern = FillPattern.SolidForeground;
                #endregion Estilos

                if (datos == null)
                {

                    IRow row = pestania.CreateRow(numeroFila++);
                    row.CreateCell(0).SetCellValue("Error en la lectura de la información");
                }
                else
                {
                    datos.Cast<Object>().ToList().ForEach(p =>
                    {

                        // obtener las propiedades del objeto actual
                        var propiedades = p.GetType().GetProperties().Select((q, index) => new
                        {
                            Posicion = (index + 1),
                            Propiedad = q.Name,
                            Valor = q.GetValue(p)
                        }).ToList();

                        numeroFila++;

                        IRow fila = pestania.CreateRow(numeroFila);

                        var numeroColumna = 0;

                        propiedades.ForEach(elemento =>
                        {
                            ICell celda = fila.CreateCell(numeroColumna++, ObtenerTipoCelda(elemento.Valor));
                            EstablecerValorCelda(celda, elemento.Valor);

                            celda.CellStyle = (numeroFila % 2 != 0) ? estiloCelda : estiloCeldaAlternativa;
                        }); // fin: columnas-valores
                    }); // fin: datos
                }

                pestania.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, numeroFila, 0, columnaPropiedades.Count - 1));
                pestania.CreateFreezePane(0, 7, 0, 7);

                numeroFila++;
                IRow rowInvalid = pestania.CreateRow(numeroFila);
                var lt = numeroFila + 20;

                while (numeroFila <= lt)
                {
                    numeroFila++;
                    rowInvalid = pestania.CreateRow(numeroFila);
                    rowInvalid.CreateCell(0).SetCellValue(" ");
                    rowInvalid.CreateCell(1).SetCellValue(" ");
                }

                addImagem(ref excel, ref pestania);

                using (MemoryStream ms = new MemoryStream())
                {
                    excel.Write(ms);
                    listaBytes = ms.ToArray().ToList();
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"ExcelUtilidades.ObtenerExcelNotificaciones => { exception.Message }\\n {exception.InnerException?.Message ?? String.Empty}");
            }

            return listaBytes.ToArray();
        }

        public static ByteArrayContent ObtenerExcellTramites(InboxTramite _result, TramiteFiltroDto tramiteFiltroDto)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Tramites");
            var laPrimeraFila = 6;

            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasTramites(ColumnasVisibles);


            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            if (tramiteFiltroDto.FiltroGradeDtos != null)
                addInfo(ref excel, ref sheet, reporte: "Mis Procesos - Trámites", fuente: $"{tramiteFiltroDto.Macroproceso} / {tramiteFiltroDto.FiltroGradeDtos.FirstOrDefault().Valor}");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.ListaGrupoTramiteEntidad == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.ListaGrupoTramiteEntidad)
                {
                    foreach (var entidad in item.GrupoTramites)
                    {
                        foreach (var obj in entidad.ListaTramites)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;

                            if (esColumnaPorNombre(ColumnasVisibles, "sector"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.Numeric);
                                cellInfoA.SetCellValue(obj.NombreSector);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "entidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "numeroTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NumeroTramite);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "tipoTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreTipoTramite);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "descripcion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.Descripcion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreFlujo);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "estadoTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.DescEstado);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "fecha"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacionTramite.HasValue ? obj.FechaCreacionTramite.Value.ToString("dd-MM-yyyy hh:MM:ss") : String.Empty);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreAccion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "fechaPaso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.ToString("dd-MM-yyyy hh:MM:ss"));
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            sheet.AutoSizeColumn(num);
                            num++;
                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            addImagem(ref excel, ref sheet);

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellTramitesConsola(InboxTramite _result, TramiteFiltroDto tramiteFiltroDto)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Tramites");
            var laPrimeraFila = 6;

            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasTramitesConsola(ColumnasVisibles);


            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            if (tramiteFiltroDto.FiltroGradeDtos != null)
                addInfo(ref excel, ref sheet, reporte: "Trámites", fuente: $"{tramiteFiltroDto.FiltroGradeDtos.FirstOrDefault().Valor}");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.ListaGrupoTramiteEntidad == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.ListaGrupoTramiteEntidad)
                {
                    foreach (var entidad in item.GrupoTramites)
                    {
                        foreach (var obj in entidad.ListaTramites)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;

                            if (esColumnaPorNombre(ColumnasVisibles, "sector"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.Numeric);
                                cellInfoA.SetCellValue(obj.NombreSector);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "entidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "numeroTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NumeroTramite);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "nombreFlujo"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreFlujo);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "tipoTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreTipoTramite);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "estadoTramite"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.DescEstado);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "fecha"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.ToString("dd-MM-yyyy hh:MM:ss"));
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "nombreAccion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreAccion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "fechaPaso"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.ToString("dd-MM-yyyy hh:MM:ss"));
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            sheet.AutoSizeColumn(num);
                            num++;
                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            addImagem(ref excel, ref sheet);

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellTramitesProyectos(ProyectosTramitesDTO _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
                "Tipo Proyecto",
               "Sector",
               "Entidad",
               "Proyecto/BPIN",
               "Operación",
               "Valor"
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Proyectos del Tramites por entidad y sectores", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            if (_result.ListaProyectos == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var obj in _result.ListaProyectos)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;

                    ICell cellInfoA = row.CreateCell(j++, CellType.String);
                    cellInfoA.SetCellValue(obj.TipoProyecto);
                    if (num % 2 != 0)
                        cellInfoA.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoA.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoB = row.CreateCell(j++, CellType.String);
                    cellInfoB.SetCellValue(obj.SectorNombre);
                    if (num % 2 != 0)
                        cellInfoB.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoB.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoC = row.CreateCell(j++, CellType.String);
                    cellInfoC.SetCellValue(obj.NombreEntidad);
                    if (num % 2 != 0)
                        cellInfoC.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoC.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoD = row.CreateCell(j++, CellType.String);
                    cellInfoD.SetCellValue($"{obj.NombreObjetoNegocio}-{obj.IdObjetoNegocio}");
                    if (num % 2 != 0)
                        cellInfoD.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoD.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoE = row.CreateCell(j++, CellType.String);
                    cellInfoE.SetCellValue(obj.Operacion);
                    if (num % 2 != 0)
                        cellInfoE.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoE.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoF = row.CreateCell(j++, CellType.Numeric);
                    cellInfoF.SetCellValue((double)obj.ValorTotal.GetValueOrDefault());
                    if (num % 2 != 0)
                        cellInfoF.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoF.CellStyle = boldStyleValueLineaDos;



                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellPerfiles(ExcelPerfilDto _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
               "Perfil",
               "Roles",
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información del perfil", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            if (_result.ListaPerfiles == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var obj in _result.ListaPerfiles)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;

                    ICell cellInfoA = row.CreateCell(j++, CellType.String);
                    cellInfoA.SetCellValue(obj.NombrePerfil);
                    if (num % 2 != 0)
                        cellInfoA.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoA.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoB = row.CreateCell(j++, CellType.String);
                    cellInfoB.SetCellValue(obj.RolesConcat);
                    if (num % 2 != 0)
                        cellInfoB.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoB.CellStyle = boldStyleValueLineaDos;

                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellConsolaMonitoreoProyectos(ProyectoResumenDto _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasMonitoreo(ColumnasVisibles);

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Consola de Monitoreo de Proyectos", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            if (_result == null || _result.GruposEntidades.Count == 0)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue("No hubo registros para los filtros seleccionados.");
            }
            else
            {
                var num = 1;
                foreach (var item in _result.GruposEntidades)
                {
                    foreach (var entidad in item.ListaEntidades)
                    {
                        foreach (var obj in entidad.ObjetosNegocio)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;

                            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoNombre"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.CodigoBpin + " - " + obj.ProyectoNombre);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.EstadoProyecto);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "AvanceFinanciero"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.AvanceFinanciero);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "AvanceFisico"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.AvanceFisico);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "AvanceProyecto"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.AvanceProyecto);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "Duracion"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.Duracion);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "PeriodoEjecucion"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.PeriodoEjecucion);
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                            {
                                CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, obj.SectorNombre);
                            }

                            num++;
                        }
                    }
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        private static void CrearColumna(IRow row, ref int num, ref int j, ICellStyle cellStyle1, ICellStyle cellStyle2, string valor)
        {
            ICell cellInfo = row.CreateCell(j++, CellType.String);
            cellInfo.SetCellValue(valor);
            if (num % 2 != 0)
                cellInfo.CellStyle = cellStyle1;
            else
                cellInfo.CellStyle = cellStyle2;
        }

        public static ByteArrayContent ObtenerExcellComum(ExcelDto objExcel)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, objExcel.Columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: objExcel.Reporte, fuente: "BACKBONE");

            #region Estilo
            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;
            #endregion Estilo

            if (objExcel.Data == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(objExcel.Mensaje);
            }
            else
            {
                var num = 1;


                foreach (var obj in objExcel.Data)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;


                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        //string propName = prop.Name; Nombre de la columna
                        //object value = prop.GetValue(obj, null); valor de la columna
                        //object value2 = prop.PropertyType.Name; tipo de la columna

                        var value = prop.GetValue(obj, null) ?? "";

                        ICell cellInfo = row.CreateCell(j++, CellType.String);
                        cellInfo.SetCellValue(value.ToString());
                        cellInfo.CellStyle = num % 2 != 0 ? boldStyleValueLineaUno : cellInfo.CellStyle = boldStyleValueLineaDos;
                    }
                    //sheet.AutoSizeColumn(num);
                    //GC.Collect();

                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, objExcel.Data.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);
            //for (int i = 1; i <= objExcel.Columnas.Count(); i++)
            //{
            //    sheet.AutoSizeColumn(i);

            //    GC.Collect(); // Add this line
            //}

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellInflexibilidad(ExcelDto objExcel)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, objExcel.Columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: objExcel.Reporte, fuente: "BACKBONE");

            #region Estilo
            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;
            #endregion Estilo

            if (objExcel.Data == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(objExcel.Mensaje);
            }
            else
            {
                var num = 1;

                if (objExcel.ColumnasHeader.Contains("Periodo"))
                    objExcel.ColumnasHeader.Add("PeriodoExcel");

                foreach (var obj in objExcel.Data)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;


                    foreach (var prop in obj.GetType().GetProperties())
                    {

                        if (objExcel.ColumnasHeader.Contains(prop.Name))
                        {
                            var value = prop.GetValue(obj, null) ?? "";

                            ICell cellInfo = row.CreateCell(j++, CellType.String);
                            cellInfo.SetCellValue(value.ToString());
                            cellInfo.CellStyle = num % 2 != 0 ? boldStyleValueLineaUno : cellInfo.CellStyle = boldStyleValueLineaDos;

                        }
                    }
                    //sheet.AutoSizeColumn(num);
                    //GC.Collect();

                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, objExcel.Data.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);
            //for (int i = 1; i <= objExcel.Columnas.Count(); i++)
            //{
            //    sheet.AutoSizeColumn(i);

            //    GC.Collect(); // Add this line
            //}

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        private static void addImagem(ref IWorkbook excel, ref ISheet sheet)
        {
            //image
            //image
            var path = $"{AppDomain.CurrentDomain.BaseDirectory.ToString().Replace(@"\", @"/")}Content/Img/header_logos_2022.png";
            byte[] data = File.ReadAllBytes(path);


            int pictureIndex = excel.AddPicture(data, PictureType.PNG);


            ICreationHelper helper = excel.GetCreationHelper();
            IDrawing drawing = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor = helper.CreateClientAnchor();
            anchor.Col1 = 1;//0 index based column
            anchor.Row1 = 1;//0 index based row
            anchor.AnchorType = AnchorType.MoveDontResize;
            IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
            picture.Resize();

        }

        private static void addColumnas(ref IWorkbook excel, ref ISheet sheet, List<string> columnas, int laPrimeraFila)
        {
            //styling
            IFont boldFont = excel.CreateFont();
            boldFont.IsBold = true;
            boldFont.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyle = excel.CreateCellStyle();
            boldStyle.SetFont(boldFont);
            boldStyle.BorderBottom = BorderStyle.Medium;
            boldStyle.BorderTop = BorderStyle.Medium;
            boldStyle.BorderRight = BorderStyle.Medium;
            boldStyle.BorderLeft = BorderStyle.Medium;
            boldStyle.BorderDiagonalColor = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;

            int i = 0;
            IRow hRow = sheet.CreateRow(laPrimeraFila);
            foreach (var item in columnas)
            {
                ICell cell = hRow.CreateCell(i);
                string nombreColumna = string.Empty;
                nombreColumna = item;

                if (item.Equals("Notificacion"))
                    nombreColumna = "Notificación";
                else if(item.Equals("FechaCadena"))
                    nombreColumna = "Fecha";
                else if (item.Equals("Estado"))
                    nombreColumna = "Estado";

                cell.SetCellValue(nombreColumna);
                cell.CellStyle = boldStyle;
                i++;
            }
        }

        private static void addInfo(ref IWorkbook excel, ref ISheet sheet, string reporte, string fuente)
        {
            //styling
            IFont boldFontInfo = excel.CreateFont();
            boldFontInfo.IsBold = false;
            boldFontInfo.FontName = "Calibri";
            boldFontInfo.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleInfo = excel.CreateCellStyle();
            boldStyleInfo.SetFont(boldFontInfo);
            boldStyleInfo.BorderBottom = BorderStyle.None;

            //styling
            IFont boldFontValue = excel.CreateFont();
            boldFontValue.IsBold = true;
            boldFontValue.FontName = "Calibri";
            boldFontValue.Color = NPOI.HSSF.Util.HSSFColor.RoyalBlue.Index;
            ICellStyle boldStyleValue = excel.CreateCellStyle();
            boldStyleValue.SetFont(boldFontValue);
            boldStyleValue.BorderBottom = BorderStyle.None;

            IRow hRowInfoA = sheet.CreateRow(1);
            ICell cellInfoR = hRowInfoA.CreateCell(3);
            cellInfoR.SetCellValue("REPORTE");
            cellInfoR.CellStyle = boldStyleInfo;
            ICell cellInfoV = hRowInfoA.CreateCell(4);
            cellInfoV.SetCellValue(reporte);
            cellInfoV.CellStyle = boldStyleValue;

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            DateTime serverTime = DateTime.Now;
            DateTimeOffset colombiaTime = TimeZoneInfo.ConvertTime(serverTime, timeZone);


            IRow hRowInfoB = sheet.CreateRow(2);
            ICell cellInfoBR = hRowInfoB.CreateCell(3);
            cellInfoBR.SetCellValue("FECHA");
            cellInfoBR.CellStyle = boldStyleInfo;
            ICell cellInfoBV = hRowInfoB.CreateCell(4);
            cellInfoBV.SetCellValue(colombiaTime.ToString("dd/MM/yyyy HH:mm:ss"));
            cellInfoBV.CellStyle = boldStyleValue;

            IRow hRowInfoC = sheet.CreateRow(3);
            ICell cellInfoCR = hRowInfoC.CreateCell(3);
            cellInfoCR.SetCellValue("FUENTE");
            cellInfoCR.CellStyle = boldStyleInfo;
            ICell cellInfoCV = hRowInfoC.CreateCell(4);
            cellInfoCV.SetCellValue(fuente);
            cellInfoCV.CellStyle = boldStyleValue;
        }

        private static List<string> columnasProyectos(string[] ColumnasVisibles)
        {
            var columnas = new List<string>();

            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                columnas.Add("Sector");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreEntidad"))
                columnas.Add("Entidad");
            //proyecto
            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoId"))
                columnas.Add("ID");
            if (esColumnaPorNombre(ColumnasVisibles, "IdObjetoNegocio"))
                columnas.Add("BPIN");
            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                columnas.Add("Estado del Proyecto");

            if (esColumnaPorNombre(ColumnasVisibles, "NombreObjetoNegocio"))
                columnas.Add("Nombre del proyecto");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                columnas.Add("Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "CodigoProceso"))
                columnas.Add("Codigo Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "EstadoInstancia"))
                columnas.Add("Estado Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "FechaCreacion"))
                columnas.Add("Fecha de Inicio del Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                columnas.Add("Paso actual");
            if (esColumnaPorNombre(ColumnasVisibles, "FechaPaso"))
                columnas.Add("Fecha de Inicio del Paso");
            return columnas;
        }

        private static List<string> columnasProyectosConsola(string[] ColumnasVisibles)
        {
            var columnas = new List<string>();

            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                columnas.Add("Sector");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreEntidad"))
                columnas.Add("Entidad");
            //proyecto
            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoId"))
                columnas.Add("ID");
            if (esColumnaPorNombre(ColumnasVisibles, "IdObjetoNegocio"))
                columnas.Add("BPIN");
            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                columnas.Add("Estado del Proyecto");

            if (esColumnaPorNombre(ColumnasVisibles, "NombreObjetoNegocio"))
                columnas.Add("Nombre del proyecto");
            if (esColumnaPorNombre(ColumnasVisibles, "Macroproceso"))
                columnas.Add("Macroproceso");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                columnas.Add("Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "CodigoProceso"))
                columnas.Add("Codigo Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "EstadoInstancia"))
                columnas.Add("Estado Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "FechaCreacion"))
                columnas.Add("Fecha de Inicio del Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                columnas.Add("Paso actual");
            if (esColumnaPorNombre(ColumnasVisibles, "FechaPaso"))
                columnas.Add("Fecha de Inicio del Paso");
            return columnas;
        }

        private static List<string> columnasTramites(string[] ColumnasVisibles)
        {
            var columnas = new List<string>();

            if (esColumnaPorNombre(ColumnasVisibles, "sector"))
                columnas.Add("Sector");
            if (esColumnaPorNombre(ColumnasVisibles, "entidad"))
                columnas.Add("Entidad");
            if (esColumnaPorNombre(ColumnasVisibles, "numeroTramite"))
                columnas.Add("Código");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreFlujo"))
                columnas.Add("Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "tipoTramite"))
                columnas.Add("Tipo");
            if (esColumnaPorNombre(ColumnasVisibles, "estadoTramite"))
                columnas.Add("Estado de proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "fecha"))
                columnas.Add("Fecha de inicio del proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "NombreAccion"))
                columnas.Add("Paso actual");
            if (esColumnaPorNombre(ColumnasVisibles, "fechaPaso"))
                columnas.Add("Fecha de inicio del paso");

            return columnas;
        }

        private static List<string> columnasTramitesConsola(string[] ColumnasVisibles)
        {
            var columnas = new List<string>();

            if (esColumnaPorNombre(ColumnasVisibles, "sector"))
                columnas.Add("Sector");
            if (esColumnaPorNombre(ColumnasVisibles, "entidad"))
                columnas.Add("Entidad");
            if (esColumnaPorNombre(ColumnasVisibles, "numeroTramite"))
                columnas.Add("Código");
            if (esColumnaPorNombre(ColumnasVisibles, "nombreFlujo"))
                columnas.Add("Proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "tipoTramite"))
                columnas.Add("Tipo");
            if (esColumnaPorNombre(ColumnasVisibles, "estadoTramite"))
                columnas.Add("Estado de proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "fecha"))
                columnas.Add("Fecha de inicio del proceso");
            if (esColumnaPorNombre(ColumnasVisibles, "nombreAccion"))
                columnas.Add("Paso actual");
            if (esColumnaPorNombre(ColumnasVisibles, "fechaPaso"))
                columnas.Add("Fecha de inicio del paso");

            return columnas;
        }

        private static List<string> columnasMonitoreo(string[] ColumnasVisibles)
        {
            var columnas = new List<string>();

            if (esColumnaPorNombre(ColumnasVisibles, "ProyectoNombre"))
                columnas.Add("Proyecto/BPIN");
            if (esColumnaPorNombre(ColumnasVisibles, "EstadoProyecto"))
                columnas.Add("Estado");
            if (esColumnaPorNombre(ColumnasVisibles, "AvanceFinanciero"))
                columnas.Add("Avance Financiero");
            if (esColumnaPorNombre(ColumnasVisibles, "AvanceFisico"))
                columnas.Add("Avance Fisico");
            if (esColumnaPorNombre(ColumnasVisibles, "AvanceProyecto"))
                columnas.Add("Avance Proyecto");
            if (esColumnaPorNombre(ColumnasVisibles, "Duracion"))
                columnas.Add("Duracion");
            if (esColumnaPorNombre(ColumnasVisibles, "PeriodoEjecucion"))
                columnas.Add("Periodo Ejecucion");
            if (esColumnaPorNombre(ColumnasVisibles, "SectorNombre"))
                columnas.Add("Sector");

            return columnas;
        }

        private static bool esColumnaPorNombre(string[] ColumnasVisibles, string columna)
        {
            columna = columna.ToLower();
            return (from c in ColumnasVisibles
                    where c.ToLower().Equals(columna)
                    select true).FirstOrDefault();
        }

        public static ByteArrayContent ObtenerExcellConsolaAlertaConfig(List<AlertasConfigDto> _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
                "Nombre De La Alerta",
                "Tipo",
                "Mensaje de La alerta",
                "Estado"
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Consola de Configuración del Alerta", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            if (_result == null || _result.Count == 0)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue("No hubo registros para los filtros seleccionados.");
            }
            else
            {
                var num = 1;
                foreach (var obj in _result)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;

                    ICell cellInfoA = row.CreateCell(j++, CellType.String);
                    cellInfoA.SetCellValue($"{obj.NombreAlerta}");
                    if (num % 2 != 0)
                        cellInfoA.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoA.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoB = row.CreateCell(j++, CellType.String);
                    cellInfoB.SetCellValue(obj.TipoAlertaDescripcion);
                    if (num % 2 != 0)
                        cellInfoB.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoB.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoC = row.CreateCell(j++, CellType.String);
                    cellInfoC.SetCellValue(obj.MensajeAlerta);
                    if (num % 2 != 0)
                        cellInfoC.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoC.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoD = row.CreateCell(j++, CellType.String);
                    cellInfoD.SetCellValue(obj.EstadoDescripcion);
                    if (num % 2 != 0)
                        cellInfoD.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoD.CellStyle = boldStyleValueLineaDos;

                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }


        public static ByteArrayContent ObtenerExcellConsolaTramites(ConsolaTramiteDto _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Tramites");
            var laPrimeraFila = 6;

            var ColumnasVisibles = _result.ColumnasVisibles;
            var columnas = columnasTramites(ColumnasVisibles);

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre trámites por entidad y sectores", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;


            if (_result.ListaGrupoTramiteEntidad == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var item in _result.ListaGrupoTramiteEntidad)
                {
                    foreach (var entidad in item.GrupoTramites)
                    {
                        foreach (var obj in entidad.ListaTramites)
                        {
                            laPrimeraFila++;
                            IRow row = sheet.CreateRow(laPrimeraFila);
                            int j = 0;
                            if (esColumnaPorNombre(ColumnasVisibles, "descripcion"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.Descripcion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "fecha"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.FechaCreacion.ToString());
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "entidad"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreEntidad);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }
                            if (esColumnaPorNombre(ColumnasVisibles, "accionFlujo"))
                            {
                                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                                cellInfoA.SetCellValue(obj.NombreTipoTramite + "/" + obj.NombreAccion);
                                if (num % 2 != 0)
                                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                                else
                                    cellInfoA.CellStyle = boldStyleValueLineaDos;
                            }

                        }
                    }
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            //for (int i = 0; i < columnas.Count; i++)
            //    sheet.AutoSizeColumn(i);

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellConsolaTramitesProyectos(ProyectosTramitesDTO _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
                "Tipo Proyecto",
               "Sector",
               "Entidad",
               "Proyecto/BPIN",
               "Operación",
               "Valor"
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Proyectos del Tramites por entidad y sectores", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            if (_result.ListaProyectos == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue(_result.Mensaje);
            }
            else
            {
                var num = 1;
                foreach (var obj in _result.ListaProyectos)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;

                    ICell cellInfoA = row.CreateCell(j++, CellType.String);
                    cellInfoA.SetCellValue(obj.TipoProyecto);
                    if (num % 2 != 0)
                        cellInfoA.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoA.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoB = row.CreateCell(j++, CellType.String);
                    cellInfoB.SetCellValue(obj.SectorNombre);
                    if (num % 2 != 0)
                        cellInfoB.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoB.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoC = row.CreateCell(j++, CellType.String);
                    cellInfoC.SetCellValue(obj.NombreEntidad);
                    if (num % 2 != 0)
                        cellInfoC.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoC.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoD = row.CreateCell(j++, CellType.String);
                    cellInfoD.SetCellValue($"{obj.NombreObjetoNegocio}-{obj.IdObjetoNegocio}");
                    if (num % 2 != 0)
                        cellInfoD.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoD.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoE = row.CreateCell(j++, CellType.String);
                    cellInfoE.SetCellValue(obj.Operacion);
                    if (num % 2 != 0)
                        cellInfoE.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoE.CellStyle = boldStyleValueLineaDos;

                    ICell cellInfoF = row.CreateCell(j++, CellType.Numeric);
                    cellInfoF.SetCellValue((double)obj.ValorTotal.GetValueOrDefault());
                    if (num % 2 != 0)
                        cellInfoF.CellStyle = boldStyleValueLineaUno;
                    else
                        cellInfoF.CellStyle = boldStyleValueLineaDos;



                    num++;
                }
            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellLogInstancia(IList<LogsInstanciasDto> _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
               "Codigo",
               "Fecha",
               "Entidad",
               "BPIN",
               "Descripción",
               "Estado",
               "Usuario"
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Logs de instancias.", fuente: "BACKBONE");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            var num = 1;
            foreach (var obj in _result)
            {
                laPrimeraFila++;
                IRow row = sheet.CreateRow(laPrimeraFila);
                int j = 0;

                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                cellInfoA.SetCellValue(obj.Id.ToString());
                if (num % 2 != 0)
                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoA.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoB = row.CreateCell(j++, CellType.String);
                cellInfoB.SetCellValue(obj.Fecha.ToString());
                if (num % 2 != 0)
                    cellInfoB.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoB.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoC = row.CreateCell(j++, CellType.String);
                cellInfoC.SetCellValue(string.IsNullOrEmpty(obj.NombreEntidad) ? " _ " : obj.NombreEntidad);
                if (num % 2 != 0)
                    cellInfoC.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoC.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoD = row.CreateCell(j++, CellType.String);
                cellInfoD.SetCellValue(obj.BPIN);
                if (num % 2 != 0)
                    cellInfoD.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoD.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoE = row.CreateCell(j++, CellType.String);
                cellInfoE.SetCellValue(obj.Descripcion);
                if (num % 2 != 0)
                    cellInfoE.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoE.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoF = row.CreateCell(j++, CellType.String);
                cellInfoF.SetCellValue(obj.Estado == null ? " _ " : obj.Estado.ToString());
                if (num % 2 != 0)
                    cellInfoF.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoF.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoG = row.CreateCell(j++, CellType.String);
                cellInfoG.SetCellValue(string.IsNullOrEmpty(obj.NombreUsuario) ? " _ " : obj.NombreUsuario);
                if (num % 2 != 0)
                    cellInfoG.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoG.CellStyle = boldStyleValueLineaDos;

                num++;
            }

            sheet.SetAutoFilter(new CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);


            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }
        internal static ByteArrayContent ObtenerExcellUsuarios(List<UsuarioReportesDto> result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Usuarios");

            var columnas = columnasUsuarios();

            var laPrimeraFila = 8;
            //addImagem(ref excel, ref sheet);

            var path = $"{AppDomain.CurrentDomain.BaseDirectory.ToString().Replace(@"\", @"/")}Content/Img/header_logos_2022.png";
            byte[] data = File.ReadAllBytes(path);


            int pictureIndex = excel.AddPicture(data, PictureType.PNG);
            ICreationHelper helper = excel.GetCreationHelper();
            IDrawing drawing = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor = helper.CreateClientAnchor();
            anchor.Col1 = 1;//0 index based column
            anchor.Row1 = 1;//0 index based row
            anchor.AnchorType = AnchorType.MoveAndResize;
            IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
            picture.Resize(2, 2);

            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre usuarios", fuente: "PIIP");

            //styling
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground; //FillPatternType.SOLID_FOREGROUND;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            if (result == null)
            {
                IRow row = sheet.CreateRow(laPrimeraFila++);
                row.CreateCell(0).SetCellValue("No hubo registros para los filtros seleccionados.");
            }
            else
            {
                var num = 1;
                foreach (var usuario in result)
                {
                    laPrimeraFila++;
                    IRow row = sheet.CreateRow(laPrimeraFila);
                    int j = 0;

                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.AgrupadorNombreEntidad);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.NombreEntidad);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.TipoIdentificacion);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.Identificacion);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.NombreUsuario);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.Correo);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.Perfil);
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.Activo == "true" ? "Activo" : "Inactivo");
                    CrearColumna(row, ref num, ref j, boldStyleValueLineaUno, boldStyleValueLineaDos, usuario.ActivoUsuarioPerfil == "true" ? "Activo" : "Inactivo");

                    num++;
                }

            }

            sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(8, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);
            sheet.SetColumnWidth(0, 5000);
            sheet.SetColumnWidth(1, 10000);
            sheet.SetColumnWidth(2, 4000);
            sheet.SetColumnWidth(3, 10000);
            sheet.SetColumnWidth(4, 10000);
            sheet.SetColumnWidth(5, 10000);
            sheet.SetColumnWidth(6, 10000);
            sheet.SetColumnWidth(7, 10000);
            sheet.SetColumnWidth(8, 2000);

            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 20;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        public static ByteArrayContent ObtenerExcellNotificaciones(IList<UsuarioNotificacionConfigDto> _result)
        {
            IWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet();

            var columnas = new List<string>()
            {
               "Nombre",
               "Fecha Inicio",
               "Fecha Final",
               "Tipo"
            };

            var laPrimeraFila = 6;

            addImagem(ref excel, ref sheet);
            addColumnas(ref excel, ref sheet, columnas, laPrimeraFila);
            addInfo(ref excel, ref sheet, reporte: "Información sobre Notificaciones.", fuente: "BACKBONE");

            //styling   
            IFont boldFontLineaUno = excel.CreateFont();
            boldFontLineaUno.IsBold = false;
            boldFontLineaUno.FontName = "Calibri";
            boldFontLineaUno.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaUno = excel.CreateCellStyle();
            boldStyleValueLineaUno.SetFont(boldFontLineaUno);
            boldStyleValueLineaUno.BorderBottom = BorderStyle.None;
            boldStyleValueLineaUno.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            boldStyleValueLineaUno.FillPattern = FillPattern.SolidForeground;

            //styling
            IFont boldFontValueLineaDos = excel.CreateFont();
            boldFontValueLineaDos.IsBold = false;
            boldFontValueLineaDos.FontName = "Calibri";
            boldFontValueLineaDos.Color = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
            ICellStyle boldStyleValueLineaDos = excel.CreateCellStyle();
            boldStyleValueLineaDos.SetFont(boldFontValueLineaDos);
            boldStyleValueLineaDos.BorderBottom = BorderStyle.None;
            boldStyleValueLineaDos.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            boldStyleValueLineaDos.FillPattern = FillPattern.SolidForeground;

            var num = 1;
            foreach (var obj in _result)
            {
                laPrimeraFila++;
                IRow row = sheet.CreateRow(laPrimeraFila);
                int j = 0;

                ICell cellInfoA = row.CreateCell(j++, CellType.String);
                cellInfoA.SetCellValue(obj.NombreNotificacion.ToString());
                if (num % 2 != 0)
                    cellInfoA.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoA.CellStyle = boldStyleValueLineaDos;

                ICell cellInfoB = row.CreateCell(j++, CellType.String);
                cellInfoB.SetCellValue(obj.FechaInicio.ToString());
                if (num % 2 != 0)
                    cellInfoB.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoB.CellStyle = boldStyleValueLineaDos;


                ICell cellInfoC = row.CreateCell(j++, CellType.String);
                cellInfoC.SetCellValue(obj.FechaFin.ToString());
                if (num % 2 != 0)
                    cellInfoC.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoC.CellStyle = boldStyleValueLineaDos;


                ICell cellInfoD = row.CreateCell(j++, CellType.String);
                cellInfoD.SetCellValue(obj.NombreTipo);
                if (num % 2 != 0)
                    cellInfoD.CellStyle = boldStyleValueLineaUno;
                else
                    cellInfoD.CellStyle = boldStyleValueLineaDos;

                num++;
            }

            sheet.SetAutoFilter(new CellRangeAddress(6, laPrimeraFila, 0, columnas.Count - 1));
            sheet.CreateFreezePane(0, 7, 0, 7);


            //Resuelve el problema: Excel encontró contenido ilegible / archivo no válido o corrupto (contenido ilegible)
            laPrimeraFila++;
            IRow rowInvalid = sheet.CreateRow(laPrimeraFila);
            var lt = laPrimeraFila + 100;
            while (laPrimeraFila <= lt)
            {
                laPrimeraFila++;
                rowInvalid = sheet.CreateRow(laPrimeraFila);
                rowInvalid.CreateCell(0).SetCellValue(" ");
                rowInvalid.CreateCell(1).SetCellValue(" ");
            }


            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                return new ByteArrayContent(ms.ToArray());
            }
        }

        private static List<string> columnasUsuarios()
        {
            var columnas = new List<string>();
            columnas.Add("Sector");
            columnas.Add("Entidad");
            columnas.Add("Tipo Identificación");
            columnas.Add("Numero Identificación");
            columnas.Add("Usuario");
            columnas.Add("Correo");
            columnas.Add("Perfil");
            columnas.Add("Estado Usuario");
            columnas.Add("Estado Perfil");

            return columnas;
        }


        /// <summary>
        ///     Establece el valor proporcionado a la celda proporcionada de acuerdo al tipo de dato de la celda
        /// </summary>
        /// <param name="celda">Celda actual</param>
        /// <param name="valor">Valor a establecer a la celda actual</param>
        private static void EstablecerValorCelda(ICell celda, object valor)
        {

            if (valor != null)
            {

                switch (celda.CellType)
                {

                    case CellType.Numeric: celda.SetCellValue(valor.GetType() == typeof(int) ? (int)valor : (double)valor); break;
                    case CellType.String: celda.SetCellValue((String)valor); break;
                    case CellType.Boolean: celda.SetCellValue((bool)valor); break;
                    default: celda.SetCellValue(valor.ToString());break;
                }
            }
        }

        /// <summary>
        ///  Obtiene el tipo de dato de la celda de acuerdo al tipo de dato del valor a establecer
        /// </summary>
        /// <param name="valor">Valor actual</param>
        /// <returns></returns>
        private static CellType ObtenerTipoCelda(object valor)
        {

            var tipoCelda = CellType.Blank;
            var tipoValor = new Dictionary<System.Type, CellType> {
                { typeof(int) , CellType.Numeric  },
                { typeof(double), CellType.Numeric },
                { typeof(decimal), CellType.Numeric },
                { typeof(String), CellType.String },
            };

            if (valor != null)
            {
                tipoCelda = tipoValor[valor.GetType()];
            }

            return tipoCelda;
        }
    }
}
