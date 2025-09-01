(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlTextoLargo;

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
                "/Scripts/schema-form/directives/textoLargo.html"
            );
        }
    ]);

    angular.module("backbone").controller("textoLargoController", ["$scope",
        function ($scope) {
            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            function constructor() {
                var propiedades = $scope.schema.properties[$scope.form.key.slice(-1)[0]];
                if (propiedades.maxLength) {
                    $scope.form.maxLength = propiedades.maxLength;    
                }
                $scope.form.pattern = propiedades.pattern;
                $scope.form.required = $scope.schema.required.indexOf($scope.form.key.slice(-1)[0]) >= 0;
            }

            constructor();
        }
    ]);
})();