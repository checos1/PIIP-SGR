using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Administracion
{


    [ExcludeFromCodeCoverage]
    public class AdministracionDocumentoUsoDto
    {
        public string Id { get; set; }
        public int IdFase { get; set; }
        public int? ObligatoriedadId { get; set; }
        public Boolean? Obligatorio { get; set; }
        public List<Rol> Roles { get; set; }
        public string TipoDocumentoId { get; set; }
        public string TipoTramiteId { get; set; }
        public int? ValidacionId { get; set; }
        public Boolean? Visible { get; set; }
        public string ModificadoPor { get; set; }
    }

    public class Rol
    {
        public int FaseId { get; set; }
        public string IdRol { get; set; }
        public string NombreRol { get; set; }
        public int TipoTramiteId { get; set; }
        public Boolean? selected { get; set; }
    }
}
