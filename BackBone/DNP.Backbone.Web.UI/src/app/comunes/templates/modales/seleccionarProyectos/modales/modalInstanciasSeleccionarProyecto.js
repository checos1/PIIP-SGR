(function () {
    'use strict';

    modalInstanciasSeleccionarProyectoController.$inject = [
        '$scope',
        '$uibModalInstance',
        'objProyecto',
        'servicioSeleccionarProyectos',
        'sesionServicios',
        'backboneServicios',
        '$sessionStorage',
        '$timeout',
        '$location'
    ];

    function modalInstanciasSeleccionarProyectoController(
        $scope,
        $uibModalInstance,
        objProyecto,
        servicioSeleccionarProyectos,
        sesionServicios,
        backboneServicios,
        $sessionStorage,
        $timeout,
        $location
    ) {
        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.peticion;
        
        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.criticidadTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaCriticidad.html';
        vm.consultarAccionFlujoTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaConsultarAccionFlujo.html';
        vm.proyectoFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaProyecto.html';

        vm.gridOptions;

        vm.listaEntidades = [];
        vm.filasFiltradas = null;
        vm.Mensaje = "";
        vm.consultarAccion = consultarAccion;

        vm.filtro = {
            nombre: null,
            entidadId: null,
            tipoEntidadId: null,
            bpin: objProyecto.bpin,
            tipoEntidad: objProyecto.tipoEntidad,
        };

        vm.columnDefPrincial = [{
            field: 'entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
        }];

        vm.columnDef = [{
            field: 'ProyectoId',
            displayName: 'Identificador',
            enableHiding: false,
            width: '10%%'
        },
        {
            field: 'IdObjetoNegocio',
            displayName: 'BPIN',
            enableHiding: false,
            width: '10%'
        },
        {
            field: 'NombreObjetoNegocio',
            displayName: 'Nombre',
            enableHiding: false,
            width: '25%'
        },
        {
            field: 'NombreEntidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '25%'
        },
        {
            field: 'DescripcionCR',
            displayName: 'Identificador CR',
            enableHiding: false,
            width: '20%'
        },
        {
            field: 'Criticidad',
            displayName: 'Prioridad',
            enableHiding: false,
            enableColumnMenu: false,
            width: '8%',
            cellTemplate: vm.criticidadTemplate,
        },
        {
            field: 'EstadoProyecto',
            displayName: 'Estado del Proyecto',
            enableHiding: false,
            width: '14%'
        },
        {
            field: 'accionFlujo',
            displayName: 'Nombre/Accion Flujo',
            enableHiding: false,
            enableColumnMenu: false,
            width: '20%',
            cellTemplate: vm.consultarAccionFlujoTemplate,
        },
        {
            field: 'Horizonte',
            displayName: 'Horizonte',
            enableHiding: false,
            width: '10%'
        },
        {
            field: 'SectorNombre',
            displayName: 'Sector',
            enableHiding: false,
            width: '20%'
        }
        ];

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        init();

        function init() {

            cargarPeticion();
            
            if (!vm.gridOptions) {
                vm.gridOptions = {
                    expandableRowTemplate: vm.proyectoFilaTemplate,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    expandableRowHeight: 220,
                    enableColumnResizing: true,
                    showHeader: false,
                    useExternalFiltering: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: onRegisterApi
                };

                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.listaEntidades;
            }

            if (objProyecto.bpin) {
                obtenerInstanciasProyecto();
            }
        }

        function obtenerInstanciasProyecto() {
            
            servicioSeleccionarProyectos.obtenerInstanciasProyecto(vm.peticion, vm.filtro)
                .then(function (respuesta) {
                    vm.listaEntidades = [];
                    
                    if (respuesta.data.GruposEntidades && respuesta.data.GruposEntidades.length > 0) {
                        const listaGrupoEntidades = respuesta.data.GruposEntidades;
                        listaGrupoEntidades.forEach(grupoEntidade => {
                            grupoEntidade.ListaEntidades.forEach(entidad => {
                                const nombreEntidade = entidad.NombreEntidad;
                                const tipoEntidad = entidad.TipoEntidad;
                                const nombreSector = entidad.ObjetosNegocio[0].SectorNombre;

                                vm.listaDatos = [];
                                entidad.ObjetosNegocio.forEach(negocio => {
                                    vm.listaDatos.push({
                                        ProyectoId: negocio.ProyectoId,
                                        IdObjetoNegocio: negocio.IdObjetoNegocio,
                                        NombreObjetoNegocio: negocio.NombreObjetoNegocio,
                                        NombreEntidad: negocio.NombreEntidad,
                                        DescripcionCR: negocio.DescripcionCR,
                                        FechaCreacion: negocio.FechaCreacion,
                                        Criticidad: negocio.Criticidad,
                                        EstadoProyecto: negocio.EstadoProyecto,
                                        Horizonte: negocio.Horizonte,
                                        SectorNombre: negocio.SectorNombre,
                                        NombreAccion: negocio.NombreAccion,
                                        IdEntidad: negocio.IdEntidad,
                                        IdInstancia: negocio.IdInstancia,
                                        NombreFlujo: negocio.NombreFlujo
                                    });
                                });


                                vm.listaEntidades.push({
                                    sector: nombreSector,
                                    entidad: nombreEntidade,
                                    tipoEntidad: tipoEntidad,
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        enableVerticalScrollbar: 1,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,
                                        data: vm.listaDatos,
                                        //rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                    }
                                });

                            });
                        });

                    }

                    vm.gridOptions.data = vm.listaEntidades;
                    vm.filasFiltradas = vm.gridOptions.data.length > 0;
                    vm.Mensaje = respuesta.data.Mensaje;
                })
        }

        function cargarPeticion() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticion = {
                    IdUsuario: usuarioDNP,
                    IdObjeto: idTipoProyecto,
                    Aplicacion: nombreAplicacionBackbone,
                    ListaIdsRoles: roles
                };

            }
        }

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {
            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            vm.cerrar();
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }

    }

    angular.module('backbone').controller('modalInstanciasSeleccionarProyectoController', modalInstanciasSeleccionarProyectoController);
})();