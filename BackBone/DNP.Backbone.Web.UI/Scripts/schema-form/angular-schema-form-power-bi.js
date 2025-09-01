(function () {
    "use strict";

    angular.module("backbone").config(["schemaFormProvider", "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
        function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {

            var control = constantesFormularios.controlPowerBI;

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
                "/Scripts/schema-form/directives/powerbi.html"
            );
        }
    ]);

    angular.module("backbone").controller("PowerBIController", ["$scope", "sesionServicios", "servicioConsolaMonitoreo" ,
        function ($scope, sesionServicios, servicioConsolaMonitoreo) {
            var vm = this;

            var mapearValores = function (filtros) {
                var retorno = [];
                console.log(filtros);
                angular.forEach(filtros, function (filtro) {
                    var prop = (filtro.target.column + "").toLowerCase();
                    var value = null;
                    for (var p in $scope.model) {
                        if ($scope.model.hasOwnProperty(p) && prop == (p + "").toLowerCase()) {
                            value = $scope.model[p];
                            break;
                        }
                    }

                    if (value == null) {
                        angular.forEach($scope.form.configuracion.parametros, function (parametro) {
                            for (var param in parametro) {
                                if (param['id'] == filtro.target.column && param['dato'] != null) {
                                    value = param['dato'];
                                    break;
                                }
                            }
                        });
                    }
                 
                    


                    filtro.values = [value];
                    //filtro.target.column = filtro.target.column.toUpperCase();
                    retorno.push(filtro);
                });

                return retorno;
            }

            this.$onInit = function () {
                var configuracion = $scope.form.configuracion;
                console.log($scope.model);
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                if (roles != null &&
                    roles.length > 0) {
                        var peticion = {
                            IdUsuario: usuarioDNP,
                            Aplicacion: nombreAplicacionBackbone,
                            IdObjeto: "bc154cba-50a5-4209-81ce-7c0ff0aec2ce",
                            ListaIdsRoles: roles
                        };

                    servicioConsolaMonitoreo
                        .obtenerConsolaMonitoreoReportes(peticion, configuracion.filtroPeticion)
                            .then(
                                function (retorno) {
                                    
                                    vm.configuracion = $scope.form.configuracion;
                                    vm.configuracion.embedFiltro = mapearValores(vm.configuracion.embedFiltro);
                                    console.log(vm.configuracion);
                                    vm.configuracion.embedConfig = retorno.data;
                                },
                                function (error) {

                                }
                            );
                    }
               
           
            }
        }
    ]);
})();