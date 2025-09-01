namespace DNP.ServiciosNegocio.Dominio.Dto.Transferencias
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ObjetoNegocio
    {
        public string ObjetoNegocioId { get; set; }

        public string NivelId { get; set; }
        public string FlujoId { get; set; }

        public string Vigencia { get; set; }

        public string InstanciaId { get; set; }
        public string IdAccion { get; set; }
        public string IdRol { get; set; }
    }
}
