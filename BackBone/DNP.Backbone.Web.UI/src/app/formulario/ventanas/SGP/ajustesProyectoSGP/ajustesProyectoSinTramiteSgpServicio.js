(function () {
    'use strict';
    angular.module('backbone').factory('ajustesProyectoSinTramiteSgpServicio', ajustesProyectoSinTramiteSgpServicio);

    ajustesProyectoSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function ajustesProyectoSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerTokenMGA: obtenerTokenMGA,
            validarCapitulos: validarCapitulos,
            obtenerErroresProyecto: obtenerErroresProyecto,
        };
        function obtenerTokenMGA(parametros, tipoUsuario) {
            return $http.get(`${apiBackboneServicioBaseUri}${constantesBackbone.apiObtenerTokenMGA}?bpin=${parametros}&tipoUsuario=${tipoUsuario}`);
        }
        function validarCapitulos(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneValidarCapitulos;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }
                
    }
})();