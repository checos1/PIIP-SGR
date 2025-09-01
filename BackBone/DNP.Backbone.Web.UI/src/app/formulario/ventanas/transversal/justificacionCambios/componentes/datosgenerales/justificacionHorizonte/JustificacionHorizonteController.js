(function () {
	'use strict';

	DatosgeneralesjustificacionHorizonteController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'justificacionHorizonteServicio',
		'utilidades',
		'justificacionCambiosServicio'
	];



	function DatosgeneralesjustificacionHorizonteController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		justificacionHorizonteServicio,
		utilidades,
		justificacionCambiosServicio
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
		vm.JustificacionError = '';
		vm.paramJustificacion;
		vm.isEdicion = false;
		vm.guardar;
		vm.erroresActivos = [];
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

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


		vm.editar = function (estado) {
			vm.isEdicion = null;
			vm.isEdicion = estado == 'editar';
			if (vm.isEdicion) {
				document.getElementById("Justificacion-horizonte").disabled = false;
				document.getElementById("Justificacion-horizonte").classList.remove('disabled');
				document.getElementById("btn-guardar-edicion-horizonte").classList.remove('disabled');
				vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
			} else {
				vm.Justificacion = document.getElementById("Justificacion-horizonte").value;
				document.getElementById("Justificacion-horizonte").disabled = true;
				document.getElementById("Justificacion-horizonte").classList.add('disabled');
				/*document.getElementById("btn-guardar-edicion-horizonte").classList.add('disabled');*/
				vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
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
							vm.editar('');
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
			return justificacionHorizonteServicio.obtenerCambiosFirme(vm.idProyecto).then(
				function (respuesta) {

					vm.datosJustificacionHorizonte = [];
					vm.verBotones = false;
					if (respuesta.data != null && respuesta.data != "") {
						vm.listaDatos = respuesta.data;
					}
					console.log(vm.listaDatos);
				});
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

	angular.module('backbone').component('datosgeneraleshorizonte', {
		templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonte.html",
		controller: DatosgeneralesjustificacionHorizonteController,
		controllerAs: "vm",
		bindings: {
			justificacioncapitulo: '@',
			notificacioncambios: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&'
		}
	});

})();