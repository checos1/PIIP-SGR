namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CategoriaProductoPoliticaDto
    {
        public int? ProyectoId { get; set; }
        public int? FuenteId { get; set; }
        public string BPIN { get; set; }
        public List<PoliticasDto> Politicas { get; set; }
    }

    public class PoliticasDto
    {
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<ProductosDto> Productos { get; set; }
    }

    public class ProductosDto
    {
        public int? ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<IndicadoresDto> Indicadores { get; set; }
        public List<LocalizacionesCatDto> Localizaciones { get; set; }
    }

    public class IndicadoresDto
    {
        public string Indicador { get; set; }
        public string MedidaId { get; set; }
        public string UnidadMedida { get; set; }
        public int Cantidad { get; set; }
        public double Costo { get; set; }
        public int Acumulable { get; set; }
        public string TipoIndicador { get; set; }
    }

    public class LocalizacionesCatDto
    {
        public int LocalizacionId { get; set; }
        public string Localizacion { get; set; }
        public List<CategoriasDto> Categorias { get; set; }
    }

    public class CategoriasDto
    {
        public int DimensionId { get; set; }
        public string Categoria1 { get; set; }
        public string Categoria2 { get; set; }
        public int MetaTotalCategoriaMGA { get; set; }
        public string UnidadMedida { get; set; }
        public int BeneficiariosDirectos { get; set; }
        public double CostoTotalCategoriaMGA { get; set; }
        public double CostoTotalEnRecursos { get; set; }
        public List<VigenciasCatDto> Vigencias { get; set; }
    }

    public class VigenciasCatDto
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public double CostosVigenciaMGA { get; set; }
        public double CostosCategoriaMGA { get; set; }
        public double MetaCategoriaMGA { get; set; }
        public double CostosfuentesRegionalizacion { get; set; }
        public double SolicitudRecursosCategoria { get; set; }
        public bool AplicaDetalleEtnicos { get; set; }
        public List<DetalleEtnicosDto> DetalleEtnicos { get; set; }
    }

    public class DetalleEtnicosDto
    {
        public int CostosMGA { get; set; }
        public int GrupoId { get; set; }
        public string NombreGrupo { get; set; }
        public int PoblacionMGA { get; set; }
        public int SolicitudRecurso { get; set; }
    }
}
