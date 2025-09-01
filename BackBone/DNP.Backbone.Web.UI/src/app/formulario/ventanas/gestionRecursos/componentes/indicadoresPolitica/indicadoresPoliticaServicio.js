(function () {
    'use strict';

    ///////////////////////////////////////////////////////////
    /* Servicio 1 */
    ///////////////////////////////////////////////////////////

    angular.module('backbone')
        .factory('obtenerDatosIndicadoresPoliticaServicio', obtenerDatosIndicadoresPoliticaServicio);

    obtenerDatosIndicadoresPoliticaServicio.$inject = [
        '$q', '$http', 'constantesIndicadorPolitica'
    ];

    /// Obtener información de indicadores de politica.
    function obtenerDatosIndicadoresPoliticaServicio($q, $http, constantesIndicadorPolitica) {

        function obtenerDatosIndicadoresPolitica(id) {
            var url = apiBackboneServicioBaseUri + constantesIndicadorPolitica.apiBackboneObtenerDatosIP;
            var params = { bpin: id };
            return $http.get(url, { params });
            //return $q(function (resolve, reject) {
            //    setTimeout(function () {
            //        if (id) {
            //            resolve('Hello, ' + id + '!');
            //        } else {
            //            reject('Greeting is not allowed.');
            //        }
            //    }, 1000);
            //});
        }

        return { obtenerDatosIndicadoresPolitica };

        //function obtenerLocalizacionProyecto(Bpin) {
        //    return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaLocalizaciones + "?bpin=" + Bpin);
        //}

        //function obtenerListaEtapas(parametros) {
        //    var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEtapas;
        //    return $http.post(url, parametros);
        //}

    }


    ///////////////////////////////////////////////////////////
    /* Servicio 2 */
    ///////////////////////////////////////////////////////////

    angular.module('backbone')
        .factory('obtenerDatosIndicadoresPoliticaServicio2', obtenerDatosIndicadoresPoliticaServicio2);

    obtenerDatosIndicadoresPoliticaServicio2.$inject = [
        '$q', '$http', 'constantesIndicadorPolitica'
    ];

    /// Obtener información de indicadores de politica.
    function obtenerDatosIndicadoresPoliticaServicio2($q, $http, constantesIndicadorPolitica) {

        function obtenerDatosIndicadoresPolitica(id) {
            var url = apiBackboneServicioBaseUri + constantesIndicadorPolitica.apiBackboneObtenerDatosIP;
            var params = {};
            //return $http.get(url, { params });
            return $q(function (resolve, reject) {
                setTimeout(function () {
                    if (id) {
                        resolve('Hello, ' + id + '!');
                    } else {
                        reject('Greeting is not allowed.');
                    }
                }, 1000);
            });
        }

        return { obtenerDatosIndicadoresPolitica };

        //function obtenerLocalizacionProyecto(Bpin) {
        //    return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaLocalizaciones + "?bpin=" + Bpin);
        //}

        //function obtenerListaEtapas(parametros) {
        //    var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEtapas;
        //    return $http.post(url, parametros);
        //}

    }

})();