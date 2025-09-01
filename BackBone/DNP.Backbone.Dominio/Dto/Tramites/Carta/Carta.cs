namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class Carta
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public string Proceso { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoId { get; set; }
        public string Tipo { get; set; }
        public int EntidadId { get; set; }
        public string Entidad { get; set; }
        public bool Firmada { get; set; }
        public string RadicadoEntrada { get; set; }
        public string RadicadoSalida { get; set; }

        public List<CartaSecciones> ListaCartaSecciones { get; set; }
    }

}
