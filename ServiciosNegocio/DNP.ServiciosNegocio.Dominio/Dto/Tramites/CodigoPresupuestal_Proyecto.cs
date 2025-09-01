using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    public class CodigoPresupuestal_Proyecto
    {
        public int Id { get; set; }
        public int EntidadId { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreProyecto { get; set; }
        public string Bpin { get; set; }
        public int ProyectoId { get; set; }
        public int TramiteId { get; set; }
        public string CodigoPresupuestal { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public string CodigoPrograma { get; set; }
        public string CodigoSubprograma { get; set; }
        public int Consecutivo { get; set; }
        public string NombreFuente { get; set; }
        public decimal ValorVigente { get; set; }

        public Guid InstanciaId { get; set; }
        public string UsuarioEnvio { get; set; }
        public string Titulo { get; set; }
        public string Plantilla { get; set; }

        public string CodigoProceso { get; set; }
        public List<DatosUsuarioDto> ListaUsuarios { get; set; }
    }
}
