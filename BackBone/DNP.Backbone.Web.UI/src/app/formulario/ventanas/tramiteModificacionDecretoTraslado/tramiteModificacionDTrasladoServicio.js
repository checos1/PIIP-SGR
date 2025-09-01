(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('tramiteModificacionDTrasladoServicio', tramiteModificacionDTrasladoServicio);

    tramiteModificacionDTrasladoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteModificacionDTrasladoServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            obtenerErroresProgramacion: obtenerErroresProgramacion,
        };

        function obtenerErroresProgramacion(idInstancia, accionid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProgramacion;
            url += "?IdInstancia=" + idInstancia;
            url += "&accionid=" + accionid;
            return $http.get(url);
        }
    }

})();