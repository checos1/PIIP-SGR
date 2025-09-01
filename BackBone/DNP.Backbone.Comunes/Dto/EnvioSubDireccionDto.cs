using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class EnvioSubDireccionDto
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public string IdUsuarioDNP { get; set; }
        public int EntityTypeCatalogOptionId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public bool Enviado { get; set; }
        public int ParentId { get; set; }
        public string Usuario { get; set; }
        public bool Visible { get; set; }
        public string NombreUsuarioDNP { get; set; }
        public string NombreEntidad { get; set; }
        public string Correo { get; set; }
        public string IdUsuarioDNPQueEnvia { get; set; }
        public string NombreUsuarioQueEnvia { get; set; }
        public DateTime FechaEntrega { get; set; }

    }
}
