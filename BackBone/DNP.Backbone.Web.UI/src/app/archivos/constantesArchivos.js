(function () {
    'use strict';
    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.archivo').constant("constantesArchivos", {
        apiArchivoBaseUri: apiArchivo,
        apiObtenerExtensionesPorAplicacionYNivel: 'api/ConfiguracionExtension/ObtenerExtensionesPorAplicacionYNivel',
        apiTraerConfiguracionTamanoArchivo: 'api/Archivos/TraerConfiguracionTamanoPorIdAppNivel',
        apiCargarArchivos: 'api/Archivos/CargarArchivos',
        apiObtenerArchivosMetadatosCategoriasPorAplicacionNivel: 'api/Archivos/ObtenerArchivosMetadatosCategoriasPorAplicacionNivel',
        apiAsociarMetadatosAplicacionArchivos: 'api/Metadatos/AsociarMetadatosAplicacionArchivos',
        apiObtenerExtensiones: 'api/ConfiguracionExtension/ObtenerExtensiones',
        apiManejadorArchivosUri: apiManejadorArchivos,
        apiArchivos: 'Archivos',
        keyCollection: keyCollection,
        usuarioLogueado: usuarioLogueado,
        apiArchivoInfo: 'ArchivoInfo/',
        metadatosConstantes: ['BPIN']
    });
})();