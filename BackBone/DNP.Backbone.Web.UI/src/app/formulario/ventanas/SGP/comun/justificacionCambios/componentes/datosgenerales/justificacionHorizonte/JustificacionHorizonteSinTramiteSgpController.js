(function () {
	'use strict';

	justificacionHorizonteSinTramiteSgpController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'justificacionHorizonteSinTramiteSgpServicio',
		'utilidades',
		'justificacionCambiosServicio',
		'horizonteSinTramiteSgpServicio'
	];



	function justificacionHorizonteSinTramiteSgpController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		justificacionHorizonteSinTramiteSgpServicio,
		utilidades,
		justificacionCambiosServicio,
		horizonteSinTramiteSgpServicio
	) {
		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "datosgeneraleshorizonte";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.habilitaGuardar = false;
		vm.estadoProyecto;
		vm.mensaje1 = "Justifique la modificación* (Maximo 8.000 caracteres)";
		vm.mensaje2 = "";
		vm.Usuario = usuarioDNP;

		vm.parametros = {
			idInstancia: $sessionStorage.idInstancia,
			idFlujo: $sessionStorage.idFlujoIframe,
			idNivel: $sessionStorage.idNivel,
			idProyecto: vm.idProyecto,
			idProyectoStr: $sessionStorage.idObjetoNegocio,
			Bpin: vm.codigoBpin

		};
		vm.obtenerCambiosFirme = obtenerCambiosFirme;
		vm.datosJustificacionHorizonte;
		vm.horizonteId;
		vm.periodo;
		vm.vigencia;
		vm.estado;
		vm.listaDatos;
		vm.seccionesCapitulos;
		vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.Justificacion;
		$sessionStorage.Justificacion;
		vm.JustificacionError = '';
		vm.paramJustificacion;
		vm.isEdicion = false;
		vm.guardar;
		vm.erroresActivos = [];
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
		vm.justificacionTmp = undefined;
		vm.soloLectura = false;
		//Inicio

		vm.paramJustificacion = {
			ProyectoId: vm.idProyecto,
			SeccionCapituloId: 0,
			InstanciaId: $sessionStorage.idInstancia,
			Justificacion: '',
			AplicaJustificacion: 1,
		};
		
		vm.init = function () {
			vm.Justificacion = vm.justificacioncapitulo;
			vm.justificacionTmp = angular.copy(vm.Justificacion);
			$sessionStorage.Justificacion = vm.Justificacion;
			obtenerCambiosFirme();
			vm.notificacioncambios({ handler: vm.notificacionJustificacion });
			vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
			vm.idProyecto = $sessionStorage.proyectoId;
			vm.idInstancia = $sessionStorage.idInstancia;
			vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
			vm.isEdicion = false;
			document.getElementById("Justificacion-horizonte").disabled = true;
			document.getElementById("Justificacion-horizonte").classList.add('disabled');
			/*document.getElementById("btn-guardar-edicion-horizonte").classList.add('disabled');*/
		};
		function OkCancelar() {
			setTimeout(function () {
				utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
			}, 500);
		}

		vm.editar = function (estado) {
			vm.isEdicion = null;
			vm.isEdicion = estado == 'editar';
			if (vm.isEdicion) {
				document.getElementById("Justificacion-horizonte").disabled = false;
				document.getElementById("Justificacion-horizonte").classList.remove('disabled');
				document.getElementById("btn-guardar-edicion-horizonte").classList.remove('disabled');
				vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
			} else {

				if (estado == 'cancelar') {

					utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
						OkCancelar();
						//vm.Justificacion = $sessionStorag.Justificacion;//document.getElementById("Justificacion-horizonte").value;
						vm.Justificacion = vm.justificacionTmp;
						document.getElementById("Justificacion-horizonte").disabled = true;
						document.getElementById("Justificacion-horizonte").classList.add('disabled');
						document.getElementById("btn-guardar-edicion-horizonte").classList.add('disabled');
						vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

					}, function funcionCancelar(reason) {
						vm.isEdicion = true;
						return;
					}, null, null, "Advertencia");
				}
                else {
					//vm.Justificacion = $sessionStorag.Justificacion;//document.getElementById("Justificacion-horizonte").value;
					vm.Justificacion = vm.justificacionTmp;
					document.getElementById("Justificacion-horizonte").disabled = true;
					document.getElementById("Justificacion-horizonte").classList.add('disabled');
					document.getElementById("btn-guardar-edicion-horizonte").classList.add('disabled');
					vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
                }

				
			}
		}

		vm.guardar = function () {
			if (vm.Justificacion == '' || vm.Justificacion == undefined) {
				utilidades.mensajeError('Debe ingresar una justificación.');
				return false;
			}
			var seccionCapitulo = document.getElementById("seccion-capitulo-datosgeneraleshorizonte");

			vm.paramJustificacion.Justificacion = vm.Justificacion;
			vm.paramJustificacion.SeccionCapituloId = seccionCapitulo.value;

			utilidades.mensajeWarning("Se va a actualizar la Justificacion, desea Continuar?", function funcionContinuar() {
				justificacionCambiosServicio.guardarCambiosFirme(vm.paramJustificacion).then(function (response) {

					if (response.data && (response.statusText === "OK" || response.status === 200)) {

						if (response.data.Exito) {
							parent.postMessage("cerrarModal", window.location.origin);
							utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
							vm.justificacionTmp = angular.copy(vm.Justificacion);
							document.getElementById("Justificacion-horizonte").classList.add('disabled');
							//vm.editar('');
							//vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
							document.getElementById("btn-guardar-edicion-horizonte").classList.add('disabled');
							vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
							vm.isEdicion = false;
							vm.ejecutarErrores();
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

		function obtenerCambiosFirme() {

		
			return horizonteSinTramiteSgpServicio.ObtenerHorizonte(vm.parametros).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						const listaDatos = respuesta.data;
						vm.anioInicio = listaDatos.VigenciaInicial;
						vm.anioFinal = listaDatos.VigenciaFinal;
						vm.anioInicioOriginal = listaDatos.VigenciaInicial;
						vm.anioFinalOriginal = listaDatos.PuntajeSEP;
						vm.estadoProyecto = listaDatos.Estado;
						vm.anioInicioMGA = listaDatos.AnioEstudio;
						vm.anioFinalMGA = listaDatos.PuntajeSEP;
						vm.soloLectura = $sessionStorage.soloLectura;
	
					}

				});

			//return justificacionHorizonteSinTramiteSgpServicio.obtenerCambiosFirme(vm.idProyecto).then(
			//	function (respuesta) {

			//		vm.datosJustificacionHorizonte = [];
			//		vm.verBotones = false;
			//		if (respuesta.data != null && respuesta.data != "") {
			//			vm.listaDatos = respuesta.data;
			//		}
			//		console.log(vm.listaDatos);
			//	});
		}

		/* ------------------------- Validación ---------------------------*/

		vm.notificacionValidacion = function (errores) {
			console.log("Validación  - Justificación Horizonte");
			vm.limpiarErrores();
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				}
				var isValid = (erroresJson == null || erroresJson.length == 0);
				if (!isValid) {
					erroresJson.errores.forEach(p => {
						if (vm.errores[p.Error] != undefined) {
							vm.erroresActivos.push({
								Error: p.Error,
								Descripcion: p.Descripcion
							});
							vm.errores[p.Error](p.Error, p.Descripcion);
						}
					});
				}
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
			}
		};

		vm.limpiarErrores = function () {
			vm.JustificacionError = '';
		}

		vm.ejecutarErrores = function () {
			vm.erroresActivos.forEach(p => {
				if (vm.errores[p.Error] != undefined) {
					vm.errores[p.Error](p.Error, p.Descripcion);
				}
			});
		}

		vm.validarErroresActivos = function (codError) {
			if (vm.erroresActivos != null) {
				vm.erroresActivos = vm.erroresActivos.filter(function (value, index, arr) {
					return value.Error != codError;
				});
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: (vm.erroresActivos.length <= 0) });
			}
		}

		vm.validarJustificacion = function (errores, descripcion) {
			if (vm.Justificacion == null || vm.Justificacion.length <= 0) {
				vm.JustificacionError = descripcion;
			} else {
				vm.limpiarErrores();
				vm.validarErroresActivos(errores);
			}
		}

		vm.errores = {
			'JUST001': vm.validarJustificacion
		}

	}

	angular.module('backbone').component('datosgeneralessgphorizontesintramitesgp', {
		templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonteSinTramiteSgp.html",
		controller: justificacionHorizonteSinTramiteSgpController,
		controllerAs: "vm",
		bindings: {
			justificacioncapitulo: '@',
			notificacioncambios: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&'
		}
	});

})();