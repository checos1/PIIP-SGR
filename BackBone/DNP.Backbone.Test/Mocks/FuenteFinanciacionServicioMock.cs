namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FuenteFinanciacionServicioMock : IFuenteFinanciacionServicios
    {
        public Task<string> AgregarFuenteFinanciacion(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ObtenerFuenteFinanciacionAgregarN(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }
    }
}
