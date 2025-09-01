(function () {
    'use strict';

    modalAccionPerfilController.$inject = [
        'objPerfil',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades'
    ];

    function modalAccionPerfilController(
        objPerfil,
        $uibModalInstance,
        servicioUsuarios,
        utilidades
    ) {
        var vm = this;

        vm.listaRoles;
        vm.perfil;
        vm.gridOptions;
        
        vm.addRol = addRol;
        vm.eliminarRol = eliminarRol;
        vm.guardarPerfil = guardarPerfil;
        vm.cerrar = $uibModalInstance.dismiss;

        /// Comienzo
        vm.init = function () {
            servicioUsuarios.obtenerRoles("")
                .then(function (response) {
                    vm.listaRoles = response.data;
                });

            vm.gridOptions = {
                columnDefs: [{
                    field: 'Nombre',
                    displayName: 'Roles',
                    enableHiding: false,
                    width: '85%'
                },
                {
                    field: 'accion',
                    displayName: 'Acción',
                    headerCellClass: 'text-center',
                    enableFiltering: false,
                    enableHiding: false,
                    enableSorting: false,
                    enableColumnMenu: false,
                    cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.eliminarRol(row.entity)" tooltip-placement="Auto" uib-tooltip="Eliminar">' +
                                  '    <span style="cursor:pointer"> <img class="grid-icon-accion"  src="Img/iconsgrid/iconAccionEliminar.png"></span>' +
                                  '</button></div>',
                    width: '15%'
                }],
                enableVerticalScrollbar: true,
                enableSorting: true,
                showHeader: true,
                showGridFooter: false
            };

            if (objPerfil != null) {
                vm.idPerfil = objPerfil.IdPerfil;
                vm.perfil = objPerfil.NombrePerfil;
                obtenerRoles();
            }
        }

        function addRol() {
            let rol = vm.listaRoles.filter(x => x.IdRol == vm.idRol.toString())[0];
            let rolGrid = vm.gridOptions.data.filter(x => x.IdRol == vm.idRol)[0];

            if (!rolGrid) {
                vm.gridOptions.data.push({ IdRol: rol.IdRol, Nombre: rol.Nombre, Agregar: true });
            }
        }

        function eliminarRol(row) {
            let rowIndex = vm.gridOptions.data.indexOf(row);
            vm.gridOptions.data.splice(rowIndex, 1);
        }



        function guardarPerfil() {
            var perfilDto = {
                NombrePerfil: vm.perfil,
                Roles: vm.gridOptions.data,
                UsuarioDNP: usuarioDNP
            };

            if (vm.idPerfil) {
                perfilDto.IdPerfil = vm.idPerfil;
            }

            servicioUsuarios.guardarPerfil(perfilDto)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                    } else {
                        utilidades.mensajeError(response.data.Mensaje, false);
                    }
                });
        }

        function obtenerRoles() {
            var peticionObtenerRoles = {
                usuario: usuarioDNP,
                idPerfil: vm.idPerfil
            };
            if (vm.idPerfil) {
                vm.gridOptions.data = [];
                servicioUsuarios.obtenerRolesDePerfil(peticionObtenerRoles)
                    .then(function (response) {
                        if (response != null)
                            vm.gridOptions.data = response.data;
                    })
            }
        }
    }

    angular.module('backbone.usuarios').controller('modalAccionPerfilController', modalAccionPerfilController);
})();