(function () {
    'use strict';

    angular.module('backbone').factory('creditoServicio', creditoServicio);
    creditoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    // descripción de la columnas
    var columnasPorDefectoProyecto = ['CAPITULO', 'FECHA DESDE', 'FECHA HASTA'];

    function creditoServicio($q, $http, $location, constantesBackbone) {

        return {
            columnasPorDefectoProyecto: columnasPorDefectoProyecto,
            ObtenerCargaMasivaCreditos: ObtenerCargaMasivaCreditos,
            ValidarCargaMasivaCreditos: ValidarCargaMasivaCreditos,
            RegistrarCargaMasivaCreditos: RegistrarCargaMasivaCreditos,
        }

        function ObtenerCargaMasivaCreditos() {
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerCargaMasivaCreditos}`;
                return $http.get(url);
            }
            catch (exception) {
                throw { message: `creditoServicio.ObtenerCargaMasivaCreditos: ${exception.message}` };
            }
        }

        function ValidarCargaMasivaCreditos(lista) {
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarCargaMasivaCreditos}`;
                return $http.post(url, lista);
            }
            catch (exception) {
                throw { message: `creditoServicio.ValidarCargaMasivaCreditos: ${exception.message}` };
            }
        }

        function RegistrarCargaMasivaCreditos(lista) {
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneRegistrarCargaMasivaCreditos}`;
                return $http.post(url, lista);
            }
            catch (exception) {
                throw { message: `creditoServicio.RegistrarCargaMasivaCreditos: ${exception.message}` };
            }
        }
    }
})();