(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlWbs;

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
                "/Scripts/schema-form/directives/wbs.html"
            );
        }
    ]);

    angular.module("backbone").controller("WbsController", ["$scope", "$http",
        function ($scope, $http) {
            var vm = this;

            vm.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                vm.modelo = $scope.form.configuracion;
                vm.url = $scope.form.urlServicioWbs;
                vm.schema = $scope.schemaForm.schema;
                vm.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //vm.opciones.deshabilitarControles = true;

                if ($scope.model.Bpin || $scope.model.BPIN)
                    vm.datos = $scope.model;
            }
        }
    ]);
})();