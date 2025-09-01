using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class AgrupacionCodeDto
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public int? MunicipalityId { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}