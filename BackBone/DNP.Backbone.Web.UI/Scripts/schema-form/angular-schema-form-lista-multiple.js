
(function() {
    "use strict";

    angular.module("backbone").config([
        "schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function(schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = "listaMultiple";

            var funcion = function(name, schema, options) {
                if (schema.type === control) {
                    var f = schemaFormProvider.stdFormObj(name, schema, options);
                    f.key = options.path;
                    f.type = schema.type;
                    options.lookup[sfPathProvider.stringify(options.path)] = f;
                    return f;
                }
            }

            schemaFormProvider.defaults.string.unshift(funcion);

            schemaFormDecoratorsProvider.addMapping(
                "bootstrapDecorator",
                control,
                "/Scripts/schema-form/directives/listaMultiple.html"
            );
        }
    ]);

    angular.module("backbone").controller("listaMultipleController", ["$scope",
        function($scope) {
            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            $scope.estanTodosSeleccionados = false;
            $scope.seleccionarODeseleccionarTodo = seleccionarODeseleccionarTodo;
            $scope.obtenerValoresPorDefecto = obtenerValoresPorDefecto;
            $scope.cambioDeValor = cambioDeValor;
            $scope.inicializarLista = inicializarLista;

            function inicializarLista() {
                $scope.$watch("form.selectedValue", cambioDeValor, true);
                $scope.form.selectedValue = obtenerValoresPorDefecto();
                $scope.form.required = $scope.schema.required.indexOf($scope.form.key.slice(-1)[0]) >= 0;
            }

            function cambioDeValor(newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.model[$scope.form.key[0]] = obtenerIdentificadoresDeLista(newValue);
                    $scope.estanTodosSeleccionados = ($scope.model[$scope.form.key[0]] &&
                        $scope.model[$scope.form.key[0]].length === $scope.form.titleMap.length);
                }
            }

            function obtenerIdentificadoresDeLista(lista) {
                var resultado = null;

                if (lista && lista.length > 0) {
                    resultado = [];

                    for (var i = 0; i < lista.length; i++) {
                        resultado.push(lista[i].value);
                    }
                }

                return resultado;
            }

            function seleccionarODeseleccionarTodo($event) {
                var checkbox = $event.target;

                if ($scope.form.hasOwnProperty('titleMap')) {
                    if (checkbox.checked) {
                        $scope.form.selectedValue = $scope.form.titleMap;
                    } else {
                        $scope.form.selectedValue = [];
                    }
                }
            };

            function obtenerValoresPorDefecto() {
                var valoresPorDefectoEnFuenteDeDatos = null;

                if ($scope.model[$scope.form.key[0]]) {
                    if (esSeleccionarTodosPorDefecto($scope.model[$scope.form.key[0]])) {
                        valoresPorDefectoEnFuenteDeDatos = $scope.form.titleMap;
                    } else {
                        valoresPorDefectoEnFuenteDeDatos = $scope.form.titleMap.filter(function(elementoFuenteDeDatos) {
                            var estaElementoPorDefectoEnFuenteDeDatos = false;

                            for (var i = 0; i < $scope.model[$scope.form.key[0]].length; i++) {
                                if (elementoFuenteDeDatos.value === $scope.model[$scope.form.key[0]][i].value) {
                                    estaElementoPorDefectoEnFuenteDeDatos = true;
                                    break;
                                }
                            }

                            return estaElementoPorDefectoEnFuenteDeDatos;
                        });
                    }

                    if (valoresPorDefectoEnFuenteDeDatos.length === 0) {
                        valoresPorDefectoEnFuenteDeDatos = null;
                    }
                }

                return valoresPorDefectoEnFuenteDeDatos;
            }

            function esSeleccionarTodosPorDefecto(valoresPorDefecto) {
                var tieneSeleccionarTodosPorDefecto = false;

                for (var i = 0; i < valoresPorDefecto.length; i++) {
                    if (valoresPorDefecto[i].hasOwnProperty("todos")) {
                        tieneSeleccionarTodosPorDefecto = valoresPorDefecto[i].todos;
                        break;
                    }
                }

                return tieneSeleccionarTodosPorDefecto;
            }
        }
    ]);
})();