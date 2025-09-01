(function () {
    'use strict';

    principalCartaController.$inject = ['$scope', 'requerimientosTramitesServicio', '$sessionStorage', 'utilidades', 'constantesBackbone',
        'servicioFichasProyectos',
        'FileSaver',
        '$q'];

    function principalCartaController(
        $scope,
        requerimientosTramitesServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone,
        servicioFichasProyectos,
        FileSaver,
        $q
    ) {
        var vm = this;
        vm.idTramite = $sessionStorage.TramiteId;
        vm.tipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.TipoProyecto = $sessionStorage.TipoProyecto;
        vm.EntidadId = $sessionStorage.EntidadId;
        vm.NombreProyecto = $sessionStorage.NombreProyecto;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.nombreEntidad = $sessionStorage.nombreEntidad;
        vm.nombreTipoTramite = $sessionStorage.nombreTipoTramite;

        vm.retornar = retornar;
        vm.Ficha = '';


      
        function retornar() {
            vm.desactivarcarta();
        }

        vm.Actualizar = function () { }

        vm.verPDF = function () {
            var ficha = {
                Nombre: constantesBackbone.apiBackBoneNombrePDFCarta,
            };

            vm.Ficha = ficha;

            var fichaPlantilla = {
                NombreReporte: ficha.Nombre,
                PARAM_BORRADOR: true,
                PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                TramiteId: vm.idTramite
            };

            crearDocumento(fichaPlantilla).then(function (fichaTemporal) {
                FileSaver.saveAs(fichaTemporal, fichaTemporal.name);
            }, function (error) {
                utilidades.mensajeError(error);
            });



        }

        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = vm.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha(vm.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        //var blob = new Blob([respuesta], { type: 'application/pdf' });
                        const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                        var fileOfBlob = new File([blob], nombreArchivo, { type: 'application/pdf' });
                        var archivo = {};

                        var metadatos = {
                            NombreAccion: $sessionStorage.nombreAccion,
                            IdAplicacion: 1,//$sessionStorage.IdAplicacion,
                            IdNivel: $sessionStorage.idNivel,
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAccion: $sessionStorage.idAccion,
                            IdInstanciaFlujoPrincipal: $sessionStorage.idInstanciaFlujoPrincipal,
                            IdObjetoNegocio: $sessionStorage.idObjetoNegocio,
                            Size: blob.size,
                            ContenType: 'application/pdf',
                            Extension: extension,
                            FechaCreacion: new Date(),
                            Tipo: 'Ficha',
                            NombreFicha: respuestaFicha.Nombre,
                            TipoFicha: respuestaFicha.Descripcion
                        }

                        archivo = {
                            FormFile: fileOfBlob,
                            Nombre: nombreArchivo,
                            Metadatos: metadatos
                        };

                        if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            archivoServicios.cargarArchivo(archivo, $sessionStorage.IdAplicacion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    reject(response);
                                } else {
                                    resolve(fileOfBlob);
                                }
                            }, function (error) {
                                reject(error);
                            });
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }



    }

    //angular.module('backbone').controller('principalCartaController', principalCartaController);
    angular.module('backbone').component('principalCarta', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/carta/principalCarta.html",
        controller: principalCartaController,
        controllerAs: "vm",
        bindings: {
            desactivarcarta: '&'
        }
    });

})();