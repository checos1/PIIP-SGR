using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class PlanNacionalDesarrolloDto
    {
        public int? IdSector { get; set; }
        public string Sector { get; set; }
        public string CodigoPrograma { get; set; }
        public string Programa { get; set; }
        public string CodigoSubPrograma { get; set; }
        public string NombreSubPrograma { get; set; }
        public string PND { get; set; }
        public string PND2 { get; set; }
        public string Nivel1 { get; set; }
        public string Nivel1Nombre { get; set; }
        public string Nivel1Code { get; set; }
        public string Nivel2 { get; set; }
        public string Nivel2Nombre { get; set; }
        public string Nivel2Code { get; set; }
        public string Nivel3 { get; set; }
        public string Nivel3Nombre { get; set; }
        public string Nivel3Code { get; set; }
        public int IdProyecto { get; set; }
        public string Bpin { get; set; }
        public string NombreProyecto { get; set; }
    }
}
