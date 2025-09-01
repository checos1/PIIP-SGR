using System;

namespace DNP.Backbone.Dominio.Dto.SGP.Transversal
{
    public class ProyectoViabilidadInvolucradosSGPDto
    {
        public int? Id { get; set; }
        public int ProyectoId { get; set; }
        public int TipoConceptoViabilidadId { get; set; }
        public string TipoConceptoViabilidad { get; set; }
        public int TipoRolConceptoId { get; set; }
        public string TipoRolConcepto { get; set; }
        public string Nombre { get; set; }
        public int DependenciaFuncionarioInvolucradoId { get; set; }
        public string Area { get; set; }
    }

    public class ProyectoViabilidadInvolucradosFirmaSGPDto
    {
        public string IdUsuarioDNP { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Area { get; set; }
        public string Accion { get; set; }
        public DateTime? FechaFirma { get; set; }
        public bool Firmado { get; set; }
    }

    public class DependenciasFuncionarioInvolucradoSGPDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
