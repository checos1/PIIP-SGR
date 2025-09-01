using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Programacion
{
    public class CargueSaldoDto
    {
        public int? Id { get; set; }
        public string Codigo { get; set; }
        public string NombreUnidadEjecutora { get; set; }
        public string TipoGasto { get; set; }
        public int CodigoPrograma { get; set; }
        public string CodigoSubprograma { get; set; }        
        public int Ord_SubP_Gasto { get; set; }
        public decimal ValorProyecto { get; set; }
        public int Fuente { get; set; }
        public int TipoRecursos { get; set; }
        public string SituacionFondos { get; set; }
        public string Rubro { get; set; }
    }
}

