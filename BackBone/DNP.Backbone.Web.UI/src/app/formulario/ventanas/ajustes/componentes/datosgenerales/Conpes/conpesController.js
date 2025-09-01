(function () {
	'use strict';

	conpesController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'conpesServicio',
		'utilidades',
		'justificacionCambiosServicio'
	];



	function conpesController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		conpesServicio,
		utilidades,
		justificacionCambiosServicio
	) {
		var vm = this;
		vm.lang = "es";
		vm.conpes = "";
		vm.nombreComponente = "datosgeneralesrelacionconlapl";
		vm.guiMacroproceso = '';
		vm.encontroconpes = false;
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.gridOptions;
		vm.gridOptionsConpes;
		vm.result = "";
		vm.resultConpes = '';
		vm.busquedaConpes = '';
		vm.justificacionConpes = '';
		vm.justificacionAnterior = '';
		vm.consultoConpes = false;
		vm.renderizado = false;
		vm.cantresult = 0;
		vm.conpesAsociados = 0;
		vm.arrayAdicionarConpes = [];
		vm.dataCONPES;
		vm.dataCONPESAsociados = [];
		vm.BanderaMensajeSucces = false;
		vm.habilitarFinal = false;
		vm.habilitaGuardar = false;
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
		vm.habilitarBotones = habilitarBotones;
		vm.habilitarJustificacion = false;
		vm.ClasesinputIni = " ";

		vm.proyectoconpes = [{
			Id: 0,
			ProyectoId: 0,
			ConpesId: 0,
			NumeroConpes: "",
			NombreConpes: "",
			FechaAprobacion: "",
			CreadoPor: "",
			ModificadoPor: "",
		}];

		vm.DocumentoCONPES = [{
			id: 0,
			titulo: "",
			numeroCONPES: "",
			fechaAprobacion: "",
			tipoCONPES: "",
			asociado: 0
		}];

		vm.DocumentoCONPESAsociados = [{
			id: 0,
			titulo: "",
			numeroCONPES: "",
			fechaAprobacion: "",
			tipoCONPES: "",
			asociado: 0
		}];

		function onRegisterApi(gridApi) {
			$scope.gridApi = gridApi;
		}

		function onRegisterApiConpes(gridApi) {
			$scope.gridApi = gridApi;
		}

		function habilitarBotones() {
			if (vm.habilitarFinal) {
				vm.habilitarFinal = false;
				vm.habilitaGuardar = false;
				vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
				vm.habilitarJustificacion = false;
				document.getElementById("justificacionConpes").value = vm.justificacionAnterior;
			}
			else {
				vm.habilitarFinal = true;
				vm.habilitaGuardar = true;
				vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
				vm.habilitarJustificacion = true;

			}
		}


		//Inicio
		vm.init = function () {

			if (!vm.gridOptions) {
				vm.gridOptions = {
					columnDefs: vm.columnDef,
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

			if (!vm.gridOptionsConpes) {
				vm.gridOptionsConpes = {
					columnDefs: vm.columnDefConpes,
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
					onRegisterApi: onRegisterApiConpes
				};

			}

			vm.result = "Aún no hay documentos asociados";
			vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.CargarConpes();

		};

		vm.guardarConpes = function () {

			if (!$sessionStorage.edicionConpes) {
				utilidades.mensajeError('No se pueden realizar modificaciones, presiona el botón habilitar edición.');
				return false;
			}
			var seccionCapituloId = document.getElementById("id-capitulo-" + vm.nombreComponente);
			var data = {
				Conpes: vm.dataCONPESAsociados,
				ProyectoId: vm.idProyecto,
				Justificacion: document.getElementById("justificacionConpes").value,
				SeccionCapituloId: seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0,
				InstanciaId: vm.idInstancia,
				GuiMacroproceso: vm.guiMacroproceso
			}
			conpesServicio.AdicionarConpes(data)
				.then(function (response) {
					if (response.data.Exito) {
						if (vm.BanderaMensajeSucces == true) {
							vm.BanderaMensajeSucces = false;
							vm.justificacionAnterior = document.getElementById("justificacionConpes").value;
							utilidades.mensajeSuccess('Se han adicionado líneas de información en la parte inferior de la tabla "Documentos Conpes".Recuerde justificar la asociación en el campo "Justificación".', false, false, false, "Los datos fueron agregados y guardados con éxito.");
						}
						else {
							vm.habilitarFinal = false;
							vm.habilitaGuardar = false;
							vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
							vm.habilitarJustificacion = false;
							utilidades.mensajeSuccess("", false, false, false, 'Los datos fueron guardados con éxito.');

						}

						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
					}
					else {
						utilidades.mensajeError(response.data.Mensaje);
					}
				});
		}

		vm.getTableHeightCDPGR = function () {
			var rowHeight = 30;
			var headerHeight = 50;
			if (!vm.renderizado) {
				$(window).resize();
			}

			return {
				height: (((vm.dataCONPESAsociados.length + 1) * rowHeight + headerHeight) + 100) + "px"
			};
		}

		vm.CargarConpes = function () {
			conpesServicio.CargarConpes(vm.idProyecto, vm.idInstancia, vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe)
				.then(resultado => {
					if (resultado.statusText === 'OK' || resultado.status === 200) {
						vm.resultConpes = '';
						vm.consultoConpes = false;
						vm.dataCONPESAsociados = resultado.data.Conpes;
						document.getElementById("justificacionConpes").value = resultado.data.Justificacion;
						vm.justificacionAnterior = resultado.data.Justificacion;
						vm.DocumentoCONPESAsociados.push({

							subGridOptions: {
								columnDefs: vm.columnDefConpes,
								enableVerticalScrollbar: 1,
								appScopeProvider: $scope,
								paginationPageSizes: [5, 10, 15, 25, 50, 100],
								paginationPageSize: 5,
								excessRows: vm.dataCONPESAsociados.length,
								data: vm.dataCONPESAsociados,

							}
						});
						if (vm.DocumentoCONPESAsociados.length > 1) {
							vm.result = "Documentos Conpes Coincidentes ";
							vm.cantresult = "Total: " + (vm.dataCONPESAsociados.length);
						}
						else {
							vm.result = "Aún no hay documentos asociados";
							vm.cantresult = "Total: " + 0;
						}

						vm.gridOptionsConpes.showHeader = true;
						vm.gridOptionsConpes.columnDefs = vm.columnDefConpes;
						vm.gridOptionsConpes.data = vm.dataCONPESAsociados;
						vm.filasFiltradas = vm.gridOptionsConpes.data.length > 0;
						vm.arrayAdicionarConpes = [];

					}

				})
		}

		vm.CheckConpesAsociadosTemplate = '<button class="btnaccion" ng-click="grid.appScope.vm.eliminarConpes(row.entity.id)" tooltip-placement="Auto" uib-tooltip="Eliminar" ng-show="row.entity.seleccionado != 1">' +
			'    <span style="cursor:pointer"> <img src="Img/u928.svg"></span>' +
			'</button>' +
			'<button class="btn"  tooltip-placement="Auto" uib-tooltip="Eliminar" ng-disabled="row.entity.seleccionado == 1" ng-show="row.entity.seleccionado == 1">' +
			'    <span > <img src="Img/btnElimdisabled.svg"></span>' +
			'</button>';

		vm.CheckConpesTemplate = '<input type="checkbox" id="{{row.entity.id}}" ng-click="grid.appScope.vm.adicionarConpes(row.entity.id)" ' +
			'ng-show="row.entity.seleccionado == 0"> ';

		vm.TituloConpesTemplate = '<div class="row text-justify" style="padding:15px; text-align: justify;vertical-align: middle;float: left;"><label>{{row.entity.titulo}}</label></div >';

		vm.ConpesTemplete = '<div class="row">' +
			'<div class="col-md-12" >' +
			'<div class="row text-justify" style="padding:15px; text-align: justify;vertical-align: middle;float: left;"><label>{{ row.entity.titulo }}</label></div>' +
			'</div >' +
			'</div>';

		vm.columnDef = [
			{
				field: 'ColumnaVacia',
				displayName: '',
				enableHiding: false,
				enableColumnMenu: false,
				width: '4%',
				pinnedRight: false,
				cellClass: 'text-center'
			},
			{
				field: 'numeroCONPES',
				headerCellClass: 'TituloHeaderAling',
				displayName: 'Número',
				enableHiding: false,
				enableColumnMenu: false,
				width: '12%',
				pinnedRight: false,
				cellClass: 'text-right'
			},
			{
				field: 'titulo',
				displayName: 'Documento Conpes',
				enableHiding: false,
				enableColumnMenu: false,
				width: '80%',
				pinnedRight: false,
				cellTemplate: vm.ConpesTemplete
			},
			{
				field: 'accion',
				displayName: '',
				enableHiding: false,
				enableColumnMenu: false,
				width: '4%',
				pinnedRight: false,
				cellTemplate: vm.CheckConpesTemplate
			}
		];

		vm.columnDefConpes = [
			{
				field: 'ColumnaVacia',
				displayName: '',
				enableHiding: false,
				enableColumnMenu: false,
				width: '4%',
				pinnedRight: false,
				cellClass: 'text-center'

			},
			{
				field: 'numeroCONPES',
				headerCellClass: 'TituloHeaderAling',
				displayName: 'Número',
				enableHiding: false,
				enableColumnMenu: false,
				width: '12%',
				pinnedRight: false,
				cellClass: 'text-right'

			},
			{
				field: 'titulo',
				displayName: 'Documento Conpes',
				enableHiding: false,
				enableColumnMenu: false,
				width: '80%',
				pinnedRight: false,
				cellTemplate: vm.ConpesTemplete
			},
			{
				field: 'accion',
				displayName: '',
				enableHiding: false,
				enableColumnMenu: false,
				width: '4%',
				pinnedRight: false,
				cellTemplate: vm.CheckConpesAsociadosTemplate
			}
		];

		vm.limpiar = function (response) {
			vm.renderizado = true;
			$('input[type=checkbox]').prop('checked', false);
			vm.conpes = "";
			vm.resultConpes = '';
			vm.consultoConpes = false;
			vm.encontroconpes = false;

		}
		vm.limpiartxt = function (response) {
			vm.renderizado = true;
			vm.conpes = "";
		}

		vm.buscar = function (response) {
			if (!$sessionStorage.edicionConpes) {
				utilidades.mensajeError('No se pueden realizar modificaciones, presiona el botón habilitar edición.');
				return false;
			}
			vm.renderizado = true;
			vm.busquedaConpes = vm.conpes;
			$('input[type=checkbox]').prop('checked', false);
			vm.arrayAdicionarConpes = [];
			if (validarCampos()) {
				vm.consultoConpes = false;
				conpesServicio.BuscarConpes(vm.conpes)
					.then(function (response) {
						vm.resultConpes = '';
						if (response.data) {
							vm.dataCONPES = null;
							if (response.data.mensaje == "OK") {
								response.data.documentosCONPES = response.data.documentosCONPES.filter(function (obj) {
									let objPpal = vm.dataCONPESAsociados.find(o => o.id === obj.id);
									if (objPpal != undefined) {
										obj.seleccionado = 1;
									}
									return obj;
								});
								vm.dataCONPES = response.data.documentosCONPES;
								vm.DocumentoCONPES.push({

									subGridOptions: {
										columnDefs: vm.columnDef,
										enableVerticalScrollbar: 1,
										appScopeProvider: $scope,
										paginationPageSizes: [5, 10, 15, 25, 50, 100],
										paginationPageSize: 5,
										data: response.data.documentosCONPES


									}
								});

								if (vm.DocumentoCONPES.length > 1) {
									vm.encontroconpes = true;
									vm.resultConpes = response.data.documentosCONPES.length;
								}
								else {
									vm.consultoConpes = true;
									vm.encontroconpes = false;
								}
								vm.gridOptions.showHeader = true;
								vm.gridOptions.columnDefs = vm.columnDef;
								vm.gridOptions.data = response.data.documentosCONPES;
								vm.gridOptions.excessRows = response.data.documentosCONPES.length;
								vm.filasFiltradas = vm.gridOptions.data.length > 0;
							}
							else {
								vm.encontroconpes = false;
								vm.limpiar();
								vm.consultoConpes = true;
							}
						} else {
							vm.consultoConpes = true;
							swal('', "Error al realizar la operación", 'error');
							vm.encontroconpes = false;
						}
					});
			}
			else {
				utilidades.mensajeError('Debe buscar por número o nombre del Conpes.');
			}
		}

		vm.adicionarConpes = function (idConpes) {
			vm.resultConpes = '';
			vm.consultoConpes = false;
			if ($('#' + idConpes).prop('checked')) {
				let objPpal = vm.arrayAdicionarConpes.find(o => o.id === idConpes);
				if (objPpal == undefined)
					vm.arrayAdicionarConpes.push({ id: idConpes, titulo: '', numeroCONPES: '', fechaAprobacion: '' });
			}
			else {
				vm.arrayAdicionarConpes = vm.arrayAdicionarConpes.filter(function (obj) {
					return obj.id !== idConpes;
				});
			}
		}

		vm.agregarConpes = function () {
			if (!$sessionStorage.edicionConpes) {
				utilidades.mensajeError('No se pueden realizar modificaciones, presiona el botón habilitar edición.');
				return false;
			}
			if (vm.arrayAdicionarConpes.length == 0) {
				utilidades.mensajeError('Debe seleccionar al menos un documento.');
				return false;
			}

			vm.renderizado = true;
			vm.arrayAdicionarConpes = vm.arrayAdicionarConpes.filter(function (obj) {
				let objPpal = vm.dataCONPES.find(o => o.id === obj.id);
				vm.dataCONPESAsociados.push(objPpal);
				return obj;
			});
			vm.DocumentoCONPESAsociados.push({

				subGridOptions: {
					columnDefs: vm.columnDefConpes,
					enableVerticalScrollbar: 1,
					appScopeProvider: $scope,
					paginationPageSizes: [5, 10, 15, 25, 50, 100],
					paginationPageSize: 5,

					data: vm.dataCONPESAsociados,

				}
			});
			if (vm.DocumentoCONPESAsociados.length > 1) {
				vm.result = "Documentos Conpes Coincidentes ";
				vm.cantresult = "Total: " + (vm.dataCONPESAsociados.length);
			}
			else {
				vm.result = "Aún no hay documentos asociados";
				vm.cantresult = "Total: " + 0;
			}

			vm.gridOptionsConpes.showHeader = true;
			vm.gridOptionsConpes.columnDefs = vm.columnDefConpes;
			vm.gridOptionsConpes.data = vm.dataCONPESAsociados;
			vm.filasFiltradas = vm.gridOptionsConpes.data.length > 0;
			vm.arrayAdicionarConpes = [];

			vm.dataCONPES = vm.dataCONPES.filter(function (obj) {
				let objPpal = vm.dataCONPESAsociados.find(o => o.id === obj.id);
				if (objPpal != undefined) {
					obj.seleccionado = 2;
				}
				return obj;
			});

			vm.DocumentoCONPES.push({

				subGridOptions: {
					columnDefs: vm.columnDef,
					enableVerticalScrollbar: 1,
					appScopeProvider: $scope,
					paginationPageSizes: [5, 10, 15, 25, 50, 100],
					paginationPageSize: 5,
					data: vm.dataCONPES,

				}
			});

			if (vm.DocumentoCONPES.length > 1) {
				vm.encontroconpes = true;
				vm.resultConpes = vm.dataCONPES.length;
			}
			else {
				vm.consultoConpes = true;
				vm.encontroconpes = false;
			}
			vm.gridOptions.showHeader = true;
			vm.gridOptions.columnDefs = vm.columnDef;
			vm.gridOptions.data = vm.dataCONPES;
			vm.filasFiltradas = vm.gridOptions.data.length > 0;

			vm.BanderaMensajeSucces = true;
			vm.encontroconpes = false;
			vm.resultConpes = '';
			vm.consultoConpes = false;
			vm.guardarConpes();
		}

		vm.eliminarConpes = function (conpesId) {
			if (!$sessionStorage.edicionConpes) {
				utilidades.mensajeError('No se pueden realizar modificaciones, presiona el botón habilitar edición.');
				return false;
			}
			vm.renderizado = true;
			vm.dataCONPESAsociados = vm.dataCONPESAsociados.filter(function (obj) {
				return obj.id !== conpesId;
			});
			vm.DocumentoCONPESAsociados.push({

				subGridOptions: {
					columnDefs: vm.columnDefConpes,
					enableVerticalScrollbar: 1,
					appScopeProvider: $scope,
					paginationPageSizes: [5, 10, 15, 25, 50, 100],
					paginationPageSize: 5,

					data: vm.dataCONPESAsociados,

				}
			});
			if (vm.DocumentoCONPESAsociados.length > 1) {
				vm.result = "Documentos Conpes Coincidentes ";
				vm.cantresult = "Total: " + (vm.dataCONPESAsociados.length);
			}
			else {
				vm.result = "Aún no hay documentos asociados";
				vm.cantresult = "Total: " + 0;
			}

			vm.gridOptionsConpes.showHeader = true;
			vm.gridOptionsConpes.columnDefs = vm.columnDefConpes;
			vm.gridOptionsConpes.data = vm.dataCONPESAsociados;
			vm.filasFiltradas = vm.gridOptionsConpes.data.length > 0;
			vm.arrayAdicionarConpes = [];

			if (vm.dataCONPES == null || vm.dataCONPES == undefined || vm.dataCONPES.length == 0) {
				vm.guardarConpes();
				return false;
			}

			vm.dataCONPES = vm.dataCONPES.filter(function (obj) {
				if (obj.id === conpesId) {
					var objDoom = document.getElementById(conpesId);
					if (objDoom != null) objDoom.checked = false;
					obj.seleccionado = 0
				}
				return obj;

			});

			vm.DocumentoCONPES.push({

				subGridOptions: {
					columnDefs: vm.columnDef,
					enableVerticalScrollbar: 1,
					appScopeProvider: $scope,
					paginationPageSizes: [5, 10, 15, 25, 50, 100],
					paginationPageSize: 5,
					data: vm.dataCONPES,

				}
			});

			if (vm.DocumentoCONPES.length > 1) {
				vm.encontroconpes = true;
				vm.resultConpes = vm.dataCONPES.length;
			}
			else {
				vm.consultoConpes = true;
				vm.encontroconpes = false;
			}
			vm.gridOptions.showHeader = true;
			vm.gridOptions.columnDefs = vm.columnDef;
			vm.gridOptions.data = vm.dataCONPES;
			vm.filasFiltradas = vm.gridOptions.data.length > 0;
			vm.guardarConpes();
		}

		function validarCampos() {
			if (vm.conpes == null || vm.conpes == '') {
				return false;
			}
			return true
		}

		vm.limpiarErrores = function () {
			var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-justificacionconpes-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}
			var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-proyectosconpes-error");
			if (campoObligatorioProyectos != undefined) {
				campoObligatorioProyectos.innerHTML = "";
				campoObligatorioProyectos.classList.add('hidden');
			}
		}

		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Conpes");
			vm.limpiarErrores();
			if (errores != undefined) {
				var isValid = true;
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
					isValid = (erroresJson == null || erroresJson.length == 0);
				}
				if (!isValid) {
					erroresJson[vm.nombreComponente].forEach(p => {
						if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
					});
				}
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
			}
		}

		vm.validarExistenciaConpes = function (errores) {
			var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-justificacionconpes-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}

		vm.validarExistenciaJustificacion = function (errores) {
			var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyectosconpes-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}

		vm.errores = {
			'REL001': vm.validarExistenciaConpes,
			'REL002': vm.validarExistenciaJustificacion
		}

	}

	angular.module('backbone').component('conpes', {

		templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/conpes/conpes.html",
		controller: conpesController,
		controllerAs: "vm",
		bindings: {
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&'
		}
	});

})();