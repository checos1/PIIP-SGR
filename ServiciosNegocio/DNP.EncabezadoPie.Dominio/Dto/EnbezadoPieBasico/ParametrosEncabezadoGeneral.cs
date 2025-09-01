using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.EncabezadoPie.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public class ParametrosEncabezadoGeneral
    {
        public Guid idInstancia { get; set; }
        public Guid idFlujo { get; set; }
        public Guid idNivel { get; set; }
        public string idProyectoStr { get; set; }
        public int idProyecto { get; set; }
        public string tramite { get; set; }
    }
}
