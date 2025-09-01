(function () {
	'use strict';

	justificacionAlDetalleController.$inject = [
		'$sessionStorage',
		'aclaracionLeyendaServicio',
		'justificacionCambiosServicio',
		'utilidades',
		'$scope',
	];

	function justificacionAlDetalleController(
		$sessionStorage,
		aclaracionLeyendaServicio,
		justificacionCambiosServicio,
		utilidades,
		$scope,
	) {
		this.esEdicion = false;
		this.nombreComponente = "justificacionjustificacionmo";
		this.idProyecto = null;
		this.idInstancia = $sessionStorage.idInstancia;
		// this.tramiteid = $sessionStorage.tramiteId;
		this.modificacionLeyenda = null;
		this.seccionCapitulo = null;

		this.errorTranscripcion = false;
		this.errorAritmetico = false;
		this.sustentoModificacion = '';

		this.errorTipoModificacion = false;
		this.errorSustentoModificacion = false;
		this.mensajesError = [];

		this.ActivarEditar = (vieneGuardado) => {
			this.esEdicion = !this.esEdicion;
			$("#EditarJ").html(this.esEdicion ? "CANCELAR" : "EDITAR");
			if (!this.esEdicion && vieneGuardado == false)
				this.ObtenerModificacionLeyenda();
		}

		this.ObtenerModificacionLeyenda = () => {
			if ((this.tramiteid != null) && (this.idProyecto != null)) {
				aclaracionLeyendaServicio.obtenerModificacionLeyenda(this.tramiteid, $sessionStorage.proyectoId)
					.then((respuesta) => {
						if (respuesta != null && respuesta != "") {
							this.modificacionLeyenda = respuesta;
							this.errorTranscripcion = this.modificacionLeyenda.ErrorTranscripcion ? this.modificacionLeyenda.ErrorTranscripcion: false;
							this.errorAritmetico = this.modificacionLeyenda.ErrorAritmetico ? this.modificacionLeyenda.ErrorAritmetico: false;
							this.sustentoModificacion = this.modificacionLeyenda.Justificacion;
						}
					})
					.catch((error) => { new utilidades.mensajeError("Error al realizar la operación"); console.error(error); });
			}
		}

		this.init = () => {
			
			$scope.$watch( () => $sessionStorage.proyectoId
			 , (newVal, oldVal) => {
				if(newVal){
					this.idProyecto = $sessionStorage.proyectoId;
					this.ObtenerModificacionLeyenda();
				}
			 }, true);
			//validacion
			this.notificacionvalidacion({ handler: this.notificacionValidacionPadre, nombreComponente: this.nombreComponente, esValido: true });
		};

		this.limpiarValidacion = () => {
			this.errorTipoModificacion = false;
			this.errorSustentoModificacion = false;
		}

		this.validar = () => {
			this.limpiarValidacion();
			var errorValidacion = false;
			var mensajesError = new Set();
			if (!this.errorTranscripcion && !this.errorAritmetico) {
				this.errorTipoModificacion = true;
				errorValidacion = true;
				mensajesError.add("Falta campo tipo de modificación");
			}

			if (this.sustentoModificacion == null || this.sustentoModificacion.length == 0) {
				this.errorSustentoModificacion = true;
				errorValidacion = true;
				mensajesError.add("Falta campo sustento de la modificación");
			}

			return { errorValidacion, mensajesError: Array.from(mensajesError) };
		}

		this.ObtenerSeccionCapitulo =()=> {
            const span = document.getElementById(`id-capitulo-${this.nombreComponente}`);
            this.seccionCapitulo = span.textContent;
        }

        this.guardarCapituloModificado =() => {
           this.ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: this.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then( (response) => {
                    if (response.data.Exito) {
                        this.guardadoevent({ nombreComponenteHijo: this.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
				})
				.catch(error => console.log(error));
        }

		this.guardar = () => {
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

		this.limpiarErrores = (errores) => {

			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-pregunta-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}

			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-modificacion-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}

			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-justificacion-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}

			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-campos-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "";
				campoObligatorioJustificacion.classList.add('hidden');
			}

			//if (vm.Justificaciones !== undefined)
			//	vm.Justificaciones.forEach(p => {
			//		var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-" + p.JustificacionPreguntaId);
			//		if (campoObligatorioProyectos != undefined) {
			//			campoObligatorioProyectos.innerHTML = "";
			//			campoObligatorioProyectos.classList.add('hidden');
			//		}
			//	}
			//	);
		}

		this.notificacionValidacionPadre = (errores) => {
			//console.log("Validación  - CD Pvigencias futuras");
			this.limpiarErrores(errores);
			var isValid = true;
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == this.nombreComponente);
				var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				if (erroresJson != undefined) {
					isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson[this.nombreComponente].forEach(p => {

							if (this.errores[p.Error] != undefined) this.errores[p.Error](p.Descripcion);
						});
					}
				}
				this.notificacionestado({ nombreComponente: this.nombreComponente, esValido: isValid });
			}
		}

		this.validarValoresVigenciaJustificacionModificacion = (errores) => {
			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-modificacion-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}

		this.validarValoresVigenciaJustificacionDiligenciarJustificacion = (errores) => {
			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-justificacion-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}

		this.validarValoresVigenciaJustificacionDiligenciarCampos = (errores) => {

			var campoObligatorioJustificacion = document.getElementById(this.nombreComponente + "-campos-error");
			if (campoObligatorioJustificacion != undefined) {
				campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
				campoObligatorioJustificacion.classList.remove('hidden');
			}
		}

		this.errores = {
			'AL006': this.validarValoresVigenciaJustificacionModificacion,
			'AL007': this.validarValoresVigenciaJustificacionDiligenciarJustificacion,
			'AL008': this.validarValoresVigenciaJustificacionDiligenciarCampos,

		}

		/* ------------------------ FIN Validaciones ---------------------------------*/



	}

	angular.module('backbone').component('justificacionAlDetalle', {
		templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/justificacionAl/justificacionAlDetalle/justificacionAlDetalle.html",
		controller: justificacionAlDetalleController,
		bindings: {
			callback: '&',
			tramiteid: '@',
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',		},
	});

})();