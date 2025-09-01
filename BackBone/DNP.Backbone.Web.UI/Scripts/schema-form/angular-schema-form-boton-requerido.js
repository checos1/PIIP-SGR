(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

            var control = "boton-requerido";

            var funcion = function(name, schema, options) {
                if (schema.type === control) {
                    var f = schemaFormProvider.stdFormObj(name, schema, options);
                    f.key = options.path;
                    f.type = schema.type;
                    options.lookup[sfPathProvider.stringify(options.path)] = f;
                    return f;
                }
            };

            schemaFormProvider.defaults.string.unshift(funcion);

            schemaFormDecoratorsProvider.addMapping(
                "bootstrapDecorator",
                control,
                "/Scripts/schema-form/directives/botonRequerido.html"
            );
        }
    ]);

    angular.module("backbone").controller("botonRequeridoController", ["$scope",
        function ($scope) {
            $scope.enClick = enClick;

            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            function enClick() {
                var token = $scope.form.tokenEnviadoEnAccionClick
                    ? $scope.form.tokenEnviadoEnAccionClick
                    : $scope.form.key.slice(-1)[0];
                $scope.$emit("onClick", token);
            }
        }
    ]);
})();