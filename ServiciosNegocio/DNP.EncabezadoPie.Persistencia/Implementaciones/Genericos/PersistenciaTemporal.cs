using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.EncabezadoPie.Persistencia.Implementaciones.Genericos
{
    using Interfaces;
    using Interfaces.Genericos;
    using Modelo;
    public class PersistenciaTemporal : Persistencia, IPersistenciaTemporal
    {
        public PersistenciaTemporal(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public AlmacenamientoTemporal ObtenerTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            var resultado = Contexto.AlmacenamientoTemporal.FirstOrDefault(t => t.InstanciaId == parametrosConsultaDto.InstanciaId && t.AccionId == parametrosConsultaDto.AccionId);

            return resultado;
        }
    }
}
