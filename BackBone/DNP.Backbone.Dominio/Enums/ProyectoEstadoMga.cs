using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Enums
{
    public enum ProyectoEstadoMga
    {
        [Description("Formulado")]
        Formulado = 1,
        [Description("Viable")]
        Viable = 2,
        [Description("No Viable")]
        NoViable = 3,
        [Description("Aprobado")]
        Aprobado = 4,
        [Description("No Aprobado")]
        NoAprobado = 5,
        [Description("En Ejecucion")]
        EnEjecucion = 6,
        [Description("Cerrado")]
        Cerrado = 7,
    }

    public static class ProyectoEstadoMgaExtensions
    {
        public static string DescriptionAttr(this ProyectoEstadoMga source)
        {
            var fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
