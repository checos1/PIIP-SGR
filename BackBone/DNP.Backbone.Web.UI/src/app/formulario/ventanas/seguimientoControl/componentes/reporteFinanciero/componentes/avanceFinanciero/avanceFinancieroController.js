(function () {
	'use strict';

	avanceFinancieroController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
		'constantesBackbone', '$timeout', 'focalizacionAjustesServicio', 'serviciosComponenteNotificaciones', 'reporteFinancieroServicio',
		'utilsValidacionSeccionCapitulosServicio'
	];

	function avanceFinancieroController(
		$scope,
		$sessionStorage,
		$uibModal,
		utilidades,
		constantesBackbone,
		$timeout,
		focalizacionAjustesServicio,
		serviciosComponenteNotificaciones,
		reporteFinancieroServicio,
		utilsValidacionSeccionCapitulosServicio
	) {


		var vm = this;
		vm.init = init;
		vm.nombreComponente = "financieroavancefinanciero";
		vm.BPIN = $sessionStorage.idObjetoNegocio;
		vm.PreguntasAvanceFinanciero;
		vm.PreguntasAvanceFinancieroorigen;
		vm.cumple = false;
		vm.RespuestasPreguntas = [];
		vm.abrirMensajefechaestimada = abrirMensajefechaestimada;
		vm.abrirMensajeInffecha = abrirMensajeInffecha;
		vm.abrirMensajeTablaResumen = abrirMensajeTablaResumen;
		vm.abrirMensajePreguntasBasicas = abrirMensajePreguntasBasicas;
		vm.abrirMensajePregunta = abrirMensajePregunta;
		vm.abrirMensajeEjecucionPeriodo = abrirMensajeEjecucionPeriodo;

		vm.AvanceFinanciero = [];
		vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
		vm.buscar = buscar;
		$sessionStorage.FlujoIgualPresupuestal = 0;
		vm.VerErrormsnRV = 3;
		vm.VerErrormsnRP = 3;
		vm.MensajeErrorRV = "";
		vm.MensajeErrorRP = "";
		vm.MensajeErrorRVTotal = "";
		vm.MensajeErrorRpTotal = "";
		vm.TieneError = false;
		vm.soloLectura = $sessionStorage.soloLectura;

		vm.componentesRefresh = [
			'recursosfuentesdefinanc',
			'datosgeneraleslocalizaciones',
			'focalizacionpoliticastransv',
			'focalizacionpolresumendefocali',
			'focalizacionpolpoliticastransv',
		];



		function init() {
			vm.model = {
				modulos: {
					administracion: false,
					backbone: true
				}
			}


			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

			vm.ObtenerPreguntasAvanceFinanciero();
			vm.ObtenerAvanceFinanciero(0, 0);
		}

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				vm.init();
			}
		}

		vm.notificacionValidacionHijo = function (handler) {
			vm.notificacionErrores = handler;
		}

		vm.validacionGuardadoHijo = function (handler) {
			vm.validacionGuardado = handler;
		}

		vm.ObtenerAvanceFinanciero = function (vigenciaId, periodoPeriodicidadId) {

			return reporteFinancieroServicio.ObtenerAvanceFinanciero($sessionStorage.idInstancia, $sessionStorage.idProyectoEncabezado, $sessionStorage.idObjetoNegocio, vigenciaId, periodoPeriodicidadId, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(

				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						vm.AvanceFinanciero = [];
						var arreglolistas = jQuery.parseJSON(respuesta.data);
						vm.AvanceFinanciero = jQuery.parseJSON(arreglolistas);
						vm.AvanceFinancieroorigen = jQuery.parseJSON(arreglolistas);

						if (vm.AvanceFinanciero.Fuentes != null) {

							IniciarFormulario(vigenciaId, periodoPeriodicidadId);


						}
					}
				});
		}

		function IniciarFormulario(vigenciaId, periodoPeriodicidadId) {
			var SumaRecursosVigentesApropiacionInicial = 0;
			var SumaRecursosVigentesApropiacionVigente = 0;
			var SumaRecursosVigentesAcumuladoCompromisos = 0;
			var SumaRecursosVigentesAcumuladoObligaciones = 0;
			var SumaRecursosVigentesAcumuladoPagos = 0;

			var SumaReservaPresupuestalApropiacionInicial = 0;
			var SumaReservaPresupuestalApropiacionVigente = 0;
			var SumaReservaPresupuestalAcumuladoCompromisos = 0;
			var SumaReservaPresupuestalAcumuladoObligaciones = 0;
			var SumaReservaPresupuestalAcumuladoPagos = 0;

			vm.AvanceFinanciero.Usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

			vm.AvanceFinanciero.Fuentes.forEach(fuente => {
				if (fuente.RecursosVigentes != null) {
					fuente.RecursosVigentes.forEach(rv => {
						SumaRecursosVigentesApropiacionInicial += parseFloat(rv.RecursosVigentesApropiacionInicial);
						SumaRecursosVigentesApropiacionVigente += parseFloat(rv.RecursosVigentesApropiacionVigente);
						SumaRecursosVigentesAcumuladoCompromisos += parseFloat(rv.RecursosVigentesAcumuladoCompromisos);
						SumaRecursosVigentesAcumuladoObligaciones += parseFloat(rv.RecursosVigentesAcumuladoObligaciones);
						SumaRecursosVigentesAcumuladoPagos += parseFloat(rv.RecursosVigentesAcumuladoPagos);
						rv.HabilitaEditar = false;
					});
				}
				if (fuente.RecursosPresupuestales != null) {
					fuente.RecursosPresupuestales.forEach(rp => {
						SumaReservaPresupuestalApropiacionInicial += parseFloat(rp.ReservaPresupuestalApropiacionInicial);
						SumaReservaPresupuestalApropiacionVigente += parseFloat(rp.ReservaPresupuestalApropiacionVigente);
						SumaReservaPresupuestalAcumuladoCompromisos += parseFloat(rp.ReservaPresupuestalAcumuladoCompromisos);
						SumaReservaPresupuestalAcumuladoObligaciones += parseFloat(rp.ReservaPresupuestalAcumuladoObligaciones);
						SumaReservaPresupuestalAcumuladoPagos += parseFloat(rp.ReservaPresupuestalAcumuladoPagos);
						rp.HabilitaEditar = false;
					});
				}
				
			});
			vm.AvanceFinanciero.SumaRecursosVigentesApropiacionInicial = SumaRecursosVigentesApropiacionInicial;
			vm.AvanceFinanciero.SumaRecursosVigentesApropiacionVigente = SumaRecursosVigentesApropiacionVigente;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoCompromisos = SumaRecursosVigentesAcumuladoCompromisos;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoObligaciones = SumaRecursosVigentesAcumuladoObligaciones;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoPagos = SumaRecursosVigentesAcumuladoPagos;

			vm.AvanceFinanciero.SumaReservaPresupuestalApropiacionInicial = SumaReservaPresupuestalApropiacionInicial;
			vm.AvanceFinanciero.SumaReservaPresupuestalApropiacionVigente = SumaReservaPresupuestalApropiacionVigente;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoCompromisos = SumaReservaPresupuestalAcumuladoCompromisos;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoObligaciones = SumaReservaPresupuestalAcumuladoObligaciones;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoPagos = SumaReservaPresupuestalAcumuladoPagos;

			vm.AvanceFinanciero.SumaAB = SumaRecursosVigentesAcumuladoObligaciones + SumaReservaPresupuestalAcumuladoObligaciones;

			var fechaperiodo = "";
			if (vigenciaId == 0) {
				fechaperiodo = vm.AvanceFinanciero.PeriodosActivos[0].FechaDesde + ' - ' + vm.AvanceFinanciero.PeriodosActivos[0].FechaHasta
			}
			else {
				vm.AvanceFinanciero.TodosPeriodos.forEach(tp => {
					if (tp.Vigencia == vigenciaId && tp.PeriodosPeriodicidadId == periodoPeriodicidadId) {
						fechaperiodo = tp.FechaDesde + ' - ' + tp.FechaHasta;
					}
				});
			}
			vm.AvanceFinanciero.FechaPeriodo = fechaperiodo;

			var nomvigencia = "";
			var nomperiodo = "";
			if (vigenciaId == 0) {
				nomvigencia = vm.AvanceFinanciero.PeriodosActivos[0].Vigencia
			}
			else {
				nomvigencia = vigenciaId;
			}

			vm.AvanceFinanciero.Periodos.forEach(pr => {
				if (periodoPeriodicidadId == 0) {
					if (pr.PeriodosPeriodicidadId == vm.AvanceFinanciero.PeriodosActivos[0].PeriodosPeriodicidadId) {
						nomperiodo = pr.Mes;
					}
				}
				else {
					if (pr.PeriodosPeriodicidadId == periodoPeriodicidadId) {
						nomperiodo = pr.Mes;
					}
				}
			});
			vm.AvanceFinanciero.VigenciaPeriodo = nomvigencia + '/' + nomperiodo;

			if (vigenciaId == 0 && periodoPeriodicidadId == 0) {
				vm.AvanceFinanciero.Estado = 1;
			}
			else {
				var estado = 0;
				vm.AvanceFinanciero.PeriodosActivos.forEach(pra => {
					if (vigenciaId == pra.Vigencia && periodoPeriodicidadId == pra.PeriodosPeriodicidadId) {
						estado = 1;
					}
				});
				vm.AvanceFinanciero.Estado = estado;
			}
		}

		vm.ObtenerPreguntasAvanceFinanciero = function () {

			return reporteFinancieroServicio.ObtenerPreguntasAvanceFinanciero($sessionStorage.idInstancia, $sessionStorage.idProyectoEncabezado, $sessionStorage.idObjetoNegocio, $sessionStorage.idNivel, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(

				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						var arreglolistas = jQuery.parseJSON(respuesta.data);
						vm.PreguntasAvanceFinanciero = jQuery.parseJSON(arreglolistas);


						if (vm.PreguntasAvanceFinanciero != null && vm.PreguntasAvanceFinanciero.Preguntas != null) {
							vm.PreguntasAvanceFinanciero.Preguntas.forEach(paf => {
								if (paf.OpcionesRespuesta != null) {
									paf.OpcionesRespuesta = jQuery.parseJSON(paf.OpcionesRespuesta);
								}
								if (paf.TipoPregunta == "Fecha" && paf.Respuesta != null) {
									paf.Respuesta = paf.Respuesta.replace('/', '-');
									paf.Respuesta = paf.Respuesta.replace('/', '-');
								}
							});
						}
						vm.PreguntasAvanceFinancieroorigen = jQuery.parseJSON(arreglolistas);
						if (vm.PreguntasAvanceFinancieroorigen.Preguntas.length >= 3)
							$sessionStorage.FlujoIgualPresupuestal = vm.PreguntasAvanceFinancieroorigen.Preguntas[2].Respuesta;
					}
				});
		}

		function buscar(Vigencia, PeriodosPeriodicidadId, combo) {
			if (Vigencia == undefined) {
				utilidades.mensajeError("Por favor seleccione una vigencia");
				return;
			}
			if (PeriodosPeriodicidadId == undefined) {
				utilidades.mensajeError("Por favor seleccione un Periodo");
				return;
			}
			var VigenciaId = 0;
			if (combo == 1) {
				vm.AvanceFinanciero.Vigencias.forEach(vg => {
					if (vg.PeriodoProyectoId == Vigencia) {
						VigenciaId = vg.Vigencia;
					}
				});
			}
			else {
				VigenciaId = Vigencia;
			}

			vm.ObtenerAvanceFinanciero(VigenciaId, PeriodosPeriodicidadId)



			var valor = 0;
		}

		vm.ActivarEditar = function () {

			//var fecha1 = document.getElementById("fecha" + vm.PreguntasAvanceFinanciero.Preguntas[0].PreguntaId);
			var fecha2 = document.getElementById("fecha" + vm.PreguntasAvanceFinanciero.Preguntas[1].PreguntaId);
			var radiosi = document.getElementById("radio" + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 0);
			var radiono = document.getElementById("radio" + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 1);


			var variable = $("#Editar")[0].innerText;
			if (variable == "EDITAR") {
				$("#Editar").html("CANCELAR");
				document.getElementById("GuardarPreguntas").classList.remove('btnguardarDisabledDNP');
				document.getElementById("GuardarPreguntas").classList.add('btnguardarDNP');

				if (radiosi != undefined) {
					radiosi.removeAttribute('disabled');
				}
				if (radiono != undefined) {
					radiono.removeAttribute('disabled');
				}
				//if (fecha1 != undefined) {
				//	fecha1.removeAttribute('disabled');
				//}
				if (fecha2 != undefined && vm.PreguntasAvanceFinancieroorigen.Preguntas[1].Respuesta == null) {
					fecha2.removeAttribute('disabled');
				}

			}
			else {
				$("#Editar").html("EDITAR");
				document.getElementById("GuardarPreguntas").classList.add('btnguardarDisabledDNP');
				document.getElementById("GuardarPreguntas").classList.remove('btnguardarDNP');

				/*
				if (radiosi.checked == true) {
					paf.Respuesta = 1;
				}
				if (radiono.checked == true) {
					paf.Respuesta = 2;
				}
				*/

				if (radiosi != undefined) {
					$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 0).attr("disabled", true);
					if (vm.PreguntasAvanceFinancieroorigen.Preguntas[2].Respuesta == null) {
						$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 0).prop('checked', false);
					}
					if (vm.PreguntasAvanceFinancieroorigen.Preguntas[2].Respuesta == "1") {
						$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 0).prop('checked', true);
					}
				}

				if (radiono != undefined) {
					$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 1).attr("disabled", true);
					if (vm.PreguntasAvanceFinancieroorigen.Preguntas[2].Respuesta == null) {
						$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 1).prop('checked', false);
					}
					if (vm.PreguntasAvanceFinancieroorigen.Preguntas[2].Respuesta == "2") {
						$('#radio' + vm.PreguntasAvanceFinanciero.Preguntas[2].PreguntaId + 1).prop('checked', true);
					}
				}


				//if (fecha1 != undefined) {
				//	$("#fecha" + vm.PreguntasAvanceFinanciero.Preguntas[0].PreguntaId).attr("disabled", true);
				//	$("#fecha" + vm.PreguntasAvanceFinanciero.Preguntas[0].PreguntaId).Respuesta = vm.PreguntasAvanceFinancieroorigen.Preguntas[0].Respuesta;
				//	fecha1.value = vm.PreguntasAvanceFinancieroorigen.Preguntas[0].Respuesta;
				//}
				if (fecha2 != undefined) {
					$("#fecha" + vm.PreguntasAvanceFinanciero.Preguntas[1].PreguntaId).attr("disabled", true);
					$("#fecha" + vm.PreguntasAvanceFinanciero.Preguntas[1].PreguntaId).Respuesta = vm.PreguntasAvanceFinancieroorigen.Preguntas[1].Respuesta;
					fecha2.value = vm.PreguntasAvanceFinancieroorigen.Preguntas[1].Respuesta;
				}

				if (vm.PreguntasAvanceFinanciero != null && vm.PreguntasAvanceFinanciero.Preguntas != null) {
					vm.PreguntasAvanceFinanciero.Preguntas.forEach(paf => {
						if (paf.TipoPregunta == 'Fecha') {
							$("#fechaerrorfecha" + paf.PreguntaId).attr('disabled', true);
							$("#fechaerrorfecha" + paf.PreguntaId).fadeOut();
							$("#fechaerrorfechamsn" + paf.PreguntaId).attr('disabled', true);
							$("#fechaerrorfechamsn" + paf.PreguntaId).fadeOut();
							var fechamsn = document.getElementById("fechaerrorfechamsn" + paf.PreguntaId);
							if (fechamsn != undefined) {
								fechamsn.innerHTML = '<span></span>';
							}
						}
						if (paf.TipoPregunta == 'Opcion') {
							$("#radioerrorfecha" + paf.PreguntaId).attr('disabled', true);
							$("#radioerrorfecha" + paf.PreguntaId).fadeOut();
							$("#radioerrorfechamsn" + paf.PreguntaId).attr('disabled', true);
							$("#radioerrorfechamsn" + paf.PreguntaId).fadeOut();
							var radiomsn = document.getElementById("radioerrorfechamsn" + paf.PreguntaId);
							if (radiomsn != undefined) {
								radiomsn.innerHTML = '<span></span>';
							}
						}
					});
				}

			}
		}

		vm.registroRespuesta = function () {
			vm.RespuestasPreguntas = [];
			validarCampos();
			if (!vm.cumple) {
				//utilidades.mensajeError("Hay datos que presentan inconsistencias, Verifique los campos señalados");
				return;
			}

			if (vm.PreguntasAvanceFinanciero != null && vm.PreguntasAvanceFinanciero.Preguntas != null) {
				vm.PreguntasAvanceFinanciero.Preguntas.forEach(paf => {

					var RespuestaPreguntas = {
						IdInstancia: $sessionStorage.idInstancia,
						IdProyecto: $sessionStorage.idProyectoEncabezado,
						IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
						IdNivel: $sessionStorage.idNivel,
						PoliticaId: 0,
						Respuesta: paf.Respuesta,
						ObservacionPregunta: "",
						EnvioPoliticaSubDireccionIdAgrupa: 0,
						PreguntaId: paf.PreguntaId
					};
					vm.RespuestasPreguntas.push(RespuestaPreguntas);

				});
			};

			reporteFinancieroServicio.GuardarPreguntasAvanceFinanciero(vm.RespuestasPreguntas).then(function (response) {
				if (response.statusText === "OK" || response.status === 200) {
					if (vm.PreguntasAvanceFinanciero.Preguntas.length >= 3)
						$sessionStorage.FlujoIgualPresupuestal = vm.PreguntasAvanceFinanciero.Preguntas[2].Respuesta;
					setTimeout(function () {
						utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
					}, 500);

					$("#Editar").html("EDITAR");
					document.getElementById("GuardarPreguntas").classList.add('btnguardarDisabledDNP');
					document.getElementById("GuardarPreguntas").classList.remove('btnguardarDNP');

					init();
				}
				else {
					utilidades.mensajeError(response.data.Mensaje);
				}
			});
		}

		function abrirMensajeInffecha() {
			utilidades.mensajeInformacionN("", null, "<span class='tituhori' > Comprende la realización de las actividades orientadas a entregar bienes o servicios. Esta se debe ingresar por el gestor del proyecto tomando como soporte la fecha de aprobación de las garantías a que haya lugar o desde la suscripción del acta de inicio si ésta se ha pactado en la relación contractual. Para proyectos a través de los cuales se adelanten varios procesos de contratación, la fecha de inicio de las actividades será aquella en la que se aprueben las garantías correspondientes del primer contrato.</span>");
		}

		function abrirMensajefechaestimada() {
			utilidades.mensajeInformacionN("", null, "<span class='tituhori' > Fecha registrada por el Formulador del proyecto en el paso de solicitud de recursos del proceso Gestión de recursos, con la cual se indica una fecha tentativa de inicio de las actividades del proyecto.</span>");
		}

		function abrirMensajePregunta() {
			utilidades.mensajeInformacionN("", null, "<span class='tituhori' > El avance de flujo de caja es diferente al avance presupuestal cuando el proyecto se ejecuta a través del mecanismo de fiducias, encargos fiduciarios o patrimonios autónomos y sus derivados.</span>");
		}

		function abrirMensajeEjecucionPeriodo(){
			utilidades.mensajeInformacionN("", null, "<span class='tituhori' > Corresponde a la sumatoria del acumulado de las obligaciones de los recursos vigentes más el acumulado de las obligaciones de las reservas presupuestales.</span>");
		}

		function abrirMensajeTablaResumen() {
			utilidades.mensajeInformacionN("", null, null, "<span class='tituhori' >Mensaje table de resumen.</span>");
		}

		function abrirMensajePreguntasBasicas() {
			utilidades.mensajeInformacionN("", null, null, "<span class='tituhori' > Mensaje preguntas básicas.</span>");
		}

		function validarCampos() {
			vm.cumple = false;
			var error = "";
			if (vm.PreguntasAvanceFinanciero != null && vm.PreguntasAvanceFinanciero.Preguntas != null) {
				vm.PreguntasAvanceFinanciero.Preguntas.forEach(paf => {
					if (paf.TipoPregunta == 'Fecha') {
						if ((document.getElementById("fecha" + paf.PreguntaId).value == '')) {
							$("#fechaerrorfecha" + paf.PreguntaId).attr('disabled', false);
							$("#fechaerrorfecha" + paf.PreguntaId).fadeIn();
							$("#fechaerrorfechamsn" + paf.PreguntaId).attr('disabled', false);
							$("#fechaerrorfechamsn" + paf.PreguntaId).fadeIn();
							var Errormsn = document.getElementById("fechaerrorfechamsn" + paf.PreguntaId);
							if (Errormsn != undefined) {
								Errormsn.innerHTML = '<span>Campo obligarotio</span>';
							}
							error = "seleccion obligatoria";
						}
						else {
							$("#fechaerrorfecha" + paf.PreguntaId).attr('disabled', true);
							$("#fechaerrorfecha" + paf.PreguntaId).fadeOut();
							$("#fechaerrorfechamsn" + paf.PreguntaId).attr('disabled', true);
							$("#fechaerrorfechamsn" + paf.PreguntaId).fadeOut();
							var Errormsn = document.getElementById("fechaerrorfechamsn" + paf.PreguntaId);
							if (Errormsn != undefined) {
								Errormsn.innerHTML = '<span></span>';
							}
							paf.Respuesta = document.getElementById("fecha" + paf.PreguntaId).value;
						}
					}
					if (paf.TipoPregunta == 'Opcion') {
						var radiosi = document.getElementById("radio" + paf.PreguntaId + 0);
						var radiono = document.getElementById("radio" + paf.PreguntaId + 1);
						if (radiosi.checked == false && radiono.checked == false) {
							$("#radioerrorfecha" + paf.PreguntaId).attr('disabled', false);
							$("#radioerrorfecha" + paf.PreguntaId).fadeIn();
							$("#radioerrorfechamsn" + paf.PreguntaId).attr('disabled', false);
							$("#radioerrorfechamsn" + paf.PreguntaId).fadeIn();
							var Errormsn = document.getElementById("radioerrorfechamsn" + paf.PreguntaId);
							if (Errormsn != undefined) {
								Errormsn.innerHTML = '<span>La respuesta a esta pregunta es obligatoria</span>';
							}
							error = "seleccion obligatoria";
						}
						else {
							$("#radioerrorfecha" + paf.PreguntaId).attr('disabled', true);
							$("#radioerrorfecha" + paf.PreguntaId).fadeOut();
							$("#radioerrorfechamsn" + paf.PreguntaId).attr('disabled', true);
							$("#radioerrorfechamsn" + paf.PreguntaId).fadeOut();
							var Errormsn = document.getElementById("radioerrorfechamsn" + paf.PreguntaId);
							if (Errormsn != undefined) {
								Errormsn.innerHTML = '<span></span>';
							}
							if (radiosi.checked == true) {
								paf.Respuesta = 1;
							}
							if (radiono.checked == true) {
								paf.Respuesta = 2;
							}
						}
					}
				});
			}
			if (error == "") {
				vm.cumple = true;
			}
		}

		function ConvertirNumero2decimales(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 2,
			}).format(numero);
		}
		vm.validateFormat = function (event, cantidad) {

			if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
				event.preventDefault();
			}

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 12;
			var tamanio = event.target.value.length;
			var permitido = false;
			permitido = event.target.value.toString().includes(".");
			if (permitido) {
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, cantidad);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > cantidad) {
						tamanioPermitido = n[0].length + cantidad;
						event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
						return;
					}

					if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			} else {
				if (tamanio > 15 && event.keyCode != 44) {
					event.preventDefault();
				}
			}

			if (event.keyCode === 44 && tamanio == 15) {
			}
			else {
				if (tamanio > tamanioPermitido || tamanio > 15) {
					event.preventDefault();
				}
			}
		}
		vm.validarTamanio = function (event, cantidad) {
			var regexp = /^\d+\.\d{0,2}$/;
			var valida = regexp.test(event.target.value);

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
				if (decimales > cantidad) {
				}
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, cantidad);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 4) {
						tamanioPermitido = n[0].length + cantidad;
						event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
						return;
					}

					if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			}
		}

		vm.actualizaFila = function (fuente, valor, fuenteid, indexFuentes, indexrecursos) {
			calcularTotales(fuente, valor, fuenteid, indexFuentes, indexrecursos);
		}

		function calcularTotales(fuente, valor, fuenteid, indexFuentes, indexrecursos) {
			vm.MensajeErrorRV = "";
			vm.MensajeErrorRP = "";
			vm.MensajeErrorRVTotal = "";
			vm.MensajeErrorRpTotal = "";
			var MensajeErrorRV = "";
			var MensajeErrorRP = "";

			var SumaRecursosVigentesApropiacionInicial = 0;
			var SumaRecursosVigentesApropiacionVigente = 0;
			var SumaRecursosVigentesAcumuladoCompromisos = 0;
			var SumaRecursosVigentesAcumuladoObligaciones = 0;
			var SumaRecursosVigentesAcumuladoPagos = 0;

			var SumaReservaPresupuestalApropiacionInicial = 0;
			var SumaReservaPresupuestalApropiacionVigente = 0;
			var SumaReservaPresupuestalAcumuladoCompromisos = 0;
			var SumaReservaPresupuestalAcumuladoObligaciones = 0;
			var SumaReservaPresupuestalAcumuladoPagos = 0;


			vm.AvanceFinanciero.Fuentes.forEach(fuente => {
				fuente.RecursosVigentes.forEach(rv => {
					SumaRecursosVigentesApropiacionInicial += parseFloat(rv.RecursosVigentesApropiacionInicial);
					SumaRecursosVigentesApropiacionVigente += parseFloat(rv.RecursosVigentesApropiacionVigente);
					SumaRecursosVigentesAcumuladoCompromisos += parseFloat(rv.RecursosVigentesAcumuladoCompromisos);
					SumaRecursosVigentesAcumuladoObligaciones += parseFloat(rv.RecursosVigentesAcumuladoObligaciones);
					SumaRecursosVigentesAcumuladoPagos += parseFloat(rv.RecursosVigentesAcumuladoPagos);
					rv.HabilitaEditar = false;

					/*
					if (parseFloat(rv.RecursosVigentesApropiacionVigente) > parseFloat(rv.RecursosVigentesApropiacionInicial)) {
						MensajeErrorRV += fuente.NombreFuente + ", ";
						$("#inputvalorafapv" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafapv" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafapv" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafapv" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					*/
					if (parseFloat(rv.RecursosVigentesAcumuladoCompromisos) > parseFloat(rv.RecursosVigentesApropiacionVigente)) {
						MensajeErrorRV += MensajeErrorRV.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					if (parseFloat(rv.RecursosVigentesAcumuladoObligaciones) > parseFloat(rv.RecursosVigentesAcumuladoCompromisos)) {
						MensajeErrorRV += MensajeErrorRV.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafob" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafob" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafob" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafob" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					if (parseFloat(rv.RecursosVigentesAcumuladoPagos) > parseFloat(rv.RecursosVigentesAcumuladoObligaciones)) {
						MensajeErrorRV += MensajeErrorRV.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}



				});
				fuente.RecursosPresupuestales.forEach(rp => {
					SumaReservaPresupuestalApropiacionInicial += parseFloat(rp.ReservaPresupuestalApropiacionInicial);
					SumaReservaPresupuestalApropiacionVigente += parseFloat(rp.ReservaPresupuestalApropiacionVigente);
					SumaReservaPresupuestalAcumuladoCompromisos += parseFloat(rp.ReservaPresupuestalAcumuladoCompromisos);
					SumaReservaPresupuestalAcumuladoObligaciones += parseFloat(rp.ReservaPresupuestalAcumuladoObligaciones);
					SumaReservaPresupuestalAcumuladoPagos += parseFloat(rp.ReservaPresupuestalAcumuladoPagos);
					rp.HabilitaEditar = false;

					/*
					if (parseFloat(rp.ReservaPresupuestalApropiacionVigente) > parseFloat(rp.ReservaPresupuestalApropiacionInicial)) {
						MensajeErrorRP += MensajeErrorRP.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";

						$("#inputvalorafrpap1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafrpap1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafrpap1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafrpap1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					*/
					if (parseFloat(rp.ReservaPresupuestalAcumuladoCompromisos) > parseFloat(rp.ReservaPresupuestalApropiacionVigente)) {
						MensajeErrorRP += MensajeErrorRP.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafrpac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafrpac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafrpac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafrpac" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					if (parseFloat(rp.ReservaPresupuestalAcumuladoObligaciones) > parseFloat(rp.ReservaPresupuestalAcumuladoCompromisos)) {
						MensajeErrorRP += MensajeErrorRP.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafrpao" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafrpao" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafrpao" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafrpao" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}
					if (parseFloat(rp.ReservaPresupuestalAcumuladoPagos) > parseFloat(rp.ReservaPresupuestalAcumuladoObligaciones)) {
						MensajeErrorRP += MensajeErrorRP.includes(fuente.NombreFuente) ? "" : fuente.NombreFuente + ", ";
						$("#inputvalorafrpap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "#AA0014");
						$("#inputvalorafrpap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "dotted");
					}
					else {
						$("#inputvalorafrpap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-color", "");
						$("#inputvalorafrpap" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).css("border-style", "hidden");
					}

				});
			});

			if (MensajeErrorRV != "") {
				MensajeErrorRV += "se debe cumplir que el Vigente >= Compromisos >= Obligaciones >= Pagos."
				$("#imgavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', false);
				$("#imgavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeIn();
				$("#icoavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', false);
				$("#icoavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeIn();
				$("#GuardarPreguntasgrilla").fadeOut();

				vm.MensajeErrorRVTotal = MensajeErrorRV;

				if (MensajeErrorRV.length < 90) {
					vm.VerErrormsnRV = 2;
					vm.MensajeErrorRV = MensajeErrorRV;
				}
				else {
					vm.VerErrormsnRV = 0;
					vm.MensajeErrorRV = MensajeErrorRV.substring(0, 90);
				}

				vm.TieneError = true;

			}
			else {
				$("#imgavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', true);
				$("#imgavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeOut();
				$("#icoavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', true);
				$("#icoavfrvval" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeOut();
				vm.VerErrormsnRV = 3;
			}

			if (MensajeErrorRP != "") {
				MensajeErrorRP += "se debe cumplir que el Vigente >= Compromisos >= Obligaciones >= Pagos."
				$("#imgavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', false);
				$("#imgavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeIn();
				$("#icoavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', false);
				$("#icoavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeIn();
				$("#GuardarPreguntasgrilla").fadeOut();

				vm.MensajeErrorRPTotal = MensajeErrorRP;

				if (MensajeErrorRP.length < 90) {
					vm.VerErrormsnRP = 2;
					vm.MensajeErrorRP = MensajeErrorRP;
				}
				else {
					vm.VerErrormsnRP = 0;
					vm.MensajeErrorRP = MensajeErrorRP.substring(0, 90);
				}

				vm.TieneError = true;

			}
			else {
				$("#imgavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', true);
				$("#imgavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeOut();
				$("#icoavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).attr('disabled', true);
				$("#icoavrpval1" + fuenteid + "-" + indexFuentes + "-" + indexrecursos).fadeOut();
				vm.VerErrormsnRP = 3;
			}

			if (MensajeErrorRP == "" && MensajeErrorRV == "") {
				$("#GuardarPreguntasgrilla").fadeIn();
				vm.TieneError = false;
			}


			vm.AvanceFinanciero.SumaRecursosVigentesApropiacionInicial = SumaRecursosVigentesApropiacionInicial;
			vm.AvanceFinanciero.SumaRecursosVigentesApropiacionVigente = SumaRecursosVigentesApropiacionVigente;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoCompromisos = SumaRecursosVigentesAcumuladoCompromisos;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoObligaciones = SumaRecursosVigentesAcumuladoObligaciones;
			vm.AvanceFinanciero.SumaRecursosVigentesAcumuladoPagos = SumaRecursosVigentesAcumuladoPagos;

			vm.AvanceFinanciero.SumaReservaPresupuestalApropiacionInicial = SumaReservaPresupuestalApropiacionInicial;
			vm.AvanceFinanciero.SumaReservaPresupuestalApropiacionVigente = SumaReservaPresupuestalApropiacionVigente;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoCompromisos = SumaReservaPresupuestalAcumuladoCompromisos;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoObligaciones = SumaReservaPresupuestalAcumuladoObligaciones;
			vm.AvanceFinanciero.SumaReservaPresupuestalAcumuladoPagos = SumaReservaPresupuestalAcumuladoPagos;

			vm.AvanceFinanciero.SumaAB = SumaRecursosVigentesAcumuladoObligaciones + SumaReservaPresupuestalAcumuladoObligaciones;
		}

		vm.verNombreCompletoRV = function (index) {
			if (index == 1) {
				vm.VerErrormsnRV = 1;
			}
			else {
				vm.VerErrormsnRV = 0;
			}
		}
		vm.verNombreCompletoRP = function (index) {
			if (index == 1) {
				vm.VerErrormsnRP = 1;
			}
			else {
				vm.VerErrormsnRP = 0;
			}
		}

		vm.ActivarEditarGrilla = function () {
			var variable = $("#Editargrilla")[0].innerText;
			if (variable == "EDITAR") {
				vm.AvanceFinancieroorigen = [];
				vm.AvanceFinancieroorigen = JSON.stringify(vm.AvanceFinanciero);
				$("#Editargrilla").html("CANCELAR");
				document.getElementById("GuardarPreguntasgrilla").classList.remove('btnguardarDisabledDNP');
				document.getElementById("GuardarPreguntasgrilla").classList.add('btnguardarDNP');
				vm.AvanceFinanciero.Fuentes.forEach(fuente => {
					fuente.RecursosVigentes.forEach(rv => {
						rv.HabilitaApropiacionInicialValor = rv.HabilitaApropiacionInicial
						rv.HabilitaApropiacionVigenteValor = rv.HabilitaApropiacionVigente;
						rv.HabilitaAcumuladoCompromisosValor = rv.HabilitaAcumuladoCompromisos;
						rv.HabilitaAcumuladoObligacionValor = rv.HabilitaAcumuladoObligacion;
						rv.HabilitaAcumuladoPagosValor = rv.HabilitaAcumuladoPagos;
					});
					fuente.RecursosPresupuestales.forEach(rp => {
						rp.HabilitaApropiacionInicialValor = rp.HabilitaApropiacionInicial;
						rp.HabilitaApropiacionVigenteValor = rp.HabilitaApropiacionVigente;
						rp.HabilitaAcumuladoCompromisosValor = rp.HabilitaAcumuladoCompromisos;
						rp.HabilitaAcumuladoObligacionValor = rp.HabilitaAcumuladoObligacion;
						rp.HabilitaAcumuladoPagosValor = rp.HabilitaAcumuladoPagos;
					});
				});
			}
			else {
				vm.AvanceFinanciero = [];
				vm.AvanceFinanciero = JSON.parse(vm.AvanceFinancieroorigen);

				$("#Editargrilla").html("EDITAR");
				document.getElementById("GuardarPreguntasgrilla").classList.add('btnguardarDisabledDNP');
				document.getElementById("GuardarPreguntasgrilla").classList.remove('btnguardarDNP');
				//vm.AvanceFinanciero = vm.AvanceFinancieroorigen;

				IniciarFormulario(0, 0);
				var indexFuentes = 0;

				vm.AvanceFinanciero.Fuentes.forEach(fuente => {
					var indexrecursosv = 0;
					fuente.RecursosVigentes.forEach(rv => {
						rv.HabilitaApropiacionInicialValor = false;
						rv.HabilitaApropiacionVigenteValor = false;
						rv.HabilitaAcumuladoCompromisosValor = false;
						rv.HabilitaAcumuladoObligacionValor = false;
						rv.HabilitaAcumuladoPagosValor = false;

						$("#inputvalorafac" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-color", "");
						$("#inputvalorafac" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-style", "hidden");
						$("#inputvalorafob" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-color", "");
						$("#inputvalorafob" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-style", "hidden");
						$("#inputvalorafap" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-color", "");
						$("#inputvalorafap" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).css("border-style", "hidden");

						$("#imgavfrvval" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).attr('disabled', true);
						$("#imgavfrvval" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).fadeOut();
						$("#icoavfrvval" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).attr('disabled', true);
						$("#icoavfrvval" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosv).fadeOut();
						vm.VerErrormsnRV = 3;

						indexrecursosv++;

					});
					indexFuentes++;
				});

				indexFuentes = 0;
				vm.AvanceFinanciero.Fuentes.forEach(fuente => {
					var indexrecursosp = 0;
					fuente.RecursosPresupuestales.forEach(rp => {
						rp.HabilitaApropiacionInicialValor = false;
						rp.HabilitaApropiacionVigenteValor = false;
						rp.HabilitaAcumuladoCompromisosValor = false;
						rp.HabilitaAcumuladoObligacionValor = false;
						rp.HabilitaAcumuladoPagosValor = false;


						$("#inputvalorafrpac" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-color", "");
						$("#inputvalorafrpac" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-style", "hidden");
						$("#inputvalorafrpao" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-color", "");
						$("#inputvalorafrpao" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-style", "hidden");
						$("#inputvalorafrpap" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-color", "");
						$("#inputvalorafrpap" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).css("border-style", "hidden");

						$("#imgavrpval1" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).attr('disabled', true);
						$("#imgavrpval1" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).fadeOut();
						$("#icoavrpval1" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).attr('disabled', true);
						$("#icoavrpval1" + fuente.FuenteId + "-" + indexFuentes + "-" + indexrecursosp).fadeOut();
						vm.VerErrormsnRP = 3;

						indexrecursosp++;
					});

					indexFuentes++;
				});

				$("#GuardarPreguntasgrilla").fadeIn();
				vm.TieneError = false;
			}

		}

		vm.registroRespuestaGrilla = function () {

			if (vm.TieneError) {
				return;
			}

			reporteFinancieroServicio.GuardarAvanceFinanciero(vm.AvanceFinanciero).then(function (response) {
				if (response.statusText === "OK" || response.status === 200) {

					setTimeout(function () {
						utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
					}, 500);

					$("#Editargrilla").html("EDITAR");
					document.getElementById("GuardarPreguntasgrilla").classList.add('btnguardarDisabledDNP');
					document.getElementById("GuardarPreguntasgrilla").classList.remove('btnguardarDNP');

					init();
				}
				else {
					utilidades.mensajeError(response.data.Mensaje);
				}
			});
		}

		/* ------------------------ Validaciones ---------------------------------*/

		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Financiero avanceFinanciero", errores);
			if (errores != undefined) {
				vm.erroresActivos = [];
				var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
				vm.erroresActivos = erroresFiltrados.erroresActivos;
				console.log("erroresActivos", vm.erroresActivos)
				vm.ejecutarErrores();
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
			}
		}

		vm.ejecutarErrores = function () {
			vm.limpiarErrores();
			vm.erroresActivos.forEach(p => {
				if (vm.errores[p.Error] != undefined) {
					vm.errores[p.Error]({
						error: p.Error,
						descripcion: p.Descripcion,
						data: p.Data
					});
				}
			});
		}

		vm.getErrorAvanceFinanciero = function ({ error, descripcion, data }) {
			
			var dataObj = JSON.parse(descripcion)
			console.log("getErrorAvanceFinanciero", dataObj);
			var imgError = '<span class="img iconadverdnp mr-2"><img src="Img/u4630.svg"></span>';
			dataObj.forEach((itemError) => {

				var preguntaHT = document.getElementById("preguntaAvanceFinanciero-error-button-" + itemError.PreguntaId)
				console.log(preguntaHT);
				if (preguntaHT != undefined && preguntaHT != null) {
					preguntaHT.classList.remove("d-none");
					preguntaHT.classList.remove("d-none");
					preguntaHT.innerHTML = imgError + itemError.MensajeError;
				}

			})
		}

		vm.errores = {
			'AVANCE001': vm.getErrorAvanceFinanciero
		}

		vm.limpiarErrores = function () {
			var listadoErrores = document.getElementsByClassName("messagealerttableDNP");
			var listadoErroresContainer = document.getElementsByClassName("errores-contenedorFinanciero");
			for (var i = 0; i < listadoErroresContainer.length; i++) {
				if (!listadoErroresContainer[i].classList.contains("d-none")) {
					listadoErroresContainer[i].classList.add("d-none");

				}
			}

			for (var i = 0; i < listadoErrores.length; i++) {
				if (!listadoErrores[i].classList.contains("d-none")) {
					listadoErrores[i].classList.add("d-none")
					listadoErrores[i].innerHTML = ''
				}
			}
		}

		
	}

	angular.module('backbone').component('avanceFinanciero', {
		templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avanceFinanciero/avanceFinanciero.html",
		controller: avanceFinancieroController,
		controllerAs: "vm",
		bindings: {
			callback: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			guardadoevent: '&',
			notificarrefresco: '&',
			notificacioncambios: '&',
			guardadocomponent: '&'
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