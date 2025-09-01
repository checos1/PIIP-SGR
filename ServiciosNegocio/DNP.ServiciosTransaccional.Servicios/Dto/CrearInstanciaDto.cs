using System.Collections.Generic;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class CrearInstanciaDto
    {
        public string FlujoId { get; set; }
        public string UsuarioId { get; set; }
        public string ObjetoId { get; set; }
        public string RolId { get; set; }
        public string IdInstancia { get; set; }        
        public List<ProyectoInstanciaDto> Proyectos { get; set; }
        public List<int> ListaEntidades { get; set; }
    }

    public class ProyectoInstanciaDto
    {
        public string FlujoId { get; set; }
        public int IdEntidad { get; set; }
        public string IdObjetoNegocio { get; set; }
    }
}
