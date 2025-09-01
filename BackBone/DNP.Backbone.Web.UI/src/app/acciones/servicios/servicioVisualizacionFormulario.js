(function () {
    'use strict';
    angular.module('backbone').factory('servicioVisualizacionFormulario', servicioVisualizacionFormulario);

    servicioVisualizacionFormulario.$inject = ['$http', 'appSettings', 'constantesBackbone'];

    function servicioVisualizacionFormulario($http, appSettings, constantesBackbone) {
        return {

            formulario: obtenerFormularioPorId,
            obtenerDatosServiciosFormulario: obtenerDatosServiciosFormulario,
            obtenerServicio: obtenerServicio,
            ejecutarServicio: ejecutarServicio

        }

        function obtenerFormularioPorId(idFormulario) {
            return $http.get(apiPiipCore + constantesBackbone.apiPiipCoreObtenerFormularioPorIdUri + idFormulario);
        }
        
        function obtenerDatosServiciosFormulario(url, bpin, idNivel, idInstancia, idAccion, idAplicacion, idFormulario) {
            var configuracion = {
                headers: {
                    'piip-idInstanciaFlujo': idInstancia,
                    'piip-idAccion': idAccion,
                    'piip-idAplicacion': idAplicacion,
                    'piip-idFormulario': idFormulario
                }
            }
            var urlServices = url + '?bpin=' + bpin + '&IdNivel=' + idNivel;
            return $http.get(urlServices, configuracion);
        }

        function obtenerServicio(idServicioCustom) {

            return $http.get(apiPiipCore + constantesBackbone.apiPiipCoreObtenerServicioCustomPorIdUri + idServicioCustom);
        }

        function parametrosATexto(parametros) {
            var parametroTexto = '';
            for (var propiedad in parametros) {
                if (parametroTexto.length > 0)
                    parametroTexto = parametroTexto + ',';

                parametroTexto = parametroTexto + "'" + parametros[propiedad] + "'";
            };

            return parametroTexto;
        }

        function ejecutarServicio(datosServicio) {
            var req = {
                method: datosServicio.metodo,
                url: datosServicio.url
            };

            if (datosServicio.metodo !== 'PROCEDURE') {

                if (datosServicio.metodo == 'GET') {

                    req.params = datosServicio.parametros;

                } else {
                    req.data = datosServicio.parametros;
                }

            } else {

                var urlProcedure = apiPiipCore + constantesBackbone.apiPiipCoreEjecutarProcedimientoUri;

                var nuevosParametros = {
                    procedimiento: datosServicio.url,
                    parametros: parametrosATexto(datosServicio.parametros)
                    
                };

              //  console.log(datosServicio.parametros);
                req.method = 'GET';
                req.url = urlProcedure;
                req.params = nuevosParametros;

            }

            return $http(req);
        }
    }
})();