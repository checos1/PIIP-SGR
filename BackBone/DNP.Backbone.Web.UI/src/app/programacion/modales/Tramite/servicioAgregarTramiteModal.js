(function () {
    'use strict';

    angular.module('backbone').factory('servicioAgregarTramiteModal', servicioAgregarTramiteModal);
    servicioAgregarTramiteModal.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    // descripción de la columnas
    var columnasPorDefectoModalTramites = ['NOMBRE DEL TRÁMITE'];

    function servicioAgregarTramiteModal($q, $http, $location, constantesBackbone) {

        return {
            columnasPorDefectoModalTramites: columnasPorDefectoModalTramites,
            ObtenerNiveles: ObtenerNiveles,
            ObtenerTramites: ObtenerTramites,
            Guardar: Guardar,
        }

        function ObtenerNiveles(idNivelPadre, claveNivelTipo) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneNivelObtener}?idPadre=${idNivelPadre}&claveTipoNivel=${claveNivelTipo}`;
            return $http.get(url);
        }

        function ObtenerTramites(idNivel) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneFlujoTramitePorNivel}?idNivel=${idNivel}`;
            return $http.get(url);
        }

        function Guardar(programacion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionGuardar;
            return $http.post(url, programacion);
        }
    }
})();