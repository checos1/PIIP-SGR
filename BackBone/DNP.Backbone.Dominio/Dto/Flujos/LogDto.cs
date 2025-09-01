using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Flujos.Dominio.Dto.Flujos
{
    public class LogDto
    {
        public Guid? IdInstanciaIngreso { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string CodigoIngreso { get; set; }
        public string ProcesoIngreso { get; set; }
        public string PasoIngreso { get; set; }
        public string UsuarioIngreso { get; set; }
        public string NombreUsuarioIngreso { get; set; }
        public string RolIngreso { get; set; }
        public string NombreRolIngreso { get; set; }
        public int? EntidadIngreso { get; set; }
        public string NombreEntidadIngreso { get; set; }
        public Guid? IdInstanciaEnvio { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string CodigoEnvio { get; set; }
        public string ProcesoEnvio { get; set; }
        public string PasoEnvio { get; set; }
        public string UsuarioEnvio { get; set; }
        public string NombreUsuarioEnvio { get; set; }
        public string RolEnvio { get; set; }
        public string NombreRolEnvio { get; set; }
        public int? EntidadEnvio { get; set; }
        public string NombreEntidadEnvio { get; set; }
    }
}
