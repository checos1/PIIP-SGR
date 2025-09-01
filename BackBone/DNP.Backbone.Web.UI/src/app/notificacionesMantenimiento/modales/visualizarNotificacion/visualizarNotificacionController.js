(function () {
    'use strict';

    visualizarNotificacionController.$inject = [
        '$scope',
        '$sce',
        'params',
        '$uibModalInstance',
        'archivoServicios',
        'FileSaver',
        'utilidades'
    ];

    function visualizarNotificacionController(
        $scope,
        $sce,
        params,
        $uibModalInstance,
        archivoServicios,
        FileSaver,
        utilidades
    ) {
        var vm = this;
        vm.params = params;
        vm.notificacion = {};
        vm.idAplicacion = "configNotificaciones";
        vm.visualizar = visualizar;

        vm.init = function() {
            vm.notificacion = vm.params.Notificacion;
        }

        vm.cerrar = function () {
            $uibModalInstance.dismiss('cerrar');
        }

        function visualizar(entity) {
            if (entity.IdArchivo) {
                //Consultar los metadatos del archivo
                archivoServicios.obtenerArchivoInfo(entity.IdArchivo, vm.idAplicacion).then(result => {
                    descargarArchivo({
                        IdArchivoBlob: result.metadatos.idarchivoblob,
                        ContenType: result.metadatos.contenttype,
                        NombreCompleto: `notificacion.${result.metadatos.extension}`
                    });
                }, error => {
                    toastr.warning("Ocurrió un error al consultar la información del archivo");
                })
            }
            else {
                toastr.warning("No hay archivo para descargar");
            }
        }

        /**
         * Permite descargar el archivo seleccionado
         * @param {object} entity
         */
        function descargarArchivo(entity) {
            archivoServicios.obtenerArchivoBytes(entity.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                //const blob = new Blob([retorno], { type: entity.ContenType });
                FileSaver.saveAs(blob, entity.NombreCompleto);
            }, function (error) {
                toastr.error("Error inesperado al descargar");
            });
        }
    }

    angular.module('backbone').controller('visualizarNotificacionController', visualizarNotificacionController);
})();