using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Administracion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Administracion
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class AdministrarDocumentoServicio : IAdministrarDocumentoServicio
    {
        private readonly IAdministrarDocumentoPersistencia _datosPersistencia;
        public AdministrarDocumentoServicio(IAdministrarDocumentoPersistencia datosPersistencia) {
            _datosPersistencia = datosPersistencia;
        }
        public string AdministrarDocumentoConsultar(string NombreDocumento) {
        return _datosPersistencia.AdministrarDocumentoConsultar(NombreDocumento);
        }
        public string AdministrarDocumentoCrear(AdministracionDocumentoDto Documento){
        return _datosPersistencia.AdministrarDocumentoCrear(Documento);
        }
        public string AdministrarDocumentoActualizar(AdministracionDocumentoDto Documento)
        {
            return _datosPersistencia.AdministrarDocumentoActualizar(Documento);
        }
        public string AdministrarDocumentoEliminar(string IdDocumento)
        {
            return _datosPersistencia.AdministrarDocumentoEliminar(IdDocumento);
        }
        public string AdministrarDocumentoEstado(AdministracionDocumentoDto Documento)
        {
            return _datosPersistencia.AdministrarDocumentoEstado(Documento);
        }
        public string AdministrarDocumentoReferencias()
        {
            return _datosPersistencia.AdministrarDocumentoReferencias();
        }
        /** Usos Documento */
        public string AdministrarDocumentoConsultarUso()
        {
            return _datosPersistencia.AdministrarDocumentoConsultarUso();
        }
        public string AdministrarCrearUsoDocumento(AdministracionDocumentoUsoDto Documento)
        {
            return _datosPersistencia.AdministrarCrearUsoDocumento(Documento);
        }
        public string AdministrarActualizarUsoDocumento(AdministracionDocumentoUsoDto Documento)
        {
            return _datosPersistencia.AdministrarActualizarUsoDocumento(Documento);
        }

        public string AdministrarDocumentoUsoEliminar(string Id)
        {
            return _datosPersistencia.AdministrarDocumentoUsoEliminar(Id);
        }


    }
}
