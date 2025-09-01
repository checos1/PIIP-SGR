using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Castle.Core.Internal;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Requisitos;
    using Persistencia.Interfaces.Requisitos;

    [ExcludeFromCodeCoverage]
    public class RequisitosServicioMock : IRequisitosPersistencia
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
