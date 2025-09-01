using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Enums
{
    /// <summary>
    ///  Definición de los tres tipos de estados de un proceso 
    /// </summary>
    public enum EstadoProceso { 
        
        /// <summary>
        ///  Determina el estado del proceso actual que no ha iniciado, y necesita un valor para inicar
        /// </summary>
        [Description("En Espera")]
        EnEspera = 0,

        /// <summary>
        ///  Determina el estado del proceso actual que ya se está ejecutando.
        /// </summary>
        [Description("En Ejecución")]
        Ejecutado = 1,

        /// <summary>
        ///  Determina el estado del proceso actual que ya finalizó
        /// </summary>
        [Description("Terminado")]
        Terminado = 2
    }
}
