(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlBoton;

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
                "/Scripts/schema-form/directives/boton.html"
            );
        }
    ]);

    angular.module("backbone").controller("botonController", ["$scope", "ejecutorReglasServicios", '$uibModal', 'formularioServicios',
    'archivoServicios', "servicioFichasProyectos", "FileSaver", "constantesAcciones", '$sessionStorage', "$q", "utilidades", "$filter",
        function ($scope, ejecutorReglasServicios, $uibModal, formularioServicios,
        		  archivoServicios, servicioFichasProyectos, FileSaver, constantesAcciones, 
                  $sessionStorage, $q, utilidades, $filter) {

            $scope.opciones = {
                deshabilitarControles: false
            };

            this.$onInit = function () {
                if ($scope.form.fichaPlantilla != undefined) {
                    var ficha = JSON.parse($scope.form.fichaPlantilla);

                    $sessionStorage.Ficha = ficha;
                    
                    $sessionStorage.fichaPlantilla = {
                        NombreReporte: ficha.Nombre,
                        PARAM_BORRADOR: false,
                        PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                        PARAM_INSTANCIA : $sessionStorage.idInstancia,
                        PARAM_FORMULARIO: $sessionStorage.idFormulario,
                        PARAM_ACCION: $sessionStorage.idAccion
                    };
                }

                $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
                //$scope.opciones.deshabilitarControles = true;
            };

            function constructor() {
                if ($scope.form.buttonTypeCustom2) {
                    if ($scope.model !== undefined) {
                        //$scope.form.modeloServicioPreguntas[$scope.form.condicion.expresion1.valor] = $scope.model[$scope.form.condicion.expresion1.valor];                        
                        logicaBotonCustom2();
                    }
                }
            }

            function base64ToArrayBuffer(base64) {
                var binaryString = window.atob(base64);
                var binaryLen = binaryString.length;
                var bytes = new Uint8Array(binaryLen);
                for (var i = 0; i < binaryLen; i++) {
                    var ascii = binaryString.charCodeAt(i);
                    bytes[i] = ascii;
                }
                return bytes;
            }
           
            $scope.onbuttonClick = function () {
                if ($scope.form.buttonTypeCustom === true) {
                    $scope.$emit('onbuttonTypeCustomClick', $scope.form);
                }
                if ($scope.form.buttonTypeReturn === true) {
                    $scope.$emit('onbuttonTypeReturnClick', 'On button Type Return Click');
                }
                if ($scope.form.buttonTypeSave === true) {
                    $scope.$emit('onbuttonTypeSaveClick', 'On button Type Save Click');
                }
                if ($scope.form.buttonTypeSearch === true) {
                    $scope.$emit('onbuttonTypeSearchClick', 'On Button Type Search Click');
                }
                if ($scope.form.buttonTypeNext === true) {
                    $scope.$emit('onbuttonTypeNextClick', 'On Button Type Next Click');
                }
                if($scope.form.buttonTypeFicha === true){
                    $scope.$emit('onbuttonTypeFichaClick', $scope.form.fichaPlantilla);
                }
            }

            constructor();
        }
    ]);
})();