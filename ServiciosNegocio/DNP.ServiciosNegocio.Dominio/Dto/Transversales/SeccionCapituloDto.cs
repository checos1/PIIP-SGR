namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class SeccionCapituloDto
    {
        public int SeccionId { get; set; }
        public int SeccionCapituloId { get; set; }
        public int CapituloId { get; set; }
        public string Macroproceso { get; set; }
        public Guid? Instancia { get; set; }
        public string Seccion { get; set; }
        public string SeccionModificado { get; set; }
        public string Capitulo { get; set; }
        public string CapituloModificado { get; set; }
        public bool? Modificado { get; set; }
        public string Justificacion { get; set; }
        public string nombreComponente { get; set; }

    }

    public class CapituloModificado
    {
        public int? ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public string Usuario { get; set; }
        public int SeccionCapituloId { get; set; }
        public int CapituloId { get; set; }
        public int SeccionId { get; set; }
        public bool Modificado { get; set; }
        public Guid? InstanciaId { get; set; }
        public int AplicaJustificacion { get; set; }
        public bool? Cuenta { get; set; }
    }

    public class SeccionesCapitulos
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

    public class JustificacionPoliticaModificada
    {
        public int ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public string Usuario { get; set; }
        public int SeccionCapituloId { get; set; }
        public Guid InstanciaId { get; set; }
        public int PoliticaId { get; set; }
    }
}
