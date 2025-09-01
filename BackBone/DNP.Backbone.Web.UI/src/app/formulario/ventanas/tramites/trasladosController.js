
(function () {
    'use strict';
       
    trasladosController.$inject = [
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
        '$location'
    ];



    function trasladosController(
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
        $location
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        //vm.idInstancia = $sessionStorage.idInstanciaIframe;

      



        vm.listaProyectoContraCredito = [];


        vm.idTipoTramite = "";
        //vm.cargarEntidades = cargarEntidades;
        //vm.buscar = buscar;
        vm.idEntidad = "1C58FFF0-E999-44C9-B4BE-0176A3CF73A5";
        vm.numeroTramite = 0;
        vm.TramiteId = 0;
        vm.nombreEntidad = "";
        vm.nombreTipoTramite = "";
        vm.etapa = $routeParams['etapa'];  // 'ej'
        vm.listaFiltroProyectosC = [];
        vm.listaFiltroProyectosD = [];
        vm.ProyectosSeleccionadosC = [];
        vm.ProyectosSeleccionadosD = [];
        vm.gridOptions;
        vm.activartraslados = false;

        vm.ObtenerProyectosContracredito = ObtenerProyectosContracredito;
        vm.ObtenerProyectosCredito = ObtenerProyectosCredito;
        vm.ObtenerProyectos = ObtenerProyectos;
        vm.agregarProyectos = agregarProyectos;
        vm.actualizaCombos = actualizaCombos;
        vm.limpiarCombos = limpiarCombos;
        vm.ObtenerProyectosTramite = vm.ObtenerProyectosTramite;
        vm.btnEliminarProyectoTramite = vm.btnEliminarProyectoTramite;
        vm.generarInstancia = generarInstancia;
        vm.ActualizarInstanciaProyecto = ActualizarInstanciaProyecto;
        vm.ActualizarValoresProyecto = ActualizarValoresProyecto;
        vm.eventoValidar = eventoValidar;
        vm.desactivartraslados = desactivartraslados;

        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyecto.html';

        vm.montosProyectoTemplate = '<div class="text-right"> ' +
            '<label">{{row.entity.ValorMontoProyecto}} </label> ' +
            '</div > ';


        vm.montosTramiteTemplate = '<div class="text-center"> ' +
            '<input type="number" style="text-align:right" class="form-control"  value="{{row.entity.ValorMontoEnTramite}}" id="textmontotramite_{{row.entity.ProyectoId}}"></div > ';


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

        //vm.peticionObtenerInbox = {
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    IdUsuario: usuarioDNP, 
        //    IdObjeto: idTipoTramite,
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    Aplicacion: nombreAplicacionBackbone,
        //    IdInstancia: $sessionStorage.idInstanciaIframe,  
        //    ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
        //    IdsEtapas: getIdEtapa()
        //};
        //vm.parametros = {
        //    idFlujo: $sessionStorage.idFlujoIframe,
        //    tipoEntidad: 'Nacional',
        //    idInstancia: $sessionStorage.idInstanciaIframe,  
        //    IdEntidad: vm.IdEntidadSeleccionada
        //};

        vm.peticionObtenerInbox = {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            IdUsuario: 'wmunoz@dnp.gov.co',
            IdObjeto: idTipoTramite,
            // ReSharper disable once UndeclaredGlobalVariableUsing
            Aplicacion: nombreAplicacionBackbone,
            IdInstancia: '6076B41C-ABB5-42AC-B441-B4585953EAE2', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83',
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdsEtapas: getIdEtapa()
        };

        vm.parametros = {
            idFlujo: '6199A015-D30E-CDC0-DF54-D53ED43B42C2', //'3D1BC935-7910-4DD4-890D-2EAB7AF8C995',
            tipoEntidad: 'Nacional',
            idInstancia: '6076B41C-ABB5-42AC-B441-B4585953EAE2', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83', 
            IdEntidad: vm.IdEntidadSeleccionada
        };

               
        vm.columnDefPrincial = [{
            field: 'Name',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate
        }];

        vm.columnDef = [
            {
                field: 'BPIN',
                displayName: 'BPIN',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'NombreProyecto',
                displayName: 'Proyecto',
                enableHiding: false,
                enableColumnMenu: false,
                width: '31%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'TipoProyecto',
                displayName: 'Tipo Operación',
                enableHiding: false,
                enableColumnMenu: false,
                width: '11%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'ValorMontoProyecto',
                displayName: 'Monto del proyecto $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '14%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                //cellTemplate: vm.montosProyectoTemplate,
                type: "number",
                cellFilter: 'currency:""'
                
            },
            {
                field: 'ValorMontoEnTramite',
                displayName: 'Monto del trámite $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '13%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field],
                //cellTemplate: vm.selectorRenglonTemplate,
                cellTemplate: vm.montosTramiteTemplate
            },
            {                
                field: 'Estado',
                displayName: 'Estado',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
                cellTooltip: (row, col) => row.entity[col.field]
                
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
                width: '10%'
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

            }
        };
        
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
                        ObtenerProyectosContracredito();
                        ObtenerProyectosCredito();
                        ObtenerProyectosTramite();
                    }

                    vm.filasFiltradas = vm.gridOptions.data.length > 0;
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

            if (vm.listaProyectosC && vm.listaProyectosC.length > 0)
            { 
                //if (vm.listaProyectosC.length > 0) {
                    vm.listaProyectosC.forEach(proyecto => {
                        //if (proyecto.IdEntidad === vm.IdEntidadSeleccionada) {
                        listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
                        //}                    
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

            //if (vm.listaProyectosD.length > 0) {
            //    vm.listaProyectosD.forEach(proyecto => {
            //        //if (proyecto.IdEntidad === vm.IdEntidadSeleccionada) {
            //            listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
            //        //}
            //    });

            //    $.each(listaProyectosGrid, function (i, el) {
            //        if ($.inArray(el, vm.listaFiltroProyectosC) === -1) {
            //            vm.listaFiltroProyectosD.push(el);
            //        }
            //    });
            //} 

            if (vm.listaProyectosD && vm.listaProyectosD.length > 0) {
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
                //}  
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

            if (vm.ProyectosSeleccionadosD && vm.ProyectosSeleccionadosD.length > 0)
            {
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

        function ObtenerProyectosTramite() {

            vm.listaEntidadesProy = []; 
            vm.listaGrupoEntidades = [];
            vm.listaGrupoProyectos = [];
            vm.datoproyectosTramite = [];
            vm.listaproyectosEntidad = [];
            vm.listaEntidadesGrilla = [];
            vm.listaGrillaProyectos = [];

            trasladosServicio.obtenerProyectosTramite(vm.TramiteId)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaEntidadesProy = response.data;
                       
                        vm.listaEntidadesProy.forEach(entidad => {
                            if (!vm.listaGrupoEntidades.find(ent => ent.EntidadId === entidad.EntidadId)) {
                                const { Sector, NombreEntidad, EntidadId } = entidad;
                                vm.listaGrupoEntidades.push({ Sector, NombreEntidad, EntidadId});
                            }
                        });

                        vm.listaGrupoEntidades.forEach(entidad => {

                            vm.listaGrupoProyectos = [];
                            vm.listaEntidadesProy.forEach(proyectoentidad => {
                                if (proyectoentidad.EntidadId === entidad.EntidadId) {
                                    if (!vm.listaGrupoProyectos.find(p => p.EntidadId === proyectoentidad.EntidadId)) {
                                        const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyecto, ValorMontoEnTramite} = proyectoentidad;
                                        vm.listaGrupoProyectos.push({ BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyecto, ValorMontoEnTramite });
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

                                    data: vm.listaGrupoProyectos
                                    // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                }
                            });

                        });

                        vm.listaGrupoProyectos = [];
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyecto, ValorMontoEnTramite } = proyectoentidad;
                            vm.listaGrupoProyectos.push({ BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyecto, ValorMontoEnTramite });
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
                                ValorMontoProyecto: entidad.ValorMontoProyecto,
                                ValorMontoEnTramite: entidad.ValorMontoEnTramite,
                     
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
 
                    }
                });
        }               

        vm.btnIniciarInstanciaProyecto_onClick = function ($event, sender) {
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
                var montoTramite = document.getElementById('textmontotramite_' + entity.ProyectoId);

                if (montoTramite.value === 0 || montoTramite.value === "0" || montoTramite.value === "" ) {
                    utilidades.mensajeError("No ha ingresado el monto del trámite!");
                } else {
                    if (montoTramite.value > entity.ValorMontoProyecto && entity.TipoProyecto === "Contracredito") {
                        utilidades.mensajeError("El monto del trámite no puede ser mayor al monto del proyecto!");
                    } else {
                        ActualizarValoresProyecto(entity, 0);
                        generarInstancia(entity);
                    }
                }
            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarInstanciaProyecto_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };


        vm.btnEliminarProyectoTramite_onClick = function ($event, sender) {
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

               var prm = {
                    TramiteId: vm.TramiteId,
                    ProyectoId: entity.ProyectoId
                };

                utilidades.mensajeWarning("Confirma la eliminación del proyecto en el trámite?", function funcionContinuar() {
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
            var montoTramite = document.getElementById('textmontotramite_' + proyecto.ProyectoId);
            vm.listaIstanciasCreadas = [];
            //vm.listaIstanciasCreadas = resultado;

            const proyectoTramiteDto = {
                TramiteId: vm.TramiteId,
                ProyectoId: proyecto.ProyectoId,
                EntidadId: proyecto.EntidadId,
                TipoRolId: 1,
                ValorMontoEnTramite: montoTramite.value
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
                        var cantidadInstanciasFallidas = instanciasFallidas.lenght;

                        if (cantidadInstanciasFallidas) {
                            utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                        } else {
                            ActualizarInstanciaProyecto(proyecto,  resultado);
                            utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                        }

                    });
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });
        }

        function ActualizarInstanciaProyecto(proyecto, resultado) {

            var montoTramite = document.getElementById('textmontotramite_' + proyecto.ProyectoId);
            vm.listaIstanciasCreadas = [];
            vm.listaIstanciasCreadas = resultado;

            vm.listaIstanciasCreadas.forEach(instancia => {
                const proyectoTramiteDto = {
                    TramiteId: vm.TramiteId,
                    InstanciaId: instancia.InstanciaId,
                    ProyectoId: proyecto.ProyectoId,
                    EntidadId: proyecto.EntidadId,
                    TipoRolId: 1,
                    ValorMontoEnTramite: montoTramite.value
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
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

                var montoTramite = document.getElementById('textmontotramite_' + entity.ProyectoId);
                var montoProyecto = document.getElementById('textmontoproyecto_' + entity.ProyectoId);


                if (montoTramite.value > entity.ValorMontoProyecto && entity.TipoProyecto === "Contracredito") {
                    utilidades.mensajeError("El monto del trámite no puede ser mayor al monto del proyecto!");
                } else {
                    ActualizarValoresProyecto(entity, 0);

                    vm.TramiteId = entity.ProyectoId;

                    $sessionStorage.ProyectoId = entity.ProyectoId;
                    $sessionStorage.TipoProyecto = entity.TipoProyecto;
                    $sessionStorage.MontoTramiteProyecto = entity.ValorMontoProyecto;
                    $sessionStorage.EntidadId = entity.EntidadId;
                    $sessionStorage.TipoTramiteId = 4; //vm.TipoTramiteId;
                    $sessionStorage.TramiteId = vm.TramiteId;
                    $sessionStorage.TipoRolId = 1;
                    vm.activartraslados = true;
                //    $timeout(function () {
                //        $location.path('/trasladosAcciones');
                //    }, 300);
                }
            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarRequisitos_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };


        function eventoValidar() {
            if (vm.IdEntidadSeleccionada == "" || vm.IdEntidadSeleccionada == null || vm.IdEntidadSeleccionada == undefined) {
                vm.callback({ arg: true });
                utilidades.mensajeError("Seleccione una entidad.");
            }
            else {
                vm.callback({ arg: false });
                utilidades.mensajeSuccess("Formulario validado correctamente.");
            }
        }

        }      

        function desactivartraslados () {
            vm.activartraslados = false;
        }

    angular.module('backbone').controller('trasladosController', trasladosController);
    //angular.module('backbone').component('traslados', {
    //    templateUrl: "src/app/formulario/ventanas/tramites/traslados.html",
    //    controller: trasladosController,
    //    controllerAs: "vm",
    //    bindings: {
    //        desactivartraslados: '&'
    //    }
    //});

})();