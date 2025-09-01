using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CostoActividades
{
    public class ObjectivosAjusteDto
    {
        public int Proyectoid { get; set; }
        public List<ObjetivoDto> Objetivos { get; set; }
    }

    public class EntregablesActividadesDto
    {
        public int EntregableActividadId { get; set; }
        public string EntregableActividad { get; set; }
        public decimal CostoAjusteProyecto { get; set; }
        public decimal CostoFirmeProyecto { get; set; }
        public decimal CostoMGAProyecto { get; set; }
        public int Vigencia { get; set; }
        public int Firme { get; set; }
    }

    public class VigenciaAjusteDto
    {
        public int Vigencia { get; set; }
        public List<EntregablesActividadesDto> EntregablesActividades { get; set; }
    }
    public class ProductoAjusteDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public string Etapa { get; set; }
        public int AplicaEDT { get; set; }
        public decimal CostoFirme { get; set; }
        public decimal CostoAjuste { get; set; }
        public decimal CostoMGA { get; set; }
        public int ProyectoId { get; set; }
        public List<VigenciaAjusteDto> Vigencias { get; set; }
        public List<CatalogoEntregables> CatalogoEntregables { get; set; }
    }
    public class ObjetivoDto
    {
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductoAjusteDto> Productos { get; set; }
    }
    public class CatalogoEntregables
    {
        public int EntregableId { get; set; }
        public string EntregableNombre { get; set; }
        public int ProductCatalogId { get; set; }
    }
    public class AgregarEntregable
    {
        public string nombre { get; set; }
        public string etapa { get; set; }
        public string productoId { get; set; }
        public string deliverable { get; set; }
        public string deliverableCatalogId { get; set; }
    }
}
