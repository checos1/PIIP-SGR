
(function () {
    'use strict';

    trasladoFirmaController.$inject = [
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
        'solicitarconceptoServicio',
        'cartaServicio',
        '$q',
        'servicioFichasProyectos', 'archivoServicios', 'FileSaver'
    ];



    function trasladoFirmaController(
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
        solicitarconceptoServicio,
        cartaServicio,
        $q,
        servicioFichasProyectos, archivoServicios, FileSaver
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.activargrilla = true;
        vm.visibleValidar = true;
        vm.desactivartraslados = desactivartraslados;
        vm.tramiteId = $sessionStorage.TramiteId;
        vm.idAplicacion = $sessionStorage.IdAplicacion;
        
        vm.analista = false;
        vm.ctrlposterior = false;

        /*botones acciones*/
        vm.mostrareleborarcp = true;
        $sessionStorage.director = true;
        $scope.firmado = false;
        $scope.mostrarFirma = false;
        vm.esArchivoOrfeo = false;
        vm.archivoPDF = undefined;
         $scope.firmaCargada = false;

        vm.idTramite = $sessionStorage.TramiteId;

        /*fin botones acciones*/

        setTimeout(function () {
            vm.callback({ arg: true, aprueba: false, titulo: 'ENVÍO PARA APROBACIÓN' });
        }, 3000);

        vm.peticion = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdFiltro: vm.EntityTypeCatalogOptionId,
        };
        vm.concepto = {
            Id: 0,
            EntityTypeCatalogOptionId: 0,
            IdUsuarioDNP: "",
            ParentId: 0
        };

        vm.listaProyectoContraCredito = [];


       


        vm.idTipoTramite = "";
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

        vm.ObtenerProyectosTramite = vm.ObtenerProyectosTramite;
        vm.NombreVerConcepto = "Ver Concepto";
        vm.verRequisitosTramite = verRequisitosTramite;
        $sessionStorage.accionEjecutandose = 'Ver requisitos proyecto';

        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/consola/tramites/plantillas/plantillaAccionesTramiteProyectoAprobacion.html';

        vm.montosProyectoTemplate = '<div class="text-right"> ' +
            '<label">{{row.entity.ValorMontoEntidad}} </label> ' +
            '</div > ';


        vm.montosTramiteTemplate = '<div class="text-center"> ' +
            '<input type="number" disabled="true" style="text-align:right" class="form-control"  value="{{row.entity.ValorMontoEnAprobacion}}" id="textmontoaprobacion_{{row.entity.ProyectoId}}"></div > ';


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
            vm.existeconcepto = false;
        vm.verbotonsolicitarconcepto = false;

        //TODO
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

        vm.montoTramiteTemplate = '<div class="row text-right" style="margin-right: 15px;"> <label>Nación </label> </div >' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorMontoTramiteNacion}}</label> </div > ' +
            '<div style="height:12px;"></div><div class="row text-right" style="margin-right: 15px;"> <label>Propios </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;">{{row.entity.ValorMontoTramitePropios}}</label> </div > ';


        vm.montoAprobacionTemplate = '<div class="row text-right" style="margin-right: 15px;"> <label>Nación </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;text-align:right;width:90%;"  >{{row.entity.ValorMontoAprobadosNacion}}</label> </div > ' +
            '<div style="height:12px;"></div><div class="row text-right" style="margin-right: 15px;"> <label>Propios </label> </div > ' +
            '<div class="row text-right    " style="margin-right: 15px;"> <label style="font-weight: 700;text-align:right;width:90%;"  >{{row.entity.ValorMontoAprobadosPropios}}</label> </div > ';

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
                displayName: 'Monto del Proyecto $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellTemplate: vm.montoProyectoTemplate
            },
            {
                field: 'mp',
                displayName: 'Monto solicitado del tramite $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellTemplate: vm.montoTramiteTemplate
            },
            {
                field: 'mt',
                displayName: 'Monto aprobado del tramite $',
                enableHiding: false,
                enableColumnMenu: false,
                width: '12%',
                pinnedRight: true,
                cellTemplate: vm.montoAprobacionTemplate
            },
            {
                field: 'EstadoActualizacion',
                displayName: 'Estado actualización',
                enableHiding: false,
                enableColumnMenu: false,
                width: '10%',
                pinnedRight: true,
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

        /*region metodos vm */
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
            cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, 1)
                .then(resultado => {
                    if (resultado != undefined && resultado.data.length > 0) {
                        var tmp = resultado.data[0];
                        $scope.firmado = tmp.Firmada;
                        //var x = trasladosServicio.validarSiExisteFirmaUsuario().then(function (response) {
                        //    if (response.data.Exito && response.statusText === "OK") {
                        //        $scope.firmaCargada = true;
                        //    } else {
                        //        $scope.firmaCargada = false;
                        //    }
                        //});
                       if ($scope.firmado)
                            vm.callback({ arg: false, aprueba: true, titulo: '', ocultarDevolver: true });
                        else
                            vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: false });
                    }
                    else
                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                });
        };

        vm.btnIniciarRequisitos_onClick = function ($event, sender) {
            try {
                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

                var montoAprobacion = document.getElementById('textmontoaprobacion_' + entity.ProyectoId);

                //ActualizarValoresProyecto(entity, 0);
                $sessionStorage.ProyectoId = entity.ProyectoId;
                $sessionStorage.TipoProyecto = entity.TipoProyecto;
                $sessionStorage.ValorMontoTramiteNacion = entity.ValorMontoTramiteNacion;
                $sessionStorage.ValorMontoTramitePropios = entity.ValorMontoTramitePropios;
                $sessionStorage.ValorMontoAprobadosNacion = entity.ValorMontoAprobadosNacion;
                $sessionStorage.ValorMontoAprobadosPropios = entity.ValorMontoAprobadosPropios;
                $sessionStorage.EntidadId = entity.EntidadId;
                $sessionStorage.TipoTramiteId = 4; //vm.TipoTramiteId;
                $sessionStorage.TramiteId = vm.TramiteId;
                $sessionStorage.TipoRolId = 2;
                $sessionStorage.numeroTramite = vm.numeroTramite;
                $sessionStorage.NombreProyecto = entity.NombreProyecto;
                $sessionStorage.BPIN = entity.BPIN;
                $sessionStorage.nombreEntidad = vm.nombreEntidad;
                $sessionStorage.nombreTipoTramite = vm.nombreTipoTramite;
                $sessionStorage.accionEjecutandose = 'Ver requisitos proyecto';
                vm.activartraslados = true;
                //$timeout(function () {
                //    $location.path('/requisitosAprobacion');
                //}, 300);

            }
            catch (exception) {
                console.log('controladorProyecto.btnIniciarRequisitos_onClick => ', exception);
                toastr.error('Ocurrió un error.');
            }
        };

        vm.VerConcepto = function () {
            var ficha = {
                Nombre: constantesBackbone.apiBackBoneNombrePDFCartaFirma,
            };

            vm.Ficha = ficha;

            var fichaPlantilla = {
                NombreReporte: ficha.Nombre,
                PARAM_BORRADOR: $scope.firmado,
                PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                TramiteId: vm.idTramite
            };

            crearDocumento(fichaPlantilla).then(function (fichaTemporal) {
                FileSaver.saveAs(fichaTemporal, fichaTemporal.name);
            }, function (error) {
                utilidades.mensajeError(error);
            });



        }

        /* fin metodos vm */


        //#region funciones

     

      
      
        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
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
                            vm.IdEntidadSeleccionada = entidad.EntidadId;
                            $sessionStorage.idEntidad = entidad.EntidadId;

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
                                    $sessionStorage.TramiteId = vm.TramiteId;
                                    $sessionStorage.idInstancia = vm.parametros.idInstancia;
                                    $sessionStorage.IdAccion = vm.listaDatos[0].IdAccion;
                                    ObtenerSolicitarConcepto();
                                    
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
                        //ObtenerProyectos();
                        //ObtenerProyectosContracredito();
                        //ObtenerProyectosCredito();
                        ObtenerProyectosTramite();
                    }

                    vm.filasFiltradas = vm.gridOptions.data.length > 0;
                },

            );
        }

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

        function ObtenerSolicitarConcepto() {
            vm.peticion.IdFiltro = vm.TramiteId;
            solicitarconceptoServicio.ObtenerSolicitarConcepto(vm.peticion)
                .then(resultado => {
                    vm.concepto = resultado.data;
                    $sessionStorage.concepto = vm.concepto;
                    vm.concepto.forEach(con => {
                        //if (vm.concepto.Id > 0 && vm.concepto.Enviado == 0) {
                        if (con.Id > 0 && con.Enviado == 0) {
                            vm.NombreSolicitarConcepto = "RECUPERAR CONCEPTO";
                            vm.activargrilla = true;
                            vm.versolicitarconcepto = true;
                            vm.callback({ arg: true, aprueba: true, titulo: '' });
                        }
                        else {
                            vm.NombreSolicitarConcepto = "SOLICITAR CONCEPTO";
                            vm.activargrilla = true;
                            vm.versolicitarconcepto = false;
                            vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                        }
                    });
                    
                })
            
        }


        function ObtenerProyectosTramite() {

            vm.listaEntidadesProy = [];
            vm.listaGrupoEntidades = [];
            vm.listaGrupoProyectos = [];
            vm.datoproyectosTramite = [];
            vm.listaproyectosEntidad = [];
            vm.listaEntidadesGrilla = [];
            vm.listaGrillaProyectos = [];

            trasladosServicio.obtenerProyectosTramiteAprobacion(vm.TramiteId, 2)
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

                            vm.ValorTotalMontoRC = 0;
                            vm.ValorTotalMontoPropiosRC = 0;
                            vm.ValorTotalMontoNacionRC = 0;
                            vm.ValorTotalMontoRCC = 0;
                            vm.ValorTotalMontoNacionRCC = 0;
                            vm.ValorTotalMontoPropiosRCC = 0;

                            vm.listaEntidadesProy.forEach(proyectoentidad => {

                                var MontoAprobadosNacion = parseInt(proyectoentidad.ValorMontoAprobadosNacion === undefined ? 0 : proyectoentidad.ValorMontoAprobadosNacion);
                                var MontoAprobadosPropios = parseInt(proyectoentidad.ValorMontoAprobadosPropios === undefined ? 0 : proyectoentidad.ValorMontoAprobadosPropios);

                                if (proyectoentidad.TipoProyecto === 'Credito') {
                                    vm.ValorTotalMontoRC += (MontoAprobadosNacion + MontoAprobadosPropios);
                                    vm.ValorTotalMontoNacionRC += (MontoAprobadosNacion);
                                    vm.ValorTotalMontoPropiosRC += (MontoAprobadosPropios);
                                }
                                else {
                                    vm.ValorTotalMontoRCC += (MontoAprobadosNacion + MontoAprobadosPropios);
                                    vm.ValorTotalMontoNacionRCC += (MontoAprobadosNacion);
                                    vm.ValorTotalMontoPropiosRCC += (MontoAprobadosPropios);
                                }
                                proyectoentidad.ValorMontoProyectoNacion = formatearNumero(proyectoentidad.ValorMontoProyectoNacion === undefined ? 0 : proyectoentidad.ValorMontoProyectoNacion);
                                proyectoentidad.ValorMontoProyectoPropios = formatearNumero(proyectoentidad.ValorMontoProyectoPropios === undefined ? 0 : proyectoentidad.ValorMontoProyectoPropios);
                                proyectoentidad.ValorMontoTramiteNacion = formatearNumero(proyectoentidad.ValorMontoTramiteNacion === undefined ? 0 : proyectoentidad.ValorMontoTramiteNacion);
                                proyectoentidad.ValorMontoTramitePropios = formatearNumero(proyectoentidad.ValorMontoTramitePropios === undefined ? 0 : proyectoentidad.ValorMontoTramitePropios);
                                proyectoentidad.ValorMontoAprobadosNacion = formatearNumero(proyectoentidad.ValorMontoAprobadosNacion === undefined ? 0 : proyectoentidad.ValorMontoAprobadosNacion);
                                proyectoentidad.ValorMontoAprobadosPropios = formatearNumero(proyectoentidad.ValorMontoAprobadosPropios === undefined ? 0 : proyectoentidad.ValorMontoAprobadosPropios);


                                if (proyectoentidad.EntidadId === entidad.EntidadId) {
                                    if (!vm.listaGrupoProyectos.find(p => p.EntidadId === proyectoentidad.EntidadId)) {
                                        const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios,
                                            ValorMontoTramiteNacion, ValorMontoTramitePropios, ValorMontoAprobadosNacion,
                                            ValorMontoAprobadosPropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                                        vm.listaGrupoProyectos.push({
                                            BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoPropios,
                                            ValorMontoTramiteNacion, ValorMontoTramitePropios, ValorMontoAprobadosNacion,
                                            ValorMontoAprobadosPropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
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
                        vm.ValorTotalMontoNacionRC = formatearNumero(vm.ValorTotalMontoNacionRC === undefined ? 0 : vm.ValorTotalMontoNacionRC);
                        vm.ValorTotalMontoPropiosRC = formatearNumero(vm.ValorTotalMontoPropiosRC === undefined ? 0 : vm.ValorTotalMontoPropiosRC);
                        vm.ValorTotalMontoRCC = formatearNumero(vm.ValorTotalMontoRCC === undefined ? 0 : vm.ValorTotalMontoRCC);
                        vm.ValorTotalMontoNacionRCC = formatearNumero(vm.ValorTotalMontoNacionRCC === undefined ? 0 : vm.ValorTotalMontoNacionRCC);
                        vm.ValorTotalMontoPropiosRCC = formatearNumero(vm.ValorTotalMontoPropiosRCC === undefined ? 0 : vm.ValorTotalMontoPropiosRCC);

                        vm.listaGrupoProyectos = [];
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoTramitNacion, ValorMontoProyectoNacion, ValorMontoProyectoPropios,
                                ValorMontoTramitPropios, ValorMontoAprobadosNacion,
                                ValorMontoAprobadosPropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                            vm.listaGrupoProyectos.push({
                                BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoTramitNacion, ValorMontoProyectoNacion, ValorMontoProyectoPropios,
                                ValorMontoTramitPropios, ValorMontoAprobadosNacion,
                                ValorMontoAprobadosPropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
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
                                EstadoActualizacion: entidad.EstadoActualizacion,
                                Programa: entidad.Programa,
                                SubPrograma: entidad.SubPrograma,
                                CodigoPresupuestal: entidad.CodigoPresupuestal,
                                ValorMontoProyectoNacion: entidad.ValorMontoProyectoNacion ? formatearNumero(entidad.ValorMontoProyectoNacion) : 0,
                                ValorMontoProyectoPropios: entidad.ValorMontoProyectoPropios ? formatearNumero(entidad.ValorMontoProyectoPropios) : 0,
                                ValorMontoTramiteNacion: entidad.ValorMontoTramiteNacion ? formatearNumero(entidad.ValorMontoTramiteNacion) : 0,
                                ValorMontoTramitePropios: entidad.ValorMontoTramitePropios ? formatearNumero(entidad.ValorMontoTramitePropios) : 0,
                                ValorMontoAprobadosNacion: entidad.ValorMontoAprobadosNacion ? formatearNumero(entidad.ValorMontoAprobadosNacion) : 0,
                                ValorMontoAprobadosPropios: entidad.ValorMontoAprobadosPropios ? formatearNumero(entidad.ValorMontoAprobadosPropios) : 0,

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

                        vm.listaGrillaProyectos.map(function (item) {
                            trasladosServicio.obtenerCodigoPresupuestal(vm.TramiteId, item.ProyectoId, item.EntidadId)
                                .then(function (resultado) {
                                    if (resultado.data != null)
                                        item.CodigoPresupuestal = resultado.data.CodigoPresupuestal;
                                });

                        });

                        vm.gridOptions.showHeader = true;
                        vm.gridOptions.columnDefs = vm.columnDef;
                        vm.gridOptions.data = vm.listaGrillaProyectos;
                        vm.filasFiltradas = vm.gridOptions.data.length > 0;

                    }
                });
        }

        function desactivartraslados() {
            vm.activartraslados = false;
            ObtenerProyectosTramite();
        }

       

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function verRequisitosTramite() {
            $sessionStorage.accionEjecutandose = 'Ver requisitos tramite';
            $sessionStorage.ProyectoId = vm.ProyectoId;
            $sessionStorage.TipoProyecto = vm.TipoProyecto;
            $sessionStorage.TipoTramiteId = 4; //vm.TipoTramiteId;
            $sessionStorage.TramiteId = vm.TramiteId;
            $sessionStorage.TipoRolId = 2;
            $sessionStorage.numeroTramite = vm.numeroTramite;
            $sessionStorage.nombreEntidad = vm.nombreEntidad;
            $sessionStorage.nombreTipoTramite = vm.nombreTipoTramite;
            $sessionStorage.accionEjecutandose = 'Ver requisitos tramite';
            vm.activartraslados = true;
            vm.activarcarta = false;
            $sessionStorage.allArchivosTramite = true;
            //$timeout(function () {
            //    $location.path('/requisitosAprobacion');
            //}, 300);
        }

        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = vm.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha(vm.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        /*var blob = new Blob([respuesta], { type: 'application/pdf' });*/
                        const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                        var fileOfBlob = new File([blob], nombreArchivo, { type: 'application/pdf' });
                        var archivo = {};

                        var metadatos = {
                            NombreAccion: $sessionStorage.nombreAccion,
                            IdAplicacion: 1,//$sessionStorage.IdAplicacion,
                            IdNivel: $sessionStorage.idNivel,
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAccion: $sessionStorage.idAccion,
                            IdInstanciaFlujoPrincipal: $sessionStorage.idInstanciaFlujoPrincipal,
                            IdObjetoNegocio: $sessionStorage.idObjetoNegocio,
                            Size: blob.size,
                            ContenType: 'application/pdf',
                            Extension: extension,
                            FechaCreacion: new Date(),
                            Tipo: 'Ficha',
                            NombreFicha: respuestaFicha.Nombre,
                            TipoFicha: respuestaFicha.Descripcion
                        }

                        archivo = {
                            FormFile: fileOfBlob,
                            Nombre: nombreArchivo,
                            Metadatos: metadatos
                        };

                        if (vm.esArchivoOrfeo) {
                            vm.esArchivoOrfeo = false;
                            vm.archivoPDF = archivo;
                            resolve('Success!');
                        }
                        else if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            //archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                            //    if (response === undefined || typeof response === 'string') {
                            //        reject(response);
                            //    } else {
                                    resolve(fileOfBlob);
                               // }
                            //}, function (error) {
                            //    reject(error);
                            //});
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }


        //#endregion

    }

    angular.module('backbone').component('trasladoFirma', {
        templateUrl: "src/app/formulario/ventanas/tramites/trasladoFirma.html",
        controller: trasladoFirmaController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });


})();