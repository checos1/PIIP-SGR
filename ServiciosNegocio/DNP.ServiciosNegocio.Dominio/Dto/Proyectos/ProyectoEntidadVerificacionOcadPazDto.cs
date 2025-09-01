using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoEntidadVerificacionOcadPazDto
    {
        public string IdObjetoNegocio { get; set; }
        public string ProyectoNombre { get; set; }
        public string CodigoBpin { get; set; }
        public string EntidadNombre { get; set; }
        public int EntidadId { get; set; }
        public Guid? IdInstancia { get; set; }
        public Guid? IdAccion { get; set; }
        public string NombreAccion { get; set; }
        public string TipoEntidad { get; set; }
        public int SectorId { get; set; }
        public string Estado { get; set; }
        public int? TipoEntidadId { get; set; }
        public string DescripcionCR { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionStr { get; set; }
        public int ProyectoId { get; set; }
        public int? EstadoId { get; set; }
        public string SectorNombre { get; set; }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
        public string NombreFlujo { get; set; }
        public int? IdEstadoInstancia { get; set; }
        public string EstadoInstancia { get; set; }
        public Guid? FlujoId { get; set; }
        public string CodigoProceso { get; set; }
        public Guid? IdNivel { get; set; }
        public DateTime? FechaPaso { get; set; }
        public string FechaPasoStr { get; set; }
        public string CodigoTramite { get; set; }
        public Guid? InstanciaPadreId { get; set; }
        public int? CRTypeId { get; set; }
        public int? ResourceGroupId { get; set; }
        public bool? AplicaAccion { get; set; }
        public int? UsuarioEncargadoOcadPazId { get; set; }
        public string UsuarioEncargadoAsignadoOcadPaz { get; set; }
        public Guid? IdRol { get; set; }
        public string AsignadoA { get; set; }
        public string CorreoAsignadoA { get; set; }
        public string Cumple { get; set; }
    }
}
