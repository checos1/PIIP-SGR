(function () {
	'use strict';

	vigfuturavalconstantesController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'vigfuturavalconstantesServicio',
		'utilidades',
		'justificacionCambiosServicio',
		'ajustesolicitudvigfuturaServicio',
		'vigfuturavalcorrientesServicio',
	];



	function vigfuturavalconstantesController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		vigfuturavalconstantesServicio,
		utilidades,
		justificacionCambiosServicio,
		ajustesolicitudvigfuturaServicio,
		vigfuturavalcorrientesServicio

	) {

		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "ajustesvigenciasfuturasvaloresconstantes";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.isEdicion = false;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.idFlujo = $sessionStorage.idFlujoIframe;
		vm.idNivel = $sessionStorage.idNivel;
		vm.TramiteId = $sessionStorage.TramiteId;
		vm.abrirMensajeInformacion = abrirMensajeInformacion;
		vm.ObtenerFuentesFinanciacionVigenciaFuturaConstante = ObtenerFuentesFinanciacionVigenciaFuturaConstante;
		vm.ConvertirNumero = ConvertirNumero;
		vm.ConvertirNumeroCuatro = ConvertirNumeroCuatro;
		vm.vigenciaActual = new Date().getFullYear();
		vm.Cancelar = Cancelar;
		vm.habilitaEditar = habilitaEditar;
		vm.Guardar = Guardar;
		vm.erroresActivosArray = [];
		vm.SeccionCapituloId = 0;
		//Inicio

		vm.parametros = {


		};

		vm.init = function () {
			//ObtenerFuentesFinanciacionVigenciaFuturaConstante();
			vm.refresh({ handler: vm.refreshData, nombreComponente: vm.nombreComponente });
			vm.errores({ handler: vm.showErrores, nombreComponente: vm.nombreComponente });
			vm.vigenciaActual = new Date().getFullYear();
		};

		$scope.$watch('vm.aniobase', function () {
			if (vm.aniobase && vm.aniobase !== null && vm.aniobase !== '') {
				ObtenerFuentesFinanciacionVigenciaFuturaConstante(vm.aniobase);
			}
		});

		vm.refreshData = function () {
			vm.vigenciaActual = new Date().getFullYear();
			ObtenerFuentesFinanciacionVigenciaFuturaConstante(vm.aniobase);
			vm.limpiarErrores();
		}

		vm.showErrores = function (listErrores) {
			console.log("ajustesvigenciasfuturasvaloresconstantes", listErrores)
			vm.limpiarErrores();
			vm.erroresActivosArray = listErrores.erroresActivos;
			vm.erroresActivosArray.forEach(p => {
				if (vm.erroresActivos[p.Error] != undefined) vm.erroresActivos[p.Error](p.Descripcion, p.Data);
			});

		}

		vm.AVFFFD001 = function (descripcionError, dataError) {
			var idSeccion = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001");
			var idMensajeError = document.getElementById(vm.nombreComponente + "-validacionAVFFFD001-error-mns");
			if (idSeccion != undefined && idSeccion != null && idMensajeError != undefined && idMensajeError != null) {
				idMensajeError.innerHTML = descripcionError;
				idSeccion.classList.remove("hidden");

				/* Recorre listado de fuentes*/
				var listadoFuentes = JSON.parse(dataError);
				listadoFuentes.map(item => {
					var fuenteHtml = document.getElementById("fuente-vigencia-futura-constante-" + item.FuenteId);
					if (fuenteHtml != undefined && fuenteHtml != null) {
						fuenteHtml.classList.remove('hidden');
						if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");

						/* Recorre listado de vigencias*/
						var listadoVigencias = item["ListadoVigencias"];
						if (listadoVigencias != undefined && listadoVigencias != null) {
							listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
							listadoVigencias.map(itemVig => {
								var vigenciaHtml = document.getElementById("vigencia-futura-constante-" + item.FuenteId + '-' + itemVig.Vigencia);
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

		vm.AVFFFD004 = function (descripcionError, dataError) {
			/* Recorre listado de fuentes*/
			var listadoFuentes = JSON.parse(dataError);
			listadoFuentes.map(item => {
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-constante-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-constante-" + item.FuenteId + "-errorAVFFFD002-mns");
				if (fuenteHtml != undefined && fuenteHtml != null && fuentemnsHtml != undefined && fuentemnsHtml != null) {
					fuenteHtml.classList.remove('hidden');
					if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");
					fuentemnsHtml.innerHTML = descripcionError;

					/* Recorre listado de vigencias*/
					var listadoVigencias = item["ListadoVigencias"];
					if (listadoVigencias != undefined && listadoVigencias != null) {
						listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
						listadoVigencias.map(itemVig => {
							var vigenciaHtml = document.getElementById("vigencia-futura-constante-" + item.FuenteId + '-' + itemVig.Vigencia);
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

		vm.AVFFFD005 = function (descripcionError, dataError) {
			/* Recorre listado de fuentes*/
			var listadoFuentes = JSON.parse(dataError);
			listadoFuentes.map(item => {
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-constante-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-constante-" + item.FuenteId + "-errorAVFFFD003-mns");
				if (fuenteHtml != undefined && fuenteHtml != null && fuentemnsHtml != undefined && fuentemnsHtml != null) {
					fuenteHtml.classList.remove('hidden');
					if (!fuenteHtml.classList.contains("d-inline-block")) fuenteHtml.classList.add("d-inline-block");
					fuentemnsHtml.innerHTML = descripcionError;

					/* Recorre listado de vigencias*/
					var listadoVigencias = item["ListadoVigencias"];
					if (listadoVigencias != undefined && listadoVigencias != null) {
						listadoVigencias = listadoVigencias.filter(p => p.FuenteId == item.FuenteId);
						listadoVigencias.map(itemVig => {
							var vigenciaHtml = document.getElementById("vigencia-futura-constante-" + item.FuenteId + '-' + itemVig.Vigencia);
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
				var fuenteHtml = document.getElementById("fuente-vigencia-futura-constante-" + item.FuenteId);
				var fuentemnsHtml = document.getElementById("fuente-futura-constante-" + item.FuenteId + "-errorAVFFFD002-mns");
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
							var vigenciaHtml = document.getElementById("vigencia-futura-constante-" + item.FuenteId + '-' + itemVig.Vigencia);
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
				if ( vm.erroresActivosArray.length > 0) {
					vm.erroresActivosArray.forEach(p => {
						vm.limpiarAVFFD00(p.Data);
					});
				}
			}
		}

		function abrirMensajeInformacion() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");

		}

		function ObtenerFuentesFinanciacionVigenciaFuturaConstante(AnioBase) {
			return vigfuturavalconstantesServicio.ObtenerFuentesFinanciacionVigenciaFuturaConstante($sessionStorage.idInstancia, AnioBase).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						//console.log(respuesta.data);
						$scope.datosConstantes = respuesta.data;
					}
				});
		}

		function ConvertirNumero(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 2,
			}).format(numero);
		}

		function ConvertirNumeroCuatro(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 4,
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
				fuente.ValorTotalVigenciaFuturaCorriente = fuente.ValorTotalVigenciaFuturaCorrienteOriginal;
				angular.forEach(fuente.Vigencias, function (series) {
					series.ValorVigenteFutura = parseFloat(series.ValorVigenteFuturaOriginal).toFixed(2);
					series.ValorVigenteFuturaCorriente = parseFloat(series.ValorVigenteFuturaCorrienteOriginal).toFixed(2);
				});
				fuente.ValorTotalVigenciaFuturas = fuente.ValorTotalVigenciaFuturasOriginal;

				var acumulaFuturas = 0;
				angular.forEach($scope.datosConstantes.Fuentes, function (fuente) {
					acumulaFuturas = acumulaFuturas + parseFloat(fuente.ValorTotalVigenciaFuturas);
				});
				$scope.datosConstantes.ValorTotalVigenteFutura = parseFloat(acumulaFuturas.toFixed(2));
				$scope.datosConstantes.ValorPorcentaje = parseFloat(($scope.datosConstantes.ValorTotalVigenteFutura * 0.15).toFixed(2));
				if ($scope.datosConstantes.ValorTotalVigente > $scope.datosConstantes.ValorPorcentaje) {
					$scope.datosConstantes.cumple = true;
				} else {
					$scope.datosConstantes.cumple = false;
				}

				vigfuturavalconstantesServicio.ObtenerFuentesFinanciacionVigenciaFuturaConstante(vm.codigoBpin, vm.aniobase).then(
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
				series.ValorVigenteFuturaCorriente = parseFloat(series.ValorVigenteFuturaCorriente).toFixed(2);
			});
		}

		function Guardar(fuente) {
			var validar = false;
			angular.forEach(fuente.Vigencias, function (series) {
				if (series.ValorVigenteFutura == "" || series.ValorVigenteFutura == null) {
					series.ValorVigenteFutura = 0.00;
				} else {
					if (series.ValorVigenteFuturaCorriente > series.ApropiacionVigente) {
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
			fuente.proyectoId = vm.idProyecto;

			var tramiteProyectoDto = {
				ProyectoId: vm.idProyecto,
				TramiteId: vm.TramiteId,
				EsConstante: 1,
				AnioBase: vm.aniobase
			};

			ajustesolicitudvigfuturaServicio.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto).then(function (response) {
				if (response.data && (response.statusText === "OK" || response.status === 200)) {

				} else {
					new utilidades.mensajeError("Error al realizar la operación");
				}
			});


			return vigfuturavalcorrientesServicio.actualizarVigenciaFuturaFuente(fuente).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						guardarCapituloModificado();
						utilidades.mensajeSuccess("", false, false, false, "Los datos para valores constantes fueron guardados con éxito");
						fuente.HabilitaEditarFuente = false;
						fuente.ValorTotalVigenciaFuturaOriginal = fuente.ValorTotalVigenciaFutura;
						fuente.ValorTotalVigenciaFuturaCorrienteOriginal = fuente.ValorTotalVigenciaFuturaCorriente;
						angular.forEach(fuente.Vigencias, function (series) {
							series.ValorVigenteFuturaOriginal = parseFloat(series.ValorVigenteFutura).toFixed(2);
							series.ValorVigenteFuturaCorrienteOriginal = parseFloat(series.ValorVigenteFuturaCorriente).toFixed(2);
						});
						vm.init();
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
				if (decimales > 2) {
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
			var acumulaCorriente = 0;
			var acumulaFutura = 0;
			angular.forEach(fuente.Vigencias, function (series) {
				series.ValorVigenteFuturaCorriente = series.ValorVigenteFutura * parseFloat(series.Deflactor.toFixed(4));
				series.ValorVigenteFuturaCorriente = parseFloat(series.ValorVigenteFuturaCorriente.toFixed(2));
				acumula = acumula + parseFloat(series.ValorVigenteFutura);
				acumulaCorriente = acumulaCorriente + parseFloat(series.ValorVigenteFuturaCorriente);
				
				if (series.Vigencia > vm.vigenciaActual) {
					acumulaFutura = acumulaFutura + parseFloat(series.ValorVigenteFuturaCorriente);
					
				}
				fuente.ValorTotalVigenciaFutura = parseFloat(acumula.toFixed(2));
				fuente.ValorTotalVigenciaFuturaCorriente = parseFloat(acumulaCorriente.toFixed(2));
				fuente.ValorTotalVigenciaFuturas = acumulaFutura;


				var acumulaFuturas = 0;
				angular.forEach($scope.datosConstantes.Fuentes, function (fuente) {
					acumulaFuturas = acumulaFuturas + parseFloat(fuente.ValorTotalVigenciaFuturas);
				});
				$scope.datosConstantes.ValorTotalVigenteFutura = parseFloat(acumulaFuturas.toFixed(2));
				$scope.datosConstantes.ValorPorcentaje = parseFloat(($scope.datosConstantes.ValorTotalVigenteFutura * 0.15).toFixed(2));
				if ($scope.datosConstantes.ValorTotalVigente > $scope.datosConstantes.ValorPorcentaje) {
					$scope.datosConstantes.cumple = true;
				} else {
					$scope.datosConstantes.cumple = false;
				}
			});

			const val = event.target.value;
			const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
			var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
			return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
		}

		vm.erroresActivos = {
			'AVFFFD001': vm.AVFFFD001,
			'AVFFFD004': vm.AVFFFD004,
			'AVFFFD005': vm.AVFFFD005,
		}

		
	}

	angular.module('backbone').component('vigfuturavalconstantes', {

		templateUrl: "src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresConstantes/vigfuturavalconstantes.html",
		controller: vigfuturavalconstantesController,
		controllerAs: "vm",
		bindings: {
			aniobase: '@',
			refresh: '&',
			errores: '&',
			guardadoevent: '&'
		}
	});

})();