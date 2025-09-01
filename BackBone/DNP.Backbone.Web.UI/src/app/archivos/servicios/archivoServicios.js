(function () {
    'use strict';

    angular.module('backbone.archivo').factory('archivoServicios', archivoServicios);

    archivoServicios.$inject = ['$http', 'utilidades', 'constantesArchivos', 'constantesBackbone'];

    function archivoServicios($http, utilidades, constantesArchivos, constantesBackbone) {

        return {
            cargarArchivos: cargarArchivos,
            cargarArchivo: cargarArchivo,
            obtenerArchivosMetadatosCategoriasPorAplicacionNivel: obtenerArchivosMetadatosCategoriasPorAplicacionNivel,
            asociarMetadatosAplicacionArchivos: asociarMetadatosAplicacionArchivos,
            obtenerExtensionesPorAplicacionNivel: obtenerExtensionesPorAplicacionNivel,
            traerConfiguracionTamanoPorId: traerConfiguracionTamanoPorId,
            obtenerArchivoInfo: obtenerArchivoInfo,
            obtenerExtensiones: obtenerExtensiones,
            obtenerListadoArchivos: obtenerListadoArchivos,
            obtenerArchivoBytes: obtenerArchivoBytes,
            eliminarArchivo: eliminarArchivo,
            actualizarArchivo: actualizarArchivo,
            obtenerTipoDocumentoTramite: obtenerTipoDocumentoTramite,
            cambiarEstadoDataArchivo: cambiarEstadoDataArchivo,
            guardarArchivoRepositorio: guardarArchivoRepositorio,
            obtenerTipoDocumentoTramitePorRol: obtenerTipoDocumentoTramitePorRol,
            ConsultarSystemConfiguracion: ConsultarSystemConfiguracion,
            obtenerTipoDocumentoSoportePorRol: obtenerTipoDocumentoSoportePorRol,
            obtenerListaTipoDocumentoSoportePorRol: obtenerListaTipoDocumentoSoportePorRol
        }

        function obtenerExtensionesPorAplicacionNivel(parametros) {
            return $http.post(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiObtenerExtensionesPorAplicacionYNivel, parametros)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function traerConfiguracionTamanoPorId(parametros) {
            return $http.get(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiTraerConfiguracionTamanoArchivo, { params: parametros })
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function cargarArchivos(archivos) {
            return $http.post(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiCargarArchivos, jQuery.param(archivos), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerArchivosMetadatosCategoriasPorAplicacionNivel(parametros) {
            return $http.post(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiObtenerArchivosMetadatosCategoriasPorAplicacionNivel, jQuery.param(parametros), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function asociarMetadatosAplicacionArchivos(metadatos) {
            return $http.post(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiAsociarMetadatosAplicacionArchivos, metadatos)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerExtensiones() {
            return $http.get(constantesArchivos.apiArchivoBaseUri + constantesArchivos.apiObtenerExtensiones)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function cargarArchivo(archivo, coleccion) {

            var formdata = new FormData();
            formdata.append("FormFile", archivo.FormFile);
            formdata.append("Nombre", archivo.Nombre);
            formdata.append("Coleccion", coleccion);
            formdata.append("Status", "Creado");
            formdata.append("Metadatos", JSON.stringify(archivo.Metadatos));

            var urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCargarArchivo;
            var header = {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined },
            };

            return $http.post(urlApi, formdata, header)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);            
        }

        function obtenerArchivoInfo(Id, coleccion) {
            var urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerArchivoInfo + Id + '/' + coleccion;

            return $http.get(urlApi)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerListadoArchivos(params, coleccion) {
            var urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListadoArchivos + coleccion;

            var config = {
                headers: { 'Content-Type': 'application/json' }
            };

            return $http.post(urlApi, JSON.stringify(params), config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function guardarArchivoRepositorio(archivo) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiManejadorArchivosGuardarArchivo, archivo)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function cambiarEstadoDataArchivo(idArchivo, coleccion) {
            const urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCambiarEstadoDataArchivo + coleccion + '/' + idArchivo + '/Eliminado';

            return $http.delete(urlApi)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerTipoDocumentoTramite(tipoTramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTipoDocumentoTramite}` + '?TipoTramiteId=' + tipoTramiteId;
            return $http.get(url);
        }

        function obtenerArchivoBytes(IdArchivoBlob, coleccion) {
            const urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerArchivoBytes + '?coleccion=' + coleccion + '&IdArchivoBlob=' + IdArchivoBlob;

            delete $http.defaults.headers.common['X-Requested-With'];
            return $http.get(urlApi)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function eliminarArchivo(Id, status, coleccion) {

            const urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarArchivo + coleccion + '/' + Id + '/' + status;

            return $http.delete(urlApi)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function actualizarArchivo(Id, params, coleccion) {
            var urlApi = apiBackboneServicioBaseUri + apiBackboneActualizarArchivo + coleccion + '/' + Id;
            params.usuario = constantesArchivos.usuarioLogueado;

            return $http.put(urlApi, params)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerTipoDocumentoTramitePorRol(tipoTramiteId, rol, tramiteId, nivelId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTipoDocumentoTramite}` + '?TipoTramiteId=' + tipoTramiteId +
                "&Rol=" + rol + "&tramiteId=" + tramiteId + "&nivelId=" + nivelId;
            return $http.get(url);
        }

        function obtenerTipoDocumentoTramitePorNivel(tipoTramiteId, rolId, nivelId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTipoDocumentoTramite}` + '?TipoTramiteId=' + tipoTramiteId +
                "&Rol=" + rol + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function ConsultarSystemConfiguracion(VariableKey, separador) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarSystemConfiguracion}` + '?VariableKey=' + VariableKey + "&separador=" + separador;
            return $http.get(url);
        }

        function obtenerTipoDocumentoSoportePorRol(tipoTramiteId, rol, tramiteId, nivelId, instanciaId, accionId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_ObtenerTipoDocumentoSoporte}` + '?tipoTramiteId=' + tipoTramiteId +
                "&roles=" + rol + "&tramiteId=" + tramiteId + "&nivelId=" + nivelId + "&instanciaId=" + instanciaId + "&accionId=" + accionId;
            return $http.get(url);
        }

        function obtenerListaTipoDocumentoSoportePorRol(tipoTramiteId, rol, tramiteId, nivelId, instanciaId, accionId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_ObtenerListaTipoDocumentoSoporte}` + '?tipoTramiteId=' + tipoTramiteId +
                "&roles=" + rol + "&tramiteId=" + tramiteId + "&nivelId=" + nivelId + "&instanciaId=" + instanciaId + "&accionId=" + accionId;
            return $http.get(url);
        }
    }
    // ReSharper restore UndeclaredGlobalVariableUsing

})();
