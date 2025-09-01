using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Genericos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class RespuestaGeneralDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public string IdRegistro { get; set; }
        public List<object> Registros { get; set; }
    }
}
