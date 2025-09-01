using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProyectoCreditoParametroDto
    {
        public string TipoEntidad { get; set; }
        public int? IdEntidad { get; set; }
        public Guid IdFLujo { get; set; } 
        public int? IdEntidadFiltro { get; set; } 
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
    }
}
