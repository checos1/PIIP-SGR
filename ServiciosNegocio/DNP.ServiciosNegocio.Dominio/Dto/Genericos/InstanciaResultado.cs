using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Genericos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class InstanciaResultado
    {
        public bool Exitoso { get; set; }
        public string MensajeOperacion { get; set; }
        public Guid? InstanciaId { get; set; }
        public Guid? AccionPorInstanciaId { get; set; }
        public string NumeroTramite { get; set; }
    }
}
