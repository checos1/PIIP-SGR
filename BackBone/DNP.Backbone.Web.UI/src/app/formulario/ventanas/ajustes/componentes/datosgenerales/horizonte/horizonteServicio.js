(function () {
    'use strict';
    angular.module('backbone').factory('horizonteServicio', horizonteServicio);

    horizonteServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function horizonteServicio($q, $http, $location, constantesBackbone) {
        return {

            // ObtenerPreguntasProyectoActualizacion: ObtenerPreguntasProyectoActualizacion,
            ObtenerHorizonte: ObtenerHorizonte,
            actualizarHorizonte: actualizarHorizonte,
            obtenerCambiosFirme: obtenerCambiosFirme
        };

        //falta crear el metodo que trae la informacion api/proyecto/ObtenerEncabezadoGeneral
        function ObtenerHorizonte(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEncabezadoGeneral;
            return $http.post(url, parametros);

        }

        function actualizarHorizonte(parametros) {
            //var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneActualizarHorizonte}` + '?idProyecto=' + idProyecto + '&vigenciaInicial=' + vigenciaInicial + '&vigenciaFinal=' + vigenciaFinal + '&mantiene=' + mantiene;
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarHorizonte;
            return $http.post(url, parametros);
        }

        function obtenerCambiosFirme(idProyecto, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiCambiosFirmeObtenerJustificacionHorizonte;
            url += "?idProyecto=" + idProyecto;
            return $http.get(url);
        }


    }
})();
