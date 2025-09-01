(function () {
    'use strict';
    angular.module('backbone').factory('tramiteCargueMasivoServicio', tramiteCargueMasivoServicio);

    tramiteCargueMasivoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteCargueMasivoServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            obtenerErroresTramite: obtenerErroresTramite
        };


        function obtenerErroresTramite(guiMacroproceso, idInstancia, accionid, tieneCDP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresTramite;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idInstancia=" + idInstancia;
            url += "&accionid=" + accionid;
            if (tieneCDP === undefined) {
                url += "&tieneCDP=false";
            }
            else {
                url += "&tieneCDP=" + tieneCDP;
            }
            return $http.get(url);
        }

    }
})();