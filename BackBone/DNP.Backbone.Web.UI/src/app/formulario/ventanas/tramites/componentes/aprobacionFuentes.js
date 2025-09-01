(function () {
    'use strict';

    aprobacionFuentesController.$inject = [
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

    function aprobacionFuentesController(
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
        vm.director = $sessionStorage.director ? $sessionStorage.director : false;

        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyecto.html';


        vm.montoProyectoFuenteTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">CSF</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">SSF</label> </div > ';

        vm.montoProyectoInicialTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorInicialCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorInicialSSF}}</label> </div > ';

        vm.montoProyectoVigenteTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorVigenteCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorVigenteSSF}}</label> </div > ';

        vm.montoProyectoDisponibleTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorDisponibleCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorDisponibleSSF}}</label> </div > ';

        vm.montoProyectoSolicitadoTemplate = '<div class="row text-right " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorSolicitadoCSF}}</label> </div > ' +
            '<div class="row text-right" style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorSolicitadoSSF}}</label> </div > ';

        vm.montoAprobadoTemplate = '<div class="row text-right " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-value="row.entity.ValorAprobadoCSF" ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila($event,1)" /> </div > ' +
            '<div class="row text-right " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-value="row.entity.ValorAprobadoSSF"  ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila($event, 2)"/> </div > ';


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

        vm.TramiteId = $sessionStorage.TramiteId; //26;
        vm.ProyectoId = $sessionStorage.ProyectoId; //67056;
        vm.EntidadId = $sessionStorage.EntidadId; //1418;
        vm.AccionEjecutandose = $sessionStorage.accionEjecutandose;

        vm.ValorTotalMontoCSFNacion = 0;
        vm.ValorTotalMontoSSFNacion = 0;
        vm.ValorTotalMontoCSFPropios = 0;
        vm.ValorTotalMontoSSFPropios = 0;
        vm.ValorTotalCSF = 0;
        vm.ValorTotalSSF = 0;

        vm.ValorInicialNacion = 0;
        vm.ValorVigenteNacion = 0;
        vm.ValorDisponibleNacion = 0;
        vm.ValorVigenciasFuturasNacion = 0;
        vm.ValorInicialPropios = 0;
        vm.ValorVigentePropios = 0;
        vm.ValorDisponiblePropios = 0;
        vm.ValorVigenciasFuturasPropios = 0;

        //vm.TramiteId = 26;
        //vm.ProyectoId = 67056;
        //vm.EntidadId = 1418;


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
        vm.actualizaValoresPorFuente = actualizaValoresPorFuente;
        // grid main
        vm.gridOptions;

      
        vm.columnDefFuente = [
            {
                field: "idproyectotramite",
                displayName: 'Id Proyecto fuente presupuestal',
                enableHiding: false,
                visible: false

            },
            {
                field: "idproyectofuentetramite",
                displayName: 'Id Proyecto fuente presupuestal',
                enableHiding: false,
                visible: false

            },
            {
                field: 'idfuente',
                enableHiding: false,
                visible: false
            },
            {
                field: "idTipoValorInicial",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorVigente",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorContracreditoCSF",
                enableHiding: false,
                visible: false

            },
            {
                field: "idTipoValorContracreditoSSF",
                enableHiding: false,
                visible: false

            },
            {
                field: 'tiporecurso',
                displayName: 'Tipo Recurso',
                enableHiding: false,
                minWidth: 200,
                cellClass: 'text-justificado',
                enableColumnMenu: false,
            },
            {
                field: 'valor',
                displayName: ' ',
                enableHiding: false,
                width: '10%',
                enableColumnMenu: false,
                cellTemplate: vm.montoProyectoFuenteTemplate
            },
            {
                field: 'valorinicial',
                displayName: 'Valor inicial ',
                enableHiding: false,
                minWidth: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoProyectoInicialTemplate
            },
            {
                field: 'valorvigente',
                displayName: 'Valor vigente',
                enableHiding: false,
                enableColumnMenu: false,
                minWidth: 100,
                enableCellEdit: true,
                cellTemplate: vm.montoProyectoVigenteTemplate

            },
            {
                field: 'valodisponible',
                displayName: 'Valor disponible',
                enableHiding: false,
                enableColumnMenu: false,
                minWidth: 100,
                enableCellEdit: true,
                cellTemplate: vm.montoProyectoDisponibleTemplate

            },
            {
                field: 'valorsolicitado',
                displayName: 'Valor solicitado',
                Width: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoProyectoSolicitadoTemplate
            },
            {
                field: 'valoraprobado',
                displayName: 'Valor aprobado',
                Width: 100,
                enableColumnMenu: false,
                cellTemplate: vm.montoAprobadoTemplate
            },
           
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
                vm.gridOptions.appScopeProvider = vm;
            }
            ObtenerFuentesAprobacion();
            actualizaValoresPorFuente();
        };


        function ObtenerFuentesAprobacion() {

            vm.listaFuentesProyecto = [];
            vm.listaGrillaFuentesProyectos = [];
            vm.gridOptions.data = vm.listaGrillaFuentesProyectos;
            vm.gridOptions.columnDefs = vm.columnDefFuente;
            var proyectoidtmp = vm.ProyectoId === undefined ? 0 : vm.ProyectoId;
            trasladosServicio.ObtenerFuentesTramiteProyectoAprobacion(vm.TramiteId, proyectoidtmp, vm.TipoProyecto)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaFuentesProyecto = response.data;


                        vm.listaGrupoFuentesProyectos = [];
                        vm.listaFuentesProyecto.forEach(fuente => {
                            const { FuenteId, NombreFuente, TipoAccion, ValorInicialCSF, ValorInicialSSF, ValorVigenteCSF, ValorVigenteSSF,
                                ValorDisponibleCSF, ValorDisponibleSSF, ValorSolicitadoCSF, ValorSolicitadoSSF, ValorAprobadoCSF, ValorAprobadoSSF } = fuente;
                            vm.listaGrupoFuentesProyectos.push({ FuenteId, NombreFuente, TipoAccion, ValorVigenteCSF, ValorVigenteSSF, ValorSolicitadoCSF, ValorSolicitadoSSF, ValorAprobadoCSF, ValorAprobadoSSF });
                        });

                        vm.listaFuentesProyecto.forEach(fuente => {
                            vm.listaGrillaFuentesProyectos.push({
                                idfuente: fuente.FuenteId,
                                tiporecurso: fuente.NombreFuente + ' - ' + (fuente.Origen === 'N' ? 'Nación' : 'Propios'),
                                TipoAccion: fuente.TipoAccion,
                                Origen : fuente.Origen,
                                TipoProyecto: fuente.TipoProyecto,
                                ValorInicialCSF: fuente.ValorInicialCSF ? formatearNumero(fuente.ValorInicialCSF) : 0,
                                ValorInicialSSF: fuente.ValorInicialSSF ? formatearNumero(fuente.ValorInicialSSF) : 0,
                                ValorVigenteCSF: fuente.ValorVigenteCSF ? formatearNumero(fuente.ValorVigenteCSF) : 0,
                                ValorVigenteSSF: fuente.ValorVigenteSSF ? formatearNumero(fuente.ValorVigenteSSF) : 0,
                                ValorDisponibleCSF: fuente.ValorDisponibleCSF ? formatearNumero(fuente.ValorDisponibleCSF) : 0,
                                ValorDisponibleSSF: fuente.ValorDisponibleSSF ? formatearNumero(fuente.ValorDisponibleSSF) : 0,
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
                        vm.gridOptions.data = vm.listaGrillaFuentesProyectos;
                        vm.filasFiltradas = vm.gridOptions.data.length > 0;
                        vm.actualizaValores();
                    }
                });
        }

        function guardarAprobacionFuentes() {
            let fuentes = [];
            var valoraprobadoCSF = 0;
            var valoraprobadoSSF = 0;
            var valorsolicitadoCSF = 0;
            var valorsolicitadoSSF = 0;
            var pasovalidacion = true;
            vm.gridOptions.data.map(function (item, index) {
                valoraprobadoCSF = parseInt(limpiaNumero(item.ValorAprobadoCSF === '' || item.ValorAprobadoCSF === undefined  ? 0 : item.ValorAprobadoCSF));
                valoraprobadoSSF = parseInt(limpiaNumero(item.ValorAprobadoSSF === '' || item.ValorAprobadoSSF === undefined ? 0 : item.ValorAprobadoSSF));
                valorsolicitadoCSF = parseInt(limpiaNumero(item.ValorSolicitadoCSF === '' || item.ValorSolicitadoCSF === undefined ? 0 : item.ValorSolicitadoCSF));
                valorsolicitadoSSF = parseInt(limpiaNumero(item.ValorSolicitadoSSF === '' || item.ValorSolicitadoSSF === undefined ? 0 : item.ValorSolicitadoSSF));
                if (valoraprobadoCSF > valorsolicitadoCSF || valoraprobadoSSF > valorsolicitadoSSF)
                    pasovalidacion = false;

                if (pasovalidacion) {
                    let c = {
                        TramiteId: vm.TramiteId,
                        ProyectoId: vm.ProyectoId,
                        EntidadId: vm.EntidadId,
                        FuenteId: item.idfuente,
                        TipoAccion: item.TipoAccion,
                        ValorAprobadoCSF: valoraprobadoCSF,
                        ValorAprobadoSSF: valoraprobadoSSF
                    };
                    fuentes.push(c);
                }

            });

            if (!pasovalidacion) {
                utilidades.mensajeError("El valor aprobado en mayor al valor solicitado en alguno de los registros!");
                return;
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

        function actualizaValoresPorFuente (){
            vm.ValorInicialNacion = 0;
            vm.ValorVigenteNacion = 0;
            vm.ValorDisponibleNacion = 0;
            vm.ValorVigenciasFuturasNacion = 0;
            vm.ValorInicialPropios = 0;
            vm.ValorVigentePropios = 0;
            vm.ValorDisponiblePropios = 0;
            vm.ValorVigenciasFuturasPropios = 0;
            var proyectoidtmp = vm.ProyectoId === undefined ? 0 : vm.ProyectoId;
            var entidadtmp = vm.EntidadId === undefined ? 0 : vm.EntidadId;
            var tr = trasladosServicio.obtenerValoresProyectos(vm.TramiteId, proyectoidtmp, entidadtmp)
                .then(function (result) {
                    if (result != null && result.data != undefined) {
                                vm.ValorInicialNacion = formatearNumero(result.data.DecretoNacion);
                                vm.ValorVigenteNacion = formatearNumero(result.data.VigenteNacion);
                                vm.ValorDisponibleNacion = formatearNumero(result.data.DisponibleNacion);
                                vm.ValorVigenciasFuturasNacions = formatearNumero(result.data.VigenciaFuturaNacion);
                                vm.ValorInicialPropios = formatearNumero(result.data.DecretoPropios);
                                vm.ValorVigentePropios = formatearNumero(result.data.VigentePropios);
                                vm.ValorDisponiblePropios = formatearNumero(result.data.DisponiblePropios);
                                vm.ValorVigenciasFuturasPropios = formatearNumero(result.data.VigenciaFuturaPropios);
                       
                    }
                });
        }

        vm.actualizaValores = function() {
            vm.listaGrupoProyectos = [];
            vm.ValorTotalMontoCSFNacion = 0;
            vm.ValorTotalMontoSSFNacion = 0;
            vm.ValorTotalMontoCSFPropios = 0;
            vm.ValorTotalMontoSSFPropios = 0;
            vm.ValorTotalCSF = 0;
            vm.ValorTotalSSF = 0;
            $timeout(function () {
                $scope.$apply(function () {
                    vm.gridOptions.data.forEach(proyectoentidad => {

                        if (proyectoentidad.Origen === 'P') {
                            vm.ValorTotalMontoCSFPropios += parseInt(limpiaNumero(proyectoentidad.ValorAprobadoCSF));
                            vm.ValorTotalMontoSSFPropios += parseInt(limpiaNumero(proyectoentidad.ValorAprobadoSSF));
                           
                        }
                        else if (proyectoentidad.Origen === 'N'){
                            vm.ValorTotalMontoCSFNacion += parseInt(limpiaNumero(proyectoentidad.ValorAprobadoCSF));
                            vm.ValorTotalMontoSSFNacion += parseInt(limpiaNumero(proyectoentidad.ValorAprobadoSSF));
                           
                        }
                       
                    });
                    vm.ValorTotalCSF = (vm.ValorTotalMontoCSFPropios + vm.ValorTotalMontoCSFNacion);
                    vm.ValorTotalSSF = (vm.ValorTotalMontoSSFNacion + vm.ValorTotalMontoSSFPropios);
                    vm.ValorTotalMontoCSFNacion = formatearNumero(vm.ValorTotalMontoCSFNacion);
                    vm.ValorTotalMontoSSFNacion = formatearNumero(vm.ValorTotalMontoSSFNacion);
                    vm.ValorTotalMontoCSFPropios = formatearNumero(vm.ValorTotalMontoCSFPropios);
                    vm.ValorTotalMontoSSFPropios = formatearNumero(vm.ValorTotalMontoSSFPropios);
                    vm.ValorTotalCSF = formatearNumero(vm.ValorTotalCSF);
                    vm.ValorTotalSSF = formatearNumero(vm.ValorTotalSSF);
                });
            });
              
           

        }

        vm.actualizaFila = function (event, tipovalor) {
            var valor = '0';
            $(event.target).val(function (index, value) {
                valor = formatearNumero(value === '' ? 0 : value);
            });
            const btnScope = angular.element(event.target).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
            if (tipovalor === 1)
                entity.ValorAprobadoCSF = valor;
            else if (tipovalor === 2)
                entity.ValorAprobadoSSF = valor;
            vm.actualizaValores(entity);
           

            //const btnScope = angular.element(sender).scope();
            //const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
            //vm.actualizaValores();
            //$(event.target).val(function (index, value) {
            //    return formatearNumero(value === '' ? 0 : value);
            //});



        }

        vm.onKeyPress_st = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();;
            }

        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }


        function retornar() {
            $timeout(function () {
                $location.path('/seleccionProyectosTramite');
            }, 300);
        }

    }

    angular.module('backbone').component('aprobacionFuentes', {
        templateUrl: "src/app/formulario/ventanas/tramites/trasladosAprobacionFuentes.html",
        controller: aprobacionFuentesController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();