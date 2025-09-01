using DNP.Backbone.Comunes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class UsuarioAnalistaConceptoDto
    {
        public Guid IdAplicacion { get; set; }
        public string IdAplicacionDnp { get; set; }

        public Guid IdUsuario { get; set; }

        public string IdUsuarioDnp { get; set; }

        public string Nombre { get; set; }

        public string TipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public bool Activo { get; set; }

        public bool Administrador { get; set; }

        public bool Seleccionado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }

        public List<UsuarioCuentaDto> UsuarioCuentas { get; set; }
    }
}
