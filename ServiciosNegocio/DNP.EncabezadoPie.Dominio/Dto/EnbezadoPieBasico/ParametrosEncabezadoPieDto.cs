using System.Diagnostics.CodeAnalysis;

namespace DNP.EncabezadoPie.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public class ParametrosEncabezadoPieDto
    {
        public int? Id { get; set; }
        public string Bpin { get; set; }
        public string Sector { get; set; }

        public string Entidad { get; set; }

        public int AnioInicio { get; set; }
    }
}
