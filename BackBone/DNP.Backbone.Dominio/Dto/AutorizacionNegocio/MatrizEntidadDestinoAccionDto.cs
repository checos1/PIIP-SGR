using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class MatrizEntidadDestinoAccionDto
    {
        public int Id { get; set; }
        public int CRTypeId { get; set; }
        public Nullable<int> EntidadResponsableId { get; set; }
        public string EntidadResponsable { get; set; }
        public int SectorId { get; set; }
        public string Sector { get; set; }
        public System.Guid RolId { get; set; }
        public string Rol { get; set; }
        public int EntidadDestinoAccionId { get; set; }
        public string EntidadDestinoAccion { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Nullable<System.Guid> FaseGuid { get; set; }
        public Nullable<System.Guid> TipoFlujo { get; set; }
    }
}
