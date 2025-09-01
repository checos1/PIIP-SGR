using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProgramacionFuenteDto
    {
        public int TramiteProyectoId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public List<ValoresFuente> ValoresFuente { get; set; }
        public List<ValoresCredito> ValoresCredito { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ValoresFuente
    {
        public int FuenteId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ValoresCredito
    {
        public int CreditoId { get; set; }
        public int FuenteId { get; set; }
        public decimal? NacionCSF { get; set; }
        public decimal? NacionSSF { get; set; }
        public decimal? Propios { get; set; }
    }
}
