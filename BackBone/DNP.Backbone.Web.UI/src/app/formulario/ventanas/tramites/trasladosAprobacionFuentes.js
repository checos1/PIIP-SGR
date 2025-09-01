
(function () {
    'use strict';

    trasladosAprobacionFuentesController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'flujoServicios',
        'constantesBackbone',
        'trasladosServicio',
        '$routeParams',
        '$route',
        'servicioCargaDatos',
        '$timeout',
        '$location'
    ];



    function trasladosAprobacionFuentesController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        flujoServicios,
        constantesBackbone,
        trasladosServicio,
        $routeParams,
        $route,
        servicioCargaDatos,
        $timeout,
        $location
    ) {
        var vm = this;
        vm.user = {};
        
        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyecto.html';

        vm.situacionTemplate = '<div class="row text-center"> <label>CSF</label>  </div > <div class="linea"></div>' +
            '<div class="row text-center"> <label>SSF</label> </div > ';

        vm.montosVigenteTemplate = '<div class="row textcenter"> <label>{{row.entity.ValorVigenteCSF}} </label> </div > <div class="linea"></div>' +
            '<div class="row text-center"> <label>{{row.entity.ValorVigenteSSF}} </label> </div > ';

        vm.montosSolicitadosTemplate = '<div class="row text-center"> <label>{{row.entity.ValorSolicitadoCSF}} </label> </div > <div class="linea"></div>' +
            '<div class="row text-center    "> <label>{{row.entity.ValorSolicitadoSSF}}</label> </div > ';

        vm.montosAprobadoTemplate = '<div align="right"> ' +
            '<input type="number" style="text-align:right; height:30px; width:200px;" class="form-control"  value="{{row.entity.ValorAprobadoCSF}}" id="textmontoaprobadoCSF_{{row.entity.FuenteId}}"></div > ' +
            '<div align="right"> ' +
            '<input type="number" style="text-align:right; height:30px; width:200px;" class="form-control"  value="{{row.entity.ValorAprobadoSSF}}" id="textmontoaprobadoSSF_{{row.entity.FuenteId}}"></div > ';

        //style = "width:300px; height:55px; font-size:50px;"

        //vm.idInstancia = $sessionStorage.idInstanciaIframe;
        vm.tipoFiltroProyecto = "Proyecto";
        vm.tipoFiltroTramite = "Tramite";
        vm.Bpin = $sessionStorage.BPIN;
        vm.TipoProyecto = $sessionStorage.TipoProyecto;
        vm.EntidadId = $sessionStorage.EntidadId;
        vm.TipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.nombreEntidad = $sessionStorage.nombreEntidad;
        vm.nombreTipoTramite = $sessionStorage.nombreTipoTramite;
        vm.ProyectoId = $sessionStorage.ProyectoId;

        vm.TramiteId =26;
        vm.ProyectoId = 67056;
        vm.EntidadId = 1418;

        vm.idProyectoTramite = 38; // $sessionStorage.TramiteId;
        vm.idFuentePresupuestal = 0;
        vm.idProyectoRequisito = 0;
        vm.valorFuente = formatearNumero($sessionStorage.ValorMontoProyecto ? $sessionStorage.ValorMontoProyecto : 0);
        vm.valorFuenteVigencia = formatearNumero($sessionStorage.ValorMontoProyecto ? $sessionStorage.ValorMontoProyecto : 0);

        vm.idTipoTramite = "";
        vm.idEntidad = "1C58FFF0-E999-44C9-B4BE-0176A3CF73A5";
        vm.etapa = $routeParams['etapa'];
        vm.etapa = 'ej';
        vm.tipoEntidad = 'Nacional';
        vm.nombreArchivo = '';
        vm.NombreProyecto = $sessionStorage.NombreProyecto;
        vm.ObtenerFuentesAprobacion = ObtenerFuentesAprobacion;
        vm.guardarAprobacionFuentes = guardarAprobacionFuentes;
        vm.formatearNumero = formatearNumero;
        // grid main
        vm.gridOptions;

        vm.columnDef = [
            {
                field: 'NombreFuente',
                displayName: 'Fuente',
                enableHiding: false,
                enableColumnMenu: false,
                width: '25%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'TipoAccion',
                displayName: 'Tipo',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'TipoSituacion',
                displayName: 'CSF/SSF',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                cellTemplate: vm.situacionTemplate
            },
            {
                field: 'ValorVigenteCSF',
                displayName: 'Valor vigente $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '18%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                cellTemplate: vm.montosVigenteTemplate
                //type: "number",
                //cellFilter: 'currency:""'

            },
            {
                field: 'ValorSolicitadoCSF',
                displayName: 'Monto solicitado $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '18%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                cellTemplate: vm.montosSolicitadosTemplate
            },
            {
                field: 'ValorAprobadoCSF',
                displayName: 'Monto aprobado $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '18%',
                //pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                //cellTemplate: vm.selectorRenglonTemplate,
                cellTemplate: vm.montosAprobadoTemplate
            }
        ];

     


        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        vm.init = function () {
            vm.tipoEntidad = 'Nacional';
            vm.filtro = ''; 
            if (!vm.gridOptions) {
                vm.gridOptions = {
                    enableColumnResizing: false,
                    showGridFooter: false,
                    enablePaginationControls: true,
                    useExternalPagination: false,
                    useExternalSorting: false,
                    paginationCurrentPage: 1,
                    enableVerticalScrollbar: 1,
                    enableFiltering: false,
                    showHeader: true,
                    useExternalFiltering: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10  
                };
            }
            ObtenerFuentesAprobacion();
        };


        function ObtenerFuentesAprobacion() {

            vm.listaFuentesProyecto = [];
            vm.listaGrillaFuentesProyectos = [];
            vm.gridOptions.data = vm.listaGrillaFuentesProyectos;
            vm.gridOptions.columnDefs = [];
            var tipoproyectotmp = vm.TipoProyecto === undefined ? 0 : vm.TipoProyecto;
            trasladosServicio.ObtenerFuentesTramiteProyectoAprobacion(vm.TramiteId, vm.ProyectoId, tipoproyectotmp)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaFuentesProyecto = response.data;


                        vm.listaGrupoFuentesProyectos = [];
                        vm.listaFuentesProyecto.forEach(fuente => {
                            const { FuenteId, NombreFuente, TipoAccion, ValorVigenteCSF, ValorVigenteSSF, ValorSolicitadoCSF, ValorSolicitadoSSF, ValorAprobadoCSF, ValorAprobadoSSF } = fuente;
                            vm.listaGrupoFuentesProyectos.push({ FuenteId, NombreFuente, TipoAccion, ValorVigenteCSF, ValorVigenteSSF, ValorSolicitadoCSF, ValorSolicitadoSSF, ValorAprobadoCSF, ValorAprobadoSSF });
                        });

                        vm.listaFuentesProyecto.forEach(fuente => {
                            vm.listaGrillaFuentesProyectos.push({
                                FuenteId: fuente.FuenteId,
                                NombreFuente: fuente.NombreFuente,
                                TipoAccion: fuente.TipoAccion,
                                TipoProyecto: fuente.TipoProyecto,
                                ValorVigenteCSF: fuente.ValorVigenteCSF ? formatearNumero(fuente.ValorVigenteCSF) : 0,
                                ValorVigenteSSF: fuente.ValorVigenteSSF ? formatearNumero(fuente.ValorVigenteSSF) : 0,
                                ValorSolicitadoCSF: fuente.ValorSolicitadoCSF ? formatearNumero(fuente.ValorSolicitadoCSF) : 0,
                                ValorSolicitadoSSF: fuente.ValorSolicitadoSSF ? formatearNumero(fuente.ValorSolicitadoSSF) : 0,
                                ValorAprobadoCSF: fuente.ValorAprobadoCSF ? formatearNumero(fuente.ValorAprobadoCSF) : 0,
                                ValorAprobadoSSF: fuente.ValorAprobadoSSF ? formatearNumero(fuente.ValorAprobadoSSF) : 0,

                                subGridOptions: {
                                    columnDefs: vm.columnDef,
                                    showHeader: true,
                                    enableVerticalScrollbar: 1,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,
                                    data: vm.listaGrupoFuentesProyectos
                                    // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                }
                            });

                        });

                        vm.gridOptions.showHeader = true;
                        vm.gridOptions.columnDefs = vm.columnDef;
                        vm.gridOptions.data = vm.listaGrillaFuentesProyectos;
                        vm.filasFiltradas = vm.gridOptions.data.length > 0;            
                    }
                });
        }

        function guardarAprobacionFuentes() {
            let fuentes = [];

            if (vm.listaGrillaFuentesProyectos && vm.listaGrillaFuentesProyectos.length > 0) {
                vm.listaGrillaFuentesProyectos.forEach(fuente => {
                    var valorAprobadoCSF = document.getElementById('textmontoaprobadoCSF_' + fuente.FuenteId);
                    var valorAprobadoSSF = document.getElementById('textmontoaprobadoSSF_' + fuente.FuenteId);


                    let c = {
                        TramiteId: vm.TramiteId,
                        ProyectoId: vm.ProyectoId,
                        EntidadId: vm.EntidadId,
                        FuenteId: fuente.FuenteId,
                        TipoAccion: fuente.TipoAccion,
                        ValorAprobadoCSF: valorAprobadoCSF.value,
                        ValorAprobadoSSF: valorAprobadoSSF.value
                    };
                    fuentes.push(c);
                });
            }

            var prm = {
                TramiteId: vm.TramiteId,
                ProyectoId: vm.ProyectoId,
                EntidadId: vm.EntidadId,
                Fuentes: fuentes
            };

            trasladosServicio.guardarFuentesTramiteProyectoAprobacion(prm.Fuentes)
                .then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            limpiarCombos();
                            ObtenerProyectosTramite();
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });


        }


        function formatearNumero(value) {
            var numerotmp = value.toString().replace(/,/g, '');
            return Number(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
        }


        function retornar() {
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }

    }

    angular.module('backbone').controller('trasladosAprobacionFuentesController', trasladosAprobacionFuentesController);
    

})();