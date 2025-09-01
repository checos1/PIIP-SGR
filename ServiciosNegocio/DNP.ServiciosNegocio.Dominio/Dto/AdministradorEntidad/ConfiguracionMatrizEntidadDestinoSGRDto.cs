using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad
{
    [ExcludeFromCodeCoverage]
    public class MatrizEntidadDestinoSGRDto
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> CRTypeId { get; set; }
        public Nullable<int> EntidadResponsableId { get; set; }
        public Guid TipoFlujo { get; set; }
        public Nullable<int> IdDetalle { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<Guid> RolId { get; set; }
        public Nullable<int> EntidadDestinoId { get; set; }
        public string EntidadDestinoAccion { get; set; }
        public Nullable<int> Estado { get; set; } //0-Actual , 1-Adicionado, 2-Eliminado
    }

    [ExcludeFromCodeCoverage]
    public class ConfiguracionMatrizEntidadDestinoSGRDto
    {
        public List<MatrizEntidadDestinoSGRDto> Respuesta { get; set; }
        public int TipoFlujo { get; set; }
        public int EntidadResponsableId { get; set; }
        public Guid FlowId { get; set; }
    }
}
