using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Acciones
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetoContextoDto
    {
        public Guid IdRol { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdFormulario { get; set; }
        public Guid? IdNotifiacion { get; set; }
    }
}
