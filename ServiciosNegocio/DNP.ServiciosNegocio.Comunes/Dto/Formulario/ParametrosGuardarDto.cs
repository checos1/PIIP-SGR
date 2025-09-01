namespace DNP.ServiciosNegocio.Comunes.Dto.Formulario
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosGuardarDto<T>
    {
        public Guid? InstanciaId { get; set; }
        public Guid? AccionId { get; set; }
        public Guid? FormularioId { get; set; }
        public T Contenido { get; set; }
        public string Usuario { get; set; }
        int CartaId { get; set; }
        int CartaSeccionId { get; set; }
        int TramiteId { get; set; }
        int tipoTramite { get; set; }
        
    }
}
