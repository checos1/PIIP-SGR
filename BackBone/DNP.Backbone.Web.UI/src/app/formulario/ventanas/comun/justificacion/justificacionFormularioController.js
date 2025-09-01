(function () {
	'use strict';

	justificacionFormularioController.$inject = [
		'$sessionStorage',
		'aclaracionLeyendaServicio',
		'justificacionCambiosServicio',
		'utilidades',
		'$scope',
		'comunesServicio',
		'constantesBackbone'
	];

	function justificacionFormularioController(
		$sessionStorage,
		aclaracionLeyendaServicio,
		justificacionCambiosServicio,
		utilidades,
		$scope,
		comunesServicio,
		constantesBackbone
	) {
		var vm = this;
		this.esEdicion = false;
		this.nombreComponente = "justificacionjustificacion";
		this.idProyecto = null;
		this.idInstancia = $sessionStorage.idInstancia;
		vm.initJustificacionFormulario = initJustificacionFormulario;
		vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
		// this.tramiteid = $sessionStorage.tramiteId;
		vm.etapa = "ej";
		vm.seccionCapitulo = null;
		vm.listaPreguntasProyectos = [];
		vm.listaPreguntasTramite = [];
		vm.IdNivel = $sessionStorage.idNivel;
		vm.disabledJustificacion = true;
		
		vm.modelo = {
			coleccion: "proyectos", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
			idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: $sessionStorage.TipoTramiteId, allArchivos: $sessionStorage.allArchivosTramite,
			BPIN: $sessionStorage.BPIN
		}

		
		$scope.$watch('vm.actualizacomponentes', function () {
			if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
				obtenerPreguntas(vm.tramiteid, vm.tipotramiteid, 0);
				ObtenerPreguntasProyectos(vm.tramiteid);
			}
		});
			

		function initJustificacionFormulario () {

			$scope.$watch('vm.tramiteid', function () {
				if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
					obtenerPreguntas(vm.tramiteid, vm.tipotramiteid, 0);
					ObtenerPreguntasProyectos(vm.tramiteid);

				}
			});
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			
		}

		vm.mostrarOcultarPreguntasProyecto = function (objeto) {
			var variable = $("#ico" + objeto).attr("src");

			if (variable === "Img/btnMas.svg") {
				$("#ico" + objeto).attr("src", "Img/btnMenos.svg");
			}
			else {
				$("#ico" + objeto).attr("src", "Img/btnMas.svg");
			}
		}

		vm.mostrarOcultarPreguntasDeCadaProyecto = function (objeto) {
			var variable = $("#ico" + objeto).attr("src");

			if (variable === "Img/btnMas.svg") {
				$("#ico" + objeto).attr("src", "Img/btnMenos.svg");
			}
			else {
				$("#ico" + objeto).attr("src", "Img/btnMas.svg");
			}
		}

		vm.mostrarOcultarPreguntasTramite = function (objeto) {
			var variable = $("#ico" + objeto).attr("src");

			if (variable === "Img/btnMas.svg") {
				$("#ico" + objeto).attr("src", "Img/btnMenos.svg");
			}
			else {
				$("#ico" + objeto).attr("src", "Img/btnMas.svg");
			}
		}

		vm.verNombreCompleto = function (idVerMas, idProyecto) {
			if (document.getElementById(idVerMas).classList.contains("proyecto-nombreFP")) {
				document.getElementById(idVerMas).classList.remove("proyecto-nombreFP");
				document.getElementById(idVerMas).classList.add("proyecto-nombreFP-completo");
				document.getElementById(idVerMas).innerText = vm.listaPreguntasProyectos.find(w => w.ProyectoId == idProyecto).NombreProyecto;
				document.getElementById("btnVerMasNombreJustifica-" + idProyecto).innerText = "Ver menos"
			} else {
				document.getElementById(idVerMas).classList.remove("proyecto-nombreFP-completo");
				document.getElementById(idVerMas).classList.add("proyecto-nombreFP");
				document.getElementById(idVerMas).innerText = vm.listaPreguntasProyectos.find(w => w.ProyectoId == idProyecto).NombreCorto;
				document.getElementById("btnVerMasNombreJustifica-" + idProyecto).innerText = "Ver mas"
			}
		}

		vm.ActivarEditar = function () {
			if (vm.disabledJustificacion == true) {
				var paneltmp = document.getElementById('EditarJustificacion');
				paneltmp.innerText = "CANCELAR";
				vm.disabledJustificacion = false;

			}
			else {
				var paneltmp = document.getElementById('EditarJustificacion');
				paneltmp.innerText = "EDITAR";
				vm.Justificaciones = angular.copy(vm.Preguntas);
				vm.disabledJustificacion = true;

			}
		}

		vm.registrorespuesta = function (response) {
			let respuestas = {};
			if (response !== undefined) {
				respuestas = response.JustificacionRespuesta;
			}


			for (const element of vm.Justificaciones) {

				for (const [key, value] of Object.entries(respuestas)) {
					if (element.JustificacionPreguntaId == key) {
						element.JustificacionRespuesta = value;

					}

				}
				element.NivelId = vm.IdNivel;
				element.InstanciaId = $sessionStorage.idInstanciaIframe;
				element.ProyectoId = null;

			}

			return comunesServicio.guardarRespuestasJustificacion(vm.Justificaciones).then(
				function (response) {
					if (response.data && (response.statusText === "OK" || response.status === 200)) {

						if (vm.Justificaciones.filter(p => p.JustificacionRespuesta !== null && p.JustificacionRespuesta !== "").length > 0) {
							guardarCapituloModificado();
						}
						else {
							eliminarCapitulosModificados();
						}



						if (response.data.Exito) {
							var paneltmp = document.getElementById('EditarJustificacion');
							paneltmp.innerText = "EDITAR";
							vm.disabledJustificacion = true;
							utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
						} else {
							swal('', response.data.Mensaje, 'warning');
						}

					} else {
						swal('', "Error al realizar la operación", 'error');
					}
				}

			);


		}


		function obtenerPreguntas(idTramite, tipoTramiteId, tipoRolId) {

			return comunesServicio.obtenerPreguntasJustificacion(idTramite, 0, tipoTramiteId, tipoRolId, vm.IdNivel).then(
				function (respuesta) {
					vm.Preguntas = respuesta.data;
					vm.Justificaciones = respuesta.data;
					if (vm.Justificaciones[0].NombreUsuario !== null) {
						let fecha = new Date(vm.Justificaciones[0].FechaEnvio).toLocaleDateString();
						vm.Usuario = vm.Justificaciones[0].NombreUsuario + " " + vm.Justificaciones[0].Cuenta + ".  Fecha  de envío: " + fecha;
					}
				});

		}

		function ObtenerPreguntasProyectos() {
			var TipoProyecto = $sessionStorage.TipoProyecto;
			vm.tipoJustificacion = "Justificacion del " + TipoProyecto;
			return comunesServicio.obtenerPreguntastramiteProyectos(vm.tramiteid, vm.tipotramiteid, vm.idnivel, 0).then(
				function (respuesta) {
					if (vm.IdNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos)
						vm.listaPreguntasProyectos = respuesta.data.filter(c => c.Paso !== 'Viabilidad sectorial' || c.Paso !== 'Viabilidad definitiva');
					else
						vm.listaPreguntasProyectos = respuesta.data;
					vm.listaPreguntasProyectos.map(function (item) {
						item.NombreCorto = mapearNombreProyecto(item.NombreProyecto);
					});
				});

		}

		function mapearNombreProyecto(nombreProyecto) {
			if (nombreProyecto !== undefined && nombreProyecto.length > 80)
				return nombreProyecto.substring(0, 80);
			else
				return nombreProyecto;

		}

		function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-'+ vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado ()  {
           ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
			comunesServicio.guardarCambiosFirme(data)
                .then( (response) => {
                    if (response.data.Exito) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
				})
				.catch(error => console.log(error));
		}

		function eliminarCapitulosModificados() {
			ObtenerSeccionCapitulo();
			var data = {
				//ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
				Justificacion: "",
				SeccionCapituloId: vm.seccionCapitulo,
				InstanciaId: $sessionStorage.idInstancia,

			}
			justificacionCambiosServicio.eliminarCapitulosModificados(data)
				.then(function (response) {
					if (response.data.Exito) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
					}
					else {
						utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
					}
				});
		}

		function guardar () {
			if(this.modificacionLeyenda == null) {
				utilidades.mensajeError('No hay un proyecto asociada', () => { }, 'Hay datos que presentan inconsistencias');
				return;
			}
			var modificacionLeyenda = {
				TramiteProyectoId: this.modificacionLeyenda.TramiteProyectoId,
				justificacion: this.sustentoModificacion,
				errorAritmetico: this.errorAritmetico,
				errorTranscripcion: this.errorTranscripcion,
				tipoUpdate: 2,
			}
			this.mensajesError = [];
			const validar = this.validar();
			if (validar.errorValidacion) {
				utilidades.mensajeError('revise los campos señalados y valide nuevamente.', () => { }, 'Hay datos que presentan inconsistencias');
				this.mensajesError = validar.mensajesError;
				return;
			};

			aclaracionLeyendaServicio.actualizaModificacionLeyenda(modificacionLeyenda)
				.then((response) => {
					if (response.data === "OK") {
						new utilidades.mensajeSuccess("Los datos han sido guardados con exito", false, false, false);
						this.guardarCapituloModificado();
						this.ActivarEditar(true);
					}
					else {
						new utilidades.mensajeError("Error al realizar la operación");
					}
				})
				.catch((error) => { new utilidades.mensajeError("Error al realizar la operación"); console.error(error); });
		}

		/* ------------------------ Validaciones ---------------------------------*/

		vm.limpiarErrores = function (errores) {
			var campoObligatorioJustificacion = document.getElementById("VFO005-pregunta-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}

			var idSpanAlertComponent = document.getElementById("alert-Justificaciondet");
			if (idSpanAlertComponent != undefined) {
				idSpanAlertComponent.classList.remove("ico-advertencia");

			}
			if (vm.Justificaciones !== undefined)
				vm.Justificaciones.forEach(p => {
					var campoObligatorioProyectos = document.getElementById("VFO005-" + p.JustificacionPreguntaId);
					if (campoObligatorioProyectos != undefined) {
						campoObligatorioProyectos.innerHTML = "";
						campoObligatorioProyectos.classList.add('hidden');
					}
				}
				);
		}

		vm.notificacionValidacionPadre = function (errores) {

			vm.limpiarErrores(errores);
			var isValid = true;
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				if (erroresJson != undefined) {
					isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson[vm.nombreComponente].forEach(p => {

							if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
						});
					}
				}
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
			}
		}



		vm.validarValoresVigenciaInformacionPresupuestalJustificacion = function (errores) {
			var campoObligatorioJustificacion = document.getElementById("VFO005-pregunta-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}



		vm.validarValoresVigenciaInformacionPresupuestalPregunta = function (errores) {
			var campoObligatorioJustificacion = document.getElementById("VFO005-" + errores);
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
			var idSpanAlertComponent = document.getElementById("alert-Justificaciondet");
			if (idSpanAlertComponent != undefined) {
					idSpanAlertComponent.classList.add("ico-advertencia");
				
			}
		}

		vm.errores = {
			'VFO005': vm.validarValoresVigenciaInformacionPresupuestalJustificacion,
			'VFO005-': vm.validarValoresVigenciaInformacionPresupuestalPregunta,




		}

		/* ------------------------ FIN Validaciones ---------------------------------*/



	}

	angular.module('backbone').component('justificacionFormulario', {
		templateUrl: "src/app/formulario/ventanas/comun/justificacion/justificacionFormulario.html",
		controller: justificacionFormularioController,
		controllerAs: "vm",
		bindings: {
			callback: '&',
			tramiteid: '@',
			tipotramiteid: '@',
			idnivel: '@',
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			nombrecomponentepaso: '@',
			actualizacomponentes: '@',

		},
	});

})();