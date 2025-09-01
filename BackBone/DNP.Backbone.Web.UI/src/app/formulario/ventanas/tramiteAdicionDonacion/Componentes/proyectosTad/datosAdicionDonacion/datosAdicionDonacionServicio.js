(function () {
    'use strict';
    angular.module('backbone').factory('datosAdicionDonacionServicio', datosAdicionDonacionServicio);

    datosAdicionDonacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function datosAdicionDonacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerDatosAdicionDonacion: ObtenerDatosAdicionDonacion,
            ObtenerSectores: ObtenerSectores,
            ObtenerListaEntidades: ObtenerListaEntidades,
            GuardarDatosAdicionDonacion: GuardarDatosAdicionDonacion,
            eliminarDatosAdicionDonacion: eliminarDatosAdicionDonacion
        };

        function ObtenerDatosAdicionDonacion(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosIncorporacion + "?tramiteId=" + tramiteId;
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

        function GuardarDatosAdicionDonacion(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatosIncorporacion + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function eliminarDatosAdicionDonacion(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEiliminarDatosIncorporacion + "?usuario=" + usuarioDNP;
            return $http.post(url, parametros);
        }

    }
})();