
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.EncabezadoPie.Persistencia.Interfaces.Genericos
{
    using Modelo;
    public interface IPersistenciaTemporal
    {
        AlmacenamientoTemporal ObtenerTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
