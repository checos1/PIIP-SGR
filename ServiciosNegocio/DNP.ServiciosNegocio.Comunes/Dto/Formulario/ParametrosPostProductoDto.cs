namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosPostProductoDto<T>
    {
        public Guid? InstanciaId { get; set; }
        public Guid? AccionId { get; set; }
        public T Contenido { get; set; }
    }
}
