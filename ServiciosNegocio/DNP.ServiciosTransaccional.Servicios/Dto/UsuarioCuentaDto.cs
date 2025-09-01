using System;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class UsuarioCuentaDto
    {
        public int Id { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioDto Usuario { get; set; }
        public string Cuenta { get; set; }
        public Guid? EntidadId { get; set; }

    }
}
