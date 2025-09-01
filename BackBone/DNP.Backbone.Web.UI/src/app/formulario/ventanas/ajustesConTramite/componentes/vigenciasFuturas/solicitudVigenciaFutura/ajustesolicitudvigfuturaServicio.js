(function () {
    'use strict';
    angular.module('backbone').factory('ajustesolicitudvigfuturaServicio', ajustesolicitudvigfuturaServicio);

    ajustesolicitudvigfuturaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function ajustesolicitudvigfuturaServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerDeflactores: ObtenerDeflactores,
            ObtenerProyectoTramite: ObtenerProyectoTramite,
            ActualizaVigenciaFuturaProyectoTramite: ActualizaVigenciaFuturaProyectoTramite
        };

        function ObtenerDeflactores() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerDeflactores);
        }

        function ObtenerProyectoTramite(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerProyectoTramite + "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function ActualizaVigenciaFuturaProyectoTramite(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizaVigenciaFuturaProyectoTramite;
            return $http.post(url, parametros);
        }
    }
})();