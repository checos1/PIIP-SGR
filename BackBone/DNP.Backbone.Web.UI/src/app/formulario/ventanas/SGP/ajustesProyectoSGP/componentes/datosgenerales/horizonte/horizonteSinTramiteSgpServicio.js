(function () {
    'use strict';
    angular.module('backbone').factory('horizonteSinTramiteSgpServicio', horizonteSinTramiteSgpServicio);

    horizonteSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function horizonteSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {            
            ObtenerHorizonte: ObtenerHorizonte,
            actualizarHorizonte: actualizarHorizonte,
            obtenerCambiosFirme: obtenerCambiosFirme
        };
        
        function ObtenerHorizonte(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerHorizonteSgp;
            return $http.post(url, parametros);
        }

        function actualizarHorizonte(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarHorizonteSGP;
            return $http.post(url, parametros);
        }

        function obtenerCambiosFirme(idProyecto, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiCambiosFirmeObtenerJustificacionHorizonteSGP;
            url += "?idProyecto=" + idProyecto;
            return $http.get(url);
        }
    }
})();
