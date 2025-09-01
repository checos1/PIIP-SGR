using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.Viabilidad
{
    [ExcludeFromCodeCoverage]

    public class LstAcuerdoSectorClasificadorDto
    {
        public List<Acuerdos> Acuerdos { get; set; }
        public List<Sectores> Sectores { get; set; }
        public List<Clasificadores> Clasificadores { get; set; }
        public List<AcuerdoProyecto> AcuerdoProyecto { get; set; }
        
    }
    public class Acuerdos
    {
        public int AcuerdoNivelId { get; set; }
        public int AcuerdoId { get; set; }
        public string NombreAcuerdo { get; set; }
    }

    public class Sectores
    {
        public int AcuerdoId { get; set; }
        public int AcuerdoSectorId { get; set; }
        public string NombreSector { get; set; }
    }

    public class Clasificadores
    {
        public int AcuerdoSectorId { get; set; }
        public int AcuerdoSectorClasificadorId { get; set; }
        public string NombreClasificador { get; set; }
    }


    public class AcuerdoProyecto
    {
        public int Id { get; set; }
        public int AcuerdoNivelId { get; set; }
        public int AcuerdoId { get; set; }
        public string NombreAcuerdo { get; set; }
        public int AcuerdoSectorId { get; set; }
        public string NombreSector { get; set; }
        public int AcuerdoSectorClasificadorId { get; set; }
        public string NombreClasificador { get; set; }
    }

    

}