(function () {
	'use strict';

	horizonteController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'horizonteServicio',
		'utilidades',
		'justificacionCambiosServicio'
	];



	function horizonteController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		horizonteServicio,
		utilidades,
		justificacionCambiosServicio

	) {

		$sessionStorage.vigenciaHorizonte = '';
		var vm = this;
		vm.lang = "es";
		vm.actualizaresumen = 0;
		vm.nombreComponente = "datosgeneraleshorizonte";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.habilitaGuardar = false;
		vm.habilitaAlertaError = false;
		vm.AlertaMensajeError = "";
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
		vm.ClasesinputIni = " ";
		vm.ClasesinputFin = " ";
		vm.anioInicio;
		vm.anioFinal;
		vm.estadoProyecto;
		vm.mensaje1 = "";
		vm.mensaje2 = "";
		vm.habilitar = false;
		vm.habilitarFinal = false;
		vm.verBotones = false;
		vm.anioInicioOriginal;
		vm.anioFinalOriginal;
		vm.evaluarVerBotones = evaluarVerBotones;
		vm.habilitarVigencias = habilitarVigencias;
		vm.habilitarInicio = false;
		vm.Guardar = Guardar;
		vm.ObtenerHorizonte = ObtenerHorizonte;
		vm.obtenerCambiosFirme = obtenerCambiosFirme;
		vm.EsMover = false;
		vm.Usuario = usuarioDNP;
		vm.vigenciaActual;
		vm.añoFirme;
		vm.abrirMensajeInformacionHorizonte = abrirMensajeInformacionHorizonte
		vm.abrirMensajeInformacionHorizonte2 = abrirMensajeInformacionHorizonte2
		//Inicio

		vm.parametros = {

			//idInstancia: "13039628-3b7c-4f03-90d0-cc05af96ca65",
			//idFlujo: "CF1592AA-9087-3D77-B451-6F3557EF3F82",
			//idNivel: "efaa069d-918a-4b31-8d07-75cc1bee4366",
			//idProyecto: 6773,
			//idProyectoStr: "6773",
			//Bpin: vm.Bpin,
			//tramite: 26,

			idInstancia: $sessionStorage.idInstancia,
			idFlujo: $sessionStorage.idFlujoIframe,
			idNivel: $sessionStorage.idNivel,
			idProyecto: vm.idProyecto,
			idProyectoStr: $sessionStorage.idObjetoNegocio,
			Bpin: vm.Bpin

		};

		vm.init = function () {
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			ObtenerHorizonte();
			obtenerCambiosFirme();
		};


		function obtenerCambiosFirme() {
			return horizonteServicio.obtenerCambiosFirme(vm.idProyecto).then(
				function (respuesta) {

					vm.datosJustificacionHorizonte = [];

					if (respuesta.data != null && respuesta.data != "") {
						vm.listaDatos = respuesta.data;
						vm.añoFirme = vm.listaDatos.VigenciaFirme[0].VigenciaFirme;
					}
					console.log(vm.listaDatos);
				});
		}

		function Guardar() {
			var validaCambio = 1;
			if (vm.EsMover) {
				validaCambio = 0;
			}

			var d = new Date();
			vm.vigenciaActual = d.getFullYear();
			var seccionCapituloId = document.getElementById("id-capitulo-" + vm.nombreComponente);
			var params = {
				IdProyecto: vm.idProyecto,
				Mantiene: validaCambio,//!vm.EsMover,
				VigenciaInicio: vm.anioInicio,
				VigenciaFinal: vm.anioFinal,
				Usuario: vm.Usuario,
				GuiMacroproceso: justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa),
				InstanciaId: $sessionStorage.idInstancia,
				SeccionCapituloId: seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0,
			};
			vm.ClasesinputIni = " ";
			vm.ClasesinputFin = " ";
			vm.habilitaAlertaError = false;
			vm.AlertaMensajeError = "";

			if (vm.anioFinal == null || vm.anioFinal == "") {
				vm.ClasesinputFin = "edithorizonteError";
				vm.habilitaAlertaError = true;
				vm.AlertaMensajeError = "El año fin no puede ser nulo o mayor a 4 digitos.";
				utilidades.mensajeError("El año fin no puede ser nulo.", false); return false;
				vm.habilitaAlertaError = true;
			}
			else {
				if (vm.anioInicio > vm.anioFinal) {
					vm.ClasesinputFin = "edithorizonteError";
					vm.habilitaAlertaError = true;
					vm.AlertaMensajeError = "El año fin no puede ser inferior al año inicio.";
					utilidades.mensajeError("El año fin no puede ser inferior al año inicio.", false); return false;
					vm.habilitaAlertaError = true;
				}
				else {
					if (vm.anioInicio < vm.vigenciaActual && vm.anioInicio != vm.anioInicioOriginal) {
						vm.ClasesinputFin = "edithorizonteError";
						vm.habilitaAlertaError = true;
						vm.AlertaMensajeError = "El año de inicio no puede ser inferior al año vigente."
						utilidades.mensajeError("El año de inicio no puede ser inferior al año vigente.", false); return false;
						vm.habilitaAlertaError = true;
					}
					else {
						if (vm.anioFinal < vm.vigenciaActual) {
							vm.ClasesinputFin = "edithorizonteError";
							vm.habilitaAlertaError = true;
							vm.AlertaMensajeError = "El año fin no puede ser menor a la vigencia actual.";
							utilidades.mensajeError("El año fin no puede ser menor a la vigencia actual.", false); return false;
							vm.habilitaAlertaError = true;
						}
						//modificacion del viernes 27 de mayo 2022 para integrar con uat

						if (vm.anioFinal < vm.añoFirme) {
							vm.ClasesinputFin = "edithorizonteError";
							vm.habilitaAlertaError = true;
							vm.AlertaMensajeError = "El año fin no puede ser menor al año en firme actual.";
							utilidades.mensajeError("El año fin no puede ser menor al año en firme actual.", false); return false;
							vm.habilitaAlertaError = true;
						}
						else {
							if (vm.anioFinal <= vm.anioFinalOriginal) {
								utilidades.mensajeWarning("Se va a eliminar información registrada en el proyecto para las vigencias que se van a eliminar, ¿desea continuar?", function funcionContinuar() {
									horizonteServicio.actualizarHorizonte(params).then(function (response) {
										if (response.data && (response.statusText === "OK" || response.status === 200)) {

											if (response.data.Exito) {
												/* Notificación guardado */
												$sessionStorage.vigenciaHorizonte = vm.anioInicio + ' - ' + vm.anioFinal;
												vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
												parent.postMessage("cerrarModal", window.location.origin);
												utilidades.mensajeSuccess("", false, false, false, "Los datos de la tabla han sido guardados con éxito");
												vm.habilitarFinal = false;
												vm.habilitar = false;
												vm.habilitaGuardar = false;
												vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
												vm.actualizaresumen++;
												$sessionStorage.actualizaresumen = vm.actualizaresumen;
												vm.verBotones = false;
												limpiarCombos();
												ObtenerProyectosTramite();

											} else {
												swal('', response.data.Mensaje, 'warning');
											}


										} else {
											utilidades.mensajeError("Error al realizar la operación", false, null);
											vm.habilitarFinal = false;
											vm.habilitar = false;
											vm.habilitaGuardar = false;
											vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
											vm.verBotones = false;
										}


									});
								}, function funcionCancelar(reason) {
									console.log("reason", reason);
								});

							}

							else {
								utilidades.mensajeWarning("Se van a incrementar las vigencias registradas en el proyecto ¿Desea continuar?", function funcionContinuar() {
									horizonteServicio.actualizarHorizonte(params).then(function (response) {

										if (response.data && (response.statusText === "OK" || response.status === 200)) {

											if (response.data.Exito) {

												/* Notificación guardado */
												$sessionStorage.vigenciaHorizonte = vm.anioInicio + ' - ' + vm.anioFinal;
												vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
												parent.postMessage("cerrarModal", window.location.origin);
												utilidades.mensajeSuccess("", false, false, false, "Los datos de la tabla han sido guardados con éxito");
												vm.habilitarFinal = false;
												vm.habilitar = false;
												vm.habilitaGuardar = false;
												vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
												vm.actualizaresumen++;
												$sessionStorage.actualizaresumen = vm.actualizaresumen;
												vm.verBotones = false;												
												limpiarCombos();												
												ObtenerProyectosTramite();

											} else {
												swal('', response.data.Mensaje, 'warning');
											}


										} else {
											utilidades.mensajeError("Error al realizar la operación", false, null);
											vm.habilitarFinal = false;
											vm.habilitar = false;
											vm.habilitaGuardar = false;
											vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
											vm.verBotones = false;
										}

									});
								}, function funcionCancelar(reason) {
									console.log("reason", reason);
								});
							}
						}
					}
				}
			}
		}

		function ObtenerHorizonte() {

			return horizonteServicio.ObtenerHorizonte(vm.parametros).then(
				function (respuesta) {

					vm.datosHorizonte = [];
					vm.verBotones = false;
					if (respuesta.data != null && respuesta.data != "") {
						const listaDatos = respuesta.data;
						vm.anioInicio = listaDatos.vigenciaInicial;
						vm.anioFinal = listaDatos.vigenciaFinal;
						vm.anioInicioOriginal = listaDatos.vigenciaInicial;
						vm.anioFinalOriginal = listaDatos.vigenciaFinal;
						vm.estadoProyecto = listaDatos.Estado;

						if (vm.estadoProyecto == "En Ejecucion") {
							vm.mensaje1 = "El proyecto se encuentra en Ejecución.                                        ";
							vm.mensaje2 = " Se puede modificar únicamente el año fin, siempre y cuando se cumpla con las condiciones establecidas. ";


						}
						else {
							vm.mensaje1 = "El proyecto no se encuentra aún en Ejecución.                                 ";
							vm.mensaje2 = "Se puede modificar el año inicio y fin, siempre y cuando se cumpla con las condiciones establecidas. ";
							vm.habilitarInicio = true;

						}
					}

				});
		}
		function evaluarVerBotones() {
			vm.verBotones = false;
			if (vm.anioInicioOriginal != vm.anioInicio) {
				vm.verBotones = true;
			}
		}

		function habilitarVigencias() {
			if (vm.habilitarFinal) {
				vm.habilitarFinal = false;
				vm.habilitar = false;
				vm.habilitaGuardar = false;
				vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
				vm.ClasesinputIni = " ";
				vm.ClasesinputFin = " ";
				vm.habilitaAlertaError = false;
				vm.AlertaMensajeError = "";
				vm.verBotones = false;
				vm.anioInicio = vm.anioInicioOriginal;
				vm.anioFinal = vm.anioFinalOriginal;
			}
			else {
				vm.habilitarFinal = true;
				vm.habilitaGuardar = true;
				vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
				vm.ClasesinputIni = " ";
				vm.ClasesinputFin = " ";
				vm.habilitaAlertaError = false;
				vm.AlertaMensajeError = "";
				if (vm.habilitarInicio) {
					vm.habilitar = true;
				}
			}
		}
		function abrirMensajeInformacionHorizonte() {
			if (vm.estadoProyecto == "En Ejecucion") {
				utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Proyectos que se encuentran en Ejecución.</span>");
			}
			else {
				utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Proyectos que no se encuentran aún en Ejecución.</span>");
			}
		}
		function abrirMensajeInformacionHorizonte2() {
			if (vm.estadoProyecto == "En Ejecucion") {
				utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Opciones de ajuste del proyecto, </span><br /> <span class='tituhori' > Para proyectos que se encuentran en Ejecución.</span>");
			}
			else {
				utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Opciones de ajuste del proyecto, </span><br /> <span class='tituhori' > Para proyectos que no se encuentran aún en Ejecución.</span>");
			}
		}

		/* ------------------------ Validaciones ---------------------------------*/

		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Horizonte");
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				}
				var isValid = (erroresJson == null || erroresJson.length == 0);
				if (!isValid) {
					erroresJson[vm.nombreComponente].forEach(p => {
						if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
					});
				}
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
			}
		};

		vm.validarExitenciaHorizonte = function (errores) {
		}

		vm.errores = {
			'HOR001': vm.validarExitenciaHorizonte
		}
	}

	angular.module('backbone').component('horizonteProyecto', {

		templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/horizonte/horizonte.html",
		controller: horizonteController,
		controllerAs: "vm",
		bindings: {
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&'
		}
	});

})();