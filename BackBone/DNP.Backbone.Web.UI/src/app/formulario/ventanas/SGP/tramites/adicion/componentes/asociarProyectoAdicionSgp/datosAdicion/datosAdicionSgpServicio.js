(function () {
    'use strict';
    angular.module('backbone').factory('datosAdicionSgpServicio', datosAdicionSgpServicio);

    datosAdicionSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function datosAdicionSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerDatosAdicionSgp: ObtenerDatosAdicionSgp,
            ObtenerSectores: ObtenerSectores,
            ObtenerListaEntidades: ObtenerListaEntidades,
            GuardarDatosAdicionSgp: GuardarDatosAdicionSgp,
            eliminarDatosAdicionSgp: eliminarDatosAdicionSgp
        };

        function ObtenerDatosAdicionSgp(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosAdicionSgp + "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function ObtenerSectores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectoresEntity;
            return $http.post(url, parametros);
        }

        function ObtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
            return $http.post(url, peticionobtenerProyectos);
        }

        function GuardarDatosAdicionSgp(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatosAdicionSgp + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function eliminarDatosAdicionSgp(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEiliminarDatosAdicionSgp + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

    }
})();