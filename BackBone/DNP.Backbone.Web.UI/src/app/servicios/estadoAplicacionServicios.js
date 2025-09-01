(function (idTipoProyecto) {
    'use strict';

    angular.module('backbone').factory('estadoAplicacionServicios', estadoAplicacionServicios);

    estadoAplicacionServicios.$inject = ['$rootScope'];

    function estadoAplicacionServicios($rootScope) {

        var tipoObjetoSeleccionado = {
            Id: idTipoProyecto,
            Nombre: 'Proyecto'
        }

        return {
            tipoObjetoSeleccionado: tipoObjetoSeleccionado,
            emitirEvento: emitirEvento
        }

        ////////////

        function emitirEvento(evento, cuerpo) {
            $rootScope.$broadcast(evento, cuerpo);
        }
    }
})(idTipoProyecto);