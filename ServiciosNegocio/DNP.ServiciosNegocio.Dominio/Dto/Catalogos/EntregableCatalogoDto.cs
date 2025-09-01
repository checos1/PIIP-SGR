namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EntregableCatalogoDto:CatalogoDto
    {
        public int MeasureTypeId { get; set; }

        public string MeasuredThrough { get; set; }

        public int ProductCId { get; set; }

        public bool IsActive { get; set; }

    }
}
