(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = "listaSeleccionMultiple";

            var funcion = function (name, schema, options) {
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
                "/Scripts/schema-form/directives/listaSeleccionMultiple.html"
            );
        }
    ]);

    angular.module("backbone").controller("controladorListaSeleccionMultiple", ["$scope",
        function ($scope) {
            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            $scope.estanTodosSeleccionados = false;
            $scope.form.valor = [];

            $scope.obtenerValoresPorDefecto = obtenerValoresPorDefecto;
            $scope.cambioValorCheckbox = cambioValorCheckbox;
            $scope.cambioValorRadio = cambioValorRadio;
            $scope.estaSeleccionado = estaSeleccionado;
            $scope.constructor = constructor;

            function constructor() {
                $scope.model[$scope.form.key.slice(-1)[0]] = obtenerValoresPorDefecto();
                $scope.form.valor = $scope.model[$scope.form.key.slice(-1)[0]];
            }

            function obtenerValoresPorDefecto() {
                var valoresPorDefectoEnFuenteDeDatos = [];
                var valoresPorDefecto = $scope.form.defaultValue;

                if (valoresPorDefecto) {
                    if (valoresPorDefecto instanceof Array) {
                        if (esSeleccionarTodosPorDefecto(valoresPorDefecto)) {
                            for (var j = 0; j < $scope.form.titleMap.length; j++) {
                                valoresPorDefectoEnFuenteDeDatos.push($scope.form.titleMap[j].value);
                            }
                        } else {
                            for (var i = 0; i < valoresPorDefecto.length; i++) {
                                for (var j = 0; j < $scope.form.titleMap.length; j++) {
                                    if (valoresPorDefecto[i].value === $scope.form.titleMap[j].value) {
                                        valoresPorDefectoEnFuenteDeDatos.push($scope.form.titleMap[j].value);
                                    }
                                }
                            }
                        }
                    } else {
                        valoresPorDefectoEnFuenteDeDatos = valoresPorDefecto;
                    }
                }

                return valoresPorDefectoEnFuenteDeDatos;
            }

            function cambioValorCheckbox(nombreCheckbox) {
                var posicion = -1;
                var modelo = $scope.model[$scope.form.key.slice(-1)[0]];

                if (!modelo) {
                    modelo = [];
                } else {
                    posicion = modelo.indexOf(nombreCheckbox);
                }

                if (posicion > -1) {
                    modelo.splice(posicion, 1);
                } else {
                    modelo.push(nombreCheckbox);
                }

                $scope.model[$scope.form.key.slice(-1)[0]] = modelo;
            };

            function cambioValorRadio(valor) {
                $scope.model[$scope.form.key.slice(-1)[0]] = [valor];
            }

            function estaSeleccionado(nombreCheckbox) {
                var resultado = false;

                if ($scope.model[$scope.form.key.slice(-1)[0]]) {
                    resultado = $scope.model[$scope.form.key.slice(-1)[0]].indexOf(nombreCheckbox) > -1;
                }

                return resultado;
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

            constructor();
        }
    ]);
})();