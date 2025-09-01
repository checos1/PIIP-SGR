using DNP.Autorizacion.Dominio.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    public class ExcelDto
    {
        public string Mensaje { get; set; }
        public string Reporte { get; set; }
        public int Codigo { get; set; }
        public List<string> Columnas { get; set; }
        public List<string> ColumnasHeader { get; set; }
        public dynamic Data { get; set; }

        public ExcelDto()
        {
            Columnas = new List<string>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class ExcelPerfilDto
    {
        public string Mensaje { get; set; }
        public string Reporte { get; set; }
        public int Codigo { get; set; }
        public List<string> Columnas { get; set; }
        public List<PerfilDto> ListaPerfiles { get; set; }

        public ExcelPerfilDto()
        {
            ListaPerfiles = new List<PerfilDto>();
            Columnas = new List<string>();
        }
    }

}
