namespace DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos
{
    using DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System;
    using System.Collections.Generic;

    public interface ICambiosJustificacionHorizontePersistencia
    {
        List<JustificaccionHorizonteDto> ObtenerCambiosJustificacionHorizonte(int IdProyecto);
    }
}
