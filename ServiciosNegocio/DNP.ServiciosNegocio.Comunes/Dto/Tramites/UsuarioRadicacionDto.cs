using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class  UsuarioRadicacionDto
    {
        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Documento { get; set; }

        public int CodigoDependencia { get; set; }

        public int Codigo { get; set; }

        public string Login { get; set; }

       
    }
}
