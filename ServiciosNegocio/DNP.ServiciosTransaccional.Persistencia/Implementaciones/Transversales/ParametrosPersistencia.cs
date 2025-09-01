namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transversales
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Transversales;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ParametrosPersistencia: Persistencia, IParametrosPersistencia
    {
        public ParametrosPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public string ConsultarParametro(string llave)
        {
            var valor = Contexto.SystemConfigurations.FirstOrDefault(x => x.Key == llave).Value.ToString();
            return valor;
        }

        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
