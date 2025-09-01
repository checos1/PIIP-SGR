using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Requisitos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ServicioAgregarRequisitosDto
    {
        public string Bpin { get; set; }
        public Guid IdNivel { get; set; }
        public List<Atributo> ListadoAtributos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class Atributo
    {
        public string Nombre { get; set; }
        public int? IdValor { get; set; }
        public string Valor { get; set; }
        public bool? AgregadoPorRequisito { get; set; }
    }
}
