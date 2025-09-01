using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Castle.Core.Internal;
    using Comunes.Dto.Formulario;
    using Persistencia.Interfaces.Requisitos;
    using Dominio.Dto.Requisitos;

    public class RequisitosPersistenciaMock : IRequisitosPersistencia
    {
        public void GuardarDefinitivamente(ParametrosGuardarDto<ServicioAgregarRequisitosDto> parametrosGuardar, string usuario)
        {
        }

        public List<Atributo> ObtenerRequisitos(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId)
        {
            if (!bPin.IsNullOrEmpty())
            {
                return new List<Atributo>()
                {
                    new Atributo()
                    {
                        Nombre= "Sector",
                        IdValor= 33,
                        Valor= "Cultura",
                        AgregadoPorRequisito= false
                    },
                    new Atributo()
                    {
                        Nombre= "Programa",
                        IdValor= 1127,
                        Valor= "3399  - Fortalecimiento de la gestión y dirección del Sector Cultura",
                        AgregadoPorRequisito= false
                    }
                };
            }
            else
            {
                return new List<Atributo>();
            }
        }
    }
}
