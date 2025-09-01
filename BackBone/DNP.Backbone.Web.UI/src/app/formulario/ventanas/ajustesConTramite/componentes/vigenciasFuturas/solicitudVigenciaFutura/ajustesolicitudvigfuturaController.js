(function () {
	'use strict';

	ajustesolicitudvigfuturaController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'ajustesolicitudvigfuturaServicio',
		'utilidades',
		'justificacionCambiosServicio',
		'utilsValidacionSeccionCapitulosServicio',
		'$timeout',
	];



	function ajustesolicitudvigfuturaController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		ajustesolicitudvigfuturaServicio,
		utilidades,
		justificacionCambiosServicio,
		utilsValidacionSeccionCapitulosServicio,
		$timeout,

	) {

		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "ajustesvigenciasfuturasajusolvigfutura";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.isEdicion = false;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.idFlujo = $sessionStorage.idFlujoIframe;
		vm.idNivel = $sessionStorage.idNivel;
		vm.TramiteId = $sessionStorage.TramiteId;
		vm.cambiaTipoValor = cambiaTipoValor;
		vm.cambiaAnioBase = cambiaAnioBase;
		vm.isConstante = false;
		vm.isCorriente = false;
		vm.isAnio = 0;
		vm.valorOriginal = null;
		vm.anioOriginal = null;
		$scope.data = {
			model: null,
			availableOptions: [
				{ id: 1, name: 'Constantes' },
				{ id: 2, name: 'Corrientes' }
			]
		};
		vm.ObtenerDeflactores = ObtenerDeflactores;
		vm.abrirMensajeInformacionTipoValor = abrirMensajeInformacionTipoValor;
		vm.abrirMensajeInformacionAnioBase = abrirMensajeInformacionAnioBase;

		vm.componentesRefresh = [
			'recursoscostosdelasacti',
			'datosgeneraleshorizonte',
			'recursosfuentesdefinanc',
		];

		vm.handlerComponentes = [
			{ id: 1, componente: 'ajustesvigenciasfuturasvaloresconstantes', handlerValidacion: null, handlerCambios: null, esValido: true },
			{ id: 2, componente: 'ajustesvigenciasfuturasvalorescorrientes', handlerValidacion: null, handlerCambios: null, esValido: true }

		];

		//Inicio

		vm.parametros = {

			
		};

		vm.init = function () {
			CargarDatos();
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
		};


		function CargarDatos() {
			ObtenerDeflactores();
			ObtenerProyectoTramite();

        }
		function ObtenerDeflactores() {
			return ajustesolicitudvigfuturaServicio.ObtenerDeflactores().then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						$scope.datos = respuesta.data;
					}
				});
		}

		function ObtenerProyectoTramite() {
			return ajustesolicitudvigfuturaServicio.ObtenerProyectoTramite(vm.idProyecto, vm.TramiteId).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						//console.log(respuesta.data);
						$scope.datosProyecto = respuesta.data[0];
					}
					cargarCombosIniciales();
				});
		}

		function cargarCombosIniciales() {
			document.getElementById("vTipoValor").value = $scope.datosProyecto.Constante;
			document.getElementById("vAnioBase").value = $scope.datosProyecto.AnioBase;
			vm.valorOriginal = $scope.datosProyecto.Constante;
			vm.anioOriginal = $scope.datosProyecto.AnioBase;

			if ($scope.datosProyecto.Constante == 0) {
				vm.isConstante = false;
				vm.isCorriente = false;
				vm.isAnio = null;
			} else if ($scope.datosProyecto.Constante == 1) {
				vm.isConstante = true;
				vm.isCorriente = false;
				vm.isAnio = $scope.datosProyecto.AnioBase;
			} else {
				vm.isConstante = false;
				vm.isCorriente = true;
				vm.isAnio = null;
            }
		}

		vm.editar = function () {
			vm.isEdicion = true;
			vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
		}

		vm.cancelar = function () {
			vm.isEdicion = false;
			vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
			$scope.datosProyecto.Constante = vm.valorOriginal;
			$scope.datosProyecto.AnioBas = vm.anioOriginal;
			if (vm.valorOriginal == 0) {
				vm.isConstante = false;
				vm.isCorriente = false;
			} else if (vm.valorOriginal == 1) {
				vm.isConstante = true;
				vm.isCorriente = false;
			} else {
				vm.isConstante = false;
				vm.isCorriente = true;
			}
			
		}

		vm.guardar = function () {

			if ($scope.datosProyecto.Constante == '' || $scope.datosProyecto.Constante == undefined || $scope.datosProyecto.Constante == 0) {
				utilidades.mensajeError('Debe ingresar un tipo de Valor.');
				return false;
			}

			if ($scope.datosProyecto.Constante == 1) {

				if ($scope.datosProyecto.AnioBase == '' || $scope.datosProyecto.AnioBase == undefined) {
					utilidades.mensajeError('Debe ingresar un año base.');
					return false;
				}
			}

			var constanteDef = false;
			var anioBaseDef = null;
			if ($scope.datosProyecto.Constante == 1) {
				constanteDef = true;
				anioBaseDef = $scope.datosProyecto.AnioBase;
            }

			vm.valorOriginal = $scope.datosProyecto.Constante;
			vm.anioOriginal = anioBaseDef;

			var tramiteProyectoDto = {
				ProyectoId: vm.idProyecto,
				TramiteId: vm.TramiteId,
				EsConstante: constanteDef,
				AnioBase: anioBaseDef
			};

			ajustesolicitudvigfuturaServicio.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto).then(function (response) {
				if (response.data && (response.statusText === "OK" || response.status === 200)) {
					//guardarCapituloModificado();
					const mensaje2 = "Tipo de Valor guardado exitosamente!";
					new utilidades.mensajeSuccess(mensaje2, false, false, false);
					$scope.datosProyecto.Constante = document.getElementById("vTipoValor").value;
					$scope.datosProyecto.AnioBase = document.getElementById("vAnioBase").value;
					//vm.init();
				} else {
					new utilidades.mensajeError("Error al realizar la operación");
				}

			});

		}

		function cambiaTipoValor(tipoValor) {

			return ajustesolicitudvigfuturaServicio.ObtenerProyectoTramite(vm.idProyecto, vm.TramiteId).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						
						vm.valorOriginal = respuesta.data[0].Constante;
						vm.anioOriginal = respuesta.data[0].AnioBase;

						if (tipoValor == 0) {
							vm.isConstante = false;
							vm.isCorriente = false;
							vm.isAnio = 0;
							$scope.datosProyecto.Constante = vm.valorOriginal;
							$scope.datosProyecto.AnioBase = vm.anioOriginal;
						} else if (tipoValor == 1) {
							if (vm.valorOriginal == 2) { // cambio de valores corrientes a constantes.
								utilidades.mensajeWarning("Los datos que posiblemente haya diligenciado y guardado en Valores corrientes se perderán. ¿Está seguro de continuar?", function funcionContinuar() {

									$timeout(function () {
									vm.isConstante = true;
									vm.isCorriente = false;
									vm.isAnio = 0;
									var constanteDef = false;
									var anioBaseDef = null;
									if ($scope.datosProyecto.Constante == 1) {
										constanteDef = true;
										anioBaseDef = null;//$scope.datosProyecto.AnioBase;
										$scope.datosProyecto.AnioBase = null;
									}

									vm.valorOriginal = $scope.datosProyecto.Constante;
									vm.anioOriginal = anioBaseDef;

									var tramiteProyectoDto = {
										ProyectoId: vm.idProyecto,
										TramiteId: vm.TramiteId,
										EsConstante: constanteDef,
										AnioBase: anioBaseDef
									};

										ajustesolicitudvigfuturaServicio.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto).then(function (response) {
										if (response.data && (response.statusText === "OK" || response.status === 200)) {
											//guardarCapituloModificado();
											vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
											utilidades.mensajeSuccess("", false, false, false, "Los datos han sido cambiados a Valores constantes");
											$scope.datosProyecto.Constante = document.getElementById("vTipoValor").value;
											$scope.datosProyecto.AnioBase = document.getElementById("vAnioBase").value;
											//vm.init();
										} else {
											new utilidades.mensajeError("Error al realizar la operación");
										}
									});

									}, 200);
								}, function funcionCancelar(reason) {
									$scope.datosProyecto.Constante = vm.valorOriginal;
									$scope.datosProyecto.AnioBas = vm.anioOriginal;
									vm.isConstante = false;
								});
							} else {
								vm.isConstante = true;
								vm.isCorriente = false;
							}
						} else {
							if (vm.valorOriginal == 1) { // cambio de valores constantes a corrientes.
								utilidades.mensajeWarning("Los datos que posiblemente haya diligenciado y guardado en Valores constantes se perderán. ¿Está seguro de continuar?", function funcionContinuar() {

									$timeout(function () {
									vm.isConstante = false;
									vm.isCorriente = true;
									vm.isAnio = 0;
									var constanteDef = false;
									var anioBaseDef = null;
									if ($scope.datosProyecto.Constante == 1) {
										constanteDef = true;
										anioBaseDef = $scope.datosProyecto.AnioBase;
									}

									vm.valorOriginal = $scope.datosProyecto.Constante;
									vm.anioOriginal = anioBaseDef;

									var tramiteProyectoDto = {
										ProyectoId: vm.idProyecto,
										TramiteId: vm.TramiteId,
										EsConstante: constanteDef,
										AnioBase: anioBaseDef
									};

									 ajustesolicitudvigfuturaServicio.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto).then(function (response) {
										if (response.data && (response.statusText === "OK" || response.status === 200)) {
											//guardarCapituloModificado();
											vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
											utilidades.mensajeSuccess("", false, false, false, "Los datos han sido cambiados a Valores corrientes" );
											$scope.datosProyecto.Constante = document.getElementById("vTipoValor").value;
											$scope.datosProyecto.AnioBase = document.getElementById("vAnioBase").value;
											//vm.init();
										} else {
											new utilidades.mensajeError("Error al realizar la operación");
										}
									});
									}, 200);
								}, function funcionCancelar(reason) {
									$scope.datosProyecto.Constante = vm.valorOriginal;
									$scope.datosProyecto.AnioBas = vm.anioOriginal;
									vm.isConstante = true;
								});
							} else {
								vm.isConstante = false;
								vm.isCorriente = true;
								vm.isAnio = 0;
							}
						}
					}
				});			
		}
		

		function cambiaAnioBase(AnioBase) {
			vm.isAnio = AnioBase;
			vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente, data: AnioBase });
        }

		function abrirMensajeInformacionTipoValor() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Tipo Valor, </span><br /> <span class='tituhori' > Proyectos que se encuentran en Ejecución.</span>");
		}

		function abrirMensajeInformacionAnioBase() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Año Constante, </span><br /> <span class='tituhori' > Proyectos que se encuentran en Ejecución.</span>");
		}

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				CargarDatos();
				vm.handlerComponentes.map(item => {
					if (item.handler != null) item.handler();
				});
			}
		}

		vm.refresh = function (handler, nombreComponente) {
			vm.handlerComponentes.map(item => {
				if (item.componente == nombreComponente) item.handler = handler;
			});
		}

		vm.setErrores = function (handler, nombreComponente) {
			vm.handlerComponentes.map(item => {
				if (item.componente == nombreComponente) item.handlerValidacion = handler;
			});
		}

		vm.guardadoHijos = function (nombreComponenteHijo) {
			vm.guardadoevent({ nombreComponenteHijo: nombreComponenteHijo });
		}

		/* --------------------------------- Notificación de Validaciones ---------------------------*/

		/**
		 * Función que recibe listado de errores de su componente padre por medio del binding notificacionvalidacion
		 * @param {any} errores
		 */
		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Vigencias futuras fuentes");
			if (errores != undefined) {
				var listadoErrores = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
				var isValid = (listadoErrores != null && listadoErrores.erroresActivos != undefined && listadoErrores.erroresActivos != null && listadoErrores.erroresActivos.length <= 0)
				vm.handlerComponentes.map(item => {
					if (item.handlerValidacion != null) item.handlerValidacion(listadoErrores);
				});

				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
			}
		};
	}

	angular.module('backbone').component('ajustesolicitudvigfuturas', {

		templateUrl: "src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/solicitudVigenciaFutura/ajustesolicitudvigfutura.html",
		controller: ajustesolicitudvigfuturaController,
		controllerAs: "vm",
		bindings: {
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			notificacioncambios: '&'
		}
	});

})();