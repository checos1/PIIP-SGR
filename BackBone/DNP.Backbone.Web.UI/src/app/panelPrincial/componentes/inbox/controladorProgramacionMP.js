(function () {
	'use strict';
	angular.module('backbone').controller('programacionMPController', programacionMPController);

	programacionMPController.$inject = [
		'$scope',
		'servicioPanelPrincipal',
		'$uibModal',
		'$log',
		'$q',
		'$sessionStorage',
		'$localStorage',
		'$timeout',
		'$location',
		'$filter',
		'estadoAplicacionServicios',
		'constantesBackbone',
		'sesionServicios',
		'backboneServicios',
		'constantesAutorizacion',
		'configurarEntidadRolSectorServicio',
		'constantesTipoFiltro',
		'uiGridConstants',
		'FileSaver',
		'Blob',
		'constantesCondicionFiltro',
		'$routeParams',
		'flujoServicios',
		'utilidades',
		'autorizacionServicios',
		'$interval'
	];

	function programacionMPController(
		$scope,
		servicioPanelPrincipal,
		$uibModal,
		$log,
		$q,
		$sessionStorage,
		$localStorage,
		$timeout,
		$location,
		$filter,
		estadoAplicacionServicios,
		constantesBackbone,
		sesionServicios,
		backboneServicios,
		constantesAutorizacion,
		configurarEntidadRolSectorServicio,
		constantesTipoFiltro,
		uiGridConstants,
		FileSaver,
		Blob,
		constantesCondicionFiltro,
		$routeParams,
		flujoServicios,
		utilidades,
		autorizacionServicios,
		$interval) {

		var vm = this;
		vm.listanio = [];
		var currentTime = new Date();
		vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));
		vm.fechaUltimoDiaAnio = formatearFecha(obtenerFechaSinHoras(new Date(currentTime.getFullYear(), 11, 31)));
		vm.fechaInicialAnioInicial = formatearFecha(obtenerFechaSinHoras(new Date(currentTime.getFullYear() + 1, 0, 1)));
		vm.fechaFinalAnioInicial = formatearFecha(obtenerFechaSinHoras(new Date(currentTime.getFullYear() + 31, 11, 31)));;
		vm.fechaInicialAnioFinal = vm.fechaInicialAnioInicial;
		vm.fechaFinalAnioFinal = vm.fechaFinalAnioInicial;
		vm.listanioActual = [];
		var year = currentTime.getFullYear()
		vm.listanio.push({ Id: year, Name: year });
		vm.listanioActual.push({ Id: year, Name: year });
		for (var i = 1; i <= 20; i++) {
			vm.listanio.push({ Id: year + i, Name: year + i });
		}

		//Filtro

		vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
		vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
		vm.BusquedaRealizada = false;

		//Métodos
		vm.conmutadorFiltro = conmutadorFiltro;
		vm.abrirLogInstancias = abrirLogInstancias;
		// vm.conmutadorFiltroTramites = conmutadorFiltroTramites;
		// vm.filtrarPorNombreObjetoNegocio = filtrarPorNombreObjetoNegocio;
		// vm.mostrarInformacionDePropiedad = mostrarInformacionDePropiedad;
		// vm.cambiarEstado = cambiarEstado;
		vm.obtenerInbox = obtenerInbox;
		//vm.mostrarModal = mostrarModal;
		vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
		vm.cargarEntidades = cargarEntidades;
		//vm.contarCantidadObjetosNegocio = contarCantidadObjetosNegocio;
		vm.mostrarMensajeRespuesta = mostrarMensajeRespuesta;
		//vm.consultarAccion = consultarAccion;
		//vm.seleccionarTipoObjeto = seleccionarTipoObjeto;
		vm.cambioTipoEntidad = cambioTipoEntidad;
		vm.buscar = buscar;
		vm.limpiarCamposFiltro = limpiarCamposFiltro;
		vm.consultarAccion = consultarAccion;
		vm.mostrarLog = mostrarLog;
		vm.tipoProceso = "";
		vm.app = "";
		vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoTramite';
		vm.getTipoTramite = getTipoTramite;
		vm.cantidadFlujosProgramacion = 0;

		vm.toggleEvento = toggleEvento;
		vm.VisiblePlaneacion = false;
		vm.VisibleRecursos = false;
		vm.VisibleEjecucion = false;
		vm.VisibleEvaluacion = false;
		vm.classProcesos = "unProceso";
		vm.classDivSeleccionado = "u1923_div";
		vm.classIcoSeleccionado = "u1923";
		vm.classFlechas = "";
		vm.imgenSeleccion = "Img/etapas/macroprocesos/u829.svg";

		vm.cantidades = [];
		vm.macroProcesosCantidad = [];
		vm.listaProcesos = [];

		vm.getCodEntidade = getCodEntidade;
		vm.getAnioFinal = getAnioFinal;
		vm.getAnioInicial = getAnioInicial;
		vm.getDescripcion = getDescripcion;
		vm.getObjeto = getObjeto;
		vm.getTipoProceso = getTipoProceso;
		vm.getApp = getApp;
		//variables
		vm.tipoFiltro = constantesTipoFiltro.tramites;
		vm.cantidadDeProyectos = 0;
		vm.cantidadDeTramites = 0;
		vm.mostrarFiltro = false;
		vm.mostrarFiltroTramites = false;
		vm.busquedaNombreConId = "";
		vm.Mensaje = "";
		vm.columnas = servicioPanelPrincipal.columnasPorDefectoTramites;
		vm.columnasDisponiblesPorAgregar = servicioPanelPrincipal.columnasDisponiblesTramites;
		vm.mostrarCrearEditarLista = false;
		vm.gruposEntidadesProyectos = false;
		vm.gruposEntidadesTramites = false;
		vm.mostrarMensajeProyectos = false;
		vm.mostrarMensajeTramites = false;
		vm.idTipoProyecto = "";
		vm.idTipoTramiteProgramacion = "";
		vm.yaSeCargoInbox = false;
		vm.tipoEntidad = null;
		vm.filasFiltradas = null;
		vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
		vm.listaTipoEntidad = [];
		vm.listaFiltroSectores = [];
		vm.listaFiltroEntidades = [];
		vm.listaFiltroEstadoTramites = [];
		vm.listaFiltroTipoTramites = [];
		vm.tipoTramiteTexto = '';
		vm.plantillaTramites = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaTramites.html';
		vm.plantillaFilaTramites = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaFilaTramites.html';
		vm.sectorEntidadFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaSectorEntidad.html';
		vm.accionesFilaTramitesTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaAccionesTramite.html';
		vm.consultarAccionFlujoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaConsultarAccionFlujo.html';

		vm.puedeVerFiltroCodigo;
		vm.puedeVerFiltroDescripcion;
		vm.puedeVerFiltroFecha;
		vm.puedeVerFiltroValorProprio;
		vm.puedeVerFiltroValorSGR;
		vm.puedeVerFiltroIdentificador;
		vm.puedeVerFiltroSector;
		vm.puedeVerFiltroEntidad;
		vm.puedeVerFiltroIdentificadorCR;
		vm.puedeVerFiltroEstadoTramite;
		vm.downloadPdf = downloadPdf;
		vm.downloadExcel = downloadExcel;
		vm.puedeVerFiltroTipoTramite;
		vm.puedeVerFiltroNombreFlujo;
		vm.puedeVerFiltroAccionFlujo;
		vm.restablecerBusqueda = restablecerBusqueda;

		vm.etapa = $routeParams['etapa'];
		vm.nombreEtapa = "";
		vm.tipoProceso = 'tramites';
		vm.misProcesos = {};
		vm.cantidadDeTramites = 0;

		function obtenerFechaSinHoras(fecha) {
			return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
		}

		function formatearFecha(fecha) {
			let fechaString = fecha.toISOString();
			return fechaString.substring(0, 10);
		}

		$interval(function () {
			vm.misProcesos = $localStorage.misProcesos;
		}, 1800);

		vm.peticionObtenerInbox = {
			// ReSharper disable once UndeclaredGlobalVariableUsing
			IdUsuario: usuarioDNP,
			IdObjeto: idTipoTramiteProgramacion,
			// ReSharper disable once UndeclaredGlobalVariableUsing
			Aplicacion: nombreAplicacionBackbone,
			ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
			IdsEtapas: getIdEtapa()
		};

		//#region Componente Crear Tramites

		vm.isOpen = false;
		vm.tramites = [];
		vm.cargarFlujosTramites = cargarFlujosTramites;
		vm.listaEntidades = [];
		vm.listaEntidadesGeneral = [];
		vm.listarFiltroEntidades = listarFiltroEntidades;
		vm.guardarSolicitud = guardarSolicitud;
		vm.cerrarTramite = cerrarTramite;

		vm.popOverOptionsCrearTramite = {
			isOpen: false,
			templateUrl: 'myPopoverTemplate.html',
			toggle: function () {
				vm.popOverOptionsCrearTramite.isOpen = !vm.popOverOptionsCrearTramite.isOpen;
			}
		};

		vm.objSolicitud = {
			codTramite: null,
			codEntidade: null,
			descripcion: null
		};

		vm.AbrilNivel = function (idEntidad) {
			vm.listaEntidadesGeneral.forEach(function (value, index) {
				if (value.idEntidad == idEntidad) {
					if (value.estadoEntidad == '+')
						value.estadoEntidad = '-';
					else
						value.estadoEntidad = '+';
				}
			});
		}

		if (vm.etapa == "pl") {
			vm.nombreEtapa = "Planeación";
			toggleEvento(1);
		}
		if (vm.etapa == "gr") {
			vm.nombreEtapa = "Gestión de recursos";
			toggleEvento(2);
		}
		if (vm.etapa == "ej") {
			vm.nombreEtapa = "Ejecución";
			toggleEvento(3);
		}
		if (vm.etapa == "ev") {
			vm.nombreEtapa = "Evaluación";
			toggleEvento(4);
		}

		vm.columnasReporte = ['numeroTramite', 'tipoTramite', 'nombreFlujo', 'sector', 'entidad', 'estadoTramite', 'fecha', 'nombreAccion', 'fechaPaso'];
		function toggleEvento(proceso) {

			if (proceso === 1 && !vm.VisiblePlaneacion) {
				vm.VisiblePlaneacion = true;
				vm.classFlechas = "planeacionSeleccionado";
				vm.imgenSeleccionPlaneacion = "Img/etapas/macroprocesos/u829.svg";
			}
			else {
				vm.VisiblePlaneacion = false;
				vm.imgenSeleccionPlaneacion = "Img/etapas/macroprocesos/u838.svg";
			}
			if (proceso === 2 && !vm.VisibleRecursos) {
				vm.VisibleRecursos = true;
				vm.classFlechas = "recursosSeleccionado";
				vm.imgenSeleccionRecursos = "Img/etapas/macroprocesos/u840.svg";
			}
			else {
				vm.VisibleRecursos = false;
				vm.imgenSeleccionRecursos = "Img/etapas/macroprocesos/u821.svg";
			}
			if (proceso === 3 && !vm.VisibleEjecucion) {
				vm.VisibleEjecucion = true;
				vm.classFlechas = "ejecucionSeleccionado";
				vm.imgenSeleccionEjecucion = "Img/etapas/macroprocesos/u840.svg";
			}
			else {
				vm.VisibleEjecucion = false;
				vm.imgenSeleccionEjecucion = "Img/etapas/macroprocesos/u821.svg";
			}
			if (proceso === 4 && !vm.VisibleEvaluacion) {
				vm.VisibleEvaluacion = true;
				vm.classFlechas = "evaluacionSeleccionado";
				vm.imgenSeleccionEvaluacion = "Img/etapas/macroprocesos/u840.svg";
			}
			else {
				vm.VisibleEvaluacion = false;
				vm.imgenSeleccionEvaluacion = "Img/etapas/macroprocesos/u821.svg";
			}


		}

		function abrirLogInstancias(row) {


			var modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/comunes/log/modalLogInstancias.html',
				controller: 'modalLogInstanciasController',
				controllerAs: "vm",
				openedClass: "consola-modal-soportesDNP",
				resolve: {
					idInstancia: () => row.IdInstancia,
					BPIN: () => row.IdObjetoNegocio,
					nombreFlujo: () => row.NombreFlujo,
					codigoProceso: () => row.IdObjetoNegocio
				}
			});

			modalInstance.result.then(function (selectedItem) {


			}, function () {

			});
		}
		function getCodEntidade() {
			if (vm.objSolicitud.codEntidade != '') {
				document.getElementById("codEntidadeError").innerHTML = "";
				document.getElementById("codEntidadeError").classList.add('hidden');
			}
		}
		function getAnioFinal() {
			if (vm.objSolicitud.anioFinal != '') {
				document.getElementById("anioFinalErrorVFO").innerHTML = "";
				document.getElementById("anioFinalErrorVFO").classList.add('hidden');
				document.getElementById("anioFinalErrorVFE").innerHTML = "";
				document.getElementById("anioFinalErrorVFE").classList.add('hidden');
			}
		}
		function getAnioInicial() {
			if (vm.objSolicitud.anioInicial != '') {
				document.getElementById("anioInicialErrorVFO").innerHTML = "";
				document.getElementById("anioInicialErrorVFO").classList.add('hidden');
				document.getElementById("anioInicialErrorVFE").innerHTML = "";
				document.getElementById("anioInicialErrorVFE").classList.add('hidden');
			}
		}
		function getDescripcion() {
			if (vm.objSolicitud.descripcion != '') {
				document.getElementById("descripcionError").innerHTML = "";
				document.getElementById("descripcionError").classList.add('hidden');
			}
		}
		function getObjeto() {
			if (vm.objSolicitud.objeto != '') {
				document.getElementById("objetoErrorVFE").innerHTML = "";
				document.getElementById("objetoErrorVFE").classList.add('hidden');
				document.getElementById("objetoErrorVFO").innerHTML = "";
				document.getElementById("objetoErrorVFO").classList.add('hidden');
			}
		}
		function getTipoProceso() {
			document.getElementById("tipoProcesoError").innerHTML = "";
			document.getElementById("tipoProcesoError").classList.add('hidden');
		}
		function getApp() {
			document.getElementById("appErrorVFE").innerHTML = "";
			document.getElementById("appErrorVFE").classList.add('hidden');
			document.getElementById("appErrorVFO").innerHTML = "";
			document.getElementById("appErrorVFO").classList.add('hidden');
		}
		function restablecerBusqueda() {
			vm.limpiarCamposFiltro();
			vm.buscar(true);
			var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
			iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
		}
		//#endregion
		function getTipoTramite() {
			if (vm.objSolicitud.codTramite != '') {
				document.getElementById("tipoTramiteError").innerHTML = "";
				document.getElementById("tipoTramiteError").classList.add('hidden');
			}
			vm.objSolicitud.codEntidade = '';
			vm.objSolicitud.descripcion = '';
			vm.objSolicitud.objeto = '';
			vm.objSolicitud.anioFinal = '';
			vm.objSolicitud.anioInicial = '';
			vm.tipoProceso = '';
			vm.app = '';
			var data = vm.tramites.find(t => t.id === vm.objSolicitud.codTramite);
			vm.tipoProceso = null;
			vm.app = null;
			removerMensajeError();
			if (data.alias == 'VFO' || data.alias == 'VFE' || data.alias == 'VF') {
				if (data.alias == 'VFO') {
					$('#seccionTipoProceso').show();
					$('#seccionPublicoPrivada').hide();
					$('#seccionObjetoProcesoVFO').show();
					$('#seccionObjetoProcesoVFE').hide();
				}
				if (data.alias == 'VFE') {
					$('#seccionTipoProceso').hide();
					$('#seccionPublicoPrivada').show();
					$('#seccionObjetoProcesoVFE').show();
					$('#seccionObjetoProcesoVFO').hide();
				}

				$('#seccionDescripcion').hide();
				vm.tipoTramiteTexto = ' tipo vigencia futura';
				var year = currentTime.getFullYear();
				vm.objSolicitud.anioInicio = year;
			}
			else {
				$('#seccionPublicoPrivada').hide();
				$('#seccionTipoProceso').hide();
				$('#seccionObjetoProcesoVFE').hide();
				$('#seccionObjetoProcesoVFO').hide();
				$('#seccionDescripcion').show();
				vm.tipoTramiteTexto = '';
				vm.objSolicitud.anioInicio = '';
			}
		}

		function downloadExcel() {

			servicioPanelPrincipal.obtenerExcelTramites(vm.peticionObtenerInbox, vm.tramiteFiltro, vm.columnasReporte).then(function (retorno) {
				var blob = new Blob([retorno.data], {
					type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
				});
				// FileSaver.saveAs(blob,nombreDelArchivo(retorno));
				FileSaver.saveAs(blob, "Tramites.xlsx");
			}, function (error) {
				vm.Mensaje = error.data.Message;
				mostrarMensajeRespuesta();
			});

		}

		function downloadPdf() {
			//var peticionObtenerInbox = {
			//    IdUsuario: usuarioDNP,
			//    IdObjeto: idTipoTramite,
			//    Aplicacion: nombreAplicacionBackbone,
			//    ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
			//};
			servicioPanelPrincipal.obtenerPdfInboxTramites(vm.peticionObtenerInbox, vm.tramiteFiltro, vm.columnasReporte).then(
				function (data) {
					servicioPanelPrincipal.imprimirPdfTramites(data.data).then(function (retorno) {
						FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
					});
				}, function (error) {
					vm.Mensaje = error.data.Message;
					mostrarMensajeRespuesta();
				});
		};

		function nombreDelArchivo(response) {
			var filename = "";
			var disposition = response.headers("content-disposition");
			if (disposition && disposition.indexOf('attachment') !== -1) {
				var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
				var matches = filenameRegex.exec(disposition);
				if (matches != null && matches[1]) {
					filename = matches[1].replace(/['"]/g, '');
				}
			}
			return filename;
		}

		vm.tramiteFiltro = {
			codigo: {
				campo: 'Id',
				valor: null,
				tipo: constantesCondicionFiltro.igual,
				width: '20%'
			},
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
			estadoId: {
				campo: 'EstadoId',
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
			},
			estado: {
				campo: 'DescEstado',
				valor: null,
				tipo: constantesCondicionFiltro.contiene
			},
			tipoTramiteId: {
				campo: 'TipoTramiteId',
				valor: null,
				tipo: constantesCondicionFiltro.igual
			},
			numeroTramite: null,
			Macroproceso: vm.nombreEtapa
		};

		//const tipoEntidad = entidad.GrupoTramites[0].NombreTipoEntidad;
		vm.columnDefPrincial = [{
			field: 'composto',
			displayName: 'Entidad',
			enableHiding: false,
			width: '96%',
			cellTemplate: vm.sectorEntidadFilaTemplate,
		}];

		vm.columnDefTramite = [{
			field: 'composto',
			displayName: 'Trámite',
			enableFiltering: false,
			showHeader: false,
			enableHiding: false,
			width: '95%',
			cellTemplate: vm.sectorEntidadFilaTemplate,
			headerCellTemplate: '<div></div>'
		}];

		vm.columnDef = [
			//{
			//    field: 'codigo',
			//    displayName: 'Codigo',
			//    enableHiding: false,
			//    width: '9%'
			//},
			{
				field: 'descripcion',
				displayName: 'Descripción',
				enableHiding: false,
				width: '39%',
				cellTooltip: (row, col) => row.entity[col.field]
			},
			{
				field: 'fecha',
				displayName: 'Fecha',
				enableHiding: false,
				width: '9%',
				type: "date",
				cellFilter: 'date:\'dd/MM/yyyy\'',
				cellTooltip: (row, col) => row.entity[col.field]
			},
			{
				field: 'entidad',
				displayName: 'Entidad',
				enableHiding: false,
				width: '9%',
				cellTooltip: (row, col) => row.entity[col.field]
			},
			{
				field: 'estadoTramite',
				displayName: 'Estado Trámite',
				enableHiding: false,
				width: '10%'
			},
			{
				field: 'accionFlujo',
				displayName: 'Nombre/Accion Flujo',
				enableHiding: false,
				enableColumnMenu: false,
				width: '20%',
				cellTemplate: vm.consultarAccionFlujoTemplate,
				cellTooltip: (row, col) => row.entity[col.field]
			},
			{
				field: 'sector',
				displayName: 'Sector',
				enableHiding: false,
				width: '9%',
				cellTooltip: (row, col) => row.entity[col.field]

			},
			{
				field: 'accion',
				displayName: 'Acción',
				enableFiltering: false,
				enableHiding: false,
				enableSorting: false,
				enableColumnMenu: false,
				// pinnedRight: true,
				cellTemplate: vm.accionesFilaTramitesTemplate,
				width: '200',
				cellTooltip: (row, col) => row.entity[col.field]
			}
		];

		vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
		vm.listaEntidades = [];
		vm.listaTramites = [];
		vm.listaTramitesCompleta = [];
		vm.listaDatos = [];

		function onRegisterApi(gridApi) {
			$scope.gridApi = gridApi;
			$scope.gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
				if (row.entity.subGridOptions.data.length === 1) {
					$scope.vm.gridOptions.expandableRowHeight = 80;
					if (row.isExpanded === true) {
						row.expandedRowHeight = row.expandedRowHeight - 80;
					}
					else {
						row.expandedRowHeight = 80;
					}


				}
				else {
					$scope.vm.gridOptions.expandableRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
					var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
					if (row.isExpanded === true) {
						row.expandedRowHeight = row.expandedRowHeight - heigrow;
					}
					else {
						row.expandedRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
					}

				}
			})
		}
		function onRegisterApi2(gridApi) {
			$scope.gridApi = gridApi;
			$scope.gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
				if (row.entity.subGridOptions.data.length === 1) {
					row.grid.options.expandableRowHeight = 80;
					var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
					if (row.isExpanded === true) {
						row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight - 80;
					}
					else {
						row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight + 80;
					}
				}
				else {
					row.grid.options.expandableRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
					var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
					if (row.isExpanded === true) {
						row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight - heigrow;
					}
					else {
						row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight + 50 + row.entity.subGridOptions.data.length * 30;
					}

				}
			})
		}

		function agregarColumnas() {
			let lista = vm.listaTramites;
			var colAcciones;
			var addCol;
			for (var i = 0, len = lista.length; i < len; i++) {
				var tramite = lista[i];

				for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
					var col = vm.columnas[j];

					if (col !== "Nombre Flujo" && col !== "Accion Flujo" && vm.columnDef.map(x => x && x.displayName).indexOf(col) == -1) {
						addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
						colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

						tramite.subGridOptions.columnDefs.pop();
						tramite.subGridOptions.columnDefs.push(addCol);
						tramite.subGridOptions.columnDefs.push(colAcciones);
					} else {


						//if (col == 'Identificador') {
						//    if (vm.columnDef.map(x => x.displayName).indexOf('Tipo de Trámite') == -1) {
						//        addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Tipo de Trámite')[0]
						//        colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

						//        tramite.subGridOptions.columnDefs.pop();
						//        tramite.subGridOptions.columnDefs.push(addCol);
						//        tramite.subGridOptions.columnDefs.push(colAcciones);
						//    }
						//} else {
						//if (vm.columnDef.map(x => x.displayName).indexOf(col) == -1) {
						//    addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
						//    colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

						//    tramite.subGridOptions.columnDefs.pop();
						//    tramite.subGridOptions.columnDefs.push(addCol);
						//    tramite.subGridOptions.columnDefs.push(colAcciones);
						//}
						//}
					}
				}

				vm.listaTramites[i] = tramite;
			}
		}

		function borrarColumnas() {
			let lista = vm.listaTramites;
			for (var i = 0, len = lista.length; i < len; i++) {
				var tramite = lista[i];

				for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
					var indexEliminar = -1;
					var col = vm.columnasDisponiblesPorAgregar[j];

					//if (col == 'Identificador') {
					//    if (tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite') > -1) {
					//        indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite');
					//    }
					//} else {
					indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf(col);
					//}

					if (indexEliminar >= 0) tramite.subGridOptions.columnDefs.splice(indexEliminar, 1);
				}

				vm.listaTramites[i] = tramite;
			}
		}

		function configurarColumnas() {
			// vuelve a las columnas originales primero
			borrarColumnas();
			// agrega nuevas columnas en todas las filas del modelo
			agregarColumnas();
		}

		function buscarColumnasLocalStorage() {
			if ($localStorage.tipoFiltro) {
				if ($localStorage.tipoFiltro.tramites) {
					vm.columnas = $localStorage.tipoFiltro.tramites.columnasActivas;
					vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.tramites.columnasDisponibles;
				}
			}

			vm.puedeVerFiltroCodigo = vm.columnas.indexOf('Codigo') > -1;
			vm.puedeVerFiltroDescripcion = vm.columnas.indexOf('Descripción') > -1;
			vm.puedeVerFiltroFecha = vm.columnas.indexOf('Fecha') > -1;
			vm.puedeVerFiltroValorProprio = vm.columnas.indexOf('Valor Proprio') > -1;
			vm.puedeVerFiltroValorSGR = vm.columnas.indexOf('Valor SGR') > -1;
			vm.puedeVerFiltroIdentificador = vm.columnas.indexOf('Identificador') > -1;
			vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;
			vm.puedeVerFiltroEntidad = vm.columnas.indexOf('Entidad') > -1;
			vm.puedeVerFiltroIdentificadorCR = vm.columnas.indexOf('Identificador CR') > -1;
			vm.puedeVerFiltroEstadoTramite = vm.columnas.indexOf('Estado Trámite') > -1;
			vm.puedeVerFiltroTipoTramite = vm.columnas.indexOf('Tipo de Trámite') > -1;
			vm.puedeVerFiltroNombreFlujo = vm.columnas.indexOf('Nombre Flujo') > -1;
			vm.puedeVerFiltroAccionFlujo = vm.columnas.indexOf('Accion Flujo') > -1;
		}

		function crearListaTipoEntidad() {
			return [{
				Nombre: constantesAutorizacion.tipoEntidadNacional,
				Descripcion: constantesAutorizacion.tipoEntidadNacional,
				Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadNacional)
			},
			{
				Nombre: constantesAutorizacion.tipoEntidadTerritorial,
				Descripcion: constantesAutorizacion.tipoEntidadTerritorial,
				Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadTerritorial)
			},
			{
				Nombre: constantesAutorizacion.tipoEntidadSGR,
				Descripcion: constantesAutorizacion.tipoEntidadSGR,
				Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadSGR)
			},
			{
				nombre: constantesAutorizacion.tipoEntidadPrivadas,
				Descripcion: constantesAutorizacion.tipoEntidadPrivadas,
				Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPrivadas)
			},
			{
				Nombre: constantesAutorizacion.tipoEntidadPublicas,
				Descripcion: "Públicas",
				Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPublicas)
			}
			];
		}

		function cambioTipoEntidad(tipoEntidad) {
			limpiarCamposFiltro();
			vm.tipoEntidad = tipoEntidad;
			vm.tramiteFiltro.tipoEntidad.valor = vm.tipoEntidad;
			vm.BusquedaRealizada = false;
			cargarEntidades(idTipoProyecto).then(function () {
				mostrarMensajeRespuesta();
				listarFiltroSectores();
				listarFiltroEntidades();
				listarFiltroEntidadesCrearTramite();
				listarFiltroEstadoTramites();
				listarFiltroTipoTramites();
				vm.BusquedaRealizada = false;
			});
		}

		function obtenerConfiguracionesRolSector() {
			var parametros = {
				usuarioDnp: usuarioDNP,
				nombreAplicacion: nombreAplicacionBackbone
			}
			return configurarEntidadRolSectorServicio.obtenerConfiguracionesRolSector(parametros).then(function (respuesta) {
				vm.listaConfiguracionesRolSector = respuesta;
				vm.listaTipoEntidad = crearListaTipoEntidad();
			});
		}

		function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
			var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
				TipoEntidad: tipoEntidad
			});
			return tipoConfiguracion ? true : false;
		}

		function buscar(restablecer) {
			var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
			iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
			return cargarEntidades(idTipoProyecto).then(function () {
				mostrarMensajeRespuesta();
				if (restablecer)
					vm.BusquedaRealizada = false;
				else
					vm.BusquedaRealizada = true;
			});
		}

		function limpiarCamposFiltro() {
			//vm.tramiteFiltro.codigo.valor = null;
			vm.tramiteFiltro.descripcion.valor = null;
			vm.tramiteFiltro.fechaDesde.valor = null;
			vm.tramiteFiltro.fechaHasta.valor = null;
			vm.tramiteFiltro.sectorId.valor = null;
			vm.tramiteFiltro.entidadId.valor = null;
			vm.tramiteFiltro.nombreFlujo.valor = null;
			vm.tramiteFiltro.accionFlujo.valor = null;
			vm.tramiteFiltro.tipoTramiteId.valor = null;
			vm.tramiteFiltro.estado.valor = null;
			vm.tramiteFiltro.numeroTramite = null;
		}

		function listarFiltroSectores() {
			servicioPanelPrincipal.obtenerSectores(vm.peticionObtenerInbox).then(exito, error);

			function exito(respuesta) {
				let listaSectoresGrid = [];
				//vm.listaEntidades.forEach(entidade => {
				//    entidade.subGridOptions.data.forEach(item =>
				//        listaSectoresGrid.push(item.SectorNombre));
				//});
				let listaFiltroSectores = [];
				if (respuesta.data && respuesta.data.length > 0) {
					listaFiltroSectores = respuesta.data;
					vm.listaFiltroSectores = listaFiltroSectores; //.filter(item => listaSectoresGrid.includes(item.Name));
				}
			}

			function error() {
				vm.listaFiltroSectores = [];
			}
		}

		function listarFiltroEntidades() {
			servicioPanelPrincipal.obtenerEntidadesVisualizador(usuarioDNP).then(exito, error);

			function exito(respuesta) {
				let listaEntidadesGrid = [];
				//vm.listaEntidades.forEach(entidade => {
				//    entidade.subGridOptions.data.forEach(item =>
				//        listaEntidadesGrid.push(item.NombreEntidad));
				//});
				let listaFiltroEntidades = [];
				if (respuesta.data && respuesta.data.length > 0) {
					listaFiltroEntidades = respuesta.data;
				}
				vm.listaFiltroEntidades = listaFiltroEntidades;
			}

			function error() {
				vm.listaFiltroEntidades = [];
			}
		}

		function listarFiltroEstadoTramites() {
			//let listaEstadoTramitesGrid = [];

			//vm.listaEntidades.forEach(entidade => {
			//    if (entidade.tipoEntidad == vm.tipoEntidad) {
			//        entidade.subGridOptions.data.forEach(item => {

			//            item.subGridOptions.data.forEach(tramite => {

			//                listaEstadoTramitesGrid.push({ value: tramite.estadoTramite, text: tramite.estadoTramite });
			//            });

			//        });
			//    }
			//});

			//const seen = new Set();
			vm.listaFiltroEstadoTramites = [];
			vm.listaFiltroEstadoTramites.push({ value: "Activo", text: "Activo" });
			vm.listaFiltroEstadoTramites.push({ value: "Anulado por alcance", text: "Anulado por alcance" });
			vm.listaFiltroEstadoTramites.push({ value: "Completado", text: "Completado" });
			vm.listaFiltroEstadoTramites.push({ value: "Pausado", text: "Pausado" });
			vm.listaFiltroEstadoTramites.push({ value: "Cancelado", text: "Cancelado" });
			vm.listaFiltroEstadoTramites.push({ value: "Anulado", text: "Anulado" });
			//vm.listaFiltroEstadoTramites = listaEstadoTramitesGrid.filter(el => {
			//    const duplicate = seen.has(el.value);
			//    seen.add(el.value);
			//    return !duplicate;
			//});

		}

		function listarFiltroTipoTramites() {
			servicioPanelPrincipal.obtenerTiposTramite(vm.peticionObtenerInbox, vm.tramiteFiltro).then(exito, error);

			function exito(respuesta) {
				let listaFiltroTipoTramites = [];
				if (respuesta.data && respuesta.data.length > 0) {
					listaFiltroTipoTramites = respuesta.data;
				}
				vm.listaFiltroTipoTramites = listaFiltroTipoTramites;
			}

			function error(error) {
				vm.listaFiltroTipoTramites = [];
			}
			/*
			let listaTipoTramitesGrid = [];

			vm.listaEntidades.forEach(entidade => {
				if (entidade.tipoEntidad == vm.tipoEntidad) {
					entidade.subGridOptions.data.forEach(item => {

						item.subGridOptions.data.forEach(tramite => {
							if (tramite.tipoTramiteId != null)
								listaTipoTramitesGrid.push({ value: tramite.tipoTramiteId, text: tramite.tipoTramite });
						});

					});
				}
			});

			const seen = new Set();
			vm.listaFiltroTipoTramites = [];
			vm.listaFiltroTipoTramites = listaTipoTramitesGrid.filter(el => {
				const duplicate = seen.has(el.value);
				seen.add(el.value);
				return !duplicate;
			});*/

		}

		//Implementaciones
		$scope.$on('AutorizacionConfirmada', function () {
			$timeout(function () {
				if (vm.yaSeCargoInbox === false) {
					var roles = sesionServicios.obtenerUsuarioIdsRoles();
					if (roles != null && roles.length > 0) {
						activar(); //Realizar la carga
					} else {
						vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
						mostrarMensajeRespuesta(idTipoProyecto);
					}
				}
			});
		});

		function cargueInicial() {
			limpiarCamposFiltro();
			servicioPanelPrincipal.obtenerMacroprocesosCantidad().then(exitoCantidad, errorCantidad);
			function exitoCantidad(respuesta) {
				vm.macroProcesosCantidad = [];
				vm.cantidades = [];
				for (var i = 0; i < respuesta.data.length; i++) {
					vm.macroProcesosCantidad.push(respuesta.data[i]);
				}
				$localStorage.cantidadMacroprocesosList = vm.macroProcesosCantidad;
				var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
				var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
				var cantidadPlaneacionProyecto = 0;
				if (data.length > 0) {
					for (var i = 0; i < data.length; i++) {
						cantidadPlaneacionProyecto = cantidadPlaneacionProyecto + data[i].Cantidad;
					}
				}
				if (vm.etapa == 'pl') {
					vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion);
				}
				var dataGR = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSolicitudRecursos ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaRevisionRequisitos ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAprobacion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesGR) && x.TipoObjeto == 'Proyecto');
				var cantidadRecursosProyecto = 0;
				if (dataGR.length > 0) {
					for (var i = 0; i < dataGR.length; i++) {
						cantidadRecursosProyecto = cantidadRecursosProyecto + dataGR[i].Cantidad;
					}
				}
				if (vm.etapa == 'gr') {
					vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos);
				}
				var dataE = vm.macroProcesosCantidad.filter(x => (String(x.idEtapaNuevaEjecucion).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaProgramacionEjecucion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesEjecucion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaTramitesEjecucion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSeguimientoControl) && x.TipoObjeto == 'Proyecto');
				var cantidadEjecucionProyecto = 0;
				if (dataE.length > 0) {
					for (var i = 0; i < dataE.length; i++) {
						cantidadEjecucionProyecto = cantidadEjecucionProyecto + dataE[i].Cantidad;
					}
				}
				if (vm.etapa == 'ej') {
					vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion);

				}
				var dataEV = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaCortoPlazo ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaMedianoPlazo ||
					String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaLargoPlazo) && x.TipoObjeto == 'Proyecto');
				var cantidadEvaluacionProyecto = 0;
				if (dataEV.length > 0) {
					for (var i = 0; i < dataEV.length; i++) {
						cantidadEvaluacionProyecto = cantidadEvaluacionProyecto + dataEV[i].Cantidad;
					}
				}
				if (vm.etapa == 'ev') {
					vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion);
				}


				var cantidadTramiteEjecucion = 0;

				var dataTramite = vm.macroProcesosCantidad.filter(x => (String(x.IdNivelPadre == constantesBackbone.idEtapaNuevaEjecucion)) && x.TipoObjeto == 'Trámite');
				if (dataTramite.length > 0) {
					for (var i = 0; i < dataTramite.length; i++) {
						cantidadTramiteEjecucion = cantidadTramiteEjecucion + dataTramite[i].Cantidad;
					}
				}

				var cantidadProgramacionEjecucion = 0;

				var dataPE = vm.macroProcesosCantidad.filter(x => (String(x.IdNivelPadre == constantesBackbone.idEtapaProgramacionEjecucion)) && x.TipoObjeto == 'Programación');
				if (dataPE.length > 0) {
					for (var i = 0; i < dataPE.length; i++) {
						cantidadProgramacionEjecucion = cantidadProgramacionEjecucion + dataPE[i].Cantidad;
					}
				}
				if (cantidadProgramacionEjecucion > 0) {
					vm.cantidadFlujosProgramacion = cantidadProgramacionEjecucion;
				}
				vm.cantidades.push({ 'PProyecto': cantidadPlaneacionProyecto, 'GRProyecto': cantidadRecursosProyecto, 'EJProyecto': cantidadEjecucionProyecto, 'EVProyecto': cantidadEvaluacionProyecto, 'EJTramite': cantidadTramiteEjecucion, 'EJProgramacion': cantidadProgramacionEjecucion, 'EJTotal': cantidadTramiteEjecucion + cantidadEjecucionProyecto + cantidadProgramacionEjecucion });
				$localStorage.cantidadesMisproyectos = vm.cantidades;
			}
			function errorCantidad(respuesta) {
			}


			// buscar columnas en el localstorage
			buscarColumnasLocalStorage();

			if (!vm.gridOptions) {
				vm.gridOptions = {
					expandableRowTemplate: vm.plantillaTramites,
					expandableRowScope: {
						subGridVariable: 'subGridScopeVariable'
					},
					enableFiltering: false,
					showHeader: false,
					paginationPageSizes: [10, 15, 25, 50, 100],
					paginationPageSize: 10,
					onRegisterApi: onRegisterApi
				};

				vm.gridOptions.columnDefs = vm.columnDefPrincial;
				vm.gridOptions.data = vm.listaEntidades;
			}

			vm.idTipoTramiteProgramacion = idTipoTramiteProgramacion;
			var roles = sesionServicios.obtenerUsuarioIdsRoles();
			if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
				activar();
			}
			
			//configurarColumnas();
			configurarNombreEtapa();

			listarFiltroEntidades();
			listarFiltroEntidadesCrearTramite();
			cargarFlujosTramites();
		}
		this.$onInit = function () {
			cargueInicial();
		};


		function activar() {
			vm.yaSeCargoInbox = true;
			// ReSharper disable once UndeclaredGlobalVariableUsing
			// obtenerInbox(idTipoTramite).then(function () {
			//     // ReSharper disable once UndeclaredGlobalVariableUsing
			//     return obtenerInbox(vm.idTipoProyecto);
			// });
			//escucharEventos();


			obtenerInbox(idTipoProyecto); // PARA TESTAR
			//obtenerInbox(idTipoTramite); CORRETO
		}

		function buscarColumnasPorColumnasFiltroSeleccionadas() {
			let listaColumnas = [];
			let columna = ''

			for (let i = 0; i < vm.columnas.length; i++) {
				var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

				if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
					nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
				}

				var result = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro);
				if (result != null && result.length > 0) {
					columna = result[0].field;
					if (listaColumnas.indexOf(columna) == -1) {
						listaColumnas.push(columna);
					}
				}
			}

			return listaColumnas;
		}

		function cargarEntidades(idObjeto) {
			//const columnasVisibles = vm.listaTramites[0].subGridOptions.columnDefs.map(x => x.field).filter(x => x != 'accion');
			const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();

			return servicioPanelPrincipal.obtenerProgramaciones(vm.peticionObtenerInbox, vm.tramiteFiltro, columnasVisibles).then(
				function (respuesta) {

					vm.cantidadDeTramites = 0;
					// ReSharper disable once UndeclaredGlobalVariableUsing
					// if (idObjeto === vm.idTipoProyecto) {
					//     vm.gruposEntidadesProyectos = respuesta.data.Gru posEntidades;
					// } else {
					//     vm.gruposEntidadesTramites = respuesta.data.GruposEntidades;
					// }

					vm.listaEntidades = [];
					vm.listaTramites = [];
					vm.listaEntidadesGeneral = [];
					vm.listaDatos = [];
					vm.listaTramitesCompleta = [];

					if (respuesta.data.ListaGrupoTramiteEntidad && respuesta.data.ListaGrupoTramiteEntidad.length > 0) {
						vm.listaEntidades = [];
						vm.listaEntidadesGeneral = [];
						const listaGrupoEntidades = respuesta.data.ListaGrupoTramiteEntidad;
						listaGrupoEntidades.forEach(entidad => {
							const nombreEntidad = entidad.NombreEntidad;
							const tipoEntidad = entidad.GrupoTramites[0].NombreTipoEntidad;
							const nombreSector = entidad.Sector;
							const idSector = entidad.IdSector;
							const idEntidad = entidad.EntidadId;
							vm.listaTramitesCompleta = [];
							vm.listaTramites = [];
							entidad.GrupoTramites.forEach(tramite => {
								const nombreTipoTramite = tramite.NombreTipoTramite;
								vm.listaDatos = [];
								vm.cantidadDeTramites += tramite.ListaTramites.length;
								tramite.ListaTramites.forEach(instancia => {
									vm.listaTramitesCompleta.push({
										tipoTramite: nombreTipoTramite,
										codigo: instancia.Id,
										descripcion: instancia.Descripcion,
										fecha: instancia.FechaCreacionTramite,
										valorProprio: instancia.ValorProprio,
										valorSGR: instancia.ValorSGP,
										tipoTramite: instancia.TipoTramite.Nombre,
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
										IdFlujo: instancia.FlujoId,
										NombreFlujo: instancia.NombreFlujo,
										entidadId: idEntidad,
										tramiteId: instancia.TramiteId,
										fechaPaso: instancia.FechaCreacion,
										numeroTramite: instancia.NumeroTramite == null ? i.idObjetoNegocio : instancia.NumeroTramite
									});
									vm.listaDatos.push({
										codigo: instancia.Id,
										descripcion: instancia.Descripcion,
										fecha: instancia.FechaCreacion,
										valorProprio: instancia.ValorProprio,
										valorSGR: instancia.ValorSGP,
										tipoTramite: instancia.TipoTramite.Nombre,
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
										IdFlujo: instancia.FlujoId,
										NombreFlujo: instancia.NombreTipoTramite,
										tramiteId: instancia.TramiteId,
										entidadId: idEntidad
									});
								});

								vm.listaTramites.push({
									sector: 'Tramite',
									entidad: nombreTipoTramite,
									subGridOptions: {
										columnDefs: vm.columnDef,
										appScopeProvider: $scope,
										paginationPageSizes: [5, 10, 15, 25, 50, 100],
										paginationPageSize: 5,
										data: vm.listaDatos,
										excessRows: vm.listaDatos.length
									}
								});
							});

							vm.listaEntidadesGeneral.push({
								sector: nombreSector,
								entidad: nombreEntidad,
								tipoEntidad: tipoEntidad,
								idSector: idSector,
								entidadId: idEntidad,
								estadoEntidad: "+",
								subGridOptions: {
									data: vm.listaTramitesCompleta,
									excessRows: vm.listaTramitesCompleta.length,
								}
							});

							vm.listaEntidades.push({
								sector: nombreSector,
								entidad: nombreEntidad,
								tipoEntidad: tipoEntidad,
								idSector: idSector,
								entidadId: idEntidad,
								estadoEntidad: "+",
								subGridOptions: {
									columnDefs: vm.columnDefTramite,
									appScopeProvider: $scope,
									expandableRowTemplate: vm.plantillaFilaTramites,
									expandableRowScope: {
										subGridVariable: 'subGridScopeVariable'
									},
									enableFiltering: false,
									showHeader: false,
									data: vm.listaTramites,
									excessRows: vm.listaTramites.length,
									onRegisterApi: onRegisterApi2
								}
							});
						});

						configurarColumnas();
					}

					vm.gridOptions.data = vm.listaEntidades;
					var entidades = vm.listaEntidadesGeneral;

					vm.filasFiltradas = vm.gridOptions.data.length > 0;
					vm.Mensaje = respuesta.data.Mensaje;
				},
				function (error) {
					if (error) {
						if (error.status) {
							switch (error.status) {
								case 401:
									vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
									break;
								case 500:
									vm.Mensaje = $filter('language')('ErrorObtenerDatos');
								default:
									vm.Mensaje = error.statusText;
									break;
							}
						}
					}
					mostrarMensajeRespuesta(idObjeto);
				}
			);
		}

		function conmutadorFiltro() {
			limpiarCamposFiltro();
			vm.mostrarFiltro = !vm.mostrarFiltro;
			var idSpanArrow = 'arrow-IdPanelBuscador';
			var arrowCapitulo = document.getElementById(idSpanArrow);
			var arrowClasses = arrowCapitulo.classList;
			for (var i = 0; i < arrowClasses.length; i++) {
				if (arrowClasses[i] == vm.arrowIcoDown2) {
					document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
					document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
					document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
					break;
				} else if (arrowClasses[i] == vm.arrowIcoUp2) {
					document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
					document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
					document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
					break;
				}
			}
		}

		// function contarCantidadObjetosNegocio(idObjeto) {

		//     var gruposEntidades = null;

		//     // ReSharper disable once UndeclaredGlobalVariableUsing
		//     if (idObjeto === vm.idTipoProyecto) {
		//         gruposEntidades = vm.gruposEntidadesProyectos;
		//     } else {
		//         gruposEntidades = vm.gruposEntidadesTramites;
		//     }

		//     var cantidadDeObjetosDeNegocio = 0;

		//     // ReSharper disable once UndeclaredGlobalVariableUsing
		//     angular.forEach(gruposEntidades, function (grupoEntidad) {
		//         // ReSharper disable once UndeclaredGlobalVariableUsing
		//         angular.forEach(grupoEntidad.ListaEntidades, function (entidad) {
		//             cantidadDeObjetosDeNegocio += entidad.ObjetosNegocio.length;
		//         });
		//     });

		//     // ReSharper disable once UndeclaredGlobalVariableUsing
		//     if (idObjeto === vm.idTipoProyecto) {
		//         vm.cantidadDeProyectos = cantidadDeObjetosDeNegocio;
		//     } else {
		//         vm.cantidadDeTramites = cantidadDeObjetosDeNegocio;
		//     }
		// }

		function mostrarMensajeRespuesta(idObjeto) {
			// ReSharper disable once UndeclaredGlobalVariableUsing
			if (idObjeto === vm.idTipoProyecto && vm.cantidadDeProyectos === 0) {
				vm.mostrarMensajeProyectos = true;
			} else {
				vm.mostrarMensajeProyectos = false;
			}

			// ReSharper disable once UndeclaredGlobalVariableUsing
			if (idObjeto === idTipoTramiteProgramacion && vm.cantidadDeTramites === 0) {
				vm.mostrarMensajeTramites = true;
			} else {
				vm.mostrarMensajeTramites = false;
			}
		}

		function conmutadorFiltro() {
			limpiarCamposFiltro();
			vm.mostrarFiltro = !vm.mostrarFiltro;
			var idSpanArrow = 'arrow-IdPanelBuscador';
			var arrowCapitulo = document.getElementById(idSpanArrow);
			var arrowClasses = arrowCapitulo.classList;
			for (var i = 0; i < arrowClasses.length; i++) {
				if (arrowClasses[i] == vm.arrowIcoDown2) {
					document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
					document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
					document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
					break;
				} else if (arrowClasses[i] == vm.arrowIcoUp2) {
					document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
					document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
					document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
					break;
				}
			}

		}

		// function conmutadorFiltroTramites() {
		//     limpiarCampos();
		//     vm.mostrarFiltroTramites = !vm.mostrarFiltroTramites;
		// }

		// function cambiarEstado(bpin, estado) {
		//     window.location.href = "/ejecutarAccion/" + bpin + "/" + estado;
		// }

		// function mostrarInformacionDePropiedad(propiedad) {
		//     var debeMostrar = false;
		//     // ReSharper disable once UndeclaredGlobalVariableUsing
		//     angular.forEach(vm.columnas, function (columna) {
		//         if (columna === propiedad) {
		//             debeMostrar = true;
		//         }
		//     });
		//     return debeMostrar;
		// }

		// function mostrarModal() {
		//     vm.mostrarCrearEditarLista = true;
		// };

		function obtenerInbox(idObjeto) {
			limpiarCamposFiltro();

			return cargarEntidades(idObjeto).then(function () {

				let tipoEntidad;
				for (let i = 0; i < vm.listaTipoEntidad.length; i++) {
					tipoEntidad = vm.listaTipoEntidad[i];

					if (!tipoEntidad.Deshabilitado) {
						vm.tipoEntidad = tipoEntidad.Nombre;
						cambioTipoEntidad(vm.tipoEntidad);
						break;
					}
				}
				listarFiltroSectores();
				listarFiltroEntidades();
				listarFiltroEntidadesCrearTramite();
				listarFiltroEstadoTramites();
				listarFiltroTipoTramites();
				mostrarMensajeRespuesta(idObjeto);
			});
		};

		// function seleccionarTipoObjeto(idObjeto) {

		//     var tipoObjeto = {
		//         Id: idObjeto
		//     }
		//     var roles = sesionServicios.obtenerUsuarioIdsRoles();
		//     if (roles != undefined && roles.length > 0) {
		//         estadoAplicacionServicios.tipoObjetoSeleccionado = tipoObjeto;
		//         obtenerInbox(idObjeto);
		//     }
		//     else {
		//         vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
		//         mostrarMensajeRespuesta(idObjeto);
		//     }
		// }

		// function limpiarCampos() {
		//     vm.busquedaNombreConId = "";
		// };

		function abrirModalAdicionarColumnas() {

			var modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
				controller: 'controladorConfigurarColumnas',
				controllerAs: "vm",
				size: 'lg',
				resolve: {
					items: function () {
						return {
							'columnasActivas': vm.columnas,
							'columnasDisponibles': vm.columnasDisponiblesPorAgregar,
							'tipoFiltro': vm.tipoFiltro
						};
					}
				}
			});

			modalInstance.result.then(function (selectedItem) {

				if (!$localStorage.tipoFiltro) {
					$localStorage.tipoFiltro = {
						'tramites': {
							'columnasActivas': selectedItem.columnasActivas,
							'columnasDisponibles': selectedItem.columnasDisponibles
						}
					};
				} else {
					$localStorage.tipoFiltro['tramites'] = {
						'columnasActivas': selectedItem.columnasActivas,
						'columnasDisponibles': selectedItem.columnasDisponibles
					}
				}
				buscarColumnasLocalStorage();
				configurarColumnas();
			}, function () {
				$log.info('Modal dismissed at: ' + new Date());
			});
		};

		function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad, nombreFlujo, row) {

			$sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
			$sessionStorage.nombreProyecto = nombreProyecto;
			$sessionStorage.idObjetoNegocio = idObjetoNegocio;
			$sessionStorage.idEntidad = idEntidad;
			if (nombreFlujo !== null) {
				$sessionStorage.nombreFlujo = nombreFlujo;
				$sessionStorage.etapa = vm.etapa;
				$sessionStorage.InstanciaSeleccionada = row;
			}
			$timeout(function () {
				$location.path('/Acciones/ConsultarAcciones');
			}, 300);



		}

		function mostrarLog(row) {
			$uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/panelPrincial/modales/logs/logsModal.html',
				controller: 'logsModalController',
				controllerAs: "vm",
				size: 'lg',
				openedClass: "entidad-modal-adherencia",
				resolve: {
					IdInstancia: () => row.IdInstancia
				},
			});
		}


		// function filtrarPorNombreObjetoNegocio(objetoNegocio) {

		//     var nombreConId = objetoNegocio.NombreObjetoNegocio + " - " + objetoNegocio.IdObjetoNegocio;

		//     if (!vm.busquedaNombreConId || (nombreConId.toLowerCase().indexOf(vm.busquedaNombreConId) !== -1) || (nombreConId.toLowerCase().indexOf(vm.busquedaNombreConId.toLowerCase()) !== -1)) {
		//         return true;
		//     }
		//     return false;

		// };

		// function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {
		//     $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
		//     $sessionStorage.nombreProyecto = nombreProyecto;
		//     $sessionStorage.idObjetoNegocio = idObjetoNegocio;
		//     $sessionStorage.idEntidad = idEntidad;
		//     $timeout(function () {
		//         $location.path('/Acciones/ConsultarAcciones');
		//     }, 300);
		// }

		// function escucharEventos() {
		//     $scope.$on(constantesBackbone.eventoInstanciaCreada, cuandoSeCreaUnaInstancia);

		//     ////////////

		//     function cuandoSeCreaUnaInstancia(evento, idTipoObjeto) {
		//         obtenerInbox(idTipoObjeto);
		//     }
		// }        

		function configurarNombreEtapa() {
			switch (vm.etapa) {
				case 'pl':
					vm.nombreEtapa = 'Planeación';
					break;
				case 'pr':
					vm.nombreEtapa = 'Programación';
					break;
				case 'ej':
					vm.nombreEtapa = 'Ejecución';
					break;
				case 'se':
					vm.nombreEtapa = 'Seguimiento';
					break;
				case 'ev':
					vm.nombreEtapa = 'Evolución';
					break;
			}

		}

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
					idEtapa = [constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
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

		//#region Componente Crear Tramites

		function listarFiltroEntidadesCrearTramite() {
			autorizacionServicios.obtenerListaEntidad(usuarioDNP, idTipoTramiteProgramacion).then(exito, error);

			function exito(respuesta) {

				var listaAuxEntidades = [];
				let entidadesTodas = respuesta.filter(p => p.TipoEntidad == vm.tipoEntidad);
				entidadesTodas.forEach(element => {
					listaAuxEntidades.push({
						id: element.EntityTypeCatalogOptionId,
						nombre: element.Entidad
					})
				});

				listaAuxEntidades.sort((a, b) => {
					let aSinAcento = a.nombre.toLowerCase().replace("á", "a").replace("é", "e").replace("í", "i").replace("ó", "o").replace("ú", "u");
					let bSinAcento = b.nombre.toLowerCase().replace("á", "a").replace("é", "e").replace("í", "i").replace("ó", "o").replace("ú", "u");
					if (aSinAcento < bSinAcento) {
						return -1;
					}
					if (aSinAcento > bSinAcento) {
						return 1;
					}
					return 0;
				})
				vm.listaEntidades = listaAuxEntidades;
			}

			function error() {
				vm.listaEntidades = [];
			}
		}

		function todosLosFlujos() {
			servicioPanelPrincipal.ObtenerFlujosPorTipoObjeto(idTipoTramiteProgramacion).then(exito, error);

			function exito(respuesta) {
				vm.listaFiltroFlujos = respuesta.data || [];
				vm.cantidadFlujosProgramacion = vm.listaFiltroFlujos.length;
				if (vm.cantidadFlujosProgramacion > 0) {
					vm.classProcesos = "conProgramacion";
				}
			}

			function error() {
				vm.listaFiltroFlujos = [];
			}
		}

		function cargarFlujosTramites() {
			todosLosFlujos();
			return flujoServicios.obtenerFlujosPorRoles().then(
				function (flujos) {
					flujos = flujos.filter(filtrarFlujosPorTipoObjetoSeleccionado);
					var listaAuxFlujos = [];

					flujos.forEach(element => {
						if (element.PadreTramiteId == null) {
							listaAuxFlujos.push({
								id: element.IdOpcionDnp,
								nombre: element.NombreOpcion,
								alias: element.AliasTipoTramite
							})
						}
					});
					vm.flujos = flujos;
					vm.tramites = listaAuxFlujos;
					
				},
				function (error) {
					if (error) {
						if (error.status) {
							switch (error.status) {
								case 401:
									utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
									break;
								case 500:
									utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
									break;
								case 404:
									return [];
							}
							return;
						}
					}
				}
			);
			////////////
			function filtrarFlujosPorTipoObjetoSeleccionado(flujo) {
				return flujo.TipoObjeto && flujo.TipoObjeto.Id === idTipoTramiteProgramacion && flujo.ActivoTipoTramite === true;
			};
		}

		function removerMensajeError() {
			document.getElementById("tipoTramiteError").innerHTML = "";
			document.getElementById("tipoTramiteError").classList.add('hidden');
			document.getElementById("codEntidadeError").innerHTML = "";
			document.getElementById("codEntidadeError").classList.add('hidden');
			document.getElementById("objetoErrorVFE").innerHTML = "";
			document.getElementById("objetoErrorVFE").classList.add('hidden');
			document.getElementById("objetoErrorVFO").innerHTML = "";
			document.getElementById("objetoErrorVFO").classList.add('hidden');
			document.getElementById("descripcionError").innerHTML = "";
			document.getElementById("descripcionError").classList.add('hidden');
			document.getElementById("anioFinalErrorVFE").innerHTML = "";
			document.getElementById("anioFinalErrorVFE").classList.add('hidden');
			document.getElementById("anioFinalErrorVFO").innerHTML = "";
			document.getElementById("anioFinalErrorVFO").classList.add('hidden');
			document.getElementById("tipoProcesoError").innerHTML = "";
			document.getElementById("tipoProcesoError").classList.add('hidden');
			document.getElementById("appErrorVFE").innerHTML = "";
			document.getElementById("appErrorVFE").classList.add('hidden');
			document.getElementById("appErrorVFO").innerHTML = "";
			document.getElementById("appErrorVFO").classList.add('hidden');
		}

		function validacionNovigenciaFutura() {
			if (!vm.objSolicitud.codTramite) {
				document.getElementById("tipoTramiteError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Seleccione una opción.</span>";
				document.getElementById("tipoTramiteError").classList.remove('hidden');
			}
			if (!vm.objSolicitud.codEntidade) {
				document.getElementById("codEntidadeError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Seleccione una opción.</span>";
				document.getElementById("codEntidadeError").classList.remove('hidden');
			}
			if (!vm.objSolicitud.descripcion) {
				document.getElementById("descripcionError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Ingrese la descripción.</span>";
				document.getElementById("descripcionError").classList.remove('hidden');
			}
			utilidades.mensajeError('Revise los campos señalados y valide nuevamente', false, 'Hay datos que presentan inconsistencias');
		}

		function guardarSolicitud() {

			var descripcion = vm.objSolicitud.descripcion;

			var data = vm.tramites.find(t => t.id === vm.objSolicitud.codTramite);
			if (!vm.objSolicitud.codTramite || !vm.objSolicitud.codEntidade || !vm.objSolicitud.descripcion) {
				if (!vm.objSolicitud.codTramite) {
					document.getElementById("tipoTramiteError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Seleccione una opción.</span>";
					document.getElementById("tipoTramiteError").classList.remove('hidden');
				}
				if (!vm.objSolicitud.codEntidade) {
					document.getElementById("codEntidadeError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Seleccione una opción.</span>";
					document.getElementById("codEntidadeError").classList.remove('hidden');
				}
				if (!vm.objSolicitud.descripcion) {
					document.getElementById("descripcionError").innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>  Ingrese la descripción.</span>";
					document.getElementById("descripcionError").classList.remove('hidden');
				}
				utilidades.mensajeError('Revise los campos señalados y valide nuevamente', false, 'Hay datos que presentan inconsistencias');
				return;
			}
			return flujoServicios.ExisteFlujoProgramacion(vm.objSolicitud.codEntidade, vm.objSolicitud.codTramite).then(
				function (existe) {
					if (existe.data) {
						utilidades.mensajeError('No se creó instancia, porque la entidad ya tiene una instancia completada en la vigencia actual');
					}
					else {
						var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

						var instanciaDto = {
							FlujoId: vm.objSolicitud.codTramite,
							ObjetoId: vm.objSolicitud.codEntidade,
							UsuarioId: usuarioDNP,
							RolId: usuarioRolId,
							TipoObjetoId: idTipoTramiteProgramacion,
							ListaEntidades: [vm.objSolicitud.codEntidade],
							Descripcion: descripcion,
							TipoProceso: vm.tipoProceso,
							App: vm.app,
							Macroproceso: vm.etapa.toUpperCase(),
							anioInicio: vm.objSolicitud.anioInicio,
							FechaFinal: vm.objSolicitud.anioFinal,
							FechaInicial: vm.objSolicitud.anioInicial
						}

						flujoServicios.crearInstancia(instanciaDto).then(
							function (resultado) {
								if (!resultado.length) {
									utilidades.mensajeError('No se creó instancia');
									return;
								}
								vm.cerrarTramite();
								utilidades.mensajeSuccess('Se crearon instancias exitosamente.<br/>Proceso No. ' + resultado[0].NumeroTramite, false, cargueInicial, '', 'El proceso fue creado con éxito.');
								//$("#tramits").scope().vm.obtenerInbox(idTipoTramiteProgramacion);
								//$("#cantidadTramites").scope().vm.obtenerNotificacionesCantidadDeTramites();
								//$("#tramitesCantidad").scope().vm.obtenerNotificacionesCantidadDeTramites();
								//cargueInicial();
							},
							function (error) {
								if (error) {
									utilidades.mensajeError(error);
								}
							}
						);
					}
				},
				function (error) {
					if (error) {
						if (error.status) {
							switch (error.status) {
								case 500:
									utilidades.mensajeError("Ocurrio un error validando la existencia de flujos para la vigencia");
									break;
								case 404:
									return [];
							}
							return;
						}
					}
				}
			);



		}

		function cerrarTramite() {
			vm.objSolicitud = {
				codTramite: null,
				codEntidade: null,
				descripcion: null
			};
			vm.popOverOptionsCrearTramite.toggle();
		}

		//#endregion
	}

	angular.module('backbone')
		.component('programacionMisProcesos', {
			templateUrl: "/src/app/panelPrincial/componentes/inbox/programacion.template.html",
			controller: 'programacionMPController',
			controllerAs: 'vm'
		});

})();