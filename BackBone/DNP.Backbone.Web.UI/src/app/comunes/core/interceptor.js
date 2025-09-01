(function () {
    'use strict';

    angular.module('backbone').factory('httpInterceptor', httpInterceptor);

    httpInterceptor.$inject = ['$q', '$location', '$localStorage', '$injector', '$rootScope', 'sesionServicios', 'constantesBackbone'];

    function httpInterceptor($q, $location, $localStorage, $injector, $rootScope, sesionServicios, constantesBackbone) {
        return {
            request: function (config) {
                if (!($localStorage.interceptPromises || []).some(x => x && x.hasOwnProperty("isPending")))
                    $localStorage.interceptPromises = [];

                config.headers = config.headers || {};
                if ($localStorage.authorizationData) {
                    if (config.url.indexOf(apiBackboneServicioBaseUri) == 0) {
                        config.headers['Authorization-Type'] = 'JWT';
                        config.headers.Authorization = 'Basic ' + $localStorage.authorizationData;
                    } else {
                        config.headers.Authorization = 'Basic ' + $localStorage.oldToken;
                    }

                    //const _$htpp = $injector.get('$http');
                    //const regex = new RegExp("/Home/GetToken|api/MensajeMantenimiento/ObtenerListaMensajes|visualizarMensaje");
                    //const promisesPendientes = $localStorage.interceptPromises.some(p => p.isPending());
                    //if (!regex.test(config.url) && !promisesPendientes && _estaAutorizado()) {
                    //    let promise = _closurePromise(_obtenerMensajesMentenimientoRestritivas(_$htpp, $rootScope, sesionServicios));
                    //    $localStorage.interceptPromises.push(promise);
                    //}
                }

                return config;
            }
        };

        function _obtenerMensajesMentenimientoRestritivas($http, $rootScope, sesionServicios) {
            const idsRoles = sesionServicios.obtenerUsuarioIdsRoles();
            const idsEntidades = sesionServicios.obtenerUsuarioIdsEntidades();

            if (idsRoles && !idsRoles.length || idsEntidades && !idsEntidades.length)
                return new Promise(resolve => resolve(true));

            const peticion = {
                IdUsuarioDNP: usuarioDNP,
                Aplicacion: nombreAplicacionBackbone,
                IdsRoles: idsRoles
            };

            const filtro = {
                ParametrosDto: peticion,
                MensajeMantenimientoDto: {},
                FiltroDto: {
                    EntidadesIds: idsEntidades,
                    RolesIds: idsRoles,
                    FechaComprobacion: moment().toDate(),
                    ComprobarMensajes: true,
                    TieneRestringeAcesso: true,
                    EstadosMensajes: ['Habilitado']
                }
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaMensajes + "?noblockui", filtro)
                .then(respuesta => {
                    if(respuesta.data && respuesta.data.length)
                        $rootScope.$broadcast('MensajesMantenimientoConfirmada', respuesta.data);
                });
        }

        function _closurePromise(promise) {
            let isPending = true;
            let isRejected = false;
            let isFulfilled = false;

            const task = promise.then(
                function () {
                    isFulfilled = true;
                    isPending = false;
                    return v;
                })
                .catch(function (e) {
                    isRejected = true;
                    isPending = false;
                });

            task.isFulfilled = () => isFulfilled;
            task.isPending = () => isPending;
            task.isRejected = () => isRejected;
            return task;
        }

        function _estaAutorizado() {
            return $localStorage.authorizationData || false;
        }
    }

    angular.module('backbone').config(function ($httpProvider) {
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.interceptors.push('httpInterceptor');
    });
})();