(function () {
    'use strict';

    requerimientosTramiteController.$inject = ['$scope', 'requerimientosTramitesServicio', '$sessionStorage', 'constantesBackbone', 'sesionServicios',];

    function requerimientosTramiteController(
        $scope,
        requerimientosTramitesServicio,
        $sessionStorage,
        constantesBackbone,
        sesionServicios
    ) {
        var vm = this;
        vm.activartabconceptotecnico = false;

        vm.listaIdsRoles = sesionServicios.obtenerUsuarioIdsRoles();
        var i = 0;
        vm.listaIdsRoles.forEach(rol => {
            if (i == 0) {
                if (rol.toUpperCase() == constantesBackbone.idRAnalistaDIFP || rol.toUpperCase() == constantesBackbone.idRControlPosteriorDireccionesTecnicas) {
                    vm.activartabconceptotecnico = true;
                    i = 1;
                }
            }
        });

        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
        vm.idAccionParam = $sessionStorage.idAccion;
        if (vm.disabledJefePlaneacion) {
            vm.idAccionParam = $sessionStorage.idAccionAnterior;
        }
        vm.infoArchivo = {
            coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: $sessionStorage.TipoTramiteId, noJefePlaneacion: vm.noJefePlaneacion, disabledJefePlaneacion: vm.disabledJefePlaneacion, allArchivos: $sessionStorage.allArchivosTramite,
            soloLectura: $sessionStorage.soloLectura
        }
    }


    angular.module('backbone').component('requerimientosTramite', {
        templateUrl: "src/app/formulario/ventanas/tramites/requerimientosTramites/requerimientosTramites.html",
        controller: requerimientosTramiteController,
        controllerAs: "vm",
        bindings: {
            disabled: '='
        }
    });

})();