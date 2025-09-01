(function () {
	'use strict';

	vigfuturavalcorrientesController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'vigfuturavalcorrientesServicio',
		'utilidades',
		'justificacionCambiosServicio'
	];



	function vigfuturavalcorrientesController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		vigfuturavalcorrientesServicio,
		utilidades,
		justificacionCambiosServicio

	) {

		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "ajustesvigenciasfuturasvalorescorrientes";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.isEdicion = false;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.idFlujo = $sessionStorage.idFlujoIframe;
		vm.idNivel = $sessionStorage.idNivel;
		vm.TramiteId = $sessionStorage.TramiteId;
		vm.abrirMensajeInformacion = abrirMensajeInformacion;
		vm.ObtenerFuentesFinanciacionVigenciaFuturaCorriente = ObtenerFuentesFinanciacionVigenciaFuturaCorriente;
		vm.ConvertirNumero = ConvertirNumero;
		vm.vigenciaActual = new Date().getFullYear();
		vm.Cancelar = Cancelar;
		vm.habilitaEditar = habilitaEditar;
		vm.Guardar = Guardar;
		vm.erroresActivosArray = [];
		//Inicio
		vm.SeccionCapituloId = 0;

		vm.parametros = {


		};

		vm.init = function () {
			vm.refresh({ handler: vm.refreshData, nombreComponente: vm.nombreComponente });
			vm.errores({ handler: vm.showErrores, nombreComponente: vm.nombreComponente });
			vm.refreshData();
		};

		function abrirMensajeInformacion() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");

		}

		vm.refreshData = function () {
			ObtenerFuentesFinanciacionVigenciaFuturaCorriente();
			vm.vigenciaActual = new Date().getFullYear();
			vm.limpiarErrores();
		}

		vm.showErrores = function (listErrores) {
			//console.log("ajustesvigenciasfuturasvalorescorrientes", listErrores)
			vm.limpiarErrores();
			vm.erroresActivosArray = listErrores.erroresActivos;
			vm.erroresActivosArray.forEach(p => {
				if (vm.erroresActivos[p.Error] != undefined) vm.erroresActivos[p.Error](p.Descripcion, p.Data);
			});

		}

		vm.AVFFFD001 = function(descripcionError, dataError) {
			var idSeccion = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001");
			var idMensajeError = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001-error-mns");
			if (idSeccion != undefined && idSeccion != null && idMensajeError != undefined && idMensajeError != null) {
				idMensajeError.innerHTML = descripcionError;
				idSeccion.classList.remove("hidden");

				/* Recorre listado de fuentes*/
				var listadoFuentes = JSON.parse(dataError);
				listadoFuentes.map(item => {
					var fuenteHtml = document.getElementById("fuente-vigencia-futura-corriente-" + item.FuenteId);
					if (fuenteHtml != undefined && fuenteHtml != null) {
						fuenteHtml.classList.remove('hidden');
						if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");

						/* Recorre listado de vigencias*/
						var listadoVigencias = item["ListadoVigencias"];
						if (listadoVigencias != undefined && listadoVigencias != null) {
							listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
							listadoVigencias.map(itemVig => {
								var vigenciaHtml = document.getElementById("vigencia-futura-corriente-" + item.FuenteId + '-' + itemVig.Vigencia);
								var valorVigenciaHtml = document.getElementById("input-" + item.FuenteId + '-' + itemVig.Vigencia);
								if (vigenciaHtml != undefined && vigenciaHtml != null && valorVigenciaHtml != undefined && valorVigenciaHtml != null) {
									vigenciaHtml.classList.remove('hidden');
									valorVigenciaHtml.classList.add('divInconsistencia');
								}
							});
                        }
                    }
				});
            }
		}

		vm.AVFFFD002 = function(descripcionError, dataError) {
			/* Recorre listado de fuentes*/
			var listadoFuentes = JSON.parse(dataError);
			listadoFuentes.map(item => {
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-corriente-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-corriente-" + item.FuenteId +"-errorAVFFFD002-mns");
				if (fuenteHtml != undefined && fuenteHtml != null && fuentemnsHtml != undefined && fuentemnsHtml != null) {
					fuenteHtml.classList.remove('hidden');
					if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");
					fuentemnsHtml.innerHTML = descripcionError;

					/* Recorre listado de vigencias*/
					var listadoVigencias = item["ListadoVigencias"];
					if (listadoVigencias != undefined && listadoVigencias != null) {
						listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
						listadoVigencias.map(itemVig => {
							var vigenciaHtml = document.getElementById("vigencia-futura-corriente-" + item.FuenteId + '-' + itemVig.Vigencia);
							var valorVigenciaHtml = document.getElementById("input-" + item.FuenteId + '-' + itemVig.Vigencia);
							if (vigenciaHtml != undefined && vigenciaHtml != null && valorVigenciaHtml != undefined && valorVigenciaHtml != null) {
								vigenciaHtml.classList.remove('hidden');
								valorVigenciaHtml.classList.add('divInconsistencia');
							}
						});
					}
				}
			});	
		}

		vm.AVFFFD003 = function (descripcionError, dataError) {
			/* Recorre listado de fuentes*/
			var listadoFuentes = JSON.parse(dataError);
			listadoFuentes.map(item => {
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-corriente-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-corriente-" + item.FuenteId + "-errorAVFFFD003-mns");
				if (fuenteHtml != undefined && fuenteHtml != null && fuentemnsHtml != undefined && fuentemnsHtml != null) {
					fuenteHtml.classList.remove('hidden');
					if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");
					fuentemnsHtml.innerHTML = descripcionError;

					/* Recorre listado de vigencias*/
					var listadoVigencias = item["ListadoVigencias"];
					if (listadoVigencias != undefined && listadoVigencias != null) {
						listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
						listadoVigencias.map(itemVig => {
							var vigenciaHtml = document.getElementById("vigencia-futura-corriente-" + item.FuenteId + '-' + itemVig.Vigencia);
							var valorVigenciaHtml = document.getElementById("input-" + item.FuenteId + '-' + itemVig.Vigencia);
							if (vigenciaHtml != undefined && vigenciaHtml != null && valorVigenciaHtml != undefined && valorVigenciaHtml != null) {
								vigenciaHtml.classList.remove('hidden');
								valorVigenciaHtml.classList.add('divInconsistencia');
							}
						});
					}
				}
			});

		}

		vm.limpiarAVFFD00 = function (dataError) {
			var listadoFuentes = JSON.parse(dataError);
			listadoFuentes.map(item => {
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-corriente-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-corriente-" + item.FuenteId + "-errorAVFFFD002-mns");
				if (fuenteHtml != undefined && fuenteHtml != null && fuentemnsHtml != undefined && fuentemnsHtml != null) {
					if (!fuenteHtml.classList.contains("hidden")) {
						fuenteHtml.classList.add('hidden');
						fuenteHtml.classList.remove('d-inline-block');
                    }
					fuentemnsHtml.innerHTML = "";

					/* Recorre listado de vigencias*/
					var listadoVigencias = item["ListadoVigencias"];
					if (listadoVigencias != undefined && listadoVigencias != null) {
						listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
						listadoVigencias.map(itemVig => {
							var vigenciaHtml = document.getElementById("vigencia-futura-corriente-" + item.FuenteId + '-' + itemVig.Vigencia);
							var valorVigenciaHtml = document.getElementById("input-" + item.FuenteId + '-' + itemVig.Vigencia);
							if (vigenciaHtml != undefined && vigenciaHtml != null && valorVigenciaHtml != undefined && valorVigenciaHtml != null) {
								if (!vigenciaHtml.classList.contains("hidden")) {
									vigenciaHtml.classList.add('hidden');
								}
								valorVigenciaHtml.classList.remove('divInconsistencia');
							}
						});
					}
				}
			});
		}

		vm.limpiarErrores = function () {
			var idSeccion = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001");
			var idMensajeError = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001-error-mns");
			if (idSeccion != undefined && idSeccion != null && idMensajeError != undefined && idMensajeError != null) {
				idMensajeError.innerHTML = '';
				idSeccion.classList.add("hidden");
				if (vm.erroresActivosArray.length > 0) {
					vm.erroresActivosArray.forEach(p => {
						vm.limpiarAVFFD00(p.Data);
					});
				}
			}
		}

		function ObtenerFuentesFinanciacionVigenciaFuturaCorriente() {
			return vigfuturavalcorrientesServicio.ObtenerFuentesFinanciacionVigenciaFuturaCorriente($sessionStorage.idInstancia).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						$scope.datosCorrientes = respuesta.data;
					}
				});
		}

		function ConvertirNumero(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 2,
			}).format(numero);
		}

		vm.BtnFuentes = function (fuente) {
			if (fuente.LabelBotonFuente == '+') {
				fuente.LabelBotonFuente = '-'
			} else {
				fuente.LabelBotonFuente = '+'
			}
			return fuente.LabelBotonFuente;
		}

		function Cancelar(fuente) {

			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

				fuente.HabilitaEditarFuente = false;

				fuente.ValorTotalVigenciaFutura = fuente.ValorTotalVigenciaFuturaOriginal;
				angular.forEach(fuente.Vigencias, function (series) {
					series.ValorVigenteFutura = parseFloat(series.ValorVigenteFuturaOriginal).toFixed(2);
				});
				fuente.ValorTotalVigenciaFuturas = fuente.ValorTotalVigenciaFuturasOriginal;

				var acumulaFuturas = 0;
				angular.forEach($scope.datosCorrientes.Fuentes, function (fuente) {
					acumulaFuturas = acumulaFuturas + parseFloat(fuente.ValorTotalVigenciaFuturas);
				});
				$scope.datosCorrientes.ValorTotalVigenteFutura = parseFloat(acumulaFuturas.toFixed(2));
				$scope.datosCorrientes.ValorPorcentaje = parseFloat(($scope.datosCorrientes.ValorTotalVigenteFutura * 0.15).toFixed(2));
				if ($scope.datosCorrientes.ValorTotalVigente > $scope.datosCorrientes.ValorPorcentaje) {
					$scope.datosCorrientes.cumple = true;
				} else {
					$scope.datosCorrientes.cumple = false;
				}

				vigfuturavalcorrientesServicio.ObtenerFuentesFinanciacionVigenciaFuturaCorriente($sessionStorage.idInstancia).then(
					function (respuesta) {
						if (respuesta.data != null && respuesta.data != "") {
							utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
						}
					});

			}, function funcionCancelar(reason) {
				//poner aquí q pasa cuando cancela
			}, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");		
		}

		function habilitaEditar(fuente) {

			fuente.HabilitaEditarFuente = true;

			angular.forEach(fuente.Vigencias, function (series) {
				series.ValorVigenteFutura = parseFloat(series.ValorVigenteFutura).toFixed(2);
			});
		}

		function Guardar(fuente) {
			var validar = false;
			angular.forEach(fuente.Vigencias, function (series) {
				if (series.ValorVigenteFutura == "" || series.ValorVigenteFutura == null) {
					series.ValorVigenteFutura = 0.00;
				} else {
					if (series.ValorVigenteFutura > series.ApropiacionVigente) {
						series.ValidacionValores = true;
						validar = true;
                    }
                }
			});

			if (validar) {
				utilidades.mensajeError("En esta fuente existen vigencias donde el proyecto no tiene recursos solicitados mayores o iguales al valor solicitado de VF");
				return;
            }

			fuente.TramiteId = vm.TramiteId;
			fuente.ProyectoId = vm.idProyecto;
			fuente.Fuente = "corriente";
			return vigfuturavalcorrientesServicio.actualizarVigenciaFuturaFuente(fuente).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						guardarCapituloModificado();
						utilidades.mensajeSuccess("", false, false, false, "Los datos para valores corrientes fueron guardados con éxito");
						fuente.HabilitaEditarFuente = false;
						fuente.ValorTotalVigenciaFuturaOriginal = fuente.ValorTotalVigenciaFutura;
						angular.forEach(fuente.Vigencias, function (series) {
							series.ValorVigenteFuturaOriginal = parseFloat(series.ValorVigenteFutura).toFixed(4);
						});
						//vm.init();
					} else {
						utilidades.mensajeError("Error al realizar la operación");
					}

				});
		}

		function ObtenerSeccionCapitulo() {
			const span = document.getElementById('id-capitulo-' + 'ajustesvigenciasfuturasajusolvigfutura');
			vm.seccionCapitulo = span.textContent;
		}

		function guardarCapituloModificado() {
			ObtenerSeccionCapitulo();
			var data = {
				ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
				Justificacion: "",
				//SeccionCapituloId: vm.SeccionCapituloId,
				SeccionCapituloId: vm.seccionCapitulo,
				InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
				Modificado: false,
			}
			justificacionCambiosServicio.guardarCambiosFirme(data)
				.then(function (response) {
					if (response.data.Exito) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
					}
					else {
						utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
					}
				});
		}

		vm.validateFormat = function (event) {

			if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
				event.preventDefault();
			}

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 11;
			var tamanio = event.target.value.length;
			var permitido = false;
			permitido = event.target.value.toString().includes(".");
			if (permitido) {
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, 2);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 2) {
						tamanioPermitido = n[0].length + 2;
						event.target.value = n[0] + '.' + n[1].slice(0, 2);
						return;
					}

					if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			} else {
				if (tamanio > 12 && event.keyCode != 44) {
					event.preventDefault();
				}
			}

			if (event.keyCode === 44 && tamanio == 12) {
			}
			else {
				if (tamanio > tamanioPermitido || tamanio > 16) {
					event.preventDefault();
				}
			}
		}

		vm.validarTamanio = function (event) {

			if (Number.isNaN(event.target.value)) {
				event.target.value = "0"
				return;
			}

			if (event.target.value == null) {
				event.target.value = "0"
				return;
			}

			if (event.target.value == "") {
				event.target.value = "0"
				return;
			}

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 11;
			var tamanio = event.target.value.length;
			var permitido = false;
			permitido = event.target.value.toString().includes(".");
			if (permitido) {
				var indicePunto = event.target.value.toString().indexOf(".");
				var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
				if (decimales > 4) {
				}
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, 2);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 2) {
						tamanioPermitido = n[0].length + 2;
						event.target.value = n[0] + '.' + n[1].slice(0, 2);
						return;
					}

					if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			}
		}

		vm.actualizaFila = function (event, fuente) {

			if (Number.isNaN(event.target.value)) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
			}

			if (event.target.value == null) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
			}

			if (event.target.value == "") {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
			}

			event.target.value = parseFloat(event.target.value.replace(",", "."));

			var acumula = 0;
			var acumulaFutura = 0;
			angular.forEach(fuente.Vigencias, function (series) {
				acumula = acumula + parseFloat(series.ValorVigenteFutura);
				if (series.Vigencia > vm.vigenciaActual) {
					acumulaFutura = acumulaFutura + parseFloat(series.ValorVigenteFutura);
				}
			});
			fuente.ValorTotalVigenciaFutura = parseFloat(acumula.toFixed(2));
			fuente.ValorTotalVigenciaFuturas = acumulaFutura;

			var acumulaFuturas = 0;
			angular.forEach($scope.datosCorrientes.Fuentes, function (fuente) {
				acumulaFuturas = acumulaFuturas + parseFloat(fuente.ValorTotalVigenciaFuturas);
			});
			$scope.datosCorrientes.ValorTotalVigenteFutura = parseFloat(acumulaFuturas.toFixed(2));
			$scope.datosCorrientes.ValorPorcentaje = parseFloat(($scope.datosCorrientes.ValorTotalVigenteFutura * 0.15).toFixed(2));
			if ($scope.datosCorrientes.ValorTotalVigente > $scope.datosCorrientes.ValorPorcentaje) {
				$scope.datosCorrientes.cumple = true;
			} else {
				$scope.datosCorrientes.cumple = false;
			}

			const val = event.target.value;
			const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
			var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
			return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
		}

		vm.erroresActivos = {
			'AVFFFD001': vm.AVFFFD001,
			'AVFFFD002': vm.AVFFFD002,
			'AVFFFD003': vm.AVFFFD003,
		}
	}

	angular.module('backbone').component('vigfuturavalcorrientes', {

		templateUrl: "src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresCorrientes/vigfuturavalcorrientes.html",
		controller: vigfuturavalcorrientesController,
		controllerAs: "vm",
		bindings: {
			refresh: "&",
			errores: '&',
			guardadoevent: '&'
		}
	});

})();