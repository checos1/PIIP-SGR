using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Enums
{
    /// <summary>
    ///  Clase principal de un catálogo simple. 
    /// </summary>
    public class CatalogoSimple
    {
        /// <summary>
        ///  Obtiene o establece el identificador unico del objeto
        /// </summary>
        public object Identificador {
            get { return this.identificador; }
            set {
                this.identificador = value;
            }
        }

        /// <summary>
        ///  Obtiene o establece el nombre generico del catalogo 
        /// </summary>
        public String Nombre {
            get { return this.nombre; }
            set {
                if (!String.IsNullOrEmpty(value) && this.nombre.Equals(value)) return;
                this.nombre = value;
            }
        }

        private object identificador = new object();
        private String nombre = String.Empty;
    }

    /// <summary>
    ///  Clase principal para extender propiedades y métodos del tipo <see cref="Enum"/>
    /// </summary>
    public static class EnumExtended {

        /// <summary>
        ///  Obtiene el atributo Description parametrizado de cada elemento Enum com un String
        /// </summary>
        /// <param name="source">Objeto <see cref="Enum"/> para obtener la descripción de su atributo</param>
        /// <returns></returns>
        public static String GetDescription(this Enum source){
            var members      = source.GetType().GetMember(source.ToString());
            var descriptions = new List<String> (); 
            if (members != null && members.Length > 0)
                descriptions =members.Select(p => p.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Select(p => p.Description).ToList();

            return descriptions?.FirstOrDefault() ?? String.Empty;

        }
    }
}
