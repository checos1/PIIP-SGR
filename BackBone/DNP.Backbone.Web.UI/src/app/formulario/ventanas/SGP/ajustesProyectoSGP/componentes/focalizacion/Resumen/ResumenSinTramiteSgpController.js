(function () {
    'use strict';

    ResumenSinTramiteSgpController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'focalizacionAjustesSinTramiteSgpServicio', 'justificacionCambiosServicio'
    ];

    function ResumenSinTramiteSgpController(
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
        vm.nombreComponente = "focalizacionpolsgpresumendefocalisintramitesgp";
        vm.listaPoliticasCategorias = null;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.componentesRefresh = [
            'focalizacionpolsgppoliticastransvsintramitesgp ',
            'focalizacionpolsgpcategoriapoliticassintramitesgp',
            'focalizacionpolsgpresumendefocalisintramitesgp'
        ];
        vm.seccionCapitulo = null;

        /*Carga Masiva*/
        vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ObtenerTotalFilas = function (politica) {

            let totalPorPolitica = 0;

            for (let categoria of politica.Categorias) {
                let totalPorCategoria = 0;
                if (categoria.SubCategorias && categoria.SubCategorias.length > 0) {
                    totalPorPolitica = totalPorPolitica + categoria.SubCategorias.length;
                    totalPorCategoria = totalPorCategoria + categoria.SubCategorias.length;
                }

                categoria.totalFilas = totalPorCategoria;
            }

            politica.totalFilas = total;

            return politica;
        }

        function init() {
            //vm.permiteEditar = false;
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            vm.obtenerPoliticasTransversalesResumen(vm.BPIN);
        }


        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.obtenerPoliticasTransversalesResumen(vm.BPIN);
            }

        }
        vm.obtenerPoliticasTransversalesResumen = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;

            return focalizacionAjustesSinTramiteSgpServicio.ObtenerPoliticasTransversalesResumen($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.listaPoliticasCategorias = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        for (let politica of vm.listaPoliticasCategorias.Politicas) {
                            politica = vm.ObtenerTotalFilas(politica);
                        }

                        console.log(vm.listaPoliticasCategorias);
                    }
                });
        }

        function ConvertirNumero2decimales(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        /*Carga Masiva*/

        /* ------------------------ Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('resumenSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/Resumen/ResumenSinTramiteSgp.html",
        controller: ResumenSinTramiteSgpController,
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