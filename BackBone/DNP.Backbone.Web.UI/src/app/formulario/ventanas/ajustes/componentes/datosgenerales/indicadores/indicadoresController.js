
(function () {
	'use strict';

	indicadoresController.$inject = [
		'$scope',
		'$sessionStorage',
		'constantesBackbone',
		'indicadoresServicio',
		'utilidades',
		'$uibModal',
		'utilsValidacionSeccionCapitulosServicio',
		'justificacionCambiosServicio',
		'justificacionIndicadoresServicio'
	];



	function indicadoresController(
		$scope,
		$sessionStorage,
		constantesBackbone,
		indicadoresServicio,
		utilidades,
		$uibModal,
		utilsValidacionSeccionCapitulosServicio,
		justificacionCambiosServicio,
		justificacionIndicadoresServicio

	) {
		var vm = this;
		vm.lang = "es";
		vm.nombreComponente = "datosgeneralesindicadoresdepr";
		vm.idProyecto = $sessionStorage.proyectoId;
		vm.codigoBpin = $sessionStorage.idObjetoNegocio;
		vm.habilitaGuardar = false;
		vm.anioInicio;
		vm.anioFinal;
		vm.estadoProyecto;
		vm.habilitar = false;
		vm.habilitarFinal = false;
		vm.verBotones = false;
		vm.anioInicioOriginal;
		vm.anioFinalOriginal;
		vm.evaluarVerBotones = evaluarVerBotones;
		vm.habilitarInicio = false;
		vm.Guardar = Guardar;
		vm.Obtenerindicadores = Obtenerindicadores;
		vm.EsMover = false;
		vm.Usuario = usuarioDNP;
		vm.vigenciaActual = new Date().getFullYear();
		vm.limpiartxt = limpiartxt;
		vm.editarIndicador = false;
		vm.AgregarIndSecundario = AgregarIndSecundario;
		vm.habilitaEditar = habilitaEditar;
		vm.ObjetivoVerMas = ObjetivoVerMas;
		vm.ProductoVerMas = ProductoVerMas;
		vm.EliminarIndicador = EliminarIndicador;
		vm.cambiaAcumulativo = cambiaAcumulativo;
		vm.Cancelar = Cancelar;
		vm.erroresActivos = null;
		vm.ConvertirNumero = ConvertirNumero;
		vm.SeccionCapituloId = 0;
		vm.abrirMensajeInformacionObjetivo = abrirMensajeInformacionObjetivo;
		vm.validacionGuardado = null;
		//Inicio

		vm.parametros = {

			idInstancia: $sessionStorage.idInstancia,
			idFlujo: $sessionStorage.idFlujoIframe,
			idNivel: $sessionStorage.idNivel,
			idProyecto: vm.idProyecto,
			idProyectoStr: $sessionStorage.idObjetoNegocio,
			Bpin: vm.Bpin

		};

		vm.init = function () {
			Obtenerindicadores();
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
			vm.notificarrefresco({ handler: vm.refrescarIndicadoresModificados, nombreComponente: vm.nombreComponente });
			vm.vigenciaActual = new Date().getFullYear();
		};

		function ConvertirNumero(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 4,
			}).format(numero);
        }

		function Guardar(indicador) {

			angular.forEach(indicador.Vigencias, function (series) {

				if (series.MetaVigencialIndicadorAjuste == "" || series.MetaVigencialIndicadorAjuste == null) {
					series.MetaVigencialIndicadorAjuste = 0.0000;
                }				
			});

			return indicadoresServicio.ActualizarMetaAjusteIndicador(indicador).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						guardarCapituloModificado();
						utilidades.mensajeSuccess("Los datos diligenciados se reflejarán en la tabla de indicadores dentro de la celda Meta ajuste del indicador intervenido.", false, false, false, "Los datos fueron guardados con éxito");
						indicador.HabilitaEditarIndicador = false;
						indicador.IndicadorAcumulaOriginal = indicador.IndicadorAcumula;
						indicador.IndicadorAcumulaAjustadoOriginal = indicador.IndicadorAcumulaAjustado;
						indicador.MetaTotalFirmeAjustadoOriginal = indicador.MetaTotalFirmeAjustado;
						angular.forEach(indicador.Vigencias, function (series) {
							series.MetaVigencialIndicadorAjusteOriginal = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
						});
						vm.init();
					} else {
						utilidades.mensajeError("Error al realizar la operación");
					}

				});
		}
		function abrirMensajeInformacionObjetivo() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");

		}

		function Cancelar(indicador, productoId, ObjetivoId) {
			indicador.HabilitaEditarIndicador = false;

			//var objetivoOriginal = $scope.datosOriginales.ObjetivosEspecificos.find(element => element.ObjetivoId == ObjetivoId);
			//var productoOriginal = objetivoOriginal.Productos.find(element => element.ProductoId == productoId);
			//var indicadorOriginal = productoOriginal.Indicadores.find(element => element.IndicadorId == indicador.IndicadorId);

			//indicador.Vigencias = indicadorOriginal.Vigencias;
			indicador.IndicadorAcumula = indicador.IndicadorAcumulaOriginal;
			indicador.IndicadorAcumulaAjustado = indicador.IndicadorAcumulaAjustadoOriginal;
			indicador.MetaTotalFirmeAjustado = indicador.MetaTotalFirmeAjustadoOriginal;
			angular.forEach(indicador.Vigencias, function (series) {
				series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjusteOriginal).toFixed(4);
			});
		}

		function Obtenerindicadores() {
			return indicadoresServicio.ObtenerIndicadoresProducto($sessionStorage.idInstancia).then(
				function (respuesta) {
					vm.verBotones = false;
					if (respuesta.data != null && respuesta.data != "") {
						$scope.datos = respuesta.data;
					}
				});
		}

		function evaluarVerBotones() {
			vm.verBotones = false;
			if (vm.anioInicioOriginal != vm.anioInicio) {
				vm.verBotones = true;
			}
		}

		function habilitaEditar(indicador) {

			indicador.HabilitaEditarIndicador = true;

			angular.forEach(indicador.Vigencias, function (series) {
				series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
			});
		}

		function limpiartxt(indicador) {
			vm.editarIndicador = true;// con true despliega las vigencias
			if (indicador.LabelBotonIndicador == '+') {
				indicador.LabelBotonIndicador = '-'
			} else {
				indicador.LabelBotonIndicador = '+'
			}
			return indicador.LabelBotonIndicador;
		}

		function AgregarIndSecundario(prod, ObjetivoId) {

			angular.forEach(prod.CatalogoIndicadoresSecundarios, function (series) {

				var result = prod.Indicadores.find(element => element.CodigoIndicador > series.CodigoIndicador);

				if (series.Marcado == 1) {
					series.Marcado = 2;
				}

			});
			var cantidadIndSec = prod.CatalogoIndicadoresSecundarios.length;
			if (cantidadIndSec == 1 && (prod.CatalogoIndicadoresSecundarios[0].CodigoIndicador == "" || prod.CatalogoIndicadoresSecundarios[0].CodigoIndicador == null)) {
				const mensaje1 = "El producto no tiene indicadores secundarios asociados.";
				new utilidades.mensajeInformacionV(mensaje1, false, false, false);

			}
			else {
				let modalInstance = $uibModal.open({
					animation: $scope.animationsEnabled,
					templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModal.html',
					controller: 'agregarIndicadorSecModalController',
					controllerAs: "vm",
					size: 'lg',
					openedClass: "entidad-modal-adherencia",
					resolve: {
						Programacion: function () {
							return 1;
						},
						ProductoId: function () {
							return prod.ProductoId;
						},
						IndicadoresSec: function () {
							return prod.CatalogoIndicadoresSecundarios;
						},
						ObjetivoId: function () {
							return ObjetivoId;
						}
					},
				});
				modalInstance.result.then(data => {
					if (data != null) {

						var paramsIndSec = {
							IdProducto: prod.ProductoId,
							Lista: data
						};

						indicadoresServicio.agregarIndicadorSecundario(paramsIndSec).then(function (response) {

							if (response.data && (response.statusText === "OK" || response.status === 200)) {
								guardarCapituloModificado();
								const mensaje2 = "Los indicadores secundarios seleccionados fueron agregados correctamente al producto.";
								new utilidades.mensajeSuccess(mensaje2, false, false, false);
								vm.init();
							} else {
								new utilidades.mensajeError("Error al realizar la operación");
							}

						});
					}
				});
			}
		}

		vm.actualizaFila = function (event, indicador) {

			if (Number.isNaN(event.target.value)) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			if (event.target.value == null) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			if (event.target.value == "") {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			event.target.value = parseFloat(event.target.value.replace(",", "."));

			if (indicador.IndicadorAcumula) {
				var acumula = 0;
				angular.forEach(indicador.Vigencias, function (series) {
					acumula = acumula + parseFloat(series.MetaVigencialIndicadorAjuste);
					indicador.MetaTotalFirmeAjustado = parseFloat(acumula.toFixed(4));
				});
			} else {
				indicador.MetaTotalFirmeAjustado = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigencialIndicadorAjuste; }));
            }

			const val = event.target.value;
			const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
			var total = event.target.value = decimalCnt && decimalCnt > 4 ? event.target.value : parseFloat(val).toFixed(4);
			return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(total);
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
					var n2 = n[1].slice(0, 4);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 4) {
						tamanioPermitido = n[0].length + 4;
						event.target.value = n[0] + '.' + n[1].slice(0, 4);
						return;
					}

					if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			} 
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
					var n2 = n[1].slice(0, 4);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 4) {
						tamanioPermitido = n[0].length + 4;
						event.target.value = n[0] + '.' + n[1].slice(0, 4);
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

		function formatearNumero(value) {
			var numerotmp = value.toString().replaceAll('.', '');
			return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
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
						return 'Indicadores';
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
						return prod.NombreProducto;
					},
					IdObjetivo: function () {
						return prod.ProductoId;
					},
					Tipo: function () {
						return 'Producto';
					}
				},
			});
		}

		function EliminarIndicador(IndicadorId, CodigoIndicador) {
			utilidades.mensajeWarning("La línea de información del indicador con código  " + CodigoIndicador + " se perderá.¿Está seguro de continuar ?", function funcionContinuar() {
			const mensaje3 = "El indicador ha sido eliminado correctamente.";
			return indicadoresServicio.EliminarIndicadorProducto(IndicadorId).then(
				function (respuesta) {
					if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
						guardarCapituloModificado();
						new utilidades.mensajeSuccess(mensaje3, false, false, false);
						vm.init();
					} else {
						new utilidades.mensajeError("Error al realizar la operación");
					}
				});
			}, function funcionCancelar(reason) {
				//console.log("reason", reason);
			}, null, null, "Los datos serán eliminados.");
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

		function cambiaAcumulativo(indicador) {
			if (indicador.IndicadorAcumula) {

				var ajustado = 0;
				var mga = 0;
				var firme = 0;
				angular.forEach(indicador.Vigencias, function (series) {
					ajustado = ajustado + Number(series.MetaVigencialIndicadorAjuste);
					mga = mga + Number(series.MetaVigenciaIndicadorMga);
					firme = firme + Number(series.MetaVigenciaIndicadorFirme);
					
				});
				indicador.MetaTotalFirmeAjustado = ajustado;
				indicador.MetaTotalIndicadorMga = mga;
				indicador.MetaTotalFirme = firme;
				indicador.IndicadorAcumulaAjustado = "SI";
				return;
			} else {

				var ajustado = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigencialIndicadorAjuste; }));
				var mga = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigenciaIndicadorMga; }));
				var firme = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigenciaIndicadorFirme; }));

				indicador.MetaTotalFirmeAjustado = ajustado;
				indicador.MetaTotalIndicadorMga = mga;
				indicador.MetaTotalFirme = firme;
				indicador.IndicadorAcumulaAjustado = "NO";
				return;
            }
		}

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			if (nombreCapituloHijo == 'datosgeneraleshorizonte') {
				vm.init();
			}
		}

		/* ------------------------ Validaciones ---------------------------------*/

		vm.notificacionValidacionPadre = function (errores) {
			//console.log("Validación  - Indicadores");
			//console.log(vm.erroresActivos);
			/*if (vm.erroresActivos != null) {
				vm.erroresActivos.forEach(p => {
					vm.validacionAJIND002(p.Error, '', p.Data)
				});
            }*/

			if (errores != undefined) {
				var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
				vm.erroresActivos = erroresFiltrados.erroresActivos;
				//TODO: Ejecutar manera de gestionar errorees.
				//Se coloca método usado en otros componentes
				vm.ejecutarErrores(); 	
			}
			vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
		};

		vm.ejecutarErrores = function () {
			//vm.limpiarErrores();
			$scope.errores = vm.erroresActivos;
			vm.erroresActivos.forEach(p => {
				if (vm.errores[p.Error] != undefined) {
					vm.errores[p.Error](p.Error, p.Descripcion, p.Data);
				}
			});
		}

		vm.limpiarErrores = function () {
			vm.erroresActivos.forEach(p => {
				vm.validacionAJIND002(p.Error, '', p.Data)
			});
        }

		vm.validacionAJIND001 = function (errores, descripcion, data) {
			var indErr001 = JSON.parse(data);
			indErr001.forEach(p => {
				var objetivoError001 = document.getElementById("objetivo-" + p.ObjetivoEspecificoId);
				var productoError001 = document.getElementById("producto-" + p.ProductoId);
				var indicadorError001 = document.getElementById("indicador-" + p.IndicadorId);
				var indicadoriconError001 = document.getElementById("indicadoricon-" + p.IndicadorId);

				if (descripcion != '') {
					if (objetivoError001 != undefined) {
						/*objetivoError001.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						objetivoError001.classList.remove('hidden');
					}

					if (productoError001 != undefined) {
						/*productoError001.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						productoError001.classList.remove('hidden');
					}

					if (indicadorError001 != undefined) {
						indicadorError001.innerHTML = "<span class='messagealerttableDNP'>" + descripcion + "</span>";
						indicadorError001.classList.remove('hidden');
						indicadoriconError001.classList.remove('hidden');
					}
				} else {
					if (objetivoError001 != undefined) {
						/*objetivoError001.innerHTML = "";*/
						objetivoError001.classList.add('hidden');
					}
					if (productoError001 != undefined) {
						/*	productoError001.innerHTML = "";*/
						productoError001.classList.add('hidden');
					}
					if (indicadorError001 != undefined) {
						indicadorError001.innerHTML = "";
						indicadorError001.classList.add('hidden');
						indicadoriconError001.classList.add('hidden');
					}
				}
			});
		}

		vm.validacionAJIND002 = function (errores, descripcion, data) {
			//console.log(JSON.parse(data));
			var indErr002 = JSON.parse(data);
			indErr002.forEach(p => {
				var objetivoError002 = document.getElementById("objetivo-" + p.ObjetivoEspecificoId);
				var productoError002 = document.getElementById("producto-" + p.ProductoId);
				var indicadorError002 = document.getElementById("indicador-" + p.IndicadorId);
				var indicadoriconError002 = document.getElementById("indicadoricon-" + p.IndicadorId);

				if (descripcion != '') {
					if (objetivoError002 != undefined) {
						/*	objetivoError002.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						objetivoError002.classList.remove('hidden');
					}

					if (productoError002 != undefined) {
						/*	productoError002.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						productoError002.classList.remove('hidden');
					}

					if (indicadorError002 != undefined) {

						indicadorError002.innerHTML = "<span class='messagealerttableDNP'>" + descripcion + "</span>";
						indicadorError002.classList.remove('hidden');
						indicadoriconError002.classList.remove('hidden');
					}
				} else {
					if (objetivoError002 != undefined) {
						/*objetivoError002.innerHTML = "";*/
						objetivoError002.classList.add('hidden');
					}
					if (productoError002 != undefined) {
						/*	productoError002.innerHTML = "";*/
						indicadoriconError002.classList.add('hidden');
					}
					if (indicadorError002 != undefined) {
						indicadorError002.innerHTML = "";
						indicadorError002.classList.add('hidden');
						indicadorError002.classList.add('hidden');
					}
				}
			});
		}

		vm.validacionAJIND003 = function (errores, descripcion, data) {
			var indErr003 = JSON.parse(data);
			indErr003.forEach(p => {
				var objetivoError003 = document.getElementById("objetivo-" + p.ObjetivoEspecificoId);
				var productoError003 = document.getElementById("producto-" + p.ProductoId);
				var indicadorError003 = document.getElementById("indicador-" + p.IndicadorId);
				var indicadoriconError003 = document.getElementById("indicadoricon-" + p.IndicadorId);

				if (descripcion != '') {
					if (objetivoError003 != undefined) {
						/*	objetivoError003.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						objetivoError003.classList.remove('hidden');
					}

					if (productoError003 != undefined) {
						/*productoError003.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
						productoError003.classList.remove('hidden');
					}

					if (indicadorError003 != undefined) {
						indicadorError003.innerHTML = "<span class='messagealerttableDNP'>" + descripcion + "</span>";
						indicadorError003.classList.remove('hidden');
						indicadoriconError003.classList.remove('hidden');
					}
				} else {
					if (objetivoError003 != undefined) {
						/*objetivoError003.innerHTML = "";*/
						objetivoError003.classList.add('hidden');
					}
					if (productoError003 != undefined) {
						/*	productoError003.innerHTML = "";*/
						productoError003.classList.add('hidden');
					}
					if (indicadorError003 != undefined) {
						indicadorError003.innerHTML = "";
						indicadorError003.classList.add('hidden');
						indicadoriconError003.classList.add('hidden');
					}
				}
			});
		}

		vm.validacionAJIND004 = function (errores, descripcion, data) {
			var indErr004 = JSON.parse(data);
			indErr004.forEach(p => {
				var objetivoError004 = document.getElementById("objetivo-" + p.ObjetivoEspecificoId);
				var productoError004 = document.getElementById("producto-" + p.ProductoId);
				var indicadorError004 = document.getElementById("indicador-" + p.IndicadorId);
				var indicadoriconError004 = document.getElementById("indicadoricon-" + p.IndicadorId);
				if (objetivoError004 != undefined) {
					/*	objetivoError004.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
					objetivoError004.classList.remove('hidden');
				}

				if (productoError004 != undefined) {
					/*	productoError004.innerHTML = "<span class='d-inline-block ico-advertencia'></span>";*/
					productoError004.classList.remove('hidden');
				}

				if (indicadorError004 != undefined) {
					indicadorError004.innerHTML = "<span class='messagealerttableDNP'>" + descripcion + "</span>";
					indicadorError004.classList.remove('hidden');
					indicadoriconError004.classList.remove('hidden');
				}
			});
		}

		vm.validacionAJIND005 = function (errores, descripcion, data) {
			var indErr005 = JSON.parse(data);
			indErr005.forEach(p => {
				/*var indicadorError005 = document.getElementById("indicador-" + p.IndicadorId);
				if (indicadorError005 != undefined) {
					indicadorError005.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + descripcion + "</span>";
					indicadorError005.classList.remove('hidden');
				}*/
			});
		}

		vm.errores = {
			'AJIND001': vm.validacionAJIND001,
			'AJIND002': vm.validacionAJIND002,
			'AJIND003': vm.validacionAJIND003,
			'AJIND004': vm.validacionAJIND004,
			'AJIND005': vm.validacionAJIND005
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

		vm.refrescarIndicadoresModificados = function () {

			return justificacionIndicadoresServicio.IndicadoresValidarCapituloModificado(vm.codigoBpin).then(
				function (resp) {
					if (resp.data != null && resp.data != "") {
						vm.validacionGuardado();
					}
				}
			);
		}

	}

	angular.module('backbone').component('indicadoresProyecto', {

		templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/indicadores.html",
		controller: indicadoresController,
		controllerAs: "vm",
		bindings: {
			guardadoevent: '&',
			notificacioncambios: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			notificarrefresco: '&'
		}
	})
		.directive('stringToNumber', function () {
			return {
				require: 'ngModel',
				link: function (scope, element, attrs, ngModel) {
					ngModel.$parsers.push(function (value) {

						return '' + value;
					});
					ngModel.$formatters.push(function (value) {
						return parseFloat(value);
					});
				}
			};
		})
		.directive('nksOnlyNumber', function () {
			return {
				restrict: 'EA',
				require: 'ngModel',
				link: function (scope, element, attrs, ngModel) {
					scope.$watch(attrs.ngModel, function (newValue, oldValue) {
						var spiltArray = String(newValue).split("");

						if (attrs.allowNegative == "false") {
							if (spiltArray[0] == '-') {
								newValue = newValue.replace("-", "");
								ngModel.$setViewValue(newValue);
								ngModel.$render();
							}
						}

						if (attrs.allowDecimal == "false") {
							newValue = parseInt(newValue);
							ngModel.$setViewValue(newValue);
							ngModel.$render();
						}

						if (attrs.allowDecimal != "false") {
							if (attrs.decimalUpto) {
								var n = String(newValue).split(".");
								if (n[1]) {
									var n2 = n[1].slice(0, attrs.decimalUpto);
									newValue = [n[0], n2].join(".");
									ngModel.$setViewValue(newValue);
									ngModel.$render();
								}
							}
						}


						if (spiltArray.length === 0) return;
						if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
						if (spiltArray.length === 2 && newValue === '-.') return;

						/*Check it is number or not.*/
						if (isNaN(newValue)) {
							ngModel.$setViewValue(oldValue || '');
							ngModel.$render();
						}
					});
				}
			};
		});

})();