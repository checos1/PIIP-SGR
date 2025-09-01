using System;

namespace DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio
{
    public class ParametrosProyectoVerificacionSgrDto
    {
        public Guid IdTipoObjetoNegocio { get; set; }
        public string IdUsuarioDNP { get; set; }
        public string ListRol { get; set; }
        public string ListNivel { get; set; }
        public string ListSubPasos { get; set; }
        public int? ValidarActual { get; set; }
    }
}