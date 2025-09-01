(function () {
    'use strict';
    conceptosPreviosEmitidosSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'conceptosPreviosEmitidosSgrServicio'
    ];

    function conceptosPreviosEmitidosSgrController(
        utilidades,
        $sessionStorage,
        conceptosPreviosEmitidosSgrServicio
    ) {

        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.Bpin = $sessionStorage.idObjetoNegocio;

        vm.conceptosAnteriores = {};

        vm.mensaje1 = "";
        vm.mensaje2 = ""

        vm.init = function () {

            if (vm.tipoconcepto == 1) {
                vm.mensaje1 = "1. Solicitud y emisión de concepto de Viabilidad"
                vm.mensaje2 = "2. Conceptos de Viabilidad previos emitidos"
            } else if (vm.tipoconcepto == 2) {
                vm.mensaje1 = "1. Solicitud y emisión de CTUS"
                vm.mensaje2 = "2. CTUS previos emitidos"
            } else if (vm.tipoconcepto == 3) {
                vm.mensaje1 = "1. Solicitud y emisión de Concepto Integrado"
                vm.mensaje2 = "2. Conceptos Integrados previos emitidos"
            }

            conceptosPreviosEmitidosSgrServicio.SGR_obtenerConceptosPreviosEmitidos(vm.Bpin, vm.tipoconcepto).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.conceptosAnteriores = response.data;
                    }
                });
        };
    }

    angular.module('backbone').component('conceptosPreviosEmitidosSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/comun/conceptosPrevios/conceptosPreviosEmitidosSgr.html",
        controller: conceptosPreviosEmitidosSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<',
            tipoconcepto: '@'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();