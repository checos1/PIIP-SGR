using System.Diagnostics.CodeAnalysis;


namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    public class MesDto
    {
        public int? Mes{ get; set; }
        public string NombreMes { get; set; }
        public List<FuenteDto> Fuente { get; set; }
    }
}
