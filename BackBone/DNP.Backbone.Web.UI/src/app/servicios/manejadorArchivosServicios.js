(function (apiManejadorArchivos) {
    'use strict';

    angular.module('backbone').factory('manejadorArchivosServicios', manejadorArchivosServicios);

    manejadorArchivosServicios.$inject = ['$http', 'utilidades', 'constantesBackbone'];

    function manejadorArchivosServicios($http, utilidades, constantesBackbone) {

        return {
            obtenerArchivoRepositorio: obtenerArchivoRepositorio,
            guardarArchivoRepositorio: guardarArchivoRepositorio
        }

        //#region Invocación de las API mediante funciones $http
        /**
         * Devuelve el archivo del repositorio de MongDb filtrando por el código BPIN
         * @param codigoBpin
         * @returns {true|false}
         */
        function obtenerArchivoRepositorio(codigoBpin) {
            return $http.get(apiManejadorArchivos + constantesBackbone.apiManejadorArchivosObtenerArchivo + codigoBpin)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        /**
         * Guarda el archivo en el repositorio de MongoDB
         * @param archivo
         * @returns {true|false}
         */
        function guardarArchivoRepositorio(archivo) {
            return $http.post(apiManejadorArchivos + constantesBackbone.apiManejadorArchivosGuardarArchivo, archivo)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }
        //#endregion
    }
})(apiManejadorArchivos);
