using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.OcadPaz
{
    public class ProyectoCtusDto
    {
        public int id { get; set; }
        public Nullable<int> ProyectoId { get; set; }
        public Nullable<System.Guid> Instancia { get; set; }
        public int TipoCtus { get; set; }
        public Nullable<bool> SolicitaCtus { get; set; }
        public string UsuarioSolicita { get; set; }
        public Nullable<System.Guid> RolSolicita { get; set; }
        public Nullable<int> EntidadConcepto { get; set; }
        public Nullable<int> EntidadDestino { get; set; }
        public Nullable<bool> Concepto { get; set; }
        public Nullable<System.DateTime> FechaConcepto { get; set; }
        public Nullable<bool> Devolvio { get; set; }
        public Nullable<System.DateTime> FechaDevolucion { get; set; }
        public Nullable<System.Guid> usuarioDevolucion { get; set; }
        public Nullable<System.Guid> RolDevolucion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Nullable<System.DateTime> VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public Nullable<bool> EsProyectoTipo { get; set; }
        public Nullable<bool> TieneCTUSVigente { get; set; }
        public System.DateTime FechaCTUSVigente { get; set; }
        public Guid? RolDirectorId { get; set; }
        public Guid? RolTecnicoId { get; set; }
        public Guid? RolEmiteId { get; set; }
        public string NombreEntidadDestino { get; set; }
        public bool InstanciaParalela { get; set; }
    }
}
