(function () {
    'use strict';

    modalRolesPerfilController.$inject = [
        'perfilSeleccionado',
        '$uibModalInstance',
        'servicioUsuarios'
    ];

    function modalRolesPerfilController(
        perfilSeleccionado,
        $uibModalInstance,
        servicioUsuarios
    ) {
        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;

        init();

        function init() {
            vm.nombrePerfil = perfilSeleccionado.perfil;

            vm.opciones = {
                tablaDeModal: true,
                nivelJerarquico: 0,
                gridOptions: {
                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                    paginationPageSize: 10
                }
            };

            vm.columnDefs = [{
                field: 'Nombre',
                displayName: 'Nombre del Rol',
                enableHiding: false
            }];

            obtenerRoles();
        }

        function obtenerRoles() {

            var peticionObtenerRoles = {
                usuario: usuarioDNP,
                idPerfil: perfilSeleccionado.idPerfil
            };

            servicioUsuarios.obtenerRolesDePerfil(peticionObtenerRoles)
                .then(function (response) {
                    vm.datos = response.data;
                })
        }
    }

    angular.module('backbone.usuarios').controller('modalRolesPerfilController', modalRolesPerfilController);
})();