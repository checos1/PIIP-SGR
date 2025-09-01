using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Auditoria
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class GuardarAuditoriaDto<T>
    {
        public string Mensaje { get; set; }
        public T Contenido { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? AccionId { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaAccion { get; set; }
    }
}
