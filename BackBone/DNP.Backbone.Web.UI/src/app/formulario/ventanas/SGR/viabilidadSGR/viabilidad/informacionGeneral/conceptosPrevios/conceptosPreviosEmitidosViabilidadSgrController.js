(function () {
    'use strict';
    conceptosPreviosEmitidosViabilidadSgrController.$inject = [       
        'utilidades',
        '$sessionStorage',
        'viabilidadSgrServicio'
    ];

    function conceptosPreviosEmitidosViabilidadSgrController(      
        utilidades,
        $sessionStorage,
        viabilidadSgrServicio
    ) {

        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrviabilidadinformaciongeneralconceptospreviosemitidos';
        vm.Bpin = $sessionStorage.idObjetoNegocio;

        vm.conceptosAnteriores = {};

        vm.init = function () {
            viabilidadSgrServicio.SGR_Viabilidad_obtenerConceptosPreviosEmitidos(vm.Bpin).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.conceptosAnteriores = response.data;
                    }
                });
        };
    }

    angular.module('backbone').component('conceptosPreviosEmitidosViabilidadSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosViabilidadSgr.html",
        controller: conceptosPreviosEmitidosViabilidadSgrController,
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