using DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.AdministradorEntidad
{
    public class AdministradorEntidadSgpServicioMock : IAdministradorEntidadSgpServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }
        public string ObtenerSectores()
        {
            throw new NotImplementedException();
        }

        public string ObtenerFlowCatalog()
        {
            throw new NotImplementedException();
        }

        public List<ConfiguracionMatrizEntidadDestinoSGRDto> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
