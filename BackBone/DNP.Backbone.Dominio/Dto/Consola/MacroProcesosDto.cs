using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Consola
{
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MacroProcesosDto
    {
        public Guid NivelId { get; set; }
        public string Nombre { get; set; }
        public string Archivo { get; set; }
        public string Descripcion { get; set; }
        public Guid? NivelPadreId { get; set; }
    }
}
