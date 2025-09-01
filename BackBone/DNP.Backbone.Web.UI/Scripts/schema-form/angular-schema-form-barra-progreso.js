angular.module("schemaForm").config([
    "schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider",
    function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider) {

        schemaFormProvider.defaults.string.unshift(function (name, schema, options) {
            if ((schema.type === "barraDeProgreso") && schema.format == "barraDeProgreso") {
                var f = schemaFormProvider.stdFormObj(name, schema, options);
                f.key = options.path;
                f.type = "barraDeProgreso";
                options.lookup[sfPathProvider.stringify(options.path)] = f;
                return f;
            }
        });

        schemaFormDecoratorsProvider.addMapping(
            "bootstrapDecorator",
            "barraDeProgreso",
            "/Scripts/schema-form/directives/barraDeProgreso.html"
        );
    }
]).directive("barradeprogreso",
    [
        "$filter", function ($filter) {
            return {
                restrict: "A",
                scope: false,
                controller: ["$scope", controladorBarraDeProgreso],
                link: function (scope, iElement, iAttrs, ngModelCtrl) {
                    scope.ngModel = ngModelCtrl;
                    scope.filter = $filter;
                }
            };
        }
    ]);


function controladorBarraDeProgreso($scope) {
    $scope.valorMaximo = obtenerNumeroDeControlesRequeridos($scope.schema);
    $scope.calcularPorcentaje = calcularPorcentaje;
    $scope.calcularTamano = calcularTamano;

    function calcularPorcentaje(modeloSchemaForm, esquemaSchemaForm) {
        $scope.porcentaje = 0;
        var contador = 0;

        if ($scope.valorMaximo > 0) {
            for (var k in modeloSchemaForm) {
                if (modeloSchemaForm.hasOwnProperty(k) &&
                    tieneValor(modeloSchemaForm[k]) &&
                    esquemaSchemaForm.required.indexOf(k) >= 0) {
                    contador++;
                }
            }

            $scope.porcentaje = Math.ceil(contador / $scope.valorMaximo * 100);
        }

        return $scope.porcentaje;
    }

    function calcularTamano(tamano) {
        var resultado;

        switch (tamano) {
            case "small":
                resultado = 50;
                break;
            case "medium":
                resultado = 75;
                break;
            default:
                resultado = 100;
        }
        return resultado;
    }

    function obtenerNumeroDeControlesRequeridos(esquemaSchemaForm) {
        var resultado = 0;

        if (esquemaSchemaForm.required != null) {
            resultado = esquemaSchemaForm.required.length;
        }

        return resultado;
    }

    function tieneValor(expresion) {
        var resultado = false;

        switch (typeof (expresion)) {
        case "string":
            resultado = expresion.length > 0;
            break;
        case "object":
            if (expresion) {
                if (expresion instanceof Array) {
                    resultado = expresion.length > 0;
                } else if (expresion instanceof Date) {
                    resultado = true;
                } else if (expresion.hasOwnProperty("value")) {
                    resultado = true;
                }
            }
            break;
        case "number":
            resultado = !isNaN(expresion);
            break;
        case "undefined":
            resultado = false;
            break;
        default:
            resultado = true;
            break;
        }

        return resultado;
    }
};