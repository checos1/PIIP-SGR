using System;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class ProyectoCtusDto
    {
        public int id { get; set; }
        public bool SolicitaCtus { get; set; }
        public int? EntidadDestino { get; set; }
        public Guid RolDirectorId { get; set; }
        public string NombreEntidadDestino { get; set; }
    }

    public class CrearProyectoCtusDto
    {
        public int ProyectoId { get; set; }
        public string Instancia { get; set; }
        public bool SolicitaCtus { get; set; }
        public int EntidadConcepto { get; set; }
        public string RolSolicita { get; set; }
    }
}
