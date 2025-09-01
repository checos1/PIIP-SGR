(function () {
	'use strict';

	DatosgeneralesjustificacionIndicadoresController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'justificacionIndicadoresServicio',
		'utilidades',
		'justificacionCambiosServicio'
	];



	function DatosgeneralesjustificacionIndicadoresController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		justificacionIndicadoresServicio,
		utilidades,
		justificacionCambiosServicio
	) {
		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "datosgeneralesindicadoresdepr";
		vm.ObtenerAjustesIndicadoresPorBpin = ObtenerAjustesIndicadoresPorBpin;
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
		vm.mensajejustificacion = "Justifique la modificación* (Maximo 8.000 caracteres)";
		vm.isEdicion = false;
		//vm.seccionCapitulo = 3;
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
		vm.ConvertirNumero = ConvertirNumero;
		vm.tablaVisible = false;
		vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;


		$scope.myFilter = function (item) {
			//return item.Ajuste === 'Nuevo' || item.Ajuste === 'Meta Ajustada' || item.Ajuste === 'Eliminado';
			return ((item.Ajuste === 'Nuevo' || item.Ajuste === 'Meta Ajustada') && item.ClaseCSS === 'tabla');
		};

		$scope.myFilter2 = function (item) {

			return ((item.Ajuste === 'Nuevo' || item.Ajuste === 'Modificado' || item.Ajuste === 'Eliminado') && item.ClaseCSS === 'detalle');
		};
		vm.validacion = "";

		vm.init = function () {
			ObtenerAjustesIndicadoresPorBpin();
			vm.justificacion = vm.justificacioncapitulo;
			vm.notificacioncambios({ handler: vm.notificacionJustificacion });
			vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
			$sessionStorage.verDiferencia = true;
		};

		vm.editar = function () {
			vm.isEdicion = true;
			vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
		}

		vm.cancelar = function () {
			vm.isEdicion = false;
			vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
			document.getElementById("justificacion").value = vm.justificacioncapitulo;
		}

		function ObtenerSeccionCapitulo() {
			const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
			vm.seccionCapitulo = span.textContent;
		}

		vm.guardar = function () {
			if (vm.justificacion == '' || vm.justificacion == undefined) {
				utilidades.mensajeError('Debe ingresar una justificación.');
				return false;
			}

			ObtenerSeccionCapitulo();
			var data = {
				ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
				Justificacion: vm.justificacion,
				SeccionCapituloId: vm.seccionCapitulo,
				InstanciaId: vm.idInstancia,
				AplicaJustificacion: 1,
			}

			justificacionCambiosServicio.guardarCambiosFirme(data)
				.then(function (response) {
					if (response.data.Exito) {
						utilidades.mensajeSuccess(response.data.Mensaje);
						document.getElementById("justificacion").value = vm.justificacion;
						vm.justificacioncapitulo = vm.justificacion;
						vm.isEdicion = false;
					}
					else {
						utilidades.mensajeError(response.data.Mensaje);
					}
				});
		}

		function ObtenerAjustesIndicadoresPorBpin() {
			return justificacionIndicadoresServicio.IndicadoresValidarCapituloModificado(vm.codigoBpin).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						console.log(respuesta.data)
						$scope.datos = respuesta.data;

						$scope.datosTabla = respuesta.data.find(p => p.ClaseCSS === 'tabla');
					}

				});
		}

		function ConvertirNumero(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 4,
			}).format(numero);
		}

		vm.validar = function () {
			if (vm.justificacion == '' || vm.justificacion == undefined) {
				vm.validacion = "Debe diligenciar la justificación del capítulo Horizonte dentro de la pestaña Justificación.";
				return false;
			}
			vm.validacion = "";
			return true;
		}

		vm.notificacionValidacion = function (errores) {
			console.log("Validación  - Justificación Fuentes de financiación");
			vm.limpiarErrores();
			var isValid = true;
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
					isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson.errores.forEach(p => {
							if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
						});
					}
				}
			}
			vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
		};

		vm.limpiarErrores = function () {
			var campoObligatorioJustificacion = document.getElementById("justificacionindicadores-justificacion-error");
			var ValidacionFFR1Error = document.getElementById("justificacionindicadores-justificacion-error-mns");

			if (campoObligatorioJustificacion != undefined) {
				if (ValidacionFFR1Error != undefined) {
					ValidacionFFR1Error.innerHTML = '';
					campoObligatorioJustificacion.classList.add('hidden');
				}
			}
		}

		vm.validarJustificacion = function (errores) {
			var campoObligatorioJustificacion = document.getElementById("justificacionindicadores-justificacion-error");
			var ValidacionFFR1Error = document.getElementById("justificacionindicadores-justificacion-error-mns");

			if (campoObligatorioJustificacion != undefined) {
				if (ValidacionFFR1Error != undefined) {
					ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
					campoObligatorioJustificacion.classList.remove('hidden');
				}
			}
		}

		vm.errores = {
			'JUST001': vm.validarJustificacion
		}
	}

	angular.module('backbone').component('datosgeneralesindicadoresdepr', {
		templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionIndicadores/justificacionIndicadores.html",
		controller: DatosgeneralesjustificacionIndicadoresController,
		controllerAs: "vm",
		bindings: {
			justificacioncapitulo: '@',
			notificacioncambios: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&'
		}
	});

})();