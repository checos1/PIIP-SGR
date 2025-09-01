(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = "fecha";

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
                "/Scripts/schema-form/directives/fecha.html"
            );
        }
    ]);

    angular.module("backbone").controller("fechaController", ["$scope",
        function ($scope) {
            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            function constructor() {
                 

                if ($scope.form.dateBeg != "" && $scope.form.dateBeg != undefined) {
                    $scope.form.dateBeg = new Date($scope.form.dateBeg);
                    $scope.form.dateEnd = new Date($scope.form.dateEnd);

                    $scope.form.dateBeg.setDate($scope.form.dateBeg.getDate() - 1);
                    $scope.fechaInicial = $scope.form.dateBeg;
                }

                if ($scope.model[$scope.form.key.slice(-1)[0]] === null || $scope.model[$scope.form.key.slice(-1)[0]] === "" || $scope.model[$scope.form.key.slice(-1)[0]] === undefined) {
                    $scope.model[$scope.form.key.slice(-1)[0]] = undefined;
                }

                if ($scope.form.fechaActualPorDefecto) {
                    var fechaActualSinHoras = new Date(new Date().setHours(0, 0, 0, 0));
                    $scope.model[$scope.form.key.slice(-1)[0]] = fechaActualSinHoras;
                    $scope.form.id = $scope.form.title;
                }
            }

            $scope.beforeRender = function ($dates, fecIni, fecFin) {
                if (fecIni != "" && fecIni != undefined) {
                    $dates.filter(function (date) {
                        return date.utcDateValue < $scope.fechaInicial;
                    }).forEach(function (date) {
                        date.selectable = false;
                    });
                }
                if (fecFin != "" && fecFin != undefined) {
                    $dates.filter(function (date) {
                        return date.utcDateValue > fecFin.getTime();
                    }).forEach(function (date) {
                        date.selectable = false;
                    });
                }
            };

            constructor();
        }
    ]);
})();