using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.AjusteAgregarPoliticas
{
    public class AjusteIncluirPoliticasTDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Politicas> Politicas { get; set; }
    }
    public class Politicas
    {
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public int? Dimension1Id { get; set; }
        public string Dimension1 { get; set; }
        public int? Dimension2Id { get; set; }
        public string Dimension2 { get; set; }
    }
}
