(function () {
    'use strict';

    angular.module('backbone').factory('modalActualizaEstadoAjusteProyectoServicio', modalActualizaEstadoAjusteProyectoServicio);

    modalActualizaEstadoAjusteProyectoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function modalActualizaEstadoAjusteProyectoServicio($q, $http, $location, constantesBackbone) {
        return {
            actualizaEstadoAjusteProyecto: actualizaEstadoAjusteProyecto,

        };

        function actualizaEstadoAjusteProyecto(tipoDevolucion,objetoNegocioId, tramiteId, observacion) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneActualizarEstadoAjusteProyecto}` + '?tipoDevolucion=' + tipoDevolucion + '&ObjetoNegocioId=' + objetoNegocioId + '&tramiteId=' + tramiteId + '&observacion=' + observacion ;
            return $http.post(url);//, ObjetoNegocioId, TramiteId, Observacion);


        }

        
    }


    }) ();