using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    //Se excluye por ser un DTO que se comunica con el GraphAPI y otros servicios externos por lo que se sigue su estandares de nombramiento.
    public class PerfilClaveDto
    {
        public string password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
    }
}
