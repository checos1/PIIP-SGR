(function () {
	'use strict';

	DatosgeneralesjustificacionLocalizacionController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'justificacionLocalizacionServicio',
		'utilidades',
		'justificacionCambiosServicio',
		'localizacionJustificacionServicio',
		'utilsValidacionSeccionCapitulosServicio'
	];



	function DatosgeneralesjustificacionLocalizacionController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		justificacionLocalizacionServicio,
		utilidades,
		justificacionCambiosServicio,
		localizacionJustificacionServicio,
		utilsValidacionSeccionCapitulosServicio

	) {
		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "datosgeneraleslocalizaciones";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.habilitaGuardar = false;
		vm.estadoProyecto;
		vm.mensaje1 = "";
		vm.mensaje2 = "";
		vm.Usuario = usuarioDNP;
		vm.listaDatos;
		vm.seccionesCapitulos;
		vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.isEdicion = false;
		vm.MostrarError = false;
		vm.guardar;
		vm.erroresActivos = [];
		vm.listaLocalizacionMod = [];
		vm.listaLocalizacionNuevos = [];
		vm.listaLocalizacionModBorrados = [];
		vm.Modificar;
		vm.Modificar1;
		vm.obtenerJustificacion;
		vm.paramJustificacion;
		vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

		//Inicio
		vm.paramJustificacion = {
			ProyectoId: vm.idProyecto,
			SeccionCapituloId: 0,
			InstanciaId: $sessionStorage.idInstancia,
			Justificacion: ''

		};

		vm.init = function () {
			vm.obtenerJustificacion();
			vm.notificacioncambios({ handler: vm.notificacionJustificacion });
			//vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });

			vm.idProyecto = $sessionStorage.proyectoId;
			vm.idInstancia = $sessionStorage.idInstancia;
			vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
			vm.isEdicion = false;
			ObtenerJustificacionLocalizacionProyectoFirme();
			document.getElementById("Justificacion-justificacionLocalizacion").disabled = true;
			document.getElementById("Justificacion-justificacionLocalizacion").classList.add('disabled');
			document.getElementById("btn-guardar-edicion-localizacion").classList.add('disabled');
		};
		//fin inicio


		vm.mostrarTab = function (origen) {
			vm.mensaje = "";
			switch (origen) {
				case 1:
					{
						ObtenerJustificacionLocalizacionProyectoFirme();
						$('ul.tabsLocalizaciones > li').removeClass('active');
						$('ul.tabsLocalizaciones > li:first-child').addClass('active');

						break;
					}
				case 2:
					{
						ObtenerJustificacionLocalizacionProyecto()
						break;
					}
			}
		}

		// grilla en firme
		function ObtenerJustificacionLocalizacionProyectoFirme() {
			return justificacionLocalizacionServicio.ObtenerJustificacionLocalizacionProyecto(vm.idProyecto, usuarioDNP).then(
				function (respuesta) {
					vm.verBotones = false;

					if (respuesta.data != null && respuesta.data != "") {
						vm.Modificar = 0;
						vm.Modificar1 = 0;
						vm.DatosLocalizacion = respuesta.data.ProyectosLocalizacionFirme;
					}
					console.log(vm.DatosLocalizacion);
				});
		}
		///  fin grilla en firme

		// grilla Modificados
		function ObtenerJustificacionLocalizacionProyecto() {
			vm.listaLocalizacionMod = [];

			return justificacionLocalizacionServicio.ObtenerJustificacionLocalizacionProyecto(vm.idProyecto, usuarioDNP).then(
				function (respuesta) {
					vm.verBotones = false;
					vm.Modificar = 1;
					vm.Modificar1 = 1;
					if (respuesta.data.VerificacionColumnas != null && respuesta.data.VerificacionColumnas != "") {
						vm.listaLocalizacionMod = respuesta.data.VerificacionColumnas.filter(x => x.DepartamentId == 1 || x.MunicipalityId == 1 || x.AgrupacionId == 1 || x.TipoAgrupacionId == 1);
						vm.listaLocalizacionModBorrados = respuesta.data.ListadoBorrados;
						if (vm.listaLocalizacionModBorrados == null) {
							vm.Modificar1 = 0;
						}
						vm.listaLocalizacionNuevos = respuesta.data.ListadoNuevos;
						if (vm.listaLocalizacionNuevos == null) {
							vm.Modificar = 0;
						}
						vm.DatosLocalizacion = vm.listaLocalizacionMod.concat(respuesta.data.ListadoNuevos);
					}
					console.log(vm.DatosLocalizacion);
				});
		}

		///  fin grilla Modificados

		vm.editar = function (estado) {
			vm.isEdicion = null;
			vm.isEdicion = estado == 'editar';
			if (vm.isEdicion) {
				document.getElementById("Justificacion-justificacionLocalizacion").disabled = false;
				document.getElementById("Justificacion-justificacionLocalizacion").classList.remove('disabled');
				document.getElementById("btn-guardar-edicion-localizacion").classList.remove('disabled');
				vm.ClasesbtnGuardar = "btn btn-default btn-mdLocalizacion";
			} else {
				document.getElementById("Justificacion-justificacionLocalizacion").value = $sessionStorage.Justificacionoriginal;
				document.getElementById("Justificacion-justificacionLocalizacion").disabled = true;
				document.getElementById("Justificacion-justificacionLocalizacion").classList.add('disabled');
				document.getElementById("btn-guardar-edicion-localizacion").classList.add('disabled');
				vm.ClasesbtnGuardar = "btn btnguardarLocalizacion";
			}
		}

		vm.guardar = function () { 

			var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
			var seccionCapituloIdValor = seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0;
			vm.paramJustificacion.ProyectoId = vm.idProyecto;
			vm.paramJustificacion.Justificacion = document.getElementById("Justificacion-justificacionLocalizacion").value;
			vm.paramJustificacion.SeccionCapituloId = seccionCapituloIdValor;
			vm.paramJustificacion.AplicaJustificacion = 1;
			utilidades.mensajeWarning("Se va a actualizar la Justificación, desea Continuar?", function funcionContinuar() {
				justificacionCambiosServicio.guardarCambiosFirme(vm.paramJustificacion).then(function (response) {

					if (response.statusText === "OK" || response.status === 200) {
						parent.postMessage("cerrarModal", window.location.origin);
						utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
						document.getElementById("Justificacionlocalizacion1").value = document.getElementById("Justificacion-justificacionLocalizacion").value;
						vm.Justificacionoriginal = document.getElementById("Justificacion-justificacionLocalizacion").value;
						$sessionStorage.Justificacionoriginal = vm.Justificacionoriginal;
						//vm.guardadoevent({ nombreComponenteHijo: "localizacionJustificacion" });
						document.getElementById("Justificacion-justificacionLocalizacion").disabled = true;
						document.getElementById("Justificacion-justificacionLocalizacion").classList.add('disabled');
						document.getElementById("btn-guardar-edicion-localizacion").classList.add('disabled');
						vm.ClasesbtnGuardar = "btn btnguardarLocalizacion";
						vm.editar('');
						vm.ejecutarErrores();
					} else {
						swal('', response.data.Mensaje, 'warning');
					}
				});

			}, function funcionCancelar(reason) {
				console.log("reason", reason);
			});

		}

		vm.obtenerJustificacion = function () {
			var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
			var seccionCapituloIdValor = seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0;
			vm.paramJustificacion.Justificacion = document.getElementById("Justificacion-justificacionLocalizacion").value == undefined ? '' : document.getElementById("Justificacion-justificacionLocalizacion").value;
			vm.paramJustificacion.SeccionCapituloId = seccionCapituloIdValor;
			vm.paramJustificacion.ProyectoId = vm.idProyecto;
			vm.paramJustificacion.AplicaJustificacion = 1;
			return localizacionJustificacionServicio.ObtenerCapitulosModificados(vm.guiMacroproceso, vm.idProyecto, vm.idInstancia).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						vm.Justificacionoriginal = respuesta.data.filter(x => x.SeccionCapituloId == vm.paramJustificacion.SeccionCapituloId)[0].Justificacion;
						document.getElementById("Justificacion-justificacionLocalizacion").value = vm.Justificacionoriginal;
					}
				}
			);
		}

		/* ------------------------- Validación ---------------------------*/

		vm.notificacionValidacion = function (errores) {
			console.log("Validación-Justificación Localizacion");

			vm.limpiarErrores();
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);

					var isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson.errores.forEach(p => {
							if (vm.errores[p.Error] != undefined) {
								vm.erroresActivos.push({
									Error: p.Error,
									Descripcion: p.Descripcion
								});
								vm.mensajepruebas = p.Descripcion;
								vm.errores[p.Error](p.Error, p.Descripcion);
							}
						});
					}
					vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
				}
			}
		};

		vm.limpiarErrores = function () {
			var campoObligatorioJustificacionLoc = document.getElementById("justificacionlocalizacion-justificacion-error");
			var ValidacionJustificacionLoc = document.getElementById("justificacionlocalizacion-justificacion-error-mns");
			if (ValidacionJustificacionLoc != undefined) {
				ValidacionJustificacionLoc.innerHTML = '';
				campoObligatorioJustificacionLoc.classList.add('hidden');
			}
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
			var campoObligatorioJustificacionLoc = document.getElementById("justificacionlocalizacion-justificacion-error");
			var ValidacionJustificacionLoc = document.getElementById("justificacionlocalizacion-justificacion-error-mns");

			if (campoObligatorioJustificacionLoc != undefined) {
				if (ValidacionJustificacionLoc != undefined) {
					ValidacionJustificacionLoc.innerHTML = '<span>' + descripcion + "</span>";
					campoObligatorioJustificacionLoc.classList.remove('hidden');
				}
			}
		}

		vm.errores = {
			'JUST001': vm.validarJustificacion,
			'JUST002': vm.validarJustificacion
		}

	}

	angular.module('backbone').component('datosgeneraleslocalizaciones', {
		templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionLocalizacion/JustificacionLocalizacion.html",
		controller: DatosgeneralesjustificacionLocalizacionController,
		controllerAs: "vm",
		bindings: {
			justificacioncapitulo: '@',
			notificacioncambios: '&',
			callback: '&',
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',

		}
	});

})();