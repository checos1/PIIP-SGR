namespace DNP.Backbone.Comunes.Dto
{
    using System;
    public class ProyectoParametrosEncabezadoDto
    {
        public Guid idInstancia { get; set; }
        public Guid idFlujo { get; set; }
        public Guid idNivel { get; set; }
        public string idProyectoStr { get; set; }
        public int idProyecto { get; set; }
        public string tramite { get; set; }
    }
}