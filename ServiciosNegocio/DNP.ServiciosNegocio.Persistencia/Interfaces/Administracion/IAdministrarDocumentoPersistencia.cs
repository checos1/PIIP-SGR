
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion
{
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IAdministrarDocumentoPersistencia
    {
        string AdministrarDocumentoConsultar(string NombreDocumento);
        string AdministrarDocumentoCrear(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoActualizar(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoEliminar(string IdDocumento);
        string AdministrarDocumentoEstado(AdministracionDocumentoDto Documento);
        string AdministrarDocumentoReferencias();
        
        /** Usos Documento */
        string AdministrarDocumentoConsultarUso();
        string AdministrarCrearUsoDocumento(AdministracionDocumentoUsoDto Documento);
        string AdministrarActualizarUsoDocumento(AdministracionDocumentoUsoDto Documento);
        string AdministrarDocumentoUsoEliminar(string Id);
        
    }
}