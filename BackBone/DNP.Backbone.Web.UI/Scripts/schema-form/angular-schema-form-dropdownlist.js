(function () {
    "use strict";

    angular.module("backbone").config([
        "schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlSelect;

            var dropdownlist = function (name, schema, options) {
                if (schema.type === control) {
                    var f = stdFormObj(name, schema, options);
                    f.key = options.path;
                    f.type = schema.type;
                    if (!f.titleMap) {
                        f.titleMap = schemaFormProvider.enumToTitleMap(schema['enum']);
                    }
                    options.lookup[sfPathProvider.stringify(options.path)] = f;
                    return f;
                }
            };

            schemaFormProvider.defaults.string.unshift(dropdownlist);

            schemaFormDecoratorsProvider.addMapping(
                'bootstrapDecorator',
                control,
                '/Scripts/schema-form/directives/dropdownlist.html'
            );
        }]).directive('dropdownlistdirectiva', ['$filter', '$http', function ($filter) {
            return {
                restrict: 'A',
                scope: false,
                controller: ['$scope', '$http', dropdownlistControllerFunction],
                link: function (scope, iElement, iAttrs, ngModelCtrl) {
                    scope.ngModel = ngModelCtrl;
                    scope.filter = $filter;
                }
            };
        }]);

    var dropdownlistControllerFunction = function ($scope, $http) {

        //Métodos
        $scope.finalizeTitleMap = finalizeTitleMap;
        $scope.populateTitleMap = populateTitleMap;
        $scope.refrescarSelect = refrescarSelect;
        $scope.obtenerUrl = obtenerUrl;


        //Variables
        $scope.errorCargaItems = undefined;
        $scope.form.required = $scope.schema.required.indexOf($scope.form.key.slice(-1)[0]) >= 0;

        $scope.opciones = {
            deshabilitarControles: false
        };

        this.$onInit = function () {
            $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
            //$scope.opciones.deshabilitarControles = true;
        };

        ////////////////////
        function constructor() {
            if ($scope.form.listaDependiente !== undefined && $scope.form.listaDependiente !== null && $scope.form.listaDependiente !== "") {
                if ($scope.form.listaDependiente.Id && $scope.model.hasOwnProperty($scope.form.listaDependiente.Id)) {
                    var variable = 'model.' + $scope.form.listaDependiente.Id;
                    $scope.$watch(variable, $scope.refrescarSelect);
                }
            }
            $scope.populateTitleMap($scope.form);
        }

        function finalizeTitleMap(form, data) {
            form.titleMap = [];

            if (angular.isArray(data)) {
                angular.forEach(data,
                    function (registro) {
                        if (registro.hasOwnProperty(form.valueSelect) && registro.hasOwnProperty(form.textSelect)) {
                            form.titleMap.push({ value: registro[form.valueSelect], name: registro[form.textSelect] });
                        }
                    });
            }

            cargarValoresProDefecto();
        };

        function obtenerUrl(form) {
            var url = form.urlGet;
            if (form.listaDependiente) {
                if ($scope.seleccionPadre) {
                    url += $scope.seleccionPadre.value;
                }
            }

            return url;
        }

        function populateTitleMap(form) {

            if (form.urlGet) {
                form.titleMap = [];

                if (form.listaDependiente && !$scope.seleccionPadre) {
                    return false;
                }

                var url = $scope.obtenerUrl(form);
                return $http.get(url).then(
                    function (data) {
                        asignarValorAControl(null);
                        $scope.finalizeTitleMap(form, data.data);
                    },
                    function (data, status) {
                        $scope.errorCargaItems = "Error cargando los items (URL: " + String(url) + ". Error: " + status;
                    });
            }
        };


        function refrescarSelect(newValue, oldValue) {
            $scope.seleccionPadre = newValue;
            $scope.populateTitleMap($scope.form);
        };

        function asignarValorAControl(valor) {
            if ($scope.model.hasOwnProperty($scope.form.key[0])) {
                $scope.model[$scope.form.key[0]] = valor;
            }
        }

        function cargarValoresProDefecto() {
            if ($scope.form.defaultValue && $scope.form.titleMap) {
                for (var i = 0; i < $scope.form.titleMap.length; i++) {
                    if ($scope.form.titleMap[i].value === $scope.form.defaultValue) {
                        $scope.model[$scope.form.key.slice(-1)[0]] = $scope.form.titleMap[i];
                    }
                }
            }
        }

        constructor();
    }
})();