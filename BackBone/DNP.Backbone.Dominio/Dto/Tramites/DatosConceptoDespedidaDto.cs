using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class DatosConceptoDespedidaDto
    {
        public int? TramiteId { get; set; }
        public int? CartaId { get; set; }
        public int? CartaSeccionId { get; set; }
        public int? TipoTramite { get; set; }
        public int? PantillaSeccionId { get; set; }
        public List<Tramite> Tramite { get; set; }
        public List<Campos> Campos { get; set; }
    }

    public class Tramite
    {
        public int? TramiteId { get; set; }
        public int? CartaConceptoId { get; set; }
        public string NumeroTramite { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public string Cartafirmada { get; set; }
        public string PalabraFraseDespedida { get; set; }
        public string RemitenteDNP { get; set; }
        public string UsuarioRemitenteDNP { get; set; }
        public string PreparoEntidad { get; set; }
        public string PreparoColaborador { get; set; }
        public string RevisoEntidad { get; set; }
        public string RevisoColaborador { get; set; }
        public List<Copiar> Copiar { get; set; }
    }

    public class Copiar
    {
        public int? CopiarEntidadId { get; set; }
        public int? CopiarServidorId { get; set; }
        public string CopiarEntidad { get; set; }
        public string CopiarServidor { get; set; }
        public int? Bloquear { get; set; }

    }

    public class Campos
    {
        public int? Id { get; set; }
        public int? PlantillaCartaCampoId { get; set; }
        public string DatoValor { get; set; }
        public string NombreCampo { get; set; }
        public List<CamposCopiar> CamposCopiar { get; set; }
    }

    public class CamposCopiar
    {
        public int? Id { get; set; }
        public int? PlantillaCartaCampoId { get; set; }
        public string DatoValor { get; set; }
        public string NombreCampo { get; set; }
        public int? OrdenAgrupa { get; set; }

    }

}
