namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosConsultaDto
    {
        public Guid InstanciaId { get; set; }
        public Guid AccionId { get; set; }
        public Guid FormularioId { get; set; }
        public string Bpin { get; set; }
        public Guid IdNivel { get; set; }
        public string Token { get; set; }
        public string Usuario { get; set; }
        public string ListaRoles { get; set; }
        public int TramiteId { get; set; }
        public int? plantillaCartaSeccionId { get; set; }
        public int IdFuente { get; set; }
    }
}