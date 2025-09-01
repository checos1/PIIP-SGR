(function () {
    'use strict';

    angular.module('backbone').factory('pestanasFormularioServicio', pestanasFormularioServicio);

    pestanasFormularioServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function pestanasFormularioServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerSeccionesTramite: obtenerSeccionesTramite,
        };

        function obtenerSeccionesTramite(idMacroproceso, idInstancia, idFaseNivel) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerSeccionesTramite;
            url += "?IdMacroproceso=" + idMacroproceso;
            url += "&IdInstancia=" + idInstancia;
            url += "&IdFase=" + idFaseNivel;
            return $http.get(url);
        }
    }
})();