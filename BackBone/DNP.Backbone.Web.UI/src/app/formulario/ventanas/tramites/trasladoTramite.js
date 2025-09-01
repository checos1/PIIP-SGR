
(function () {
    'use strict';

    trasladosTramiteController.$inject = [
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
        'servicioCreditos',
        'servicioResumenDeProyectos',
        'uiGridConstants',
        '$timeout',
        '$location',
        'archivoServicios'
    ];



    function trasladosTramiteController(
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
        servicioCreditos,
        servicioResumenDeProyectos,
        uiGridConstants,
        $timeout,
        $location,
        archivoServicios
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.visibleValidar = true;
        //vm.idInstancia = $sessionStorage.idInstanciaIframe;

        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
        vm.observacionAnterior = '';

        vm.idnivel = $sessionStorage.idNivel;

        vm.seleccionProyectos = false;
        vm.aprobacionEntidad = false;
        vm.elaboracionConcepto = false;
        vm.revisionConcepto = false;
        vm.aprobaciónTramite = false;

        setTimeout(function () {
            if (vm.noJefePlaneacion == false)
                vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
            else
                vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
        }, 3000);


        vm.activartraslados = false;

        vm.listaProyectoContraCredito = [];

        vm.eventoValidar = eventoValidar;
        vm.idTipoTramite = "";
        //vm.cargarEntidades = cargarEntidades;
        //vm.buscar = buscar;
        //vm.idEntidad = "1C58FFF0-E999-44C9-B4BE-0176A3CF73A5";
        vm.numeroTramite = 0;
        vm.nombreSectorTramite = '';
        vm.FechaCreacionTramite = '';
        vm.DescripcionTramite = '';
        vm.TramiteId = 0;
        vm.nombreEntidad = "";
        vm.nombreTipoTramite = "";
        vm.tipoTramiteId = 0;
        vm.etapa = $routeParams['etapa'];  // 'ej'
        vm.listaFiltroProyectosC = [];
        vm.listaFiltroProyectosD = [];
        vm.ProyectosSeleccionadosC = [];
        vm.ProyectosSeleccionadosD = [];
        vm.gridOptions;

        vm.ObtenerProyectosContracredito = ObtenerProyectosContracredito;
        vm.ObtenerProyectosCredito = ObtenerProyectosCredito;
        vm.ObtenerProyectos = ObtenerProyectos;
        vm.agregarProyectos = agregarProyectos;
        vm.generarInstanciasMasiva = generarInstanciasMasiva;
        vm.actualizaCombos = actualizaCombos;
        vm.limpiarCombos = limpiarCombos;
        vm.ObtenerProyectosTramite = vm.ObtenerProyectosTramite;
        vm.btnEliminarProyectoTramite = vm.btnEliminarProyectoTramite;
        vm.generarInstancia = generarInstancia;
        vm.ActualizarInstanciaProyecto = ActualizarInstanciaProyecto;
        vm.ActualizarValoresProyecto = ActualizarValoresProyecto;
        vm.ValidarEnviarDatosTramite = ValidarEnviarDatosTramite;
        vm.desactivartraslados = desactivartraslados;
        vm.consultarArchivosTramite = consultarArchivosTramite;


        vm.ValorTotalMontoRC = 0;
        vm.ValorTotalMontoNacionRC = 0;
        vm.ValorTotalMontoPropiosRC = 0;
        vm.ValorTotalMontoRCC = 0;
        vm.ValorTotalMontoNacionRCC = 0;
        vm.ValorTotalMontoPropiosRCC = 0;

        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = (vm.noJefePlaneacion == true && !$sessionStorage.soloLectura) ? 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyecto.html' : 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyectoJefe.html';
        vm.montosProyectoTemplate = '<div><input disabled="disabled" class="form-control text-right" style="height: 30px;" id="textmontoproyecto_{{row.entity.ProyectoId}}" ng-model="row.entity.ValorMontoProyecto"   style="width:70%" /></div>';
        vm.montosTramiteTemplate = '<div><input class="form-control text-right" style="height: 30px;" id="textmontotramite_{{row.entity.ProyectoId}}" ng-model="row.entity.ValorMontoEnTramite" placeholder="Digite el valor" ng-change="vm.totalizar()" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event, $event.target)" style="width:70%" /></div>';


        vm.filtros = {
            nombreUsuario: null,
            cuentaUsuario: null,
            estado: true,

            catalogos: {

                entidades: [],
                usuarioLista: []
            }
        };

        vm.etapa = "ej",

            vm.peticionObtenerInbox = {
                // ReSharper disable once UndeclaredGlobalVariableUsing
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoTramite,
                // ReSharper disable once UndeclaredGlobalVariableUsing
                Aplicacion: nombreAplicacionBackbone,
                IdInstancia: $sessionStorage.idInstanciaIframe,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
                IdsEtapas: getIdEtapa()
            };
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: $sessionStorage.idInstanciaIframe,
            IdEntidad: vm.IdEntidadSeleccionada
        };

        //vm.peticionObtenerInbox = {
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    IdUsuario: 'CC505050',//'wmunoz@dnp.gov.co',
        //    IdObjeto: idTipoTramite,
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    Aplicacion: nombreAplicacionBackbone,
        //    IdInstancia: '7C005B81-B9EB-4779-B005-7A752BCB05CC', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83',
        //    ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
        //    IdsEtapas: getIdEtapa()
        //};

        //vm.parametros = {
        //    idFlujo: '75F74D06-BCEF-4BF0-B939-91AADA18D563', //'3D1BC935-7910-4DD4-890D-2EAB7AF8C995',
        //    tipoEntidad: 'Nacional',
        //    idInstancia: '7C005B81-B9EB-4779-B005-7A752BCB05CC', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83', 
        //    IdEntidad: vm.IdEntidadSeleccionada
        //};


        vm.onKeyPress = function (e) {
            const charCode = e.which ? e.which : e.keyCode;
            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
            }
        };

        function onKeypressEvent(event) {
            $(event.target).val(function (index, value) {
                value = parseFloat(value).toFixed(0);
                return value.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
            });
        }

        function onKeypressDecimalEvent(event) {
            $(event.target).val(function (index, value) {
                value = parseFloat(value).toFixed(2);
                return value.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
            });
        }

        function removerDecimales(event) {
            $(event.target).val(function (index, value) {
                return value.replaceAll(",", "");
            });
        }

        function totalizar() {
            alert('totalizar');
        }

        vm.actualizaFila = function (event, sender) {
            const btnScope = angular.element(sender).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
            ActualizarValoresProyecto(entity, 0);

            $(event.target).val(function (index, value) {
                return formatearNumero(value === '' ? 0 : value);
            });
        };

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }


        vm.columnDefPrincial = [{
            field: 'Name',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate
        }];

        vm.proyectoTemplate = '<div class="row text-left" style="margin-left: 0;"> <label>{{row.entity.BPIN}} </label> </div > ' +
            '<div class="row text-left    " style="margin-left: 0;font-weight: 700;"> <label style="font-weight: 700;">{{row.entity.NombreProyecto}}</label> </div > ' +
            '<div style="height:12px;"></div><div class="row text-left    " style="margin-left: 0;"> <label><span>Cod. programa: </span> {{row.entity.Programa}}</label> </div >' +
            '</div><div class="row text-left    " style="margin-left: 0;"> <label><span>Cod. subprograma: </span> {{row.entity.SubPrograma}}</label> </div >';

        vm.montoProyectoTemplate = '<div class="row text-right" style="margin-right: 15px;"> <label>Nación </label> </div >' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorMontoProyectoNacion}}</label> </div > ' +
            '<div style="height:12px;"></div><div class="row text-right" style="margin-right: 15px;"> <label>Propios </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorMontoProyectoPropios}}</label> </div > ';


        vm.montoTramiteTemplate = (vm.disabledJefePlaneacion || $sessionStorage.soloLectura) ? '<div class="row text-right" style="margin-right: 15px;"> <label>Nación </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-model="row.entity.ValorMontoTramiteNacion" id="textmontotramitenacion_{{row.entity.ProyectoId}}"  disabled /> </div > ' +
            '<div style="height:12px;"></div><div class="row text-right" style="margin-right: 15px;"> <label>Propios </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;" ng-model="row.entity.ValorMontoTramitePropios" id="textmontotramitepropio_{{row.entity.ProyectoId}}" disabled /> </div > '
            :
            '<div class="row text-right" style="margin-right: 15px;"> <label>Nación </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;"  ng-value="row.entity.ValorMontoTramiteNacion" id="textmontotramitenacion_{{row.entity.ProyectoId}}" ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila_st($event, 1)" /> </div > ' +
            '<div style="height:12px;"></div><div class="row text-right" style="margin-right: 15px;"> <label>Propios </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <input style="font-weight: 700;text-align:right;width:90%;"  ng-value="row.entity.ValorMontoTramitePropios" id="textmontotramitepropio_{{row.entity.ProyectoId}}" ng-keypress="grid.appScope.onKeyPress_st($event)" ng-blur="grid.appScope.actualizaFila_st($event,  2)" /> </div > ';

        vm.campoEstadoActualizacion = '<div class="row text-left" style="margin-left: 15px;"> <label style="font-weight: 700;">{{row.entity.EstadoActualizacion}}</label> </div > ';

        vm.columnDef = [
            {
                field: 'pro',
                displayName: 'Proyecto',
                enableHiding: false,
                enableColumnMenu: false,
                width: '20%',
                pinnedRight: true,
                cellTemplate: vm.proyectoTemplate
            },
            {
                field: 'TipoProyecto',
                displayName: 'Tipo operación',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
                cellClass: 'negrita text-center'
            },
            {
                field: 'CodigoPresupuestal',
                displayName: 'Código presupuestal',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellClass: 'negrita text-center'

            },
            {
                field: 'mp',
                displayName: 'Monto del proyecto $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellTemplate: vm.montoProyectoTemplate
            },
            {
                field: 'mt',
                displayName: 'Monto del tramite $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellTemplate: vm.montoTramiteTemplate
            },
            {
                field: 'Estado',
                displayName: 'Estado proyecto',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
                cellClass: 'text-center'

            },
            {
                field: 'EstadoActualizacion',
                displayName: 'Estado actualización',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
                cellClass: 'text-center',
                cellTemplate: vm.campoEstadoActualizacion

            },
            {
                field: 'accion',
                displayName: 'Acciones',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                pinnedRight: true,
                cellTemplate: vm.accionesFilaProyectoTemplate,
                width: '10%',
                cellClass: 'text-center'
            }

            
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);

        function getIdEtapa() {
            var idEtapa = [];
            switch (vm.etapa) {
                case 'pl':
                    idEtapa = [constantesBackbone.idEtapaPlaneacion, constantesBackbone.idEtapaViabilidadRegistro, constantesBackbone.idEtapaAjustes, constantesBackbone.idEtapaPriorizacion];
                    break;
                case 'pr':
                    idEtapa = [constantesBackbone.idEtapaProgramacion];
                    break;
                case 'gr':
                    idEtapa = [constantesBackbone.idEtapaGestionRecursos, constantesBackbone.idEtapaSolicitudRecursos, constantesBackbone.idEtapaRevisionRequisitos, constantesBackbone.idEtapaAprobacion, constantesBackbone.idEtapaAjustesGR];
                    break;
                case 'ej':
                    idEtapa = [constantesBackbone.idEtapaEjecucion, constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = [constantesBackbone.idEtapaEvaluacion, constantesBackbone.idEtapaCortoPlazo, constantesBackbone.idEtapaMedianoPlazo, constantesBackbone.idEtapaLargoPlazo];
                    break;
            }
            return idEtapa;
        }


        vm.listaEntidades = [];
        vm.listaFiltroEntidades = [];
        // grid main
        vm.gridOptions;


        vm.tramiteFiltro = {
            descripcion: {
                campo: 'Descripcion',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            fechaDesde: {
                campo: 'FechaCreacion',
                valor: null,
                tipo: constantesCondicionFiltro.mayorIgual
            },
            fechaHasta: {
                campo: 'FechaCreacion',
                valor: null,
                tipo: constantesCondicionFiltro.menorIgual
            },
            sectorId: {
                campo: 'SectorId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            entidadId: {
                campo: 'EntidadId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            tipoEntidad: {
                campo: 'NombreTipoEntidad',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            nombreFlujo: {
                campo: 'TipoTramite.Nombre',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            accionFlujo: {
                campo: 'NombreAccion',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            }
        };
        //#region Métodos
        function eventoValidar() {
            if (vm.IdEntidadSeleccionada == "" || vm.IdEntidadSeleccionada == null || vm.IdEntidadSeleccionada == undefined) {
                vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                utilidades.mensajeError("Seleccione una entidad.");
            }
            else {
                vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                utilidades.mensajeSuccess("Formulario validado correctamente.");
            }
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        vm.init = function () {
            vm.tipoEntidad = 'Nacional';
            vm.filtro = '';
            obtenerTramite();

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
                    showHeader: false,
                    useExternalFiltering: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: onRegisterApi
                };
                vm.gridOptions.appScopeProvider = vm;
            }

            if (vm.idnivel == constantesBackbone.idNivelSeleccionProyectos)
                vm.seleccionProyectos = true;
            else if (vm.idnivel == constantesBackbone.idNivelAprobacionEntidad)
                vm.aprobacionEntidad = true;
            else if (vm.idnivel == constantesBackbone.idNivelElaboracionConcepto)
                vm.elaboracionConcepto = true;
            else if (vm.idnivel == constantesBackbone.idNivelRevisionConcepto)
                vm.revisionConcepto = true;
            else if (vm.idnivel == constantesBackbone.idNivelAprobacionTramite)
                vm.aprobaciónTramite = false;
        };

        vm.actualizaFila_st = function (event,  tipovalor) {
            var valor = '';
            $(event.target).val(function (index, value) {
                valor =  value === '' ? 0 : value;
            });
            const btnScope = angular.element(event.target).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
            if (tipovalor === 1)
                entity.ValorMontoTramiteNacion = valor;
            else if (tipovalor === 2)
                entity.ValorMontoTramitePropios = valor;
            vm.actualizaValores(entity);
            $(event.target).val(function (index, value) {
                return formatearNumero(value === '' ? 0 : value);
            });



        }

        vm.onKeyPress_st = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();;
            }

        }

        function obtenerTramite() {

            let listaColumnas = [];
            return trasladosServicio.obtenerTramites(vm.peticionObtenerInbox, vm.tramiteFiltro, listaColumnas).then(
                function (respuesta) {

                    vm.listaEntidades = [];
                    vm.listaTramites = [];
                    vm.listaDatos = [];

                    let listaEntidadesGrid = [];
                    if (respuesta.data.ListaGrupoTramiteEntidad && respuesta.data.ListaGrupoTramiteEntidad.length > 0) {
                        vm.listaEntidades = [];
                        const listaGrupoEntidades = respuesta.data.ListaGrupoTramiteEntidad;
                        listaGrupoEntidades.forEach(entidad => {
                            const nombreEntidad = entidad.NombreEntidad;

                            const idEntidad = entidad.EntidadId;
                            vm.idEntidad = entidad.EntidadId;

                            vm.listaTramites = [];
                            entidad.GrupoTramites.forEach(tramite => {

                                const nombreTipoTramite = tramite.NombreTipoTramite;
                                vm.listaDatos = [];
                                tramite.ListaTramites.forEach(instancia => {
                                    listaEntidadesGrid.push({ Id: instancia.EntidadId, Name: instancia.NombreObjetoNegocio });
                                    vm.listaDatos.push({
                                        codigo: instancia.Id,
                                        descripcion: instancia.Descripcion,
                                        fecha: instancia.FechaCreacion,
                                        valorProprio: instancia.ValorProprio,
                                        valorSGR: instancia.ValorSGP,
                                        tipoTramite: instancia.NombreTipoTramite,
                                        entidad: nombreEntidad,
                                        identificadorCR: instancia.IdentificadorCR,
                                        estadoTramite: instancia.DescEstado,
                                        sector: instancia.NombreSector,
                                        estadoId: instancia.EstadoId,
                                        tipoTramiteId: instancia.TipoTramiteId,
                                        IdObjetoNegocio: instancia.IdObjetoNegocio,
                                        NombreObjetoNegocio: instancia.NombreObjetoNegocio,
                                        NombreAccion: instancia.NombreAccion,
                                        IdInstancia: instancia.IdInstancia,
                                        NombreFlujo: instancia.NombreTipoTramite,
                                        entidadId: idEntidad,
                                        IdAccion: instancia.IdAccion
                                    });

                                    vm.numeroTramite = instancia.NumeroTramite;
                                    vm.nombreEntidad = instancia.NombreObjetoNegocio;
                                    vm.nombreTipoTramite = instancia.NombreTipoTramite;
                                    vm.TramiteId = instancia.TramiteId;
                                    vm.tipoTramiteId = instancia.TipoTramiteId;
                                    vm.nombreSectorTramite = instancia.NombreSectorTramite;
                                    vm.FechaCreacionTramite = instancia.FechaCreacionTramite.toString().replace("T", " ").substring(0, 19);
                                    vm.DescripcionTramite = instancia.DescripcionTramite;
                                    $sessionStorage.TipoTramiteId = instancia.TipoTramiteId;
                                });
                            });

                            vm.listaEntidades.push({
                                sector: vm.nombreEntidad,
                                entidad: vm.nombreEntidad,
                                tipoEntidad: vm.nombreEntidad,
                                subGridOptions: {
                                    columnDefs: vm.columnDef,
                                    enableVerticalScrollbar: 1,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,

                                    data: vm.listaDatos,
                                    // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                }
                            });

                        });

                        vm.listaFiltroEntidades = [];
                        $.each(listaEntidadesGrid, function (i, el) {
                            if ($.inArray(el, vm.listaFiltroEntidades) === -1) {
                                vm.listaFiltroEntidades.push(el);
                            }
                        });
                        ObtenerProyectos();
                        ObtenerProyectosTramite();
                        consultarArchivosTramite();
                    }

                },

            );
        }

        //#endregion

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = '';

            for (let i = 0; i < 2; i++) {
                var nombreColumnasSeleccionadaFiltro = 'Nombre Flujo';

                if (nombreColumnasSeleccionadaFiltro === 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro === 'Accion Flujo') {
                    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                }

                columna = vm.todasColumnasDefinicion.filter(x => x.displayName === nombreColumnasSeleccionadaFiltro)[0].field;
                if (listaColumnas.indexOf(columna) === -1) {
                    listaColumnas.push(columna);
                }
            }

            return listaColumnas;
        }

        function actualizaCombos() {
            ObtenerProyectosContracredito();
            ObtenerProyectosCredito();
        }
        function ObtenerProyectosContracredito() {

            vm.listaFiltroProyectosC = [];
            let listaProyectosGrid = [];

            if (vm.listaProyectosC && vm.listaProyectosC.length > 0 && vm.IdEntidadSeleccionada > 0) {
                //if (vm.listaProyectosC.length > 0) {
                vm.listaProyectosC.forEach(proyecto => {
                    if (proyecto.IdEntidad === vm.IdEntidadSeleccionada) {
                        listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
                    }
                });

                $.each(listaProyectosGrid, function (i, el) {
                    if ($.inArray(el, vm.listaFiltroProyectosC) === -1) {
                        vm.listaFiltroProyectosC.push(el);
                    }
                });
                //}  
            }

        }
        function ObtenerProyectosCredito() {
            vm.listaFiltroProyectosD = [];
            let listaProyectosGrid = [];


            if (vm.listaProyectosD && vm.listaProyectosD.length > 0 && vm.IdEntidadSeleccionada > 0) {
                //if (vm.listaProyectosC.length > 0) {
                vm.listaProyectosD.forEach(proyecto => {
                    //if (proyecto.IdEntidad === vm.IdEntidadSeleccionada) {
                    listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
                    //}                    
                });

                $.each(listaProyectosGrid, function (i, el) {
                    if ($.inArray(el, vm.listaFiltroProyectosD) === -1) {
                        vm.listaFiltroProyectosD.push(el);
                    }
                });

            }

        }

        function ObtenerProyectos() {

            let listaProyectosGrid = [];
            var prm = {
                idFlujo: vm.parametros.idFlujo, tipoEntidad: vm.parametros.tipoEntidad, IdEntidad: vm.idEntidad, idInstancia: vm.parametros.idInstancia
            };

            servicioCreditos.obtenerContraCreditos(prm)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaProyectosC = response.data;
                    }
                });

            vm.listaProyectosD = [];
            servicioCreditos.obtenerCreditos(prm)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaProyectosD = response.data;
                    }
                });

            //ObtenerProyectosContracredito();
            //ObtenerProyectosCredito();
        }

        function agregarProyectos() {
            let proyectos = [];

            if (vm.ProyectosSeleccionadosC && vm.ProyectosSeleccionadosC.length > 0) {
                vm.ProyectosSeleccionadosC.forEach(proyecto => {
                    var datoproyectoC = vm.listaProyectosC.filter(function (datoproyectoC) {
                        return datoproyectoC.IdProyecto === proyecto;
                    });
                    datoproyectoC.forEach(p => {
                        let c = {
                            ProyectoId: p.IdProyecto,
                            EntidadId: p.IdEntidad,
                            TipoProyecto: 'Contracredito',
                            NombreProyecto: p.NombreProyecto
                        };
                        proyectos.push(c);
                    });
                });
            }

            if (vm.ProyectosSeleccionadosD && vm.ProyectosSeleccionadosD.length > 0) {
                vm.ProyectosSeleccionadosD.forEach(proyecto => {
                    var datoproyectoD = vm.listaProyectosD.filter(function (datoproyectoD) {
                        return datoproyectoD.IdProyecto === proyecto;
                    });
                    datoproyectoD.forEach(p => {
                        let c = {
                            ProyectoId: p.IdProyecto,
                            EntidadId: p.IdEntidad,
                            TipoProyecto: 'Credito',
                            NombreProyecto: p.NombreProyecto
                        };
                        proyectos.push(c);
                    });
                });
            }


            var prm = {
                TramiteId: vm.TramiteId,
                Proyectos: proyectos
            };

            trasladosServicio.guardarProyectos(prm)
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

        function generarInstanciasMasiva() {
            var boolproyectossinmontotramite = false;
            var listaproyectos = [];
            vm.continuarMasivo = false;

            vm.variableObligatoriosNoIngresados = localStorage.getItem('contObligatoriosNoIngresados');
            vm.continuarMasivo = true;
            //if (vm.variableObligatoriosNoIngresados === '0') {
            //    vm.continuarMasivo = true;
            //}
            //else
            //    utilidades.mensajeError("No se han ingresado todos los documentos soporte obligatorios para el trámite!");

            if (vm.continuarMasivo == true) {
                vm.listaGrillaProyectos.map(function (item) {
                    if (parseInt(limpiaNumero(item.ValorMontoTramiteNacion)) <= 0 && parseInt(limpiaNumero(item.ValorMontoTramitePropios)) <= 0) {
                        boolproyectossinmontotramite = true;
                    }
                    else
                        listaproyectos.push(item.BPIN);
                });

                if (boolproyectossinmontotramite)
                    utilidades.mensajeError("Hay proyectos que no tienen monto tramite!");
                else {

                    var listaparagenerar = [];
                    var listaqueyatiene = [];
                    trasladosServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                        .then(function (resultado) {
                            listaqueyatiene = resultado.data;
                            vm.listaGrillaProyectos.map(function (item) {
                                var tmp = listaqueyatiene.find(x => x === item.BPIN);
                                if (tmp === undefined)
                                    listaparagenerar.push(item);
                            })

                        }).then(function (result) {
                            if (listaparagenerar.length <= 0)
                                utilidades.mensajeError("Todos los proyectos ya tienen generada la instancia!");
                            else
                                generarInstanciaMasivo(listaparagenerar);
                        });
                }
            }
        }

        function ObtenerProyectosTramite() {

            vm.listaEntidadesProy = [];
            vm.listaGrupoEntidades = [];
            vm.listaGrupoProyectos = [];
            vm.datoproyectosTramite = [];
            vm.listaproyectosEntidad = [];
            vm.listaEntidadesGrilla = [];
            vm.listaGrillaProyectos = [];
            vm.gridOptions.data = vm.listaGrillaProyectos;
            vm.gridOptions.columnDefs = [];

            vm.ValorTotalMontoRC = 0;
            vm.ValorTotalMontoPropiosRC = 0;
            vm.ValorTotalMontoNacionRC = 0;
            vm.ValorTotalMontoRCC = 0;
            vm.ValorTotalMontoNacionRCC = 0;
            vm.ValorTotalMontoPropiosRCC = 0;

            trasladosServicio.obtenerProyectosTramite(vm.TramiteId)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaEntidadesProy = response.data;

                        vm.listaEntidadesProy.forEach(entidad => {
                            if (!vm.listaGrupoEntidades.find(ent => ent.EntidadId === entidad.EntidadId)) {
                                const { Sector, NombreEntidad, EntidadId } = entidad;
                                vm.listaGrupoEntidades.push({ Sector, NombreEntidad, EntidadId });
                            }
                        });

                        vm.listaGrupoEntidades.forEach(entidad => {

                            vm.listaGrupoProyectos = [];

                            vm.listaEntidadesProy.forEach(proyectoentidad => {
                                var MontoProyectoNacion = parseInt(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion);
                                var MontoProyectoPropios = parseInt(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios);
                                var MontoTramiteNacion = parseInt(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                                var MontoTramitePropios = parseInt(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);

                                if (proyectoentidad.TipoProyecto === 'Credito') {
                                    vm.ValorTotalMontoRC += (MontoTramiteNacion + MontoTramitePropios);
                                    vm.ValorTotalMontoNacionRC += (MontoTramiteNacion);
                                    vm.ValorTotalMontoPropiosRC += (MontoTramitePropios);
                                }
                                else {
                                    vm.ValorTotalMontoRCC += (MontoTramiteNacion + MontoTramitePropios);
                                    vm.ValorTotalMontoNacionRCC += (MontoTramiteNacion);
                                    vm.ValorTotalMontoPropiosRCC += (MontoTramitePropios);
                                }
                                proyectoentidad.ValorMontoProyectoNacion = formatearNumero(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion);
                                proyectoentidad.ValorMontoProyectoPropios = formatearNumero(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios);
                                proyectoentidad.ValorMontoTramiteNacion = formatearNumero(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                                proyectoentidad.ValorMontoTramitePropios = formatearNumero(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);





                                if (proyectoentidad.EntidadId === entidad.EntidadId) {
                                    if (!vm.listaGrupoProyectos.find(p => p.EntidadId === proyectoentidad.EntidadId)) {
                                        const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                                        vm.listaGrupoProyectos.push({
                                            BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
                                        });
                                    }
                                }
                            });


                            vm.listaEntidadesGrilla.push({
                                sector: entidad.Sector,
                                entidad: entidad.NombreEntidad,
                                tipoEntidad: 'Nacional',
                                subGridOptions: {
                                    columnDefs: vm.columnDef,
                                    enableVerticalScrollbar: 1,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,

                                    data: vm.listaGrupoProyectos,
                                    // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                }
                            });
                        });

                        vm.ValorTotalMontoRC = formatearNumero(vm.ValorTotalMontoRC);
                        vm.ValorTotalMontoNacionRC = formatearNumero(vm.ValorTotalMontoNacionRC);
                        vm.ValorTotalMontoPropiosRC = formatearNumero(vm.ValorTotalMontoPropiosRC);
                        vm.ValorTotalMontoRCC = formatearNumero(vm.ValorTotalMontoRCC);
                        vm.ValorTotalMontoNacionRCC = formatearNumero(vm.ValorTotalMontoNacionRCC);
                        vm.ValorTotalMontoPropiosRCC = formatearNumero(vm.ValorTotalMontoPropiosRCC);

                        vm.listaGrupoProyectos = [];
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                            vm.listaGrupoProyectos.push({
                                BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
                            });
                        });

                        vm.listaEntidadesProy.forEach(entidad => {
                            vm.listaGrillaProyectos.push({
                                entidad: entidad.NombreEntidad,
                                BPIN: entidad.BPIN,
                                NombreProyecto: entidad.NombreProyecto,
                                ProyectoId: entidad.ProyectoId,
                                TipoProyecto: entidad.TipoProyecto,
                                EntidadId: entidad.EntidadId,
                                Estado: entidad.Estado,
                                ValorMontoProyectoNacion: entidad.ValorMontoProyectoNacion === null ? '0' : entidad.ValorMontoProyectoNacion,
                                ValorMontoProyectoPropios: entidad.ValorMontoProyectoPropios === null ? '0' : entidad.ValorMontoProyectoPropios,
                                ValorMontoTramiteNacion: entidad.ValorMontoTramiteNacion === null ? '0' : entidad.ValorMontoTramiteNacion,
                                ValorMontoTramitePropios: entidad.ValorMontoTramitePropios === null ? '0' : entidad.ValorMontoTramitePropios,
                                Programa: entidad.Programa,
                                SubPrograma: entidad.SubPrograma,
                                EstadoActualizacion: entidad.EstadoActualizacion,
                                CodigoPresupuestal: entidad.CodigoPresupuestal,

                                subGridOptions: {
                                    columnDefs: vm.columnDef,
                                    showHeader: true,
                                    enableVerticalScrollbar: 1,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,
                                    data: vm.listaGrupoProyectos
                                    // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                }
                            });

                        });

                        vm.gridOptions.showHeader = true;
                        vm.gridOptions.columnDefs = vm.columnDef;
                        vm.gridOptions.data = vm.listaGrillaProyectos;
                        vm.filasFiltradas = vm.gridOptions.data.length > 0;
                        vm.gridOptions.enableVerticalScrollbar = vm.scrollbars.WHEN_NEEDED;


                    }
                });
        }

        vm.actualizaValores = function (entity) {
            vm.ValorTotalMontoRC = 0;
            vm.ValorTotalMontoNacionRC = 0;
            vm.ValorTotalMontoPropiosRC = 0;
            vm.ValorTotalMontoRCC = 0;
            vm.ValorTotalMontoNacionRCC = 0;
            vm.ValorTotalMontoPropiosRCC = 0;
            vm.gridOptions.data.forEach(proyectoentidad => {
                var MontoTramiteNacion = parseInt(proyectoentidad.ValorMontoTramiteNacion === '' || proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion.replaceAll('.', ''));
                var MontoTramitePropios = parseInt(proyectoentidad.ValorMontoTramitePropios === '' || proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios.replaceAll('.', ''));

                if (proyectoentidad.TipoProyecto === 'Credito') {
                    vm.ValorTotalMontoRC += (MontoTramiteNacion + MontoTramitePropios);
                    vm.ValorTotalMontoNacionRC += (MontoTramiteNacion);
                    vm.ValorTotalMontoPropiosRC += (MontoTramitePropios);
                }
                else {
                    vm.ValorTotalMontoRCC += (MontoTramiteNacion + MontoTramitePropios);
                    vm.ValorTotalMontoNacionRCC += (MontoTramiteNacion);
                    vm.ValorTotalMontoPropiosRCC += (MontoTramitePropios);
                }


            })
            vm.ValorTotalMontoRC = formatearNumero(vm.ValorTotalMontoRC);
            vm.ValorTotalMontoNacionRC = formatearNumero(vm.ValorTotalMontoNacionRC);
            vm.ValorTotalMontoPropiosRC = formatearNumero(vm.ValorTotalMontoPropiosRC);
            vm.ValorTotalMontoRCC = formatearNumero(vm.ValorTotalMontoRCC);
            vm.ValorTotalMontoNacionRCC = formatearNumero(vm.ValorTotalMontoNacionRCC);
            vm.ValorTotalMontoPropiosRCC = formatearNumero(vm.ValorTotalMontoPropiosRCC);

            ActualizarValoresProyecto(entity, 0);
        }

        vm.btnIniciarInstanciaProyecto_onClick = function ($event, sender) {
            if (!vm.visibleValidar) {
                return;
            }

            vm.variableObligatoriosNoIngresados = localStorage.getItem('contObligatoriosNoIngresados');
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
                var montotramitenacion = parseInt(entity.ValorMontoTramiteNacion.replaceAll(".", ""));
                var montotramitepropio = parseInt(entity.ValorMontoTramitePropios.replaceAll(".", ""));
                var montoTramite = montotramitenacion + montotramitepropio;
                var montoproyecto = parseInt(limpiaNumero(entity.ValorMontoProyectoPropios)) + parseInt(limpiaNumero(entity.ValorMontoProyectoNacion));

                if (montoTramite === 0 || montoTramite === "0" || montoTramite === "") {
                    utilidades.mensajeError("No ha ingresado el monto del trámite!");
                } else {
                    if (parseInt(montoTramite, 10) > parseInt(montoproyecto, 0) && entity.TipoProyecto === "Contracredito") {
                        utilidades.mensajeError("El monto del trámite no puede ser mayor al monto del proyecto!");
                    } else {
                        //ActualizarValoresProyecto(entity, 0);
                        var listaproyectos = [];
                        listaproyectos.push(entity.BPIN);
                        trasladosServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                            .then(function (resultado) {
                                if (resultado.data.length === 0)
                                    //if (vm.variableObligatoriosNoIngresados === '0') {
                                    generarInstancia(entity);
                                //}
                                //else
                                //   utilidades.mensajeError("No se han ingresado todos los documentos soporte obligatorios para el trámite!");
                                else
                                    utilidades.mensajeError("El proyecto ya tiene instacia generada!");
                            });
                    }
                }
            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarInstanciaProyecto_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };


        vm.btnEliminarProyectoTramite_onClick = function ($event, sender) {
            if (!vm.visibleValidar) {
                return;
            }

            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

                var prm = {
                    TramiteId: vm.TramiteId,
                    ProyectoId: entity.ProyectoId
                };

                utilidades.mensajeWarning("Confirma la eliminación del proyecto en el trámite?", function funcionContinuar() {
                    trasladosServicio.eliminarInstanciaProyectoTramite(vm.peticionObtenerInbox.IdInstancia, entity.BPIN)
                        .then(function () {
                            trasladosServicio.eliminarProyectoTramite(vm.peticionObtenerInbox, prm)
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
                        })

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                });
            }
            catch (exception) {
                console.log('controladorProyecto.btnEliminarProyectoTramite_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };

        function limpiarCombos() {
            vm.ProyectosSeleccionadosC = 0;
            vm.ProyectosSeleccionadosD = 0;
        }

        function ActualizarValoresProyecto(proyecto, vermensaje) {
            proyecto.ValorMontoTramiteNacion = proyecto.ValorMontoTramiteNacion === '' || proyecto.ValorMontoTramiteNacion === null ? '0' : proyecto.ValorMontoTramiteNacion;
            proyecto.ValorMontoTramitePropios = proyecto.ValorMontoTramitePropios === '' || proyecto.ValorMontoTramitePropios === null ? '0' : proyecto.ValorMontoTramitePropios;
            var montotramitenacion = parseInt(proyecto.ValorMontoTramiteNacion.replaceAll(".", ""));
            var montotramitepropio = parseInt(proyecto.ValorMontoTramitePropios.replaceAll(".", ""));
            var montoTramite = montotramitenacion + montotramitepropio;

            vm.listaIstanciasCreadas = [];
            //vm.listaIstanciasCreadas = resultado;

            const proyectoTramiteDto = {
                TramiteId: vm.TramiteId,
                ProyectoId: proyecto.ProyectoId,
                EntidadId: proyecto.EntidadId,
                TipoRolId: 1,
                ValorMontoNacionEnTramite: parseInt(limpiaNumero(montotramitenacion), 0),
                ValorMontoPropiosEnTramite: parseInt(limpiaNumero(montotramitepropio), 0)
            };

            trasladosServicio.ActualizarValoresProyecto(proyectoTramiteDto).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (response.data.Exito) {
                            if (vermensaje === 1) {
                                utilidades.mensajeSuccess('Valores actualizados exitosamente.');
                            }
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }
                    } else {
                        swal('', "Error al actualizar valroes del proyecto.", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        function generarInstanciaMasivo(listaparagenerar) {
            var cargo = false;
            var listatramites = [];
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];
            listaparagenerar.map(function (item) {
                var tramiteDto = {
                    FlujoId: vm.parametros.idFlujo,
                    ObjetoId: item.BPIN,
                    UsuarioId: vm.peticionObtenerInbox.IdUsuario,
                    RolId: usuarioRolId,
                    TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                    ListaEntidades: [item.EntidadId],
                    IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                    Proyectos: [{
                        IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                        IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                        IdObjetoNegocio: item.BPIN,
                        IdEntidad: item.EntidadId,
                        IdAccion: '',
                        ProyectoId: item.ProyectoId,
                        FlujoId: vm.parametros.idFlujo
                    }]
                };
                listatramites.push(tramiteDto);

            });

            flujoServicios.generarInstanciaMasivo(listatramites)
                .then(function (resultado) {
                    if (!resultado.length) {
                        utilidades.mensajeError('No se creó la instancia');
                        return;
                    }
                    var instanciasFallidas = resultado.filter(function (instancia) {
                        return !instancia.Exitoso;
                    });
                    var cantidadInstanciasFallidas = instanciasFallidas.length;

                    if (cantidadInstanciasFallidas) {
                        utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                    } else {
                        listaparagenerar.map(function (item) {
                            // var tmpresult = resultado.find(x => x.id);
                            ActualizarInstanciaProyecto(item, resultado);
                            cargo = true;
                        })

                    }

                },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error);
                        }
                    }).then(function (result) {
                        if (cargo)
                            utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                    });;

        }

        function generarInstancia(proyecto) {
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];


            const tramiteDto = {
                FlujoId: vm.parametros.idFlujo,
                ObjetoId: proyecto.BPIN,
                UsuarioId: vm.peticionObtenerInbox.IdUsuario,
                RolId: usuarioRolId,
                TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                ListaEntidades: [proyecto.EntidadId],
                IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                Proyectos: [{
                    IdInstancia: vm.peticionObtenerInbox.IdInstancia,
                    IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                    IdObjetoNegocio: proyecto.BPIN,
                    IdEntidad: proyecto.EntidadId,
                    IdAccion: '',
                    ProyectoId: proyecto.ProyectoId,
                    FlujoId: vm.parametros.idFlujo
                }]
            };

            utilidades.mensajeWarning("Confirma la creación de la instancia para el proyecto?", function funcionContinuar() {
                flujoServicios.generarInstancia(tramiteDto)
                    .then(function (resultado) {
                        if (!resultado.length) {
                            utilidades.mensajeError('No se creó la instancia');
                            return;
                        }
                        var instanciasFallidas = resultado.filter(function (instancia) {
                            return !instancia.Exitoso;
                        });
                        var cantidadInstanciasFallidas = instanciasFallidas.length;

                        if (cantidadInstanciasFallidas) {
                            utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                        } else {
                            ActualizarInstanciaProyecto(proyecto, resultado);
                            utilidades.mensajeSuccess('Subproceso de gestión de recursos creado exitosamente ');
                        }

                    });
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });



        }

        function ActualizarInstanciaProyecto(proyecto, resultado) {

            var montotramitenacion = parseInt(proyecto.ValorMontoTramiteNacion.replaceAll(".", ""));
            var montotramitepropio = parseInt(proyecto.ValorMontoTramitePropios.replaceAll(".", ""));
            var montoTramite = montotramitenacion + montotramitepropio;

            vm.listaIstanciasCreadas = [];
            vm.listaIstanciasCreadas = resultado;

            vm.listaIstanciasCreadas.forEach(instancia => {
                const proyectoTramiteDto = {
                    TramiteId: vm.TramiteId,
                    InstanciaId: instancia.InstanciaId,
                    ProyectoId: proyecto.ProyectoId,
                    EntidadId: proyecto.EntidadId,
                    TipoRolId: 1,
                    ValorMontoEnTramite: limpiaNumero(montoTramite)
                };

                trasladosServicio.ActualizarInstanciaProyecto(proyectoTramiteDto).then(
                    function (resultado) {

                        if (resultado.data && (resultado.statusText === "OK" || resultado.status === 200)) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            //generarInstancia(proyecto);
                        } else {
                            swal('', "Error al actualizar la instancia en el proyecto", 'error');
                        }
                    },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error);
                        }
                    }
                );
            });
        }

        vm.btnIniciarRequisitos_onClick = function ($event, sender) {
            var paso = true;
            if (!vm.visibleValidar) {
                return;
            }
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
                var montotramitenacion = parseInt(entity.ValorMontoTramiteNacion.replaceAll(".", ""));
                var montotramitepropio = parseInt(entity.ValorMontoTramitePropios.replaceAll(".", ""));
                var montoTramite = montotramitenacion + montotramitepropio;
                var montoproyecto = entity.ValorMontoProyectoPropios + entity.ValorMontoProyectoNacion;

                if (montoTramite <= 0) {
                    utilidades.mensajeError("Debe diligenciar los valores del monto solicitado primero!");
                    paso = false;
                }

                if (entity.TipoProyecto === 'Credito') {
                    vm.gridOptions.data.map(function (item) {
                        var montotramitenacion = parseInt(limpiaNumero(item.ValorMontoTramiteNacion));
                        var montotramitepropios = parseInt(limpiaNumero(item.ValorMontoTramitePropios));
                        if (item.TipoProyecto === 'Contracredito' && entity.Programa === item.Programa && entity.Subprograma === item.Subprograma && montotramitenacion <= 0 && montotramitepropios <= 0) {
                            utilidades.mensajeError("Debe diligenciar los proyectos contracredito primero!");
                            paso = false;
                        }

                    });

                }
                else {
                    if (parseInt(limpiaNumero(montoTramite), 10) > parseInt(limpiaNumero(montoproyecto), 10) && entity.TipoProyecto === "Contracredito") {
                        utilidades.mensajeError("El monto del trámite no puede ser mayor al monto del proyecto!");
                        paso = false;
                    }
                }
                if (paso) {
                    $sessionStorage.ProyectoId = entity.ProyectoId;
                    $sessionStorage.TipoProyecto = entity.TipoProyecto;
                    $sessionStorage.MontoTramiteNacion = montotramitenacion;
                    $sessionStorage.MontoTramitePropios = montotramitepropio;
                    $sessionStorage.EntidadId = entity.EntidadId;
                    $sessionStorage.TipoTramiteId = vm.tipoTramiteId;
                    $sessionStorage.TramiteId = vm.TramiteId;
                    $sessionStorage.TipoRolId = 1;
                    $sessionStorage.NombreProyecto = entity.NombreProyecto;
                    $sessionStorage.BPIN = entity.BPIN;
                    $sessionStorage.numeroTramite = vm.numeroTramite;
                    $sessionStorage.nombreEntidad = vm.nombreEntidad;
                    $sessionStorage.nombreTipoTramite = vm.nombreTipoTramite;
                    vm.activartraslados = true;
                    $sessionStorage.accionEjecutandose = 'Ver requisitos proyecto';
                }



            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarRequisitos_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };

        vm.btnIniciarRequisitosTramite_onClick = function ($event, sender) {
            var paso = true;
            if (!vm.visibleValidar) {
                return;
            }
            try {

                //$sessionStorage.MontoTramiteNacion = montotramitenacion;
                //$sessionStorage.MontoTramitePropios = montotramitepropio;

                $sessionStorage.TipoTramiteId = vm.tipoTramiteId;
                $sessionStorage.TramiteId = vm.TramiteId;
                $sessionStorage.TipoRolId = 1;


                $sessionStorage.numeroTramite = vm.numeroTramite;
                $sessionStorage.nombreEntidad = vm.nombreEntidad;
                $sessionStorage.nombreTipoTramite = vm.nombreTipoTramite;
                vm.activartraslados = true;
                $sessionStorage.accionEjecutandose = 'Ver requisitos tramite';

            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarRequisitosTramite_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };


        function desactivartraslados() {
            vm.activartraslados = false;
        }

        function ValidarEnviarDatosTramite() {
            vm.continuarValidacion = false;
            if (!vm.visibleValidar) {
                vm.visibleValidar = true;
                $scope.edit = true;
                vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                return;
            }
            vm.variableObligatoriosNoIngresados = localStorage.getItem('contObligatoriosNoIngresados');
            if (vm.variableObligatoriosNoIngresados === '0') {
                vm.continuarValidacion = true;
            }
            else
                utilidades.mensajeError("No se han ingresado todos los documentos soporte obligatorios para el trámite!");

            if (vm.continuarValidacion == true) {
                var prm = {
                    TramiteId: vm.TramiteId
                };

                trasladosServicio.ValidarEnviarDatosTramite(prm)
                    .then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {
                            if (response.data.Exito) {
                                vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                                vm.visibleValidar = false;
                            } else {
                                vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                                swal('', response.data.Mensaje, 'warning');
                                vm.visibleValidar = true;
                            }
                        } else {
                            vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                            vm.visibleValidar = true;
                            swal('', "Error al realizar la validación.", 'error');
                        }
                    });

            }
        }


        function consultarArchivosTramite() {
            vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
            vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
            vm.idAccionParam = $sessionStorage.idAccion;
            if (vm.disabledJefePlaneacion) {
                vm.idAccionParam = $sessionStorage.idAccionAnterior;
            }

            vm.infoArchivo = {
                coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
                idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: $sessionStorage.TipoTramiteId, noJefePlaneacion: vm.noJefePlaneacion, disabledJefePlaneacion: vm.disabledJefePlaneacion
            };

            let param = {
                idInstancia: vm.infoArchivo.idInstancia,
                section: vm.infoArchivo.section,
                idAccion: vm.infoArchivo.idAccion
            };

            archivoServicios.obtenerListadoArchivos(param, vm.infoArchivo.coleccion).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.gridOptions.columnDefs = [];
                    vm.archivosLoad = [];
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            vm.archivosLoad.push({
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcionTramite: archivo.metadatos.descripciontramite,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumentoSoporte: archivo.metadatos.tipodocumentosoporte,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id
                            });
                        }
                    });



                    vm.contObligatoriosNoIngresados = 0;
                    $sessionStorage.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados;
                    obtenerTiposDocumentos()
                        .then(function (response) {
                            if (response.data !== null && response.data.length > 0) {
                                response.data.forEach(tipoArchivo => {
                                    //vm.listTipoArchivo.push({
                                    //    Id: tipoArchivo.Id,
                                    //    Name: tipoArchivo.TipoDocumento
                                    //});                                    
                                    if (tipoArchivo.Obligatorio === true) {
                                        vm.tieneArchivosAdjuntos = false;
                                        vm.archivosLoad.forEach(archivo => {
                                            if (archivo.tipoDocumentoSoporte === tipoArchivo.TipoDocumento) {
                                                vm.tieneArchivosAdjuntos = true;
                                            }
                                        });

                                        if (vm.tieneArchivosAdjuntos === false) {
                                            vm.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados + 1;
                                        }
                                    }
                                });
                            }
                            $sessionStorage.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados;
                            localStorage.setItem('contObligatoriosNoIngresados', $sessionStorage.contObligatoriosNoIngresados);
                        });
                }
            }, error => {
                console.log(error);
            });
        }

        function obtenerTiposDocumentos() {
            return archivoServicios.obtenerTipoDocumentoTramitePorRol($sessionStorage.TipoTramiteId, '', vm.TramiteId, vm.idnivel);
        }

    }

    angular.module('backbone').component('trasladoTramite', {
        templateUrl: "src/app/formulario/ventanas/tramites/traslados.html",
        controller: trasladosTramiteController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });

})();