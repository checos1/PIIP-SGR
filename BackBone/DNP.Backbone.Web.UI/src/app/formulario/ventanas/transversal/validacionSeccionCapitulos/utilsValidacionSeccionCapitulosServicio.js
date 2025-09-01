(function () {
    'use strict';
    angular.module('backbone').factory('utilsValidacionSeccionCapitulosServicio', utilsValidacionSeccionCapitulosServicio);

    utilsValidacionSeccionCapitulosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

  
    function utilsValidacionSeccionCapitulosServicio($q, $http, $location, constantesBackbone) {
        return {
            getErroresValidadosJustificacion,
            getErroresValidados,
            getSeccionCapitulo
        };

        function getErroresValidadosJustificacion(nombreComponente, erroresJson) {
            var erroresBd = erroresJson.find(p => p.Capitulo == nombreComponente);
            return getEstadoErrores(erroresBd, nombreComponente);
        }

        function getErroresValidados(nombreComponente, erroresJson) {
            var erroresBd = erroresJson.find(p => (p.Seccion + p.Capitulo == nombreComponente));
            return getEstadoErrores(erroresBd, nombreComponente);
        }
        function getEstadoErrores(erroresBd, nombreComponente) {
            var erroresActivos = [];
            var erroresJson = (erroresBd == undefined || erroresBd.Errores == "") ? [] : JSON.parse(erroresBd.Errores);
            var isValid = (erroresJson == null || erroresJson.length == 0);
            if (!isValid) {
                nombreComponente = erroresJson.errores == undefined ? nombreComponente : "errores";

                erroresJson[nombreComponente].forEach(p => {
                    erroresActivos.push({
                        Error: p.Error,
                        Descripcion: p.Descripcion,
                        Data: p.Data
                    });
                });
            }
            return {
                isValid,
                erroresActivos
            }
        }
        function getSeccionCapitulo(guidMacroproceso,NivelId,FlujoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosByMacroproceso;
            url += "?guiMacroproceso=" + guidMacroproceso;
            url += "&NivelId=" + NivelId;
            url += "&FlujoId=" + FlujoId;
            return $http.get(url);
        }
    }
})();