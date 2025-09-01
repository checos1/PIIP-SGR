using DNP.ServiciosNegocio.Dominio.Dto.Administracion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Administracion
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IAdministrarDocumentoServicio
    {
        string AdministrarDocumentoConsultar(string NombreDocumento);
        string AdministrarDocumentoCrear(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoActualizar(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoEliminar(string Id);
        string AdministrarDocumentoEstado(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoReferencias();
        
        /** Usos Documento */
        string AdministrarDocumentoConsultarUso();
        string AdministrarCrearUsoDocumento(AdministracionDocumentoUsoDto Documento);
        string AdministrarActualizarUsoDocumento(AdministracionDocumentoUsoDto Documento);
        string AdministrarDocumentoUsoEliminar(string Id);
        



    }
}
