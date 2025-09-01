(function () {
    'use strict';

    angular.module('backbone.formulario').directive('jerarquiaWbs', jerarquiaWbs).filter('orderObjectBy', orderObjectBy);

    jerarquiaController.$inject = ['wbsServicio', 'templatesServicio', '$timeout', '$filter', '$scope', 'utilidades', '$localStorage'];

    function jerarquiaController(wbsServicio, templatesServicio, $timeout, $filter, $scope, utilidades, $localStorage) {
        var vm = this;

        vm.mostrarAdicionar = mostrarAdicionar;
        vm.templatesServicio = templatesServicio;
        vm.eliminarDato = eliminarDato;
        vm.totalizar = totalizar;
        vm.determinaFocoPibot = determinaFocoPibot;

        vm.nuevaJerarquia = {};
        vm.propiedadesTotalizarJerarquia = {};

        this.$onInit = function () {
            vm.modelo.mostrarAdicionar = false;

            return $timeout(function () {
                totalizar();
                if (vm.datos && Object.keys(vm.datos).length > 0) {
                    if (Array.isArray(vm.datos)) {
                        _.each(vm.datos, function (dato) {
                            if (!dato.hasOwnProperty("colapsarPanelWBS"))
                                dato.colapsarPanelWBS = true;
                        });
                    } else {
                        if (!vm.datos.hasOwnProperty("colapsarPanelWBS"))
                            vm.datos.colapsarPanelWBS = true;
                    }
                }
            }, 1000);
        }

        $scope.$on("totalizar",
            function (evento) {
                evento.stopPropagation();
                totalizar();
                $scope.$apply();

            });

        function determinaFocoPibot(dato) {
            if (dato.hasOwnProperty("colapsarPanelWBS") && dato.hasOwnProperty("Filtrar"))
                return dato.colapsarPanelWBS && dato.Filtrar;
            else if (dato.hasOwnProperty("colapsarPanelWBS"))
                return dato.colapsarPanelWBS;
            else if (dato.hasOwnProperty("Filtrar"))
                return dato.Filtrar;
        }
                
        function totalizar() {
            //if ((vm.datos['MostrarNoId'] != undefined) || (vm.datos['MostrarId'] != undefined)) {
            //    if (vm.datos['MostrarNoId'] != undefined) {
            //        $localStorage.GruposRecursosNoId = vm.datos['MostrarNoId'];
            //        $localStorage.GruposRecursosId = null;
            //    }
            //    else if (vm.datos['MostrarId'] != undefined) {
            //        $localStorage.GruposRecursosId = vm.datos['MostrarId'];
            //        $localStorage.GruposRecursosNoId = null;
            //    }
            //}
            angular.forEach(vm.modelo.items, function (value) {
                if (value.visible) {
                    vm.propiedadesTotalizarJerarquia[value.propiedad] = {
                        total: 0,
                        totalizar: (value.totalizar &&
                            (['decimal', 'int', 'integer', 'number', 'double'].indexOf(value.tipo) > -1)),
                        propiedadAMostrar: value.propiedad,
                        tipo: value.tipo
                    };
                }

            });
            angular.forEach(vm.datos, function (value, atributo) {
                var tipoDato = tipoDeAtributo(value);
                if (tipoDato === "array") {
                    value.forEach(function (valor) {
                        angular.forEach(valor, function (valor, propiedad) {
                            if (vm.propiedadesTotalizarJerarquia.hasOwnProperty(propiedad) && vm.propiedadesTotalizarJerarquia[propiedad].totalizar) {
                                if (isNaN(valor)) {
                                    valor = 0;
                                } else if (!valor) {
                                    valor = 0;
                                }

                                switch (vm.propiedadesTotalizarJerarquia[propiedad].tipo) {
                                    case 'number':
                                        valor = Number(valor); break;
                                    case 'integer':
                                        valor = parseInt(valor, 10); break;
                                }

                                vm.propiedadesTotalizarJerarquia[propiedad].total += valor;
                            }
                        });
                    });
                }
            });
        }

        vm.filtrar = function (nodo) {
            if (!nodo.hasOwnProperty('Filtrar')) {
                return true;
            }
            else {
                return nodo.Filtrar;
            }
        }

        vm.pibot = function (nodo, prop) {
            var pibot = false;
            if (nodo && nodo.items) {
                nodo.items.forEach(function (item) {
                    if (item.pibot)
                        pibot = true;
                })
            }
            if (pibot && prop) {
                var datos = vm.datos[prop];
                var colapsarPanelWBS = false;
                datos.forEach(function (item) {
                    if (item.colapsarPanelWBS && !colapsarPanelWBS && vm.filtrar(item))
                        colapsarPanelWBS = true;
                    else
                        item.colapsarPanelWBS = false;
                })

                if (!colapsarPanelWBS)
                    vm.datos[prop][0].colapsarPanelWBS = true;
            }

            return pibot;
        }

        vm.colapsarPanelWBS = function (dato, datos) {
            datos.forEach(function (item) {
            //    console.log("item", item)
                item.colapsarPanelWBS = false;
            })
            dato.colapsarPanelWBS = !dato.colapsarPanelWBS;
        }


        vm.existeHijosConFiltro = function (arrayDatos) {
            for (var i = 0; i < arrayDatos.length; i++) {
                if (!arrayDatos[i].hasOwnProperty('Filtrar')) {
                    return true;
                }
                else {
                    if (arrayDatos[i].Filtrar === true) {
                        return true;

                    }
                }

            }
            return false;
        }

        function tipoDeAtributo(obj) {
            return ({}).toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
        }


        function eliminarDato(dato, propiedad) {
            utilidades.mensajeWarning($filter('language')('ConfirmarEliminarRegistro'),
                function () {
                    if (vm.datos &&
                        vm.datos[propiedad] &&
                        vm.datos[propiedad].length > 0) {

                        var indice = vm.datos[propiedad].indexOf(dato);
                        if (indice != -1)
                            vm.datos[propiedad].splice(indice, 1);
                        totalizar();
                        $scope.$apply();
                    }
                });
        }

        function mostrarAdicionar() {
            if (vm.datos[vm.modelo.propiedad] === undefined || vm.datos[vm.modelo.propiedad] === null) vm.datos[vm.modelo.propiedad] = [];
            vm.datos[vm.modelo.propiedad].mostrarAdicionar = true;
        }

    }


    function orderObjectBy() {
        return function (items, field, reverse) {
            var filtered = [];
            angular.forEach(items, function (item) {
                filtered.push(item);
            });
            filtered.sort(function (a, b) {
                return (a[field] > b[field] ? 1 : -1);
            });
            if (reverse) filtered.reverse();
            return filtered;
        };
    };


    function jerarquiaWbs() {
        return {
            restrict: 'E',
            scope: {
                modelo: '=',
                datos: '=',
                idPadre: '@',
                opciones: '<',
                datosMaster: "=",
                schema: "="
            },
            templateUrl: '/src/app/formulario/componentes/WBS/directivas/jerarquia/jerarquia.template.html',
            controller: jerarquiaController,
            controllerAs: 'vm',
            bindToController: true
        };
    }
})();