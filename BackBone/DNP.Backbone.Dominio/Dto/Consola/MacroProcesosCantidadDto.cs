using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Consola
{
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MacroProcesosCantidadDto
    {
        public Guid NivelPadreId { get; set; }
        public string TipoObjeto { get; set; }
        public int Cantidad { get; set; }
    }
}
