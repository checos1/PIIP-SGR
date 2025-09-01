
(function () {
    'use strict';

    trasladosAprobacionTramiteController.$inject = [
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
        '$uibModal',
        'servicioFichasProyectos',
        'FileSaver',
        '$q'
    ];



    function trasladosAprobacionTramiteController(
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
        $uibModal,
        servicioFichasProyectos,
        FileSaver,
        $q

    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        //vm.versolicitarconcepto = false;
        vm.activargrilla = true;
        vm.visibleValidar = true;
        vm.GenerarConcepto = generarConcepto;
        //vm.idInstancia = $sessionStorage.idInstanciaIframe;
        vm.anteriorMostrarSolicitarConcpeto = undefined;
        vm.mostrareleborarcp = true;
        $sessionStorage.director = false;
        vm.activarbotones = true;

        vm.analista = false;
        vm.ctrlposterior = false;
        vm.DevolverA = false;
        vm.MensajeEstado = "";
        vm.EstadoOK = true;
        vm.subdirector = false;
  

        setTimeout(function () {
            if (vm.analista == false) {
                vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
            }
            else {
                vm.callback({ arg: true, aprueba: false, titulo: 'ENVÍO PARA APROBACIÓN' });
            }
            if (vm.subdirector == true) {
                vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
            }
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

        vm.SolicitarConcepto = SolicitarConcepto;
        function SolicitarConcepto() {
            vm.activarcarta = false;
            //vm.solicitaconcepto = false;
            $sessionStorage.TramiteId = vm.TramiteId;
            if (vm.solicitaconcepto) {
                vm.solicitaconcepto = false;
            }
            else {
                vm.solicitaconcepto = true;
            }

            AdministrarVistaFormulario();
        }

        vm.DevolverAnalista = DevolverAnalista;
        function DevolverAnalista() {
            vm.DevolverA = true;
            vm.peticion.IdFiltro = vm.TramiteId;
            solicitarconceptoServicio.ObtenerSolicitarConcepto(vm.peticion)
                .then(resultado => {
                    vm.concepto = resultado.data;
                    $sessionStorage.concepto = vm.concepto;
                    vm.concepto.forEach(con => {
                        if (con.IdUsuarioDNP == vm.peticion.IdUsuario && con.Enviado == false) {
                            vm.DevolverA = false;
                        }
                    });
                    if (vm.concepto != null && vm.concepto.length > 0) {
                        if (vm.DevolverA) {
                            solicitarconceptoServicio.eliminarPermisos($sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.TramiteId, 'TEC');
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        }
                        else {
                            utilidades.mensajeError('Debe dar respuesta al concepto técnico');
                        }
                    }
                });

        }

        function validaHabilita(arg) {

            vm.solicitaconcepto = arg;
            if (!arg) {
                vm.NombreSolicitarConcepto = "RECUPERAR CONCEPTO";
                vm.activargrilla = true;
            }
            else {
                vm.NombreSolicitarConcepto = "SOLICITAR CONCEPTO";
                //vm.activargrilla = true;
                SolicitarConcepto();
            }
            ObtenerSolicitarConcepto();
            vm.solicitaconcepto = false;
        }

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
        vm.eventoValidarAnalista = eventoValidarAnalista;
        vm.validaHabilita = validaHabilita;
        vm.NombreSolicitarConcepto = "Solicitar Concepto";
        vm.ValidarEnviarDatosTramite = ValidarEnviarDatosTramite;
        vm.verRequisitosTramite = verRequisitosTramite;
        vm.desactivartraslados = desactivartraslados;
        vm.desactivarcarta = desactivarcarta;
        $sessionStorage.accionEjecutandose = 'Ver requisitos proyecto';
        vm.solicitaconcepto = false;
        vm.conceptoenviado = false;
        vm.conceptoiguales = [];

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

        vm.btnActualizaEstadoAjusteProyecto_onClick = ActualizaEstadoAjusteProyecto_onClick;

        //vm.peticionObtenerInbox = {
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    IdUsuario: 'CC505050',//'wmunoz@dnp.gov.co',
        //    IdObjeto: idTipoTramite,
        //    // ReSharper disable once UndeclaredGlobalVariableUsing
        //    Aplicacion: nombreAplicacionBackbone,
        //    IdInstancia: 'DFB33FA3-3100-4B31-BA0E-94706291038F', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83',
        //    ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
        //    IdsEtapas: getIdEtapa()
        //};

        //vm.parametros = {
        //    idFlujo: 'E8FC3694-C566-4944-A487-DAA494EB3581',//'062B1A7E-5CB4-4A0D-A84F-F532F0E20C21', //'3D1BC935-7910-4DD4-890D-2EAB7AF8C995',
        //    tipoEntidad: 'Nacional',
        //    idInstancia: 'DFB33FA3-3100-4B31-BA0E-94706291038F', //'BF51D8D7-2CF5-4AE3-9236-03870A559E83',
        //    IdEntidad: vm.IdEntidadSeleccionada
        //};


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
            //codigo: {
            //    campo: 'Id',
            //    valor: null,
            //    tipo: constantesCondicionFiltro.igual,
            //    width: '20%'
            //},
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
        function ValidarEnviarDatosTramite() {

            vm.continuarValidacion = false;
            if (!vm.visibleValidar) {
                vm.visibleValidar = true;
                $scope.edit = true;
                vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                if (vm.activarbotones == false) {
                    vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                }
                return;
            }

            var prm = {
                TramiteId: vm.TramiteId
            };

            trasladosServicio.ValidarEnviarDatosTramiteAprobacion(prm)
                .then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        if (vm.MensajeEstado !== "" && response.data.Exito) {
                            vm.callback({ arg: true, aprueba: true, titulo: '' });
                            swal('', vm.MensajeEstado, 'warning');
                            vm.visibleValidar = true;
                        }
                        else if (response.data.Exito) {
                            vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                            vm.visibleValidar = false;
                        } 
                        else {
                            vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                            swal('', response.data.Mensaje + vm.MensajeEstado , 'warning');
                            vm.visibleValidar = true;
                        }
                        if (vm.activarbotones == false) {
                            vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        }
                    } else {
                        if (vm.activarbotones == false) {
                            vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        }
                        else {
                            vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                            vm.visibleValidar = true;
                            swal('', "Error al realizar la validación.", 'error');
                        }
                    }

                });

            VerificarEstadoProyecto();

        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        vm.init = function () {
            vm.tipoEntidad = 'Nacional';
            vm.filtro = '';
            obtenerTramite();
            //ObtenerSolicitarConcepto();
            //AdministrarVistaFormulario();

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

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
        };

        function AdministrarVistaFormulario() {
            var i = 0

            vm.peticion.ListaIdsRoles.forEach(rol => {
                if (i == 0) {
                    if (rol.toUpperCase() == constantesBackbone.idRAnalistaDIFP) {
                        vm.analista = true;
                        vm.aprobacionEntidad = false;
                        vm.ctrlposterior = false;
                        if (vm.concepto.length > 0) {

                            vm.ctrlposterior = false;
                            i = 1;
                            if (vm.concepto[vm.concepto.length - 1].Enviado == true && vm.EstadoOK) {
                                vm.conceptoenviado = false;
                            }
                            else {
                                vm.conceptoenviado = false;
                            }
                        }
                    }
                    else if (rol.toUpperCase() == constantesBackbone.idRControlPosteriorDireccionesTecnicas && vm.concepto != null && vm.concepto.length > 0) {
                        if (vm.concepto[vm.concepto.length - 1].IdUsuarioDNP == vm.peticionObtenerInbox.IdUsuario && vm.concepto[vm.concepto.length - 1].Enviado == false) {
                            vm.analista = false;
                            vm.ctrlposterior = true;
                            //vm.versolicitarconcepto = true;
                            vm.activargrilla = true;
                        }
                        else {
                            //vm.versolicitarconcepto = false;
                            vm.activargrilla = true;
                        }
                        i = 1;
                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        vm.aprobacionEntidad = true;
                    } else if (rol.toUpperCase() == constantesBackbone.idRSubdirectorEntidad) {
                        vm.analista = false;
                        vm.subdirector = true;
                        i = 1;
                        vm.callback({ arg: false, aprueba: true, titulo: 'ENVIO PARA APROBACIÓN', ocultarDevolver: true });
                        vm.aprobacionEntidad = true;
                    }
                    else {
                        vm.analista = false;
                        vm.subdirector = false;
                        vm.ctrlposterior = false;
                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        vm.aprobacionEntidad = true;
                    }
                }
            });

            if (vm.analista == false && vm.subdirector == false ) {
                vm.concepto.forEach(conc => {
                    if (conc.IdUsuarioDNP == vm.peticion.IdUsuario) {
                        vm.conceptoiguales.push(conc);
                    }
                });

                if (vm.conceptoiguales.length == 0) {
                    vm.conceptoenviado = true;
                }
                if (vm.conceptoiguales.length >= 1) {
                    vm.concepto.forEach(conc => {
                        if (conc.Id == vm.conceptoiguales[vm.conceptoiguales.length - 1].Id && conc.Enviado == false && vm.EstadoOK) {
                            vm.conceptoenviado = false;
                        }
                    });
                }
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
                                    //AdministrarVistaFormulario();
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

            if (vm.listaProyectosC && vm.listaProyectosC.length > 0) {
                vm.listaProyectosC.forEach(proyecto => {
                    listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
                });

                $.each(listaProyectosGrid, function (i, el) {
                    if ($.inArray(el, vm.listaFiltroProyectosC) === -1) {
                        vm.listaFiltroProyectosC.push(el);
                    }
                });
            }

        }
        function ObtenerProyectosCredito() {
            vm.listaFiltroProyectosD = [];
            let listaProyectosGrid = [];

            if (vm.listaProyectosD && vm.listaProyectosD.length > 0) {
                vm.listaProyectosD.forEach(proyecto => {
                    listaProyectosGrid.push({ Id: proyecto.IdProyecto, Name: proyecto.BPIN + '-' + proyecto.NombreProyecto });
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
                idFlujo: vm.parametros.idFlujo, tipoEntidad: vm.parametros.tipoEntidad, IdEntidad: vm.IdEntidadSeleccionada, idInstancia: vm.parametros.idInstancia
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
            vm.ProyectosSeleccionadosC.forEach(proyecto => {
                var datoproyectoC = vm.listaProyectosC.filter(function (datoproyectoC) {
                    return datoproyectoC.IdProyecto === proyecto;
                });
                datoproyectoC.forEach(p => {
                    let c = {
                        ProyectoId: p.IdProyecto,
                        EntidadId: p.IdEntidad,
                        TipoProyecto: 'Contracredito',
                        NombreProyecto: p.NombreProyecto,
                        CodigoPresupuestal: ''
                    };
                    proyectos.push(c);
                });
                datoproyectoC.map(function (item) {
                    trasladosServicio.obtenerCodigoPresupuestal(vm.TramiteId, item.ProyectoId, item.EntidadId)
                        .then(function (resultado) {
                            if (resultado.data != null)
                                item.CodigoPresupuestal = resultado.data.CodigoPresupuestal;
                        });

                });
            });

            vm.ProyectosSeleccionadosD.forEach(proyecto => {
                var datoproyectoD = vm.listaProyectosD.filter(function (datoproyectoD) {
                    return datoproyectoD.IdProyecto === proyecto;
                });
                datoproyectoD.forEach(p => {
                    let c = {
                        ProyectoId: p.IdProyecto,
                        EntidadId: p.IdEntidad,
                        TipoProyecto: 'Credito',
                        NombreProyecto: p.NombreProyecto,
                        CodigoPresupuestal: ''
                    };
                    proyectos.push(c);
                });
                datoproyectoD.map(function (item) {
                    trasladosServicio.obtenerCodigoPresupuestal(vm.TramiteId, item.ProyectoId, item.EntidadId)
                        .then(function (resultado) {
                            if (resultado.data != null)
                                item.CodigoPresupuestal = resultado.data.CodigoPresupuestal;
                        });

                });
            });


            var prm = {
                TramiteId: vm.TramiteId,
                Proyectos: proyectos
            };

            trasladosServicio.guardarProyectos(prm)
                .then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        limpiarCombos();
                        ObtenerProyectosTramite();
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

                        if (vm.listaEntidadesProy !== undefined) {
                            vm.listaEntidadesProy.forEach(con => {
                                if (con.EstadoActualizacion.substring(0, 30) !== "Control Posterior DNP Aplicado") {

                                    vm.callback({ arg: true, aprueba: true, titulo: '' });
                                    vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                                    vm.conceptoenviado = true;
                                    vm.visibleValidar = true;
                                    vm.activarbotones = false;
                                    vm.EstadoOK = false;
                                }

                            });
                        }

                    }
                });
        }

        vm.btnIniciarInstanciaProyecto_onClick = function ($event, sender) {
            try {

                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
                generarInstancia(entity);

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
                alert(entity.ProyectoId);

                var prm = {
                    TramiteId: vm.TramiteId,
                    ProyectoId: entity.ProyectoId
                };

                trasladosServicio.eliminarProyectoTramite(vm.peticionObtenerInbox, prm)
                    .then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            limpiarCombos();
                            ObtenerProyectosTramite();
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
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

            flujoServicios.generarInstancia(tramiteDto).then(
                function (resultado) {
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
                        ActualizarInstanciaProyecto(proyecto.ProyectoId, resultado);
                        utilidades.mensajeSuccess('Subproceso de gestión de recursos creado exitosamente');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        function ActualizarInstanciaProyecto(ProyectoId, resultado) {

            vm.listaIstanciasCreadas = [];
            vm.listaIstanciasCreadas = resultado;

            vm.listaIstanciasCreadas.forEach(instancia => {
                const proyectoTramiteDto = {
                    TramiteId: vm.TramiteId,
                    InstanciaId: instancia.InstanciaId,
                    ProyectoId: ProyectoId
                };

                trasladosServicio.ActualizarInstanciaProyecto(proyectoTramiteDto).then(
                    function (resultado) {

                        if (resultado.data && (resultado.statusText === "OK" || resultado.status === 200)) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        } else {
                            swal('', "Error al realizar la operación", 'error');
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

        vm.btnGenerarCodigoPresupuestal_onClick = function ($event, sender) {
            const btnScope = angular.element(sender).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? btnScope.$parent.$parent.row.entity : null;
            trasladosServicio.actualizarCodigoPresupuestal(vm.TramiteId, entity.ProyectoId, entity.EntidadId).
                then(function (result) {
                    if (result != null && result.data != null)
                        trasladosServicio.obtenerCodigoPresupuestal(vm.TramiteId, entity.ProyectoId, entity.EntidadId)
                            .then(function (resultado) {
                                if (resultado.data != null)
                                    entity.CodigoPresupuestal = resultado.data.CodigoPresupuestal;
                            });
                });
        }

        function eventoValidarAnalista() {
            if (vm.IdEntidadSeleccionada == "" || vm.IdEntidadSeleccionada == null || vm.IdEntidadSeleccionada == undefined) {
                vm.callback({ arg: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                utilidades.mensajeError("Seleccione una entidad.");
            }
            else {
                vm.callback({ arg: false, titulo: 'ENVÍO PARA APROBACIÓN' });
                utilidades.mensajeSuccess("Formulario validado correctamente.");
            }
        }

        vm.ConsultarCodigoPresupuestal = function (proyectoId, tramiteId, entidadId) {
            //trasladosServicio.obtenerCodigoPresupuestal(vm.TramiteId, entity.ProyectoId, entity.EntidadId)
            //    .then(function (resultado) {
            //        if (resultado.data != null)
            //            return resultado.data.CodigoPresupuestal;
            //        else
            //            return '';
            //    });

        }

        function ObtenerSolicitarConcepto() {
            vm.peticion.IdFiltro = vm.TramiteId;
            vm.activarbotones = true;
            solicitarconceptoServicio.ObtenerSolicitarConcepto(vm.peticion)
                .then(resultado => {
                    vm.concepto = resultado.data;
                    $sessionStorage.concepto = vm.concepto;
                    vm.concepto.forEach(con => {
                        //if (vm.concepto.Id > 0 && vm.concepto.Enviado == 0) {
                        if (con.Id > 0 && con.Activo == 1 && con.Enviado == 0) {
                            vm.NombreSolicitarConcepto = "RECUPERAR CONCEPTO";
                            vm.activarbotones = false;
                            vm.callback({ arg: true, aprueba: true, titulo: '' });
                        }
                        else {
                            vm.NombreSolicitarConcepto = "SOLICITAR CONCEPTO";
                            vm.activarbotones = true;
                            vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROBACIÓN' });
                        }
                    });
                    //if (vm.activarbotones == false) {
                    //    vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                    //}
                    //else {
                    //    vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: false });
                    //}
                    AdministrarVistaFormulario();
                    if (vm.analista == false) {
                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                    }
                    else {
                        if (vm.activarbotones == false) {
                            vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        }
                        else {
                            vm.callback({ arg: true, aprueba: true, titulo: 'ENVÍO PARA APROVACIÓN', ocultarDevolver: false });
                        }
                    }
                    if (vm.subdirector == true) {
                        vm.callback({ arg: false, aprueba: true, titulo: 'ENVÍO PARA APROVACIÓN' });
                    }
                })
            //console.log(vm.isdisabled);
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

        function desactivartraslados() {
            vm.activartraslados = false;
            ObtenerProyectosTramite();
        }

        function desactivarcarta() {
            vm.activarcarta = false;
            vm.solicitaconcepto = false;
            vm.anteriorMostrarSolicitarConcpeto = undefined;
        }

        function generarConcepto() {
            $sessionStorage.TipoTramiteId = vm.tipoTramiteId;
            $sessionStorage.TramiteId = vm.TramiteId;
            $sessionStorage.TipoRolId = 1;
            $sessionStorage.numeroTramite = vm.numeroTramite;
            $timeout(function () {
                vm.activarcarta = true;
                if (vm.anteriorMostrarSolicitarConcpeto === undefined) {
                    vm.anteriorMostrarSolicitarConcpeto = vm.solicitaconcepto;
                    vm.solicitaconcepto = false;
                }
            }, 300);
        }

        function cambiaMostrarsolicitaconcepto() {

        }

        function ActualizaEstadoAjusteProyecto_onClick($event, sender) {
            const btnScope = angular.element(sender).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ?
                /*Acceder al scope de la fila y obtener el elemento enlazado*/
                btnScope.$parent.$parent.row.entity : null;
            if (entity.EstadoActualizacion.substring(0, 30) !== "Control Posterior DNP Aplicado") {
                 swal('', "El proyecto ya tiene un estado diferente a Control Posterior DNP Aplicado", 'warning');
            }
            else {
                $sessionStorage.ProyectoId = entity.ProyectoId;
                $sessionStorage.TramiteId = vm.TramiteId;
                $sessionStorage.BPIN = entity.BPIN;
                $uibModal.open({

                    templateUrl: '/src/app/formulario/ventanas/tramites/componentes/modalActualizaEstadoAjusteProyecto/modalActualizaEstadoAjusteProyecto.html',
                    controller: 'modalActualizaEstadoAjusteProyectoController',
                }).result.then(function (result) {
                    ObtenerProyectosTramite();
                }, function (reason) {

                }), err => {
                    toastr.error("Ocurrió un error Ver actualiza estado ajuste proyecto");
                };
            }
        };

        function VerificarEstadoProyecto() {
            if (vm.listaEntidadesProy !== undefined) {
                vm.listaEntidadesProy.forEach(con => {
                    if (con.EstadoActualizacion.substring(0, 30) !== "Control Posterior DNP Aplicado") {
                        vm.callback({ arg: true, aprueba: true, titulo: '' });
                        vm.conceptoenviado = true;
                        vm.visibleValidar = true;
                        vm.activarbotones = false;
                        
                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: true });
                        vm.MensajeEstado = "    Todos los proyectos deben estar en 'Control Posterior DNP Aplicado' ";

                        
                    }

                });
            }
        }



        vm.verPDF = function () {
            var ficha = {
                Nombre: constantesBackbone.apiBackBoneNombrePDFCarta,
            };
            vm.Ficha = ficha;
            var fichaPlantilla = {
                NombreReporte: ficha.Nombre,
                PARAM_BORRADOR: true,
                PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                TramiteId: vm.TramiteId
            };
            crearDocumento(fichaPlantilla).then(function (fichaTemporal) {
                FileSaver.saveAs(fichaTemporal, fichaTemporal.name);
            }, function (error) {
                utilidades.mensajeError(error);
            });
        };


        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = vm.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha(vm.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        //var blob = new Blob([respuesta], { type: 'application/pdf' });
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

                        if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            archivoServicios.cargarArchivo(archivo, $sessionStorage.IdAplicacion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    reject(response);
                                } else {
                                    resolve(fileOfBlob);
                                }
                            }, function (error) {
                                reject(error);
                            });
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }



    }

    angular.module('backbone').component('trasladoAprobacion', {
        templateUrl: "src/app/formulario/ventanas/tramites/trasladosAprobacion.html",
        controller: trasladosAprobacionTramiteController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });


})();