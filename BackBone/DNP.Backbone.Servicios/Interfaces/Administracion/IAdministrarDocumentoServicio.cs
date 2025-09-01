using DNP.Backbone.Dominio.Dto.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Administracion
{
    public interface IAdministrarDocumentoServicio
    {
        Task<string> AdministrarDocumentoConsultar(string usuarioDnp, string IdDocumento);
        Task<string> AdministrarDocumentoCrear(string usuarioDnp, AdministracionDocumentoDto Documento);
        Task<string> AdministrarDocumentoActualizar(string usuarioDnp, AdministracionDocumentoDto Documento);
        Task<string> AdministrarDocumentoEstado(string usuarioDnp, AdministracionDocumentoDto Documento);
        Task<string> AdministrarDocumentoEliminar(string usuarioDnp, string IdDocumento);
        Task<string> AdministrarDocumentoReferencias(string usuarioDnp);
        Task<string> AdministrarDocumentoConsultarUso(string usuarioDnp);
        Task<string> AdministrarCrearUsoDocumento(string usuarioDnp, AdministracionDocumentoUsoDto Documento);
        Task<string> AdministrarActualizarUsoDocumento(string usuarioDnp, AdministracionDocumentoUsoDto Documento);
        
        Task<string> AdministrarEliminarUsoDocumento(string usuarioDnp, string Id);
    }
}
