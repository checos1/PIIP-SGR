(function() {

    /// <summary>
    /// s this instance.
    /// </summary>
    /// <returns></returns>
// ReSharper disable once UseOfImplicitGlobalInFunctionScope
    var serviciosBackbone = angular.module('mocks.tests', []);


// ReSharper disable once PossiblyUnassignedProperty
    serviciosBackbone.factory('servicioPanelPrincipalMock',
        [
            '$http',
            '$q',
            function ($http, $q) {

                var result =
                {
                    data: {
                        Mensaje: "No existen datos",
                        GruposEntidades: []
                    }
                };

                function obtenerInbox() {
                    var deferred = $q.defer();
                    deferred.resolve(result);
                    return deferred.promise;
                };

                var servicioPanelPrincipalMockFactory = {};

                servicioPanelPrincipalMockFactory.obtenerInbox = obtenerInbox;
                //servicioPanelPrincipalMockFactory.activarDestinatarios = activarDestinatarios;

                return servicioPanelPrincipalMockFactory;

            }
        ]);


    serviciosBackbone.factory('servicioVisualizacionFormularioMock',
        [
            '$http',
            '$q',
            function ($http, $q) {

                var result =
                {
                    data: {
                        Mensaje: "No existen datos"
                    }
                };

                function obtenerFormularioPorId() {
                    var deferred = $q.defer();
                    deferred.resolve(result);
                    return deferred.promise;
                };
                return {

                    formulario: obtenerFormularioPorId
                }

            }
        ]);




})();