(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

            var control = "inputaccion";

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
                "/Scripts/schema-form/directives/inputAccion.html"
            );
        }
    ]);

    angular.module("backbone").controller("inputAccionController", ["$scope",
        function ($scope) {

            function constructor() {
                var accion = $scope.form.accion;
            }

            $scope.clickInput = function () {
                $scope.$emit("clickBotonPanel", $scope.form.accion);
            }

            $scope.eliminarInput = function () {
                $scope.$emit("clickBotonPanel", $scope.form.accionEliminar);
            }

            

            constructor();
        }
    ]);
})();