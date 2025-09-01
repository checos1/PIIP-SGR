using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    public class DatosCartaConceptoDespedidaDto
    {
        public int TramiteId { get; set; }
        public string NumeroTramite { get; set; }
        public int EntidadId { get; set; }
        public string Cartafirmada { get; set; }
        public string PalabraFraseDespedida { get; set; }     
         public string RemitenteDNP { get; set; }
        public string UsuarioRemitenteDNP { get; set; }
        public string PreparoEntidad { get; set; }
        public string PreparoColaborador { get; set; }
        public string RevisoEntidad { get; set; }
        public string RevisoColaborador { get; set; }
        public string CopiarEntidad { get; set; }
        public string CopiarServidor { get; set; }













    }
}
