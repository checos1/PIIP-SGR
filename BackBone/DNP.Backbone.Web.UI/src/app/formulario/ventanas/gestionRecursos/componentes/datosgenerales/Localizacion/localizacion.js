(function () {
    'use strict';

    localizacionController.$inject = ['$scope', 'gestionRecursosServicio', '$sessionStorage', 'utilidades', 'constantesBackbone'];

    function localizacionController(
        $scope,
        gestionRecursosServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone
    ) {
        var vm = this;

        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.seccionCapitulo = null;
        vm.nombreComponente = "datosgeneralesgrlocalizaciones";


        vm.init = function () {
            vm.ObtenerLocalizacionProyecto(vm.BPIN);
        };

        vm.ObtenerLocalizacionProyecto = function(Bpin) {

            return gestionRecursosServicio.ObtenerLocalizacionProyecto($sessionStorage.idInstancia).then(
                function (respuesta) {

                    var listaLocalizaciones = [
                        //{ Departamento: "BOG", Municipio: "Bogota", Tipo: "nacional", Agrupacion: "pruebas" },
                        //{ Departamento: "MED", Municipio: "Medellin", Tipo: "local", Agrupacion: "pruebas" },
                    ]

                    var localizacion = "";
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistaLocalizaciones = jQuery.parseJSON(respuesta.data);
                        for (var ls = 0; ls < arreglolistaLocalizaciones.Localizacion.length; ls++) {
                            if (arreglolistaLocalizaciones.Localizacion[ls].RegionId != null) {
                                localizacion =
                                {
                                    "Departamento": arreglolistaLocalizaciones.Localizacion[ls].Departamento,
                                    "Municipio": arreglolistaLocalizaciones.Localizacion[ls].Municipio,
                                    "Tipo": arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion,
                                    "Agrupacion": arreglolistaLocalizaciones.Localizacion[ls].Agrupacion
                                }

                                listaLocalizaciones.push(localizacion);
                            }
                        }
                        for (var po = 0; po < arreglolistaLocalizaciones.NuevaLocalizacion.length; po++) {
                            if (arreglolistaLocalizaciones.NuevaLocalizacion[po].RegionId != null) {
                                var NuevaLocalizacion =
                                {
                                    "Departamento": arreglolistaLocalizaciones.NuevaLocalizacion[po].Departamento,
                                    "Municipio": arreglolistaLocalizaciones.NuevaLocalizacion[po].Municipio,
                                    "Tipo": arreglolistaLocalizaciones.NuevaLocalizacion[po].TipoAgrupacion,
                                    "Agrupacion": arreglolistaLocalizaciones.NuevaLocalizacion[po].Agrupacion
                                }

                                listaLocalizaciones.push(NuevaLocalizacion);
                            }
                        }
                        vm.DatosLocalizacion = listaLocalizaciones;

                    }
                }
            );
        }

        

     
    }

    angular.module('backbone').component('localizacion', {
        templateUrl: "src/app/formulario/ventanas/gestionRecursos/componentes/datosgenerales/localizacion/localizacion.html",
        controller: localizacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();