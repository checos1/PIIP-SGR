using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]

    public class GestionProyectoDto
    {
        public string GuidMacroproceso { get; set; }
        public string GuidInstancia { get; set; }
        public int IdProyecto { get; set; }
        public string IdUsuario { get; set; }

        public GestionProyectoDto()
        {
        }
    }
}
