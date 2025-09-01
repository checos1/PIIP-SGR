(function () {
	'use strict';

	vigfuturaproductosController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'vigfuturaproductosServicio',
		'utilidades',
		'utilsValidacionSeccionCapitulosServicio',
		'$timeout',
		'ajustesolicitudvigfuturaServicio',
		'justificacionCambiosServicio',
		'$uibModal',
	];



	function vigfuturaproductosController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		vigfuturaproductosServicio,
		utilidades,
		utilsValidacionSeccionCapitulosServicio,
		$timeout,
		ajustesolicitudvigfuturaServicio,
		justificacionCambiosServicio,
		$uibModal,

	) {

		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "ajustesvigenciasfuturasvfproductos";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.isEdicion = false;
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.idFlujo = $sessionStorage.idFlujoIframe;
		vm.idNivel = $sessionStorage.idNivel;
		vm.TramiteId = $sessionStorage.TramiteId;
		vm.vigenciaActual = new Date().getFullYear();
		vm.abrirMensajeInformacion = abrirMensajeInformacion;
		vm.ObtenerProductosVigenciaFuturaConstante = ObtenerProductosVigenciaFuturaConstante;
		vm.pagina = 0;
		vm.cambiarPagina = cambiarPagina;
		vm.ObjetivoVerMas = ObjetivoVerMas;
		vm.ProductoVerMas = ProductoVerMas;
		vm.ConvertirNumero = ConvertirNumero;
		vm.ConvertirNumeroCuatro = ConvertirNumeroCuatro;
		vm.Cancelar = Cancelar;
		vm.habilitaEditar = habilitaEditar;
		vm.Guardar = Guardar;
		vm.tipoValor = null;
		vm.aniobase = null;
		vm.ObtenerProductosVigenciaFuturaCorriente = ObtenerProductosVigenciaFuturaCorriente;
		vm.CancelarCorriente = CancelarCorriente;
		vm.GuardarCorriente = GuardarCorriente;
		vm.erroresActivosArray = [];
		vm.erroresActivos004Array = [];
		vm.erroresActivos = [];

		vm.componentesRefresh = [
			'datosgeneraleshorizonte',
			'recursoscostosdelasacti',
			'ajustesvigenciasfuturasajusolvigfutura',
			'ajustesvigenciasfuturasvaloresconstantes',
			'ajustesvigenciasfuturasvalorescorrientes'
		];

		vm.handlerComponentes = [
			

		];

		//Inicio

		vm.parametros = {


		};

		vm.init = function () {
			ObtenerProyectoTramite();
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
		};

		function ObtenerProyectoTramite() {
			return ajustesolicitudvigfuturaServicio.ObtenerProyectoTramite(vm.idProyecto, vm.TramiteId).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						//console.log(respuesta.data);
						vm.aniobase = respuesta.data[0].AnioBase;
						vm.tipoValor = respuesta.data[0].EsConstante;
						if (respuesta.data[0].EsConstante == true && vm.aniobase != null) {
							ObtenerProductosVigenciaFuturaConstante(vm.aniobase);
						}

						if (respuesta.data[0].EsConstante == false) {
							ObtenerProductosVigenciaFuturaCorriente();
						}
					}
				});
		}

		function ObtenerProductosVigenciaFuturaConstante(AnioBase) {
			return vigfuturaproductosServicio.ObtenerProductosVigenciaFuturaConstante($sessionStorage.idInstancia, vm.TramiteId, AnioBase).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						console.log(respuesta.data);
						$scope.datosConstantes = respuesta.data;
					}
				});
		}

		function ObtenerProductosVigenciaFuturaCorriente() {
			return vigfuturaproductosServicio.ObtenerProductosVigenciaFuturaCorriente($sessionStorage.idInstancia, vm.TramiteId).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						console.log(respuesta.data);
						$scope.datosCorrientes = respuesta.data;
					}
				});
		}

		function abrirMensajeInformacion() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Tipo Valor, </span><br /> <span class='tituhori' > Proyectos que se encuentran en Ejecución.</span>");
		}

		function abrirMensajeInformacionAnioBase() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Año Constante, </span><br /> <span class='tituhori' > Proyectos que se encuentran en Ejecución.</span>");
		}

		function cambiarPagina(index) {
			vm.pagina = index;
		}

		function ObjetivoVerMas(objetivo) {
			let modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
				controller: 'objetivosIndicadorModalController',
				controllerAs: "vm",
				size: 'lg',
				openedClass: "entidad-modal-adherencia",
				resolve: {
					Objetivo: function () {
						return objetivo.ObjetivoEspecifico;
					},
					IdObjetivo: function () {
						return objetivo.ObjetivoId;
					},
					Tipo: function () {
						return 'Objetivo';
					},
					Titulo: function () {
						return 'Solicitud Vigencia Futura por Producto';
					}
				},
			});
		}

		function ProductoVerMas(prod) {
			let modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
				controller: 'objetivosIndicadorModalController',
				controllerAs: "vm",
				size: 'lg',
				openedClass: "entidad-modal-adherencia",
				resolve: {
					Objetivo: function () {
						return prod.Producto;
					},
					IdObjetivo: function () {
						return prod.ProductoId;
					},
					Tipo: function () {
						return 'Producto';
					},
					Titulo: function () {
						return 'Solicitud Vigencia Futura por Producto';
					}
				},
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

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo, data) {
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				vm.limpiarErrores();
				if (data != undefined) {
					console.log(data);
					vm.aniobase = data;

					return ajustesolicitudvigfuturaServicio.ObtenerProyectoTramite(vm.idProyecto, vm.TramiteId).then(
						function (respuesta) {
							if (respuesta.data != null && respuesta.data != "") {
								vm.aniobase = vm.aniobase;
								vm.tipoValor = respuesta.data[0].EsConstante;
								if (vm.tipoValor == true && vm.aniobase != null) {
									ObtenerProductosVigenciaFuturaConstante(vm.aniobase);
								}

								if (vm.aniobase == 0) {
									ObtenerProductosVigenciaFuturaCorriente();
								}
							}
						});
				} else {
					ObtenerProyectoTramite();
                }
			}
		}

		vm.BtnObjetivos = function (objetivo) {
			if (objetivo.LabelBotonObjetivo == '+') {
				objetivo.LabelBotonObjetivo = '-'
			} else {
				objetivo.LabelBotonObjetivo = '+'
			}
			return objetivo.LabelBotonObjetivo;
		}

		vm.BtnProductos = function (prod) {
			if (prod.LabelBotonProducto == '+') {
				prod.LabelBotonProducto = '-'
			} else {
				prod.LabelBotonProducto = '+'
			}
			return prod.LabelBotonProducto;
		}

		function Cancelar(prod) {
			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

				prod.HabilitaEditarProducto = false;
				angular.forEach(prod.Vigencias, function (series) {
					series.TotalVigenciasFuturaSolicitada = parseFloat(series.TotalVigenciasFuturaSolicitadaOriginal).toFixed(2);
				});
				

				return vigfuturaproductosServicio.ObtenerProductosVigenciaFuturaConstante(vm.codigoBpin, vm.TramiteId, vm.aniobase).then(
					function (respuesta) {
						if (respuesta.data != null && respuesta.data != "") {
							utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
						}
					});

			}, function funcionCancelar(reason) {
				//poner aquí q pasa cuando cancela
			}, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
		}

		function CancelarCorriente(prod) {
			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

				prod.HabilitaEditarProducto = false;
				angular.forEach(prod.Vigencias, function (series) {
					series.TotalVigenciasFuturaSolicitada = parseFloat(series.TotalVigenciasFuturaSolicitadaOriginal).toFixed(2);
				});


				return vigfuturaproductosServicio.ObtenerProductosVigenciaFuturaCorriente(vm.codigoBpin, vm.TramiteId).then(
					function (respuesta) {
						if (respuesta.data != null && respuesta.data != "") {
							utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
						}
					});

			}, function funcionCancelar(reason) {
				//poner aquí q pasa cuando cancela
			}, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
		}

		function habilitaEditar(prod) {

			prod.HabilitaEditarProducto = true;

			angular.forEach(prod.Vigencias, function (series) {
				series.TotalVigenciasFuturaSolicitada = parseFloat(series.TotalVigenciasFuturaSolicitada).toFixed(2);
			});
		}

		function Guardar(prod) {
			var validar = false;
			angular.forEach(prod.Vigencias, function (series) {
				if (series.TotalVigenciasFuturaSolicitada == "" || series.TotalVigenciasFuturaSolicitada == null) {
					series.TotalVigenciasFuturaSolicitada = 0.00;
				} else {
					if ((parseFloat(series.TotalVigenciasFuturaSolicitada).toFixed(2) * parseFloat(series.Deflactor).toFixed(4)) > (series.ValorVigenteSolicitado - series.VigenteFuturasAnteriores)) {
						validar = true;
					}
				}
			});

			if (validar) {
				utilidades.mensajeError("En esta fuente existen vigencias donde el proyecto no tiene recursos solicitados mayores o iguales al valor solicitado de VF");
				return;
			}

			prod.TramiteId = vm.TramiteId;
			prod.ProyectoId = vm.idProyecto;
			prod.Producto = 'Constante';

			return vigfuturaproductosServicio.actualizarVigenciaFuturaProducto(prod).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						guardarCapituloModificado();
						utilidades.mensajeSuccess("La tabla resumen de solicitud de vigencia futura se ha actualizado", false, false, false, "Los datos han sido guardados con éxito");
						prod.HabilitaEditarProducto = false;
						angular.forEach(prod.Vigencias, function (series) {
							series.TotalVigenciasFuturaSolicitadaOriginal = parseFloat(series.TotalVigenciasFuturaSolicitada).toFixed(2);
						});
						vm.init();
					} else {
						utilidades.mensajeError("Error al realizar la operación");
					}

				});
		}

		function GuardarCorriente(prod) {
			var validar = false;
			angular.forEach(prod.Vigencias, function (series) {
				if (series.TotalVigenciasFuturaSolicitada == "" || series.TotalVigenciasFuturaSolicitada == null) {
					series.TotalVigenciasFuturaSolicitada = 0.00;
				} else {
					if ((parseFloat(series.TotalVigenciasFuturaSolicitada).toFixed(2)) > (series.ValorVigenteSolicitado - series.VigenteFuturasAnteriores)) {
						validar = true;
					}
				}
			});

			if (validar) {
				utilidades.mensajeError("En esta fuente existen vigencias donde el proyecto no tiene recursos solicitados mayores o iguales al valor solicitado de VF");
				return;
			}

			prod.TramiteId = vm.TramiteId;
			prod.ProyectoId = vm.idProyecto;
			prod.Producto = 'Corriente';

			return vigfuturaproductosServicio.actualizarVigenciaFuturaProducto(prod).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						guardarCapituloModificado();
						utilidades.mensajeSuccess("La tabla resumen de solicitud de vigencia futura se ha actualizado", false, false, false, "Los datos han sido guardados con éxito");
						prod.HabilitaEditarProducto = false;
						angular.forEach(prod.Vigencias, function (series) {
							series.TotalVigenciasFuturaSolicitadaOriginal = parseFloat(series.TotalVigenciasFuturaSolicitada).toFixed(2);
						});
						vm.init();
					} else {
						utilidades.mensajeError("Error al realizar la operación");
					}

				});
		}

		function ObtenerSeccionCapitulo() {
			const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
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

		vm.actualizaFila = function (event, prod) {

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

			const val = event.target.value;
			const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
			var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
			return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
		}

		vm.refresh = function (handler, nombreComponente) {
			vm.handlerComponentes.map(item => {
				if (item.componente == nombreComponente) item.handler = handler;
			});
		}

		vm.setErrores = function (handler, nombreComponente) {
			vm.handlerComponentes.map(item => {
				if (item.componente == nombreComponente) item.handlerValidacion = handler;
			});
		}

		vm.AVPPFD001 = function (descripcionError, dataError) {
			var listadoVigenciasError = vm.AVFFFD00X(dataError, "AVPPFD001", descripcionError);
			listadoVigenciasError.map(vig => {
				var vigencia = document.getElementById("total-vigencias-producto-vigencia-futura-producto-" + vig["ProductoId"] + "-" + vig["Vigencia"])
				if (vigencia != undefined && vigencia != null && !vigencia.classList.contains("divInconsistencia")) {
					vigencia.classList.add("divInconsistencia")
				}
			})
			vm.erroresActivosArray.push({
				Error: "AVPPFD001",
				Data: listadoVigenciasError
			});
		}

		vm.AVPPFD002 = function (descripcionError, dataError) {
			var listadoVigenciasError = vm.AVFFFD00X(dataError, "AVPPFD002", descripcionError);
			listadoVigenciasError.map(vig => {
				var vigencia = document.getElementById("total-vigencias-producto-vigencia-futura-producto-" + vig["ProductoId"] + "-" + vig["Vigencia"])
				if (vigencia != undefined && vigencia != null && !vigencia.classList.contains("divInconsistencia")) {
					vigencia.classList.add("divInconsistencia")
				}
			})
			vm.erroresActivosArray.push({
				Error: "AVPPFD002",
				Data: listadoVigenciasError
			});
		}

		vm.AVPPFD003 = function (descripcionError, dataError) {
			var listadoVigenciasError = vm.AVFFFD00X(dataError, "AVPPFD003", descripcionError);
			listadoVigenciasError.map(vig => {
				var vigencia = document.getElementById("vigencia-solicitada-producto-vigencia-futura-producto-" + vig["ProductoId"] + "-" + vig["Vigencia"])
				if (vigencia != undefined && vigencia != null && !vigencia.classList.contains("divInconsistencia")) {
					vigencia.classList.add("divInconsistencia")
				}
			})
			vm.erroresActivosArray.push({
				Error: "AVPPFD003",
				Data: listadoVigenciasError
			});
		}

		vm.AVPPFD004 = function (descripcionError, dataError) {

			var resumenHtml = document.getElementById("producto-vigencia-futura-producto-resumen");
			var resumenConstanteHtml = document.getElementById("producto-vigencia-futura-producto-constante-resumen");
			var resumenErrorContainerHtml = document.getElementById("producto-vigencia-futura-resumen-mns");
			var resumenErrorMnsHtml = document.getElementById("producto-futura-resumen-errorAVPPFD004-mns");
			var resumenCorrienteHtml = document.getElementById("producto-vigencia-futura-producto-corriente-resumen");
			var resumenErrorCorrienteContainerHtml = document.getElementById("producto-vigencia-futura-resumencorriente-mns");
			var resumenErrorCorrienteMnsHtml = document.getElementById("producto-futura-resumencorriente-errorAVPPFD004-mns");

			

			var data = JSON.parse(dataError);

			if (resumenHtml != undefined && resumenHtml != null) {
				resumenHtml.classList.remove("hidden")
			}

			if (resumenConstanteHtml != undefined && resumenConstanteHtml != null) {
				resumenConstanteHtml.classList.remove("hidden")
				resumenConstanteHtml.classList.add("d-inline")
			}

			if (resumenErrorContainerHtml != undefined && resumenErrorContainerHtml != null) {
				resumenErrorContainerHtml.classList.remove("hidden")
			}

			if (resumenErrorMnsHtml != undefined && resumenErrorMnsHtml != null) {
				resumenErrorMnsHtml.innerHTML = '<img src="Img/u4630.svg" class="mr-3"></div>' + descripcionError;
			}

			if (resumenCorrienteHtml != undefined && resumenCorrienteHtml != null) {
				resumenCorrienteHtml.classList.remove("hidden")
				resumenCorrienteHtml.classList.add("d-inline")
			}

			if (resumenErrorCorrienteContainerHtml != undefined && resumenErrorCorrienteContainerHtml != null) {
				resumenErrorCorrienteContainerHtml.classList.remove("hidden")
			}

			if (resumenErrorCorrienteMnsHtml != undefined && resumenErrorCorrienteMnsHtml != null) {
				resumenErrorCorrienteMnsHtml.innerHTML = '<img src="Img/u4630.svg" class="mr-3"></div>' + descripcionError;
			}

			if (data != undefined && data != null) {
				data.map(vig => {
					var resumenVigencia = document.getElementById("resumen-producto-vigencia-futura-" + vig["Vigencia"]);

					if (resumenVigencia != undefined && resumenVigencia != null) {
						if (!resumenVigencia.classList.contains("divInconsistencia")) {
							resumenVigencia.classList.add("divInconsistencia")
                        }
					}
				})

				vm.erroresActivosArray.push({
					Error: "AVPPFD004",
					Data: data
				});
			}

		}

		vm.AVFFFD00X = function (dataError, codError, descripcionError) {
			var data = JSON.parse(dataError);
			var listadoVigenciasError = []
			var titulo = document.getElementById("producto-vigencia-futura-producto-valor");
			const listadoObjetivos = data.reduce((acc, curr) => {
				if (!acc[curr['ObjetivoEspecificoId']]) acc[curr['ObjetivoEspecificoId']] = [];
				acc[curr['ObjetivoEspecificoId']].push(curr);
				return acc;
			}, {});

			var objetivosId = Object.keys(listadoObjetivos);

			objetivosId.map(obj => {
				var objetivoErrorHtml = document.getElementById("objetivo-vigencia-futura-producto-" + obj);

				if (objetivoErrorHtml != undefined && objetivoErrorHtml != null) {
					const listadoProductos = listadoObjetivos[obj].reduce((acc, curr) => {
						if (!acc[curr['ProductoId']]) acc[curr['ProductoId']] = [];
						acc[curr['ProductoId']].push(curr);
						return acc;
					}, {});

					var productosId = Object.keys(listadoProductos);
					productosId.map(prod => {
						var productoErrorHtml = document.getElementById("producto-vigencia-futura-producto-" + prod);
						var productoErrorMnsContainerHtml = document.getElementById("producto-vigencia-futura-" + prod);
						var productoErrorMnsHtml = document.getElementById("producto-futura-" + prod + "-error" + codError + "-mns");
						var productoConstanteTabErrorHtml = document.getElementById("producto-vigencia-futura-producto-constante-" + prod);
						var productoConstanteErrorHtml = document.getElementById("producto-vigencia-futura-errores-" + prod);
						if (productoErrorHtml != undefined && productoErrorHtml != null) {

							if (productoConstanteTabErrorHtml != undefined && productoConstanteTabErrorHtml != null) {
								productoConstanteTabErrorHtml.classList.remove("hidden")
								productoConstanteTabErrorHtml.classList.add("d-inline")
							}
							if (productoConstanteErrorHtml != undefined && productoConstanteErrorHtml != null) {
								productoConstanteErrorHtml.classList.remove("hidden")
							}
							if (productoErrorMnsContainerHtml != undefined && productoErrorMnsContainerHtml != null && productoErrorMnsHtml != undefined && productoErrorMnsHtml != null) {
								productoErrorMnsHtml.innerHTML = '<img src="Img/u4630.svg" class="mr-3"></div>' + descripcionError;
								productoErrorMnsContainerHtml.classList.remove("hidden")
							}

							objetivoErrorHtml.classList.remove("hidden");
							productoErrorHtml.classList.remove("hidden");

							listadoProductos[prod].map(vig => {
								var id = "producto-vigencia-futura-producto-" + prod + "-" + vig["Vigencia"];
								var vigHtml = document.getElementById(id)
								if (vigHtml != undefined && vigHtml != null) {
									vigHtml.classList.remove("hidden")
									listadoVigenciasError.push({
										ObjetivoId: obj,
										ProductoId: prod,
										Vigencia: vig["Vigencia"]
									});
								}
							});
						}						
					})
				}
			});

			if (titulo != undefined && titulo != null && titulo.classList.contains("hidden")) {
				titulo.classList.remove("hidden")
			}

			return listadoVigenciasError

		}

		vm.limpiarAVPPFD00 = function (codError, dataError) {
			if (codError != "AVPPFD004") {
				var obj = dataError.ObjetivoId;
				var prod = dataError.ProductoId;
				var vig = dataError.Vigencia
				var titulo = document.getElementById("producto-vigencia-futura-producto-valor");

				var objetivoErrorHtml = document.getElementById("objetivo-vigencia-futura-producto-" + obj);
				var productoErrorHtml = document.getElementById("producto-vigencia-futura-producto-" + prod);
				var productoErrorMnsContainerHtml = document.getElementById("producto-vigencia-futura-" + prod);
				var productoErrorMnsHtml = document.getElementById("producto-futura-" + prod + "-error" + codError + "-mns");
				var productoConstanteTabErrorHtml = document.getElementById("producto-vigencia-futura-producto-constante-" + prod);
				var productoConstanteErrorHtml = document.getElementById("producto-vigencia-futura-errores-" + prod);

				if (objetivoErrorHtml != undefined && objetivoErrorHtml != null && !objetivoErrorHtml.classList.contains("hidden")) {
					objetivoErrorHtml.classList.add("hidden")
				}
				if (productoErrorHtml != undefined && productoErrorHtml != null && !productoErrorHtml.classList.contains("hidden")) {
					productoErrorHtml.classList.add("hidden")
				}
				if (productoErrorMnsContainerHtml != undefined && productoErrorMnsContainerHtml != null && productoErrorMnsHtml != undefined && productoErrorMnsHtml != null) {
					productoErrorMnsHtml.innerHTML = '';
					productoErrorMnsContainerHtml.classList.add("hidden")
				}
				if (productoConstanteTabErrorHtml != undefined && productoConstanteTabErrorHtml != null && !productoConstanteTabErrorHtml.classList.contains("hidden")) {
					productoConstanteTabErrorHtml.classList.add("hidden")
					productoConstanteTabErrorHtml.classList.remove("d-inline")
				}
				if (productoConstanteErrorHtml != undefined && productoConstanteErrorHtml != null && !productoConstanteErrorHtml.classList.contains("hidden")) {
					productoConstanteErrorHtml.classList.add("hidden")
				}

				
				var vigencia = document.getElementById("total-vigencias-producto-vigencia-futura-producto-" + prod + "-" + vig)
				var vigencia003 = document.getElementById("vigencia-solicitada-producto-vigencia-futura-producto-" + prod + "-" + vig)
				if (vigencia003 != undefined && vigencia003 != null && !vigencia003.classList.contains("divInconsistencia")) {
					vigencia003.classList.remove("divInconsistencia")
				}
				if (vigencia != undefined && vigencia != null &&
					!vigencia.classList.contains("divInconsistencia")) {
					vigencia.classList.remove("divInconsistencia")
				}

				if (titulo != undefined && titulo != null && !titulo.classList.contains("hidden")) {
					titulo.classList.add("hidden")
				}

			} else {
				var resumenHtml = document.getElementById("producto-vigencia-futura-producto-resumen");
				var resumenConstanteHtml = document.getElementById("producto-vigencia-futura-producto-constante-resumen");
				var resumenErrorContainerHtml = document.getElementById("producto-vigencia-futura-resumen-mns");
				var resumenErrorMnsHtml = document.getElementById("producto-futura-resumen-errorAVPPFD004-mns");

				if (resumenHtml != undefined && resumenHtml != null && !resumenHtml.classList.contains("hidden")) {
					resumenHtml.classList.add("hidden")
				}

				if (resumenConstanteHtml != undefined && resumenConstanteHtml != null && !resumenConstanteHtml.classList.contains("hidden")) {
					resumenConstanteHtml.classList.add("hidden")
					resumenConstanteHtml.classList.remove("d-inline")
				}

				if (resumenErrorContainerHtml != undefined && resumenErrorContainerHtml != null && !resumenErrorContainerHtml.classList.contains("hidden")) {
					resumenErrorContainerHtml.classList.add("hidden")
				}

				if (resumenErrorMnsHtml != undefined && resumenErrorMnsHtml != null && !resumenErrorMnsHtml.classList.contains("hidden")) {
					resumenErrorMnsHtml.innerHTML = '';
				}

				var resumenVigencia = document.getElementById("resumen-producto-vigencia-futura-" + dataError["Vigencia"]);
				if (resumenVigencia != undefined && resumenVigencia != null) {
					if (resumenVigencia.classList.contains("divInconsistencia")) {
						resumenVigencia.classList.remove("divInconsistencia")
					}
				}
            }
		}

		vm.limpiarErrores = function () {
			if (vm.erroresActivosArray.length > 0) {
				vm.erroresActivosArray.forEach(p => {
					p.Data.forEach(errors => {
						vm.limpiarAVPPFD00(p.Error, errors);
					})
				});
			}

			vm.erroresActivosArray = [];
		}

		vm.erroresActivos = {
			'AVPPFD001': vm.AVPPFD001,
			'AVPPFD002': vm.AVPPFD002,
			'AVPPFD003': vm.AVPPFD003,
			'AVPPFD004': vm.AVPPFD004,
			'AVPPFD005': vm.AVPPFD001,
			'AVPPFD006': vm.AVPPFD002,
			'AVPPFD007': vm.AVPPFD003,
			'AVPPFD008': vm.AVPPFD004
		}
		/* --------------------------------- Notificación de Validaciones ---------------------------*/

		/**
		 * Función que recibe listado de errores de su componente padre por medio del binding notificacionvalidacion
		 * @param {any} errores
		 */
		vm.notificacionValidacionPadre = function (errores) {
			
			console.log("Validación  - Vigencias futuras productos", errores);
			vm.limpiarErrores();
			if (errores != undefined) {
				var listadoErrores = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
				if (listadoErrores != null && listadoErrores["erroresActivos"] != undefined) {
					var erroresActivos = listadoErrores["erroresActivos"]
					erroresActivos.forEach(p => {
						if (vm.erroresActivos[p.Error] != undefined) vm.erroresActivos[p.Error](p.Descripcion, p.Data);
					});
					var isValid = (erroresActivos.length <= 0);
					vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
				}				
			}
		};
	}

	angular.module('backbone').component('vigfuturaproductoss', {

		templateUrl: "src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/vigenciaFuturaProductos/vigfuturaproductos.html",
		controller: vigfuturaproductosController,
		controllerAs: "vm",
		bindings: {
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			notificacioncambios: '&'
		}
	});

})();