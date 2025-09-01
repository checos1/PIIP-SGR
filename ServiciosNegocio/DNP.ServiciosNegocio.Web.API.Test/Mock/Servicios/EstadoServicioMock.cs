namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class EstadoServicioMock : IEstadoServicio
    {
        public Task<List<EstadoDto>> ConsultarEstados()
        {
            return Task.FromResult(new List<EstadoDto>() { new EstadoDto() { Id = 1, Estado = "Estado 1", Codigo = "01" } });
        }

        

    }
}
