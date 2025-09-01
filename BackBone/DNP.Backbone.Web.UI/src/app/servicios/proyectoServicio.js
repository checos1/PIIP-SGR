(function () {

    'use strict';

    angular.module('backbone').factory('proyectoServicio', proyectoServicio);
    proyectoServicio.$inject = ['$http', 'constantesBackbone'];

    function proyectoServicio($http, constantesBackbone) {        
        return {
            obtenerProyectos: function (peticionObtenerInbox = {}, proyectoFiltro = {}, columnasVisibles = []) {
                const filter = {
                    ProyectoParametrosDto: peticionObtenerInbox,
                    ProyectoFiltroDto: proyectoFiltro,
                    ColumnasVisibles: columnasVisibles
                };
                console.log(filter);
                return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyecto}`, filter);
            }
        };
    }
})();