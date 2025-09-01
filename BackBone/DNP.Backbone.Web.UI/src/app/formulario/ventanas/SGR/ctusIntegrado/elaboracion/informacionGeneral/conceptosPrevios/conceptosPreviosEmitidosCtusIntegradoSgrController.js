(function () {
    'use strict';
    conceptosPreviosEmitidosCtusIntegradoSgrController.$inject = ['utilidades','$sessionStorage' ];

    function conceptosPreviosEmitidosCtusIntegradoSgrController(
        utilidades,
        $sessionStorage
    ) {

        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusintegradoelaboracioninfogeneralconceptospreviosemitidos';

        vm.TipoConcepto = 3;

        vm.init = function () {
        };
    }

    angular.module('backbone').component('conceptosPreviosEmitidosCtusIntegradoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosCtusIntegradoSgr.html",
        controller: conceptosPreviosEmitidosCtusIntegradoSgrController,
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