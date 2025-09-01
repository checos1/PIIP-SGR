using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DNP.Backbone.Dominio.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MunicipioCatalogoDto : CatalogoDto
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public int DepartmentId { get; set; }
    }
}
