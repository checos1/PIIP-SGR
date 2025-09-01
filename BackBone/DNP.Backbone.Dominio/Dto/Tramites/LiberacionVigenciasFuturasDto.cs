namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System.Diagnostics.CodeAnalysis;
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class LiberacionVigenciasFuturasDto
    {
        public int tramiteProyectoId { get; set; }
        public int tramiteId { get; set; }
        public string creadoPor { get; set; }
        public System.DateTime? vigenciaDesde { get; set; }
        public System.DateTime? vigenciaHasta { get; set; }
    }

    public class tramiteVFAsociarproyecto
    {
        public int? Id { get; set; }
        public string NumeroTramite { get; set; }
        public string Descripcion { get; set; }
        public string ObjContratacion { get; set; }
        public int? tipotramiteId { get; set; }
        public DateTime? fecha { get; set; }
    }
}
