(function () {
    'use strict';
    conceptosPreviosEmitidosCtusSgrController.$inject = [
        'utilidades',
        '$sessionStorage'];

    function conceptosPreviosEmitidosCtusSgrController(
        utilidades,
        $sessionStorage
    ) {

        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctuselaboracioninfogeneralconceptospreviosemitidos';

        vm.TipoConcepto = 2;

        vm.init = function () {
        };
    }

    angular.module('backbone').component('conceptosPreviosEmitidosCtusSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctus/elaboracion/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosCtusSgr.html",
        controller: conceptosPreviosEmitidosCtusSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
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