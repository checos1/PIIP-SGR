using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    [ExcludeFromCodeCoverage]
    public class ProyectoCreditoDto
    {
        public string Sector { get; set; }
        public string NombreEntidad { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int IdProyecto { get; set; }
        public int IdEntidad { get; set; }
        public bool? GruposPermitidos { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public string MarcaTraslado { get; set; }
    }
}
