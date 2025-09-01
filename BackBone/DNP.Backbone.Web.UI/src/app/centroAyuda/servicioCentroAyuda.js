(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('servicioCentroAyuda', servicioCentroAyuda);

    servicioCentroAyuda.$inject = ['$http', 'constantesBackbone', '$location', '$rootScope'];

    function servicioCentroAyuda($http, constantesBackbone, $location, $rootScope) {

        return {
            obtenerJsonLocal: obtenerJsonLocal,
            obtenerListaTemas: obtenerListaTemas,
            crearActualizarAyudaTema: crearActualizarAyudaTema,
            eliminarAyudaTema: eliminarAyudaTema,
            obtenerListaTemasBroadcast: obtenerListaTemasBroadcast,
            ObtenerPdfTemario: ObtenerPdfTemario,
            ObtenerPdfVideos:ObtenerPdfVideos
        }

        function obtenerListaTemas(filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTemas + "?noblockui";
            return $http.post(url, filtro);
        }
        
        function crearActualizarAyudaTema(model) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearActualizarAyudaTema;
            return $http.post(url, model);
        }

        function eliminarAyudaTema(id) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneEliminarAyudaTema}`;
            return $http.delete(`${url}${id}`)
        }

        /**
         * 
         * @description . Obtiene un archivo binario desde el servidor con los datos proporcionados por el parámetro datos
         * @param {Array} datos. Lista de datos a mostrar en el archivo PDF. Ver clase DNP.Backbone.Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto 
         */
        function ObtenerPdfTemario(datos){
            try {
                let url = `${urlPDFBackbone}${constantesBackbone.apiBackboneObtenerPdfTemarioAyuda}`;
                return  $http.post(url, {datos : datos });
            }
            catch(excepcion){
                throw { message: `servicioCentroAyuda.ObtenerPdfTemario => ${excepcion.message}` };
            }
        }

         /**
         * 
         * @description . Obtiene un archivo binario desde el servidor con los datos proporcionados por el parámetro datos
         * @param {Array} datos. Lista de datos a mostrar en el archivo PDF. Ver clase DNP.Backbone.Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto 
         */
        function ObtenerPdfVideos(datos){
            try {
                let url = `${urlPDFBackbone}${constantesBackbone.apiBackboneObtenerPdfVideosAyuda}`;
                return  $http.post(url, {datos : datos });
            }
            catch(excepcion){
                throw { message: `servicioCentroAyuda.ObtenerPdfTemario => ${excepcion.message}` };
            }
        }

        function obtenerListaTemasBroadcast() {

            return this.obtenerListaTemas({ tipo: 0, SoloActivos: true }).then(respuesta => {
                $rootScope.$broadcast('ListaTemasBroadcast', respuesta.data);
            })
        }





        //--------p/ mock------------//
        function obtenerJsonLocal(nombreJson) {
            var url = 'http://localhost:3024/src/assets/' + nombreJson + '.json';

            return $http({
                method: 'GET',
                'Content-Type': 'application/json;charset=utf-8',
                url: url
            });
        }
    }
})();
