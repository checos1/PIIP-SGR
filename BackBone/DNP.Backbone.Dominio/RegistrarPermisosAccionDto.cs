using DNP.Backbone.Dominio.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio
{
    public class RegistrarPermisosAccionDto
    {
        public string ObjetoNegocioId { get; set; }

        public Guid IdAccion { get; set; }

        public Guid IdInstancia { get; set; }

        public int EntityTypeCatalogOptionId { get; set; }

        public List<ListadoUsuario> listadoUsuarios { get; set; }
    }
}
