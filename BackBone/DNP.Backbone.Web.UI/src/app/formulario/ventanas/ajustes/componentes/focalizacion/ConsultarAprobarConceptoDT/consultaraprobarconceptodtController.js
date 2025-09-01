(function () {
	'use strict';

	consultaraprobarconceptodtController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
		'constantesBackbone', '$timeout', 'focalizacionAjustesServicio', 'serviciosComponenteNotificaciones', 'trasladosServicio', 'solicitarconceptoServicio', 'archivoServicios'
	];

	function consultaraprobarconceptodtController(
		$scope,
		$sessionStorage,
		$uibModal,
		utilidades,
		constantesBackbone,
		$timeout,
		focalizacionAjustesServicio,
		serviciosComponenteNotificaciones,
		trasladosServicio,
		solicitarconceptoServicio,
		archivoServicios,
	) {


		var vm = this;
		vm.init = init;
		vm.nombreComponente = "focalizacionpolSolicitarConAju";
		vm.BPIN = $sessionStorage.idObjetoNegocio;
		vm.abrirMensajeInfConceptoRespuesta = abrirMensajeInfConceptoRespuesta;

		vm.notificacionErrores = null;
		vm.erroresActivos = [];
		vm.arregloGeneralPTResumen = null;
		vm.verTablaResumen = false;
		vm.TienePoliticas = 0;
		vm.DocumentoJustificacion = "No",
		vm.NombreArchivo = "Justificación modificación política";

		vm.Politicas = [];
		vm.analistas = [];
		vm.PreguntasEnvioPoliticaSubDireccion = [];
		vm.PreguntasEnvioPoliticaSubDireccionOrigen = [];
		vm.cumple = false;
		

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
			vm.ObtenerPreguntasEnvioPoliticaSubDireccion();
			vm.consultarArchivosTramite();

			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

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

		vm.consultarArchivosTramite = function () {
			var valor = 0;
			var param = {
				bpin: $sessionStorage.idObjetoNegocio,
				idinstancia: $sessionStorage.idInstancia
			};

			archivoServicios.obtenerListadoArchivos(param, "proyectos").then(function (response) {
				if (response === undefined || typeof response === 'string') {
					vm.mensajeError = response;
					utilidades.mensajeError(response);
				} else {
					if (response != null) {
						response.forEach(archivo => {
							if (archivo.status != 'Eliminado') {
								if (archivo.nombre == vm.NombreArchivo) {
									vm.DocumentoJustificacion = "Si";
								}
							}
						});
					}
				}
			}, error => {
				console.log(error);
			});

		}

		vm.ObtenerPreguntasEnvioPoliticaSubDireccion = function () {

			var PreguntasPoliticasSubdireccion = {
				IdInstancia: $sessionStorage.idInstancia,
				IdProyecto: $sessionStorage.idProyectoEncabezado,
				IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
				IdNivel: $sessionStorage.idNivel
			};

			return focalizacionAjustesServicio.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasPoliticasSubdireccion).then(

				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						vm.ResumenSolicitudConcepto = [];
						var arreglolistas = jQuery.parseJSON(respuesta.data);
						vm.PreguntasEnvioPoliticaSubDireccion = jQuery.parseJSON(arreglolistas);
						if (vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length > 0) {
							vm.TienePoliticas = vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length;

							vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
								pol.VerPolitica = 0;
								pol.EnviosSubdireccion.forEach(envs => {
									envs.VerPregunta = 0;
									if (envs.IdUsuarioDNP == $sessionStorage.usuario.permisos.IdUsuarioDNP && envs.Activo == true) {
										pol.VerPolitica = 1;
										envs.VerPregunta = 1;
										envs.Preguntas.forEach(pre => {
											//pre.Respuesta = parseFloat(pre.Respuesta);
											pre.OpcionesRespuesta = jQuery.parseJSON(pre.OpcionesRespuesta);
											/*
											pre.OpcionesRespuesta.forEach(opc => {
												opc.OpcionId = parseFloat(opc.OpcionId);
											});
											*/
											pre.Editar = 0;
										});
									}
								});
							});

							vm.PreguntasEnvioPoliticaSubDireccionOrigen = jQuery.parseJSON(arreglolistas);
						}
					}
				});
		}

		function abrirMensajeInfConceptoRespuesta() {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Política para solicitud de concepto.</span>");
		}

		vm.AbrilNivel = function (politicaId, indexpoliticas) {

			var variable = $("#icojusrespuesta-" + politicaId + "-" + indexpoliticas)[0].innerText;
			variable = variable.replace(/ /g, "");
			var imgmas = document.getElementById("imgmasjusrespuesta-" + politicaId + "-" + indexpoliticas);
			var imgmenos = document.getElementById("imgmenosjusrespuesta-" + politicaId + "-" + indexpoliticas);
			if (variable === "+") {
				$("#icojusrespuesta-" + politicaId + "-" + indexpoliticas).html('-');
				imgmas.style.display = 'none';
				imgmenos.style.display = 'block';

			} else {
				$("#icojusrespuesta-" + politicaId + "-" + indexpoliticas).html('+');
				imgmas.style.display = 'block';
				imgmenos.style.display = 'none';
			}

			vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
				if (politicaId == pol.PoliticaId) {
					pol.EnviosSubdireccion.forEach(envs => {
						if (envs.RespuestaEnviada == true) {
							envs.Preguntas.forEach(pre => {
								var radiosirp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0);
								var radionorp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1);
								if (radiosirp != undefined && radionorp != undefined) {
									if (pre.Respuesta == "1") {
										$('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0).prop('checked', true);
									}
									if (pre.Respuesta == "2") {
										$('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1).prop('checked', true);
									}
								}

							});
						}

						if (envs.RespuestaEnviada == false) {
							envs.Preguntas.forEach(pre => {
								var radiosirp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0);
								var radionorp = document.getElementById("radio" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1);
								if (radiosirp != undefined && radionorp != undefined) {
									if (pre.Respuesta == "1") {
										$('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0).prop('checked', true);
									}
									if (pre.Respuesta == "2") {
										$('#radio' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1).prop('checked', true);
									}
								}

							});
						}
					});
				}
			});

		}

		vm.ActivarEditar = function (politicaId, resumen) {

			var variable = $("#Editar" + politicaId)[0].innerText;

			var radiosi = document.getElementById("radio" + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 0);
			var radiono = document.getElementById("radio" + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 1);
			var obs = document.getElementById("obs" + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId);

			if (variable == "EDITAR") {
				$("#Editar" + politicaId).html("CANCELAR");

				document.getElementById("Guardarpsca" + politicaId).classList.remove('btnguardarDisabledDNP');
				document.getElementById("Guardarpsca" + politicaId).classList.add('btnguardarDNP');
				document.getElementById("Enviarpsca" + politicaId).classList.remove('btnguardarDNP');
				document.getElementById("Enviarpsca" + politicaId).classList.add('btnguardarDisabledDNP');
				if (radiosi != undefined) {
					radiosi.removeAttribute('disabled');
				}
				if (radiono != undefined) {
					radiono.removeAttribute('disabled');
				}
				if (obs != undefined) {
					obs.removeAttribute('readonly');
				}
			}
			else {
				utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
					setTimeout(function () {
						utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
					}, 500);
					document.getElementById("Guardarpsca" + politicaId).classList.add('btnguardarDisabledDNP');
					document.getElementById("Guardarpsca" + politicaId).classList.remove('btnguardarDNP');
					var observacionpregunta = "";
					var respuesta = "";

					vm.PreguntasEnvioPoliticaSubDireccionOrigen.Politicas.forEach(pol => {
						if (pol.PoliticaId == politicaId) {
							pol.EnviosSubdireccion.forEach(envs => {
								if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
									envs.Preguntas.forEach(pre => {
										observacionpregunta = pre.ObservacionPregunta;
										respuesta = pre.Respuesta;
									});
								}
							});
						}
					});

					if (radiosi != undefined) {
						$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 0).attr("disabled", true);
						$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 1).prop('checked', false);
					}

					if (radiono != undefined) {
						$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 1).attr("disabled", true);
						$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 1).prop('checked', false);
					}

					if (obs != undefined) {
						obs.value = observacionpregunta;
						$('input[type =obs' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + '], textarea').attr('readonly', 'readonly');
					}

					$("#Editar" + politicaId).html("EDITAR");
					$("#obserrorconceptoajuste" + politicaId).attr('disabled', true);
					$("#obserrorconceptoajuste" + politicaId).fadeOut();
					$("#obserrorconceptoajustemsn" + politicaId).attr('disabled', true);
					$("#obserrorconceptoajustemsn" + politicaId).fadeOut();
					var Errormsn = document.getElementById("obserrorconceptoajustemsn" + politicaId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span></span>';
					}

					$("#radioerrorconceptoajuste" + politicaId).attr('disabled', true);
					$("#radioerrorconceptoajuste" + politicaId).fadeOut();
					$("#radioerrorconceptoajustemsn" + politicaId).attr('disabled', true);
					$("#radioerrorconceptoajustemsn" + politicaId).fadeOut();
					var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + politicaId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span></span>';
					}


				}, function funcionCancelar(reason) {
					//return;

				}, null, null, "¿Los posibles datos que haya diligenciado se perderán.");
			}
		}

		vm.GuardarRespuesta = function (politicaId, resumen) {

			validarCampos(politicaId, resumen);
			if (!vm.cumple) {
				utilidades.mensajeError("Hay datos que presentan inconsistencias, Verifique los campos señalados");
				return;
			}

			var RespuestaPoliticasSubdireccion = {
				IdInstancia: $sessionStorage.idInstancia,
				IdProyecto: $sessionStorage.idProyectoEncabezado,
				IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
				IdNivel: $sessionStorage.idNivel,
				PoliticaId: politicaId,
				Respuesta: resumen.Preguntas[0].Respuesta,
				ObservacionPregunta: resumen.Preguntas[0].ObservacionPregunta,
				EnvioPoliticaSubDireccionIdAgrupa: resumen.EnvioPoliticaSubDireccionIdAgrupa,
				PreguntaId: resumen.Preguntas[0].PreguntaId
			};


			focalizacionAjustesServicio.GuardarPreguntasEnvioPoliticaSubDireccionAjustes(RespuestaPoliticasSubdireccion).then(function (response) {
				if (response.statusText === "OK" || response.status === 200) {

					$("#Editar" + politicaId).html("EDITAR");
					document.getElementById("Enviarpsca" + politicaId).classList.remove('btnguardarDisabledDNP');
					document.getElementById("Enviarpsca" + politicaId).classList.add('btnguardarDNP');
					document.getElementById("Guardarpsca" + politicaId).classList.remove('btnguardarDNP');
					document.getElementById("Guardarpsca" + politicaId).classList.add('btnguardarDisabledDNP');
					$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 0).attr("disabled", true);
					$('#radio' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + 1).attr("disabled", true);
					$('input[type =obs' + politicaId + resumen.EnvioPoliticaSubDireccionIdAgrupa + resumen.Preguntas[0].PreguntaId + '], textarea').attr('readonly', 'readonly');
					setTimeout(function () {
						utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
					}, 500);

					vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
						if (pol.PoliticaId == politicaId) {
							pol.EnviosSubdireccion.forEach(envs => {
								if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
									envs.Preguntas.forEach(pre => {
										observacionpregunta = resumen.Preguntas[0].ObservacionPregunta;
										respuesta = resumen.Preguntas[0].Respuesta;
									});
								}
							});
						}
					});
					vm.PreguntasEnvioPoliticaSubDireccionOrigen.Politicas.forEach(pol => {
						if (pol.PoliticaId == politicaId) {
							pol.EnviosSubdireccion.forEach(envs => {
								if (envs.EnvioPoliticaSubDireccionIdAgrupa == resumen.EnvioPoliticaSubDireccionIdAgrupa) {
									envs.Preguntas.forEach(pre => {
										observacionpregunta = resumen.Preguntas[0].ObservacionPregunta;
										respuesta = resumen.Preguntas[0].Respuesta;
									});
								}
							});
						}
					});

				}
				else {
					utilidades.mensajeError(response.data.Mensaje);
				}
			});


		}

		function validarCampos(politicaId, resumen) {
			vm.cumple = false;
			var error = "";
			if (resumen.Preguntas[0].ObservacionPregunta == "" || resumen.Preguntas[0].ObservacionPregunta == null) {
				$("#obserrorconceptoajuste" + politicaId).attr('disabled', false);
				$("#obserrorconceptoajuste" + politicaId).fadeIn();
				$("#obserrorconceptoajustemsn" + politicaId).attr('disabled', false);
				$("#obserrorconceptoajustemsn" + politicaId).fadeIn();
				var Errormsn = document.getElementById("obserrorconceptoajustemsn" + politicaId);
				if (Errormsn != undefined) {
					Errormsn.innerHTML = '<span>Campo Obligatorio</span>';
				}
				error = "Observacion obligatoria";
			}
			else if (resumen.Preguntas[0].ObservacionPregunta.length > 5000) {
				$("#obserrorconceptoajuste" + politicaId).attr('disabled', false);
				$("#obserrorconceptoajuste" + politicaId).fadeIn();
				$("#obserrorconceptoajustemsn" + politicaId).attr('disabled', false);
				$("#obserrorconceptoajustemsn" + politicaId).fadeIn();
				var Errormsn = document.getElementById("obserrorconceptoajustemsn" + politicaId);
				if (Errormsn != undefined) {
					Errormsn.innerHTML = '<span>Campo supera el limte de 5.000 caracteres</span>';
				}
				error = "Campo supera el limte de 5.000 caracteres";
			}
			else {
				$("#obserrorconceptoajuste" + politicaId).attr('disabled', true);
				$("#obserrorconceptoajuste" + politicaId).fadeOut();
				$("#obserrorconceptoajustemsn" + politicaId).attr('disabled', true);
				$("#obserrorconceptoajustemsn" + politicaId).fadeOut();
				var Errormsn = document.getElementById("obserrorconceptoajustemsn" + politicaId);
				if (Errormsn != undefined) {
					Errormsn.innerHTML = '<span></span>';
				}
			}

			if (resumen.Preguntas[0].Respuesta != "1" && resumen.Preguntas[0].Respuesta != "2") {
				$("#radioerrorconceptoajuste" + politicaId).attr('disabled', false);
				$("#radioerrorconceptoajuste" + politicaId).fadeIn();
				$("#radioerrorconceptoajustemsn" + politicaId).attr('disabled', false);
				$("#radioerrorconceptoajustemsn" + politicaId).fadeIn();
				var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + politicaId);
				if (Errormsn != undefined) {
					Errormsn.innerHTML = '<span>Campo Obligatorio</span>';
				}
				error = "seleccion obligatoria";
			}
			else {
				$("#radioerrorconceptoajuste" + politicaId).attr('disabled', true);
				$("#radioerrorconceptoajuste" + politicaId).fadeOut();
				$("#radioerrorconceptoajustemsn" + politicaId).attr('disabled', true);
				$("#radioerrorconceptoajustemsn" + politicaId).fadeOut();
				var Errormsn = document.getElementById("radioerrorconceptoajustemsn" + politicaId);
				if (Errormsn != undefined) {
					Errormsn.innerHTML = '<span></span>';
				}
			}
			if (error == "") {
				vm.cumple = true;
			}

		}

		vm.Enviar = function (politicaId, resumen) {
			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

				var EnvioRespuestaPoliticaSubDireccion = {
					Id: resumen.EnvioPoliticaSubDireccionIdAgrupa,
					ProyectoId: vm.PreguntasEnvioPoliticaSubDireccion.ProyectoId,
					PoliticaId: politicaId,
					IdUsuarioDNP: resumen.IdUsuarioDNP,

				};

				NotificacionUsuarios(politicaId, resumen);


				focalizacionAjustesServicio.GuardarRespuestaEnvioPoliticaSubDireccionAjustes(EnvioRespuestaPoliticaSubDireccion).then(function (response) {
					if (response.statusText === "OK" || response.status === 200) {

						//solicitarconceptoServicio.eliminarPermisos(resumen.IdUsuarioDNP, $sessionStorage.TramiteId, 'TEC', $sessionStorage.idInstancia);

						$("#Editar" + politicaId).html("EDITAR");
						document.getElementById("Enviarpsca" + politicaId).classList.remove('btnguardarDNP');
						document.getElementById("Enviarpsca" + politicaId).classList.add('btnguardarDisabledDNP');
						document.getElementById("Guardarpsca" + politicaId).classList.remove('btnguardarDNP');
						document.getElementById("Guardarpsca" + politicaId).classList.add('btnguardarDisabledDNP');
						vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						init();
						setTimeout(function () {
							utilidades.mensajeSuccess('', false, false, false, "Su respuesta fue enviada exitosamente al solicitante.");
						}, 500);

					}
					else {
						utilidades.mensajeError(response.data.Mensaje);
					}
				});

			}, function funcionCancelar(reason) {
				//return;

			}, null, null, "Su respuesta de aprobación de modificación será enviada al solicitante.");

		}

		function NotificacionUsuarios(politicaId, resumen) {
			vm.LstNotificacionFlujo = [];

			var PoliticaTransversal = "";
			vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
				if (pol.PoliticaId == politicaId) {
					PoliticaTransversal = pol.NombrePolitica;
				}
			});


			$scope.mydate = new Date();

			var NotificacionFlujo = {
				IdUsuarioDNP: resumen.UsuarioFormulador,
				NombreNotificacion: "Notificar a Dirección Técnica",
				FechaInicio: new Date(),
				FechaFin: new Date(),
				//EsManual:0,
				//Tipo
				ContenidoNotificacion: "se dio respuesta al concepto tecnico de la politica " + PoliticaTransversal + " y se envía a su bandeja para continuar con el proceso.",
				//NombreArchivo   
				IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
			}
			vm.LstNotificacionFlujo.push(NotificacionFlujo);

			serviciosComponenteNotificaciones.NotificarUsuarios(vm.LstNotificacionFlujo, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
				if (response.statusText === "OK" || response.status === 200) {

				}
			});

		}



		/* ------------------------ Validaciones ---------------------------------*/

		vm.notificacionValidacionPadre = function (errores) {
			console.log("Validación  - Focalizacion consultaraprobarconceptodt");
			vm.limpiarErrores();
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
				if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

				var isValid = (erroresJson == null || erroresJson.length == 0);
				if (!isValid) {
					erroresJson[vm.nombreComponente].forEach(p => {
						if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
					});
				}
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

			}
		};

		vm.limpiarErrores = function () {
			/*
			var validacionffr1 = document.getElementById(vm.nombreComponente + "-validacionffr1-error");
			var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionffr1-error-mns");
			if (validacionffr1 != undefined) {
				if (ValidacionFFR1Error != undefined) {
					ValidacionFFR1Error.innerHTML = "";
					validacionffr1.classList.add('hidden');
				}
			}
			*/
		}

		vm.errores = {

		}
	}

	angular.module('backbone').component('consultaraprobarconceptodt', {
		templateUrl: "src/app/formulario/ventanas/ajustes/componentes/focalizacion/ConsultarAprobarConceptoDT/consultaraprobarconceptodt.html",
		controller: consultaraprobarconceptodtController,
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