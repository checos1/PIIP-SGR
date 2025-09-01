(function () {
    'use strict';

    angular.module('backbone').factory('cuotaServicio', cuotaServicio);
    cuotaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    // descripción de la columnas
    var columnasPorDefectoProyecto = ['CAPITULO', 'FECHA DESDE', 'FECHA HASTA'];

    function cuotaServicio($q, $http, $location, constantesBackbone) {

        return {
            columnasPorDefectoProyecto: columnasPorDefectoProyecto,
            ObtenerCargaMasivaCuotas: ObtenerCargaMasivaCuotas,
            ValidarCargaMasivaCuotas: ValidarCargaMasivaCuotas,
            RegistrarCargaMasivaCuotas: RegistrarCargaMasivaCuotas,
        }

        function ObtenerCargaMasivaCuotas() {
            try {
                let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCargaMasivaCuotas + '?Vigencia=2023&EntityTypeCatalogOptionId=0';
                return $http.get(url);
            }
            catch (exception) {
                throw { message: `cuotaServicio.ObtenerCargaMasivaCuotas: ${exception.message}` };
            }
        }

        function ValidarCargaMasivaCuotas(lista) {
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarCargaMasivaCuotas}`;
                return $http.post(url, lista);
            }
            catch (exception) {
                throw { message: `cuotaServicio.ValidarCargaMasivaCuotas: ${exception.message}` };
            }
        }

        function RegistrarCargaMasivaCuotas(lista) {
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneRegistrarCargaMasivaCuotas}`;
                return $http.post(url, lista);
            }
            catch (exception) {
                throw { message: `cuotaServicio.RegistrarCargaMasivaCuotas: ${exception.message}` };
            }
        }
    }
})();