namespace DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class JustificaccionHorizonteDto
    {
        public int HorizonteId { get; set; }
        public int Periodo { get; set; }
        public int Vigencia { get; set; }
        public string Estado { get; set; }
        public int? VigenciaFirme { get; set; }


    }
}
