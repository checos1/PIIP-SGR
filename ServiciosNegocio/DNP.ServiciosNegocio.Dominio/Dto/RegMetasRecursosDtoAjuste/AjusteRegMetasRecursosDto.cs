using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste
{
    public class AjusteRegMetasRecursosDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<VigenciaEtapa> Vigencia_etapas { get; set; }
        public List<Producto> Productos { get; set; }
    }
    public class Etapa
    {
        public double Preinversion { get; set; }
        public double Inversion { get; set; }
        public double Operacion { get; set; }
    }

    public class VigenciaEtapa
    {
        public int Vigencia { get; set; }
        public List<Etapa> Etapas { get; set; }
    }

    public class Localizacione
    {
        public int LocalizacionId { get; set; }
        public int RegionId { get; set; }
        public int DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int TipoAgrupacioniD { get; set; }
        public int AgrupacionId { get; set; }
        public double MetaSolicitadaGR { get; set; }
        public double MetaInicial { get; set; }
        public double MetaVigenteFirme { get; set; }
        public double? MetaEnAjuste { get; set; }
        public double RecursosSolicitadosGR { get; set; }
        public double RecursosIniciales { get; set; }
        public double RecursosVigentesEnFirme { get; set; }
        public double? RecursosVigentesEnAjuste { get; set; }
    }

    public class Vigencias
    {
        public int Vigencia { get; set; }
        public int ProgramacionIndicadorId { get; set; }
        public double MetaSolicitadaGR { get; set; }
        public double MetaInicial { get; set; }
        public double MetaVigenteFirme { get; set; }
        public double MetaEnAjuste { get; set; }
        public double RecursosSolicitadosGR { get; set; }
        public double RecursosIniciales { get; set; }
        public double RecursosVigentesEnFirme { get; set; }
        public double RecursosVigentesEnAjuste { get; set; }
        public List<Localizacione> Localizaciones { get; set; }
    }

    public class Producto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int IndicadorPrincipalId { get; set; }
        public string IndicadorPrincipal { get; set; }
        public int UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public double TotalRecursosSolicitados { get; set; }
        public double MetaTotal { get; set; }
        public List<Vigencias> Vigencias { get; set; }
    }
}
