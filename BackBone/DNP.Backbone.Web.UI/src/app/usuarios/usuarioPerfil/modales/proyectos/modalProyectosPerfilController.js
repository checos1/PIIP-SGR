(function () {
    'use strict';

    modalProyectosPerfilController.$inject = [
        'perfilSeleccionado',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades'
    ];

    function modalProyectosPerfilController(
        perfilSeleccionado,
        $uibModalInstance,
        servicioUsuarios,
        utilidades
    ) {
        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.plantillaProyectoBpin = 'src/app/usuarios/usuarioPerfil/modales/proyectos/plantillas/plantillaProyectoBpin.html';

        vm.addProyecto = addProyecto;
        vm.eliminarProyecto = eliminarProyecto;
        vm.guardar = guardar;

        vm.idUsuarioPerfil = perfilSeleccionado.idUsuarioPerfil;

        vm.idEntidad = perfilSeleccionado.idEntidad;
        vm.entidad = perfilSeleccionado.entidad;
        vm.idProyecto;
        vm.listaProyectos = [];

        init();

        function init() {
            vm.nombrePerfil = perfilSeleccionado.perfil;
            obtenerProyectos();

            vm.opciones = {
                tablaDeModal: true,
                nivelJerarquico: 0,
                gridOptions: {
                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                    paginationPageSize: 10
                }
            };

            vm.gridOptions = {
                columnDefs: [{
                    field: 'CodigoBpin',
                    displayName: 'Proyecto',
                    enableFiltering: false,
                    enableHiding: false,
                    enableSorting: false,
                    enableColumnMenu: false,
                    cellTemplate: vm.plantillaProyectoBpin
                },
                {
                    field: 'accion',
                    displayName: 'Acción',
                    headerCellClass: 'text-center',
                    enableFiltering: false,
                    enableHiding: false,
                    enableSorting: false,
                    enableColumnMenu: false,
                    cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.eliminarProyecto(row.entity)" tooltip-placement="Auto" uib-tooltip="Eliminar">' +
                        '    <span  style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionEliminar.png"></span>' +
                        '</button></div>',
                    width: '15%'
                }],
                enableVerticalScrollbar: true,
                enableSorting: true,
                showHeader: true,
                showGridFooter: false
            };

            obtenerProyectosDePerfil();
        }

        function obtenerProyectosDePerfil() {

            var peticionobtenerProyectos = {
                usuario: usuarioDNP,
                idUsuarioPerfil: perfilSeleccionado.idUsuarioPerfil
            };

            servicioUsuarios.obtenerProyectosDePerfil(peticionobtenerProyectos)
                .then(function (response) {
                    if (response.data) {
                        vm.gridOptions.data = response.data;
                    } else {
                        vm.gridOptions.data = [];
                    }
                })
        }

        function obtenerProyectos() {
            var dto = { idEntidad: vm.idEntidad };
            
            servicioUsuarios.obtenerProyectosPorEntidad(dto)
                .then(function (response) {
                    response.data.forEach(item => {
                        item.ProyectoBpin = item.ProyectoNombre + ' - ' + item.CodigoBpin
                    });
                    vm.listaProyectos = response.data;
                })
        }



        function addProyecto() {
            if (vm.idProyecto) {
                let proyecto = vm.listaProyectos.filter(x => x.ProyectoId.toString() == vm.idProyecto.toString())[0];
                let proyectoGrid = vm.gridOptions.data.filter(x => x.ProyectoId == vm.idProyecto)[0];
                if (!proyectoGrid) {
                    vm.gridOptions.data.push({ CodigoBpin: proyecto.CodigoBpin, ProyectoId: proyecto.ProyectoId, ProyectoNombre: proyecto.ProyectoNombre, Agregar: true });
                }
            }
        }

        function eliminarProyecto(row) {
            vm.gridOptions.data.splice(row, 1);
        }

        function guardar() {
            var dto = {
                idUsuarioPerfil: vm.idUsuarioPerfil,
                proyectos: []
            };
            vm.gridOptions.data.forEach(item => {
                dto.proyectos.push({ idProyecto: item.ProyectoId });
            });

            servicioUsuarios.asociarProyectosAUsuarioPerfil(dto)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else {
                        utilidades.mensajeError(response.data.Mensaje, false);
                    }

                    console.log(response.data);
                });
        }


    }

    angular.module('backbone.usuarios').controller('modalProyectosPerfilController', modalProyectosPerfilController);
})();