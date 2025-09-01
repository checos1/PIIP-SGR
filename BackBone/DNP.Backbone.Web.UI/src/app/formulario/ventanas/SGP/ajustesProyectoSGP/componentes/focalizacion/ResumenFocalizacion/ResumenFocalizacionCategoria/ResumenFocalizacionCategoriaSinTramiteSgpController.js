(function () {
    'use strict';

    ResumenFocalizacionCategoriaSinTramiteSgpController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'focalizacionAjustesSinTramiteSgpServicio', 'justificacionCambiosServicio'
    ];

    function ResumenFocalizacionCategoriaSinTramiteSgpController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        focalizacionAjustesSinTramiteSgpServicio,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.nombreComponente = "focalizacionpolsgpcategoriapoliticassintramitesgp";

        vm.BPIN = $sessionStorage.idObjetoNegocio;

        function init() {
            //vm.permiteEditar = false;
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            console.log('entro categorias');

        }
    }

    angular.module('backbone').component('resumenFocalizacionCategoriaSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaSinTramiteSgp.html",
        controller: ResumenFocalizacionCategoriaSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&'
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
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();