using System;

namespace DNP.Backbone.Dominio.Dto.SGR.Viabilidad
{
    public class UsuarioEncargadoDto
    {
        public int ProyectoId { get; set; }
        public Guid InstanciaId { get; set; }
        public string UsuarioEncargado { get; set; }
        public Guid RolUsuarioEncargadoId { get; set; }
    }
}
