using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteFinanciacionAgregarAjusteDto
    {
        public string bpin { get; set; }
        public string etapa { get; set; }
        public int fuenteId { get; set; }
        public List<ListaVigencias> valores { get; set; }
    }
    public class ListaVigencias
    {
        public List<Cofinanciador> Cofinanciador { get; set; }
        public bool contieneCofinanciador { get; set; }
        public int DatosAdicionales { get; set; }
        public string Etapa { get; set; }
        public string Financiador { get; set; }
        public int FuenteFirme { get; set; }
        public int Id { get; set; }
        public string Recurso { get; set; }
        public string Sector { get; set; }
        public string TipoFinanciador { get; set; }
        public decimal ValorTotalFuente { get; set; }
        public List<vigenciaFuente> vigenciaFuente { get; set; }



        /*
    public int fuenteId { get; set; }
    public string vigencia { get; set; }
    public decimal ValorTotal { get; set; }
    */
    }
    public class Cofinanciador
    {
        public string codigo { get; set; }
        public string codigoCofinanciador { get; set; }
        public int cofinanciadorId { get; set; }
        public List<listaVigenciasCofinanciador> listaVigenciasCofinanciador { get; set; }
        public string tipoCofinanciador { get; set; }
        public decimal valorTotalCof { get; set; }
    }

    public class vigenciaFuente
    {
        public string Vigencia { get; set; }
        public decimal Valor { get; set; }
    }

    public class listaVigenciasCofinanciador
    {
        public string Vigencia { get; set; }
        public decimal Valor { get; set; }
    }
}
