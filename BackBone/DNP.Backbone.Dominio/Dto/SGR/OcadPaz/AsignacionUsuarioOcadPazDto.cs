using System;

namespace DNP.Backbone.Dominio.Dto.SGR.CTUS
{
    public class AsignacionUsuarioOcadPazDto
    {
        public int ProyectoId { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid AccionId { get; set; }
        public string UsuarioEncargado { get; set; }
        public Guid RolUsuarioEncargadoId { get; set; }
        public int? UsuarioEncargadoOcadPazId { get; set; }
    }
}
