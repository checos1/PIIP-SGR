using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Dominio.Dto.Enrutamiento;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using System.Collections.Generic;

namespace DNP.ServiciosEnrutamiento.Servicios.Interfaces
{
    public interface ITallerServicio
    {
        ResultadoEnrutamiento EjecutarReglaTaller(ObjetoNegocio objetoNegocio, ParametrosAuditoriaDto parametrosAuditoria);
        List<ReglaEnrutamiento> ObtenerReglasTaller();
    }
}
