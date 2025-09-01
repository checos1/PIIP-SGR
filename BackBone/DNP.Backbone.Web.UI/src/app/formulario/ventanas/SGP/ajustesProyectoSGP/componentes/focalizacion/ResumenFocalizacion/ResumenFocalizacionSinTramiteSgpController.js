(function () {
	'use strict';

	ResumenFocalizacionSinTramiteSgpController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades', 'focalizacionAjustesSinTramiteSgpServicio', 'justificacionCambiosServicio', 'transversalSgpServicio', 'focalizacionAjustesSGPServicio'];

	function ResumenFocalizacionSinTramiteSgpController(
		$scope,
		$sessionStorage,
		$uibModal,
		utilidades,
		focalizacionAjustesSinTramiteSgpServicio,
		justificacionCambiosServicio,
		transversalSgpServicio,
		focalizacionAjustesSGPServicio
	) {
		var listaFuentesBase = [];
		var vm = this;
		vm.init = init;
		vm.listaPoliticasProyecto = [];
		vm.listaPoliticasCategorias = null;
		vm.listaPoliticasCategoriasIndicadores = null;
		vm.BPIN = $sessionStorage.idObjetoNegocio;
		vm.habilitaVerPoliticasIndicadores = 0;
		vm.lblIndicadoresODSPND = "";
		vm.lblIndicadoresPMI = "";
		vm.abrirModalAgregarIndicador = abrirModalAgregarIndicador;
		vm.categoriaSeleccionada = "";
		vm.idInstancia = $sessionStorage.idInstancia;
		vm.ProductoConsultado = null;
		vm.FuenteConsultada = null;
		vm.localizacionConsultada = null;
		vm.CategoriaConsultada = null;
		vm.CategoriaConsultadaFocalizada = 0;
		vm.imgmasAnt;
		vm.imgmenosAnt;
		vm.detailAnt;
		vm.BloquearCargue = false;
		vm.SubcategoriaVerMas = SubcategoriaVerMas;
		vm.soloLectura = false;

		/* ------------ Estructura necesaria para botón validar --------------- */

		vm.nombreComponente = "focalizacionpolsgpcategoriapoliticassintramitesgp";
		vm.notificacionErrores = null;
		vm.erroresActivos = [];
		vm.componentesRefresh = [
			'recursossgpcostosdelasactisintramitesgp',
			'datosgeneralessgpindicadoresdeprsintramitesgp',
			'datosgeneralessgpbeneficiariosTotalessintramitesgp',
			'recursossgpfuentesdefinancsintramitesgp',
			'datosgeneralessgplocalizacionessintramitesgp',
			'focalizacionpolsgppoliticastransvsintramitesgp',
			'focalizacionpolsgpresumendefocalisintramitesgp',
			'datosgeneralessgpbeneficiariosTotalessintramitesgp',
			'datosgeneralessgphorizontesintramitesgp'
		];

		vm.currentYear = new Date().getFullYear();
		vm.validacionGuardado = null;
		vm.recargaGuardado = null;
		vm.recargaGuardadoCostos = null;
		vm.seccionCapitulo = null;

		/*Carga Masiva*/
		vm.HabilitaEditarBandera = false;
		vm.HabilitaEditar = HabilitaEditar;
		vm.adjuntarArchivo = adjuntarArchivo;
		vm.limpiarArchivo = limpiarArchivo;
		vm.validarArchivo = validarArchivo;
		vm.GuardarArchivoFocalizacion = GuardarArchivoFocalizacion;
		vm.exportarFocalizacionExcel = exportarFocalizacionExcel;
		vm.ejecutarConsultaCargueMasivo = ejecutarConsultaCargueMasivo;
		vm.abrirModalAgregarCategoriaPolitica = abrirModalAgregarCategoriaPolitica;
		vm.eliminarCategoriaPolitica = eliminarCategoriaPolitica;
		vm.nombrearchivo = "";
		vm.Focalizacion = null;
		vm.FocalizacionArchivo = [];
		vm.DetalleFocalizacionConsultada = null;
		vm.idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
		vm.ConvertirNumero = ConvertirNumero;
		vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
		vm.ConvertirNumero4decimales = ConvertirNumero4decimales;
		vm.abrirMensajeInformacionRegionalizacion = abrirMensajeInformacionRegionalizacion;
		vm.abrirMensajeMetaCategoria = abrirMensajeMetaCategoria;
		vm.abrirMensajeMetaIndicador = abrirMensajeMetaIndicador;
		vm.abrirMensajeArchivoFocalizacion = abrirMensajeArchivoFocalizacion;
		vm.valoresEnCero = valoresEnCero;
		vm.obtenerIndicador = obtenerIndicador;
		vm.obtenerIndicadorAcumulable = obtenerIndicadorAcumulable;
		vm.obtenerUnidadMedidaIndicadorSecundario = obtenerUnidadMedidaIndicadorSecundario;
		vm.obtenerMetaIndicadorSecundario = obtenerMetaIndicadorSecundario;
		vm.obtenerAcumulativoIndicadorSecundario = obtenerAcumulativoIndicadorSecundario;
		vm.longMaxText = 30;
		vm._validarTotalBeneficiarios = _validarTotalBeneficiarios;
		vm._validarTotalMetaCategoria = _validarTotalMetaCategoria;

		function habilitaCapitulo(activar) {
			var seccionNoHabilitadaFocalizacionCategorias = document.getElementById('seccionNoHabilitadaFocalizacionCategorias');
			var seccionHabilitadaFocalizacionCategorias = document.getElementById('seccionHabilitadaFocalizacionCategorias');

			if (!activar) {
				if (seccionNoHabilitadaFocalizacionCategorias != undefined) {
					seccionNoHabilitadaFocalizacionCategorias.classList.remove('hidden');
					vm.disabled = true;
				}

				if (seccionHabilitadaFocalizacionCategorias != undefined) {
					seccionHabilitadaFocalizacionCategorias.classList.add('hidden');
				}
			} else {
				if (seccionHabilitadaFocalizacionCategorias != undefined) {
					seccionHabilitadaFocalizacionCategorias.classList.remove('hidden');
					vm.disabled = false;
				}

				if (seccionNoHabilitadaFocalizacionCategorias != undefined) {
					seccionNoHabilitadaFocalizacionCategorias.classList.add('hidden');
				}
			}
		}

		function abrirMensajeArchivoFocalizacion() {
			utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Plnatilla Carga Masiva Focalizacion Categorias, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'En ajuste $' y 'Meta categoría', 'Personas categoría', 'Meta indicador secundario'</li><li>La columna 'Meta categoría' acepta valores numéricos sin separador de mil y cuatro decimales con separador coma(,)</li><li>La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li><li>El nombre del archivo no debe contener tíldes ni caracteres especiales</li></ul></span>");
		}

		vm.AbrilNivel1 = function (fuenteId, indexPoliticas, indexCategorias) {
			var variable = $("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
			variable = variable.replace(/ /g, "");
			variable = variable.replace("\r", "");
			var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias);
			var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias);
			if (variable === "+") {
				$("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
				imgmas.style.display = 'none';
				imgmenos.style.display = 'block';
			}
			else {
				$("#ico-" + fuenteId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
				imgmas.style.display = 'block';
				imgmenos.style.display = 'none';
			}
		};

		vm.AbrilNivel2 = function (fuenteId, productoId, indexPoliticas, indexCategorias, categoriaId, politicaId) {
			var variable = $("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
			variable = variable.replace(/ /g, "");
			var imgmas = document.getElementById("imgmas" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
			var imgmenos = document.getElementById("imgmenos" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias);
			if (variable === "+") {
				$("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
				imgmas.style.display = 'none';
				imgmenos.style.display = 'block';

				if (vm.imgmasProdAnt != undefined && vm.imgmenosProdAnt != undefined) {
					if (vm.imgmasProdAnt !== imgmas) {
						$("#ico" + vm.FuenteConsultada + "-" + vm.ProductoConsultado + "-" + vm.indexPoliticasConsultada + "-" + vm.indexCategoriasConsultada).html('+');
						vm.imgmasProdAnt.style.display = 'block';
						vm.imgmenosProdAnt.style.display = 'none';
					}
				}
			}
			else {
				$("#ico" + fuenteId + "-" + productoId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
				imgmas.style.display = 'block';
				imgmenos.style.display = 'none';
			}

			vm.imgmasProdAnt = imgmas;
			vm.imgmenosProdAnt = imgmenos;
			vm.ProductoConsultado = productoId;
			vm.FuenteConsultada = fuenteId;
			vm.CategoriaConsultada = categoriaId;
			vm.PoliticaConsultada = politicaId;
			vm.indexPoliticasConsultada = indexPoliticas;
			vm.indexCategoriasConsultada = indexCategorias;
		};

		vm.AbrilNivel3 = function (productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId) {
			var imgmas = document.getElementById("imgmas-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
			var variableControl = $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
			if (vm.imgmasAnt != undefined && vm.imgmenosAnt != undefined) {
				if (vm.imgmasAnt !== imgmas) {
					if (variableControl === "+") {
						if (vm.listaDetalleLocalizacion.HabilitaEditarLocalizador == true) {
							vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
							$("#Guardar" + vm.localizacionConsultada + vm.indexPoliticasConsultada + vm.indexCategoriasConsultada + vm.indexFuentesConsultada + vm.indexProductoConsultado).attr('disabled', true);
							desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
						}
						else {
							desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
						}
					}
				}
				else {
					desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
				}
			}
			else {
				desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId);
			}
		};

		function desplegarSeccion(productoId, fuenteId, localizacionId, categoriaId, indexPoliticas, indexCategorias, indexFuentes, indexProducto, politicaId) {

			var recargar = 0;
			var variable = $("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias)[0].innerText;
			variable = variable.replace(/ /g, "");
			var imgmas = document.getElementById("imgmas-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
			var imgmenos = document.getElementById("imgmenos-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
			var detail = $("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias);
			if (variable === "+") {
				$("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).html('-');
				imgmas.style.display = 'none';
				imgmenos.style.display = 'block';

				if (detail != undefined) {

					detail.removeClass("hidden");
					if (detail[0] != undefined) {
						detail[0].classList.remove("hidden");
					}
					$("#detail-" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).removeClass("hidden");
				}

				if (vm.imgmasAnt != undefined && vm.imgmenosAnt != undefined) {
					if (vm.imgmasAnt !== imgmas) {
						recargar = 1;
						$("#ico" + vm.ProductoConsultado + "-" + vm.FuenteConsultada + "-" + vm.localizacionConsultada + "-" + vm.indexPoliticasConsultada + "-" + vm.indexCategoriasConsultada).html('+');
						vm.imgmasAnt.style.display = 'block';
						vm.imgmenosAnt.style.display = 'none';
						if (vm.detailAnt != undefined) {
							if (vm.detailAnt[0] != undefined) {
								vm.detailAnt[0].classList.add("hidden");
							}
						}
					}
				}

				vm.imgmasAnt = imgmas;
				vm.imgmenosAnt = imgmenos;
				vm.detailAnt = detail;
				vm.ProductoConsultado = productoId;
				vm.FuenteConsultada = fuenteId;
				vm.localizacionConsultada = localizacionId;
				vm.CategoriaConsultada = categoriaId;
				vm.indexPoliticasConsultada = indexPoliticas;
				vm.indexCategoriasConsultada = indexCategorias;
				vm.indexFuentesConsultada = indexFuentes;
				vm.indexProductoConsultado = indexProducto;
				vm.PoliticaConsultada = politicaId;
				vm.consultarDetalleLocalizacion(productoId, fuenteId, localizacionId, categoriaId, recargar);

			}
			else {
				$("#ico" + productoId + "-" + fuenteId + "-" + localizacionId + "-" + indexPoliticas + "-" + indexCategorias).html('+');
				imgmas.style.display = 'block';
				imgmenos.style.display = 'none';
				if (detail != undefined) {
					if (detail[0] != undefined) {
						detail[0].classList.add("hidden");
					}
				}
			}
		}

		vm.existeFocalizacion = false;

		vm.volver = function () {
			$(window).scrollTop($('#fuentes').position().top + 500);
		}
		vm.VerPolitica = function (idPolitica) {
			if ($("#politica-" + idPolitica) != undefined && $("#politica-" + idPolitica).position() != undefined) $(window).scrollTop($("#politica-" + idPolitica).position().top + 250);
		}
		vm.TieneCategorias = function (itemPoliticas) {
			let tiene = itemPoliticas.Categorias != null && itemPoliticas.Categorias.length > 0;
			return tiene;
		}

		// Manuel
		vm.VerPoliticaIndicadores = function (PoliticaId) {
			if ($("#politicaI-" + PoliticaId) != undefined && $("#politicaI-" + PoliticaId).position() != undefined) $(window).scrollTop($("#politicaI-" + PoliticaId).position().top + 200);
		}

		vm.AbrilNivelIndicadores = function (PoliticaId, Categoria1Id, Categoria2Id) {
			var variable = $("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id)[0].innerText;
			var imgmas = document.getElementById("imgmas-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id);
			var imgmenos = document.getElementById("imgmenos-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id);
			if (variable === "+") {
				$("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id).html('-');
				imgmas.style.display = 'none';
				imgmenos.style.display = 'block';

			} else {
				$("#ico2-" + PoliticaId + "-" + Categoria1Id + "-" + Categoria2Id).html('+');
				imgmas.style.display = 'block';
				imgmenos.style.display = 'none';
			}
		}

		vm.mostrarOcultar = function (indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
			$("#div-producto-mas-" + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto).toggleClass('hidden');
			$("#div-producto-menos-" + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto).toggleClass('hidden');
		};


		vm.verNombresCompleto = function (idElement, idElementObj, idElementProd, idElementIppal, idElementIsec, indexElementObj, indexElementProd, indexElementIppal, indexElementIsec, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
			var elValidacion = document.getElementById(idElement + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-val');
			var elCortoObj = document.getElementById(idElementObj + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
			var elCortoProd = document.getElementById(idElementProd + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
			var elCortoIppal = document.getElementById(idElementIppal + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');
			var elCortoIsec = document.getElementById(idElementIsec + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-min');

			var elCompletoObj = document.getElementById(idElementObj + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
			var elCompletoProd = document.getElementById(idElementProd + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
			var elCompletoIppal = document.getElementById(idElementIppal + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');
			var elCompletoIsec = document.getElementById(idElementIsec + indexPoliticas + "-" + indexCategorias + "-" + indexFuentes + "-" + indexProducto + '-max');

			var lineaEncabezado = document.getElementById(`linea-encabezado-${indexElementProd}`);
			if (elCompletoObj.classList.contains('hidden')) {
				elValidacion.innerHTML = 'VER MENOS';
				elCortoObj.classList.add('hidden');
				elCompletoObj.classList.remove('hidden');

				elCortoProd.classList.add('hidden');
				elCompletoProd.classList.remove('hidden');

				elCortoIppal.classList.add('hidden');
				elCompletoIppal.classList.remove('hidden');

				elCortoIsec.classList.add('hidden');
				elCompletoIsec.classList.remove('hidden');

				if (lineaEncabezado) lineaEncabezado.style.marginTop = "180px";
			}
			else {
				elValidacion.innerHTML = 'VER MÁS';
				elCortoObj.classList.remove('hidden');
				elCompletoObj.classList.add('hidden');

				elCortoProd.classList.remove('hidden');
				elCompletoProd.classList.add('hidden');

				elCortoIppal.classList.remove('hidden');
				elCompletoIppal.classList.add('hidden');

				elCortoIsec.classList.remove('hidden');
				elCompletoIsec.classList.add('hidden');

				if (lineaEncabezado) lineaEncabezado.style.marginTop = "102px";
			}
		};

		function init() {
			vm.model = {
				modulos: {
					administracion: false,
					backbone: true
				}
			}
			vm.FocalizacionArchivo = [];
			vm.obtenerPoliticasTransversales(vm.BPIN);
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
			vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
			vm.ConsultarPoliticasCategoriasIndicadores(vm.BPIN);

			transversalSgpServicio.registrarObservador(function (datos) {
				habilitaCapitulo(datos.enableRegionalizacion);
			});
		}

		vm.obtenerPoliticasTransversales = function (bpin) {

			var idInstancia = $sessionStorage.idNivel;

			return focalizacionAjustesSGPServicio.obtenerPoliticasTransversalesProyectoSGP($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {

						var arreglolistas = jQuery.parseJSON(respuesta.data);
						var arregloGeneral = jQuery.parseJSON(arreglolistas);
						var arregloDatosGenerales = arregloGeneral.Politicas;

						var listaPoliticasProy = [];

						for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
							var habilitarVerDatos = false;
							if (arregloDatosGenerales[pl].PoliticaId == 4 || arregloDatosGenerales[pl].PoliticaId == 7) {
								habilitarVerDatos = true;
							}
							var politicasProyecto = {
								politicaId: arregloDatosGenerales[pl].PoliticaId,
								politica: arregloDatosGenerales[pl].Politica,
								enProyecto: arregloDatosGenerales[pl].EnProyecto,
								enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
								habilitarVerDatos: habilitarVerDatos
							}

							if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
								listaPoliticasProy.push(politicasProyecto);
						}

						vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
						vm.soloLectura = $sessionStorage.soloLectura;
					}
				});
		}

		vm.ConsultarPoliticasCategoriasIndicadores = function (bpin) {

			var idInstancia = $sessionStorage.idInstancia;

			return focalizacionAjustesSGPServicio.ConsultarPoliticasCategoriasIndicadoresSGP($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
				function (respuesta) {
					if (respuesta.data == "") vm.listaPoliticasCategoriasIndicadores = []
					else {
						var arreglo = jQuery.parseJSON(respuesta.data);
						var arregloGeneral = jQuery.parseJSON(arreglo);
						var arregloDatosGenerales = arregloGeneral.Politicas;
						vm.habilitaVerPoliticasIndicadores = 0;
						vm.lblIndicadores = "";
						if (respuesta.data != null && respuesta.data != "") {

							for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
								if (arregloDatosGenerales[pl].PoliticaId == 7) {
									vm.lblIndicadores1 = "Indicadores ODS y PND";
									vm.habilitaVerPoliticasIndicadores = 1;
								}
								if (arregloDatosGenerales[pl].PoliticaId == 4) {
									vm.lblIndicadores2 = "Indicadores PMI";
									vm.habilitaVerPoliticasIndicadores = 1;
								}
							}
							vm.listaPoliticasCategoriasIndicadores = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
						}
					}
				});
		}

		function abrirModalAgregarIndicador(CategoiaId1, CategoriaId2, Subcategoria, Categoria, Politica, FocalizacionId) {
			var CategoriaSelec = CategoriaId2;

			if (CategoriaId2 == null || CategoriaId2 == 0) {
				CategoriaSelec = CategoiaId1;
			}
			$sessionStorage.CategoriaSelec = CategoriaSelec;
			$sessionStorage.nombreCategoriaSelec = Categoria;
			$sessionStorage.nombreSubCategoriaSelec = Subcategoria;
			$sessionStorage.nombrePolitica = Politica;
			$sessionStorage.FocalizacionId = FocalizacionId;

			$uibModal.open({
				templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorSinTramiteSgp.html',
				controller: 'modalAgregarIndicadorSinTramiteSgpController',
				controllerAs: "vm",
				size: 'lg',
				openedClass: "entidad-modal-adherencia"
			}).result.then(function (result) {
				vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
				init();
			}, function (reason) {
				init();
			}), err => {
				toastr.error("Ocurrió un error al consultar el idAplicacion");
			};
		}

		vm.eliminarIndicador = function (IndicadorId, DimensionId, FocalizacionId) {

			var ArregloIndicadores = [{
				IndicadorId: IndicadorId,
				Indicador: null,
				ProyectoId: vm.proyectoId,
				CategoriaId: DimensionId,
				FocalizacionIndicadorId: 0, //se envia en cero porque el post no lo usa
				Accion: "Delete"
			}]


			var ArregloCategorias = [{
				FocalizacionId: FocalizacionId
				, ListaIndicadores: ArregloIndicadores

			}]
			var ArregloPoliticas = [{
				PoliticaId: null
				, Politica: null
				, Categorias: ArregloCategorias
			}]
			var indicadoresCategoriasGuardar = {
				ProyectoId: vm.proyectoId
				, Politicas: ArregloPoliticas

			};

			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
				focalizacionAjustesSGPServicio.ModificarPoliticasCategoriasIndicadoresSGP(indicadoresCategoriasGuardar, usuarioDNP, vm.idInstancia).then(function (response) {
					if ((response.statusText === "OK" || response.status === 200) && response.data) {
						var respuestaExito = JSON.parse(response.data.toString()).Exito;
						var respuestaMensaje = JSON.parse(response.data.toString()).Mensaje;
						if (respuestaExito) {
							parent.postMessage("cerrarModal", window.location.origin);
							utilidades.mensajeSuccess("El indicador fue eliminado con éxito!", false, false, "El indicador fue eliminado con éxito!");
							vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
							init();
						} else {
							swal('', respuestaMensaje, 'error');
							init();
						}
					}
				});
			}, function funcionCancelar(reason) {

			}, null, null, "El indicador asociado sera eliminado");

		}

		vm.obtenerPoliticasTransversalesCategorias = function (bpin) {

			var idInstancia = $sessionStorage.idNivel;

			const proyecto = {
				InstanciaId: $sessionStorage.idInstancia,
				ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
				Bpin: vm.BPIN,
				Texto: 'ConsultaCategorias',
				DetalleLocalizacion: []
			};
			var parametroConsulta = JSON.stringify(proyecto);
			return focalizacionAjustesSGPServicio.obtenerPoliticasTransversalesCategoriasSGP(parametroConsulta, usuarioDNP, idInstancia).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						vm.listaPoliticasCategorias = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
						vm.Focalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
						vm.existeFocalizacion = true;
						ValidarVerDescarDocumentos();
					}
					else {
						vm.existeFocalizacion = false;
					}
				});
		}

		function obtenerIndicador(indicadores, principal) {
			let indicador = "";

			for (let indicadorAux of indicadores) {

				if (principal == 1 && indicadorAux.TipoIndicador == "Principal") {
					indicador = indicadorAux.NombreIndicador;
					break;
				}

				if (principal == 0 && indicadorAux.TipoIndicador != "Principal") {
					indicador = indicadorAux.NombreIndicador;
					break;
				}
			}

			return indicador;
		}

		function obtenerIndicadorAcumulable(indicadores, principal) {
			let acumula = "NO";

			for (let indicadorAux of indicadores) {

				if (principal == 1 && indicadorAux.TipoIndicador == "Principal") {

					if (indicadorAux.Acumulable == 1) {
						acumula = "SI";
					}
					break;
				}

				if (principal == 0 && indicadorAux.TipoIndicador != "Principal") {

					if (indicadorAux.Acumulable == 1) {
						acumula = "SI";
					}
					break;
				}
			}

			return acumula;
		}

		function obtenerUnidadMedidaIndicadorSecundario(indicadores) {
			let unidad = "";
			for (let indicadorAux of indicadores) {
				if (indicadorAux.TipoIndicador != "Principal") {
					unidad = indicadorAux.UnidadMedida;
				}
			}

			return unidad;
		}

		function obtenerMetaIndicadorSecundario(indicadores) {
			let meta = "";
			for (let indicadorAux of indicadores) {
				if (indicadorAux.TipoIndicador != "Principal") {
					meta = indicadorAux.Meta;
					break;
				}
			}

			return meta;
		}

		function obtenerAcumulativoIndicadorSecundario(indicadores) {
			let acumulativo = "";
			for (let indicadorAux of indicadores) {
				if (indicadorAux.TipoIndicador != "Principal") {
					acumulativo = indicadorAux.Acumulable == 0 ? "No" : "Si";
					break;
				}
			};

			return acumulativo;
		}

		//--- Funciones para el manejo de la actualización de los valores de las focalizaciones.

		vm.habilitarEditar = function (producto, fuente, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto, localizacionDetalle) {
			localizacion.HabilitaEditarLocalizador = true;
			vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = true;
			$("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', false);

			angular.forEach(localizacionDetalle.FocalizacionAjustada, function (series) {
				series.EnAjusteOriginal = series.EnAjuste;
				series.EnAjusteFOriginal = series.EnAjusteF;
				series.MetaCategoriaOriginal = series.MetaCategoria;
				series.MetaCategoriaFOriginal = series.MetaCategoriaF;
				series.PersonasCategoriaOriginal = series.PersonasCategoria;
				series.PersonasCategoriaFOriginal = series.PersonasCategoriaF;
				series.MetaIndicadorSecundarioOriginal = series.MetaIndicadorSecundario;
				series.MetaIndicadorSecundarioFOriginal = series.MetaIndicadorSecundarioF;
			});
		}

		vm.cancelarEdicion = function (politica, categoria, fuente, producto, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
			var idInstancia = $sessionStorage.idNivel;
			utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
				localizacion.HabilitaEditarLocalizador = false;
				vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
				$("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).attr('disabled', true);
				$("#ValoresCero" + producto + fuente + localizacion.LocalizacionId).attr('disabled', true);
				asignarValoresOriginales(politica, categoria, fuente, producto, localizacion);
			}, function funcionCancelar(reason) {
			}, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado en la tabla "Focalización ajustada" se perderán.');
		}

		vm.actualizaFila = function (localizacion, fuente, productoId, vigencia, politica, categoriaId, indexPoliticas, indexCategorias, event, cantidadDecimales) {
			calcularTotales(localizacion, fuente, productoId, vigencia, politica, categoriaId, indexPoliticas, indexCategorias, event, cantidadDecimales);
		}

		vm.mostrarBotones = function (origen, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {
			switch (origen) {
				case 1:
					{
						$("#Editar" + localizacion.LocalizacionId + '-' + indexPoliticas + '-' + indexCategorias + '-' + indexFuentes + '-' + indexProducto).attr('disabled', false);
						$("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).fadeIn();
						break;
					}
				case 2:
					{
						$("#Editar" + localizacion.LocalizacionId + '-' + indexPoliticas + '-' + indexCategorias + '-' + indexFuentes + '-' + indexProducto).attr('disabled', true);
						$("#Guardar" + localizacion.LocalizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto).fadeOut();
						break;
					}
			}
		}

		function asignarValoresOriginales(politica, categoria, fuente, producto, localizacion) {

			localizacion = vm.listaDetalleLocalizacion;
			angular.forEach(localizacion.FocalizacionAjustada, function (series) {
				series.EnAjuste = series.EnAjusteOriginal;
				series.EnAjusteF = series.EnAjusteFOriginal;
				series.MetaCategoria = series.MetaCategoriaOriginal;
				series.MetaCategoriaF = series.MetaCategoriaFOriginal;
				series.PersonasCategoria = series.PersonasCategoriaOriginal;
				series.PersonasCategoriaF = series.PersonasCategoriaFOriginal;
				series.MetaIndicadorSecundario = series.MetaIndicadorSecundarioOriginal;
				series.MetaIndicadorSecundarioF = series.MetaIndicadorSecundarioFOriginal;
			});
			localizacion.HabilitaEditarLocalizador = false;
			vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
			calcularTotales(localizacion, null, null, null, null, null, null, null, null, null);
		}

		function calcularTotales(localizacion, fuente, productoId, vigencia, politica, categoriaId, indexPoliticas, indexCategorias, event, cantidadDecimales) {
			var valorTotalEnAjuste = 0;
			var valorTotalMetaCategoria = 0;
			var valorTotalPersonasCategoria = 0;
			var valorTotalMetaIndicadorSecundario = 0;

			localizacion = vm.listaDetalleLocalizacion;
			for (var laj = 0; laj < localizacion.FocalizacionAjustada.length; laj++) {
				valorTotalEnAjuste += localizacion.FocalizacionAjustada[laj].EnAjuste == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].EnAjuste);
				valorTotalMetaCategoria += localizacion.FocalizacionAjustada[laj].MetaCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaCategoria);
				valorTotalPersonasCategoria += localizacion.FocalizacionAjustada[laj].PersonasCategoria == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].PersonasCategoria);
				valorTotalMetaIndicadorSecundario += localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario == null ? 0 : parseFloat(localizacion.FocalizacionAjustada[laj].MetaIndicadorSecundario);
			}
			localizacion.valorTotalEnAjuste = vm.ConvertirNumero2decimales(valorTotalEnAjuste);
			localizacion.valorTotalMetaCategoria = valorTotalMetaCategoria;
			localizacion.valorTotalPersonasCategoria = valorTotalPersonasCategoria;
			localizacion.valorTotalMetaIndicadorSecundario = valorTotalMetaIndicadorSecundario;

			vm.listaDetalleLocalizacion.valorTotalEnAjuste = localizacion.valorTotalEnAjuste;
			vm.listaDetalleLocalizacion.valorTotalMetaCategoria = localizacion.valorTotalMetaCategoria;
			vm.listaDetalleLocalizacion.valorTotalPersonasCategoria = localizacion.valorTotalPersonasCategoria;
			vm.listaDetalleLocalizacion.valorTotalMetaIndicadorSecundario = localizacion.valorTotalMetaIndicadorSecundario;

			var mensajeFinal = "";

			if (fuente != null) {
				var numeroLimpio = limpiaNumero(fuente.TotalFuente);
				if (valorTotalEnAjuste > numeroLimpio) {
					mensajeFinal = `La sumatoria de los recursos focalizados en los productos por vigencia para la política ${politica.Politica} no puede superar el total de la fuente`;
				}

				if (mensajeFinal != "") {
					$("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', false);
					$("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeIn();
					$("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', false);
					$("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeIn();
				} else {
					$("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', true);
					$("#errortottal" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeOut();
					$("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).attr('disabled', true);
					$("#errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias).fadeOut();
				}

				var errormsn = document.getElementById("errortottalmsn" + politica.PoliticaId + "-" + categoriaId + "-" + productoId + "-" + fuente.FuenteId + "-" + localizacion.LocalizacionId + "-" + indexPoliticas + "-" + indexCategorias);
				if (errormsn != undefined) {
					errormsn.innerHTML = '<span>' + mensajeFinal + "</span>";
				}

				if (event) {
					event.target.value = cantidadDecimales == 0 ? event.target.value.replace(".", "") : procesarNumero(event.target.value, cantidadDecimales);

					const val = event.target.value.toString();
					const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
					var total = event.target.value = decimalCnt && decimalCnt > cantidadDecimales ? event.target.value : parseFloat(val).toFixed(cantidadDecimales);
					event.target.value = Intl.NumberFormat('es-co', { minimumFractionDigits: cantidadDecimales, }).format(total);
				}
			}
		}

		function procesarNumero(value, cantidadDecimales, convertFloat = true) {
			if (!Number(value)) {
				value = limpiaNumero(value, cantidadDecimales);

			} else if (!convertFloat) {
				value = value.replace(",", ".");
			} else {
				if (cantidadDecimales != undefined)
					value = parseFloat(value.replace(",", ".")).toFixed(cantidadDecimales);
			}

			return value;
		}

		function calcularTotalesGeneral() {
			var TotalFocalizacoOriginalProducto = 0;
			var TotalFocalizacoProducto = 0;
			var TotalFocalizadoFuente = 0;
			var TotalFocalizadoCategoria = 0;

			vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
				if (politicas.PoliticaId == vm.PoliticaConsultada) {
					if (politicas.Categorias != null) {
						politicas.Categorias.forEach(categorias => {
							if (categorias.CategoriaId == vm.CategoriaConsultada) {
								if (categorias.Fuentes != null) {
									categorias.Fuentes.forEach(fuentes => {
										if (fuentes.FuenteId == vm.FuenteConsultada) {
											if (fuentes.Productos != null) {
												fuentes.Productos.forEach(productos => {
													if (productos.ProductoId == vm.ProductoConsultado) {
														if (vm.listaDetalleLocalizacion.FocalizacionAjustada != null) {
															vm.listaDetalleLocalizacion.FocalizacionAjustada.forEach(focalizacionajustada => {
																TotalFocalizacoProducto = TotalFocalizacoProducto + parseFloat(focalizacionajustada.EnAjuste);
																TotalFocalizacoOriginalProducto = TotalFocalizacoOriginalProducto + parseFloat(focalizacionajustada.EnAjusteOriginal);
															});
														}
														productos.TotalFocalizadoProductoF = vm.ConvertirNumero2decimales(parseFloat(productos.TotalFocalizadoProducto) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto);
														productos.TotalFocalizadoProducto = parseFloat(productos.TotalFocalizadoProducto) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto;
													}
												});
											}
											fuentes.TotalFocalizadoFuenteF = vm.ConvertirNumero2decimales(parseFloat(fuentes.TotalFocalizadoFuente) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto);
											fuentes.TotalFocalizadoFuente = parseFloat(fuentes.TotalFocalizadoFuente) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto;
										}
									});
								}
								categorias.TotalFocalizadoCategoriaF = vm.ConvertirNumero2decimales(parseFloat(categorias.TotalFocalizadoCategoria) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto);
								categorias.TotalFocalizadoCategoria = parseFloat(categorias.TotalFocalizadoCategoria) - TotalFocalizacoOriginalProducto + TotalFocalizacoProducto;
								vm.CategoriaConsultadaFocalizada = 0;
								if (categorias.TotalFocalizadoCategoria !== 0) {
									vm.CategoriaConsultadaFocalizada = 1;
								}
							}
						});
					}
				}
			});
		}


		function actualizarIndicadoresPolitica() {
			var TotalFocalizado = 0;
			if (vm.listaPoliticasCategoriasIndicadores.Politicas != null) {
				vm.listaPoliticasCategoriasIndicadores.Politicas.forEach(politicas => {
					if (politicas.PoliticaId == vm.PoliticaConsultada) {
						if (politicas.Categorias != null) {
							politicas.Categorias.forEach(categorias => {
								if (categorias.DimensionId == vm.CategoriaConsultada) {
									categorias.EstaFocalizada = 0;
									if (vm.CategoriaConsultadaFocalizada === 1) {
										categorias.EstaFocalizada = 1;
									}
								}
							});
						}
					}
				});
			}
		}

		//--- Guardado de valores ajustados para focalización
		vm.GuardarAjustes = function (politicaId, categoriaId, fuente, producto, localizacionId, focalizacionAjustada, localizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {

			if (!_validarTotalMetaCategoria(producto, focalizacionAjustada)) {
				utilidades.mensajeError("El valor del campo \"Meta categorí­a\" debe ser menor o igual a la meta del indicador principal del producto para cada localización y vigencia");
				return;
			}
			//if (!_validarTotalBeneficiarios(producto, focalizacionAjustada)) {
			//	utilidades.mensajeError("El valor del campo \"Personas categorí­a\" debe ser menor o igual a los beneficiarios del producto para cada localización y vigencia");
			//	return;
			//}

			utilidades.mensajeWarning("Los valores de las columnas 'En Ajuste$', 'Meta Categoria', 'Personas Categoria' y 'Meta Indicador Secundario' van a ser guardados, ¿desea continuar?", function funcionContinuar() {

				vm.FocalizacionArchivo = [];

				var fuenteId = fuente.FuenteId;
				var productoId = producto.ProductoId;
				var valorTotalProducto = producto.TotalCostoProducto;
				var valorTotalFuente = fuente.TotalFuente;

				focalizacionAjustada.forEach(vi => {
					var valoresarchivo = {
						ProyectoId: vm.Focalizacion.ProyectoId,
						Bpin: vm.Focalizacion.BPIN,
						PoliticaId: politicaId,
						CategoriaId: categoriaId,
						FuenteId: fuenteId,
						ProductoId: productoId,
						LocalizacionId: localizacionId,
						Vigencia: vi.Vigencia,
						TotalFuene: valorTotalFuente,
						TotalCostoProducto: valorTotalProducto,
						EnAjuste: vi.EnAjuste,
						MetaCategoria: vi.MetaCategoria,
						PersonasCategoria: vi.PersonasCategoria,
						MetaIndicadorSecundario: vi.MetaIndicadorSecundario
					};

					vm.FocalizacionArchivo.push(valoresarchivo);
				});
				let btnInactiva = "#Guardar" + localizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto;
				GuardarArchivoFocalizacion(false, 0, localizacion, btnInactiva);
				localizacion.HabilitaEditarLocalizador = false;
				vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;
			}, function funcionCancelar(reason) {
			});
		};

		function eliminarCategoriaPolitica(politicaId, politica, categoria) {
			var idInstancia = $sessionStorage.idNivel;
			var proyectoId = $sessionStorage.idProyectoEncabezado;
			var categoriaId = categoria.CategoriaId;
			var mensaje = 'Se perderá los datos focalizados en las localizaciones de la categoría: ' + categoria.Categoria;
			if (categoria.SubCategoria !== null) {
				mensaje += ' y subcategoría: ' + categoria.SubCategoria + '.';
			}
			utilidades.mensajeWarning(mensaje + ' ¿Está seguro de continuar?', function funcionContinuar() {

				return focalizacionAjustesSGPServicio.eliminarCategoriaPoliticasProyectoSGP(proyectoId, politicaId, categoriaId, usuarioDNP, idInstancia).then(
					function (respuesta) {
						let res = JSON.parse(respuesta.data);
						if (res.StatusCode == 200) {
							guardarCapituloModificado();
							new utilidades.mensajeSuccess("", false, false, false, "Los datos fueron eliminados con éxito.");
							vm.init();
							vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
						} else {
							new utilidades.mensajeError(res.ReasonPhrase);
						}
					});
			}, function funcionCancelar(reason) {
			}, null, null, 'Los datos serán eliminados.');
		}

		function ValidarVerDescarDocumentos() {
			vm.BloquearCargue = true;
			vm.Focalizacion.Politicas.forEach(politicas => {
				if (politicas.Categorias == null)
					vm.existeFocalizacion = false;
				if (politicas.TieneConceptoPendiente == false) {
					vm.BloquearCargue = false;
				}
			});
		}

		function abrirMensajeInformacionRegionalizacion() {
			utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Las categorías sobre las que ya registró información sobre su ejecución no se encuentran disponibles para ser eliminadas.</span>");
		}

		function abrirMensajeMetaCategoria() {
			utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <br /> <br /> <span class='tituhori' > El valor del campo ''Meta categoría'' debe ser menor o igual a la meta del indicador principal del producto por vigencia.</span>");
		}

		function abrirMensajeMetaIndicador() {
			utilidades.mensajeInformacionN("", null, null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > El valor del campo ''Meta indicador secundario'' debe ser menor o igual a la meta del indicador secundario del producto que tiene la marca de equidad de la mujer, por vigencia.</span>");
		}

		function ConvertirNumero2decimales(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 2,
			}).format(numero);
		}
		function ConvertirNumero4decimales(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 4,
			}).format(numero);
		}
		function ConvertirNumero(numero) {
			return new Intl.NumberFormat('es-co', {
				minimumFractionDigits: 0,
			}).format(numero);
		}

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				vm.init();
			}
		}

		vm.disabled = function (habilitaVerMas) {
			if (habilitaVerMas) {
				return false;
			} else {
				return true;
			}
		}
		vm.notificacionValidacionHijo = function (handler) {
			vm.notificacionErrores = handler;
		}
		vm.validacionGuardadoHijo = function (handler) {
			vm.validacionGuardado = handler;
		}
		function recorrerObjetivosNumber(event) {
			if (Number.isNaN(event.target.value)) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
			}
			if (event.target.value == null) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
			}
			if (event.target.value == "") {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
			}
			event.target.value = parseFloat(event.target.value.replace(",", "."));
		}

		vm.getNombreReducido = function (data, maxCaracteres) {
			if (data.length > maxCaracteres) {
				var dataNueva = data.slice(0, maxCaracteres);
				return dataNueva + '...';
			} else return data
		}
		function abrirModalAgregarCategoriaPolitica(Politica) {
			var categoriasXPolitica = [];
			for (var lpc = 0; lpc < vm.listaPoliticasCategorias.Politicas.length; lpc++) {
				if (vm.listaPoliticasCategorias.Politicas[lpc].Categorias != null && vm.listaPoliticasCategorias.Politicas[lpc].PoliticaId === Politica.PoliticaId) {
					for (var cats = 0; cats < vm.listaPoliticasCategorias.Politicas[lpc].Categorias.length; cats++) {
						categoriasXPolitica.push(vm.listaPoliticasCategorias.Politicas[lpc].Categorias[cats].CategoriaId);
					}
				}
			}
			$sessionStorage.categoriasXPolitica = categoriasXPolitica;
			var data = {
				idpolitica: Politica.PoliticaId,
				nombrePoliticaCat: Politica.Politica
			}
			let modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: "/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/FocalizacionCategoria/modalAgregarCategoriaPoliticaSinTramiteSgp.html",
				controller: 'modalAgregarCategoriaPoliticaSinTramiteSgpController',
				controllerAs: "vm",
				openedClass: "modal-contentDNP",
				resolve: {
					categoria: function () {
						return data;
					},
				},
			});
			modalInstance.result.then(data => {
				if (data != null) {
					guardarCapituloModificado();
					init();
					vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
				}
			});
		}
		/*Carga Masiva*/
		vm.activarControles = function (evento) {
			switch (evento) {
				case "inicio":
					$("#btnFocalizacionValidarArchivo").attr('disabled', true);
					$("#btnFocalizacionLimpiarArchivo").attr('disabled', true);
					$("#btnFocalizacionArchivoSeleccionado").attr('disabled', true);
					document.getElementById('filefocalizacion').value = "";
					vm.nombrearchivo = "";
					break;
				case "cargaarchivo":
					$("#btnFocalizacionValidarArchivo").attr('disabled', false);
					$("#btnFocalizacionLimpiarArchivo").attr('disabled', false);
					$("#btnFocalizacionArchivoSeleccionado").attr('disabled', true);
					break;
				case "validado":
					$("#btnFocalizacionValidarArchivo").attr('disabled', false);
					$("#btnFocalizacionLimpiarArchivo").attr('disabled', false);
					$("#btnFocalizacionArchivoSeleccionado").attr('disabled', false);
					break;
				default:
			}
		}

		function HabilitaEditar(band) {
			vm.HabilitaEditarBandera = band;
		}

		function exportarFocalizacionExcel() {

			utilidades.mensajeWarning("Si ocurren inconvenientes de descarga o visualización, es necesario actualizar la aplicación.", function funcionContinuar() {

				const filename = 'Template_.xlsx';
				const COL_PARAMS = ['hidden', 'wpx', 'width', 'wch', 'MDW'];
				const STYLE_PARAMS = ['fill', 'font', 'alignment', 'border'];
				var styleConf = {
					'E4': {
						fill: { fgColor: { rgb: 'FFFF0000' } }
					}
				}

				var columns = [
					{
						name: 'ProyectoId', title: 'Proyecto Id'
					},
					{
						name: 'Bpin', title: 'Bpin'
					},
					{
						name: 'PoliticaId', title: 'Politica Id'
					},
					{
						name: 'CategoriaId', title: 'Categoria Id'
					},
					{
						name: 'FuenteId', title: 'Fuente Id'
					},
					{
						name: 'ProductoId', title: 'Producto Id'
					},
					{
						name: 'LocalizacionId', title: 'Localizacion Id'
					},
					{
						name: 'Politica', title: 'Politica'
					},
					{
						name: 'Categoria', title: 'Categoria'
					},
					{
						name: 'SubCategoria', title: 'SubCategoria'
					},
					{
						name: 'Fuente', title: 'Fuente'
					},
					{
						name: 'Etapa', title: 'Etapa'
					},
					{
						name: 'TipoFinanciador', title: 'Tipo Financiador'
					},
					{
						name: 'Financiador', title: 'Financiador'
					},
					{
						name: 'Recurso', title: 'Recurso'
					},
					{
						name: 'TotalFuente', title: 'Total Fuente'
					},
					{
						name: 'Producto', title: 'Producto'
					},
					{
						name: 'TotalCostoProducto', title: 'Total Costo Producto'
					},
					{
						name: 'Localizacion', title: 'Localizacion'
					},
					{
						name: 'Vigencia', title: 'Vigencia'
					},
					{
						name: 'EnAjuste', title: 'En Ajuste $'
					},
					{
						name: 'MetaCategoria', title: 'Meta Categoria'
					},
					{
						name: 'PersonasCategoria', title: 'Personas Categoria'
					},
					{
						name: 'MetaIndicadorSecundario', title: 'Meta Indicador Secundario'
					},
					{
						name: 'CuantificaPersonasCategoria', title: 'CuantificaPersonasCategoria'
					}
				];

				let colNames = columns.map(function (item) {
					return item.title;
				})

				var wb = XLSX.utils.book_new();

				wb.Props = {
					Title: "Plantilla Ajuste Focalizacion Categorias",
					Subject: "PIIP",
					Author: "PIIP",
					CreatedDate: new Date().getDate()
				};

				wb.SheetNames.push("Focalizacion Categorias");

				const header = colNames;
				const data = [];
				var NombrePolitica = "";
				vm.BloquearCargue = true;
				vm.Focalizacion.Politicas.forEach(politicas => {
					NombrePolitica = politicas.Politica;
					if (politicas.TieneConceptoPendiente == false) {
						vm.BloquearCargue = false;
					}
					politicas.Categorias.forEach(categorias => {
						categorias.Fuentes.forEach(fuentes => {
							fuentes.Productos.forEach(productos => {
								productos.Localizaciones.forEach(localizaciones => {
									localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {

										data.push({
											ProyectoId: vm.Focalizacion.ProyectoId,
											Bpin: vm.Focalizacion.BPIN,
											PoliticaId: politicas.PoliticaId,
											CategoriaId: categorias.CategoriaId,
											FuenteId: fuentes.FuenteId,
											ProductoId: productos.ProductoId,
											LocalizacionId: localizaciones.LocalizacionId,
											Politica: politicas.Politica,
											Categoria: categorias.Categoria,
											SubCategoria: categorias.SubCategoria,
											Fuente: fuentes.TipoFinanciador + " " + fuentes.Financiador + " " + fuentes.Recurso,
											Etapa: fuentes.Etapa,
											TipoFinanciador: fuentes.TipoFinanciador,
											Financiador: fuentes.Financiador,
											Recurso: fuentes.Recurso,
											TotalFuente: fuentes.TotalFuente,
											Producto: productos.NombreProducto,
											TotalCostoProducto: productos.TotalCostoProducto,
											Localizacion: localizaciones.Localizacion,
											Vigencia: focalizacionajustada.Vigencia,
											EnAjuste$: parseFloat(focalizacionajustada.EnAjuste).toFixed(2),
											MetaCategoria: parseFloat(focalizacionajustada.MetaCategoria).toFixed(4),
											PersonasCategoria: parseFloat(focalizacionajustada.PersonasCategoria),
											MetaIndicadorSecundario: parseFloat(focalizacionajustada.MetaIndicadorSecundario).toFixed(4),
											CuantificaPersonasCategoria: politicas.CuantificaPersonasCategoria
										});
									});
								});
							});
						});
					});
				});

				NombrePolitica = NombrePolitica.replace('á', 'a')
				NombrePolitica = NombrePolitica.replace('é', 'e')
				NombrePolitica = NombrePolitica.replace('í', 'i')
				NombrePolitica = NombrePolitica.replace('ó', 'o')
				NombrePolitica = NombrePolitica.replace('ú', 'u')

				const worksheet = XLSX.utils.json_to_sheet(data, {
					header: ["ProyectoId",
						"Bpin",
						"PoliticaId",
						"CategoriaId",
						"FuenteId",
						"ProductoId",
						"LocalizacionId",
						"Politica",
						"Categoria",
						"SubCategoria",
						"Fuente",
						"Etapa",
						"TipoFinanciador",
						"Financiador",
						"Recurso",
						"TotalFuente",
						"Producto",
						"TotalCostoProducto",
						"Localizacion",
						"Vigencia",
						"EnAjuste$",
						"MetaCategoria",
						"PersonasCategoria",
						"MetaIndicadorSecundario",
						"CuantificaPersonasCategoria"]
				});

				for (let col of [17, 20]) {
					formatColumn(worksheet, col, '#,##0.00')
				}

				for (let col of [21, 23]) {
					formatColumn(worksheet, col, '#,##0.0000')
				}

				/* hide second column */
				worksheet['!cols'] = [];

				worksheet['!cols'][0] = { hidden: true };
				worksheet['!cols'][1] = { hidden: true };
				worksheet['!cols'][2] = { hidden: true };
				worksheet['!cols'][3] = { hidden: true };
				worksheet['!cols'][4] = { hidden: true };
				worksheet['!cols'][5] = { hidden: true };
				worksheet['!cols'][6] = { hidden: true };
				worksheet['!cols'][24] = { hidden: true };

				wb.Sheets["Focalizacion Categorias"] = worksheet;

				var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
				//saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
				saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaAjusteFocalizacionCategorias' + NombrePolitica + '.xlsx');
			}, function funcionCancelar(reason) {
			}, false, false, "Este archivo es compatible con Office 365");
		}

		function formatColumn(worksheet, col, fmtNumero) {

			const range = XLSX.utils.decode_range(worksheet['!ref'])
			for (let row = range.s.r + 1; row <= range.e.r; ++row) {
				const ref = XLSX.utils.encode_cell({ r: row, c: col })

				if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0") {
					worksheet[ref].z = fmtNumero;
					worksheet[ref].t = 'n';
				}
				if (ref === "S1") {
					worksheet[ref].z = fmtNumero;
					worksheet[ref].t = 'n';
				}
				if (ref === "T1") {
					worksheet[ref].z = fmtNumero;
					worksheet[ref].t = 'n';
				}
			}
		}

		function s2ab(s) {
			var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
			var view = new Uint8Array(buf);  //create uint8array as viewer
			for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
			return buf;
		}

		function adjuntarArchivo() {
			document.getElementById('filefocalizacion').value = "";
			document.getElementById('filefocalizacion').click();
		}

		$scope.filefocalizacionNameChanged = function (input) {
			if (input.files.length == 1) {
				vm.nombrearchivo = input.files[0].name;
				vm.activarControles('cargaarchivo');
			}
			else {
				vm.activarControles('inicio');
			}
		}

		function limpiarArchivo() {
			$scope.filesfocalizacion = [];
			document.getElementById('filefocalizacion').value = "";
			vm.activarControles('inicio');
		}

		function validarArchivo() {
			var resultado = true;
			var enajuste = 0;
			var metaCategoria = 0;
			var personasCategoria = 0;
			var metaIndicadorSecundario = 0;
			vm.FocalizacionArchivo = [];
			if (filefocalizacion.files.length > 0) {

				let file = document.getElementById("filefocalizacion").files[0];
				if ($scope.validaFocalizacionNombreArchivo(file.name)) {
					if (typeof (FileReader) != "undefined") {
						var reader = new FileReader();
						if (reader.readAsBinaryString) {
							reader.onload = function (e) {
								var workbook = XLSX.read(e.target.result, {
									type: 'binary'
								});
								var firstSheet = workbook.SheetNames[0];
								var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
								resultado = excelRows.map(function (item, index) {

									if (item["ProyectoId"] == undefined) {
										utilidades.mensajeError("La columna ProyectoId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["ProyectoId"])) {
										utilidades.mensajeError("El valor ProyectoId " + item["ProyectoId"] + " no es númerico!");
										return false;
									}

									if (item["Bpin"] == undefined) {
										utilidades.mensajeError("La columna Bpin no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["Bpin"])) {
										utilidades.mensajeError("El valor Bpin " + item["Bpin"] + " no es númerico!");
										return false;
									}

									if (item["ProductoId"] == undefined) {
										utilidades.mensajeError("La columna ProductoId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["ProductoId"])) {
										utilidades.mensajeError("El valor ProductoId " + item["ProductoId"] + " no es númerico!");
										return false;
									}

									if (item["FuenteId"] == undefined) {
										utilidades.mensajeError("La columna FuenteId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["FuenteId"])) {
										utilidades.mensajeError("El valor FuenteId " + item["FuenteId"] + " no es númerico!");
										return false;
									}

									if (item["LocalizacionId"] == undefined) {
										utilidades.mensajeError("La columna LocalizacionId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["LocalizacionId"])) {
										utilidades.mensajeError("El valor LocalizacionId " + item["LocalizacionId"] + " no es númerico!");
										return false;
									}

									if (item["Vigencia"] == undefined) {
										utilidades.mensajeError("La columna Vigencia no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["Vigencia"])) {
										utilidades.mensajeError("El valor de la Vigencia " + item["Vigencia"] + " no es númerico!");
										return false;
									}

									if (item["PoliticaId"] == undefined) {
										utilidades.mensajeError("La columna PoliticaId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["PoliticaId"])) {
										utilidades.mensajeError("El valor PoliticaId " + item["PoliticaId"] + " no es númerico!");
										return false;
									}

									if (item["CategoriaId"] == undefined) {
										utilidades.mensajeError("La columna CategoriaId no trae valor!");
										return false;
									}
									else if (!ValidaSiEsNumero(item["CategoriaId"])) {
										utilidades.mensajeError("El valor CategoriaId " + item["CategoriaId"] + " no es númerico!");
										return false;
									}

									if (item["EnAjuste$"] == undefined) {
										utilidades.mensajeError("La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
										return false;
									}
									else if (!ValidarDicimal(item["EnAjuste$"].toString(), 2)) {
										utilidades.mensajeError("Valor no valido 'En ajuste $' " + item["EnAjuste$"] + ". La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)");
										return false;
									}
									else {
										enajuste = item["EnAjuste$"];
									}

									if (item["MetaCategoria"] == undefined) {
										utilidades.mensajeError("La columna 'Meta MetaCategoria' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
										return false;
									}
									else if (!ValidarDicimal(item["MetaCategoria"].toString(), 4)) {
										utilidades.mensajeError("Valor no valido 'Meta MetaCategoria' " + item["MetaCategoria"] + ". La columna 'Meta MetaCategoria' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
										return false;
									}
									else {
										metaCategoria = item["MetaCategoria"];
									}

									if (item["PersonasCategoria"] == undefined) {
										utilidades.mensajeError("La columna 'Personas Categoria' solo acepta valores numericos sin decimales");
										return false;
									}
									else if (!ValidarDicimal(item["PersonasCategoria"].toString(), 0)) {
										utilidades.mensajeError("Valor no valido 'Personas Categoria' " + item["PersonasCategoria"] + ". La columna 'Personas Categoria' solo acepta valores numericos sin decimales");
										return false;
									}
									else {
										personasCategoria = item["PersonasCategoria"];
									}

									if (item["MetaIndicadorSecundario"] == undefined) {
										utilidades.mensajeError("La columna 'Meta Indicador Secundario' solo acepta valores numericos sin decimales");
										return false;
									}
									else if (!ValidarDicimal(item["MetaIndicadorSecundario"].toString(), 4)) {
										utilidades.mensajeError("Valor no valido 'Meta Indicador Secundario' " + item["MetaIndicadorSecundario"] + ". acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
										return false;
									}
									else if (!ValidarMetaIndicadorSecundarioPolitica(item["MetaIndicadorSecundario"], item["PoliticaId"])) {
										utilidades.mensajeError("Valor no valido 'Meta Indicador Secundario' " + item["MetaIndicadorSecundario"] + ". La columna 'Meta Indicador Secundario' solo acepta valores mayores a cero para la política 'Equidad de la mujer'");
										return false;
									}
									else {
										metaIndicadorSecundario = item["MetaIndicadorSecundario"];
									}

									if (!ValidarCuantificaPersonasCategoria(item["CuantificaPersonasCategoria"], item["PersonasCategoria"])) {
										utilidades.mensajeError("El campo 'Personas categoría', valor del campo: " + item["PersonasCategoria"] + ", no aplica para las políticas: " + vm.Focalizacion.PoliticasNoCuentificanPersonas[0].PolicitcasNoCuantifica);
										return false;
									}


									var valoresarchivo = {
										ProyectoId: item["ProyectoId"],
										Bpin: item["Bpin"],
										PoliticaId: item["PoliticaId"],
										CategoriaId: item["CategoriaId"],
										FuenteId: item["FuenteId"],
										ProductoId: item["ProductoId"],
										LocalizacionId: item["LocalizacionId"],
										Vigencia: item["Vigencia"],
										TotalFuene: item["TotalFuente"],
										TotalCostoProducto: item["TotalCostoProducto"],
										EnAjuste: enajuste,
										MetaCategoria: metaCategoria,
										PersonasCategoria: personasCategoria,
										MetaIndicadorSecundario: metaIndicadorSecundario
									};
									vm.FocalizacionArchivo.push(valoresarchivo);

								});

								if (resultado.indexOf(false) == -1) {
									if (!ValidarRegistros()) {
										utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
										vm.activarControles('inicio');
									}
									else {
										vm.activarControles('validado');
										utilidades.mensajeSuccess("El campo 'Meta indicador secundario' solo aplica para la política Equidad de la mujer. Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validacion de Carga Exitosa.");
									}
								}
								else {
									vm.activarControles('inicio');
									vm.FocalizacionArchivo = [];
								}
							};
							reader.readAsBinaryString(file);
						}
					}
				}
			}
		}
		function ValidarRegistros() {

			var aPolicitas = [];
			var aCategorias = [];
			var aFuentes = [];
			var aProductos = [];
			var aLocalizaciones = [];
			var aVigencias = [];

			var existeproyecto = 0;
			var existeBpin = 0;
			var existePolitica = 0;
			var existeCategoria = 0;
			var existeFuente = 0;
			var existeProducto = 0;
			var existeLocaliacion = 0;
			var existeVigencia = 0;
			var existePeriodoProyecto = 0;
			var CantidadRegistros = 0;


			vm.Focalizacion.Politicas.forEach(politicas => {
				aPolicitas.push(politicas.PoliticaId);
				politicas.Categorias.forEach(categorias => {
					aCategorias.push(categorias.CategoriaId);
					categorias.Fuentes.forEach(fuentes => {
						aFuentes.push(fuentes.FuenteId);
						fuentes.Productos.forEach(productos => {
							aProductos.push(productos.ProductoId);

							if (productos.Localizaciones == null) return false;

							productos.Localizaciones.forEach(localizaciones => {
								aLocalizaciones.push(localizaciones.LocalizacionId);
								localizaciones.FocalizacionAjustada.forEach(focalizacionajustada => {
									aVigencias.push(focalizacionajustada.Vigencia);
									CantidadRegistros = CantidadRegistros + 1;
								});

							});
						});
					});
				});
			});
			vm.FocalizacionArchivo.forEach(fa => {

				if (fa.ProyectoId != vm.Focalizacion.ProyectoId) {
					existeproyecto++;
				}
				if (fa.Bpin != vm.Focalizacion.BPIN) {
					existeBpin++;
				}

				if (aPolicitas.indexOf(fa.PoliticaId) == -1) {
					existePolitica = existePolitica + 1;
				}
				if (aCategorias.indexOf(fa.CategoriaId) == -1) {
					existeCategoria = existeCategoria + 1;
				}

				if (aFuentes.indexOf(fa.FuenteId) == -1) {
					existeFuente = existeFuente + 1;
				}
				if (aProductos.indexOf(fa.ProductoId) == -1) {
					existeProducto = existeProducto + 1;
				}
				if (aLocalizaciones.indexOf(fa.LocalizacionId) == -1) {
					existeLocaliacion = existeLocaliacion + 1;
				}
				if (aVigencias.indexOf(fa.Vigencia) == -1) {
					existeVigencia = existeVigencia + 1;
				}
			});
			if (existeproyecto > 0 || existeCategoria > 0 || existeBpin > 0 || existeFuente > 0 || existeProducto > 0
				|| existeLocaliacion > 0 || existeVigencia > 0 || existePeriodoProyecto > 0) {
				return false;
			}
			else {
				if (CantidadRegistros != vm.FocalizacionArchivo.length) {
					return false;
				}
				else {
					return true;
				}
			}
		}
		function ValidarMetaIndicadorSecundarioPolitica(valor, PoliticaId) {
			if (PoliticaId != 7 && valor > 0) {
				return false;
			}
			return true;
		}
		function ValidarCuantificaPersonasCategoria(CuantificaPersonasCategoria, valor) {

			if (!CuantificaPersonasCategoria && valor > 0) {
				return false
			}
			return true;
		}
		function ValidarDicimal(valor, decimals) {

			if (valor.toString().includes('.')) {
				var entero = valor.toString().split('.')[0];
				var decimal = valor.toString().split('.')[1];

				if (isNaN(entero)) {
					return false;
				}

				if (isNaN(decimal)) {
					return false;
				}

				if (decimal.length > decimals) {
					return false;
				}
			}
			else {
				if (isNaN(valor)) {
					return false;
				}
			}
			return true;
		}

		function ValidaSiEsNumero(valor) {
			if (valor === undefined)
				return false;
			else if (!isNaN(limpiaNumero(valor))) {
				return true;
			}
			else {
				return false;
			}
		}

		function limpiaNumero(valor, cantidadDecimales) {
			if (cantidadDecimales < 4 && valor == "0.00" || valor == "0") return 0;
			if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
			return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
		}

		$scope.validaFocalizacionNombreArchivo = function (nombre) {
			var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
			if (!regex.test(nombre.toLowerCase())) {
				utilidades.mensajeError("El archivo no es de tipo Excel!");
				$scope.files = [];
				$scope.nombreArchivo = '';
				return false;
			}
			else {
				return true;
			}
		}
		function GuardarArchivoFocalizacion(valorescero, cargamasiva, localizacion, btnInactiva) {
			if (cargamasiva)
				vm.activarControles('inicio');
			return focalizacionAjustesSGPServicio.guardarFocalizacionCategoriasSGP(vm.FocalizacionArchivo, vm.idUsuario).then(function (response) {

				if (vm.listaDetalleLocalizacion != null) {
					angular.forEach(vm.listaDetalleLocalizacion.FocalizacionAjustada, function (series) {
						series.EnAjusteF = ConvertirNumero2decimales(series.EnAjuste);
						series.MetaCategoriaF = ConvertirNumero4decimales(series.MetaCategoria);
						series.PersonasCategoriaF = ConvertirNumero2decimales(series.PersonasCategoria);
						series.MetaIndicadorSecundarioF = ConvertirNumero4decimales(series.MetaIndicadorSecundario);
					});
				}

				if (response.statusText === "OK" || response.status === 200) {
					if (response.data.Mensaje == null) {
						guardarCapituloModificado();
						if (valorescero == true) {
							var Localizacion = null;
							vm.Focalizacion.Politicas.forEach(politicas => {
								if (politicas.PoliticaId == vm.FocalizacionArchivo[0].PoliticaId) {
									politicas.Categorias.forEach(categorias => {
										if (categorias.CategoriaId == vm.FocalizacionArchivo[0].CategoriaId) {
											categorias.Fuentes.forEach(fuentes => {
												if (fuentes.FuenteId == vm.FocalizacionArchivo[0].FuenteId) {
													fuentes.Productos.forEach(productos => {
														if (productos.ProductoId == vm.FocalizacionArchivo[0].ProductoId) {

															if (vm.listaDetalleLocalizacion != null) {
																vm.listaDetalleLocalizacion.FocalizacionAjustada.forEach(localizaciones => {
																	localizaciones.EnAjuste = 0;
																	localizaciones.MetaCategoria = 0;
																	localizaciones.PersonasCategoria = 0;
																	localizaciones.MetaIndicadorSecundario = 0;
																	localizaciones.EnAjusteF = 0;
																	localizaciones.MetaCategoriaF = 0;
																	localizaciones.PersonasCategoriaF = 0;
																	localizaciones.MetaIndicadorSecundarioF = 0;
																});
															}
														}
													});
												}
											});
										}
									});
								}
							});

							vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
								if (politicas.PoliticaId == vm.FocalizacionArchivo[0].PoliticaId) {
									politicas.Categorias.forEach(categorias => {
										if (categorias.CategoriaId == vm.FocalizacionArchivo[0].CategoriaId) {
											categorias.Fuentes.forEach(fuentes => {
												if (fuentes.FuenteId == vm.FocalizacionArchivo[0].FuenteId) {
													fuentes.Productos.forEach(productos => {
														if (productos.ProductoId == vm.FocalizacionArchivo[0].ProductoId) {

															if (vm.listaDetalleLocalizacion != null) {
																vm.listaDetalleLocalizacion.FocalizacionAjustada.forEach(localizaciones => {
																	localizaciones.EnAjuste = 0;
																	localizaciones.MetaCategoria = 0;
																	localizaciones.PersonasCategoria = 0;
																	localizaciones.MetaIndicadorSecundario = 0;
																	localizaciones.EnAjusteF = 0;
																	localizaciones.MetaCategoriaF = 0;
																	localizaciones.PersonasCategoriaF = 0;
																	localizaciones.MetaIndicadorSecundarioF = 0;
																});
															}
														}
													});
												}
											});
										}
									});
								}
							});

							calcularTotales(Localizacion, null, null, null, null, null, null, null, null, null);
							calcularTotalesGeneral();
							actualizarIndicadoresPolitica();
							if (cargamasiva) {
								vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
							}
							vm.FocalizacionArchivo = [];
						}

						else {
							if (cargamasiva) {
								vm.obtenerPoliticasTransversalesCategorias(vm.BPIN);
							}
							calcularTotalesGeneral();
							actualizarIndicadoresPolitica();
						}
						if (valorescero) {
							$(btnInactiva).attr('disabled', true);
							utilidades.mensajeSuccess('', false, false, false, "Se han asignado valores en cero para la tabla de 'Focalización ajustada'.");
						}
						else {
							$(btnInactiva).attr('disabled', true);
							utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
						}

					}

					else {
						asignarValoresOriginales(vm.FocalizacionArchivo[0].PoliticaId, vm.FocalizacionArchivo[0].CategoriaId, vm.FocalizacionArchivo[0].FuenteId, vm.FocalizacionArchivo[0].ProductoId, vm.FocalizacionArchivo[0].LocalizacionId);

						var mensajeError = JSON.parse(response.data.Mensaje);

						var mensajeReturn = "<span class='anttituhori'><lu>";
						console.log(mensajeError);
						try {
							for (var i = 0; i < mensajeError.ListaErrores.length; i++) {
								mensajeReturn += '<li>' + mensajeError.ListaErrores[i].Error + '</li>';
							}
							mensajeReturn += "</lu></span>";
							utilidades.mensajeWarning(mensajeReturn, null, null, null, null);
						}
						catch {
							mensajeReturn = mensajeError.Mensaje;
						}

					}
				}
			})
				.catch(error => {
					if (error.status == 400) {
						utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
						return;
					}
					utilidades.mensajeError("Error al realizar la operación");
				});

		}

		function valoresEnCero(politicaId, categoriaId, fuenteId, productoId, localizacionId, focalizacionAjustada, localiizacion, indexPoliticas, indexCategorias, indexFuentes, indexProducto) {

			utilidades.mensajeWarning("Los valores de las columnas de la tabla 'Focalización ajustada' quedarán en ceros ¿esta seguro de continuar?", function funcionContinuar() {

				vm.FocalizacionArchivo = [];

				focalizacionAjustada.forEach(vi => {
					vi.MetaEnAjuste = 0;
					vi.EnAjuste = 0;
					vi.MetaCategoria = 0;
					vi.MetaCategoriaF = 0;
					vi.EnAjusteF = 0;

					var valoresarchivo = {
						ProyectoId: vm.Focalizacion.ProyectoId,
						Bpin: vm.Focalizacion.BPIN,
						PoliticaId: politicaId,
						CategoriaId: categoriaId,
						FuenteId: fuenteId,
						ProductoId: productoId,
						LocalizacionId: localizacionId,
						Vigencia: vi.Vigencia,
						TotalFuene: 0,
						TotalCostoProducto: 0,
						EnAjuste: 0,
						MetaCategoria: 0,
						PersonasCategoria: 0,
						MetaIndicadorSecundario: 0
					};

					vm.FocalizacionArchivo.push(valoresarchivo);
				});
				let btnInactiva = "#Guardar" + localizacionId + indexPoliticas + indexCategorias + indexFuentes + indexProducto;
				GuardarArchivoFocalizacion(true, 0, localiizacion, btnInactiva);
				vm.listaDetalleLocalizacion.HabilitaEditarLocalizador = false;

			}, function funcionCancelar(reason) {
			}, null, null, "Algunos datos de la tabla se modificarán");
		}



		//2023-03-23  consulta  creada por temas de optimizacion de tiempos
		vm.consultarDetalleLocalizacion = function (productoId, fuenteId, localizacionId, categoriaId, recargar) {
			recargar = 1;
			if (recargar === 1) {
				var idInstancia = $sessionStorage.idNivel;
				const proyecto = {
					InstanciaId: $sessionStorage.idInstancia,
					ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
					Bpin: vm.BPIN,
					Texto: 'ConsultaDetalleLocalizacion',
					DetalleLocalizacion: [{
						FuenteId: fuenteId,
						ProductoId: productoId,
						LocalizacionId: localizacionId,
						DimensionId: categoriaId
					}]
				};

				vm.listaDetalleLocalizacion = null;
				var parametroConsulta = JSON.stringify(proyecto);
				return focalizacionAjustesSGPServicio.obtenerPoliticasTransversalesCategoriasSGP(parametroConsulta, usuarioDNP, idInstancia).then(
					function (respuesta) {
						if (respuesta.data != null && respuesta.data != "") {
							vm.listaDetalleLocalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
							calcularTotales(null, null, null, null, null, null, null, null, null, null);
						}
					});
			}
		};

		//2023-03-23  consulta  creada por temas de optimizacion de tiempos
		function ejecutarConsultaCargueMasivo(politicaid) {

			var idInstancia = $sessionStorage.idNivel;
			const proyecto = {
				InstanciaId: $sessionStorage.idInstancia,
				ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
				Bpin: vm.BPIN,
				Texto: 'CargueMasivo',
				DetalleLocalizacion: [],
				PoliticaId: politicaid
			};

			var parametroConsulta = JSON.stringify(proyecto);
			return focalizacionAjustesSGPServicio.obtenerPoliticasTransversalesCategoriasSGP(parametroConsulta, usuarioDNP, idInstancia).then(
				function (respuesta) {
					if (respuesta.data != null && respuesta.data != "") {
						vm.Focalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
						exportarFocalizacionExcel();
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

		/* ------------------------ Validaciones ---------------------------------*/

		vm.mostrarMensajeError = function (valorTotal, valorAjustado) {
			var respuesta = false;
			if (valorAjustado > valorTotal)
				respuesta = true;

			return respuesta;
		}

		vm.validateFormat = function (event, cantidad) {
			if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
				event.preventDefault();
			}
		}
		vm.validarTamanio = function (event, cantidad) {

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

			event.target.value = procesarNumero(event.target.value, null, false);

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 15;
			var tamanio = event.target.value.length;
			var decimal = false;
			decimal = event.target.value.toString().includes(".");

			if (decimal) {
				if (cantidad == 4) tamanioPermitido = 20;
				else tamanioPermitido = 18;

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

					if (cantidad == 2) {
						if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
							event.preventDefault();
						}
					}
					else {
						if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
							event.preventDefault();
						}
					}
				}
				else {
					var n2 = "";
					newValue = [n[0], n2].join(".");
					event.target.value = newValue;
				}
			}
			else {
				if (tamanio > tamanioPermitido && event.keyCode != 44 && event.keyCode != 188) {
					event.target.value = event.target.value.slice(0, tamanioPermitido);
					event.preventDefault();
				}
			}
		}

		vm.notificacionValidacionPadre = function (errores) {
			vm.limpiarErrores();
			var isValid = true;
			if (errores != undefined) {
				var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
				if (erroresRelacionconlapl != undefined) {
					var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
					if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

					isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson[vm.nombreComponente].forEach(p => {
							if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Data, p.Descripcion);
						});
					}
				}

			}

			vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
		};

		vm.limpiarErrores = function () {
			if (vm.listaPoliticasProyectos !== null) {
				vm.listaPoliticasProyectos.forEach(politicas => {
					politicas.presentaError = false;
					politicas.presentaErrorVacio = false;
				});
			}

			if (vm.listaPoliticasCategorias !== null) {
				vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
					politicas.presentaError = false;
					politicas.presentaErrorVacioPol = false;
					politicas.mensajeError1Politica = '';
					if (politicas.Categorias !== null) {
						politicas.Categorias.forEach(categorias => {
							categorias.presentaError1Categoria = false;
							categorias.presentaError2Categoria = false;
							if (categorias.Fuentes !== null) {
								categorias.Fuentes.forEach(fuentes => {
									fuentes.presentaError1Fuente = false;
									fuentes.mensajeError1Fuente = '';
									fuentes.presentaError2Fuente = false;
									if (fuentes.Productos !== null) {
										fuentes.Productos.forEach(producto => {
											producto.presentaError1producto = false;
											producto.mensajeError1producto = '';
										});
									}
								});
							}
						});
					}
				});
			}

			var campoObligatoriocategoria1 = document.getElementById(vm.nombreComponente + "CategoriaRecursos1");
			if (campoObligatoriocategoria1 != undefined) {
				campoObligatoriocategoria1.innerHTML = "";
				campoObligatoriocategoria1.classList.add('hidden');
			}
		}

		vm.validarFOC_CAT_001 = function (errores, descripcion) {
			var politicaId = null, categoriaId = null, fuenteId = null;
			if (errores.length > 0) {
				var erroresArr = JSON.parse(errores);

				erroresArr.forEach(e => {
					politicaId = e.PoliticaId;
					categoriaId = e.CategoriaId;
					fuenteId = e.FuenteId;

					vm.listaPoliticasProyectos.forEach(politicas => {
						if (politicas.politicaId == politicaId) {
							politicas.presentaError = true;
						}
					});

					if (vm.listaPoliticasCategorias !== null) {
						vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
							if (politicas.PoliticaId == politicaId) {
								politicas.presentaErrorVacioPol = true;
								politicas.presentaError = true;
								if (politicas.Categorias !== null) {
									politicas.Categorias.forEach(categorias => {
										if (categorias.CategoriaId == categoriaId) {
											if (categorias.Fuentes !== null) {
												categorias.Fuentes.forEach(fuentes => {
													if (fuentes.FuenteId == fuenteId) {
														fuentes.presentaError1Fuente = true;
														fuentes.mensajeError1Fuente = descripcion;
													}
												});
												categorias.presentaError1Categoria = true;
											}
										}
									});
								}
							}
						});
					}
				});
			}
		}

		vm.validarFOC_CAT_002 = function (errores, descripcion) {
			var politicaId = null, categoriaId = null, fuenteId = null, productoId = null;

			if (errores.length > 0) {
				var erroresArr = JSON.parse(errores);

				erroresArr.forEach(e => {
					politicaId = e.PoliticaId;
					categoriaId = e.CategoriaId;
					fuenteId = e.FuenteId;
					productoId = e.ProductoId;

					if (vm.listaPoliticasProyectos !== null) {
						vm.listaPoliticasProyectos.forEach(politicas => {
							if (politicas.politicaId == politicaId) {
								politicas.presentaError = true;
							}
						});
					}

					if (vm.listaPoliticasCategorias !== null) {
						vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
							if (politicas.PoliticaId == politicaId) {
								politicas.presentaErrorVacioPol = true;
								politicas.presentaError = true;
								if (politicas.Categorias !== null) {
									politicas.Categorias.forEach(categorias => {
										if (categorias.CategoriaId == categoriaId) {
											categorias.presentaError2Categoria = true;
											if (categorias.Fuentes !== null) {
												categorias.Fuentes.forEach(fuentes => {
													if (fuentes.FuenteId == fuenteId) {
														if (fuentes.Productos !== null) {
															fuentes.Productos.forEach(producto => {
																if (producto.ProductoId == productoId) {
																	producto.presentaError1producto = true;
																	producto.mensajeError1producto = descripcion;
																}
															});
															fuentes.presentaError2Fuente = true;
														}
													}
												});
												categorias.presentaError2Categoria = true;
											}
										}
									});
								}
							}
						});
					}
				});
			}

		}

		vm.validarFOC_CAT_003 = function (errores, descripcion) {

			var politicaId = null;

			if (errores.length > 0) {
				var erroresArr = JSON.parse(errores);

				erroresArr.forEach(e => {
					politicaId = e.PoliticaId;

					if (vm.listaPoliticasProyectos !== null) {
						vm.listaPoliticasProyectos.forEach(politicas => {
							if (politicas.politicaId == politicaId) {
								politicas.presentaErrorVacio = true;
							}
						});
					}

					if (vm.listaPoliticasCategorias !== null) {
						vm.listaPoliticasCategorias.Politicas.forEach(politicas => {
							if (politicas.PoliticaId == politicaId) {
								politicas.presentaError = true;
								politicas.presentaErrorVacioPol = true;
								politicas.mensajeError1Politica = descripcion;
							}
						});
					}
				});
			}
		}

		vm.validarErrores1 = function (errores, descripcion) {
			var campomensajeerror = document.getElementById(vm.nombreComponente + "CategoriaRecursos1");
			if (campomensajeerror != undefined) {
				campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + descripcion + "</span>";
				campomensajeerror.classList.remove('hidden');
			}
		}

		vm.errores = {
			'FOC_CAT_001': vm.validarFOC_CAT_001,
			'FOC_CAT_002': vm.validarFOC_CAT_002,
			'FOC_CAT_003': vm.validarFOC_CAT_003,
			'FOC_CAT_004': vm.validarErrores1,
		}

		function SubcategoriaVerMas(subcategoria) {
			let modalInstance = $uibModal.open({
				animation: $scope.animationsEnabled,
				templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalSinTramiteSgp.html',
				controller: 'objetivosIndicadorModalSinTramiteSgpController',
				controllerAs: "vm",
				size: 'lg',
				openedClass: "entidad-modal-adherencia",
				resolve: {
					Objetivo: function () {
						return subcategoria;
					},
					IdObjetivo: function () {
						return "";
					},
					Tipo: function () {
						return '';
					},
					Titulo: function () {
						return 'Subcategoría';
					}
				},
			});
		}

		function _validarTotalBeneficiarios(producto, focalizacionAjustada) {
			let validacion = true;
			//let existeValorIgual = false;
			let valorAcumulativo = 0;

			if (producto.EsAcumulativo === "NO") {
				for (let foc of focalizacionAjustada) {
					let personasCategoriaFStr = foc.PersonasCategoria.toString();
					if (foc.PersonasAjusteProductoLocalizacionF < Number(personasCategoriaFStr.replace(/\./g, '').replace(',', '.'))) {
						validacion = false;
						break;
					}
					//if (foc.PersonasAjusteProductoLocalizacionF === Number(personasCategoriaFStr.replace(/\./g, '').replace(',', '.'))) {
					//	existeValorIgual = true;
					//}
				}

				//if (existeValorIgual) {
				//	validacion = false;
				//}
			}
			else {
				focalizacionAjustada.some(foc => {
					const valor = Number(foc.PersonasAjusteProductoLocalizacionF);
					if (!isNaN(valor)) {
						valorAcumulativo += valor;
					}
				});

				if (valorAcumulativo > Number(producto.PoblacionBeneficiaria.replace(/\./g, '').replace(',', '.'))) {
					validacion = false;
				}
			}

			return validacion;
		}

		function _validarTotalMetaCategoria(producto, focalizacionAjustada) {
			let validacion = true;
			let existeValorIgual = false;
			let valorAcumulativo = 0;

			if (producto.EsAcumulativo === "NO") {
				for (let foc of focalizacionAjustada) {
					if (foc.MetaCategoria > Number(producto.Cantidad.replace(/\./g, '').replace(',', '.'))) {
						validacion = false;
						break;
					}
					if (foc.MetaCategoria === Number(producto.Cantidad.replace(/\./g, '').replace(',', '.'))) {
						existeValorIgual = true;
					}
				}

				if (existeValorIgual) {
					validacion = false;
				}
			}
			else {
				focalizacionAjustada.some(foc => {
					const valor = Number(foc.MetaCategoria);
					if (!isNaN(valor)) {
						valorAcumulativo += valor;
					}
				});

				if (valorAcumulativo > Number(producto.Cantidad.replace(/\./g, '').replace(',', '.'))) {
					validacion = false;
				}
			}

			return validacion;
		}
	}

	angular.module('backbone').component('resumenFocalizacionSinTramiteSgp', {
		templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionSinTramiteSgp.html",
		controller: ResumenFocalizacionSinTramiteSgpController,
		controllerAs: "vm",
		bindings: {
			callback: '&',
			notificacionvalidacion: '&',
			notificacionestado: '&',
			guardadoevent: '&',
			notificarrefresco: '&',
			notificacioncambios: '&'
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

						if (isNaN(newValue)) {
							ngModel.$setViewValue(oldValue || '');
							ngModel.$render();
						}
					});
				}
			};
		});
})();