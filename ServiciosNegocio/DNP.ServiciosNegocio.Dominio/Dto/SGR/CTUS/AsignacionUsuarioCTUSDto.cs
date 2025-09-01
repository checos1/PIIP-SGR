using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS
{
    public class AsignacionUsuarioCTUSDto
    {
        public int ProyectoCtusId { get; set; }
        public Guid InstanciaId { get; set; }
        public string UsuarioEncargado { get; set; }
        public Guid RolUsuarioEncargadoId { get; set; }
        public string Tipo { get; set; }
    }
}
