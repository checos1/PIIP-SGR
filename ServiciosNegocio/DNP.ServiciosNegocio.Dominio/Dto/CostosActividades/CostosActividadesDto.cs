namespace DNP.ServiciosNegocio.Dominio.Dto.CostosActividades
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CostosActividadesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Vigencia> vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Vigencia
    {
        public int vigencia { get; set; }
        public List<ObjetivoEspecifico> ObjetivosEspecificos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ObjetivoEspecifico
    {
        public int ObjetivoEspecificoId { get; set; }
        public string objetivoEspecifico { get; set; }
        public List<Producto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Producto
    {
        public int ProductoId { get; set; }
        public int CatalogoProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public double Cantidad { get; set; }
        public double ValorTotalProductoAjustado { get; set; }
        public double ValorTotalProductoFirme { get; set; }
        public double? ValorTotalSolicitadosGR { get; set; }
        public double? ValorTotalVigenteAjuste { get; set; }
        public double? ValorTotalSolictadosAjuste { get; set; }
        public List<Actividad> Actividades { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Actividad
    {
        public int? ActividadId { get; set; }
        public string NombreActividad { get; set; }
        public int? ActividadInsumoId { get; set; }
        public double? RecursosSolicitadosMGA { get; set; }
        public double? RecursosVigentesFirme { get; set; }
        public double? RecursosVigentesAjuste { get; set; }
        public double? RecursosSolicitadosFirme { get; set; }
        public double? RecursosSolicitadosAjuste { get; set; }
              
    }
}