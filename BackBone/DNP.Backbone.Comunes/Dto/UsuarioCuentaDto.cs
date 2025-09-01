using DNP.Backbone.Comunes.Dto.Base;
using System;

namespace DNP.Backbone.Comunes.Dto
{
    public class UsuarioCuentaDto : DtoBase<int>
    {
        public Guid IdUsuario { get; set; }
        public UsuarioAuthDto Usuario { get; set; }
        public string Cuenta { get; set; }
        public string NombreTenat { get; set; }
    }
}
