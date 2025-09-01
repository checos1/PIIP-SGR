using DNP.Backbone.Dominio.Dto.Proyecto;
using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto
{
    public class ParametrosInstanciaDto
    {
        public Guid FlujoId { get; set; }
        public string ObjetoId { get; set; }
        public Guid RolId { get; set; }
        public string UsuarioId { get; set; }
        public IEnumerable<int> ListaEntidades { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public string DireccionIp { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public bool CreadoAutomatico { get; set; }
        public string IdUsuarioDNP { get; set; }
        //Este campo se adiciona para el llamado a la api  desde el backbone
        public Guid IdInstancia { get; set; }

        public List<NegocioDto> Proyectos { get; set; }
    }
}
