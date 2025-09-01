namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class RubroCatalogoDto : CatalogoDto
    {
        public string Code { get; set; }
        public string Bpin { get; set; }
        public string Rubro { get; set; }
    }
}
