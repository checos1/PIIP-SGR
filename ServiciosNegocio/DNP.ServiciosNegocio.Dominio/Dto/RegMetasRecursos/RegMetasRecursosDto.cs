using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos
{
    public class RegMetasRecursosDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int CR { get; set; }
        public List<Producto> Productos { get; set; }
    }
    public class Producto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int IndicadorPrincipalId { get; set; }
        public string IndicadorPrincipal { get; set; }
        public int UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public double MetaTotal { get; set; }
        public List<Vigencia> Vigencias { get; set; }
    }
    public class Vigencia
    {
        public int VIGENCIA { get; set; }
        public int ProgramacionIndicadorId { get; set; }
        public double MetaIndicativa { get; set; }
        public double MetaVigencia { get; set; }
        public double RecursosSolicitados { get; set; }
        public List<Localizaciones> Localizaciones { get; set; }
    }
    public class Localizaciones
    {
        public string Localizacion { get; set; }
        public int RegionalizacionMetasId { get; set; }
        public int RegionalizacionMetasValoresId { get; set; }
        public int RegionId { get; set; }
        public int DepartamentoId { get; set; }
        public int? MunicipioId { get; set; }
        public int? AgrupacionId { get; set; }
        public int? TipoValorId { get; set; }
        public double? Cantidad { get; set; }
        public double? Costo { get; set; }
    }

}
