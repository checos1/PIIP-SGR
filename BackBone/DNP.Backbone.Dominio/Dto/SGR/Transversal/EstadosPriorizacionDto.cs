using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.Transversal
{
    [ExcludeFromCodeCoverage]

    public class EstadosPriorizacionDto
    {
        public List<Fuentes> Fuentes { get; set; }
        public List<Metodologias> Metodologias{ get; set; }
        
        
    }
    public class Fuentes
    {
        public int EtapaId { get; set; }
        public string Etapa { get; set; }
        public int FuenteId { get; set; }
        public string TipoEntidad { get; set; }
        public string ETFuente { get; set; }
        public string ETPadre { get; set; }
        public int ResourceTypeId { get; set; }
        
    }

    public class Metodologias
    {
        public int EtapaId { get; set; }
        public int FuenteId { get; set; }
        public int ResourceTypeId { get; set; }
        public string TipoRecurso { get; set; }
        public string Metodologia { get; set; }
        public string EntidadPrioriza { get; set; }
        public string SubEstado { get; set; }
        public decimal Valor { get; set; }
    }
    

}