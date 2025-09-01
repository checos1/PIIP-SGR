(function () {
	'use strict';
	localizacionJustificacionController.$inject = [
		'$scope',
		'gestionRecursosServicio',
		'$sessionStorage',
		'utilidades',
		'justificacionCambiosServicio',
		'$uibModal',
		'localizacionJustificacionServicio',
		'relacionPlanificacionServicio'
	];

	function localizacionJustificacionController(
		$scope,
		gestionRecursosServicio,
		$sessionStorage,
		utilidades,
		justificacionCambiosServicio,
		$uibModal,
		localizacionJustificacionServicio,
		relacionPlanificacionServicio
	) {
		var vm = this;
		vm.nombreComponente = "datosgeneraleslocalizaciones";
		vm.BPIN = $sessionStorage.idObjetoNegocio;
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.mensaje1 = "Justifique la modificación* (Maximo 8.000 caracteres)";
		vm.Usuario = usuarioDNP;
		vm.datosJustificacionHorizonte;
		vm.seccionesCapitulos;
		vm.activaTexto = false;
		vm.seccionCapituloId = 0;
		vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.JustificacionLoc;
		vm.Justificacionoriginal = "";
		vm.JustificacionError = "";
		vm.paramJustificacion;
		vm.isEdicion = false;
		vm.guardar;
		vm.guardarLocalizacion;
		vm.guardarJustificacion;
		vm.obtenerJustificacion;
		vm.BPIN = $sessionStorage.idObjetoNegocio;
		vm.abrirModalAgregarLocalizacion = abrirModalAgregarLocalizacion;
		vm.LocalizacionId;
		vm.erroresActivos = [];
		vm.habilitarBotones = false;
		vm.ClasesbtnGuardar = "btn btnguardarLocalizacion";

		var DataJson = "";
		var arreglolistaLocalizaciones = [];

		vm.paramJustificacion = {
			ProyectoId: 0,
			SeccionCapituloId: 0,
			InstanciaId: $sessionStorage.idInstancia,
			Justificacion: ''


		};

		vm.init = function () {
			vm.ObtenerLocalizacionProyecto(vm.BPIN);
			
			vm.notificacioncambios({ handler: vm.notificacionJustificacion });
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.idProyecto = $sessionStorage.proyectoId;
			vm.idInstancia = $sessionStorage.idInstancia;
			vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
			vm.habilitarBotones = false;
			document.getElementById("Justificacionlocalizacion1").disabled = true;
			document.getElementById("Justificacionlocalizacion1").classList.add('disabled');
			//$("#btn-editar-edicion1").attr('disabled', true);
			//$("#btn-cancelar-edicion").attr('disabled', true);
			$("#btn-guardar-edicion").attr('disabled', true);
			vm.obtenerJustificacion();
		};

		// grilla
		vm.ObtenerLocalizacionProyecto = function (Bpin) {

			return gestionRecursosServicio.ObtenerLocalizacionProyecto($sessionStorage.idInstancia).then(
				function (respuesta) {

					var listaLocalizaciones = []

					var localizacion = "";
					if (respuesta.data != null && respuesta.data != "") {
						arreglolistaLocalizaciones = jQuery.parseJSON(respuesta.data);
						DataJson = respuesta.data;
						if (arreglolistaLocalizaciones.Localizacion != null) {
							for (var ls = 0; ls < arreglolistaLocalizaciones.Localizacion.length; ls++) {
								if (arreglolistaLocalizaciones.Localizacion[ls].RegionId != null) {

									if (arreglolistaLocalizaciones.Localizacion[ls].Departamento == "0") {
										arreglolistaLocalizaciones.Localizacion[ls].Departamento = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].Municipio == "0") {
										arreglolistaLocalizaciones.Localizacion[ls].Municipio = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion == "0") {
										arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].Agrupacion == "0") {
										arreglolistaLocalizaciones.Localizacion[ls].Agrupacion = "";
									}

									localizacion =
									{
										"Departamento": arreglolistaLocalizaciones.Localizacion[ls].Departamento,
										"Municipio": arreglolistaLocalizaciones.Localizacion[ls].Municipio,
										"Tipo": arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion,
										"Agrupacion": arreglolistaLocalizaciones.Localizacion[ls].Agrupacion,
										"Eliminar": true,
										"LocalizacionId": 0
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].Departamento == 0) {
										arreglolistaLocalizaciones.Localizacion[ls].Departamento = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].Municipio == 0) {
										arreglolistaLocalizaciones.Localizacion[ls].Municipio = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion == 0) {
										arreglolistaLocalizaciones.Localizacion[ls].TipoAgrupacion = "";
									}
									if (arreglolistaLocalizaciones.Localizacion[ls].Agrupacion == 0) {
										arreglolistaLocalizaciones.Localizacion[ls].Agrupacion = "";
									}

									listaLocalizaciones.push(localizacion);
								}
							}
						}
						if (arreglolistaLocalizaciones.NuevaLocalizacion != null) {
							for (var po = 0; po < arreglolistaLocalizaciones.NuevaLocalizacion.length; po++) {



								if (arreglolistaLocalizaciones.NuevaLocalizacion[po].RegionId != null) {
									$("#btn-editar-edicion1").attr('disabled', false);
									//$("#btn-cancelar-edicion").attr('disabled', true);
									if (arreglolistaLocalizaciones.NuevaLocalizacion[po].Departamento == "0") {
										arreglolistaLocalizaciones.NuevaLocalizacion[po].Departamento = "";
									}
									if (arreglolistaLocalizaciones.NuevaLocalizacion[po].Municipio == "0" || arreglolistaLocalizaciones.NuevaLocalizacion[po].Municipio == "") {
										arreglolistaLocalizaciones.NuevaLocalizacion[po].Municipio = "";
									}
									if (arreglolistaLocalizaciones.NuevaLocalizacion[po].TipoAgrupacion == "0" || arreglolistaLocalizaciones.NuevaLocalizacion[po].TipoAgrupacion == "") {
										arreglolistaLocalizaciones.NuevaLocalizacion[po].TipoAgrupacion = "";
									}
									if (arreglolistaLocalizaciones.NuevaLocalizacion[po].Agrupacion == "0" || arreglolistaLocalizaciones.NuevaLocalizacion[po].Agrupacion == "") {
										arreglolistaLocalizaciones.NuevaLocalizacion[po].Agrupacion = "";
									}
									var NuevaLocalizacion =
									{
										"Departamento": arreglolistaLocalizaciones.NuevaLocalizacion[po].Departamento,
										"Municipio": arreglolistaLocalizaciones.NuevaLocalizacion[po].Municipio,
										"Tipo": arreglolistaLocalizaciones.NuevaLocalizacion[po].TipoAgrupacion,
										"Agrupacion": arreglolistaLocalizaciones.NuevaLocalizacion[po].Agrupacion,
										"Eliminar": false,
										"LocalizacionId": arreglolistaLocalizaciones.NuevaLocalizacion[po].LocalizacionId
									}

									listaLocalizaciones.push(NuevaLocalizacion);
								}
							}
						}
						vm.DatosLocalizacion = listaLocalizaciones;

					}
				}
			);
		}

		///  fin grilla
		// metodos grilla
		function abrirModalAgregarLocalizacion(LocalizacionId) {
			$sessionStorage.LocalizacionId = LocalizacionId;
			$sessionStorage.listaObjLocalizacion = DataJson;
			$uibModal.open({
				templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/localizacionJustificacion/modal/modalAgregarLocalizaciones.html',
				controller: 'modalAgregarLocalizacionController',
			}).result.then(function (result) {
				vm.ObtenerLocalizacionProyecto(vm.BPIN);
				vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
			}, function (reason) {

			}), err => {
				toastr.error("Ocurrió un error al consultar el idAplicacion");
			};
			vm.ObtenerLocalizacionProyecto(vm.BPIN);
		}

		vm.guardarLocalizacion = function () {
			abrirModalAgregarLocalizacion();
		}



		// fin metodos grilla

		vm.eliminarLocalizacion = function (LocalizacionId) {
			var arreglo = arreglolistaLocalizaciones;
			var objJson = angular.fromJson(vm.DatosLocalizacion);
			var localizacionCambio = arreglo.NuevaLocalizacion.filter(x => x.LocalizacionId == LocalizacionId)[0];
			var munName = null;
			var depName = null;
			var agrName = null;
			var tipAgrName = null;

			if (localizacionCambio != undefined) {
				munName = localizacionCambio.Municipio;
			}
			if (localizacionCambio != undefined) {
				depName = localizacionCambio.Departamento;
			}
			if (localizacionCambio != undefined) {
				tipAgrName = localizacionCambio.TipoAgrupacion;
			}
			if (localizacionCambio != undefined) {
				agrName = localizacionCambio.Agrupacion;
			}

			let localizacion = [];
			let l = {
				LocalizacionId: LocalizacionId,
				RegionId: null,
				Region: null,
				DepartamentoId: null,
				Departamento: null,
				MunicipioId: null,
				Municipio: null,
				TipoAgrupacionId: null,
				TipoAgrupacion: null,
				AgrupacionId: null,
				Agrupacion: null,
			};
			localizacion.push(l);
			var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
			vm.seccionCapituloId = seccionCapituloId != undefined && seccionCapituloId.innerHTML != '' ? seccionCapituloId.innerHTML : 657;//carga primero esta función que la de donde se le asigna el valor al html
			var parametro = {
				ProyectoId: vm.idProyecto,
				BPIN: vm.BPIN,
				Accion: "Delete",
				Justificacion: document.getElementById("Justificacionlocalizacion1").value,
				SeccionCapituloId: vm.seccionCapituloId,
				InstanciaId: vm.idInstancia,
				NuevaLocalizacion: localizacion,
			};
			if (depName == null) {
				depName = "N/A";
			}
			if (munName == null) {
				munName = "N/A";
			}
			if (tipAgrName == null) {
				tipAgrName = "N/A";
			}
			if (agrName == null) {
				agrName = "N/A";
			}
			utilidades.mensajeWarning("Se va a eliminar una localización", function funcionContinuar() {
				localizacionJustificacionServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
					if (response.data && (response.statusText === "OK" || response.status === 200)) {
						if (response.data.Exito) {
							vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
							parent.postMessage("cerrarModal", window.location.origin);
							utilidades.mensajeSuccess("Se elimino la linea de información: " + depName + " /" + munName + " /" + tipAgrName + " /" + agrName + " en la parte inferior de la tabla ''localizaciones''. Recuerde Justificar esta modificación en el campo Justificación", false, false, false, "Los datos fueron eliminados con éxito!");
							vm.ObtenerLocalizacionProyecto(vm.BPIN);
						} else {
							var mensajeError = JSON.parse(response.data.Mensaje);
							var mensajeReturn = '';
							console.log(mensajeError);
							try {
								for (var i = 0; i < mensajeError.ListaErrores.length; i++) {
									mensajeReturn = mensajeReturn + mensajeError.ListaErrores[i].Error + '\n';
								}
								
							}
							catch {
								mensajeReturn = mensajeError.Mensaje;
							}
							swal('', mensajeReturn, 'warning');
						}
					} else {
						swal('', "Error al realizar la operación", 'error');
					}
				});
			}, function funcionCancelar(reason) {
				console.log("reason", reason);
			});
		}

		vm.editar = function (estado) {
			vm.isEdicion = null;

			vm.isEdicion = estado == 'editar';
			if (vm.isEdicion) {
				document.getElementById("Justificacionlocalizacion1").disabled = false;
				document.getElementById("Justificacionlocalizacion1").classList.remove('disabled');
				$("#btn-editar-edicion1").attr('disabled', false);
				//$("#btn-cancelar-edicion").attr('disabled', false);
				$("#btn-guardar-edicion").attr('disabled', false);
				vm.ClasesbtnGuardar = "btn btn-default btn-mdLocalizacion";
			} else {
				document.getElementById("Justificacionlocalizacion1").value = $sessionStorage.Justificacionoriginal;
				document.getElementById("Justificacionlocalizacion1").disabled = true;
				document.getElementById("Justificacionlocalizacion1").classList.add('disabled');
				//$("#btn-editar-edicion1").attr('disabled', false);
				//$("#btn-cancelar-edicion").attr('disabled', true);
				$("#btn-guardar-edicion").attr('disabled', true);
				vm.ClasesbtnGuardar = "btn btnguardarLocalizacion";
			}
		}

		vm.obtenerJustificacion = function () {
			setTimeout(function () {
				var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
				vm.seccionCapituloId = seccionCapituloId != undefined && seccionCapituloId.innerHTML != '' ? seccionCapituloId.innerHTML : 657;//carga primero esta función que la de donde se le asigna el valor al html
				$sessionStorage.SeccionCapituloLocalizacionAjustes = vm.seccionCapituloId;
				vm.paramJustificacion.Justificacion = document.getElementById("Justificacionlocalizacion1").value;
				vm.paramJustificacion.SeccionCapituloId = vm.seccionCapituloId;
				vm.paramJustificacion.ProyectoId = vm.idProyecto;
				return localizacionJustificacionServicio.obtenerCapitulosModificadosLocalizacion(vm.guiMacroproceso, vm.idProyecto, vm.idInstancia, vm.seccionCapituloId).then(
					function (respuesta) {

						if (respuesta.data != null && respuesta.data != "") {
							vm.Justificacionoriginal = respuesta.data[0].Justificacion;
							document.getElementById("Justificacionlocalizacion1").value = vm.Justificacionoriginal;
							$sessionStorage.Justificacionoriginal = vm.Justificacionoriginal;
						}
					}
				);
			}, 2000);
			
		}



		vm.guardarJustificacion = function () {
			var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
			vm.seccionCapituloId = seccionCapituloId != undefined && seccionCapituloId.innerHTML != '' ? seccionCapituloId.innerHTML : 657;//carga primero esta función que la de donde se le asigna el valor al html

			vm.paramJustificacion.Justificacion = document.getElementById("Justificacionlocalizacion1").value;
			vm.paramJustificacion.SeccionCapituloId = vm.seccionCapituloId ;
			vm.paramJustificacion.ProyectoId = vm.idProyecto;
			vm.paramJustificacion.AplicaJustificacion = 1;

			utilidades.mensajeWarning("Se va a actualizar la Justificación, desea Continuar?", function funcionContinuar() {
				justificacionCambiosServicio.guardarCambiosFirme(vm.paramJustificacion).then(function (response) {

					if (response.statusText === "OK" || response.status === 200) {
						parent.postMessage("cerrarModal", window.location.origin);
						utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

						//document.getElementById("Justificacion-justificacionLocalizacion").value = vm.JustificacionLoc;
						vm.JustificacionLoc = document.getElementById("Justificacionlocalizacion1").value;
						vm.Justificacionoriginal = vm.JustificacionLoc;
						$sessionStorage.Justificacionoriginal = vm.Justificacionoriginal;
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						//$("#btn-editar-edicion1").attr('disabled', false);
						//$("#btn-cancelar-edicion").attr('disabled', true);
						$("#btn-guardar-edicion").attr('disabled', true);
						vm.ClasesbtnGuardar = "btn btnguardarLocalizacion";
						vm.editar('');
						vm.ejecutarErrores();
					} else {
						swal('', response.data.Mensaje, 'error');
					}
				});

			}, function funcionCancelar(reason) {
				console.log("reason", reason);
			});

		}

		//validacion errores en capitulos

		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Localizacion");
			vm.limpiarErrores();
			if (errores != undefined) {
				var isValid = true;
				var erroresLocalizacion = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				if (erroresLocalizacion != undefined) {
					var erroresJson = erroresLocalizacion.Errores == "" ? [] : JSON.parse(erroresLocalizacion.Errores);
					var isValid = (erroresJson == null || erroresJson.length == 0);
				}
				if (!isValid) {
					erroresJson[vm.nombreComponente].forEach(p => {
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


		vm.validarErroresActivos = function (codError) {
			if (vm.erroresActivos != null) {
				vm.erroresActivos = vm.erroresActivos.filter(function (value, index, arr) {
					return value.Error != codError;
				});
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: (vm.erroresActivos.length <= 0) });
			}
		}


		vm.validarJustificacion = function (errores, descripcion) {
			var campoObligatorioJustificacionLoc = document.getElementById("datosGenerales-justificacion-error");
			var ValidacionJustificacionLoc = document.getElementById("datosGenerales-justificacion-error-mns");

			if (campoObligatorioJustificacionLoc != undefined) {
				if (ValidacionJustificacionLoc != undefined) {
					ValidacionJustificacionLoc.innerHTML = '<span>' + descripcion + "</span>";
					campoObligatorioJustificacionLoc.classList.remove('hidden');
				}
			}
		}


		vm.limpiarErrores = function () {
			var campoObligatorioJustificacionLoc = document.getElementById("datosGenerales-justificacion-error");
			var ValidacionJustificacionLoc = document.getElementById("datosGenerales-justificacion-error-mns");
			if (ValidacionJustificacionLoc != undefined) {
				ValidacionJustificacionLoc.innerHTML = '';
				campoObligatorioJustificacionLoc.classList.add('hidden');
			}
		}

		vm.ejecutarErrores = function () {
			if (vm.erroresActivos != null && vm.erroresActivos != "") {
				vm.erroresActivos.forEach(p => {
					if (vm.errores[p.Error] != undefined) {
						vm.errores[p.Error](p.Error, p.Descripcion);
					}
				});
			}
		}


		vm.errores = {
			'JUST001': vm.validarJustificacion,
			'JUST002': vm.validarJustificacion
		}

	}

	angular.module('backbone').component('localizacionJustificacion', {
		templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/localizacionJustificacion/localizacionJustificacion.html",
		controller: localizacionJustificacionController,
		controllerAs: "vm",
		bindings: {
			justificacioncapitulo: '@',
			notificacioncambios: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			guardadoevent: '&',
		}
	});

})();